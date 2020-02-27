using System;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace Prophet.SaaS.PostgreSQL.Access
{
	/// <summary>
	/// Class to encapsulate the DB specific reader that is returned to access a record set returned by a stored procedure.
	/// </summary>
	/// <remarks>Methods are provided to access the reader through both asynchronous and synchronous means.</remarks>
	public class StoredProcDbReader : IDisposable
	{
		private readonly NpgsqlDataReader _reader;
		private bool _disposed;

		public StoredProcDbReader(DbDataReader reader)
		{
			// This reader was created in this library, so if an exception is thrown, we've got bigger issues, so no point trying to
			// handle a null condition. If something has gone awry, because we probably have some basic application set-up issues
			_reader = (NpgsqlDataReader)reader;
		}

		~StoredProcDbReader()
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
				_reader.Dispose();
			}
			_disposed = true;
		}

		private async Task<T> GetValueAsync<T>(int col, Func<T> defaultValue)
		{
			if (await _reader.IsDBNullAsync(col).ConfigureAwait(false))
			{
				return defaultValue();
			}
			else
			{
				return await _reader.GetFieldValueAsync<T>(col).ConfigureAwait(false);
			}
		}

		public bool Read()
		{
			return _reader.Read();
		}

		public Task<bool> ReadAsync()
		{
			return _reader.ReadAsync();
		}

		public bool NextRecordSet()
		{
			return _reader.NextResult();
		}

		public Task<bool> NextRecordSetAsync()
		{
			return _reader.NextResultAsync();
		}

		public short GetShort(int col)
		{
			return _reader.IsDBNull(col) ? default : _reader.GetInt16(col);    //smallint
		}

		public Task<short> GetShortAsync(int col)
		{
			return GetValueAsync<short>(col, () => default);    //smallint
		}

		public int GetInt(int col)
		{
			return _reader.IsDBNull(col) ? default : _reader.GetInt32(col);   //integer
		}

		public Task<int> GetIntAsync(int col)
		{
			return GetValueAsync<int>(col, () => default);    //integer
		}

		public long GetLong(int col)
		{
			return _reader.IsDBNull(col) ? default : _reader.GetInt64(col); //bigint
		}

		public Task<long> GetLongAsync(int col)
		{
			return GetValueAsync<long>(col, () => default);    //bigint
		}

		public double GetDouble(int col)
		{
			return _reader.IsDBNull(col) ? default : _reader.GetDouble(col);    //double precision
		}

		public Task<double> GetDoubleAsync(int col)
		{
			return GetValueAsync<double>(col, () => default);    //double precision
		}

		public bool GetBoolean(int col)
		{
			return _reader.IsDBNull(col) ? default : _reader.GetBoolean(col);
		}

		public Task<bool> GetBooleanAsync(int col)
		{
			return GetValueAsync<bool>(col, () => default);
		}

		public string GetString(int col)
		{
			return _reader.IsDBNull(col) ? string.Empty : _reader.GetString(col);
		}

		public Task<string> GetStringAsync(int col)
		{
			return GetValueAsync<string>(col, () => string.Empty);
		}

		public Guid GetGuidFromGuid(int col)
		{
			var guid = Guid.Empty;

			if (!_reader.IsDBNull(col))
			{
				try
				{
					guid = _reader.GetGuid(col);    //uuid
				}
				catch
				{
					guid = Guid.Empty;
				}
			}

			return guid;
		}

		public Task<Guid> GetGuidFromGuidAsync(int col)
		{
			try
			{
				return GetValueAsync<Guid>(col, () => Guid.Empty);   //uuid
			}
			catch
			{
				return Task.FromResult(Guid.Empty);
			}
		}

		public DateTime GetDateTime(int col)
		{
			return _reader.IsDBNull(col) ? DateTime.MinValue : _reader.GetDateTime(col);    //timestamp (UTC)
		}

		public Task<DateTime> GetDateTimeAsync(int col)
		{
			return GetValueAsync<DateTime>(col, () => DateTime.MinValue);   //timestamp (UTC)
		}

		public T[] GetArray<T>(int col)
		{
			return _reader.IsDBNull(col) ? new T[0] : _reader.GetFieldValue<T[]>(col);
		}

		public Task<T[]> GetArrayAsync<T>(int col)
		{
			try
			{
				return GetValueAsync<T[]>(col, () => new T[0]);
			}
			catch
			{
				return Task.FromResult(new T[0]);
			}
		}

		public void Close()
		{
			_reader.Close();
		}

		public Task CloseAsync()
		{
			return _reader.CloseAsync();
		}
	}
}
