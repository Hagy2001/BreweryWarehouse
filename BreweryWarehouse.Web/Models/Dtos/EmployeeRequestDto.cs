using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models.Dtos;

public class EmployeeRequestDto
{
    [Required][StringLength(50, MinimumLength = 2)]
    public string   FirstName { get; set; } = string.Empty;

    [Required][StringLength(50, MinimumLength = 2)]
    public string   LastName  { get; set; } = string.Empty;

    [Required][EmailAddress]
    public string   Email     { get; set; } = string.Empty;

    [Required][StringLength(50)]
    public string   Role      { get; set; } = string.Empty;

    [Required]
    public DateTime DateHired { get; set; }

    public bool IsActive      { get; set; }
}
