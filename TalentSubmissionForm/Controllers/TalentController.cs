using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Newtonsoft.Json;
using OfficeOpenXml;
using StackExchange.Redis;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using TalentSubmissionForm.Helper;
using TalentSubmissionForm.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TalentSubmissionForm.Validation;

namespace TalentSubmissionForm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TalentController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly TalentService _talentService;
        public TalentController(IWebHostEnvironment env, TalentService talentService)
        {
            _env = env;
            _talentService = talentService;
        }

        // Create User
        [AllowAnonymous]
        public IActionResult Create()
        {
            try
            {
                ViewData["FormAction"] = "Create";
                ViewData["FormController"] = "Talent";
                var model = new TalentUserCreate
                {
                    images = new List<string>(),
                    interests = new List<string>(),
                    dob = Convert.ToString(DateTime.Now.AddYears(-18).ToString("dd-MM-yyyy"))
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Create Form.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([Bind("firstName,lastName,dob,city,gender,email,phone,interests,socials,otherInfo,Height,images,Status,Note")] TalentUserCreate talentUserCreate)
        {
            try
            {
                ViewData["FormAction"] = "Create";
                ViewData["FormController"] = "Talent";
                if (ModelState.IsValid)
                {
                    TalentUser talentUser = new TalentUser();
                    talentUser.FirstName = talentUserCreate.firstName;
                    talentUser.LastName = talentUserCreate.lastName;
                    talentUser.DOB = talentUserCreate.dob;
                    talentUser.City = talentUserCreate.city;
                    talentUser.Gender = talentUserCreate.gender;
                    talentUser.Email = talentUserCreate.email;
                    talentUser.Phone = talentUserCreate.phone;
                    talentUser.SocialInfo = talentUserCreate.socials;
                    talentUser.OtherInfo = talentUserCreate.otherInfo;
                    talentUser.Hieght = talentUserCreate.Height;
                    foreach (var res in talentUserCreate.interests)
                    {
                        talentUser.Interest += res + ",";
                    }
                    talentUser.Interest.TrimEnd(',');
                    int Id = _talentService.SaveDetails(talentUser);
                    if (Id > 0)
                    {
                        int j = _talentService.SaveFileDetails(talentUserCreate.images, Id);
                    }
                    TempData["IsSuccess"] = true;
                    TempData["SuccessMessage"] = "“Your form has been successfully submitted., someone will be in touch”";
                    return View("Success");
                }

                return View(talentUserCreate);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Form Submission.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            try
            {
                var TalentID = HttpContext.Session.GetInt32("TalentId");
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" };
                var allowedMimeTypes = new[] { "image/png", "image/jpeg", "image/webp" };
                int maxFileSize = 1024 * 1024 * 10; // 10 MB

                if (files == null || files.Count == 0)
                {
                    return BadRequest("No files uploaded.");
                }

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        return BadRequest("One of the files is empty.");
                    }

                    var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExt))
                    {
                        return BadRequest("File extension is not allowed.");
                    }

                    if (file.Length > maxFileSize)
                    {
                        return BadRequest("File size exceeds the limit.");
                    }

                    // Check MIME type (based on header)
                    if (!allowedMimeTypes.Contains(file.ContentType))
                    {
                        return BadRequest("File MIME type is not allowed.");
                    }

                    // Read and verify magic numbers
                    using (var reader = new BinaryReader(file.OpenReadStream()))
                    {
                        var headerBytes = reader.ReadBytes(12);

                        if (!FileValidation.IsValidImageByMagicNumber(headerBytes, fileExt))
                        {
                            return BadRequest("File content does not match its extension or Content of file is changed.");
                        }
                    }

                    var guid = Guid.NewGuid();
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string newFileName = $"{guid}{fileExt}";
                    string filePath = Path.Combine(uploadsFolder, newFileName);

                    if (!filePath.StartsWith(uploadsFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        return BadRequest("Invalid file path.");
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileUrl = Url.Content($"~/uploads/{newFileName}");

                    return Json(new { fileurl = fileUrl, filename = newFileName });
                }

                return Ok("File(s) uploaded successfully.");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during File Upload.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        //Remove file
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RemoveFile([FromForm] string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    return BadRequest("Filename must be provided.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var filePath = Path.Combine(uploadsFolder, filename);

                if (!System.IO.File.Exists(filePath))
                    return NotFound("File not found.");

                try
                {
                    System.IO.File.Delete(filePath);
                    return Ok("File removed successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting file: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Removing file.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        // Reset data
        [AllowAnonymous]
        public IActionResult Reset()
        {
            int TalentID = 1;
            HttpContext.Session.SetInt32("TalentId", TalentID);
            return RedirectToAction("Create");
        }
        
    }
}
