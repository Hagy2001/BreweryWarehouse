using BreweryWarehouse.Model;
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
}
