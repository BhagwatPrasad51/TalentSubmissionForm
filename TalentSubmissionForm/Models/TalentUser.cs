using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentSubmissionForm.Models
{
    public class TalentUser
    {
        [Key]
        public int Id{get; set; }
        public string FirstName{get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? SocialInfo { get; set; }
        public string? OtherInfo { get; set; }

        public string Interest {  get; set; }
        public string? Createdby { get; set; }
        public string? Createdon { get; set; }
        public string? Updatedby { get; set; }
        public string? Updatedon { get; set; }
        public bool IsDeleted { get; set; }
        public string? Deletedon { get; set; }
        public string Hieght { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; } 
        [NotMapped]
        public string EncodeID
        {
            get
            {
                return Convert.ToBase64String(BitConverter.GetBytes(Id));
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Id = BitConverter.ToInt32(Convert.FromBase64String(value), 0);
                }
            }
        }
    }
}
