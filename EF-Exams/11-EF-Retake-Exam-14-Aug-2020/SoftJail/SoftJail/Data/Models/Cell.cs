﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class Cell
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int CellNumber { get; set; }

	[Required]
	public bool HasWindow { get; set; }

	[Required]
	[ForeignKey(nameof(Department))]
	public int DepartmentId { get; set; }
	public Department Department { get; set; } = null!;

	public ICollection<Prisoner> Prisoners { get; set; } = new HashSet<Prisoner>();
}