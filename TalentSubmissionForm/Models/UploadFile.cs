using System.ComponentModel.DataAnnotations;

namespace TalentSubmissionForm.Models
{
    public class UploadedFile
    {
        [Key]
        public int Id { get; set; }

        public string Size { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public string? Createdby { get; set; }
        public string? Createdon { get; set; }
        public string? Updatedby { get; set; }
        public string? Updatedon { get; set; }
        public int? TalentId { get; set; }
        public bool Isdeleted { get; set; }
    }
}
