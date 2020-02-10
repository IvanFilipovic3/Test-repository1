using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGw_Base.Services
{
	public interface IDiagnostic
	{
		void PerformHealthChecks();
	}
}
