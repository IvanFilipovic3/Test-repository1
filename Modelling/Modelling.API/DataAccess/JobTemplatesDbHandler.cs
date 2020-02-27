// This file is auto generated. Do not change by hand

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Prophet.SaaS.Modelling.API.DataModels;
using Prophet.SaaS.PostgreSQL.Access;

namespace Prophet.SaaS.Modelling.API.DataAccess
{
	/// <summary>
	/// Class that reflects the stored procedures in the database, providing methods to perform the stored procedures and obtain the return value.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3265:Non-flags enums should not be used in bitwise operations",
			Justification = "3rd party software relies on this operation")]	
	internal static class JobTemplatesDbHandler
	{
		const string GET_JOB_TEMPLATES_STR =
			"SELECT * "+
			"FROM job_templates " +
			"WHERE id = COALESCE(@p_id, id)";

		const string DELETE_JOB_TEMPLATE_STR =
			"DELETE FROM public.job_templates " +
			"WHERE id = @p_id";

		const string INSERT_JOB_TEMPLATE_STR =
			"INSERT INTO public.job_templates( " +
			"id, name, description, workspace_id, run_setting_id, structure_id, " +
			"priority, min_machine_units, max_machine_units, " +
			"unit_type, min_worker_memory, instance_type, " +
			"ce_version, plugin_version, compiler, " +
			"enable_avx, enable_gpu, sims_per_task, mp_batch_size, " +
			"enable_pmpp, enable_tmpo, split_type, split_nested_structs, " +
			"calculated_variables, result_definitions, start_date, " +
			"future_accum_period, past_accum_period, company_year_end, " +
			"dynamic_proj_period, run_numbers, simulations, produce_xps_runlog) " +
			"VALUES(uuid_generate_v4(), @p_name, @p_description, @p_workspace_id, @p_run_setting_id, @p_structure_id, " +
			"  @p_priority, @p_min_machine_units, @p_max_machine_units, " +
			"  @p_unit_type, @p_min_worker_memory, @p_instance_type, " +
			"  @p_ce_version, @p_plugin_version, @p_compiler, " +
			"  @p_enable_avx, @p_enable_gpu, @p_sims_per_task, @p_mp_batch_size, " +
			"  @p_enable_pmpp, @p_enable_tmpo, @p_split_type, @p_split_nested_structs, " +
			"  @p_calculated_variables, @p_result_definitions, @p_start_date, " +
			"  @p_future_accum_period, @p_past_accum_period, @p_company_year_end, " +
			"  @p_dynamic_proj_period, @p_run_numbers, @p_simulations, @p_produce_xps_runlog) " +
			"RETURNING id;";

		const string UPDATE_JOB_TEMPLATE_STR =
			"UPDATE public.job_templates " +
			"SET name=@p_name, description=@p_description, " +
			"  workspace_id=@p_workspace_id, run_setting_id=@p_run_setting_id, structure_id=@p_structure_id, " +
			"  priority=@p_priority, min_machine_units=@p_min_machine_units, max_machine_units=@p_max_machine_units, " +
			"  unit_type=@p_unit_type, min_worker_memory=@p_min_worker_memory, instance_type=@p_instance_type, " +
			"  ce_version=@p_ce_version, plugin_version=@p_plugin_version, compiler=@p_compiler, " +
			"  enable_avx=@p_enable_avx, enable_gpu=@p_enable_gpu, sims_per_task=@p_sims_per_task, mp_batch_size=@p_mp_batch_size, " +
			"  enable_pmpp=@p_enable_pmpp, enable_tmpo=@p_enable_tmpo, split_type=@p_split_type, split_nested_structs=@p_split_nested_structs, " +
			"  calculated_variables=@p_calculated_variables, result_definitions=@p_result_definitions, start_date=@p_start_date, " +
			"  future_accum_period=@p_future_accum_period, past_accum_period=@p_past_accum_period, company_year_end=@p_company_year_end, " +
			"  dynamic_proj_period=@p_dynamic_proj_period, run_numbers=@p_run_numbers, simulations=@p_simulations, produce_xps_runlog=@p_produce_xps_runlog " +
			"WHERE id=@p_id";

		static readonly KeyValuePair<bool, string> s_get_job_templates = new KeyValuePair<bool, string>(false, GET_JOB_TEMPLATES_STR);
		static readonly KeyValuePair<bool, string> s_delete_job_template = new KeyValuePair<bool, string>(false, DELETE_JOB_TEMPLATE_STR);
		static readonly KeyValuePair<bool, string> s_insert_job_template = new KeyValuePair<bool, string>(false, INSERT_JOB_TEMPLATE_STR);
		static readonly KeyValuePair<bool, string> s_update_job_template = new KeyValuePair<bool, string>(false, UPDATE_JOB_TEMPLATE_STR);


		// Wrapper class for the DbDataReader after calling get_categories 
		internal class JobTemplatesDbReader : StoredProcDbReader
		{
			public JobTemplatesDbReader(DbDataReader reader) : base(reader) { }

			private enum Fields
			{
				Id = 0,
				Name,
				Description,
				WorkspaceId,
				RunSettingId,
				StructureId,
				Priority,
				MinMachineUnits,
				MaxMachineUnits,
				UnitType,
				MinWorkerMemory,
				InstanceType,
				CeVersion,
				PluginVersion,
				Compiler,
				EnableAvx,
				EnableGpu,
				SimsPerTask,
				MpBatchSize,
				EnablePmpp,
				EnableTmpo,
				SplitType,
				SplitNestedStructs,
				CalculatedVariables,
				ResultDefinitions,
				StartDate,
				FutureAccumPeriod,
				PastAccumPeriod,
				CompanyYearEnd,
				DynamicProjPeriod,
				RunNumbers,
				Simulations,
				ProduceXpsRunlog
			}

			public static async Task<JobTemplatesDbReader> GetJobTemplates(System.Guid? templateId, DbTransaction trans)
			{
				var spParams = new IDbDataParameter[]
				{
					PgSqlStoredProcExecuter.CreateSQLInParam(templateId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id")
				};

				var dr = await PgSqlStoredProcExecuter.ExecuteReader(s_get_job_templates.Value, s_get_job_templates.Key, spParams, trans);

				return new JobTemplatesDbReader(dr);
			}

			public static async Task<JobTemplatesDbReader> GetJobTemplates(System.Guid? templateId)
			{
				var spParams = new IDbDataParameter[]
				{
					PgSqlStoredProcExecuter.CreateSQLInParam(templateId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id")
				};

				var dr = await PgSqlStoredProcExecuter.ExecuteReader(s_get_job_templates.Value, s_get_job_templates.Key, spParams);

				return new JobTemplatesDbReader(dr);
			}

			public Guid Id => GetGuidFromGuid((int)Fields.Id);
			public string Name => GetString((int)Fields.Name);
			public string Description => GetString((int)Fields.Description);
			public Guid WorkspaceId => GetGuidFromGuid((int)Fields.WorkspaceId);
			public Guid RunSettingId => GetGuidFromGuid((int)Fields.RunSettingId);
			public Guid StructureId => GetGuidFromGuid((int)Fields.StructureId);
			public int Priority => GetInt((int)Fields.Priority);
			public int MinMachineUnits => GetInt((int)Fields.MinMachineUnits);
			public int MaxMachineUnits => GetInt((int)Fields.MaxMachineUnits);
			public short UnitType => GetShort((int)Fields.UnitType);
			public int MinWorkerMemory => GetInt((int)Fields.MinWorkerMemory);
			public string InstanceType => GetString((int)Fields.InstanceType);
			public string CeVersion => GetString((int)Fields.CeVersion);
			public string PluginVersion => GetString((int)Fields.PluginVersion);
			public string Compiler => GetString((int)Fields.Compiler);
			public bool EnableAvx => GetBoolean((int)Fields.EnableAvx);
			public bool EnableGpu => GetBoolean((int)Fields.EnableGpu);
			public int SimsPerTask => GetInt((int)Fields.SimsPerTask);
			public int MpBatchSize => GetInt((int)Fields.MpBatchSize);
			public bool EnablePmpp => GetBoolean((int)Fields.EnablePmpp);
			public bool EnableTmpo => GetBoolean((int)Fields.EnableTmpo);
			public int SplitType => GetInt((int)Fields.SplitType);
			public bool SplitNestedStructs => GetBoolean((int)Fields.SplitNestedStructs);
			public string[] CalculatedVariables => GetArray<string>((int)Fields.CalculatedVariables);
			public string[] ResultDefinitions => GetArray<string>((int)Fields.ResultDefinitions);
			public DateTime StartDate => GetDateTime((int)Fields.StartDate);
			public int FutureAccumPeriod => GetInt((int)Fields.FutureAccumPeriod);
			public int PastAccumPeriod => GetInt((int)Fields.PastAccumPeriod);
			public int CompanyYearEnd => GetInt((int)Fields.CompanyYearEnd);
			public int DynamicProjPeriod => GetInt((int)Fields.DynamicProjPeriod);
			public string RunNumbers => GetString((int)Fields.RunNumbers);
			public string Simulations => GetString((int)Fields.Simulations);
			public bool ProduceXpsRunlog => GetBoolean((int)Fields.ProduceXpsRunlog);
		}

		public static Task<Guid?> InsertJobTemplate(JobTemplateData newJobTemplate, DbTransaction trans)
		{
			var spParams = new IDbDataParameter[]
			{
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Name, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_name"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Description, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 1000, "p_description"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.WorkspaceId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_workspace_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.RunSettingId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_run_setting_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.StructureId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_structure_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Priority, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_priority"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MinMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MaxMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_max_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.UnitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.SMALLINT, "p_unit_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MinWorkerMemory, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_worker_memory"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.InstanceType, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_instance_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CeVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_ce_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.PluginVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_plugin_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Compiler, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_compiler"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableAvx, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_avx"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableGpu, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_gpu"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SimsPerTask, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_sims_per_task"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MpBatchSize, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_mp_batch_size"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnablePmpp, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_pmpp"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableTmpo, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_tmpo"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SplitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_split_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SplitNestedStructs, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_split_nested_structs"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CalculatedVariables, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_calculated_variables"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.ResultDefinitions, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_result_definitions"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.StartDate, PgSqlStoredProcExecuter.NpsSqlDbTypes.DATE, "p_start_date"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.FutureAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_future_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.PastAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_past_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CompanyYearEnd, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_company_year_end"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.DynamicProjPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_dynamic_proj_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.RunNumbers, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_run_numbers"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Simulations, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_simulations"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.ProduceXpsRunlog, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_produce_xps_runlog")
			};

			// Execute as a scalar, so we get the ID of the newly inserted row
			return PgSqlStoredProcExecuter.ExecuteScalar<Guid>(s_insert_job_template.Value, s_insert_job_template.Key, spParams, trans);
		}

		public static Task<Guid?> InsertJobTemplate(JobTemplateData newJobTemplate)
		{
			var spParams = new IDbDataParameter[]
			{
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Name, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_name"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Description, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 1000, "p_description"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.WorkspaceId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_workspace_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.RunSettingId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_run_setting_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.StructureId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_structure_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Priority, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_priority"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MinMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MaxMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_max_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.UnitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.SMALLINT, "p_unit_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MinWorkerMemory, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_worker_memory"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.InstanceType, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_instance_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CeVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_ce_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.PluginVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_plugin_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Compiler, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_compiler"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableAvx, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_avx"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableGpu, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_gpu"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SimsPerTask, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_sims_per_task"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.MpBatchSize, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_mp_batch_size"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnablePmpp, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_pmpp"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.EnableTmpo, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_tmpo"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SplitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_split_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.SplitNestedStructs, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_split_nested_structs"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CalculatedVariables, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_calculated_variables"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.ResultDefinitions, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_result_definitions"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.StartDate, PgSqlStoredProcExecuter.NpsSqlDbTypes.DATE, "p_start_date"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.FutureAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_future_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.PastAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_past_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.CompanyYearEnd, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_company_year_end"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.DynamicProjPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_dynamic_proj_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.RunNumbers, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_run_numbers"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.Simulations, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_simulations"),
				PgSqlStoredProcExecuter.CreateSQLInParam(newJobTemplate.ProduceXpsRunlog, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_produce_xps_runlog")
			};

			// Execute as a scalar, so we get the ID of the newly inserted row
			return PgSqlStoredProcExecuter.ExecuteScalar<Guid>(s_insert_job_template.Value, s_insert_job_template.Key, spParams);
		}

		public static Task<int> DeleteJobTemplate(Guid templateId, DbTransaction trans)
		{

			var spParams = new IDbDataParameter[]
			{
				PgSqlStoredProcExecuter.CreateSQLInParam(templateId,  PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id")
			};

			return PgSqlStoredProcExecuter.ExecuteNonQuery(s_delete_job_template.Value, s_delete_job_template.Key, spParams, trans);
		}

		public static Task<int> DeleteJobTemplate(Guid templateId)
		{

			var spParams = new IDbDataParameter[]
			{
				PgSqlStoredProcExecuter.CreateSQLInParam(templateId,  PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id")
			};

			return PgSqlStoredProcExecuter.ExecuteNonQuery(s_delete_job_template.Value, s_delete_job_template.Key, spParams);
		}

		public static Task<int> UpdateJobTemplate(JobTemplateData updateJobTemplate, DbTransaction trans)
		{
			var spParams = new IDbDataParameter[]
			{
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Id,  PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Name, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_name"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Description, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 1000, "p_description"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.WorkspaceId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_workspace_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.RunSettingId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_run_setting_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.StructureId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_structure_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Priority, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_priority"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MinMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MaxMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_max_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.UnitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.SMALLINT, "p_unit_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MinWorkerMemory, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_worker_memory"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.InstanceType, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_instance_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CeVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_ce_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.PluginVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_plugin_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Compiler, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_compiler"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableAvx, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_avx"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableGpu, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_gpu"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SimsPerTask, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_sims_per_task"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MpBatchSize, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_mp_batch_size"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnablePmpp, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_pmpp"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableTmpo, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_tmpo"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SplitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_split_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SplitNestedStructs, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_split_nested_structs"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CalculatedVariables, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_calculated_variables"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.ResultDefinitions, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_result_definitions"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.StartDate, PgSqlStoredProcExecuter.NpsSqlDbTypes.DATE, "p_start_date"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.FutureAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_future_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.PastAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_past_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CompanyYearEnd, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_company_year_end"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.DynamicProjPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_dynamic_proj_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.RunNumbers, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_run_numbers"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Simulations, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_simulations"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.ProduceXpsRunlog, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_produce_xps_runlog")


			};

			return PgSqlStoredProcExecuter.ExecuteNonQuery(s_update_job_template.Value, s_update_job_template.Key, spParams, trans);
		}

		public static Task<int> UpdateJobTemplate(JobTemplateData updateJobTemplate)
		{
			var spParams = new IDbDataParameter[]
			{

				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Id,  PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Name, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_name"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Description, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 1000, "p_description"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.WorkspaceId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_workspace_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.RunSettingId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_run_setting_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.StructureId, PgSqlStoredProcExecuter.NpsSqlDbTypes.UUID, "p_structure_id"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Priority, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_priority"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MinMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MaxMachineUnits, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_max_machine_units"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.UnitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.SMALLINT, "p_unit_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MinWorkerMemory, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_min_worker_memory"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.InstanceType, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_instance_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CeVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_ce_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.PluginVersion, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 50, "p_plugin_version"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Compiler, PgSqlStoredProcExecuter.NpsSqlDbTypes.VARCHAR, 100, "p_compiler"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableAvx, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_avx"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableGpu, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_gpu"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SimsPerTask, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_sims_per_task"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.MpBatchSize, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_mp_batch_size"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnablePmpp, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_pmpp"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.EnableTmpo, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_enable_tmpo"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SplitType, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_split_type"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.SplitNestedStructs, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_split_nested_structs"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CalculatedVariables, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_calculated_variables"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.ResultDefinitions, PgSqlStoredProcExecuter.NpsSqlDbTypes.ARRAY | PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_result_definitions"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.StartDate, PgSqlStoredProcExecuter.NpsSqlDbTypes.DATE, "p_start_date"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.FutureAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_future_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.PastAccumPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_past_accum_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.CompanyYearEnd, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_company_year_end"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.DynamicProjPeriod, PgSqlStoredProcExecuter.NpsSqlDbTypes.INTEGER, "p_dynamic_proj_period"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.RunNumbers, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_run_numbers"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.Simulations, PgSqlStoredProcExecuter.NpsSqlDbTypes.TEXT, "p_simulations"),
				PgSqlStoredProcExecuter.CreateSQLInParam(updateJobTemplate.ProduceXpsRunlog, PgSqlStoredProcExecuter.NpsSqlDbTypes.BOOLEAN, "p_produce_xps_runlog")

			};

			return PgSqlStoredProcExecuter.ExecuteNonQuery(s_update_job_template.Value, s_update_job_template.Key, spParams);
		}
	}
}
