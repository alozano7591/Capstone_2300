using CircuitShare.Entities;
using System.ComponentModel.DataAnnotations;

namespace CircuitShare.Models
{
	public class UserAddressViewModel
	{

		public User? ActiveUser { get; set; }
		public string? Username { get; set; }

		//Billing Stuff

		[Required(ErrorMessage = "Please enter your first name.")]
		public string? FirstName { get; set; }
		[Required(ErrorMessage = "Please enter your last name.")]
		public string? LastName { get; set; }
		[RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Please insert the phone number in the format 111-222-3333")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Please enter a street address.")]
		public string? StreetAddress { get; set; }
		[Required(ErrorMessage = "Please enter a city.")]
		public string? City { get; set; }
		[Required(ErrorMessage = "Please enter a region.")]
		public string? Region { get; set; }
		[RegularExpression(@"[0-9]{5}|[a-zA-Z][0-9][a-zA-Z][0-9][a-zA-Z][0-9]|[a-zA-Z][0-9][a-zA-Z][-][0-9][a-zA-Z][0-9]$", ErrorMessage = "Please insert a valid zip/postal code")]
		[Required(ErrorMessage = "Please enter a zip/postal code.")]
		public string? ZipOrPostalCode { get; set; }
		[Required(ErrorMessage = "Please enter a country.")]
		public string? Country { get; set; }

		public bool ShippingSameAsBilling { get; set; } = false;


		public bool ShowSaveMessage { get; set; } = false;
		public string? SaveMessage { get; set; } = "Address has been successfully save!";
	}
}
