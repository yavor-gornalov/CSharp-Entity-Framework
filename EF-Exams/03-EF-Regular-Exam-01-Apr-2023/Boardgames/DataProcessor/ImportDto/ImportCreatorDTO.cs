using Boardgames.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using static Boardgames.Common.ValidationConstants;

namespace Boardgames.DataProcessor.ImportDto;

[XmlType("Creator")]
public class ImportCreatorDTO
{
	[XmlElement("FirstName")]
	[MaxLength(CreatorFirstNameMaxLength)]
	[MinLength(CreatorFirstNameMinLength)]
	public string FirstName { get; set; } = null!;


	[XmlElement("LastName")]
	[MinLength(CreatorLastNameMinLength)]
	[MaxLength(CreatorLastNameMaxLength)]
	public string LastName { get; set; } = null!;

	[XmlArray("Boardgames")]
	public ImportBoardgameDTO[] Boardgames { get; set; } = null!;
}
