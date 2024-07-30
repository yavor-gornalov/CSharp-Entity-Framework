using System.ComponentModel.DataAnnotations;

using static SoftJail.Common.GlobalConstants;

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

//•	Id – integer, Primary Key
//•	Name – text with min length 3 and max length 25 (required)
//•	Cells - collection of type Cell
