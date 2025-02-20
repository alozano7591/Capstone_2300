using CircuitShare.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CircuitShare.Models
{
	public class UserChargerViewModel
	{
		
		public Charger? ActiveCharger { get; set; }

		public string? UserId { get; set; }

	}
}
