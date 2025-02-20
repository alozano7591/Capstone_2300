namespace CircuitShare.Entities
{
	public class BillingAddress
	{
		public int BillingAddressId { get; set; }

		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }

		//Billing Address
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
		public string? ZipOrPostalCode { get; set; }
		public string? Country { get; set; }

		public bool ShippingSameAsBilling { get; set; } = false;
	}
}
