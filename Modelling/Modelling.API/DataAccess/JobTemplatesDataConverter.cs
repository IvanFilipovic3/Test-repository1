// This file is auto generated. Do not change by hand
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Prophet.SaaS.Modelling.API.DataModels;

namespace Prophet.SaaS.Modelling.API.DataAccess
{
	internal static class JobTemplatesDataConverter
	{
		// Convert a reader of rows into a list JobTemplateData objects
		internal static async Task<List<JobTemplateData>> ReaderToList(JobTemplatesDbHandler.JobTemplatesDbReader itemRdr, DbTransaction trans)
		{
			var returnList = new List<JobTemplateData>();

			// Perform the DB read asynchronously, to give other threads a change to perform some processing. Don't worry about using
			// await on the field operations, since the object will be in memory by that point anyway
			while (await itemRdr.ReadAsync())
			{
				var newItem = new JobTemplateData();

				newItem.Id = itemRdr.Id;
				newItem.Name = itemRdr.Name;
				newItem.Description = itemRdr.Description;
				newItem.WorkspaceId = itemRdr.WorkspaceId;
				newItem.RunSettingId = itemRdr.RunSettingId;
				newItem.StructureId = itemRdr.StructureId;
				newItem.Priority = itemRdr.Priority;
				newItem.MinMachineUnits = itemRdr.MinMachineUnits;
				newItem.MaxMachineUnits = itemRdr.MaxMachineUnits;
				newItem.UnitType = itemRdr.UnitType;
				newItem.MinWorkerMemory = itemRdr.MinWorkerMemory;
				newItem.InstanceType = itemRdr.InstanceType;
				newItem.CeVersion = itemRdr.CeVersion;
				newItem.PluginVersion = itemRdr.PluginVersion;
				newItem.Compiler = itemRdr.Compiler;
				newItem.EnableAvx = itemRdr.EnableAvx;
				newItem.EnableGpu = itemRdr.EnableGpu;
				newItem.SimsPerTask = itemRdr.SimsPerTask;
				newItem.MpBatchSize = itemRdr.MpBatchSize;
				newItem.EnablePmpp = itemRdr.EnablePmpp;
				newItem.EnableTmpo = itemRdr.EnableTmpo;
				newItem.SplitType = itemRdr.SplitType;
				newItem.SplitNestedStructs = itemRdr.SplitNestedStructs;
				newItem.CalculatedVariables.Clear();
				newItem.CalculatedVariables.AddRange(itemRdr.CalculatedVariables);
				newItem.ResultDefinitions.Clear();
				newItem.ResultDefinitions.AddRange(itemRdr.ResultDefinitions);
				newItem.StartDate = itemRdr.StartDate;
				newItem.FutureAccumPeriod = itemRdr.FutureAccumPeriod;
				newItem.PastAccumPeriod = itemRdr.PastAccumPeriod;
				newItem.CompanyYearEnd = itemRdr.CompanyYearEnd;
				newItem.DynamicProjPeriod = itemRdr.DynamicProjPeriod;
				newItem.RunNumbers = itemRdr.RunNumbers;
				newItem.Simulations = itemRdr.Simulations;
				newItem.ProduceXpsRunlog = itemRdr.ProduceXpsRunlog;

				returnList.Add(newItem);
			}

			return returnList;
		}
	}
}
