using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Prophet.SaaS.Database.Access.Exceptions;

namespace Prophet.SaaS.PostgreSQL.Access
{
	/// <summary>
	/// Provides the PostgreSQL specific code to configure and call stored procedures
	/// </summary>
	public static class PgSqlStoredProcExecuter
	{
		// Provide a conversion from the NpgSqlType name to the actual type to prevent having to include the Npgsql Package in all other projects 
		public struct NpsSqlDbTypes
		{
			public static NpgsqlDbType INTEGER => NpgsqlDbType.Integer;
			public static NpgsqlDbType UUID => NpgsqlDbType.Uuid;
			public static NpgsqlDbType VARCHAR => NpgsqlDbType.Varchar;
			public static NpgsqlDbType BOOLEAN => NpgsqlDbType.Boolean;
			public static NpgsqlDbType SMALLINT => NpgsqlDbType.Smallint;
			public static NpgsqlDbType TEXT => NpgsqlDbType.Text;
			public static NpgsqlDbType DATE => NpgsqlDbType.Date;
			public static NpgsqlDbType ARRAY => NpgsqlDbType.Array;
		}

		// Use the connection string provided when opening the connection to perform the DB interactions
		private static NpgsqlConnection GetConnection()
		{
			return new NpgsqlConnection(string.Empty);
		}

		/// <summary>
		/// Create a string parameter for a stored procedure.
		/// </summary>
		/// <param name="value">The string value of the parameter to pass in.</param>
		/// <param name="sqlType">The database type of the parameter.</param>
		/// <param name="size">The size of string the database column will accept.</param>
		/// <param name="name">The name of the parameter to set.</param>
		/// <returns>A new DbParameter object.</returns>
		public static NpgsqlParameter CreateSQLInParam(string value, NpgsqlDbType sqlType, int size, string name)
		{
			// If the string is going to be too long, throw an error. In theory this should never actually happen
			// because the system should be restricting the input values anyway
			if ((sqlType == NpgsqlDbType.Varchar) && (value != null) && (size < value.Length))
				throw new TruncatedDataException(name, value.Length, size);

			var param = CreateSQLInParam(value, sqlType, name);
			param.Size = size;

			return param;
		}

		/// <summary>
		/// Create a generic parameter for a stored procedure.
		/// </summary>
		/// <typeparam name="T">The C# non-nullable type of the value being passed in.</typeparam>
		/// <param name="value">The value of the parameter to pass in.</param>
		/// <param name="sqlType">The database type of the parameter.</param>
		/// <param name="name">The name of the parameter to set.</param>
		/// <returns>A new DbParameter object.</returns>
		public static NpgsqlParameter CreateSQLInParam<T>(T value, NpgsqlDbType sqlType, string name)
		{
			if (value == null)
			{
				return new NpgsqlParameter(name, sqlType)
				{
					Direction = ParameterDirection.Input,
					Value = DBNull.Value
				};
			}
			else
			{
				return new NpgsqlParameter<T>(name, sqlType)
				{
					Direction = ParameterDirection.Input,
					Value = value
				};
			}
		}

		/// <summary>
		/// Create a generic parameter for a stored procedure that takes a nullable value type.
		/// </summary>
		/// <typeparam name="T">The C# value type of the value being passed in, which can be nullable.</typeparam>
		/// <param name="value">The value of the parameter to pass in.</param>
		/// <param name="sqlType">The database type of the parameter.</param>
		/// <param name="name">The name of the parameter to set.</param>
		/// <returns>A new DbParameter object.</returns>
		public static NpgsqlParameter CreateSQLInParam<T>(T? value, NpgsqlDbType sqlType, string name) where T : struct
		{
			// Call second method to set up the type, because it'll use the underlying value type, rather than the nullable type
			if (value.HasValue)
				return CreateSQLInParam(value.Value, sqlType, name);

			// Else, just create a null value parameter
			var param = new NpgsqlParameter(name, sqlType)
			{
				Direction = ParameterDirection.Input,
				Value = (object)DBNull.Value
			};

			return param;
		}

