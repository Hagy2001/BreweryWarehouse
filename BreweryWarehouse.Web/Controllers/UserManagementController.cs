using BreweryWarehouse.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BreweryWarehouse.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserManagementController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.OrderBy(u => u.Email).ToList();
        var items = new List<UserRoleListItem>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            items.Add(new UserRoleListItem
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                CurrentRole = roles.FirstOrDefault()
            });
        }

        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var currentRole = roles.FirstOrDefault();

        await PopulateRolesDropdown(currentRole);
        ViewBag.IsCurrentUser = id == _userManager.GetUserId(User);

        return View(new UserRoleEditModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            SelectedRole = currentRole
        });
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UserRoleEditModel model)
    {
        if (id == _userManager.GetUserId(User))
        {
            ModelState.AddModelError(string.Empty, "You cannot change your own role through this page.");
            await PopulateRolesDropdown(model.SelectedRole);
            ViewBag.IsCurrentUser = true;
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var oldRoles = await _userManager.GetRolesAsync(user);
        var oldRole = oldRoles.FirstOrDefault() ?? "None";

        if (oldRoles.Count > 0)
            await _userManager.RemoveFromRolesAsync(user, oldRoles);

        if (!string.IsNullOrEmpty(model.SelectedRole))
            await _userManager.AddToRoleAsync(user, model.SelectedRole);

        var newRole = string.IsNullOrEmpty(model.SelectedRole) ? "None" : model.SelectedRole;
        _logger.LogInformation("User {Email} role changed from {OldRole} to {NewRole} by {AdminUser}",
            user.Email, oldRole, newRole, User.Identity?.Name);

        TempData["Success"] = $"Role for {user.Email} updated to {newRole}.";
        return RedirectToAction("Index");
    }

    private async Task PopulateRolesDropdown(string? selectedRole)
    {
        var allRoles = _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name)
            .ToList();

        var roleItems = allRoles
            .Select(r => new SelectListItem(r, r, r == selectedRole))
            .ToList();
        roleItems.Insert(0, new SelectListItem("— No role —", "", string.IsNullOrEmpty(selectedRole)));
        ViewBag.Roles = roleItems;
    }
}
