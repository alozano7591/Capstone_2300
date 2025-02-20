using System.ComponentModel.DataAnnotations;

namespace CircuitShare.Models
{
	public class RegisterViewModel
	{

		[Required(ErrorMessage = "Please enter an email address.")]
		[EmailAddress(ErrorMessage = "Please enter a valid email address.")]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,7})+)$", ErrorMessage = "Please enter a valid email address.")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Please enter a username.")]
		[StringLength(255)]
		public string? Username { get; set; }

		[Required(ErrorMessage = "Please enter a password.")]
		[DataType(DataType.Password)]
		[Compare("ConfirmPassword")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Please confirm your password.")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string? ConfirmPassword { get; set; }

		//public string RecaptchaResponse { get; set; }

	}
}
