using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.Data.Models;

public class Officer
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(OfficerFullNameMaxLength)]
	public string FullName { get; set; } = null!;

	[Required]
	public decimal Salary { get; set; }

	[Required]
	public Position Position { get; set; }

	[Required]
	public Weapon Weapon { get; set; }

	[Required]
	[ForeignKey(nameof(Department))]
	public int DepartmentId { get; set; }
	public Department Department { get; set; } = null!;

	public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = new HashSet<OfficerPrisoner>();
}
