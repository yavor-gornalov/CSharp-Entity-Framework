﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models;

public class GameTag
{
	[Required]
	[ForeignKey(nameof(Game))]
	public int GameId { get; set; }
	public Game Game { get; set; } = null!;

	[Required]
	[ForeignKey(nameof(Tag))]
	public int TagId { get; set; }
	public Tag Tag { get; set; } = null!;
}