using Microsoft.AspNetCore.Mvc;

namespace ATeam_MVC_WebApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}