using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Prophet.SaaS.Database.Access.Interfaces
{
	public interface IDbExecutor : IDisposable
	{
		Task<Tx> ExecuteInTransaction<Tx>(Func<DbTransaction, bool, Task<Tx>> method, bool isAsync);
	}
}
