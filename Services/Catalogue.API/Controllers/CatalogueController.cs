using Catalogue.API.DataAccess;
using Catalogue.API.DataModels;
using EventBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalogue.API.Controllers
{
	// The combination of the Route, ApiController and base class means that this class gets registered in the services list
	// as part of the dependency injection. Since the routes below are registered, the services knows what controller is required
	// to handle those calls, so it will create the controller when the call is made.
	// BE AWARE, this class will be created every time an API call is made, because it's RESTful
	[Route(@"api/[controller]/v1")]
	[ApiController]
	public class CatalogueController : ControllerBase
	{
		private readonly CatalogueDbContext _context;
		private readonly ILogger<CatalogueController> _logger;
		private readonly IEventBus _eventBus;

		// This can be any prototype you like. The system will look at the registered services (from StartUp.cs) and pass any that
		// match the arguments here. If there is no suitable type registered, then an error will be thrown
		public CatalogueController(CatalogueDbContext context, ILogger<CatalogueController> logger, IEventBus eventBus)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		}


		// GET: api/Catalogue/items+delayed
		// Registers the API call
		[HttpGet]
		[Route(@"items+delayed/{secs:int}")]
		[ProducesResponseType(typeof(IEnumerable<CatalogueItem>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<CatalogueItem>>> GetOrdersDelayed(int secs)
		{
			// In order to provide some a simulation of longer calls, so we can test load balancing on Azure
			System.Threading.Thread.Sleep(secs * 1000);

			return Ok(await _context.CatalogueItems.ToListAsync());
		}

		// GET: api/Catalogue/items
		// Registers the API call
		[HttpGet]
		[Route(@"items")]
		[ProducesResponseType(typeof(IEnumerable<CatalogueItem>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<CatalogueItem>>> GetCatalogueItems()
		{
			return Ok(await _context.CatalogueItems.ToListAsync());
		}

		// GET: api/Catalogue/items/5
		[HttpGet]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(CatalogueItem), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<CatalogueItem>> GetCatalogueItem(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			var catalogueItem = await _context.CatalogueItems.FindAsync(id);

			if (catalogueItem == null)
			{
				return NotFound();
			}
#if DEBUG
			var dupItem = new CatalogueItem
			{
				AvailableStock = catalogueItem.AvailableStock,
				Description = catalogueItem.Description,
				Name = catalogueItem.Name + " New",
				Price = catalogueItem.Price
			};
			return Ok(dupItem);
#else
			return Ok(catalogueItem);
#endif
		}

		// POST: api/Catalogue/items
		[HttpPost]
		[Route(@"items")]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult<CatalogueItem>> PostCatalogueItem([FromBody]CatalogueItem catalogueItem)
		{
			// Adding an item to the list will make EF inject that item into the database on save
			catalogueItem.Id = 0;   //Set to 0 so EF auto-generates a key Id
			_context.CatalogueItems.Add(catalogueItem);
			await _context.SaveChangesAsync();

			// Return the item, which will have an updated ID
			return CreatedAtAction(@"GetCatalogueItem", new { id = catalogueItem.Id }, catalogueItem);
		}

		// PUT: api/Catalogue/items/5
		[HttpPut]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> PutCatalogueItem(int id, [FromBody]CatalogueItem catalogueItem)
		{
			// Simple PoC logic
			if (id != catalogueItem.Id)
			{
				return BadRequest();
			}

			// Returns the entry that matches on id (the key) and sets it's state to modified. By 
			// passing in the entire item, we're actually replacing all the values that might already exist.
			_context.Entry(catalogueItem).State = EntityState.Modified;

			try
			{
				// Save the altered entity
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				// The way EF works means that someone could have removed the object since it was retrieved, so the save would
				// fail. Add a check and return an appropriate error if this was the case
				if (!CatalogueItemExists(id))
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

		// DELETE: api/Catalogue/items/5
		[HttpDelete]
		[Route(@"items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(CatalogueItem), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<CatalogueItem>> DeleteCatalogueItem(int id)
		{
			// Find the item in the list
			var catalogueItem = await _context.CatalogueItems.FindAsync(id);
			if (catalogueItem == null)
			{
				return NotFound();
			}

			// Remove the object
			_context.CatalogueItems.Remove(catalogueItem);

			// And save the context, which will update the database contents on the basis that this item is no longer in the list
			await _context.SaveChangesAsync();

			return Ok(catalogueItem);
		}

		private bool CatalogueItemExists(int id)
		{
			return _context.CatalogueItems.Any(e => e.Id == id);
		}
	}
}
