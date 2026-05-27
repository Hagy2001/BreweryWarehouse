namespace BreweryWarehouse.Web.Models.Dtos;

public class CanDto
{
    public int           Id            { get; set; }
    public string        SLCode        { get; set; } = string.Empty;
    public DateTime      BestBefore    { get; set; }
    public string        Size          { get; set; } = string.Empty;
    public string        Barcode       { get; set; } = string.Empty;
    public DateTime      PackagingDate { get; set; }
    public BeerStyleDto? BeerStyle     { get; set; }
}
