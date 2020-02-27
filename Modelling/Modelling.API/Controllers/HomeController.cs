using Microsoft.AspNetCore.Mvc;

namespace Prophet.SaaS.Modelling.API.Controllers
{
	public class HomeController : Controller
	{
		// GET: /<controller>/<action>
		public IActionResult Index() => new RedirectResult("~/swagger");
	}
}
