using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data;

public class FootballBettingContext : DbContext
{
    // For Testing
    public FootballBettingContext()
    {
    }

    // Used from Judge system
    public FootballBettingContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Town> Towns { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;

    // Establish connection with Database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Call base method first
        base.OnConfiguring(optionsBuilder);

        // Call custom configuration if no base configuration available 
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(Config.ConnectionString);
    }

    // Defines Database createion rules
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }
}
