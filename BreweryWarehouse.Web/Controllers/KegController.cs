using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("kegs")]
public class KegController : Controller
{
    private readonly KegRepository repository;

    public KegController(KegRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [Route("{id:int}/info")]
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
    public IActionResult Create()
    {
        return View(new KegCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public IActionResult Create(KegCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Keg keg = new Keg
        {
            SLCode = model.SLCode,
            BestBefore = model.BestBefore,
            Material = model.Material,
            HeadType = model.HeadType,
            VolumeInLitres = model.VolumeInLitres,
            SerialNumber = model.SerialNumber,
            LastInspection = model.LastInspection,
            BeerStyleId = model.BeerStyleId
        };

        repository.Add(keg);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
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

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    public IActionResult Edit(int id, KegEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Keg? keg = repository.GetById(id);

        if (keg is null)
        {
            return NotFound();
        }

        keg.SLCode = model.SLCode;
        keg.BestBefore = model.BestBefore;
        keg.Material = model.Material;
        keg.HeadType = model.HeadType;
        keg.VolumeInLitres = model.VolumeInLitres;
        keg.SerialNumber = model.SerialNumber;
        keg.LastInspection = model.LastInspection;
        keg.BeerStyleId = model.BeerStyleId;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
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
}
