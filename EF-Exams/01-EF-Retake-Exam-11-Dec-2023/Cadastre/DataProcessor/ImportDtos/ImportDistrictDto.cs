using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("District")]
public class ImportDistrictDto
{
	[Required]
	[XmlAttribute("Region")]
	[EnumDataType(typeof(Region))]
	public Region Region { get; set; }

	[Required]
	[XmlElement("Name")]
	[StringLength(DistrictNameMaxLength, MinimumLength = DistrictNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("PostalCode")]
	[StringLength(DistrictPostalCodeLength)]
	[RegularExpression(DistrictPostalRegex)]
	public string PostalCode { get; set; } = null!;

	[XmlArray("Properties")]
	public ImportPropertyDto[] Properties { get; set; }
}
