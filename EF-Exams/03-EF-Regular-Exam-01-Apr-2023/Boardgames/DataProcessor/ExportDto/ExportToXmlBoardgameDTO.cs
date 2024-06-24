using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Boardgame")]
public class ExportToXmlBoardgameDTO
{
	[XmlElement("BoardgameName")]
	public string Name { get; set; } = null!;

	[XmlElement("BoardgameYearPublished")]
	public int YearPublished { get; set; }
}
