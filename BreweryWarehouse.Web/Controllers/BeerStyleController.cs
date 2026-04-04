using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web;

public class BeerStyleController : Controller
{
    private readonly BeerStyleMockRepository repository;

    public BeerStyleController(BeerStyleMockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    public IActionResult Details(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        return View(beerStyle);
    }
}
