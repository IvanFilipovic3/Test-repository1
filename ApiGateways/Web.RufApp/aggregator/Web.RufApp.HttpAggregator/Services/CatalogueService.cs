using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.RufApp.HttpAggregator.DataModels;

namespace Web.RufApp.HttpAggregator.Services
{
	// We don't access the other micro-services directly, but instead use the WebAPIs they expose. This service provides a wrapper.
	public class CatalogueService : ICatalogue
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<CatalogueService> _logger;

		public CatalogueService(HttpClient httpClient, ILogger<CatalogueService> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IEnumerable<CatalogueItem>> GetCatalogItemsAsync()
		{
			// Call the web service to get the list of items. Normally the URL wouldn't be hard-coded, but some configuration would be used
			var stringContent = await _httpClient.GetStringAsync(@"http://catalogue-api/api/catalogue/v1/items");
			return JsonConvert.DeserializeObject<List<CatalogueItem>>(stringContent);
		}

		// A hack function purely to allow us to add some default values to the system
		public async Task<int> PostCatalogueItemAsync(StringContent itemAsString)
		{
			var response = await _httpClient.PostAsync(@"http://catalogue-api/api/catalogue/v1/items", itemAsString);
			response.EnsureSuccessStatusCode();
			var responseBody = await response.Content.ReadAsStringAsync();
			var returnItem = JsonConvert.DeserializeObject<CatalogueItem>(responseBody);
			return returnItem.Id;
		}
	}
}
