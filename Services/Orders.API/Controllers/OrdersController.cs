using EventBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orders.API.DataAccess;
using Orders.API.DataModels;
using Orders.API.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Orders.API.Controllers
{
	// The combination of the Route, ApiController and base class means that this class gets registered in the services list
	// as part of the dependency injection. Since the routes below are registered, the services knows what controller is required
	// to handle those calls, so it will create the controller when the call is made.
	// BE AWARE, this class will be created every time an API call is made, because it's RESTful

	[Route(@"api/[controller]/v1")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly OrderDbContext _context;
		private readonly ILogger<OrdersController> _logger;
		private readonly IEventBus _eventBus;


		// This can be any prototype you like. The system will look at the registered services (from StartUp.cs) and pass any that
		// match the arguments here. If there is no suitable type registered, then an error will be thrown
		public OrdersController(OrderDbContext context, ILogger<OrdersController> logger, IEventBus eventBus)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		}

		// GET: api/Orders/items
		// Registers the API call
		[HttpGet]
		[Route(@"items")]
		[ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return Ok(await _context.Orders.ToListAsync());
		}


		// GET: api/Orders/items+delayed
		// Registers the API call
		[HttpGet]
		[Route(@"items+delayed/{secs:int}")]
		[ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersDelayed(int secs)
		{
			// In order to provide some a simulation of longer calls, so we can test load balancing on Azure
			System.Threading.Thread.Sleep(secs * 1000);

			return Ok(await _context.Orders.ToListAsync());
		}

		// GET: api/Orders/items/5
		[HttpGet]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Order>> GetOrder(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			var order = await _context.Orders.FindAsync(id);

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
		}

		// POST: api/Orders/items
		[HttpPost]
		[Route(@"items")]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult<Order>> PostCatalogueItem([FromBody]Order newOrder)
		{
			// TODO: The DB update and event publish need to be atomic
			// This would probably be handled through a hybrid EventSource framework

			// Adding an item to the list will make EF inject that item into the database on save
			newOrder.Id = 0;	//Set to 0 so EF auto-generates a key Id
			_context.Orders.Add(newOrder);
			await _context.SaveChangesAsync();

			// Create and post an event, with the relevant details
			var eventMsg = new AvailableStockChangedIntegrationEvent(newOrder.Id, -newOrder.Quantity);
			_logger.LogDebug("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", eventMsg.Id, Program.AppName, eventMsg);

			// Publish the simple event message class on the event bus that was registered in the startup.cs
			_eventBus.Publish(eventMsg);

			// Return the item, which will have an updated ID
			return CreatedAtAction("GetOrder", new { id = newOrder.Id }, newOrder);
		}


		// PUT: api/Orders/items/5
		[HttpPut]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> PutCatalogueItem(int id, [FromBody]Order updateOrder)
		{
			// Simple PoC logic
			if (id != updateOrder.Id)
			{
				return BadRequest();
			}

			// Returns the entry that matches on id (the key) and sets it's state to modified. By 
			// passing in the entire item, we're actually replacing all the values that might already exist.
			_context.Entry(updateOrder).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// DELETE: api/Orders/items/5
		[HttpDelete]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Order>> DeleteCatalogueItem(int id)
		{
			// Find the item in the list
			var deleteOrder = await _context.Orders.FindAsync(id);
			if (deleteOrder == null)
			{
				return NotFound();
			}

			// Remove the object
			_context.Orders.Remove(deleteOrder);

			// And save the context, which will update the database contents on the basis that this item is no longer in the list
			await _context.SaveChangesAsync();

			return Ok(deleteOrder);
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}
	}
}
