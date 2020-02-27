using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;
using Npgsql;
using Prophet.SaaS.Database.Access.Interfaces;

namespace Prophet.SaaS.PostgreSQL.Access
{
	/// <summary>
	/// Base object responsible for performing the PostgreSQL DB interactions with a query.
	/// </summary>
	/// <remarks>DB interactions as performed asynchronously. The class also provides a switch
	/// to enable the specialized supporting code to also perform actions asynchronously.</remarks>
	public class PgSqlDbObject : IDbExecutor
	{
		// Category name used for any logging messages
		private readonly string _loggingCategory = "Data Access";
		protected virtual int NumRetries => 5;

		protected ILogging Logger { get; }
		private string ConnectionString { get; }

		// Dependency Injected support classes
		public PgSqlDbObject(ILogging logger, string connectionString)
		{
			Logger = logger;
			ConnectionString = connectionString;
		}

		private bool _disposed;

		~PgSqlDbObject()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				// Remove other resources
			}
			_disposed = true;
		}

		// Use the connection string provided when opening the connection to perform the DB interactions
		private NpgsqlConnection GetConnection()
		{
			return new NpgsqlConnection(ConnectionString);
		}

		/// <summary>
		/// Main function that performs the request DB interactions. All actions occur within a transactions,
		/// ensuing an atomic process.
		/// </summary>
		/// <typeparam name="Tx">The data object type that will be returned as a result of the DB interaction.</typeparam>
		/// <param name="method">Callback method responsible for access the DB and processing the data into C# classes.</param>
		/// <param name="isAsync">Whether the processing code should operate asynchronously. NOTE: underlying DB code will
		/// always operate asynchronously.</param>
		/// <returns>An Task to promise the required return type.</returns>
		[return: MaybeNull]
		public async Task<Tx> ExecuteInTransaction<Tx>(Func<DbTransaction, bool, Task<Tx>> method, bool isAsync)
		{
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
			var result = default(Tx);
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.

			var exceptionRethrown = false;

			// We want to retry the transaction several times if there was a deadlock
			var retryAgain = true;
			var tries = 0;

			using var cnn = GetConnection();
			try
			{
				while (retryAgain && tries < NumRetries)
				{
					tries++;
					retryAgain = false;
					exceptionRethrown = false;

					try
					{
						if (cnn.State == System.Data.ConnectionState.Closed)
							await cnn.OpenAsync().ConfigureAwait(false);

						var trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

						try
						{
							// Since the underlying DB operations are going to be asynchronous, we should await here
							result = await method(trans, isAsync);

							// In the case of update operations, commit those changes
							trans.Commit();
						}
						catch (Exception ex)
						{
							// Make a note of what initially went wrong
							Logger.Log(ex, _loggingCategory, @"Error processing data query.");

							try
							{
								trans.Rollback();
							}
							catch (Exception rollbackEx)
							{
								// If the rollback failed, it's probably because our DB process has failed, so we're hoping
								// the transaction has been discarded by the DB Server anyway
								Logger.Log(rollbackEx, _loggingCategory, "Transaction could not be rolled back.");
							}

							// Check for a deadlock exception and indicate a retry
							var sqlEx = ex as PostgresException;
							if (sqlEx?.SqlState == PostgresErrorCodes.DeadlockDetected)
							{
								Logger.Log(LogEntrySeverity.Error, _loggingCategory, @"Deadlock detected. Waiting to re-run query.");
								retryAgain = true;
							}

							// If we won't be pausing and retrying again, just re-throw the error
							if (!retryAgain || tries >= NumRetries)
							{
								exceptionRethrown = true;
								throw;
							}
						}
					}
					catch (NpgsqlException npEx) when (npEx.Message == @"Exception while connecting")
					{
						// In the case of a connection failure, just try again in case there were temporary network problems
						retryAgain = true;
					}
					finally
					{
						try
						{
							await cnn.CloseAsync().ConfigureAwait(false);
						}
						catch
						{
							Logger.Log(LogEntrySeverity.Error, _loggingCategory, @"Error closing connection.");
						}
					}

					if (retryAgain && tries < NumRetries)
					{
						var rndm = new Random();

						// Release the thread for other use whilst we wait
						await Task.Delay(rndm.Next(1000, 2000)).ConfigureAwait(false);
					}
				}
			}
			catch (Exception ex)
			{
				// If this was re-thrown from the internal try..catch, then we would've already logged the exception
				if (!exceptionRethrown)
					Logger.Log(ex, _loggingCategory, @"Error processing data query.");

				throw;
			}
			finally
			{
				cnn.Dispose();
			}

			return result;
		}
	}
}
