using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Boardgames.Common.ValidationConstants;

namespace Boardgames.DataProcessor.ImportDto;

public class ImportSellerDTO
{
	[Required]
	[JsonProperty("Name")]
	[MinLength(SellerNameMinLength)]
	[MaxLength(SellerNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonProperty("Address")]
	[MinLength(SellerAddressMinLength)]
	[MaxLength(SellerAddressMaxLength)]
	public string Address { get; set; } = null!;

	[Required]
	[JsonProperty("Country")]
	public string Country { get; set; } = null!;

	[Required]
	[JsonProperty("Website")]
	[RegularExpression(SellerWebsiteRegex)]
	public string Website { get; set; } = null!;

	[JsonProperty("Boardgames")]
	public int[] GameIds { get; set; } = null!;
}
