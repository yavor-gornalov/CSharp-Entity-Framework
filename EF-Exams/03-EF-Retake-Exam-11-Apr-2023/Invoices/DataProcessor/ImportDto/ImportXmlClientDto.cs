using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.ValidationConstants;

namespace Invoices.DataProcessor.ImportDto;

[XmlType("Client")]
public class ImportXmlClientDto
{
	[Required]
	[XmlElement("Name")]
	[StringLength(maximumLength: ClientNameMaxLength, MinimumLength = ClientNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("NumberVat")]
	[StringLength(maximumLength: ClientVatMaxLength, MinimumLength = ClientNameMinLength)]
	public string NumberVat { get; set; } = null!;

	[XmlArray("Addresses")]
	public List<ImportXmlAddressDto> Addresses { get; set; } = new List<ImportXmlAddressDto>();
}