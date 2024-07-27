using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Common.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto;

public class ImportProductDto
{
	[Required]
	[StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[Range(typeof(decimal), ProductPriceLowLimit, ProductPriceHighLimit)]
	public decimal Price { get; set; }

	[Required]
	[EnumDataType(typeof(CategoryType))]
	public int CategoryType { get; set; }

	public ICollection<int> Clients { get; set; } = new HashSet<int>();
}
