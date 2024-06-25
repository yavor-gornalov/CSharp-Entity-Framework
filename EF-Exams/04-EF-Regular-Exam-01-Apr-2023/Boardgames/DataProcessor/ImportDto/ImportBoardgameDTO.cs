using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using static Boardgames.Common.ValidationConstants;

namespace Boardgames.DataProcessor.ImportDto;

[XmlType("Boardgame")]
public class ImportBoardgameDTO
{
	[XmlElement("Name")]
	[MinLength(BoargameNameMinLength)]
	[MaxLength(BoargameNameMaxLength)]
	public string Name { get; set; } = null!;

	[XmlElement("Rating")]
	[Range(BoargameRatingLowLimit, BoargameRatingHighLimit)]
	public double Rating { get; set; }

	[XmlElement("YearPublished")]
	[Range(BoargameYearLowLimit, BoargameYearHighLimit)]
	public int YearPublished { get; set; }

	[XmlElement("CategoryType")]
	public int CategoryType { get; set; }

	[XmlElement("Mechanics")]
	public string Mechanics { get; set; } = null!;
}