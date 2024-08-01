using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models;

public class GameTag
{
	[Required]
	public int GameId { get; set; }

	[ForeignKey(nameof(GameId))]
	public Game Game { get; set; } = null!;

	[Required]
	public int TagId { get; set; }

	[ForeignKey(nameof(TagId))]
	public Tag Tag { get; set; } = null!;

}

//•	GameId – integer, Primary Key, foreign key(required)
//•	Game – Game
//•	TagId – integer, Primary Key, foreign key(required)
//•	Tag – Tag

