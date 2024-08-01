using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto;

[XmlType("User")]
public class ExportUserDto
{
	[XmlAttribute("username")]
	public string Username { get; set; } = null!;

	[XmlArray("Purchases")]
	public ExportPurchaseDto[] Purchases { get; set; }

    [XmlElement("TotalSpent")]
    public decimal TotalSpent { get; set; }
}
