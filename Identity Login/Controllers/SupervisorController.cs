using Identity_Login.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Login.Controllers
{
    [Authorize(Roles = RolesSD.SUPERVISOR)]
    public class SupervisorController : Controller
    {
        public IActionResult Index()
        {
            //return View();
            return RedirectToAction("Index", "QrScanner");

        }
    }
}
