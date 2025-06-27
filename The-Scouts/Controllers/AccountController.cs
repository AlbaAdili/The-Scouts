using Microsoft.AspNetCore.Mvc;

namespace The_Scouts.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View(); // Renders Views/Account/Login.cshtml
        }

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View(); // Renders Views/Account/Register.cshtml
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            return View(); // Optional, if you have a logout page
        }
    }
}