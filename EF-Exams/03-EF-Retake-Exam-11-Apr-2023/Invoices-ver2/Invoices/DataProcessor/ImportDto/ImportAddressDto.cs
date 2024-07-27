using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto;

[XmlType("Address")]
public class ImportAddressDto
{
	[XmlElement("StreetName")]
	[Required]
	[StringLength(AddressStreetNameMaxLength, MinimumLength = AddressStreetNameMinLength)]
	public string StreetName { get; set; } = null!;

	[XmlElement("StreetNumber")]
	[Required]
	public int StreetNumber { get; set; }

	[XmlElement("PostCode")]
	[Required]
	public string PostCode { get; set; } = null!;

	[XmlElement("City")]
	[Required]
	[StringLength(AddressCityMaxLength, MinimumLength = AddressCityMinLength)]
	public string City { get; set; } = null!;

	[XmlElement("Country")]
	[Required]
	[StringLength(AddressCountryMaxLength, MinimumLength = AddressCountryMinLength)]
	public string Country { get; set; } = null!;
}

//•	Id – integer, Primary Key
//•	StreetName – text with length[10…20] (required)
//•	StreetNumber – integer(required)
//•	PostCode – text(required)
//•	City – text with length[5…15] (required)
//•	Country – text with length[5…15] (required)
//•	ClientId – integer, foreign key(required)
//•	Client – Client