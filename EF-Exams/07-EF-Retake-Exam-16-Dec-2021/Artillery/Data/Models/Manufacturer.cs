using System.ComponentModel.DataAnnotations;
using static Artillery.Common.GlobalConstants;

namespace Artillery.Data.Models;

public class Manufacturer
{
	public Manufacturer()
	{
		Guns = new HashSet<Gun>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(ManufacturerNameMaxLength)]
	// Unique, declared in fluent api
	public string ManufacturerName { get; set; } = null!;

	[Required]
	[MaxLength(ManufacturerFoundedMaxLength)]
	public string Founded { get; set; } = null!;

	public ICollection<Gun> Guns { get; set; }
}
