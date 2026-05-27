using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using BreweryWarehouse.Web.Data;

namespace BreweryWarehouse.Tests;

public class BreweryWarehouseWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _dbName = "BWTests_" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["Authentication:Google:ClientId"] = "test-client-id",
                ["Authentication:Google:ClientSecret"] = "test-client-secret",
                ["ConnectionStrings:DefaultConnection"] = "TestConnection",
                ["SkipDatabaseSeeder"] = "true"
            };
            config.AddInMemoryCollection(testSettings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<BreweryWarehouseDbContext>>();
            services.RemoveAll<BreweryWarehouseDbContext>();

            services.AddDbContext<BreweryWarehouseDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
        });
    }
}
