namespace BreweryWarehouse.Model;

public abstract class Container
{
    public int Id { get; set; }

    public string SLCode { get; set; } = string.Empty;

    public DateTime BestBefore { get; set; }
}
