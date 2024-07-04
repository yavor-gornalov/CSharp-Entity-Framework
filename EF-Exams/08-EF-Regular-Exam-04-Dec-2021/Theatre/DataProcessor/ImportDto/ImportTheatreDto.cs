using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Theatre.Common.GlobalConstants;

namespace Theatre.DataProcessor.ImportDto;

public class ImportTheatreDto
{
	[Required]
	[JsonProperty("Name")]
	[StringLength(TheatreNameMaxLength, MinimumLength = TheatreDirectorMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonProperty("NumberOfHalls")]
	[Range(NumberOfHallsLowLimit, NumberOfHallsHighLimit)]
	public sbyte NumberOfHalls { get; set; }

	[Required]
	[JsonProperty("Director")]
	[StringLength(TheatreDirectorMaxLength, MinimumLength = TheatreDirectorMinLength)]
	public string Director { get; set; } = null!;

	[Required]
	[JsonProperty("Tickets")]
	public ImportTicketDto[] Tickets { get; set; }
}
