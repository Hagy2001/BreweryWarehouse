namespace BreweryWarehouse.Web.Models;

public class DatePickerModel
{
    public string FieldName { get; set; } = string.Empty;

    public DateTime? Value { get; set; }

    public string Label { get; set; } = string.Empty;

    public bool IsRequired { get; set; }
}
