using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;
using static Theatre.Common.GlobalConstants;

namespace Theatre.Data.Models;

public class Play
{
	public Play()
	{
		Casts = new HashSet<Cast>();
		Tickets = new HashSet<Ticket>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(PlayTitleMaxLength)]
	public string Title { get; set; } = null!;

	[Required]
	public TimeSpan Duration { get; set; }

	[Required]
	public float Rating { get; set; }

	[Required]
	public Genre Genre { get; set; }

	[Required]
	[MaxLength(PlayDescriptionMaxLength)]
	public string Description { get; set; } = null!;

	[Required]
	[MaxLength(PlayScreenwriterMaxLength)]
	public string Screenwriter { get; set; } = null!;

	public ICollection<Cast> Casts { get; set; }

	public ICollection<Ticket> Tickets { get; set; }
}