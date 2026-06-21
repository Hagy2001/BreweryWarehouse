using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("search")]
public class GlobalSearchController : Controller
{
    private readonly BeerStyleRepository _beerStyles;
    private readonly CanRepository _cans;
    private readonly KegRepository _kegs;
    private readonly WarehouseLocationRepository _locations;
    private readonly StockEntryRepository _stockEntries;
    private readonly EmployeeRepository _employees;

    public GlobalSearchController(
        BeerStyleRepository beerStyles,
        CanRepository cans,
        KegRepository kegs,
        WarehouseLocationRepository locations,
        StockEntryRepository stockEntries,
        EmployeeRepository employees)
    {
        _beerStyles = beerStyles;
        _cans = cans;
        _kegs = kegs;
        _locations = locations;
        _stockEntries = stockEntries;
        _employees = employees;
    }

    [HttpGet("global")]
    public JsonResult Global(string? q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
            return Json(Array.Empty<object>());

        string query = q.Trim();
        var results = new List<object>();

        results.AddRange(SearchPages(query));
        results.AddRange(SearchBeerStyles(query));
        results.AddRange(SearchCans(query));
        results.AddRange(SearchKegs(query));
        results.AddRange(SearchLocations(query));
        results.AddRange(SearchStockEntries(query));
        results.AddRange(SearchEmployees(query));

        return Json(results);
    }

    private IEnumerable<object> SearchPages(string query)
    {
        var pages = new List<(string label, string? url)>
        {
            ("Beer Styles", Url.Action("Index", "BeerStyle")),
            ("Cans", Url.Action("Index", "Can")),
            ("Kegs", Url.Action("Index", "Keg")),
            ("Locations", Url.Action("Index", "WarehouseLocation")),
            ("Stock Entries", Url.Action("Index", "StockEntry")),
            ("Employees", Url.Action("Index", "Employee")),
        };

        if (User.IsInRole("Admin"))
            pages.Add(("User Roles", Url.Action("Index", "UserManagement")));

        return pages
            .Where(p => p.label.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(p => (object)new
            {
                category = "Page",
                label = p.label,
                subtitle = (string?)null,
                url = p.url
            });
    }

    private IEnumerable<object> SearchBeerStyles(string query)
    {
        return _beerStyles.GetAll()
            .Where(b => b.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(5)
            .Select(b => (object)new
            {
                category = "Beer Style",
                label = b.Name,
                subtitle = $"{b.Category.GetDescription()}, {b.AlcoholPercentage}% ABV",
                url = Url.Action("Details", "BeerStyle", new { id = b.Id })
            });
    }

    private IEnumerable<object> SearchCans(string query)
    {
        return _cans.GetAll()
            .Where(c =>
                (c.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Barcode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            .Take(5)
            .Select(c => (object)new
            {
                category = "Can",
                label = c.SLCode ?? c.Barcode ?? $"#{c.Id}",
                subtitle = c.BeerStyle?.Name,
                url = Url.Action("Details", "Can", new { id = c.Id })
            });
    }

    private IEnumerable<object> SearchKegs(string query)
    {
        return _kegs.GetAll()
            .Where(k =>
                (k.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (k.SerialNumber?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            .Take(5)
            .Select(k => (object)new
            {
                category = "Keg",
                label = k.SerialNumber ?? k.SLCode ?? $"#{k.Id}",
                subtitle = k.BeerStyle?.Name,
                url = Url.Action("Details", "Keg", new { id = k.Id })
            });
    }

    private IEnumerable<object> SearchLocations(string query)
    {
        return _locations.GetAll()
            .Where(l => l.LocationCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
            .Take(5)
            .Select(l => (object)new
            {
                category = "Location",
                label = l.LocationCode ?? $"#{l.Id}",
                subtitle = l.Aisle != null ? $"Aisle {l.Aisle}, Shelf {l.Shelf}" : null,
                url = Url.Action("Details", "WarehouseLocation", new { id = l.Id })
            });
    }

    private IEnumerable<object> SearchStockEntries(string query)
    {
        return _stockEntries.GetAll()
            .Where(e =>
                (e.Container?.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.Location?.LocationCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            .Take(5)
            .Select(e => (object)new
            {
                category = "Stock Entry",
                label = e.Container?.SLCode ?? $"Entry #{e.Id}",
                subtitle = e.Location?.LocationCode != null ? $"@ {e.Location.LocationCode}" : null,
                url = Url.Action("Details", "StockEntry", new { id = e.Id })
            });
    }

    private IEnumerable<object> SearchEmployees(string query)
    {
        return _employees.GetAll()
            .Where(e =>
                (e.FirstName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (e.LastName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
            .Take(5)
            .Select(e => (object)new
            {
                category = "Employee",
                label = $"{e.FirstName} {e.LastName}".Trim(),
                subtitle = e.Role,
                url = Url.Action("Details", "Employee", new { id = e.Id })
            });
    }
}
