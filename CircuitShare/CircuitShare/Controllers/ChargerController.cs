using CircuitShare.Entities;
using CircuitShare.Models;
using CircuitShare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CircuitShare.Controllers
{
    public class ChargerController : Controller
	{
		private CircuitShareDbContext _circuitShareDbContext { get; set; }
        private ChargerManager _chargerManager;
		private UserManager<User> _userManager;

		public ChargerController(CircuitShareDbContext circuitShareDbContext, ChargerManager chargerManager, UserManager<User> userManager)
		{
			_circuitShareDbContext = circuitShareDbContext;
            _chargerManager = chargerManager;
            _userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("/Chargers")]
		public IActionResult GetAllChargers()
		{
			var allChargers = _chargerManager.GetChargers();

			ChargerViewModel chargerViewModel = new ChargerViewModel()
			{
				Chargers = allChargers,
			};

			return View("Chargers", chargerViewModel);
		}

        // GET: Chargers/GetChargerAsync/5
        [Route("Charger/GetChargerAsync/{id}")]
        public async Task<IActionResult> GetChargerAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charger = await _circuitShareDbContext.Chargers
                .FirstOrDefaultAsync(c => c.ChargerId == id);

            if (charger == null)
            {
                return NotFound();
            }

            await _circuitShareDbContext.SaveChangesAsync();

            return View("ChargerDetails", charger);
        }

        /// <summary>
        /// Creates a new add request
        /// </summary>
        /// <returns> A new add form using with a new chargerviewmodel</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("/Chargers/add-request")]
        public IActionResult GetAddChargerRequest()
        {
			Charger chargerViewModel = new Charger()
			{
				HourlyRate = 0
			};

            return View("AddCharger", chargerViewModel);
        }

        [HttpPost("/Chargers")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCharger(Charger charger)
        {
            if (ModelState.IsValid)
            {
                _chargerManager.AddNewCharger(charger);
                await _circuitShareDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllChargers));
            }

            return View("AddCharger", charger);
        }

        // GET: Chargers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCharger(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charger = await _circuitShareDbContext.Chargers.FindAsync(id);

            if (charger == null)
            {
                return NotFound();
            }
            return View(charger);
        }

        // POST: Chargers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCharger(int id, [Bind("ChargerId,Name,Description,HourlyRate")] Charger charger)
        {
            if (id != charger.ChargerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _circuitShareDbContext.Update(charger);
                    await _circuitShareDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChargerExists(charger.ChargerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("GetAdminPanel", "Account");
            }
            return View(charger);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCharger(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charger = await _circuitShareDbContext.Chargers
                .FirstOrDefaultAsync(c => c.ChargerId == id);
            if (charger == null)
            {
                return NotFound();
            }

            return View(charger);
        }

        [HttpPost, ActionName("DeleteCharger")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChargerConfirmed(int id)
        {
            var charger = await _circuitShareDbContext.Chargers.FindAsync(id);
            if (charger != null)
            {
                _circuitShareDbContext.Chargers.Remove(charger);
            }

            await _circuitShareDbContext.SaveChangesAsync();
            return RedirectToAction("GetAdminPanel", "Account");
        }

        private bool ChargerExists(int id)
        {
            return _circuitShareDbContext.Chargers.Any(e => e.ChargerId == id);
        }

		[Authorize]
		[HttpGet("/{username}/MyChargers")]
		public async Task<IActionResult> GetMyChargersByUsername(string username)
		{
            User? user = _circuitShareDbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
				return NotFound();
			}
			
            var userChargers = _chargerManager.GetChargersByUserId(user.Id);

			ChargerViewModel chargerViewModel = new ChargerViewModel()
			{
				Chargers = userChargers,
			};

			return View("MyChargers", chargerViewModel);
		}

		[Authorize]
		[HttpGet("/{username}/MyChargers/add-charger")]
		public async Task<IActionResult> GetUserAddChargerRequest(string username)
		{
			User? user = _circuitShareDbContext.Users.FirstOrDefault(u => u.UserName == username);

			if (user == null)
			{
				return NotFound(String.Format("User: {0} Not Found", username));
			}

			UserChargerViewModel chargerViewModel = new UserChargerViewModel()
			{
                ActiveCharger = new Charger(),
				UserId = user.Id,
			};

			return View("UserAddCharger", chargerViewModel);
		}

		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UserAddNewCharger(UserChargerViewModel model, string username)
		{
			if (ModelState.IsValid)
			{
				User? user = _circuitShareDbContext.Users.FirstOrDefault(u => u.UserName == username);

				if (user == null)
				{
					return NotFound(String.Format("User: {0} Not Found", username));
				}

                Charger newCharger = model.ActiveCharger;
				newCharger.User = user;
                newCharger.UserId = user.Id;

				_chargerManager.AddNewCharger(newCharger);
				await _circuitShareDbContext.SaveChangesAsync();
                //return RedirectToAction(nameof(GetMyChargersByUsername));

                return RedirectToAction("GetMyChargersByUsername", "Charger", new { username = user.UserName });
			}

			return View("UserAddCharger", model);
		}

        [Authorize]
		[HttpGet("/{username}/MyChargers/{chargerId}/edit-charger")]
		public async Task<IActionResult> GetUserEditChargerRequest(string username, int chargerId)
		{
			User? user = _circuitShareDbContext.Users.FirstOrDefault(u => u.UserName == username);

			if (user == null)
			{
				return NotFound(String.Format("User: {0} Not Found", username));
			}

            Charger? charger = _chargerManager.GetChargerById(chargerId);

            if (charger == null)
            {
				return NotFound(String.Format("Charger Id: {0} Not Found", chargerId));
			}

			UserChargerViewModel chargerViewModel = new UserChargerViewModel()
			{
				ActiveCharger = charger,
				UserId = user.Id,
			};

			return View("UserEditCharger", chargerViewModel);
		}

		// POST: Chargers/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UserEditCharger(UserChargerViewModel model, string username)
		{
			if (ModelState.IsValid)
			{
                // including user again since this seems to "forget" the user for some reason
				User? user = _circuitShareDbContext.Users.FirstOrDefault(u => u.UserName == username);

				if (user == null)
				{
					return NotFound(String.Format("User: {0} Not Found", username));
				}

				model.ActiveCharger.User = user;
				model.ActiveCharger.UserId = user.Id;

				try
				{
					_circuitShareDbContext.Chargers.Update(model.ActiveCharger);
					await _circuitShareDbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ChargerExists(model.ActiveCharger.ChargerId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return RedirectToAction("GetMyChargersByUsername", "Charger", new { username = user.UserName });
			}

			return View("UserAddCharger", model);
		}

		[Authorize]
		public async Task<IActionResult> UserDeleteCharger(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}

			// find user data since we will take user to their charger page later
			User? user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound("User null error.");
			}

			var charger = await _circuitShareDbContext.Chargers
				.FirstOrDefaultAsync(c => c.ChargerId == id);
			if (charger == null)
			{
				return NotFound("Charger not found.");
			}

			_circuitShareDbContext.Chargers.Remove(charger);
			_circuitShareDbContext.SaveChanges();

			return RedirectToAction("GetMyChargersByUsername", new {username = user.UserName});
		}
	}
}
