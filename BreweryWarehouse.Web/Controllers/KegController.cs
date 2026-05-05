using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("kegs")]
public class KegController : Controller
{
    private readonly KegRepository repository;

    public KegController(KegRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [Route("{id:int}/info")]
    public IActionResult Details(int id)
    {
        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        return View(keg);
    }
}
