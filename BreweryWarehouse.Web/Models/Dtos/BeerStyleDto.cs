namespace BreweryWarehouse.Web.Models.Dtos;

public class BeerStyleDto
{
    public int    Id                { get; set; }
    public string Name              { get; set; } = string.Empty;
    public string Description       { get; set; } = string.Empty;
    public double AlcoholPercentage { get; set; }
    public int    IBU               { get; set; }
    public double ColorEBC          { get; set; }
    public string Category          { get; set; } = string.Empty;
}
