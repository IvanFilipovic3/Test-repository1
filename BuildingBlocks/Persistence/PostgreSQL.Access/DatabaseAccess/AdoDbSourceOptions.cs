using System;
using FIS.Risk.Core.Logging;
using Prophet.SaaS.Database.Access.Interfaces;

namespace Prophet.SaaS.Database.Access
{
	public class AdoDbSourceOptions
	{
		public Func<ILogging, IDbExecutor>? DbExectorCreator { get; set; }
	}

	// Use a generic version of this class, so that the DI framework can be specific about which class it needs to instantiate
	public class AdoDbSourceOptions<T> : AdoDbSourceOptions
		where T : DbSource
	{ }
}
