using Invoices.Data.Models.Enums;
using Newtonsoft.Json;

namespace Invoices.DataProcessor.ExportDto;

public class ExportJsonProductDto
{
	[JsonProperty("Name")]
	public string Name { get; set; } = null!;

	[JsonProperty("Price")]
	public decimal Price { get; set; }

	[JsonProperty("Category")]
	public string CategoryType { get; set; } = null!;

	[JsonProperty("Clients")]
	public List<ExportJsonClientDto> Clients { get; set; } = new List<ExportJsonClientDto>();
}
