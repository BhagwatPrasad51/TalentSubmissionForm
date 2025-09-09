using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using TalentSubmissionForm.Data;
using TalentSubmissionForm.Helper;
using TalentSubmissionForm.Models;

namespace TalentSubmissionForm
{
    public class TalentService
    {
        private readonly TalentContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TalentService(TalentContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int RegisterDetails(RegisterViewModel register)
        {
            try
            {
                Admin admin = new Admin();
                admin.UserName = register.UserName;
                admin.Email = register.Email;
                admin.Password = register.Password;
                admin.Createdon = Convert.ToString(DateTime.Now);
                _context.admins.Add(admin);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public bool UpdateRegisterDetails(ForgetViewModel register)
        {
            try
            {
                var admin = _context.admins.FirstOrDefault(a => a.Email == register.Email);
                if (admin == null)
                {
                    return false;
                }

                admin.Email = register.Email;
                if (!string.IsNullOrEmpty(register.Password))
                {
                    admin.Password = register.Password;
                }

                admin.Createdon = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                _context.admins.Update(admin);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Admin CheckLogin(LoginViewModel login)
        {
            try
            {
                var anyUsers = _context.admins.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();

                if (anyUsers != null)
                    return anyUsers;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public int GetTalentUserId()
        {
            int n = _context.talentUsers.Any() ? _context.talentUsers.Max(u => u.Id) : 0;
            return n;
        }
        public IEnumerable<TalentUser> GetTalentUserDetails()
        {
            bool anyUsers = _context.talentUsers.Any();

            if (!anyUsers)
                return null;

            var talentuser = _context.talentUsers.Where(x => x.IsDeleted == false).ToList();
            return talentuser;
        }
        public int SaveDetails(TalentUser talentUser)
        {
            try
            {
                _context.talentUsers.Add(talentUser);
                _context.SaveChanges();
                return talentUser.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public int SaveFileDetails(List<string> images, int TalentID)
        {
            try
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");


                foreach (var fileName in images)
                {
                    var fullPath = Path.Combine(uploadFolder, fileName);
                    if (!System.IO.File.Exists(fullPath))
                        continue;

                    var fileInfo = new FileInfo(fullPath);
                    var mimeType = GetMimeType(fullPath); // use MIME mapping

                    UploadedFile uploadedFile = new UploadedFile
                    {
                        FileName = fileName,
                        FilePath = fullPath, // relative path
                        Size = Convert.ToString(fileInfo.Length),
                        Extension = fileInfo.Extension,
                        MimeType = mimeType,
                        TalentId = TalentID,
                        Createdon = Convert.ToString(DateTime.Now)
                    };
                    _context.uploadedFiles.Add(uploadedFile);
                    _context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        private string GetMimeType(string filePath)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        public TalentUserCreate GetDetailsByUserId(string EncodeId)
        {
            int DecodeId = DecodeTalentID(EncodeId);
            var talentuser = _context.talentUsers.Where(x => x.Id == DecodeId).FirstOrDefault();
            TalentUserCreate talentUserCreate = new TalentUserCreate();
            talentUserCreate.Id = talentuser.Id;
            talentUserCreate.firstName = talentuser.FirstName;
            talentUserCreate.lastName = talentuser.LastName;

            talentUserCreate.dob = talentuser.DOB;
            talentUserCreate.city = talentuser.City;
            talentUserCreate.gender = talentuser.Gender;
            talentUserCreate.email = talentuser.Email;
            talentUserCreate.phone = talentuser.Phone;
            talentUserCreate.Height = talentuser.Hieght;
            talentUserCreate.socials = talentuser.SocialInfo;
            talentUserCreate.otherInfo = talentuser.OtherInfo;
            var parsedStatus = EnumHelper.FromDisplayName<ApplicationStatus>(talentuser.Status);
            if (parsedStatus != null)
            {
                talentUserCreate.Status = parsedStatus.Value;
            }
            talentUserCreate.Note = talentuser.Note;
            talentUserCreate.interests = new List<string>();
            string[] interest = talentuser.Interest.Split(',');
            for (int i = 0; i < interest.Length; i++)
            {
                talentUserCreate.interests.Add(interest[i]);
            }

            talentUserCreate.images = new List<string>();
            List<UploadedFile> uploadFiles = GetImages(talentuser.Id);
            foreach (var image in uploadFiles)
            {
                talentUserCreate.images.Add(image.FileName);
            }
            
            return talentUserCreate;
        }
        

        public List<UploadedFile> GetImages(int TalentId)
        {
            var uploadfiles = _context.uploadedFiles.Where(x => x.TalentId == TalentId && x.Isdeleted != true).ToList();
            return uploadfiles;
        }

        public int UpdateDetails(TalentUser talentUser)
        {
            try
            {
                // _context.talentUsers.Where(x => x.Id == talentUser.Id).FirstOrDefault();
                var userName = string.IsNullOrEmpty(Convert.ToString(_httpContextAccessor.HttpContext.User.Identity.Name)) ? "" : Convert.ToString(_httpContextAccessor.HttpContext.User.Identity.Name);
                talentUser.Updatedby = userName;
                talentUser.Updatedon = Convert.ToString(DateTime.Now);
                _context.Update(talentUser);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public int UpdateUploadFiles(List<string> images, int TalentID)
        {
            try
            {
                var existingUploadfiles = _context.uploadedFiles
                    .Where(x => x.TalentId == TalentID)
                    .ToList();

                var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "";
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                foreach (var file in existingUploadfiles)
                {
                    file.Isdeleted = true;
                }

                foreach (var fileName in images)
                {
                    var fullPath = Path.Combine(uploadFolder, fileName);

                    if (!System.IO.File.Exists(fullPath))
                        continue;

                    var fileInfo = new FileInfo(fullPath);
                    var mimeType = GetMimeType(fullPath);
                    var existingFile = existingUploadfiles.FirstOrDefault(x => x.FileName == fileName);

                    if (existingFile != null)
                    {
                        existingFile.FilePath = fullPath;
                        existingFile.Size = fileInfo.Length.ToString();
                        existingFile.Extension = fileInfo.Extension;
                        existingFile.MimeType = mimeType;
                        existingFile.Updatedby = userName;
                        existingFile.Updatedon = DateTime.Now.ToString();
                        existingFile.Isdeleted = false;
                        _context.uploadedFiles.Update(existingFile);
                    }
                    else
                    {
                        var newFile = new UploadedFile
                        {
                            FileName = fileName,
                            FilePath = fullPath,
                            Size = fileInfo.Length.ToString(),
                            Extension = fileInfo.Extension,
                            MimeType = mimeType,
                            TalentId = TalentID,
                            Createdby = userName,
                            Createdon = DateTime.Now.ToString(),
                            Isdeleted = false
                        };
                        _context.uploadedFiles.Add(newFile);
                    }
                }
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private int DecodeTalentID(string encodedTalentID)
        {
            if (string.IsNullOrEmpty(encodedTalentID))
                throw new ArgumentException("ID cannot be null or empty.");

            try
            {
                var cleanInput = encodedTalentID.Trim().Replace(' ', '+');
                byte[] decodedBytes = Convert.FromBase64String(cleanInput);
                return BitConverter.ToInt32(decodedBytes, 0);
            }
            catch (FormatException)
            {
                throw new ArgumentException("The input is not a valid ID.");
            }
        }

    }
}
