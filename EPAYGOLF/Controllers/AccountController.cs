using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Repository.Settings;

namespace EPAYGOLF.Controllers
{
    public class AccountController : Controller
    {
        private ISettingsRepository _settingsRepository = new SettingsRepository();
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            ViewBag.Error = "Login Validation Failed...!!!";
            if (Username == null || Password == null)
            {

                return View();
            }

            else
            {

                var loginresult = _settingsRepository.GetUserDetail(Username, Password);
                if (loginresult != null)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, Username),
                    // You can add more claims here if needed
                     };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Allow refreshing the authentication session.
                        AllowRefresh = true,

                        // Expires when the browser is closed.
                        IsPersistent = false,
                        ExpiresUtc = DateTime.Now.AddMinutes(60)
                    };

                    HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity),
                       authProperties);
                    return RedirectToAction("Index", "Report");

                }
                return View();
            }


        }
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");


        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
