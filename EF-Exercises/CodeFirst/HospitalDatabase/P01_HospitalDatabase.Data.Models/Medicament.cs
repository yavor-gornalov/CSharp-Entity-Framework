using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Medicament
{
    public Medicament()
    {
        this.Prescriptions = new List<PatientMedicament>();
    }

    [Key]
    public int MedicamentId { get; set; }

    [Required, MaxLength(GlobalConstants.NameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<PatientMedicament> Prescriptions { get; set; }

}
