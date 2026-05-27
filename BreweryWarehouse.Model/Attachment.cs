namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;

public class Attachment
{
    [Key]
    public int Id { get; set; }

    public int StockEntryId { get; set; }

    public virtual StockEntry StockEntry { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; }
}
