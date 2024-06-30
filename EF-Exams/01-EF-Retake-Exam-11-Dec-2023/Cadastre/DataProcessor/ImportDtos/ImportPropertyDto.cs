using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("Property")]
public class ImportPropertyDto
{
	[Required]
	[XmlElement("PropertyIdentifier")]
	[StringLength(PropertyIdentifierMaxLength, MinimumLength = PropertyIdentifierMinLength)]
	public string PropertyIdentifier { get; set; } = null!;

	[Required]
	[XmlElement("Area")]
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