using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Theatre.Common.GlobalConstants;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Play")]
public class ImportPlayDto
{
	[Required]
	[XmlElement("Title")]
	[StringLength(PlayTitleMaxLength, MinimumLength = PlayTitleMinLength)]
	public string Title { get; set; } = null!;

	[Required]
	[XmlElement("Duration")]
	public string Duration { get; set; } = null!;

	[Required]
	[XmlElement("Raiting")]
	[Range(PlayRatingLowLimit, PlayRatingHighLimit)]
	public float Rating { get; set; }

	[Required]
	[XmlElement("Genre")]
	public string Genre { get; set; } = null!;

	[Required]
	[XmlElement("Description")]
	[StringLength(PlayDescriptionMaxLength, MinimumLength = PlayDescriptionMinLength)]
	public string Description { get; set; } = null!;

	[Required]
	[XmlElement("Screenwriter")]
	[StringLength(PlayScreenwriterMaxLength, MinimumLength = PlayScreenwriterMinLength)]
	public string Screenwriter { get; set; } = null!;
}
