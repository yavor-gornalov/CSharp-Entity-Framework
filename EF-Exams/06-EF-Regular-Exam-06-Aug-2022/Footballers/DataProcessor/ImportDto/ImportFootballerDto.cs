using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Common.ValidationConstants;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Footballer")]
public class ImportFootballerDto
{
	[Required]
	[StringLength(FootballerNameMaxLength, MinimumLength = FootballerNameMinLength)]
	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("ContractStartDate")]
	public string ContractStartDate { get; set; } = null!;

	[Required]
	[XmlElement("ContractEndDate")]
	public string ContractEndDate { get; set; } = null!;

	[Required]
	[XmlElement("BestSkillType")]
	[EnumDataType(typeof(BestSkillType))]
	public int BestSkillType { get; set; }

	[Required]
	[EnumDataType(typeof(PositionType))]
	[XmlElement("PositionType")]
	public int PositionType { get; set; }
}