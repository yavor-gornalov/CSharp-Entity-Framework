using Invoices.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using static Invoices.Common.ValidationConstants;

namespace Invoices.DataProcessor.ImportDto;

public class ImportJsonProductDto
{
	[Required]
	[JsonProperty("Name")]
	[StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonProperty("Price")]
	[Range((double)ProductPriceLowLimit, (double)ProductPriceHighLimit)]
	public decimal Price { get; set; }

	[Required]
	[JsonProperty("CategoryType")]
	[EnumDataType(typeof(CategoryType))]
	public CategoryType CategoryType { get; set; }

	[JsonProperty("Clients")]
	public int[] ClientIds { get; set; } = null!;
}
