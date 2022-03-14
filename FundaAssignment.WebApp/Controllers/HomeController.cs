using Microsoft.AspNetCore.Mvc;

namespace FundaAssignment.WebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
