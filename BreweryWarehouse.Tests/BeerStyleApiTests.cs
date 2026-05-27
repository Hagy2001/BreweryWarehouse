using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class BeerStyleApiTests : ApiTestBase
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
        await SeedBeerStyleAsync(db);

        var response = await Client.GetAsync("/api/beer-styles");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<BeerStyleDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);

        var response = await Client.GetAsync($"/api/beer-styles/{style.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<BeerStyleDto>(JsonOptions);
        dto!.Id.Should().Be(style.Id);
        dto.Name.Should().Be(style.Name);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/beer-styles/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_WithQuery_ReturnsFilteredResults()
    {
        var db = GetDb();
        await SeedBeerStyleAsync(db, "Unique Amber Ale");
        await SeedBeerStyleAsync(db, "Other Beer");

        var response = await Client.GetAsync("/api/beer-styles?q=Unique");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<BeerStyleDto>>(JsonOptions);
        list.Should().OnlyContain(b => b.Name.Contains("Unique"));
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var body = Json(new
        {
            name = "Test IPA",
            description = "Hoppy",
            alcoholPercentage = 6.5,
            ibu = 50,
            colorEBC = 12.0,
            category = 2
        });

        var response = await Client.PostAsync("/api/beer-styles", body);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Returns201_WhenAuthenticated()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            name = "New IPA",
            description = "Hoppy and bitter",
            alcoholPercentage = 6.5,
            ibu = 55,
            colorEBC = 10.0,
            category = 2
        });

        var response = await authClient.PostAsync("/api/beer-styles", body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_Returns400_WhenModelInvalid()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new { name = "" });

        var response = await authClient.PostAsync("/api/beer-styles", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            name = "Updated Name",
            description = "Updated description",
            alcoholPercentage = 7.0,
            ibu = 30,
            colorEBC = 15.0,
            category = 0
        });

        var response = await authClient.PutAsync(
            $"/api/beer-styles/{style.Id}", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            name = "Valid Name",
            description = "Valid description that is long enough",
            alcoholPercentage = 5.0,
            ibu = 20,
            colorEBC = 8.0,
            category = 0
        });

        var response = await authClient.PutAsync("/api/beer-styles/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/beer-styles/{style.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/beer-styles/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var style = await SeedBeerStyleAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/beer-styles/{style.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
