using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models;

public class PropertyCitizen
{
	[Required]
	public int PropertyId { get; set; }

	[Required]
	[ForeignKey(nameof(PropertyId))]
	public Property Property { get; set; } = null!;

	[Required]
	public int CitizenId { get; set; }

	[Required]
	[ForeignKey(nameof(CitizenId))]
	public Citizen Citizen { get; set; } = null!;
}

//•	PropertyId – integer, Primary Key, foreign key(required)
//•	Property – Property
//•	CitizenId – integer, Primary Key, foreign key(required)
//•	Citizen – Citizen
