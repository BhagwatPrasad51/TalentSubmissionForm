using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TalentSubmissionForm.Validation
{
    public class MinValidation:ValidationAttribute
    {
        private readonly int _minElements;

        public MinValidation(int minElements)
        {
            _minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList<string>;

            if (list == null || list.Count < _minElements)
            {
                return new ValidationResult(ErrorMessage ?? $"Please select at least {_minElements} item(s).");
            }

            // Optional: You can check if the items are non-empty strings
            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    return new ValidationResult(ErrorMessage ?? $"Selected items cannot be empty.");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
            ErrorMessage = $"You must be at least {_minimumAge} years old.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date of Birth is required");

            if (value is string stringDate)
            {
                if (DateTime.TryParseExact(stringDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                {
                    var today = DateTime.Today;
                    var age = today.Year - dob.Year;
                    if (dob > today.AddYears(-age)) age--;

                    if (age >= _minimumAge)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(ErrorMessage);
                }

                return new ValidationResult("Invalid date format. Use dd-MM-yyyy.");
            }

            return new ValidationResult("Invalid input.");
        }
    }
}