		/// <summary>
		/// Provides the ability to define an return value from the stored procedure.
		/// </summary>
		/// <typeparam name="T">The C# type that the database value will have to be converted back to.</typeparam>
		/// <param name="sqlType">The database type of the parameter.</param>
		/// <param name="name">The name of the column to match when taking the value to set the parameter.</param>
		/// <returns>A new DbParameter object.</returns>
		public static NpgsqlParameter CreateSQLOutParam<T>(NpgsqlDbType sqlType, string name)
		{
			var param = new NpgsqlParameter<T>(name, sqlType)
			{
				Direction = ParameterDirection.ReturnValue
			};

			return param;
		}

		/// <summary>
		/// Create a SQL DbCommand that can be used to run a stored procedure
		/// </summary>
		/// <param name="cnn">The connection against which to run the stored procedure.</param>
		/// <param name="commandString">The SQL query to run, or the name of a stored procedure to run.</param>
		/// <param name="isStoredProcedure">Indicates whether the command string is the name of a stored procedure.</param>
		/// <param name="paramArray">An array of DbParameter objects that will be pased as arguments to the stored procedure.</param>
		/// <returns>A new DbCommand.</returns>
		/// <remarks>Note that this method is used to run a single atomic DB operation in its own connection.</remarks>
		private static NpgsqlCommand CreateDBCommand(NpgsqlConnection cnn, string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
		{
			var dbcmd = new NpgsqlCommand(commandString, cnn);
			if (isStoredProcedure)
			{
				dbcmd.CommandType = CommandType.StoredProcedure;
			}

			dbcmd.Parameters.AddRange(paramArray);

			return dbcmd;
		}

		/// <summary>
		/// Create a SQL DbCommand that can be used to run a stored procedure
		/// </summary>
		/// <param name="trans">The existing transaction in which to run the stored procedure.</param>
		/// <param name="commandString">The SQL query to run, or the name of a stored procedure to run.</param>
		/// <param name="isStoredProcedure">Indicates whether the command string is the name of a stored procedure.</param>
		/// <param name="paramArray">An array of DbParameter objects that will be passed as arguments to the stored procedure.</param>
		/// <returns>A new DbCommand.</returns>
		/// <remarks>Note that this method is used to a DB operation within an existing transaction, so it will be part of a bigger
		/// atomic opertion.</remarks>
		private static NpgsqlCommand CreateDBCommand(DbTransaction trans, string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
		{
			// This case should never fail, since we created the transaction in this assembly
			// As such, throw an exception if something has gone awry, because we probably have some basic application set-up issues
			var dbcmd = new NpgsqlCommand(commandString, ((NpgsqlTransaction)trans).Connection, (NpgsqlTransaction)trans);
			if (isStoredProcedure)
			{
				dbcmd.CommandType = CommandType.StoredProcedure;
			}

			dbcmd.Parameters.AddRange(paramArray);

			return dbcmd;
		}

		// Executes a single scalar stored procedure that will return a value type, as an atomic operation within a new connection.
		// This method is specific to value types, because we need to cast the result differently between value types and references,
		// in order to allow a null return value.
		// This code is kept as a single produced (rather than using a common shared method) in order to reduced the nesting of awaits
		public static async Task<T?> ExecuteScalar<T>(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
			where T : struct
		{
			object? result = null;
			using var cnn = GetConnection();

			try
			{
				await cnn.OpenAsync().ConfigureAwait(false);

				using var dbcmd = CreateDBCommand(cnn, commandString, isStoredProcedure, paramArray);

				result = await dbcmd.ExecuteScalarAsync().ConfigureAwait(false);
			}
			catch (PostgresException e)
			{
				CheckDbException(e);
				throw;
			}
			catch (Exception e)
			{
				throw new DataException("Error executing SQL statement", e);
			}
			finally
			{
				await cnn.CloseAsync().ConfigureAwait(false);
			}

			// If the results was null, return a nullable value type
			return result == DBNull.Value ? (T?)null : (T)result;
		}

		// Executes a scalar stored procedure that will return a value type, within an existing transaction.
		// This method is specific to value types, because we need to cast the result differently between value types and references,
		// in order to allow a null return value.
		public static async Task<T?> ExecuteScalar<T>(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray, DbTransaction trans)
			where T : struct
		{
			using var dbcmd = CreateDBCommand(trans, commandString, isStoredProcedure, paramArray);
			var spResult = await dbcmd.ExecuteScalarAsync().ConfigureAwait(false);

			return spResult == DBNull.Value ? (T?)null : (T)spResult;
		}

		// Executes a single scalar stored procedure that will return a reference, as an atomic operation within a new connection.
		// This method is specific to references, because we need to cast the result differently between value types and references,
		// in order to allow a null return value.
		// This code is kept as a single produced (rather than using a common shared method) in order to reduced the nesting of awaits
		public static async Task<T?> ExecuteScalarRef<T>(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
			where T : class
		{
			object? result = null;
			using var cnn = GetConnection();

			try
			{
				await cnn.OpenAsync().ConfigureAwait(false);

				using var dbcmd = CreateDBCommand(cnn, commandString, isStoredProcedure, paramArray);

				result = await dbcmd.ExecuteScalarAsync().ConfigureAwait(false);
			}
			catch (PostgresException e)
			{
				CheckDbException(e);
				throw;
			}
			catch (Exception e)
			{
				throw new DataException("Error executing SQL statement", e);
			}
			finally
			{
				await cnn.CloseAsync().ConfigureAwait(false);
			}

			// If the results was null, return null
			return result == DBNull.Value ? null : (T)result;
		}

		// Executes a scalar stored procedure that will return a reference, within an existing transaction.
		public static async Task<T?> ExecuteScalarRef<T>(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray, DbTransaction trans)
			where T : class
		{
			using var dbcmd = CreateDBCommand(trans, commandString, isStoredProcedure, paramArray);
			return (T)await dbcmd.ExecuteScalarAsync().ConfigureAwait(false);
		}

		// Creates a DBReader for the results of a SELECT stored procedure, within a new connection.
		public static async Task<DbDataReader> ExecuteReader(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
		{
			using var cnn = GetConnection();

			try
			{
				await cnn.OpenAsync().ConfigureAwait(false);

				using var dbcmd = CreateDBCommand(cnn, commandString, isStoredProcedure, paramArray);

				// Set up the connection to close when the reader is closed
				return await dbcmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
			}
			catch (PostgresException e)
			{
				CheckDbException(e);
				throw;
			}
			catch (Exception e)
			{
				throw new DataException("Error executing SQL statement", e);
			}
		}

		// Creates a DBReader for the results of a SELECT stored procedure, as part of an existing transaction.
		public static async Task<DbDataReader> ExecuteReader(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray, DbTransaction trans)
		{
			using var dbcmd = CreateDBCommand(trans, commandString, isStoredProcedure, paramArray);

			return await dbcmd.ExecuteReaderAsync().ConfigureAwait(false);
		}

		// Executes a stored procedure that returns the number of affected row, within a new connection.
		// Note: most stored procedures will be set up to not actually return the row count, the return value will always be -1
		public static async Task<int> ExecuteNonQuery(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray)
		{
			var rowsAffected = 0;
			using var cnn = GetConnection();

			try
			{
				await cnn.OpenAsync().ConfigureAwait(false);

				using var dbcmd = CreateDBCommand(cnn, commandString, isStoredProcedure, paramArray);

				rowsAffected = await dbcmd.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
			catch (PostgresException e)
			{
				CheckDbException(e);
				throw;
			}
			catch (Exception e)
			{
				throw new DataException("Error executing SQL statement", e);
			}
			finally
			{
				await cnn.CloseAsync().ConfigureAwait(false);
			}

			return rowsAffected;
		}

		// Executes a stored procedure that returns the number of affected row, within an existing transaction.
		// Note: most stored procedures will be set up to not actually return the row count, the return value will always be -1
		public static async Task<int> ExecuteNonQuery(string commandString, bool isStoredProcedure, IDbDataParameter[] paramArray, DbTransaction trans)
		{
			var rowsAffected = -1;

			try
			{
				using var dbcmd = CreateDBCommand(trans, commandString, isStoredProcedure, paramArray);
				rowsAffected = await dbcmd.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
			catch (PostgresException e)
			{
				CheckDbException(e);
				throw;
			}

			return rowsAffected;
		}

		// Have a common method for checking the DB exceptions, single they'll apply to all the execution methods
		private static void CheckDbException(PostgresException ex)
		{
			switch (ex.SqlState)
			{
				case PostgresErrorCodes.StringDataRightTruncation:
				case PostgresErrorCodes.StringDataRightTruncationWarning:
					throw new TruncatedDataException("Error executing SQL statement because data would be truncated", ex);
			}
		}
	}
}
