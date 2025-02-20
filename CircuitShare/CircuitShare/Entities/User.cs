using Microsoft.AspNetCore.Identity;

namespace CircuitShare.Entities
{
	public class User : IdentityUser
	{
		// User ID, Email, and Phone inherited by Identity User

		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public DateTime? Birthday { get; set; }

		public enum Gender
		{
			Male = 1,
			Female = 2,
			Other = 3

		}

		public Gender? GenderType { get; set; }

		public BillingAddress BillingAddress { get; set; } = new BillingAddress();          //Billing Address

		public List<Charger> Chargers { get; set; } = new List<Charger>();
	}
}
