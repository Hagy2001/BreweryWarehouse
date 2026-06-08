using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Models;
using Xunit;

namespace BreweryWarehouse.Tests;

public abstract class ApiTestBase : IDisposable
{
    internal readonly BreweryWarehouseWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected ApiTestBase()
    {
        Factory = new BreweryWarehouseWebApplicationFactory();
        Client = Factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    // Seeds a BeerStyle and returns it
    protected async Task<BeerStyle> SeedBeerStyleAsync(
        BreweryWarehouseDbContext db,
        string name = "Test Lager")
    {
        var style = new BeerStyle
        {
            Name = name,
            Description = "Test description",
            AlcoholPercentage = 5.0,
            IBU = 20,
            ColorEBC = 8.0,
            Category = BeerCategory.Lager
        };
        db.BeerStyles.Add(style);
        await db.SaveChangesAsync();
        return style;
    }

    // Seeds a WarehouseLocation and returns it
    protected async Task<WarehouseLocation> SeedLocationAsync(
        BreweryWarehouseDbContext db,
        string code = "A-01")
    {
        var location = new WarehouseLocation
        {
            LocationCode = code,
            Aisle = "A",
            Shelf = 1,
            MaxCapacity = 100,
            Description = "Test location"
        };
        db.WarehouseLocations.Add(location);
        await db.SaveChangesAsync();
        return location;
    }

    // Seeds a Can and returns it
    protected async Task<Can> SeedCanAsync(
        BreweryWarehouseDbContext db,
        int beerStyleId,
        string slCode = "SL-001",
        string barcode = "1234567890")
    {
        var can = new Can
        {
            SLCode = slCode,
            BestBefore = DateTime.UtcNow.AddMonths(6),
            Size = CanSize.Can033,
            Barcode = barcode,
            PackagingDate = DateTime.UtcNow,
            BeerStyleId = beerStyleId
        };
        db.Cans.Add(can);
        await db.SaveChangesAsync();
        return can;
    }

    // Seeds a Keg and returns it
    protected async Task<Keg> SeedKegAsync(
        BreweryWarehouseDbContext db,
        int beerStyleId,
        string slCode = "SL-KEG-001",
        string serialNumber = "SN-001")
    {
        var keg = new Keg
        {
            SLCode = slCode,
            BestBefore = DateTime.UtcNow.AddMonths(12),
            Material = KegMaterial.Metal,
            HeadType = KegHeadType.SHead,
            VolumeInLitres = 30,
            SerialNumber = serialNumber,
            LastInspection = DateTime.UtcNow,
            BeerStyleId = beerStyleId
        };
        db.Kegs.Add(keg);
        await db.SaveChangesAsync();
        return keg;
    }

    // Seeds a StockEntry and returns it
    protected async Task<StockEntry> SeedStockEntryAsync(
        BreweryWarehouseDbContext db,
        int containerId,
        int locationId)
    {
        var entry = new StockEntry
        {
            ContainerId = containerId,
            LocationId = locationId,
            Quantity = 50,
            DateReceived = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            Notes = "Test entry"
        };
        db.StockEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    // Seeds an Employee and returns it
    protected async Task<Employee> SeedEmployeeAsync(
        BreweryWarehouseDbContext db,
        string email = "worker@brewery.com")
    {
        var employee = new Employee
        {
            FirstName = "Test",
            LastName = "Worker",
            Email = email,
            Role = "Operator",
            DateHired = DateTime.UtcNow.AddYears(-1),
            IsActive = true
        };
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return employee;
    }

    // Creates an admin user and returns a cookie-authenticated client
    protected async Task<HttpClient> CreateAuthenticatedClientAsync(
        string role = "Admin")
    {
        using var scope = Factory.Services.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

        var email = $"test_{role}_{Guid.NewGuid():N}@brewery.com";
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            OIB = "12345678901",
            JMBG = "1234567890123",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, "Admin123!");
        await userManager.AddToRoleAsync(user, role);

        // Use the test cookie authentication approach:
        // POST to login endpoint with credentials
        var loginClient = Factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

        // Get antiforgery token
        var getResponse = await loginClient.GetAsync("/Identity/Account/Login");
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        var loginContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.Email"] = email,
            ["Input.Password"] = "Admin123!",
            ["__RequestVerificationToken"] = token
        });

        var loginResponse = await loginClient.PostAsync(
            "/Identity/Account/Login", loginContent);

        // If redirect to / the login succeeded and cookie is set
        // If still on login page, try reading the cookie directly
        return loginClient;
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        // Find the hidden input for antiforgery
        const string inputStart = "__RequestVerificationToken";
        var idx = html.IndexOf(inputStart, StringComparison.Ordinal);
        if (idx < 0) return string.Empty;

        var valueIdx = html.IndexOf("value=\"", idx, StringComparison.Ordinal);
        if (valueIdx < 0) return string.Empty;

        valueIdx += 7; // skip past value="
        var endIdx = html.IndexOf('"', valueIdx);
        if (endIdx < 0) return string.Empty;

        return html[valueIdx..endIdx];
    }

    protected static StringContent Json(object obj) =>
        new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
