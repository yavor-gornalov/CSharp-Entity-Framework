using System.ComponentModel.DataAnnotations;
using static Theatre.Common.GlobalConstants;

namespace Theatre.Data.Models;

public class Theatre
{
	public Theatre()
	{
		Tickets = new HashSet<Ticket>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(TheatreNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	public sbyte NumberOfHalls { get; set; }

	[Required]
	[MaxLength(TheatreDirectorMaxLength)]
	public string Director { get; set; } = null!;

	public ICollection<Ticket> Tickets { get; set; }
}
