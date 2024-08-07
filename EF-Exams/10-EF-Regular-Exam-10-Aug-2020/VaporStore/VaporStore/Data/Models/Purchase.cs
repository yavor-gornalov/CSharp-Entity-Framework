﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.Data.Models;

public class Purchase
{
	[Key]
	public int Id { get; set; }

	[Required]
	public PurchaseType Type { get; set; }

	[Required]
	[MaxLength(ProductKeyMaxLength)]
	public string ProductKey { get; set; } = null!;

	[Required]
	public DateTime Date { get; set; }

	[Required]
	[ForeignKey(nameof(Card))]
	public int CardId { get; set; }
	public Card Card { get; set; } = null!;

	[Required]
	[ForeignKey(nameof(Game))]
	public int GameId { get; set; }
	public Game Game { get; set; } = null!;
}