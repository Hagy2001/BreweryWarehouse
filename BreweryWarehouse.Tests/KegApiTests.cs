using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class KegApiTests : ApiTestBase
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
        await SeedKegAsync(db, style.Id);

        var response = await Client.GetAsync("/api/kegs");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<KegDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var keg = await SeedKegAsync(db, style.Id);

        var response = await Client.GetAsync($"/api/kegs/{keg.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<KegDto>(JsonOptions);
        dto!.Id.Should().Be(keg.Id);
        dto.SLCode.Should().Be(keg.SLCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/kegs/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WithQuery_ReturnsFilteredResults()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        await SeedKegAsync(db, style.Id, "KEG-001", "SN-001");
        await SeedKegAsync(db, style.Id, "KEG-999", "SN-999");

        var response = await Client.GetAsync("/api/kegs?q=SN-001");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<KegDto>>(JsonOptions);
        list.Should().OnlyContain(k => k.SLCode.Contains("SN-001") || k.SerialNumber.Contains("SN-001"));
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);

        var body = Json(new
        {
            slCode = "KEG-NEW",
            bestBefore = DateTime.UtcNow.AddMonths(6),
            material = 0,
            headType = 0,
            volumeInLitres = 20,
            serialNumber = "SN-NEW",
            lastInspection = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await Client.PostAsync("/api/kegs", body);
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
            slCode = "KEG-NEW",
            bestBefore = DateTime.UtcNow.AddMonths(6),
            material = 0,
            headType = 0,
            volumeInLitres = 20,
            serialNumber = "SN-NEW",
            lastInspection = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PostAsync("/api/kegs", body);
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
            material = 0,
            headType = 0,
            volumeInLitres = 20,
            serialNumber = "SN-NEW",
            lastInspection = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PostAsync("/api/kegs", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var keg = await SeedKegAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            slCode = "KEG-UPDATED",
            bestBefore = DateTime.UtcNow.AddMonths(12),
            material = 1,
            headType = 1,
            volumeInLitres = 30,
            serialNumber = "SN-UPDATED",
            lastInspection = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PutAsync(
            $"/api/kegs/{keg.Id}", body);
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
            slCode = "KEG-UPDATED",
            bestBefore = DateTime.UtcNow.AddMonths(12),
            material = 1,
            headType = 1,
            volumeInLitres = 30,
            serialNumber = "SN-UPDATED",
            lastInspection = DateTime.UtcNow,
            beerStyleId = style.Id
        });

        var response = await authClient.PutAsync("/api/kegs/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var keg = await SeedKegAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/kegs/{keg.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/kegs/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var keg = await SeedKegAsync(db, style.Id);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/kegs/{keg.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
