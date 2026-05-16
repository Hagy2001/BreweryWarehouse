using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
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

    [Route("create")]
    public IActionResult Create()
    {
        return View(new WarehouseLocationCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public IActionResult Create(WarehouseLocationCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        WarehouseLocation location = new WarehouseLocation
        {
            LocationCode = model.LocationCode,
            Aisle = model.Aisle,
            Shelf = model.Shelf,
            MaxCapacity = model.MaxCapacity,
            Description = model.Description ?? string.Empty
        };

        repository.Add(location);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        WarehouseLocation? location = repository.GetById(id);

        if (location is null)
        {
            return NotFound();
        }

        WarehouseLocationEditModel model = new WarehouseLocationEditModel
        {
            Id = location.Id,
            LocationCode = location.LocationCode,
            Aisle = location.Aisle,
            Shelf = location.Shelf,
            MaxCapacity = location.MaxCapacity,
            Description = location.Description
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    public IActionResult Edit(int id, WarehouseLocationEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        WarehouseLocation? location = repository.GetById(id);

        if (location is null)
        {
            return NotFound();
        }

        location.LocationCode = model.LocationCode;
        location.Aisle = model.Aisle;
        location.Shelf = model.Shelf;
        location.MaxCapacity = model.MaxCapacity;
        location.Description = model.Description ?? string.Empty;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        WarehouseLocation? location = repository.GetById(id);

        if (location is null)
        {
            return NotFound();
        }

        repository.Delete(location);

        return RedirectToAction("Index");
    }
}
