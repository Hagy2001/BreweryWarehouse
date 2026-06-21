namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime DateHired { get; set; }

    public bool IsActive { get; set; }

    public string? AppUserId { get; set; }
}
