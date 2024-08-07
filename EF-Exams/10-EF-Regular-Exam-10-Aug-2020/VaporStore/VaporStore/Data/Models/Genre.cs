﻿using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models;

public class Genre
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	public ICollection<Game> Games { get; set; } = new HashSet<Game>();
}