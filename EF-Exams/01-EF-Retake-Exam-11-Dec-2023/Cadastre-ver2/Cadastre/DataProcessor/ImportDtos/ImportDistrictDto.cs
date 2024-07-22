using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.GlobalConstants;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("District")]
public class ImportDistrictDto
{
	[Required]
	[XmlAttribute("Region")]
	[EnumDataType(typeof(Region))]
	public string Region { get; set; } = null!;

	[Required]
	[XmlElement("Name")]
	[StringLength(DistrictNameMaxLength, MinimumLength = DistrictNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("PostalCode")]
	[StringLength(DistrictPostalCodeMaxLength)]
	[RegularExpression(DistrictPostalCodeRegex)]
	public string PostalCode { get; set; } = null!;

	[XmlArray("Properties")]
	public ImportPropertyDto[] Properties { get; set; } = null!;
}

//•	Id – integer, Primary Key
//•	Name – text with length[2, 80] (required)
//•	PostalCode – text with length 8. All postal codes must have the following structure:starting with two capital letters, followed by e dash '-', followed by five digits.Example: SF-10000 (required)
//•	Region – Region enum (SouthEast = 0, SouthWest, NorthEast, NorthWest) (required)
//•	Properties - collection of type Property