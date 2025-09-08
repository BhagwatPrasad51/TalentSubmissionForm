using System.ComponentModel.DataAnnotations;

namespace TalentSubmissionForm.Models
{
    public class Error
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string ErrorDetails { get; set; }
        public string? Createdon { get; set; }
        public string? Deletedon { get; set; }
    }
}
