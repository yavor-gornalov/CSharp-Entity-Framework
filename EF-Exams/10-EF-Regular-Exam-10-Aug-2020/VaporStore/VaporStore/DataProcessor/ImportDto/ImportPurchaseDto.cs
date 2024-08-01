using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.DataProcessor.ImportDto;

[XmlType("Purchase")]
public class ImportPurchaseDto
{
	[Required]
	[XmlAttribute("title")]
	public string GameName { get; set; } = null!;

	[Required]
	[XmlElement("Type")]
	[EnumDataType(typeof(PurchaseType))]
	public string CardType { get; set; } = null!;

	[Required]
	[XmlElement("Key")]
	[StringLength(ProductKeyMaxLength)]
	[RegularExpression(ProductKeyRegex)]
	public string Key { get; set; } = null!;

	[Required]
	[XmlElement("Card")]
	[StringLength(CardNumberMaxLength)]
	[RegularExpression(CardNumberRegex)]
	public string CardNumer { get; set; } = null!;

	[Required]
	public string Date { get; set; } = null!;
}


