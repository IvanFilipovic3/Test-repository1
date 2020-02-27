using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;
using Prophet.SaaS.Database.Access;
using Prophet.SaaS.Modelling.API.DataModels;

namespace Prophet.SaaS.Modelling.API.DataAccess
{
	/// <summary>
	/// Provides the logic to interact with the database, controlling transactions and ensuring an atomic operation.
	/// </summary>
	/// <remarks>All the methods return Tasks, since the underlying code and perhaps the calling code will be using awaits.</remarks>
	public class JobTemplatesDbSource : DbSource<JobTemplateData>, IDbSourceReadWrite<JobTemplateData>
	{
		public JobTemplatesDbSource(ILogging logger, AdoDbSourceOptions<JobTemplatesDbSource> options)
			: base(logger, options)
		{ }

		// Public function get the list of data model that represent all the rows in the table
		public virtual Task<List<JobTemplateData>> GetDataList(bool isAsync)
		{
			// Execute the DB operation in a transaction, using the passing action to perform the actual DB operation and
			// convert the results to the appropriate types
			return DbExecutor.ExecuteInTransaction((tx, a) => GetDataList(tx, a), isAsync);
		}

		// Public function get the data model that represents the first row in the table thats matches the requested ID
		public virtual Task<JobTemplateData?> GetDataItem(Guid objectId, bool isAsync)
		{
			return DbExecutor.ExecuteInTransaction((tx, a) => GetDataItem(tx, objectId, a), isAsync);
		}

		// Public function to insert the data model into the table
		public virtual Task<Guid?> Insert(JobTemplateData newObject, bool isAsync)
		{
			return DbExecutor.ExecuteInTransaction((tx, a) => Insert(tx, newObject), isAsync);
		}

		// Public function to update the first row in the table that matches the ID of the passed in data model,
		// with the values from data model
		public virtual Task<int> Update(JobTemplateData updateObject, bool isAsync)
		{
			return DbExecutor.ExecuteInTransaction((tx, a) => Update(tx, updateObject), isAsync);
		}

		// Public function remove the row from the table that matches the requested ID
		public virtual Task<int> Remove(Guid objectId, bool isAsync)
		{
			return DbExecutor.ExecuteInTransaction((tx, a) => Remove(tx, objectId), isAsync);
		}

		private async static Task<List<JobTemplateData>> GetDataList(DbTransaction trans, bool isAsync)
		{
			// Get a reader for the record set. Use await to allow other processes to run while DB operations occur
			using var itemReader = await JobTemplatesDbHandler.JobTemplatesDbReader.GetJobTemplates(null, trans);

			// Pass the reader into a converter, in order to translate the DB rows into the appropriate data model.
			// If the initial call was requested as asynchronous, then tell the converter it should perform operations asynchronously
			// where appropriate. 
			return isAsync
				? await JobTemplatesDataConverter.ReaderToList(itemReader, trans)
				: JobTemplatesDataConverter.ReaderToList(itemReader, trans).Result;
		}

		private async static Task<JobTemplateData?> GetDataItem(DbTransaction trans, Guid templateId, bool isAsync)
		{
			// Get a reader for the record set. Use await to allow other processes to run while DB operations occur
			using var itemReader = await JobTemplatesDbHandler.JobTemplatesDbReader.GetJobTemplates(templateId, trans);

			// Pass the reader into a converter, in order to translate the DB rows into the appropriate data model.
			// If the initial call was requested as asynchronous, then tell the converter it should perform operations asynchronously
			// where appropriate. 
			var dataList = isAsync
				? await JobTemplatesDataConverter.ReaderToList(itemReader, trans)
				: JobTemplatesDataConverter.ReaderToList(itemReader, trans).Result;

			return dataList.FirstOrDefault();
		}

		private static Task<int> Remove(DbTransaction trans, Guid templateId) => JobTemplatesDbHandler.DeleteJobTemplate(templateId, trans);

		private static Task<int> Update(DbTransaction trans, JobTemplateData template) => JobTemplatesDbHandler.UpdateJobTemplate(template, trans);

		private static Task<Guid?> Insert(DbTransaction trans, JobTemplateData template) => JobTemplatesDbHandler.InsertJobTemplate(template, trans);
	}


	// The DB context class that will be created through DI, providing access to the required table readers
	public class JobTemplatesDbContext : AdoDbContext
	{
		private IDbSourceReadWrite<JobTemplateData> SourceAsReadWrite { get; }

		public JobTemplatesDbContext(ILogging logger, JobTemplatesDbSource dbSource)
			: base(logger, dbSource)
		{
			SourceAsReadWrite = (dbSource as IDbSourceReadWrite<JobTemplateData>) ?? throw new ArgumentNullException(nameof(dbSource));
		}

		public AdoDbSet<JobTemplateData> JobTemplateItems => new AdoDbSet<JobTemplateData>(Logger, SourceAsReadWrite);
	}
}
