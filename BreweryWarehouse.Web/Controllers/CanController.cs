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

    public CanController(CanRepository repository, BeerStyleRepository beerStyleRepository)
    {
        this.repository = repository;
        this.beerStyleRepository = beerStyleRepository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [Route("{id:int}/info")]
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
    public IActionResult Create()
    {
        PopulateBeerStyles();
        return View(new CanCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public IActionResult Create(CanCreateModel model)
    {
        if (!ModelState.IsValid)
        {
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

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
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
    public IActionResult Edit(int id, CanEditModel model)
    {
        if (!ModelState.IsValid)
        {
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

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        repository.SoftDelete(can);

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
