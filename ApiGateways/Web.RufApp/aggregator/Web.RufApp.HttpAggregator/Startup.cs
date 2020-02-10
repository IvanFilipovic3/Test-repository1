using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using Web.RufApp.HttpAggregator.Services;

namespace Web.RufApp.HttpAggregator
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
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
				})
				;

			services.AddSwagger()
				.AddApplicationServices();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
				  c.SwaggerEndpoint(@"/swagger/v1/swagger.json", @"Ruf App BFF V1");
			  });
		}
	}
	internal static class CustomExtensionMethods
	{
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
					Title = @"AbstractMicroServices - RufApp Aggregator API",
					Version = @"v1",
					Description = @"The RufApp Aggregator API demonstrates combining multiple service calls",
					TermsOfService = @"Terms Of Service"
				});
			});

			return services;
		}

		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			// Tell the HttpClientFactory to create and injection a HTTPClient into the Catalogue Service
			services.AddHttpClient<ICatalogue, CatalogueService>()
				// Use polly to configure the retry and give up policy for that HTTPClient
				.AddPolicyHandler(GetRetryPolicy())
				.AddPolicyHandler(GetCircuitBreakerPolicy());


			// Tell the HttpClientFactory to create and injection a HTTPClient into the Orders Service
			services.AddHttpClient<IOrders, OrdersService>()
				// Use polly to configure the retry and give up policy for that HTTPClient
				.AddPolicyHandler(GetRetryPolicy())
				.AddPolicyHandler(GetCircuitBreakerPolicy());

			return services;
		}

		static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			// Using Polly allows us to set up an automatic retry policy should the call to the web service fail. 
			// If return code of NotFound is return, we'll assume the web service is just temporarily unavailable 
			// and we'll retry 4 times, using an exponential back-off (taking slightly longer between each retry)
			return HttpPolicyExtensions
			  .HandleTransientHttpError()
			  .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
			  .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
		{
			// If 3 HTTP errors occur, then any more requests that are made within 30 seconds from then will instantly fail, without even
			// trying to contact the web service. This stops us retrying a broken web service forever
			// The circuitbreaker is set to 1 less than the number of retries, so that we ultimately end up with a BrokenCircuitException exception
			// we can handle nicely, rather than an HTTP error
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
		}
	}
}
