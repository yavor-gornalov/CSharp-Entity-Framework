using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models;

public class CountryGun
{
	// composite primary key defined in fluent api

	[Required]
	[ForeignKey(nameof(Country))]
	public int CountryId { get; set; }
	public Country Country { get; set; } = null!;

	[Required]
	[ForeignKey(nameof(Gun))]
	public int GunId { get; set; }
	public Gun Gun { get; set; } = null!;
}