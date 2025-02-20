using System.ComponentModel.DataAnnotations;
using static CircuitShare.Entities.User;

namespace CircuitShare.Models
{
	public class UserInfoViewModel
	{

		public string? UserName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		[EnumDataType(typeof(Gender))]
		public Gender? GenderType { get; set; }

		[DateLessThanOrEqualToToday]
		[MinimumAge(18)]
		[Display(Name = "Date of Birth")]
		public DateTime? Birthday { get; set; } = DateTime.Today;

	}
}

public class DateLessThanOrEqualToToday : ValidationAttribute
{
	public override string FormatErrorMessage(string name)
	{
		return "Invalid date of birth, please enter a date in the past.";
	}

	protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
	{
		var dateValue = objValue as DateTime? ?? new DateTime();

		if (dateValue.Date > DateTime.Now.Date)
		{
			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
		}
		return ValidationResult.Success;
	}
}


public class MinimumAge : ValidationAttribute
{
	int _minimumAge = 5;

	public MinimumAge(int minimumAge)
	{
		_minimumAge = minimumAge;
	}

	public override string FormatErrorMessage(string name)
	{
		return string.Format("You need to be at least {0} years old to use this service.", _minimumAge);
	}

	protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
	{
		var dateOfBirth = objValue as DateTime? ?? new DateTime();

		int age = DateTime.Today.Year - dateOfBirth.Year;

		if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
		{
			age--;
		}

		if (age < _minimumAge)
		{
			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
		}
		return ValidationResult.Success;
	}
}