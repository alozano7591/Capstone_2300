using System.ComponentModel.DataAnnotations;

namespace CircuitShare.Models
{
	public class ChangePasswordViewModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string? CurrentPassword { get; set; }

		[Required(ErrorMessage = "Please enter a new password.")]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string? NewPassword { get; set; }

		[Required(ErrorMessage = "Please confirm password.")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password must match.")]
		public string? ConfirmPassword { get; set; }
	}
}
