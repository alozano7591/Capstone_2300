using System.ComponentModel.DataAnnotations;

namespace CircuitShare.Entities
{
	public class Charger
	{
		public int ChargerId { get; set; }

		[Required(ErrorMessage = "Please enter the name of your charger.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter a description for the charger.")]
		[StringLength(300, MinimumLength = 10, ErrorMessage = "Please enter a description with at number of characters between 10 and 300")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Please enter an hourly rate.")]
		[DataType(DataType.Currency)]
		public double HourlyRate { get; set; } = 0;

		/// <summary>
		/// User indicates who manages the charger, and not necessarily the owner. 
		/// The website could potentially be the user for the charger if it is simply a public charger that the site adds on its own.
		/// </summary>
		public User? User { get; set; }

		public string? UserId {  get; set; } 
	}
}
