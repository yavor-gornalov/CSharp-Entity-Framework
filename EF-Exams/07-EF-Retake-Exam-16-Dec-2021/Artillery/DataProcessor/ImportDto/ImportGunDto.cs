using Artillery.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using static Artillery.Common.GlobalConstants;

namespace Artillery.DataProcessor.ImportDto;

public class ImportGunDto
{
	[Required]
	[JsonProperty("ManufacturerId")]
	public int ManufacturerId { get; set; }

	[Required]
	[JsonProperty("GunWeight")]
	[Range(GunWeightLowLimit, GunWeightHighLimit)]
	public int GunWeight { get; set; }

	[Required]
	[JsonProperty("BarrelLength")]
	[Range(GunBarrelLengthLowLimit, GunBarrelLengthHighLimit)]
	public double BarrelLength { get; set; }

	[JsonProperty("NumberBuild")]
	public int? NumberBuild { get; set; }

	[Required]
	[JsonProperty("Range")]
	[Range(GunRangeLowLimit, GunRangeHighLimit)]
	public int GunRange { get; set; }

	[Required]
	[JsonProperty("GunType")]
	[EnumDataType(typeof(GunType))]
	public string GunType { get; set; }

	[Required]
	[JsonProperty("ShellId")]
	public int ShellId { get; set; }

	[JsonProperty("Countries")]
	public ImportCoutryIdDto[] CountriesIds { get; set; } = null!;

}
