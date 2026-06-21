namespace BreweryWarehouse.Web.Models;

public class UserRoleListItem
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? CurrentRole { get; set; }
}
