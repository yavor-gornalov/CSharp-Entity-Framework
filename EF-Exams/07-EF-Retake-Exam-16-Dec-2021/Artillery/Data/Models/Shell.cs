﻿using System.ComponentModel.DataAnnotations;
using static Artillery.Common.GlobalConstants;

namespace Artillery.Data.Models;

public class Shell
{
	public Shell()
	{
		Guns = new HashSet<Gun>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	public double ShellWeight { get; set; }

	[Required]
	[MaxLength(ShellCaliberMaxLength)]
	public string Caliber { get; set; } = null!;

	public ICollection<Gun> Guns { get; set; }
}
