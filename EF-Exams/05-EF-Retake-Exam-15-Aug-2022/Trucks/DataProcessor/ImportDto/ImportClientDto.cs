using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Trucks.Common.ValidationConstants;

namespace Trucks.DataProcessor.ImportDto;

public class ImportClientDto
{
	[Required]
	[JsonProperty("Name")]
	[StringLength(ClientNameMaxLength, MinimumLength = ClientNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[JsonProperty("Nationality")]
	[StringLength(NationalityMaxLength, MinimumLength = NationalityMinLength)]
	public string Nationality { get; set; } = null!;

	[Required]
	[JsonProperty("Type")]
	public string Type { get; set; } = null!;


	[JsonProperty("Trucks")]
	public int[] TruckIds { get; set; } = null!;
}
