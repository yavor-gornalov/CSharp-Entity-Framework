using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;

public class Student
{
    public Student()
    {
        this.Homeworks = new List<Homework>();
        this.StudentsCourses = new List<StudentCourse>();
    }

    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(GlobalConstants.StudentNameMaxLength)]
    public string Name { get; set; } = null!;

    [StringLength(GlobalConstants.PhoneNumberLength)]
    public string? PhoneNumber { get; set; }

    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
}
