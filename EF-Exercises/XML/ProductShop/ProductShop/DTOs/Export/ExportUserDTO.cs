using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("User")]
public class ExportUserDTO
{
	public ExportUserDTO()
	{
		ProductsSold = new List<ExportSoldProductDTO>();
	}

	[XmlElement("firstName")]
	public string FirstName { get; set; } = null!;

	[XmlElement("lastName")]
	public string LastName { get; set; } = null!;

	[XmlArray("soldProducts")]
	public List<ExportSoldProductDTO> ProductsSold { get; set; }
}
