using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Patient
{
    public Patient()
    {
        this.Visitations = new List<Visitation>();
        this.Diagnoses = new List<Diagnose>();
        this.Prescriptions = new List<PatientMedicament>();
    }

    [Key]
    public int PatientId { get; set; }

    [Required, MaxLength(GlobalConstants.NameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(GlobalConstants.NameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required, MaxLength(GlobalConstants.AddressMaxLength)]
    public string Address { get; set; }

    [Required, MaxLength(GlobalConstants.EmailMaxLength)]
    public string Email { get; set; } = null!;

    public bool HasInsurance { get; set; }

    public virtual ICollection<Visitation> Visitations { get; set; }

    public virtual ICollection<Diagnose> Diagnoses { get; set; }

    public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
}
