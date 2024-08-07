﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.Data.Models;

public class Game
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public decimal Price { get; set; }

	[Required]
	public DateTime ReleaseDate { get; set; }

	[Required]
	[ForeignKey(nameof(Developer))]
	public int DeveloperId { get; set; }
	public Developer Developer { get; set; } = null!;

	[Required]
	[ForeignKey(nameof(Genre))]
	public int GenreId { get; set; }
	public Genre Genre { get; set; } = null!;

	public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

	public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
}
