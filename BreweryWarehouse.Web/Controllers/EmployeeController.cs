using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly EmployeeRepository repository;
    private readonly UserManager<AppUser> _userManager;

    public EmployeeController(EmployeeRepository repository, UserManager<AppUser> userManager)
    {
        this.repository = repository;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [HttpGet]
    [Route("Employee/search")]
    [AllowAnonymous]
    public JsonResult Search(string? q)
    {
        IEnumerable<Employee> employees = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            employees = employees.Where(employee =>
                (employee.LastName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (employee.FirstName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (employee.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var results = employees
            .OrderBy(employee => employee.LastName)
            .ThenBy(employee => employee.FirstName)
            .Take(20)
            .Select(employee => new
            {
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.Role,
                employee.IsActive,
                DateHiredDisplay = employee.DateHired.ToString("dd MMM yyyy")
            })
            .ToList();

        return Json(results);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        Employee? employee = repository.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        if (employee.AppUserId != null)
            ViewBag.LinkedUser = await _userManager.FindByIdAsync(employee.AppUserId);

        return View(employee);
    }

    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        return View(new EmployeeCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,WarehouseManager")]
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
            DateHired = model.DateHired!.Value,
            IsActive = model.IsActive
        };

        repository.Add(employee);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin,WarehouseManager")]
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
            IsActive = employee.IsActive,
            AppUserId = employee.AppUserId
        };

        var users = _userManager.Users
            .OrderBy(u => u.Email)
            .Select(u => new { u.Id, u.Email })
            .ToList();
        ViewBag.AppUsers = new SelectList(users, "Id", "Email", employee.AppUserId);

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id, EmployeeEditModel model)
    {
        if (!ModelState.IsValid)
        {
            var users = _userManager.Users
                .OrderBy(u => u.Email)
                .Select(u => new { u.Id, u.Email })
                .ToList();
            ViewBag.AppUsers = new SelectList(users, "Id", "Email", model.AppUserId);
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
        employee.DateHired = model.DateHired!.Value;
        employee.IsActive = model.IsActive;
        employee.AppUserId = string.IsNullOrEmpty(model.AppUserId) ? null : model.AppUserId;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
