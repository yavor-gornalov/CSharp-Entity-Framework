using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Footballers.Common.ValidationConstants;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Coach")]
public class ImportCoachDto
{
	[Required]
	[StringLength(CoachNameMaxLength, MinimumLength = CoachNameMinLength)]
	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("Nationality")]
	public string Nationality { get; set; } = null!;

	[XmlArray("Footballers")]
	public ImportFootballerDto[] Footballers { get; set; } = null!;
}
