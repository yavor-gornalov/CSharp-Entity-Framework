using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftJail.Common.GlobalConstants;

namespace SoftJail.Data.Models;

public class Officer
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(OfficerFullNameMaxLength)]
	public string FullName { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    [Required]
	public Position Position { get; set; }

	[Required]
	public Weapon Weapon { get; set; }

	[Required]
	public int DepartmentId { get; set; }

	[ForeignKey(nameof(DepartmentId))]
	public Department Department { get; set; } = null!;

	public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = new HashSet<OfficerPrisoner>();

}

//•	Id – integer, Primary Key
//•	FullName – text with min length 3 and max length 30 (required)
//•	Salary – decimal (non-negative, minimum value: 0) (required)
//•	Position – Position enumeration with possible values: "Overseer, Guard, Watcher, Labour" (required)
//•	Weapon – Weapon enumeration with possible values: "Knife, FlashPulse, ChainRifle, Pistol, Sniper" (required)
//•	DepartmentId – integer, foreign key(required)
//•	Department – the officer's department (required)
//•	OfficerPrisoners – collection of type OfficerPrisoner
