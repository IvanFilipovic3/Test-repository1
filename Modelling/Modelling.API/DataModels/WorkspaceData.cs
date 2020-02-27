using Newtonsoft.Json;

namespace Prophet.SaaS.Modelling.API.DataModels
{
	public class WorkspaceData
	{
		public WorkspaceData()
		{
			Name = string.Empty;
		}

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore)]
		public string? Description { get; set; }
	}
}
