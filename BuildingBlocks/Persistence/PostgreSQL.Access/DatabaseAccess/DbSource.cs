using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FIS.Risk.Core.Logging;
using Prophet.SaaS.Database.Access.Interfaces;

namespace Prophet.SaaS.Database.Access
{
	public interface IDbSourceRead<T>
		where T : class
	{
		Task<List<T>> GetDataList(bool isAsync);
		Task<T?> GetDataItem(Guid objectId, bool isAsync);
	}

	public interface IDbSourceWrite<T>
		where T : class
	{
		Task<Guid?> Insert(T newObject, bool isAsync);
		Task<int> Update(T updateObject, bool isAsync);
		Task<int> Remove(Guid objectId, bool isAsync);
	}

	public interface IDbSourceReadWrite<T> : IDbSourceRead<T>, IDbSourceWrite<T>
		where T : class
	{ }

	public abstract class DbSource : IDisposable
	{
		protected IDbExecutor DbExecutor { get; }
		protected ILogging Logger { get; }

		protected DbSource(ILogging logger, AdoDbSourceOptions options)
			: this(logger, options.DbExectorCreator)
		{ }

		// Split out a constructor that uses the options, so the arguement exception works as expected
		private DbSource(ILogging logger, [AllowNull] Func<ILogging, IDbExecutor> dbExectorCreator)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			DbExecutor = dbExectorCreator?.Invoke(logger) ?? throw new ArgumentNullException(nameof(dbExectorCreator));
		}

		private bool _disposed;

		~DbSource()
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
				DbExecutor.Dispose();
			}
			_disposed = true;
		}
	}

	public abstract class DbSource<T> : DbSource
		where T : class
	{
		protected DbSource(ILogging logger, AdoDbSourceOptions options)
			: base(logger, options)
		{ }
	}
}
