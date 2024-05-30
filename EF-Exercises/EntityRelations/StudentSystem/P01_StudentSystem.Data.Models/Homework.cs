using P01_StudentSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    [Key]
    public int HomeworkId { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public ContentType ContentType { get; set; }

    public DateTime SubmissionTime { get; set; }

    public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
}
