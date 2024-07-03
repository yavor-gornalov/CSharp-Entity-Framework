using Artillery.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Artillery.Common.GlobalConstants;

namespace Artillery.Data.Models;

public class Gun
{
	public Gun()
	{
		CountriesGuns = new HashSet<CountryGun>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[ForeignKey(nameof(Manufacturer))]
	public int ManufacturerId { get; set; }
	public Manufacturer Manufacturer { get; set; } = null!;

	[Required]
	public int GunWeight { get; set; }

	[Required]
	public double BarrelLength { get; set; }

	public int? NumberBuild { get; set; }

	[Required]
	public int Range { get; set; }

	[Required]
	public GunType GunType { get; set; }

	[Required]
	[ForeignKey(nameof(Shell))]
	public int ShellId { get; set; }
	public Shell Shell { get; set; } = null!;

	public ICollection<CountryGun> CountriesGuns { get; set; }
}