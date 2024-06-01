using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models;

public class Visitation
{
    [Key]
    public int VisitationId { get; set; }

    public DateTime Date { get; set; }

    [Required, MaxLength(GlobalConstants.CommentMaxLength)]
    public string Comments { get; set; } = null!;

    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public virtual Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }
    [ForeignKey(nameof(DoctorId))]
    public virtual Doctor Doctor { get; set; } = null!;
}
