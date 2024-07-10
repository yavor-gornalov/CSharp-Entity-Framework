using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.DataProcessor.ImportDto;

public class ImportGameDto
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	[Range(GamePriceLowLimit, GamePriceHighLimit)]
	public decimal Price { get; set; }

	[Required]
	public string ReleaseDate { get; set; } = null!;

	[Required]
	public string Developer { get; set; } = null!;

	[Required]
	public string Genre { get; set; } = null!;

	[Required]
	public string[] Tags { get; set; }

}
