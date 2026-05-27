using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class StockEntryApiTests : ApiTestBase
{
    private BreweryWarehouseDbContext GetDb()
    {
        var scope = Factory.Services.CreateScope();
        return scope.ServiceProvider
            .GetRequiredService<BreweryWarehouseDbContext>();
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        await SeedStockEntryAsync(db, can.Id, location.Id);

        var response = await Client.GetAsync("/api/stock-entries");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<StockEntryDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var entry = await SeedStockEntryAsync(db, can.Id, location.Id);

        var response = await Client.GetAsync($"/api/stock-entries/{entry.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<StockEntryDto>(JsonOptions);
        dto!.Id.Should().Be(entry.Id);
        dto.Quantity.Should().Be(entry.Quantity);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/stock-entries/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);

        var body = Json(new
        {
            containerId = can.Id,
            locationId = location.Id,
            quantity = 10,
            dateReceived = DateTime.UtcNow,
            notes = "Test"
        });

        var response = await Client.PostAsync("/api/stock-entries", body);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Returns201_WhenAuthenticated()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            containerId = can.Id,
            locationId = location.Id,
            quantity = 10,
            dateReceived = DateTime.UtcNow,
            notes = "Test"
        });

        var response = await authClient.PostAsync("/api/stock-entries", body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_Returns400_WhenModelInvalid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            containerId = can.Id,
            locationId = location.Id,
            quantity = -1,
            dateReceived = DateTime.UtcNow,
            notes = "Test"
        });

        var response = await authClient.PostAsync("/api/stock-entries", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var entry = await SeedStockEntryAsync(db, can.Id, location.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            containerId = can.Id,
            locationId = location.Id,
            quantity = 10,
            dateReceived = DateTime.UtcNow,
            notes = "Test"
        });

        var response = await authClient.PutAsync(
            $"/api/stock-entries/{entry.Id}", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_Returns404_WhenNotFound()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            containerId = can.Id,
            locationId = location.Id,
            quantity = 10,
            dateReceived = DateTime.UtcNow,
            notes = "Test"
        });

        var response = await authClient.PutAsync("/api/stock-entries/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var entry = await SeedStockEntryAsync(db, can.Id, location.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/stock-entries/{entry.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/stock-entries/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var location = await SeedLocationAsync(db);
        var entry = await SeedStockEntryAsync(db, can.Id, location.Id);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/stock-entries/{entry.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
