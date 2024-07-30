using System.ComponentModel.DataAnnotations;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.Data.Models;

public class Department
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(DepartmentNameMaxLength)]
	public string Name { get; set; } = null!;

	public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
}