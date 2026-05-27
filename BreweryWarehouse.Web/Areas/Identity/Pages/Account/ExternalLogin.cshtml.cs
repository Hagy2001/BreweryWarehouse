using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BreweryWarehouse.Web.Models;

namespace BreweryWarehouse.Web.Areas.Identity.Pages.Account;

public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public ExternalLoginModel(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ProviderDisplayName { get; set; } = string.Empty;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB must contain only digits.")]
        public string OIB { get; set; } = string.Empty;

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG must contain only digits.")]
        public string JMBG { get; set; } = string.Empty;
    }

    public IActionResult OnGet() => RedirectToPage("./Login");

    public IActionResult OnPost(string provider)
    {
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToPage("./Login");

        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey,
            isPersistent: false, bypassTwoFactor: true);

        if (result.Succeeded)
            return LocalRedirect("/");

        // First time — show the completion form
        ProviderDisplayName = info.ProviderDisplayName ?? info.LoginProvider;
        Input.Email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmationAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToPage("./Login");

        if (!ModelState.IsValid)
        {
            ProviderDisplayName = info.ProviderDisplayName ?? info.LoginProvider;
            return Page();
        }

        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            OIB = Input.OIB,
            JMBG = Input.JMBG,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user);
        if (createResult.Succeeded)
        {
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect("/");
        }

        foreach (var error in createResult.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        ProviderDisplayName = info.ProviderDisplayName ?? info.LoginProvider;
        return Page();
    }
}
