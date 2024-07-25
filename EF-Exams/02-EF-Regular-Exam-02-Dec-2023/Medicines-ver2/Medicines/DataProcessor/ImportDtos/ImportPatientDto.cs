using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Medicines.Common.GlobalConstants;

namespace Medicines.DataProcessor.ImportDtos;

public class ImportPatientDto
{
	[JsonProperty("FullName")]
	[Required]
	[StringLength(PatientFullNameMaxLength, MinimumLength = PatientFullNameMinLength)]
	public string FullName { get; set; } = null!;

	[JsonProperty("AgeGroup")]
	[Required]
	[EnumDataType(typeof(AgeGroup))]
	public int AgeGroup { get; set; }


	[JsonProperty("Gender")]
	[Required]
	[EnumDataType(typeof(Gender))]
	public int Gender { get; set; }


	[JsonProperty("Medicines")]
	public int[] Medicines { get; set; } = null!;
}
