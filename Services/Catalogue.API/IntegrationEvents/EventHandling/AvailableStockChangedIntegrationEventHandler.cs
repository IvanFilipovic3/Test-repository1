using Catalogue.API.DataAccess;
using Catalogue.API.IntegrationEvents.Events;
using EventBus.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Catalogue.API.IntegrationEvents.EventHandling
{
	public class AvailableStockChangedIntegrationEventHandler : IIntegrationEventHandler<AvailableStockChangedIntegrationEvent>
	{
		private readonly CatalogueDbContext _catalogContext;
		private readonly ILogger<AvailableStockChangedIntegrationEventHandler> _logger;

		// IoC framework means that when this class is 'auto' created, any registered services that match the arguments will be used for the constructor
		public AvailableStockChangedIntegrationEventHandler(
			CatalogueDbContext catalogContext,
			ILogger<AvailableStockChangedIntegrationEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Handle(AvailableStockChangedIntegrationEvent eventMsg)
		{
			// TODO: Need to handle deduplication of messages
			using (LogContext.PushProperty("IntegrationEventContext", $"{eventMsg.Id}-{Program.AppName}"))
			{
				_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", eventMsg.Id, Program.AppName, eventMsg);

				var catalogItem = _catalogContext.CatalogueItems.Find(eventMsg.ProductId);
				if (catalogItem != null)
				{
#if DEBUG
					catalogItem.AvailableStock += (eventMsg.StockChange * 2);
#else
					catalogItem.AvailableStock += eventMsg.StockChange;
#endif
					await _catalogContext.SaveChangesAsync();
				}
			}
		}
	}
}

