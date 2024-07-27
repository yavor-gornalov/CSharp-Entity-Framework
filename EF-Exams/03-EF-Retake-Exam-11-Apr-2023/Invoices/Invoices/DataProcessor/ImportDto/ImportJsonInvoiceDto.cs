using Invoices.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using static Invoices.Common.ValidationConstants;

namespace Invoices.DataProcessor.ImportDto;

public class ImportJsonInvoiceDto
{

	[Required]
	[Range(minimum: InvoiceNumberLowLimit, maximum: InvoiceNumberHighLimit)]
	[JsonProperty("Number")]
	public int Number { get; set; }

	[Required]
	[JsonProperty("IssueDate")]
	public string IssueDate { get; set; } = null!;

	[Required]
	[JsonProperty("DueDate")]
	public string DueDate { get; set; } = null!;

	[Required]
	[JsonProperty("Amount")]
	[Range(minimum: 0, maximum: double.MaxValue)]
	public decimal Amount { get; set; }

	[Required]
	[JsonProperty("CurrencyType")]
	[EnumDataType(typeof(CurrencyType))]
	public CurrencyType CurrencyType { get; set; }

	[JsonProperty("ClientId")]
	public int ClientId { get; set; }
}
