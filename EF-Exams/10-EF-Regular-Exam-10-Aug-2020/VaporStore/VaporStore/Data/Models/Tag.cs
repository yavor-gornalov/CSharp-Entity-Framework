﻿using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models;

public class Tag
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
}
