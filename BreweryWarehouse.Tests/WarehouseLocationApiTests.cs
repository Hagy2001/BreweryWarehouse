using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class WarehouseLocationApiTests : ApiTestBase
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
        await SeedLocationAsync(db);

        var response = await Client.GetAsync("/api/locations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<WarehouseLocationDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var location = await SeedLocationAsync(db);

        var response = await Client.GetAsync($"/api/locations/{location.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<WarehouseLocationDto>(JsonOptions);
        dto!.Id.Should().Be(location.Id);
        dto.LocationCode.Should().Be(location.LocationCode);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/locations/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var body = Json(new
        {
            locationCode = "B-01",
            aisle = "B",
            shelf = 2,
            maxCapacity = 200,
            description = "Test location"
        });

        var response = await Client.PostAsync("/api/locations", body);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Returns201_WhenAuthenticated()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            locationCode = "B-01",
            aisle = "B",
            shelf = 2,
            maxCapacity = 200,
            description = "Test location"
        });

        var response = await authClient.PostAsync("/api/locations", body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_Returns400_WhenModelInvalid()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            locationCode = "",
            aisle = "B",
            shelf = 2,
            maxCapacity = 200,
            description = "Test location"
        });

        var response = await authClient.PostAsync("/api/locations", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            locationCode = "B-01",
            aisle = "B",
            shelf = 2,
            maxCapacity = 200,
            description = "Test location"
        });

        var response = await authClient.PutAsync(
            $"/api/locations/{location.Id}", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            locationCode = "B-01",
            aisle = "B",
            shelf = 2,
            maxCapacity = 200,
            description = "Test location"
        });

        var response = await authClient.PutAsync("/api/locations/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/locations/{location.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/locations/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var location = await SeedLocationAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/locations/{location.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
