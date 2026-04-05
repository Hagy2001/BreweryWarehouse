using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class StockEntryController : Controller
{
    private readonly StockEntryMockRepository repository;

    public StockEntryController(StockEntryMockRepository repository)
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
}
