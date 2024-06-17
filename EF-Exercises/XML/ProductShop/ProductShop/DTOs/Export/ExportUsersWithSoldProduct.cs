using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

public class ExportUsersWithSoldProduct
{
	public ExportUsersWithSoldProduct()
	{
		Users = new List<ExportUsersSoldInfoDTO>();
	}

	[XmlElement("count")]
	public int Count { get; set; }

	[XmlArray("users")]
	public List<ExportUsersSoldInfoDTO> Users { get; set; }

}
