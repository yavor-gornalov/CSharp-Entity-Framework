using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static SoftJail.Common.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

[XmlType("Officer")]
public class ImportOfficerDto
{
	[XmlElement("Name")]
	[Required]
	[StringLength(OfficerFullNameMaxLength, MinimumLength = OfficerFullNameMinLength)]
	public string FullName { get; set; } = null!;

    [XmlElement("Money")]
    [Required]
    [Range(OfficerSalaryLowLimit, OfficerSalaryHighLimit)]
    public decimal Salary { get; set; }

    [XmlElement("Position")]
    [Required]
    [EnumDataType(typeof(Position))]
    public string Position { get; set; } = null!;

    [XmlElement("Weapon")]
    [Required]
    [EnumDataType(typeof(Weapon))]
    public string Weapon { get; set; } = null!;

    [XmlElement("DepartmentId")]
    [Required]
    public int DepartmentId { get; set; }

    [XmlArray("Prisoners")]
    public ImportPrisonerId[] Prisoners { get; set; }
}
