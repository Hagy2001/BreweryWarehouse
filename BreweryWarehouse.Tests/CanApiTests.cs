using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class CanApiTests : ApiTestBase
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
        await SeedCanAsync(db, style.Id);

        var response = await Client.GetAsync("/api/cans");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<CanDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);

        var response = await Client.GetAsync($"/api/cans/{can.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<CanDto>(JsonOptions);
        dto!.Id.Should().Be(can.Id);
        dto.SLCode.Should().Be(can.SLCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/cans/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WithQuery_ReturnsFilteredResults()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        await SeedCanAsync(db, style.Id, "SL-001", "BAR-001");
        await SeedCanAsync(db, style.Id, "SL-999", "BAR-999");

        var response = await Client.GetAsync("/api/cans?q=SL-001");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<CanDto>>(JsonOptions);
        list.Should().OnlyContain(c => c.SLCode.Contains("SL-001") || c.Barcode.Contains("SL-001"));
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);

        var body = Json(new
        {
            slCode = "SL-NEW",
            bestBefore = DateTime.UtcNow.AddMonths(6),
            size = 0,
            barcode = "BAR-NEW",
            packagingDate = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await Client.PostAsync("/api/cans", body);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Returns201_WhenAuthenticated()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            slCode = "SL-NEW",
            bestBefore = DateTime.UtcNow.AddMonths(6),
            size = 0,
            barcode = "BAR-NEW",
            packagingDate = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PostAsync("/api/cans", body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_Returns400_WhenModelInvalid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            slCode = "",
            bestBefore = DateTime.UtcNow.AddMonths(6),
            size = 0,
            barcode = "BAR-NEW",
            packagingDate = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PostAsync("/api/cans", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            slCode = "SL-UPDATED",
            bestBefore = DateTime.UtcNow.AddMonths(12),
            size = 1,
            barcode = "BAR-UPDATED",
            packagingDate = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PutAsync(
            $"/api/cans/{can.Id}", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_Returns404_WhenNotFound()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            slCode = "SL-UPDATED",
            bestBefore = DateTime.UtcNow.AddMonths(12),
            size = 1,
            barcode = "BAR-UPDATED",
            packagingDate = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PutAsync("/api/cans/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/cans/{can.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/cans/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var can = await SeedCanAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/cans/{can.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
