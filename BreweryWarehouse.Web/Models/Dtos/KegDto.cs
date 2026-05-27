namespace BreweryWarehouse.Web.Models.Dtos;

public class KegDto
{
    public int           Id             { get; set; }
    public string        SLCode         { get; set; } = string.Empty;
    public DateTime      BestBefore     { get; set; }
    public string        Material       { get; set; } = string.Empty;
    public string        HeadType       { get; set; } = string.Empty;
    public int           VolumeInLitres { get; set; }
    public string        SerialNumber   { get; set; } = string.Empty;
    public DateTime      LastInspection { get; set; }
    public BeerStyleDto? BeerStyle      { get; set; }
}
