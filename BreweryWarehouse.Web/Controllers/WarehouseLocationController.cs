using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("locations")]
public class WarehouseLocationController : Controller
{
    private readonly WarehouseLocationRepository repository;

    public WarehouseLocationController(WarehouseLocationRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [Route("{id:int}/view")]
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
