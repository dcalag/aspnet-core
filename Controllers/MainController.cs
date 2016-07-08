using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DcaLag.AspNet.Controllers
{
    [Authorize]
    public class MainController : Controller
    {   
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CustomSchemeDcaLag_AspNet");
            return View("Login");
        }

        public IActionResult Inicio()
        {
            return View();
        }

        public IActionResult AcercaDe()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
