using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;

public class Course
{
    public Course()
    {
        this.Resources = new List<Resource>();
        this.Homeworks = new List<Homework>();
        this.StudentsCourses = new List<StudentCourse>();
    }

    [Key]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(GlobalConstants.CourseNameMaxLength)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Resource> Resources { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
}
