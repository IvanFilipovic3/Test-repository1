using EventBus;

namespace Catalogue.API.IntegrationEvents.Events
{
	public class AvailableStockChangedIntegrationEvent : IntegrationEvent
	{
		public int ProductId { get; }
		public int StockChange { get; }

		public AvailableStockChangedIntegrationEvent(int productId, int stockChange)
		{
			ProductId = productId;
			StockChange = stockChange;
		}
	}
}
