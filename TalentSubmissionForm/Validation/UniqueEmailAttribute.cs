using System.ComponentModel.DataAnnotations;
using TalentSubmissionForm.Data;

namespace TalentSubmissionForm.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var dbContext = validationContext.GetService<TalentContext>();

            // Get the model instance (e.g., TalentUserCreate)
            var model = validationContext.ObjectInstance;

            // Use reflection to get Id property (so it works even if model is not strongly typed)
            var idProperty = model.GetType().GetProperty("Id");
            int userId = 0;

            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                userId = (int)idProperty.GetValue(model);
            }

            // Now, check if email exists for another user
            bool emailExists = dbContext.talentUsers
                .Any(u => u.Email == email && u.Id != userId); // Ignore self during update

            if (emailExists)
            {
                return new ValidationResult("This email is already registered.");
            }

            return ValidationResult.Success;
        }
    }
    public class UniqueUserEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var dbContext = validationContext.GetService<TalentContext>();

            // Get the model instance (e.g., TalentUserCreate)
            var model = validationContext.ObjectInstance;

           
            // Now, check if email exists for another user
            bool emailExists = dbContext.admins
                .Any(u => u.Email == email); // Ignore self during update

            if (emailExists)
            {
                return new ValidationResult("This user is already registered.");
            }

            return ValidationResult.Success;
        }
    }
}
