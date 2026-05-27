using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models.Dtos;

public static class DtoExtensions
{
    public static BeerStyleDto ToDto(this BeerStyle b) => new()
    {
        Id                = b.Id,
        Name              = b.Name,
        Description       = b.Description,
        AlcoholPercentage = b.AlcoholPercentage,
        IBU               = b.IBU,
        ColorEBC          = b.ColorEBC,
        Category          = b.Category.ToString()
    };

    public static CanDto ToDto(this Can c) => new()
    {
        Id            = c.Id,
        SLCode        = c.SLCode,
        BestBefore    = c.BestBefore,
        Size          = c.Size.ToString(),
        Barcode       = c.Barcode,
        PackagingDate = c.PackagingDate,
        BeerStyle     = c.BeerStyle?.ToDto()
    };

    public static KegDto ToDto(this Keg k) => new()
    {
        Id             = k.Id,
        SLCode         = k.SLCode,
        BestBefore     = k.BestBefore,
        Material       = k.Material.ToString(),
        HeadType       = k.HeadType.ToString(),
        VolumeInLitres = k.VolumeInLitres,
        SerialNumber   = k.SerialNumber,
        LastInspection = k.LastInspection,
        BeerStyle      = k.BeerStyle?.ToDto()
    };

    public static ContainerSummaryDto ToSummaryDto(this Container c) => new()
    {
        Id            = c.Id,
        SLCode        = c.SLCode,
        BestBefore    = c.BestBefore,
        Type          = c is Can ? "Can" : "Keg",
        BeerStyleName = c is Can can
                            ? can.BeerStyle?.Name ?? string.Empty
                            : c is Keg keg
                                ? keg.BeerStyle?.Name ?? string.Empty
                                : string.Empty
    };

    public static WarehouseLocationDto ToDto(this WarehouseLocation w) => new()
    {
        Id           = w.Id,
        LocationCode = w.LocationCode,
        Aisle        = w.Aisle,
        Shelf        = w.Shelf,
        MaxCapacity  = w.MaxCapacity,
        Description  = w.Description
    };

    public static StockEntryDto ToDto(this StockEntry s) => new()
    {
        Id           = s.Id,
        Container    = s.Container?.ToSummaryDto(),
        Location     = s.Location?.ToDto(),
        Quantity     = s.Quantity,
        DateReceived = s.DateReceived,
        DateModified = s.DateModified,
        Notes        = s.Notes
    };

    public static EmployeeDto ToDto(this Employee e) => new()
    {
        Id        = e.Id,
        FirstName = e.FirstName,
        LastName  = e.LastName,
        Email     = e.Email,
        Role      = e.Role,
        DateHired = e.DateHired,
        IsActive  = e.IsActive
    };
}
