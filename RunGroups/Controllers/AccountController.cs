using Microsoft.AspNetCore.Mvc;

namespace RunGroups.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
