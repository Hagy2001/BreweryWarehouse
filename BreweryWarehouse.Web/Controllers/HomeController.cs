using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly BeerStyleMockRepository _beerStyleRepository;

    public HomeController(BeerStyleMockRepository beerStyleRepository)
    {
        _beerStyleRepository = beerStyleRepository;
    }

    public IActionResult Index()
    {
        List<Model.BeerStyle> beerStyles = _beerStyleRepository.GetAll();
        List<Model.Can> cans = beerStyles
            .SelectMany(beerStyle => beerStyle.Cans)
            .ToList();
        List<Model.Keg> kegs = beerStyles
            .SelectMany(beerStyle => beerStyle.Kegs)
            .ToList();

        List<Model.WarehouseLocation> locations = cans
            .SelectMany(can => can.StockEntries)
            .Select(stockEntry => stockEntry.Location)
            .Concat(
                kegs
                    .SelectMany(keg => keg.StockEntries)
                    .Select(stockEntry => stockEntry.Location))
            .DistinctBy(location => location.Id)
            .OrderBy(location => location.LocationCode)
            .ToList();

        DateTime cutoffDate = DateTime.Today.AddDays(30);

        DashboardViewModel viewModel = new()
        {
            TotalCans = cans.Count,
            TotalKegs = kegs.Count,
            TotalLocations = locations.Count,
            TotalBeerStyles = beerStyles.Count,
            ExpiringCans = cans
                .Where(can => can.BestBefore.Date <= cutoffDate)
                .OrderBy(can => can.BestBefore)
                .ToList(),
            ExpiringKegs = kegs
                .Where(keg => keg.BestBefore.Date <= cutoffDate)
                .OrderBy(keg => keg.BestBefore)
                .ToList(),
            Locations = locations
        };

        return View(viewModel);
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
