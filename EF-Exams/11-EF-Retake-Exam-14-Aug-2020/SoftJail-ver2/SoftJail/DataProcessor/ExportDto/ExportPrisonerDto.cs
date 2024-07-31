using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto;

[XmlType("Prisoner")]
public class ExportPrisonerDto
{
	[XmlElement("Id")]
	public int Id { get; set; }

	[XmlElement("Name")]
	public string FullName { get; set; } = null!;

	[XmlElement("IncarcerationDate")]
	public string IncarcerationDate { get; set; } = null!;

	[XmlArray("EncryptedMessages")]
	public ExportMessageDto[] Messages { get; set; }
}
