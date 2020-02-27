using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;

namespace Prophet.SaaS.Database.Access
{
	// A class designed to slightly mock the standard DbSet, providing access to the database queries of the related table
	public class AdoDbSet<T>
		where T : class
	{
		private ILogging Logger { get; }
		private IDbSourceReadWrite<T> DbSource { get; }

		public AdoDbSet(ILogging logger, IDbSourceReadWrite<T> dbSource)
		{
			Logger = logger;
			DbSource = dbSource;
		}

		// Queries the DB and returns a list of all the rows as the appropriate data models
		public List<T> ToList()
		{
			return DbSource.GetDataList(false).Result;
		}

		// Queries the DB and returns a list of all the rows as the appropriate data models
		public Task<List<T>> ToListAsync()
		{
			return DbSource.GetDataList(true);
		}

		// Queries the DB for all rows with a matching ID, and returns the first row as the appropriate data model.
		// Returns null if no match is found
		public T? Find(Guid id)
		{
			return DbSource.GetDataItem(id, false).Result;
		}

		// Queries the DB for all rows with a matching ID, and returns the first row as the appropriate data model
		// Returns null if no match is found
		public Task<T?> FindAsync(Guid id)
		{
			return DbSource.GetDataItem(id, true);
		}

		// Creates a new row in the DB for the passed in data model and returns the ID of the newly created row
		public Guid? Add(T newData)
		{
			return DbSource.Insert(newData, false).Result;
		}

		// Creates a new row in the DB for the passed in data model and returns the ID of the newly created row
		public Task<Guid?> AddAsync(T newData)
		{
			return DbSource.Insert(newData, true);
		}

		// Removes the rows from DB that have a matching ID.
		// The return value will always be -1. If no matching rows are found, the method executes and returns -1 still.
		public int Remove(Guid templateId)
		{
			return DbSource.Remove(templateId, false).Result;
		}

		// Removes the rows from DB that have a matching ID.
		// The return value will always be -1. If no matching rows are found, the method executes and returns -1 still.
		public Task<int> RemoveAsync(Guid templateId)
		{
			return DbSource.Remove(templateId, true);
		}

		// Find the first row with the matching ID to the data model passed in, and replace all the values with
		// those from the data model passing in.
		// The return value will always be -1. If no matching rows are found, the method executes and returns -1 still.
		public int Update(T newData)
		{
			return DbSource.Update(newData, false).Result;
		}

		// Find the first row with the matching ID to the data model passed in, and replace all the values with
		// those from the data model passing in.
		// The return value will always be -1. If no matching rows are found, the method executes and returns -1 still.
		public Task<int> UpdateAsync(T newData)
		{
			return DbSource.Update(newData, true);
		}
	}
}
