using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Theatre.Common.GlobalConstants;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Cast")]
public class ImportCastDto
{
	[Required]
	[XmlElement("FullName")]
	[StringLength(CastNameMaxLength, MinimumLength = CastNameMinLength)]
	public string FullName { get; set; } = null!;

	[Required]
	[XmlElement("IsMainCharacter")]
	public bool IsMainCharacter { get; set; }

	[Required]
	[XmlElement("PhoneNumber")]
	[RegularExpression(PhoneNumberRegex)]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	[XmlElement("PlayId")]
	public int PlayId { get; set; }
}
