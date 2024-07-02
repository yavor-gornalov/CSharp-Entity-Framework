using System.ComponentModel.DataAnnotations;
using static Footballers.Common.ValidationConstants;

namespace Footballers.Data.Models;

public class Team
{
	public Team()
	{
		TeamsFootballers = new HashSet<TeamFootballer>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(TeamNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(TeamNationalityMaxLength)]
	public string Nationality { get; set; } = null!;

	[Required]
	public int Trophies { get; set; }

	public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = null!;
}
