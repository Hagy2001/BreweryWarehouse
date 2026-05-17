using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web;

[Authorize]
[Route("beer-styles")]
public class BeerStyleController : Controller
{
    private readonly BeerStyleRepository repository;

    public BeerStyleController(BeerStyleRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [HttpGet]
    [Route("search")]
    public JsonResult Search(string? q)
    {
        IEnumerable<BeerStyle> beerStyles = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            beerStyles = beerStyles.Where(style =>
                style.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        var results = beerStyles
            .OrderBy(style => style.Name)
            .Take(20)
            .Select(style => new
            {
                style.Id,
                style.Name,
                Category = style.Category.GetDescription(),
                style.AlcoholPercentage,
                style.IBU,
                style.ColorEBC
            })
            .ToList();

        return Json(results);
    }

    [Route("{id:int}/detail")]
    public IActionResult Details(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        return View(beerStyle);
    }

    [Route("create")]
    public IActionResult Create()
    {
        return View(new BeerStyleCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public IActionResult Create(BeerStyleCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        BeerStyle beerStyle = new BeerStyle
        {
            Name = model.Name,
            Description = model.Description,
            AlcoholPercentage = model.AlcoholPercentage,
            IBU = model.IBU,
            ColorEBC = model.ColorEBC,
            Category = model.Category
        };

        repository.Add(beerStyle);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        BeerStyleEditModel model = new BeerStyleEditModel
        {
            Id = beerStyle.Id,
            Name = beerStyle.Name,
            Description = beerStyle.Description,
            AlcoholPercentage = beerStyle.AlcoholPercentage,
            IBU = beerStyle.IBU,
            ColorEBC = beerStyle.ColorEBC,
            Category = beerStyle.Category
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    public IActionResult Edit(int id, BeerStyleEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        beerStyle.Name = model.Name;
        beerStyle.Description = model.Description;
        beerStyle.AlcoholPercentage = model.AlcoholPercentage;
        beerStyle.IBU = model.IBU;
        beerStyle.ColorEBC = model.ColorEBC;
        beerStyle.Category = model.Category;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        repository.SoftDelete(beerStyle);

        return RedirectToAction("Index");
    }
}
