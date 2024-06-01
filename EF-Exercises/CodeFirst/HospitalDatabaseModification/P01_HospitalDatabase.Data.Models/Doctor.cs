using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Doctor
{
    public Doctor()
    {
        this.Visitations = new List<Visitation>();
    }

    [Key]
    public int DoctorId { get; set; }

    [Required, MaxLength(GlobalConstants.FullNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(GlobalConstants.SpecialityMaxLength)]
    public string Specialty { get; set; } = null!;

    public virtual ICollection<Visitation> Visitations { get; set; }
}
