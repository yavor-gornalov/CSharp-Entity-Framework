using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Common;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data;

public class HospitalContext : DbContext
{
    public HospitalContext()
    {
    }

    public HospitalContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; } = null!;

    public DbSet<Visitation> Visitations { get; set; } = null!;

    public DbSet<Diagnose> Diagnoses { get; set; } = null!;

    public DbSet<Medicament> Medicaments { get; set; } = null!;

    public DbSet<PatientMedicament> PatientsMedicaments { get; set; } = null!;

    public DbSet<Doctor> Doctors { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(Config.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set Patient Emait to non-unicode
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(p => p.Email)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PatientMedicament>()
            .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

        modelBuilder.Entity<PatientMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(pm => pm.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PatientMedicament>()
            .HasOne(pm => pm.Patient)
            .WithMany(m => m.Prescriptions)
            .HasForeignKey(pm => pm.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
