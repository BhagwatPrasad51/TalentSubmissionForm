using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Security.Claims;
using System.Text.Json;
using TalentSubmissionForm.Models;

namespace TalentSubmissionForm.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly TalentService _talentService;
        public AdminController(IWebHostEnvironment env, TalentService talentService)
        {
            _env = env;
            _talentService = talentService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new LoginViewModel()
            {

            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(login);
                }
                Admin admin = _talentService.CheckLogin(login);
                if (admin == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Creadential.");
                    return View(login);
                }
                if (User != null)
                {
                    var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, admin.UserName),
                  new Claim(ClaimTypes.Role, "Admin")
                };

                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync("MyCookieAuth", principal);
                }
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex) {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during login.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        
        public IActionResult Register(RegisterViewModel register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(register);
                }
                int n = _talentService.RegisterDetails(register);
                if (n == 0)
                {
                    return View();
                }
                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Register.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        // Update Password

        [AllowAnonymous]
        public IActionResult Update(string email)
        {
            var model=new ForgetViewModel()
            {
                Email = email
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Update(ForgetViewModel register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(register);
                }

                bool updated = _talentService.UpdateRegisterDetails(register);
                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Admin with the given email not found.");
                    return View(register);
                }

                TempData["Message"] = "Update successful!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during updating the Admin.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        // Get User
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _talentService.GetTalentUserDetails();
                var jsonResult = JsonConvert.SerializeObject(users);
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during GetAllUser.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        // Update User data
       
        [HttpGet]
        public IActionResult Edit(string  Encodeid)
        {
            try
            {
                ViewData["FormAction"] = "Edit";
                ViewData["FormController"] = "Admin";
                var talentuserCreate = _talentService.GetDetailsByUserId(Encodeid);
                return View(talentuserCreate);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Edit Details.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        [HttpPost]

        public IActionResult Edit([Bind("Id,firstName,lastName,dob,city,gender,email,phone,interests,socials,otherInfo,Height,images")] TalentUserCreate talentUserCreate)
        {
            try
            {
                ViewData["FormAction"] = "Edit";
                ViewData["FormController"] = "Admin";
                if (ModelState.IsValid)
                {
                    TalentUser talentUser = new TalentUser();
                    talentUser.Id = talentUserCreate.Id;
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
                    talentUser.Interest = talentUser.Interest.TrimEnd(',');
                    _talentService.UpdateDetails(talentUser);
                    _talentService.UpdateUploadFiles(talentUserCreate.images, talentUser.Id);
                    return RedirectToAction("Index");
                }

                return View(talentUserCreate);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Edit Details Submission.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }

        //Export To Excel

        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<TalentUser> applicants)
        {
            try
            {
                if (applicants.Count > 0)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Applicants");

                        worksheet.Cell(1, 1).Value = "First Name";
                        worksheet.Cell(1, 2).Value = "Last Name";
                        worksheet.Cell(1, 3).Value = "Date of Birth (yyyy-MM-dd)";
                        worksheet.Cell(1, 4).Value = "City (Location)";
                        worksheet.Cell(1, 5).Value = "Gender";
                        worksheet.Cell(1, 6).Value = "Email";
                        worksheet.Cell(1, 7).Value = "Phone";
                        worksheet.Cell(1, 8).Value = "Height";
                        worksheet.Cell(1, 9).Value = "Interests";
                        worksheet.Cell(1, 10).Value = "Social Information";
                        worksheet.Cell(1, 11).Value = "Other Information";

                        int row = 2;
                        foreach (var app in applicants)
                        {
                            worksheet.Cell(row, 1).Value = app.FirstName;
                            worksheet.Cell(row, 2).Value = app.LastName;
                            worksheet.Cell(row, 3).Value = app.DOB;
                            worksheet.Cell(row, 4).Value = app.City;
                            worksheet.Cell(row, 5).Value = app.Gender;
                            worksheet.Cell(row, 6).Value = app.Email;
                            worksheet.Cell(row, 7).Value = app.Phone;
                            worksheet.Cell(row, 8).Value = app.Hieght;
                            worksheet.Cell(row, 9).Value = app.Interest;
                            worksheet.Cell(row, 10).Value = app.SocialInfo;
                            worksheet.Cell(row, 11).Value = app.OtherInfo;
                            row++;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Applicants.xlsx");
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    ErrorMessage = "An error occurred during Export To Excel.",
                    Path = HttpContext.Request.Path,
                    FullErrorMessage = ex.ToString()
                });
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }
    }
}
