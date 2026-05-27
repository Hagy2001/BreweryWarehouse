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
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [HttpGet]
    [Route("search")]
    [AllowAnonymous]
    public JsonResult Search(string? q)
    {
        IEnumerable<WarehouseLocation> locations = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            locations = locations.Where(location =>
                (location.LocationCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (location.Aisle?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var results = locations
            .OrderBy(location => location.LocationCode)
            .Take(20)
            .Select(location =>
            {
                int used = location.StockEntries.Sum(entry => entry.Quantity);
                int max = location.MaxCapacity;
                bool isOverfill = max > 0 && used > max;
                double percentage = max > 0 ? (double)used / max * 100 : 0;
                int bucketedPercentage = (int)(Math.Round(Math.Min(100.0, percentage) / 5.0) * 5);

                return new
                {
                    location.Id,
                    location.LocationCode,
                    location.Aisle,
                    location.Shelf,
                    Used = used,
                    MaxCapacity = max,
                    BucketedPercentage = bucketedPercentage,
                    IsOverfill = isOverfill
                };
            })
            .ToList();

        return Json(results);
    }

    [Route("{id:int}/view")]
    [Authorize]
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
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        return View(new WarehouseLocationCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
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
    [Authorize(Roles = "Admin,WarehouseManager")]
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
    [Authorize(Roles = "Admin,WarehouseManager")]
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
    [Authorize(Roles = "Admin")]
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
