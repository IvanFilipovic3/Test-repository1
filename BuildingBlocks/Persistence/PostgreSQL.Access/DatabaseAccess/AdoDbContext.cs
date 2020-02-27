using System;
using System.Diagnostics.CodeAnalysis;
using FIS.Risk.Core.Logging;

namespace Prophet.SaaS.Database.Access
{
	public abstract class AdoDbContext : IDisposable
	{
		public ILogging Logger { get; }
		public DbSource DatabaseSource { get; }

		protected AdoDbContext(ILogging logger, [AllowNull]DbSource dbSource)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			DatabaseSource = dbSource ?? throw new ArgumentNullException(nameof(dbSource));
		}

		private bool _disposed;

		~AdoDbContext()
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
				(DatabaseSource as IDisposable)?.Dispose();
			}
			_disposed = true;
		}
	}

}
