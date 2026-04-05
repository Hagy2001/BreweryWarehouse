using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class CanController : Controller
{
    private readonly CanMockRepository repository;

    public CanController(CanMockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

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
