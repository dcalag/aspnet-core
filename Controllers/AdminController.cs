using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DcaLag.AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DcaLag.AspNet.Controllers
{
    [Authorize (Roles ="ROLE_ADMIN")]
    public class AdminController : Controller
    {
        public IActionResult Usuarios()
        {
            return View();
        }

        public IActionResult Usuario()
        {
            return View();
        }
    }
}
