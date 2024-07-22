namespace Cadastre.Data
{
	using Cadastre.Data.Models;
	using Microsoft.EntityFrameworkCore;
	public class CadastreContext : DbContext
	{
		public CadastreContext()
		{

		}

		public CadastreContext(DbContextOptions options)
			: base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PropertyCitizen>()
				.HasKey(pc => new { pc.PropertyId, pc.CitizenId });
		}

		public DbSet<Citizen> Citizens { get; set; } = null!;

		public DbSet<District> Districts { get; set; } = null!;

		public DbSet<Property> Properties { get; set; } = null!;

		public DbSet<PropertyCitizen> PropertiesCitizens { get; set; } = null!;
	}
}
