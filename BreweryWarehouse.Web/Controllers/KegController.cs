using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class KegController : Controller
{
    private readonly KegMockRepository repository;

    public KegController(KegMockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

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
