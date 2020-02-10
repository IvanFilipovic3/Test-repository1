using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppBackgroundTasks.Services
{
	public class SomeTidyupService : BackgroundService
	{
		private readonly ILogger<SomeTidyupService> _logger;
		private readonly IConfiguration _configuration;

		public SomeTidyupService(ILogger<SomeTidyupService> logger, IConfiguration configuration)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var pause = 10;
			int.TryParse(_configuration[@"WaitSeconds"], out pause);

			_logger.LogDebug($"SomeTidyupService is starting. Using a delay of {pause}.");


			stoppingToken.Register(() => _logger.LogDebug($" SomeTidyupService background task is stopping."));
			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(pause * 1000, stoppingToken);
				_logger.LogInformation($"SomeTidyupService performing background task.");
			}

			_logger.LogDebug($"SomeTidyupService background task is stopping.");
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			
			return base.StopAsync(cancellationToken);
		}
	}
}
