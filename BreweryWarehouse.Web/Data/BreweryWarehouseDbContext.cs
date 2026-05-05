using BreweryWarehouse.Model;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Data;

public class BreweryWarehouseDbContext : DbContext
{
    public BreweryWarehouseDbContext(DbContextOptions<BreweryWarehouseDbContext> options) : base(options)
    {
    }

    public DbSet<BeerStyle> BeerStyles { get; set; }

    public DbSet<Can> Cans { get; set; }

    public DbSet<Keg> Kegs { get; set; }

    public DbSet<StockEntry> StockEntries { get; set; }

    public DbSet<WarehouseLocation> WarehouseLocations { get; set; }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Can>();
        modelBuilder.Entity<Keg>();
    }
}
