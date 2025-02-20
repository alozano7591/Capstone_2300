using CircuitShare.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CircuitShare.Controllers
{
	public class UsersController : Controller
	{

		private CircuitShareDbContext _circuitShareDbContext;
		private UserManager<User> _userManager;     //used for finding user. Easier to find users without sending username through views

		public UsersController(CircuitShareDbContext circuitShareDbContext, UserManager<User> userManager)
		{
			_circuitShareDbContext = circuitShareDbContext;
			_userManager = userManager;
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}
