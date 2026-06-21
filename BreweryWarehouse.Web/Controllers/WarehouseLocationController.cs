using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("locations")]
public class WarehouseLocationController : Controller
{
    private readonly WarehouseLocationRepository repository;
    private readonly ILogger<WarehouseLocationController> _logger;

    public WarehouseLocationController(WarehouseLocationRepository repository, ILogger<WarehouseLocationController> logger)
    {
        this.repository = repository;
        _logger = logger;
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
            _logger.LogWarning("WarehouseLocation creation failed validation for {LocationCode} by {User}", model.LocationCode, User.Identity?.Name);
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
        _logger.LogInformation("WarehouseLocation {Id} '{LocationCode}' created by {User}", location.Id, location.LocationCode, User.Identity?.Name);

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
            _logger.LogWarning("WarehouseLocation {Id} edit failed validation by {User}", id, User.Identity?.Name);
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
        _logger.LogInformation("WarehouseLocation {Id} '{LocationCode}' updated by {User}", id, location.LocationCode, User.Identity?.Name);

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

        if (location.StockEntries.Any())
        {
            int count = location.StockEntries.Count;
            _logger.LogWarning("WarehouseLocation {Id} '{LocationCode}' delete blocked — {Count} stock {Plural} still reference it, by {User}",
                id, location.LocationCode, count, count == 1 ? "entry" : "entries", User.Identity?.Name);
            TempData["Error"] = $"Cannot delete '{location.LocationCode}' — {count} stock {(count == 1 ? "entry" : "entries")} reference this location. Remove the stock entries first.";
            return RedirectToAction("Details", new { id });
        }

        try
        {
            repository.Delete(location);
            _logger.LogInformation("WarehouseLocation {Id} '{LocationCode}' deleted by {User}", id, location.LocationCode, User.Identity?.Name);
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = $"Cannot delete '{location.LocationCode}' — it is still referenced by stock entries.";
            return RedirectToAction("Details", new { id });
        }

        return RedirectToAction("Index");
    }
}
