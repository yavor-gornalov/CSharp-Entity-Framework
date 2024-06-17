using System.Xml;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("User")]
public class ExportUsersSoldInfoDTO
{

	[XmlElement("firstName")]
	public string FirstName { get; set; } = null!;

	[XmlElement("lastName")]
	public string LastName { get; set; } = null!;

	[XmlElement("age")]
	public int? Age { get; set; }

	[XmlElement("SoldProducts")]
	public UserSoldProductsDTO SoldProducts { get; set; } = null!;

}
public class UserSoldProductsDTO
{
	public UserSoldProductsDTO()
	{
		Products = new List<UserProductDetailsDTO>();
	}

	[XmlElement("count")]
	public int Count { get; set; }

	[XmlArray("products")]
	[XmlArrayItem("Product")]
	public List<UserProductDetailsDTO> Products { get; set; }
}

public class UserProductDetailsDTO
{
	[XmlElement("name")]
	public string Name { get; set; } = null!;

	[XmlElement("price")]
	public decimal Price { get; set; }
}


