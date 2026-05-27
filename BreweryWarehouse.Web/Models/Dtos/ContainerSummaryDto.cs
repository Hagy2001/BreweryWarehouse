namespace BreweryWarehouse.Web.Models.Dtos;

public class ContainerSummaryDto
{
    public int      Id            { get; set; }
    public string   SLCode        { get; set; } = string.Empty;
    public DateTime BestBefore    { get; set; }
    public string   Type          { get; set; } = string.Empty; // "Can" or "Keg"
    public string   BeerStyleName { get; set; } = string.Empty;
}
