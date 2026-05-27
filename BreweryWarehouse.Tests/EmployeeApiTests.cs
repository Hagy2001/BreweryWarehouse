using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models.Dtos;
using Xunit;

namespace BreweryWarehouse.Tests;

public class EmployeeApiTests : ApiTestBase
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
        await SeedEmployeeAsync(db);

        var response = await Client.GetAsync("/api/employees");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content
            .ReadFromJsonAsync<List<EmployeeDto>>(JsonOptions);
        list.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var employee = await SeedEmployeeAsync(db);

        var response = await Client.GetAsync($"/api/employees/{employee.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content
            .ReadFromJsonAsync<EmployeeDto>(JsonOptions);
        dto!.Id.Should().Be(employee.Id);
        dto.Email.Should().Be(employee.Email);
    }

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await Client.GetAsync("/api/employees/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_Returns401_WhenNotAuthenticated()
    {
        var body = Json(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.doe@brewery.com",
            role = "Operator",
            dateHired = DateTime.UtcNow,
            isActive = true
        });

        var response = await Client.PostAsync("/api/employees", body);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Returns201_WhenAuthenticated()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.doe@brewery.com",
            role = "Operator",
            dateHired = DateTime.UtcNow,
            isActive = true
        });

        var response = await authClient.PostAsync("/api/employees", body);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_Returns400_WhenModelInvalid()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            firstName = "",
            lastName = "Doe",
            email = "john.doe@brewery.com",
            role = "Operator",
            dateHired = DateTime.UtcNow,
            isActive = true
        });

        var response = await authClient.PostAsync("/api/employees", body);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_Returns200_WhenValid()
    {
        var db = GetDb();
        var employee = await SeedEmployeeAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var body = Json(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.doe@brewery.com",
            role = "Operator",
            dateHired = DateTime.UtcNow,
            isActive = true
        });

        var response = await authClient.PutAsync(
            $"/api/employees/{employee.Id}", body);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var body = Json(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.doe@brewery.com",
            role = "Operator",
            dateHired = DateTime.UtcNow,
            isActive = true
        });

        var response = await authClient.PutAsync("/api/employees/99999", body);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns204_WhenValid()
    {
        var db = GetDb();
        var employee = await SeedEmployeeAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("Admin");

        var response = await authClient.DeleteAsync(
            $"/api/employees/{employee.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var authClient = await CreateAuthenticatedClientAsync("Admin");
        var response = await authClient.DeleteAsync("/api/employees/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Returns403_WhenNotAdmin()
    {
        var db = GetDb();
        var employee = await SeedEmployeeAsync(db);
        var authClient = await CreateAuthenticatedClientAsync("WarehouseManager");

        var response = await authClient.DeleteAsync(
            $"/api/employees/{employee.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
