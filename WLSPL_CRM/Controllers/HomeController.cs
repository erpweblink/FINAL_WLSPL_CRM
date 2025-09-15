using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WLSPL_CRM.Models;

namespace WLSPL_CRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Search(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                string[] values =  query.Split(',');

            }
            return Json("Hello");
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
