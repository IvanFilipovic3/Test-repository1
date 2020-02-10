using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Web.RufApp.HttpAggregator.DataModels;
using Web.RufApp.HttpAggregator.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Web.RufApp.HttpAggregator.Controllers
{
	// The combination of the Route, ApiController and base class means that this class gets registered in the services list
	// as part of the dependency injection. Since the routes below are registered, the services knows what controller is required
	// to handle those calls, so it will create the controller when the call is made.
	// BE AWARE, this class will be created every time an API call is made, because it's RESTful
	[Route(@"api/[controller]/v1")]
	[ApiController]

	public class OrderController : ControllerBase
	{
		private readonly ICatalogue _catalogueService;
		private readonly IOrders _ordersService;
		private readonly ILogger<OrderController> _logger;

		public OrderController(ICatalogue catalogueService, IOrders ordersService, ILogger<OrderController> logger)
		{
			_catalogueService = catalogueService ?? throw new ArgumentNullException(nameof(catalogueService));
			_ordersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		// GET: api/Order/items
		// Registers the API call
		[HttpGet]
		[Route(@"itemsDetails+delayed")]
		[ProducesResponseType(typeof(IEnumerable<OrderDetails>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetailsDelayed(int secs)
		{
			// In order to provide some a simulation of longer calls, so we can test load balancing on Azure
			System.Threading.Thread.Sleep(secs * 1000);

			return await GetOrderDetails();
		}

		// GET: api/Order/items
		// Registers the API call
		[HttpGet]
		[Route(@"itemsDetails")]
		[ProducesResponseType(typeof(IEnumerable<OrderDetails>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetails()
		{
			// Get the 2 lists
			var items = await _catalogueService.GetCatalogItemsAsync();
			var orders = await _ordersService.GetOrdersAsync();

			var combinedOrders = orders.Select(o => new OrderDetails { Id = o.Id, CustomerName = o.CustomerName, ProductId = o.ProductId,
				QuantityOrdered = o.Quantity, ProductName = items.FirstOrDefault(i => i.Id == o.ProductId)?.Name ?? string.Empty,
				ProductDescription = items.FirstOrDefault(i => i.Id == o.ProductId)?.Description ?? string.Empty });

			// Combine them
			return Ok(combinedOrders);
		}


		// POST: api/Order/defaultItems
		[HttpPost]
		[Route(@"defaultItems")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<CatalogueItem>> AddDefaultItems()
		{
			// A hacky test function to add a few items to the DB
			var catalogueItems = new[]  {
				new { Id = 0, Name = @"BigTable", Description = @"Fancy dining room table", Price = 29.99M, AvailableStock = 50 },
				new { Id = 0, Name = @"Chair", Description = @"Comfy reclining chair", Price = 9.99M, AvailableStock = 250 },
				new { Id = 0, Name = @"Sofa", Description = @"Leather 3 seater sofa", Price = 49.99M, AvailableStock = 100 }
			};

			var itemIds = new List<int>();

			foreach(var item in catalogueItems)
			{
				var messageContent = new System.Net.Http.StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
				itemIds.Add(await _catalogueService.PostCatalogueItemAsync(messageContent));
			}

			// Now we have a list of product Ids (Don't really care what goes with what), make some orders
			var orderItems = new[]
			{
				new {Id =0, CustomerName = @"John", ProductId = itemIds[0], ProductName = @"Default-0", Quantity = 3},
				new {Id =0, CustomerName = @"Harry", ProductId = itemIds[1], ProductName = @"Default-1", Quantity = 1},
				new {Id =0, CustomerName = @"Sally", ProductId = itemIds[1], ProductName = @"Default-1", Quantity = 5},
				new {Id =0, CustomerName = @"Marlene", ProductId = itemIds[2], ProductName = @"Default-2", Quantity = 2}
			};

			foreach (var item in orderItems)
			{
				var messageContent = new System.Net.Http.StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
				await _ordersService.PostOrderItemAsync(messageContent);
			}

			return Ok();
		}
	}
}