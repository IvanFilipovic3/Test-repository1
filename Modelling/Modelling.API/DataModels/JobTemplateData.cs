using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Prophet.SaaS.Modelling.API.DataModels
{
	public class JobTemplateData
	{
		public JobTemplateData()
		{
			Name = string.Empty;
			Description = string.Empty;
			InstanceType = string.Empty;
			CeVersion = string.Empty;
			PluginVersion = string.Empty;
			Compiler = string.Empty;
			CalculatedVariables = new List<string>();
			ResultDefinitions = new List<string>();
			RunNumbers = string.Empty;
			Simulations = string.Empty;
		}


		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "workspace_id")]
		public Guid WorkspaceId { get; set; }

		[JsonProperty(PropertyName = "run_setting_id")]
		public Guid RunSettingId { get; set; }

		[JsonProperty(PropertyName = "structure_id")]
		public Guid StructureId { get; set; }

		[JsonProperty(PropertyName = "priority")]
		public int Priority { get; set; }

		[JsonProperty(PropertyName = "min_machine_units")]
		public int MinMachineUnits { get; set; }

		[JsonProperty(PropertyName = "max_machine_units")]
		public int MaxMachineUnits { get; set; }

		[JsonProperty(PropertyName = "unit_type")]
		public short UnitType { get; set; }

		[JsonProperty(PropertyName = "min_worker_memory")]
		public int MinWorkerMemory { get; set; }

		[JsonProperty(PropertyName = "instance_type")]
		public string InstanceType { get; set; }

		[JsonProperty(PropertyName = "ce_version")]
		public string CeVersion { get; set; }

		[JsonProperty(PropertyName = "plugin_version")]
		public string PluginVersion { get; set; }

		[JsonProperty(PropertyName = "compiler")]
		public string Compiler { get; set; }

		[JsonProperty(PropertyName = "enable_avx")]
		public bool EnableAvx { get; set; }

		[JsonProperty(PropertyName = "enable_gpu")]
		public bool EnableGpu { get; set; }

		[JsonProperty(PropertyName = "sims_per_task")]
		public int SimsPerTask { get; set; }

		[JsonProperty(PropertyName = "mp_batch_size")]
		public int MpBatchSize { get; set; }

		[JsonProperty(PropertyName = "enable_pmpp")]
		public bool EnablePmpp { get; set; }

		[JsonProperty(PropertyName = "enable_tmpo")]
		public bool EnableTmpo { get; set; }

		[JsonProperty(PropertyName = "split_type")]
		public int SplitType { get; set; }

		[JsonProperty(PropertyName = "split_nested_structs")]
		public bool SplitNestedStructs { get; set; }

		[JsonProperty(PropertyName = "calculated_variables")]
		public List<string> CalculatedVariables { get; }

		[JsonProperty(PropertyName = "result_definitions")]
		public List<string> ResultDefinitions { get; }

		[JsonProperty(PropertyName = "start_date")]
		public DateTime StartDate { get; set; }

		[JsonProperty(PropertyName = "future_accum_period")]
		public int FutureAccumPeriod { get; set; }

		[JsonProperty(PropertyName = "past_accum_period")]
		public int PastAccumPeriod { get; set; }

		[JsonProperty(PropertyName = "company_year_end")]
		public int CompanyYearEnd { get; set; }

		[JsonProperty(PropertyName = "dynamic_proj_period")]
		public int DynamicProjPeriod { get; set; }

		[JsonProperty(PropertyName = "run_numbers")]
		public string RunNumbers { get; set; }

		[JsonProperty(PropertyName = "simulations")]
		public string Simulations { get; set; }

		[JsonProperty(PropertyName = "produce_xps_runlog")]
		public bool ProduceXpsRunlog { get; set; }
	}
}
