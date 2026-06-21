using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("cans")]
public class CanController : Controller
{
    private readonly CanRepository repository;
    private readonly BeerStyleRepository beerStyleRepository;
    private readonly ILogger<CanController> _logger;

    public CanController(CanRepository repository, BeerStyleRepository beerStyleRepository, ILogger<CanController> logger)
    {
        this.repository = repository;
        this.beerStyleRepository = beerStyleRepository;
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
        IEnumerable<Can> cans = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            cans = cans.Where(can =>
                (can.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (can.BeerStyle?.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var results = cans
            .OrderBy(can => can.SLCode)
            .Take(20)
            .Select(can => new
            {
                can.Id,
                can.SLCode,
                BeerStyleName = can.BeerStyle?.Name ?? string.Empty,
                SizeDisplay = can.Size switch
                {
                    CanSize.Can033 => "0.33 L",
                    CanSize.Can044 => "0.44 L",
                    _ => can.Size.ToString()
                },
                can.Barcode,
                PackagingDate = can.PackagingDate,
                PackagingDateDisplay = can.PackagingDate.ToString("dd MMM yyyy"),
                BestBefore = can.BestBefore,
                BestBeforeDisplay = can.BestBefore.ToString("dd MMM yyyy")
            })
            .ToList();

        return Json(results);
    }

    [Route("{id:int}/info")]
    [Authorize]
    public IActionResult Details(int id)
    {
        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        return View(can);
    }

    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        PopulateBeerStyles();
        return View(new CanCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create(CanCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Can creation failed validation for SLCode {SLCode} by {User}", model.SLCode, User.Identity?.Name);
            PopulateBeerStyles();
            return View(model);
        }

        Can can = new Can
        {
            SLCode = model.SLCode,
            BestBefore = model.BestBefore!.Value,
            Size = model.Size,
            Barcode = model.Barcode,
            PackagingDate = model.PackagingDate!.Value,
            BeerStyleId = model.BeerStyleId
        };

        repository.Add(can);
        _logger.LogInformation("Can {Id} (SLCode: {SLCode}) created by {User}", can.Id, can.SLCode, User.Identity?.Name);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id)
    {
        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        CanEditModel model = new CanEditModel
        {
            Id = can.Id,
            SLCode = can.SLCode,
            BestBefore = can.BestBefore,
            Size = can.Size,
            Barcode = can.Barcode,
            PackagingDate = can.PackagingDate,
            BeerStyleId = can.BeerStyleId,
            BeerStyleName = can.BeerStyle?.Name
        };

        PopulateBeerStyles();
        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id, CanEditModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Can {Id} edit failed validation by {User}", id, User.Identity?.Name);
            PopulateBeerStyles();
            return View(model);
        }

        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        can.SLCode = model.SLCode;
        can.BestBefore = model.BestBefore!.Value;
        can.Size = model.Size;
        can.Barcode = model.Barcode;
        can.PackagingDate = model.PackagingDate!.Value;
        can.BeerStyleId = model.BeerStyleId;

        repository.Update();
        _logger.LogInformation("Can {Id} (SLCode: {SLCode}) updated by {User}", id, can.SLCode, User.Identity?.Name);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        repository.SoftDelete(can);
        _logger.LogInformation("Can {Id} (SLCode: {SLCode}) soft-deleted by {User}", id, can.SLCode, User.Identity?.Name);

        return RedirectToAction("Index");
    }

    private void PopulateBeerStyles()
    {
        ViewBag.BeerStyles = new SelectList(
            beerStyleRepository.GetAll().OrderBy(bs => bs.Name),
            "Id",
            "Name");
    }
}
