using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

[XmlType("Officer")]
public class ImportOfficerDto
{
	[Required]
	[XmlElement("Name")]
	[StringLength(OfficerFullNameMaxLength, MinimumLength = OfficerFullNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("Money")]
	[Range(OfficerSalaryLowLimit, OfficerSalaryHighLimit)]
	public decimal Salary { get; set; }

	[Required]
	[XmlElement("Position")]
	[EnumDataType(typeof(Position))]
	public string Position { get; set; } = null!;

	[Required]
	[XmlElement("Weapon")]
	[EnumDataType(typeof(Weapon))]
	public string Weapon { get; set; } = null!;

	[Required]
	[XmlElement("DepartmentId")]
	public int DepartmentId { get; set; }

	[XmlArray("Prisoners")]
	public ImportPrisonerIdDto[] PrisonerIds { get; set; }
}