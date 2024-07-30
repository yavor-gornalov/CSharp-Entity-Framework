using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftJail.Common.GlobalConstants;

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
	public int DepartmentId { get; set; }

	[ForeignKey(nameof(DepartmentId))]
	public Department Department { get; set; } = null!;

	public ICollection<Prisoner> Prisoners { get; set; } = new HashSet<Prisoner>();
}

//•	Id – integer, Primary Key
//•	CellNumber – integer in the range[1, 1000] (required)
//•	HasWindow – bool (required)
//•	DepartmentId – integer, foreign key(required)
//•	Department – the cell's department (required)
//•	Prisoners – collection of type Prisoner
