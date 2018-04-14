using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcHybridClient.Models;

namespace MvcHybridClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Windows or local => claim http://schemas.microsoft.com/identity/claims/identityprovider
            //var claimIdentityprovider = User.Claims.FirstOrDefault(t => t.Type == "http://schemas.microsoft.com/identity/claims/identityprovider");

            //if (claimIdentityprovider != null && claimIdentityprovider.Value == "Windows")
            //{
            //    // Admin stuff allowed
            //}

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
