using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Onefocus.Web.Controllers;

public class WalletController : Controller
{
    private readonly ILogger<WalletController> _logger;

    public WalletController(ILogger<WalletController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
