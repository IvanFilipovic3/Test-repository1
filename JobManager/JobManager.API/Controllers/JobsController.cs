using System;
using System.Collections.Generic;
using System.Net;
using FIS.Risk.Core.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Prophet.SaaS.JobManager.API.Controllers
{
	[Route("api/job-manager/v1/jobs")]
	[ApiController]
	[Produces(@"application/json")]
	[Consumes(@"application/json")]
	public class JobsController : ControllerBase
	{
		private ILogging Logger { get; }

		public JobsController(ILogging logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
		public ActionResult<IEnumerable<string>> GetAllJobs()
		{
			return Ok(new[] { "value1", "value2" });
		}

		[HttpGet]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		public ActionResult<string> GetJobById(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			return Ok("value");
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public ActionResult<string> CreateJob([FromBody] string value)
		{
			return CreatedAtAction(@"GetJobById", new { id = Guid.NewGuid() }, value);
		}

		[HttpPatch]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public ActionResult UpdateJob(Guid id, [FromBody] string value)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpDelete]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public ActionResult DeleteJob(Guid id)
		{
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			return NoContent();
		}

	}
}
