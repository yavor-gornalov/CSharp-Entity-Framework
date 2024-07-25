using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Medicines.Common.ValidationConstants;

namespace Medicines.Data.Models;

public class Patient
{
	public Patient()
	{
		PatientsMedicines = new HashSet<PatientMedicine>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(PatientFullNameMaxLength)]
	public string FullName { get; set; } = null!;

	[Required]
	public AgeGroup AgeGroup { get; set; }

	[Required]
	public Gender Gender { get; set; }

	public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; }
}
