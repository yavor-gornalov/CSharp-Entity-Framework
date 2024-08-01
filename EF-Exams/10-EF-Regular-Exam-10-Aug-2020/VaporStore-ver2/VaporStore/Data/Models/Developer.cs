using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models;

public class Developer
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	public ICollection<Game> Games { get; set; } = new HashSet<Game>();
}

//•	Id – integer, Primary Key
//•	Name – text(required)
//•	Games - collection of type Game
