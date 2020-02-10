using AppBackgroundTasks.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace AppBackgroundTasks
{
	public class Program
	{
		private static readonly string Namespace = typeof(Program).Namespace;
		internal static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.') + 1);

		public static int Main(string[] args)
		{
			//Reads in the configuration options from various sources, and combines them into a single object
			var configuration = GetConfiguration(args);

			//Create the object for the Serilog static Logger object
			Log.Logger = CreateSerilogLogger(configuration);

			try
			{
				Log.Information(@"Configuring background services ({ApplicationContext})...", AppName);
				var host = CreateHostBuilder(args);

				Log.Information(@"Starting background services ({ApplicationContext})...", AppName);
				host.Run();

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

		public static IHost CreateHostBuilder(string[] args) =>
			new HostBuilder().
				ConfigureAppConfiguration(config => SetupConfiguration(config, args))
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureServices((hostContext, svcs) => ConfigureServices(hostContext, svcs))
				.ConfigureLogging(
					(hostContext, logging) =>
					{
						logging.SetMinimumLevel(LogLevel.Debug);
						logging.AddSerilog(); //Add the Serilog LoggerFactory singleton to the services collection for the web host, so asking for an ILogger will use the Serilog factory
					}
				)
				.Build();

		//Reads in the configuration options from various sources, and combines them into a single object
		private static IConfiguration GetConfiguration(string[] args)
		{
			var builder = new ConfigurationBuilder();
			SetupConfiguration(builder, args);
			return builder.Build();
		}

		//Reads in the configuration options from various sources, and combines them into a single object
		private static void SetupConfiguration(IConfigurationBuilder config, string[] args)
		{
			config.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args);
		}

		public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
		{
			var pause = 10;
			int.TryParse(hostContext.Configuration[@"WaitSeconds"], out pause);

			services
				.AddHostedService<SomeTidyupService>()
				.Configure<HostOptions>(option =>
				{
					option.ShutdownTimeout = System.TimeSpan.FromSeconds(pause * 2);
				});
		}

		//Creates a Serilog Logger object, setting up the context and output locations
		private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
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
