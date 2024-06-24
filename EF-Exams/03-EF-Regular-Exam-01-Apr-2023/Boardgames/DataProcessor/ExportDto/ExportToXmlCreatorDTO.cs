using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Creator")]
public class ExportToXmlCreatorDTO
{
    [XmlAttribute("BoardgamesCount")]
    public int BoardgamesCount { get; set; }

    [XmlElement("CreatorName")]
    public string CreatorName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public ExportToXmlBoardgameDTO[] Boardgames { get; set; }
}
