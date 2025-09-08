using System.ComponentModel.DataAnnotations;
using TalentSubmissionForm.Validation;

namespace TalentSubmissionForm.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [UniqueUserEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must include 1 uppercase, 1 lowercase, 1 number, and 1 special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords not match Please Check.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please fill User Name.")]
        [MinLength(3, ErrorMessage = "Please fill atleast 3 characters in last Name."), MaxLength(60, ErrorMessage = "last name field can't be more than 60 characters.")]
        public string UserName { get; set; }
    }
}
