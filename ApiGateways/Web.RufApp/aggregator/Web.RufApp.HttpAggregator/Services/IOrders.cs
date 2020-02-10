using System.Collections.Generic;
using System.Threading.Tasks;
using Web.RufApp.HttpAggregator.DataModels;

namespace Web.RufApp.HttpAggregator.Services
{
	public interface IOrders
	{
		Task<IEnumerable<Order>> GetOrdersAsync();
		Task PostOrderItemAsync(System.Net.Http.StringContent itemAsString);
	}
}
