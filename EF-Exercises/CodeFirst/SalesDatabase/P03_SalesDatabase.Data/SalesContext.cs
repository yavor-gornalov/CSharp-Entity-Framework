using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Common;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data;

public class SalesContext : DbContext
{
    public SalesContext()
    {
    }

    public SalesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<Sale> Sales { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(Config.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Email)
                .IsUnicode(false);
        });
    }
}
