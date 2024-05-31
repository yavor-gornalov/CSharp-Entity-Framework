using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models;

public class Diagnose
{
    [Key]
    public int DiagnoseId { get; set; }

    [Required, MaxLength(GlobalConstants.NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(GlobalConstants.CommentMaxLength)]
    public string Comments { get; set; } = null!;

    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public virtual Patient Patient { get; set; } = null!;
}
