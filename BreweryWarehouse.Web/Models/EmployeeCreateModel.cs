using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models;

public class EmployeeCreateModel
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Date Hired")]
    public DateTime? DateHired { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
