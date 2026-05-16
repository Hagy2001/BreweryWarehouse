using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class StockEntryController : Controller
{
    private readonly StockEntryRepository repository;

    public StockEntryController(StockEntryRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    public IActionResult Details(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        return View(stockEntry);
    }

    public IActionResult Create()
    {
        return View(new StockEntryCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(StockEntryCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        StockEntry stockEntry = new StockEntry
        {
            ContainerId = model.ContainerId,
            LocationId = model.LocationId,
            Quantity = model.Quantity,
            DateReceived = model.DateReceived,
            DateModified = DateTime.UtcNow,
            Notes = model.Notes ?? string.Empty
        };

        repository.Add(stockEntry);

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        StockEntryEditModel model = new StockEntryEditModel
        {
            Id = stockEntry.Id,
            ContainerId = stockEntry.ContainerId,
            LocationId = stockEntry.LocationId,
            Quantity = stockEntry.Quantity,
            DateReceived = stockEntry.DateReceived,
            Notes = stockEntry.Notes
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, StockEntryEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        stockEntry.ContainerId = model.ContainerId;
        stockEntry.LocationId = model.LocationId;
        stockEntry.Quantity = model.Quantity;
        stockEntry.DateReceived = model.DateReceived;
        stockEntry.DateModified = DateTime.UtcNow;
        stockEntry.Notes = model.Notes ?? string.Empty;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        repository.Delete(stockEntry);

        return RedirectToAction("Index");
    }
}
