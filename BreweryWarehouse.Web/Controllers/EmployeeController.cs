using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly EmployeeMockRepository repository;

    public EmployeeController(EmployeeMockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    public IActionResult Details(int id)
    {
        Employee? employee = repository.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }
}
