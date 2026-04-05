using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class WarehouseLocationController : Controller
{
    private readonly WarehouseLocationMockRepository repository;

    public WarehouseLocationController(WarehouseLocationMockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    public IActionResult Details(int id)
    {
        WarehouseLocation? location = repository.GetById(id);

        if (location is null)
        {
            return NotFound();
        }

        return View(location);
    }
}
