using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VaporStore.Data.Validations.GlobalConstants;

namespace VaporStore.Data.Models;

public class Game
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	[Required]
	[Column(TypeName = ("decimal(18,2)"))]
	public decimal Price { get; set; }

	[Required]
	public DateTime ReleaseDate { get; set; }

	[Required]
	public int DeveloperId { get; set; }

	[ForeignKey(nameof(DeveloperId))]
	public Developer Developer { get; set; } = null!;

	[Required]
	public int GenreId { get; set; }

	[ForeignKey(nameof(GenreId))]
	public Genre Genre { get; set; } = null!;

	public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

	public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
}

//•	Id – integer, Primary Key
//•	Name – text(required)
//•	Price – decimal (non-negative, minimum value: 0) (required)
//•	ReleaseDate – Date(required)
//•	DeveloperId – integer, foreign key(required)
//•	Developer – the game's developer (required)
//•	GenreId – integer, foreign key(required)
//•	Genre – the game's genre (required)
//•	Purchases - collection of type Purchase
//•	GameTags - collection of type GameTag.Each game must have at least one tag.

