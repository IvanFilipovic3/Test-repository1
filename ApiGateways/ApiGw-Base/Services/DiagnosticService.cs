using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace ApiGw_Base.Services
{
	public class DiagnosticService : IDiagnostic
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DiagnosticService> _logger;

		public DiagnosticService(HttpClient httpClient, ILogger<DiagnosticService> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));			
		}

		// Using the health check functionality to wait until all the web services are up and running. 
		// Probably not how these are meant to be used, but just want to try and stall the startup of this service until the others are running
		// NOTE: This does not stop the browser from launching, but I thought I might as well leave this in here
		public async void PerformHealthChecks()
		{
			_logger.LogInformation(@"Checking health of required services");

			await _httpClient.GetAsync(@"http://catalogue-api/hc");
			await _httpClient.GetAsync(@"http://orders-api/hc");
			await _httpClient.GetAsync(@"http://webrufappagg/hc");

			_logger.LogInformation(@"Health check pages obtained for all required services");
		}
	}
}
