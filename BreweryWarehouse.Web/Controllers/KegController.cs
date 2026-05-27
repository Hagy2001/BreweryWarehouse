using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("kegs")]
public class KegController : Controller
{
    private readonly KegRepository repository;
    private readonly BeerStyleRepository beerStyleRepository;

    public KegController(KegRepository repository, BeerStyleRepository beerStyleRepository)
    {
        this.repository = repository;
        this.beerStyleRepository = beerStyleRepository;
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
        IEnumerable<Keg> kegs = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            kegs = kegs.Where(keg =>
                (keg.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (keg.BeerStyle?.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var results = kegs
            .OrderBy(keg => keg.SLCode)
            .Take(20)
            .Select(keg => new
            {
                keg.Id,
                keg.SLCode,
                BeerStyleName = keg.BeerStyle?.Name ?? string.Empty,
                keg.VolumeInLitres,
                MaterialDisplay = keg.Material.GetDescription(),
                HeadTypeDisplay = keg.HeadType.GetDescription(),
                LastInspectionDisplay = keg.LastInspection.ToString("dd MMM yyyy"),
                BestBefore = keg.BestBefore,
                BestBeforeDisplay = keg.BestBefore.ToString("dd MMM yyyy")
            })
            .ToList();

        return Json(results);
    }

    [Route("{id:int}/info")]
    [Authorize]
    public IActionResult Details(int id)
    {
        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        return View(keg);
    }

    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        PopulateBeerStyles();
        return View(new KegCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create(KegCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateBeerStyles();
            return View(model);
        }

        Keg keg = new Keg
        {
            SLCode = model.SLCode,
            BestBefore = model.BestBefore!.Value,
            Material = model.Material,
            HeadType = model.HeadType,
            VolumeInLitres = model.VolumeInLitres,
            SerialNumber = model.SerialNumber,
            LastInspection = model.LastInspection!.Value,
            BeerStyleId = model.BeerStyleId
        };

        repository.Add(keg);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id)
    {
        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        KegEditModel model = new KegEditModel
        {
            Id = keg.Id,
            SLCode = keg.SLCode,
            BestBefore = keg.BestBefore,
            Material = keg.Material,
            HeadType = keg.HeadType,
            VolumeInLitres = keg.VolumeInLitres,
            SerialNumber = keg.SerialNumber,
            LastInspection = keg.LastInspection,
            BeerStyleId = keg.BeerStyleId,
            BeerStyleName = keg.BeerStyle?.Name
        };

        PopulateBeerStyles();
        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id, KegEditModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateBeerStyles();
            return View(model);
        }

        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        keg.SLCode = model.SLCode;
        keg.BestBefore = model.BestBefore!.Value;
        keg.Material = model.Material;
        keg.HeadType = model.HeadType;
        keg.VolumeInLitres = model.VolumeInLitres;
        keg.SerialNumber = model.SerialNumber;
        keg.LastInspection = model.LastInspection!.Value;
        keg.BeerStyleId = model.BeerStyleId;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        repository.SoftDelete(keg);

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
