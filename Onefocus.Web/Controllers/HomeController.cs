using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Onefocus.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Wallet");
    }
}
