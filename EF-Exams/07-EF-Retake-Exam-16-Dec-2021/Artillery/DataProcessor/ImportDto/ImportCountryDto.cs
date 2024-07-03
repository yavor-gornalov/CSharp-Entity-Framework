using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Artillery.Common.GlobalConstants;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Country")]
public class ImportCountryDto
{
	[Required]
	[XmlElement("CountryName")]
	[MinLength(CountryNameMinLength)]
	[MaxLength(CountryNameMaxLength)]
	public string CountryName { get; set; } = null!;

	[Required]
	[XmlElement("ArmySize")]
	[Range(CountryArmySizeLowLimit, CountryArmySizeHighLimit)]
	public int ArmySize { get; set; }
}
