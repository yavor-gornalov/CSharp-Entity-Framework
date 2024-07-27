using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.ValidationConstants;

namespace Invoices.DataProcessor.ImportDto;

[XmlType("Address")]
public class ImportXmlAddressDto
{
	[Required]
	[XmlElement("StreetName")]
	[StringLength(StreetNameMaxLength, MinimumLength = StreetNameMinLength)]
	public string StreetName { get; set; } = null!;

	[Required]
	[XmlElement("StreetNumber")]
	public int StreetNumber { get; set; }

	[Required]
	[XmlElement("PostCode")]
	public string PostCode { get; set; } = null!;

	[Required]
	[XmlElement("City")]
	[StringLength(CityNameMaxLength, MinimumLength = CityNameMinLength)]
	public string City { get; set; } = null!;

	[Required]
	[XmlElement("Country")]
	[StringLength(CountryNameMaxLength, MinimumLength = CountryNameMinLength)]
	public string Country { get; set; } = null!;
}