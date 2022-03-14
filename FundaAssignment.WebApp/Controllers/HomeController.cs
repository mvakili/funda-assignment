using FundaAssignment.WebApp.Services.Contracts;
using FundaAssignment.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FundaAssignment.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IMakelaarsService makelaarsService;

    public HomeController(IMakelaarsService makelaarsService)
    {
        this.makelaarsService = makelaarsService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var topMakelaars = makelaarsService.GetMakelaarsWithMostObjectsInAmsterdam(10);
        var topMakelaarsWithTuin = makelaarsService.GetMakelaarsWithMostObjectsWithTuinInAmsterdam(10);

        var model = new HomeViewModel
        {
            TopAmsterdamMakerlaars = topMakelaars.Select(s => ToViewModel(s)),
            TopAmsterdamMakerlaarsWithTuin = topMakelaarsWithTuin.Select(s => ToViewModel(s))
        };
        return View(model);
    }

    private static MakelaarViewModel ToViewModel(MakelaarDto s)
    {
        return new MakelaarViewModel { ItemsCount = s.ItemsCount, MakelaarId = s.MakelaarId, MakelaarNaam = s.MakelaarNaam };
    }
}
