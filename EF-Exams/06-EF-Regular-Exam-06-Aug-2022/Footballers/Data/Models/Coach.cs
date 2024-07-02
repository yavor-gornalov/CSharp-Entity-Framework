﻿using System.ComponentModel.DataAnnotations;
using static Footballers.Common.ValidationConstants;

namespace Footballers.Data.Models;

public class Coach
{
	public Coach()
	{
		Footballers = new HashSet<Footballer>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(CoachNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	public string Nationality { get; set; } = null!;

	public virtual ICollection<Footballer> Footballers { get; set; } = null!;
}