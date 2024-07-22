using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.GlobalConstants;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("Property")]
public class ImportPropertyDto
{
	[Required]
	[XmlElement("PropertyIdentifier")]
	[StringLength(PropertyIdentifierMaxLength, MinimumLength = PropertyIdentifierMinLength)]
	public string PropertyIdentifier { get; set; } = null!;

	[Required]
	[XmlElement]
	[Range(PropertyAreaLowLimit, PropertyAreaHighLimit)]
	public int Area { get; set; }

	[XmlElement("Details")]
	[StringLength(PropertyDetailsMaxLength, MinimumLength = PropertyDetailsMinLength)]
	public string? Details { get; set; }

	[Required]
	[XmlElement("Address")]
	[StringLength(PropertyAddressMaxLength, MinimumLength = PropertyAddressMinLength)]
	public string Address { get; set; } = null!;

	[Required]
	[XmlElement("DateOfAcquisition")]
	public string DateOfAcquisition { get; set; } = null!;
}

//•	Id – integer, Primary Key
//•	PropertyIdentifier – text with length[16, 20] (required)
//•	Area – int not negative(required)
//•	Details - text with length[5, 500] (not required)
//•	Address – text with length[5, 200] (required)
//•	DateOfAcquisition – DateTime(required)
//•	DistrictId – integer, foreign key(required)
//•	District – District
//•	PropertiesCitizens - collection of type PropertyCitizen