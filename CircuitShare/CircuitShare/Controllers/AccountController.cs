using CircuitShare.Entities;
using CircuitShare.Models;
using CircuitShare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CircuitShare.Controllers
{
    public class AccountController : Controller
	{

		// Using old email address from previous project until an alternative is found
		string fromAddress = "steamkillerdb@gmail.com";

		private readonly IConfiguration _configuration;
		private UserManager<User> _userManager;
		private SignInManager<User> _signInManager;
		private CircuitShareDbContext _circuitShareDbContext;
		private ChargerManager _chargerManager;

		public AccountController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, CircuitShareDbContext circuitShareDbContext, ChargerManager chargerManager)
		{
			_configuration = configuration;
			_userManager = userManager;
			_signInManager = signInManager;
			_circuitShareDbContext = circuitShareDbContext;
			_chargerManager = chargerManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("/admin")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetAdminPanel()
		{
            var allChargers = _chargerManager.GetChargers();

            ChargerViewModel chargerViewModel = new ChargerViewModel()
            {
                Chargers = allChargers,
            };

            return View("~/Views/Admin/AdminPanel.cshtml", chargerViewModel);

		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}


			var user = new User { UserName = model.Username, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);

			// Error checkpoint
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);

			// Generate token to confirm account in database.
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

			if (user != null && user.Email != null && confirmationLink != null)
			{
				//SendEmailActivationToken(user.Email, user, confirmationLink);		// for now not sending emails
				return RedirectToAction(nameof(SuccessRegistration), new { confirmationLink });
			}
			else
			{
				ModelState.AddModelError("", "Unable to send email: Either user, email, or confirmation link is null.");
				return View(model);
			}

		}

		[HttpGet]
		public IActionResult SuccessRegistration(string confirmationLink)
		{
			ViewBag.EmailConfirmationLink = confirmationLink;
			return View();
		}

		[HttpGet]
		public IActionResult LogIn(string returnURL = "")
		{
			var model = new LoginViewModel { ReturnUrl = returnURL };
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> LogIn(LoginViewModel model)
		{

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			//await _userManager.IsEmailConfirmedAsync(User user)

			var result = await _signInManager.PasswordSignInAsync(
				model.Username,
				model.Password,
				isPersistent: model.RememberMe,
				lockoutOnFailure: true
				);

			if (result.Succeeded)
			{
				if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
				{
					return Redirect(model.ReturnUrl);
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}
			}
			else if (result.IsLockedOut)
			{
				//return View("AccountLockedOut");
				ModelState.AddModelError("", "Too many failed attempts, this account has now been locked.");

				return View(model);
			}
			else
			{
				ModelState.AddModelError("", "Invalid login attempt.");
				return View(model);
			}

		}

		[HttpGet]
		public ViewResult AccessDenied()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet("Account/EmailConfirmed")]
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return View("Error");
			var result = await _userManager.ConfirmEmailAsync(user, token);
			return View(result.Succeeded ? "EmailConfirmed" : "Error");
		}

		/// <summary>
		/// Gets the UserProfile page
		/// </summary>
		/// <returns></returns>
		[HttpGet("/user/{username}")]
		[Authorize]
		public IActionResult GetUserProfile(string username)
		{
			return View("UserProfile");
		}

		[HttpGet("/user/{username}/info/edit-request")]
		[Authorize]
		public async Task<IActionResult> GetEditUserInfoByUsername(string username)
		{
			User? user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				return NotFound();
			}

			var viewModel = new UserInfoViewModel
			{
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				GenderType = user.GenderType,
				Birthday = user.Birthday ?? DateTime.Today,
			};

			return View("EditUserInfo", viewModel);
		}

		/// <summary>
		/// Posts the edit changes to the database
		/// </summary>
		/// <param name="username"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("/user/{username}/info/edit-request")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PostEditUserInfoByUsername(string username, UserInfoViewModel model)
		{
			// Find user by username
			User? user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				return NotFound();
			}

			// Check if model is valid
			if (!ModelState.IsValid)
			{
				// Return view with current model to display validation errors
				return View("EditUserInfo", model);
			}

			// Update user properties
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.GenderType = model.GenderType;

			DateTime? postgresDOB = DateTime.SpecifyKind((DateTime)model.Birthday, DateTimeKind.Utc);
			
			user.Birthday = postgresDOB;

			// Save changes to the user profile
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				// Redirect to user profile view
				return RedirectToAction(nameof(GetUserProfile), new { username = user.UserName });
			}

			// Add errors to model state if update fails
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			// If update fails, return view with error messages
			return View("EditUserInfo", model);
		}

		[HttpGet("/user/{username}/address/")]
		[Authorize]
		public async Task<IActionResult> GetEditUserAddressByUsername(string username)
		{
			//User? user = await _userManager.FindByNameAsync(username);

			User? user = _circuitShareDbContext.Users
				.Include(u => u.BillingAddress)
				.FirstOrDefault(u => u.UserName == username);

			if (user == null) return NotFound(string.Format("User: {0}", username));

			var viewModel = new UserAddressViewModel
			{
				ActiveUser = user,
				Username = user.UserName,

				//Billing Props
				FirstName = user.BillingAddress.FirstName,
				LastName = user.BillingAddress.LastName,
				PhoneNumber = user.BillingAddress.PhoneNumber,

				StreetAddress = user.BillingAddress.StreetAddress,
				City = user.BillingAddress.City,
				Region = user.BillingAddress.Region,
				ZipOrPostalCode = user.BillingAddress.ZipOrPostalCode,
				Country = user.BillingAddress.Country,

				ShippingSameAsBilling = user.BillingAddress.ShippingSameAsBilling,

				ShowSaveMessage = false,
			};

			return View("EditUserAddress", viewModel);
		}

		[HttpPost()]
		[Authorize]
		public async Task<IActionResult> ProcessUserAddressEdit(UserAddressViewModel userAddressViewModel, string username)
		{

			//var user = await _userManager.FindByNameAsync(username);

			User? user = _circuitShareDbContext.Users
				.Include(u => u.BillingAddress)
				.FirstOrDefault(u => u.UserName == username);

			if (user == null) return NotFound(string.Format("User: {0}", username));

			if (!ModelState.IsValid)
			{
				// Return view with current model to display validation errors
				return View("EditUserAddress", userAddressViewModel);
			}

			user.BillingAddress.FirstName = userAddressViewModel.FirstName;
			user.BillingAddress.LastName = userAddressViewModel.LastName;
			user.BillingAddress.PhoneNumber = userAddressViewModel.PhoneNumber;

			user.BillingAddress.StreetAddress = userAddressViewModel.StreetAddress;
			user.BillingAddress.City = userAddressViewModel.City;
			user.BillingAddress.Region = userAddressViewModel.Region;
			user.BillingAddress.ZipOrPostalCode = userAddressViewModel.ZipOrPostalCode;
			user.BillingAddress.Country = userAddressViewModel.Country;

			user.BillingAddress.ShippingSameAsBilling = userAddressViewModel.ShippingSameAsBilling;

			_circuitShareDbContext.SaveChanges();

			userAddressViewModel.ShowSaveMessage = true;

			return View("EditUserAddress", userAddressViewModel);
		}

		[HttpGet("/user/{username}/change-password")]
		[Authorize]
		public async Task<IActionResult> GetChangePasswordRequestByUsername()
		{

			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound();
			}

			ChangePasswordViewModel model = new ChangePasswordViewModel();

			return View("ChangePassword");

		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{

			if (!ModelState.IsValid)
			{
				return View("ChangePassword", model);
			}

			User? user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound();
			}

			if (user != null)
			{
				//attempt to change password
				IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);

				if (result.Succeeded)
				{

					return RedirectToAction("GetUserProfile", new { username = user.UserName });

				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}

				//Return to the Change Password View and Show the Error Details
				return View("ChangePassword", model);
			}
			//If the User Not Found, then display HttpNotFound Error
			return NotFound();

		}


		/// <summary>
		/// Send an email
		/// </summary>
		/// <param name="toEmailAddress"></param>
		/// <param name="resetToken"></param>
		void SendEmailPasswordReset(string toEmailAddress, User user, string resetToken)
		{

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromAddress, "qcfieiidlkekmfuc"),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage()
			{
				From = new MailAddress(fromAddress),
				Subject = $"CircuitShare Password Reset For \"{user.UserName}\".",
				Body = $"<h2>Hello {user.UserName},</h2>" +
				$"<p>You have requested a password reset. Use this link to reset your password:" +
				$"<p><a href=\"{resetToken}\">Password Reset Link</a></p>" +
				"<p>Sincerely,</p>" +
				"<p>CircuitShare Team</p>",
				IsBodyHtml = true
			};

			mailMessage.To.Add(toEmailAddress);

			smtpClient.SendAsync(mailMessage, null);
		}

		/// <summary>
		/// Email for sending people their activation links
		/// </summary>
		/// <param name="toEmailAddress"></param>
		/// <param name="user"></param>
		/// <param name="activationToken"></param>
		void SendEmailActivationToken(string toEmailAddress, User user, string activationToken)
		{

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromAddress, "qcfieiidlkekmfuc"),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage()
			{
				From = new MailAddress(fromAddress),
				Subject = $"Welcome To CircuitShare \"{user.UserName}\".",
				Body = $"<h2>Welcome {user.UserName}!</h2>" +
				$"<p>Congrats on your new account! Simply click on the link below for a speedy activation!" +
				$"<p><a href=\"{activationToken}\">Activation Link</a></p>" +
				"<p>Sincerely,</p>" +
				"<p>CircuitShare Team</p>",
				IsBodyHtml = true
			};

			mailMessage.To.Add(toEmailAddress);

			smtpClient.SendAsync(mailMessage, null);
		}
	}
}
