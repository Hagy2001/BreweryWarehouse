namespace BreweryWarehouse.Model;

public class Keg : Container
{
	private int _volumeInLitres;

	public KegMaterial Material { get; set; }

	public KegHeadType HeadType { get; set; }

	public int VolumeInLitres
	{
		get => _volumeInLitres;
		set
		{
			if (value != 20 && value != 30)
			{
				throw new ArgumentException("VolumeInLitres must be either 20 or 30.");
			}

			_volumeInLitres = value;
		}
	}

	public string SerialNumber { get; set; } = string.Empty;

	public DateTime LastInspection { get; set; }

	public List<StockEntry> StockEntries { get; set; }

	public BeerStyle BeerStyle { get; set; } = null!;

	public Keg()
	{
		VolumeInLitres = 20;
		StockEntries = new List<StockEntry>();
	}
}
