using Identity_Login.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Login.Controllers
{
   
        [Authorize(Roles = RolesSD.ADMIN)]
        public class AdminController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }
        }
    
}
