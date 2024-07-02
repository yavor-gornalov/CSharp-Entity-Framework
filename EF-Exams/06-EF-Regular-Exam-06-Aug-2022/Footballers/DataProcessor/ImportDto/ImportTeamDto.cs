using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Footballers.Common.ValidationConstants;

namespace Footballers.DataProcessor.ImportDto;

public class ImportTeamDto
{
	public ImportTeamDto()
	{
		FootballersIds = new HashSet<int>();
	}

	[Required]
	[JsonProperty("Name")]
	[RegularExpression(TeamNameRegex)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonProperty("Nationality")]
	[StringLength(TeamNationalityMaxLength, MinimumLength = TeamNationalityMinLength)]
	public string Nationality { get; set; } = null!;

	[Required]
	[JsonProperty("Trophies")]
	[Range(TeamTrophiesLowLimit, TeamTrophiesHighLimit)]
	public int Trophies { get; set; }

	[JsonProperty("Footballers")]
	public ICollection<int> FootballersIds { get; set; }
}
