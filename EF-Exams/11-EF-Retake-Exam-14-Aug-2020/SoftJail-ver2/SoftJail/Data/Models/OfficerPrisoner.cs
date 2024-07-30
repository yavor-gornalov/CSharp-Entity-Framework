using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class OfficerPrisoner
{
	[Required]
	public int PrisonerId { get; set; }

	[ForeignKey(nameof(PrisonerId))]
	public Prisoner Prisoner { get; set; } = null!;

	[Required]
	public int OfficerId { get; set; }

	[ForeignKey(nameof(OfficerId))]
	public Officer Officer { get; set; } = null!;
}

//•	PrisonerId – integer, Primary Key
//•	Prisoner – the officer's prisoner (required)
//•	OfficerId – integer, Primary Key
//•	Officer – the prisoner's officer (required)
