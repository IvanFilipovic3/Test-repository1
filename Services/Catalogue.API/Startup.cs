using Autofac;
using Autofac.Extensions.DependencyInjection;
using Catalogue.API.DataAccess;
using Catalogue.API.IntegrationEvents.EventHandling;
using Catalogue.API.IntegrationEvents.Events;
using EventBus;
using EventBus.Interfaces;
using EventBusRabbitMQ;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Linq;

namespace Catalogue.API
{
	internal class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime as part of the Build call made on WebHost Builder. 
		// Use this method to register the services available to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			// Add the health check class to the DI
			services.AddHealthChecks()
				.AddCheck(Program.AppName, () => HealthCheckResult.Healthy($"{Program.AppName} is OK!"));

			// Use the built in IoC to set up the objects that are used in the API
			// I believe this call will add any ControllerBase (or maybe ApiController tagged) classes to the list of services. The methods
			// tagged as Routes will also be added. So now when a route is requested, the system knows what sort of controller it requires
			// and it will create it upon request
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				//Make the output from the swagger json pretty
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
				});

			//Call extension methods defined below, to register the services we want. We use extension methods on the 
			// ServiceCollection, in the same way many 3rd party components do
			services.AddCustomDbContext(Configuration)
				.AddEventBus(Configuration)
				.RegisterEventBus(Configuration)
				.AddSwagger();

			// The default IoC framework does not provide every feature of a full-fledged IoC framework, so replace with AutoFac.
			// This is done by including the Autofac.Extensions.DependencyInjection package
			// The Eventbus uses the ILifetimeScope interface from AutoFac, so we need to set up AutoFac for our service here
			// Having registered all our services in the normal manner, create & populate the Autofac Container,
			// and create the AutofaceServiceProvider
			var container = new ContainerBuilder();
			container.Populate(services);
			return new AutofacServiceProvider(container.Build());
		}

		// This method gets called by the runtime when the Run call is made on the WebHost.
		// Use this method to configure the services request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// Register the endpoint for a basic health check, to indicate the service is alive
			app.UseHealthChecks(@"/hc", new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseMvcWithDefaultRoute();

			//Initiate the swagger middleware to provide the generated JSON and the UI 
			app.UseSwagger()
			  .UseSwaggerUI(c =>
			  {
				  c.SwaggerEndpoint(@"/swagger/v1/swagger.json", @"Catalogue.API V1");
			  });

			ConfigureEventBus(app);
		}

		private void ConfigureEventBus(IApplicationBuilder app)
		{
			// Get the generic interface for the EventBus service, which has been registered below
			var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

			//For the given event, register the given handler. This information will be used in EventBusRabbitMQ to process the incoming messages
			eventBus.Subscribe<AvailableStockChangedIntegrationEvent, AvailableStockChangedIntegrationEventHandler>();
		}
	}

	internal static class CustomExtensionMethods
	{
		public static IServiceCollection AddCustomDbContextInMemory(this IServiceCollection services, IConfiguration configuration)
		{
			//Strip out the DB name
			var conString = configuration[@"ConnectionString"].ToLower().Split(';', StringSplitOptions.RemoveEmptyEntries)
				.ToDictionary(s => s.Substring(0, s.IndexOf('=')), s => s.Substring(s.IndexOf('=') + 1));

			//Register the service, so when the system asks for a OrderDbContext service, it knows it exists
			services.AddDbContext<CatalogueDbContext>(options =>
			{
				//Tell the DBContext to use an in memory database
				options.UseInMemoryDatabase(conString[@"database"] ?? conString[@"initial catalog"]);
			});

			return services;
		}

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			//If we're using SQLData as the server, then we must be running under Compose, otherwise use the inMemoryDB
			var conString = configuration[@"ConnectionString"];
			if (conString.Contains(@"server=sqldata", StringComparison.InvariantCultureIgnoreCase))
			{
				//Register the service, so when the system asks for a OrderDbContext service, it knows it exists
				services.AddDbContext<CatalogueDbContext>(options =>
				{
					//Tell the DBContext to use SQL, using the connection string specified in the configuration settings
					options.UseSqlServer(configuration[@"ConnectionString"],
						sqlServerOptionsAction: sqlOptions =>
						{
							//Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
							sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
						});

					// Changing default behaviour when client evaluation occurs to throw. 
					// Default in EF Core would be to log a warning when client evaluation is performed.
					options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
					//Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
				});
			}
			else
			{
				return services.AddCustomDbContextInMemory(configuration);
			}

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			//AddSwaggerGen is an extension on a service collection as well. Adds the services associated with
			// generating the schema and html documents
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				//Define the options for the HTML document page
				options.SwaggerDoc(@"v1", new Swashbuckle.AspNetCore.Swagger.Info
				{
					Title = @"AbstractMicroServices - Catalogue HTTP API",
					Version = @"v1",
					Description = @"The Catalogue MicroService HTTP API. This is a Data-Driven/CRUD MicroService sample",
					TermsOfService = @"Terms Of Service"
				});
			});

			return services;
		}

		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			// TODO: Replace RabbitMQ event bus with Kafka
			services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
			{
				// Create a logger for logs categorized by DefaultRabbitMQPersistentConnection
				var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

				// Create a connection to the RabbitMQ event bus, the name of which is defined by the variable EventBusConnection in the docker-compose file
				// At this point, this controller has made the choice to use the RabbitMQ event bus
				var factory = new ConnectionFactory()
				{
					HostName = configuration[@"EventBusConnection"],
					DispatchConsumersAsync = true
				};

				// Set up a username and password, although we'll be using the default since we haven't defined any in the docker-compose file
				if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
				{
					factory.UserName = configuration["EventBusUserName"];
				}

				if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
				{
					factory.Password = configuration["EventBusPassword"];
				}

				var retryCount = 5;
				if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
				{
					retryCount = int.Parse(configuration["EventBusRetryCount"]);
				}

				// Register the Rabbit connection with the IoC framework, which will register the IRabbitMQPersistentConnection interface
				return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
			});

			return services;
		}

		public static IServiceCollection RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			var hostName = configuration[@"EventBusConnection"];
			if (hostName == @"localhost")
			{
				services.AddSingleton<IEventBus, NullEventBus>();
				return services;
			}

			var subscriptionClientName = configuration["SubscriptionClientName"];

			services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
			{
				// Having registered the service, we can now obtain said service based on the interface name
				var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				// This will exist because we've added the Autofac.Extensions.DependencyInjection package to the project
				var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
				// Create a logger for messages that will be categorized by EventBusRabbitMQ.EventBusRabbitMQ
				var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
				// This service can be found thanks to the line below, but since this delegate won't actually be called until the service is requested, 
				// it's OK to place the registration for this service further down 
				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

				var retryCount = 5;
				if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
				{
					retryCount = int.Parse(configuration["EventBusRetryCount"]);
				}

				//Finally register the specific RabbitMQ class that will provide the functionality for the generic IEventBus interface
				return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
			});


			// Register the technology independent event bus service to the provide the interface for the subscription manager
			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

			// Register a service to handle the event, that will be created every time is requested
			services.AddTransient<AvailableStockChangedIntegrationEventHandler>();

			return services;
		}
	}
}
