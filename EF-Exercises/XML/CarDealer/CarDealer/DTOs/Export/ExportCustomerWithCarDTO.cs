using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("customer")]
public class ExportCustomerWithCarDTO
{
	[XmlAttribute("full-name")]
	public string FullName { get; set; } = null!;

	[XmlAttribute("bought-cars")]
	public int CarsBoughtCount { get; set; }

	[XmlAttribute("spent-money")]
	public decimal TotalMoneySpent { get; set; }
}
