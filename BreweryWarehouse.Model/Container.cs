namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;

public abstract class Container
{
    [Key]
    public int Id { get; set; }

    public string SLCode { get; set; } = string.Empty;

    public DateTime BestBefore { get; set; }
}
