using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web;

[Authorize]
[Route("beer-styles")]
public class BeerStyleController : Controller
{
    private readonly BeerStyleRepository repository;

    public BeerStyleController(BeerStyleRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [Route("{id:int}/detail")]
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
