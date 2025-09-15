using Microsoft.AspNetCore.Mvc;

namespace WLSPL_CRM_2.Alert
{
    public class AlertController : Controller
    {
        public IActionResult Index()
        {
            var department = HttpContext.Session.GetString("Department");
            ViewBag.Department = TempData["Department"] as string;
            ViewBag.Greeting = TempData["Greeting"] as string;
            ViewBag.userName = TempData["UserName"] as string;
            return View();
        }

        [HttpGet]
        public IActionResult IndexLogin()
        {
            return View();
        }

    }
}
