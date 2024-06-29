using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using static Medicines.Common.ValidationConstants;

namespace Medicines.DataProcessor.ImportDtos;

public class ImportPatientDto
{
	[Required]
	[JsonProperty("FullName")]
	[StringLength(PatientFullNameMaxLength, MinimumLength = PatientFullNameMinLength)]
	public string FullName { get; set; } = null!;

	[Required]
	[JsonProperty("AgeGroup")]
	[EnumDataType(typeof(AgeGroup))]
	public AgeGroup AgeGroup { get; set; }

	[Required]
	[JsonProperty("Gender")]
	[EnumDataType(typeof(Gender))]
	public Gender Gender { get; set; }

	[JsonProperty("Medicines")]
	public int[] MedicineIds { get; set; } = null!;
}
