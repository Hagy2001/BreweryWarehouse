namespace BreweryWarehouse.Web.Models;

public class UserRoleEditModel
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? SelectedRole { get; set; }
}
