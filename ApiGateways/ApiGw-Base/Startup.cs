using ApiGw_Base.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace ApiGw_Base
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// Add the Ocelot service with the required configuration details
			services.AddOcelotConfiguration(Configuration)
				.AddSwaggerForOcelot(Configuration)
				.AddApplicationServices();

			// Registers required services for health checks 
			services.AddHealthChecksUI();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDiagnostic diag)
		{
			app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseHttpsRedirection();

			// Try and wait until all the other services are up and running, so the browser doesn't launch until the swagger docs are created
			// NOTE: This does not stop the browser from launching and without proper error handling, it'll more likely cause exceptions
			//diag.PerformHealthChecks();

			// Initialize the Swagger document creation, based on the Ocelot configuration file			
			app.UseSwaggerForOcelotUI(Configuration);

			//Start Ocelot and have it sit there for the life of the service, re-routing
			app.UseOcelot().Wait();
		}
	}
	internal static class CustomExtensionMethods
	{
		public static IServiceCollection AddOcelotConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOcelot(configuration);
			return services;
		}
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			// Tell the HttpClientFactory to create and injection a HTTPClient into the DiagnosticService Service
			services.AddHttpClient<IDiagnostic, DiagnosticService>()
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
