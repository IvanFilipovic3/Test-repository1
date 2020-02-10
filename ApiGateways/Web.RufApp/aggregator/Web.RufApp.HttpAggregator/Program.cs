using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;


namespace Web.RufApp.HttpAggregator
{
	public class Program
	{
		private static readonly string Namespace = typeof(Program).Namespace;
		internal static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.') + 1);

		public static int Main(string[] args)
		{
			//Reads in the configuration options from various sources, and combines them into a single object
			var configuration = GetConfiguration();

			//Create the object for the Serilog static Logger object
			Log.Logger = CreateSerilogLogger(configuration);

			try
			{
				Log.Information(@"Configuring web host ({ApplicationContext})...", AppName);
				var host = CreateWebHostBuilder(configuration, args);

				Log.Information(@"Starting web host ({ApplicationContext})...", AppName);
				host.Run(); //Calls Configure in startup

				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, @"Program terminated unexpectedly ({ApplicationContext})!", AppName);
				return 1;
			}
			finally
			{
				//Need to close and clean out the log at the end of the program
				Log.CloseAndFlush();
			}
		}

		//The service API is a WebAPI application, so we need to create the self-hosting web service and build it
		public static IWebHost CreateWebHostBuilder(IConfiguration configuration, string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()  //Tell the runtime to use the Startup class for Startup, causing Startup.Configure and Startup.ConfigureServices to be called
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseConfiguration(configuration)
				.UseSerilog()   //Add the Serilog LoggerFactory singleton to the services collection for the web host, so asking for an ILogger will use the Serilog factory
				.Build(); //Call ConfigureServices in startup

		//Reads in the configuration options from various sources, and combines them into a single object
		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables();

			return builder.Build();
		}

		//Creates a Serilog Logger object, setting up the context and output locations
		private static ILogger CreateSerilogLogger(IConfiguration configuration)
		{
			return new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich.WithProperty(@"ApplicationContext", AppName)
				.Enrich.FromLogContext()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}
	}
}
