using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Prophet.SaaS.Modelling.API.DataModels;

namespace Prophet.SaaS.Modelling.API.Controllers
{
	[Route("api/modelling/v1/workspaces")]
	[ApiController]
	[Produces(@"application/json")]
	[Consumes(@"application/json")]
	public class WorkspacesController : ControllerBase
	{
		private ILogging Logger { get; }

		public WorkspacesController(ILogging logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<WorkspaceData>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<WorkspaceData>>> GetAllWorkspaces()
		{
			await Task.FromResult(0);

			var itemList = new List<WorkspaceData>
			{
				new WorkspaceData() { Name = "One", Description = "Description 1"},
				new WorkspaceData() { Name = "Two" }
			};

			return Ok(itemList);
		}

		[HttpGet]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(WorkspaceData), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<WorkspaceData>> GetWorkspaceById(Guid id)
		{
			await Task.FromResult(0);

			if (id == Guid.Empty)
			{
				return NotFound();
			}

			return Ok(new WorkspaceData() { Name = "WorkspaceById", Description = "A workspace by ID" });
		}

		[HttpPatch]
		[Route(@"{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> UpdateWorkspace(Guid id, [FromBody] WorkspaceData value)
		{
			await Task.FromResult(0);

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
		public async Task<ActionResult> DeleteWorkspace(Guid id)
		{
			await Task.FromResult(0);

			if (id == Guid.Empty)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
