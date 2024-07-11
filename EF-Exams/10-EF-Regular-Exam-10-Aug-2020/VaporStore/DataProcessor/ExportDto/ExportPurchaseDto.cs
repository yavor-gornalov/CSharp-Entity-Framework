using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto;

[XmlType("Purchase")]
public class ExportPurchaseDto
{
	[XmlElement("Card")]
	public string CardNumber { get; set; } = null!;

	[XmlElement("Cvc")]
	public string CardCvc { get; set; } = null!;

	[XmlElement("Date")]
	public string Date { get; set; } = null!;

	[XmlElement("Game")]
	public ExportGameDto Game { get; set; } = null!;
}