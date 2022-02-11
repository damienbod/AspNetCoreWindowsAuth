using System;
using System.Diagnostics;
using System.Linq;
using AppAuthorizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcHybridClient.Models;

namespace MvcHybridClient.Controllers;

[Authorize]
public class HomeController : Controller
{
    private IAppAuthorizationService _appAuthorizationService;

    public HomeController(IAppAuthorizationService appAuthorizationService)
    {
        _appAuthorizationService = appAuthorizationService;
    }

    public IActionResult Index()
    {
        //Windows or local => claim http://schemas.microsoft.com/identity/claims/identityprovider
        var claimIdentityprovider = User.Claims.FirstOrDefault(t => t.Type == "http://schemas.microsoft.com/identity/claims/identityprovider");

        if (claimIdentityprovider != null && _appAuthorizationService.IsAdmin(User.Identity.Name, claimIdentityprovider.Value))
        {
            // yes, this is an admin
            Console.WriteLine("This is an admin, we can do some specific admin logic!");
        }

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

    public IActionResult Logout()
    {
        return new SignOutResult(new[] { "Cookies", "OpenIdConnect" });
    }
}