namespace BreweryWarehouse.Web.Models.Dtos;

public class StockEntryDto
{
    public int                   Id           { get; set; }
    public ContainerSummaryDto?  Container    { get; set; }
    public WarehouseLocationDto? Location     { get; set; }
    public int                   Quantity     { get; set; }
    public DateTime              DateReceived { get; set; }
    public DateTime              DateModified { get; set; }
    public string?               Notes        { get; set; }
}
