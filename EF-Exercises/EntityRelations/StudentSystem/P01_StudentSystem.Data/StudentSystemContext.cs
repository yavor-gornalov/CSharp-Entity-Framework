using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Common;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
    // For Testing
    public StudentSystemContext()
    {
    }

    // Used from Judge system
    public StudentSystemContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; } = null!;

    public DbSet<Homework> Homeworks { get; set; } = null!;

    public DbSet<Resource> Resources { get; set; } = null!;

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Call base method first
        base.OnConfiguring(optionsBuilder);

        // Call custom configuration if no base configuration available 
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(Config.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(s => s.PhoneNumber)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.Property(r => r.Url)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.CourseId, sc.StudentId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentsCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentsCourses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
