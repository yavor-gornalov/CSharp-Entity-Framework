//using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Theatre.Common.GlobalConstants;

namespace Theatre.DataProcessor.ImportDto;

public class ImportTheatreDto
{
	[Required]
	[JsonPropertyName("Name")]
	[StringLength(TheatreNameMaxLength, MinimumLength = TheatreDirectorMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonPropertyName("NumberOfHalls")]
	[Range(NumberOfHallsLowLimit, NumberOfHallsHighLimit)]
	public sbyte NumberOfHalls { get; set; }

	[Required]
	[JsonPropertyName("Director")]
	[StringLength(TheatreDirectorMaxLength, MinimumLength = TheatreDirectorMinLength)]
	public string Director { get; set; } = null!;

	[Required]
	[JsonPropertyName("Tickets")]
	public ImportTicketDto[] Tickets { get; set; }
}
