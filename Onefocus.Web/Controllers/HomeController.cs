using Microsoft.AspNetCore.Mvc;
using Onefocus.Common.Exceptions.Errors;

namespace Onefocus.Web.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        try
        {
            string scriptPath = System.IO.Path.Combine("Client/dist/assets/");
            string scriptWildcardPattern = "index-*.js";
            ViewBag.ScriptFiles = Directory.GetFiles(scriptPath, scriptWildcardPattern);

            string styleWildcardPattern = "index-*.css";
            ViewBag.StyleFiles = Directory.GetFiles(scriptPath, styleWildcardPattern);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonErrors.InternalErrorMessage);
        }

        return View();
    }
}
