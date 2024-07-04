using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Theatre.Common.GlobalConstants;

namespace Theatre.DataProcessor.ImportDto;
public class ImportTicketDto
{
	[JsonProperty("Price")]
	[Range(TicketPriceLowLimit, TicketPriceHighLimit)]
	public decimal Price { get; set; }

	[JsonProperty("RowNumber")]
	[Range(TicketRowNumberLowLimit, TicketRowNumberHighLimit)]
	public sbyte RowNumber { get; set; }


	[JsonProperty("PlayId")]
	public int PlayId { get; set; }
}