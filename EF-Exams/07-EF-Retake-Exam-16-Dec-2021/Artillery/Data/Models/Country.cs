using System.ComponentModel.DataAnnotations;
using static Artillery.Common.GlobalConstants;

namespace Artillery.Data.Models;

public class Country
{
	public Country()
	{
		CountriesGuns = new HashSet<CountryGun>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(CountryNameMaxLength)]
	public string CountryName { get; set; } = null!;

	[Required]
	public int ArmySize { get; set; }

	public ICollection<CountryGun> CountriesGuns { get; set; }
}
