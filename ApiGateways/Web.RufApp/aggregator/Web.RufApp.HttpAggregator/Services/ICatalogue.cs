using System.Collections.Generic;
using System.Threading.Tasks;
using Web.RufApp.HttpAggregator.DataModels;

namespace Web.RufApp.HttpAggregator.Services
{
	public interface ICatalogue
	{
		Task<IEnumerable<CatalogueItem>> GetCatalogItemsAsync();
		Task<int> PostCatalogueItemAsync(System.Net.Http.StringContent itemAsString);
	}
}
