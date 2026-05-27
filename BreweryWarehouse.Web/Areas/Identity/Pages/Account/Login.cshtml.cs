using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Web.Models;

namespace BreweryWarehouse.Web.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;

    public LoginModel(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await _signInManager.PasswordSignInAsync(
            Input.Email, Input.Password,
            isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return LocalRedirect("/");

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return Page();
    }

    public IActionResult OnPostExternalLogin(string provider)
    {
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }
}
