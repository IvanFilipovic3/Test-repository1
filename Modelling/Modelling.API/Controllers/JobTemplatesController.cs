using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Prophet.SaaS.Modelling.API.DataAccess;
using Prophet.SaaS.Modelling.API.DataModels;

namespace Prophet.SaaS.Modelling.API.Controllers
{
	[Route(@"api/modelling/v1/job-templates")]
	[ApiController]
	[Produces(@"application/json")]
	[Consumes(@"application/json")]
	public class JobTemplatesController : ControllerBase
	{
		private readonly JobTemplatesDbContext _context;
		private ILogging Logger { get; }

		public JobTemplatesController(JobTemplatesDbContext context, ILogging logger)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<JobTemplateData>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<JobTemplateData>>> GetAllJobTemplates()
		{
			return Ok(await _context.JobTemplateItems.ToListAsync());
		}

		//7B0D8303-DA26-49B1-93A5-5AD20BF40298
		[HttpGet]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(JobTemplateData), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<JobTemplateData>> GetJobTemplateById(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			var searchItem = await _context.JobTemplateItems.FindAsync(id);

			if (searchItem == null)
			{
				return NotFound();
			}
			return Ok(searchItem);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult<JobTemplateData>> CreateJobTemplate([FromBody] JobTemplateData value)
		{
			var newCategoryId = await _context.JobTemplateItems.AddAsync(value);
			value.Id = newCategoryId ?? Guid.Empty;

			return CreatedAtAction(@"GetJobTemplateById", new { id = newCategoryId }, value);
		}

		[HttpPatch]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> UpdateJobTemplate(Guid id, [FromBody] JobTemplateData value)
		{
			// Simple PoC logic
			if (id != value.Id)
			{
				return BadRequest();
			}

			await _context.JobTemplateItems.UpdateAsync(value);


			return Ok();
		}

		[HttpDelete]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> DeleteJobTemplate(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			await _context.JobTemplateItems.RemoveAsync(id);

			return NoContent();
		}
	}
}
