using System.Security.Claims;

namespace BreweryWarehouse.Web.Services;

public class AuthService
{
    private const string TestEmail = "admin@brewery.com";
    private const string TestPassword = "Brewery2026!";
    private const string TestRole = "Admin";
    private const string TestFullName = "Test Admin";

    public bool ValidateCredentials(string email, string password)
    {
        return string.Equals(email, TestEmail, StringComparison.OrdinalIgnoreCase)
            && password == TestPassword;
    }

    public ClaimsPrincipal? CreatePrincipal(string email)
    {
        if (!string.Equals(email, TestEmail, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, TestEmail),
            new Claim(ClaimTypes.Role, TestRole),
            new Claim("FullName", TestFullName)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
        return new ClaimsPrincipal(identity);
    }
}
