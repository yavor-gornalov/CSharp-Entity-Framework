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
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Bet> Bets { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

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
        // Set Composite PK for PlayersStatistics
        modelBuilder
            .Entity<PlayerStatistic>()
            .HasKey(ps => new { ps.PlayerId, ps.GameId });

        // Set Team -> PrimaryKitColor relation
        modelBuilder
            .Entity<Team>()
            .HasOne(t => t.PrimaryKitColor)
            .WithMany(c => c.PrimaryKitTeams)
            .HasForeignKey(t => t.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Set Team -> SecondaryKitColor relation
        modelBuilder
            .Entity<Team>()
            .HasOne(t => t.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(t => t.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Set Game -> HomeTeam relation
        modelBuilder
            .Entity<Game>()
            .HasOne(g => g.HomeTeam)
            .WithMany(t => t.HomeGames)
            .HasForeignKey(g => g.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        // Set Game -> AwayTeam relation
        modelBuilder
            .Entity<Game>()
            .HasOne(g => g.AwayTeam)
            .WithMany(t => t.AwayGames)
            .HasForeignKey(g => g.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        // Set Player -> Town relation
        modelBuilder
            .Entity<Player>()
            .HasOne(p => p.Town)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TownId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
