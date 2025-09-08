using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSubmissionForm.Validation;

namespace TalentSubmissionForm.Models
{
    public class TalentUserCreate
    {
        public int  Id { get; set; }
        [Required(ErrorMessage ="Please fill first Name.")]
        [MinLength(3, ErrorMessage = "Please fill atleast 3 characters in last Name."), MaxLength(60, ErrorMessage = "last name field can't be more than 60 characters.")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Please fill last Name.")]
        [MinLength(3, ErrorMessage = "Please fill atleast 3 characters in last Name."),MaxLength(60,ErrorMessage = "last name field can't be more than 60 characters.")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Please fill DOB.")]
        [MinimumAge(18, ErrorMessage = "You must be at least 18 years old for apply.")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Please fill City name.")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please select gender.")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Please fill email.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [UniqueEmail]
        public string email { get; set; }
        [Required(ErrorMessage = "Please fill Phone Number.")]
        [RegularExpression(@"^\+\d{1,3}\d{4,14}(?:x.+)?$", ErrorMessage = "Phone number must start with '+' followed by country code and digits.")]
        public string phone { get; set; }
        public string? socials { get; set; }
        public string? otherInfo { get; set; }
        [Required(ErrorMessage = "Please select at least one of Modelling.")]
        [MinValidation(1, ErrorMessage = "Please select at least one interest.")]
        public List<string> interests { get; set; }
        [Required(ErrorMessage = "Please fill Height.")]
        [Range(5, 10.00, ErrorMessage = "Height will be between 5 and 10.00")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Maximum two decimal places allowed")]
        [Column(TypeName = "decimal(5,2)")]
        public string Height { get; set; }

        [Required(ErrorMessage = "Please upload at least 2 image.")]
        [MinValidation(2, ErrorMessage = "Please upload at least 2 image.")]
        public List<string> images { get; set; }
    }
}
