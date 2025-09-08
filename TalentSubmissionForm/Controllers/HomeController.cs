using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TalentSubmissionForm.Data;
using TalentSubmissionForm.Models;

namespace TalentSubmissionForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TalentContext _context;
        public HomeController(ILogger<HomeController> logger, TalentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string ErrorMessage, string Path, string FullErrorMessage)
        {
            if (ErrorMessage != null)
            {
                var error = string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : "Error";
                var path = string.IsNullOrEmpty(Path) ? Path : "/Error";

                _logger.LogError(error, "Unhandled exception at path: {Path}", path);

                _context.ErrorLogs.Add(new Error
                {
                    Message = ErrorMessage,
                    Path = path,
                    ErrorDetails = FullErrorMessage ?? "",
                    Createdon = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")
                });
                _context.SaveChanges();

                // Pass to view
                ViewBag.ErrorMessage = ErrorMessage;
                ViewBag.Path = path;
            }

           return View(model: ErrorMessage);
        }
    }
}
