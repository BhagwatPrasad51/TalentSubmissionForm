using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using TalentSubmissionForm.Models;


namespace TalentSubmissionForm.Data
{
    public class TalentContext:DbContext
    {
        public TalentContext(DbContextOptions<TalentContext> options) : base(options) {
        }   
        public DbSet<TalentUser>talentUsers { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<UploadedFile> uploadedFiles { get; set; }
        public DbSet<Error> ErrorLogs { get; set; }
    }
}
