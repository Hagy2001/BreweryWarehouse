using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly EmployeeRepository repository;

    public EmployeeController(EmployeeRepository repository)
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

    public IActionResult Create()
    {
        return View(new EmployeeCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EmployeeCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Employee employee = new Employee
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Role = model.Role,
            DateHired = model.DateHired,
            IsActive = model.IsActive
        };

        repository.Add(employee);

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        Employee? employee = repository.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        EmployeeEditModel model = new EmployeeEditModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Role = employee.Role,
            DateHired = employee.DateHired,
            IsActive = employee.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EmployeeEditModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Employee? employee = repository.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.Email = model.Email;
        employee.Role = model.Role;
        employee.DateHired = model.DateHired;
        employee.IsActive = model.IsActive;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        Employee? employee = repository.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        repository.Delete(employee);

        return RedirectToAction("Index");
    }
}
