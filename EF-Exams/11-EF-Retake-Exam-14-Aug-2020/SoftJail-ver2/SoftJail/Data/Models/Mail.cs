using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models;

public class Mail
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string Description { get; set; } = null!;

	[Required]
	public string Sender { get; set; } = null!;

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	public int PrisonerId { get; set; }

	[ForeignKey(nameof(PrisonerId))]
	public Prisoner Prisoner { get; set; } = null!;
}

//•	Id – integer, Primary Key
//•	Description – text(required)
//•	Sender – text(required)
//•	Address – text, consisting only of letters, spaces and numbers, which ends with "str." (required) (Example: "62 Muir Hill str.")
//•	PrisonerId - integer, foreign key(required)
//•	Prisoner – the mail's Prisoner (required)
