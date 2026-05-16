using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
[Route("cans")]
public class CanController : Controller
{
    private readonly CanRepository repository;

    public CanController(CanRepository repository)
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
            return View(model);
        }

        Can can = new Can
        {
            SLCode = model.SLCode,
            BestBefore = model.BestBefore,
            Size = model.Size,
            Barcode = model.Barcode,
            PackagingDate = model.PackagingDate,
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
            return View(model);
        }

        Can? can = repository.GetById(id);

        if (can is null)
        {
            return NotFound();
        }

        can.SLCode = model.SLCode;
        can.BestBefore = model.BestBefore;
        can.Size = model.Size;
        can.Barcode = model.Barcode;
        can.PackagingDate = model.PackagingDate;
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
}
