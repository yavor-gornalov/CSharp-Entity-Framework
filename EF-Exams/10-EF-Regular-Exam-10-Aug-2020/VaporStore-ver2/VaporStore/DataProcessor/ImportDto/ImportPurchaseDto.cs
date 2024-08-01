using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;

using static VaporStore.Data.Validations.GlobalConstants;

namespace VaporStore.DataProcessor.ImportDto;

[XmlType("Purchase")]
public class ImportPurchaseDto
{
	[XmlAttribute("title")]
	public string GameTitle { get; set; } = null!;

	[XmlElement("Type")]
	[Required]
	[EnumDataType(typeof(PurchaseType))]
	public string Type { get; set; } = null!;

	[XmlElement("Key")]
	[Required]
	[RegularExpression(PurchaseProductKeyRegex)]
	public string ProductKey { get; set; } = null!;

	[XmlElement("Card")]
	[Required]
	[RegularExpression(CardNumberRegex)]
	public string CardNumber { get; set; } = null!;

	[XmlElement("Date")]
	[Required]
	public string Date { get; set; } = null!;
}
