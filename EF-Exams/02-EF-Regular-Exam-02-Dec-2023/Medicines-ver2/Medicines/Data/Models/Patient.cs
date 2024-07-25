using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Medicines.Common.GlobalConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.Data.Models;

public class Patient
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(PatientFullNameMaxLength)]
	public string FullName { get; set; } = null!;

	[Required]
	public AgeGroup AgeGroup { get; set; }

	[Required]
	public Gender Gender { get; set; }

	public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new HashSet<PatientMedicine>();
}

//•	Id – integer, Primary Key
//•	FullName – text with length[5, 100] (required)
//•	AgeGroup – AgeGroup enum (Child = 0, Adult, Senior) (required)
//•	Gender – Gender enum (Male = 0, Female) (required)
//•	PatientsMedicines - collection of type PatientMedicine
