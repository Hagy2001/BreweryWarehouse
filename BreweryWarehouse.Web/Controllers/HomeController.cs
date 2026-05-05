using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly BeerStyleRepository _beerStyleRepository;
    private readonly CanRepository _canRepository;
    private readonly KegRepository _kegRepository;
    private readonly WarehouseLocationRepository _warehouseLocationRepository;

    public HomeController(
        BeerStyleRepository beerStyleRepository,
        CanRepository canRepository,
        KegRepository kegRepository,
        WarehouseLocationRepository warehouseLocationRepository)
    {
        _beerStyleRepository = beerStyleRepository;
        _canRepository = canRepository;
        _kegRepository = kegRepository;
        _warehouseLocationRepository = warehouseLocationRepository;
    }

    public IActionResult Index()
    {
        List<Model.BeerStyle> beerStyles = _beerStyleRepository.GetAll();
        List<Model.Can> cans = _canRepository.GetAll();
        List<Model.Keg> kegs = _kegRepository.GetAll();
        List<Model.WarehouseLocation> locations = _warehouseLocationRepository.GetAll();

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
