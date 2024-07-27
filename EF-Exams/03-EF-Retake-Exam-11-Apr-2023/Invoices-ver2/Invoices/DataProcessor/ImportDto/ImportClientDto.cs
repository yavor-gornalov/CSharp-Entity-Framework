using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Common.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto;

[XmlType("Client")]
public class ImportClientDto
{
	[XmlElement("Name")]
	[Required]
	[StringLength(ClientNameMaxLength, MinimumLength = ClientNameMinLength)]
	public string Name { get; set; } = null!;

	[XmlElement("NumberVat")]
	[Required]
	[StringLength(ClientNumberVatMaxLength, MinimumLength = ClientNumberVatMinLength)]
	public string NumberVat { get; set; } = null!;

	[XmlArray("Addresses")]
	public ImportAddressDto[] Addresses { get; set; }
}

//•	Id – integer, Primary Key
//•	Name – text with length[10…25] (required)
//•	NumberVat – text with length[10…15] (required)
//•	Invoices – collection of type Invoicе
//•	Addresses – collection of type Address
//•	ProductsClients – collection of type ProductClient