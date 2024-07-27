using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Invoices.Common.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto;

public class ImportInvoiceDto
{
	[Required]
	[Range(InvoiceNumberLowLimit, InvoiceNumberHighLimit)]
	public int Number { get; set; }

	[Required]
	public string IssueDate { get; set; } = null!;

	[Required]
	public string DueDate { get; set; } = null!;

	[Required]
	[Range(InvoiceAmountLowLimit, InvoiceAmountHighLimit)]
	public decimal Amount { get; set; }

	[Required]
	[EnumDataType(typeof(CurrencyType))]
	public int CurrencyType { get; set; }

	[Required]
	public int ClientId { get; set; }
}

//•	Id – integer, Primary Key
//•	Number – integer in range[1, 000, 000, 000…1, 500, 000, 000] (required)
//•	IssueDate – DateTime(required)
//•	DueDate – DateTime(required)
//•	Amount – decimal (required)
//•	CurrencyType – enumeration of type CurrencyType, with possible values(BGN, EUR, USD) (required)
//•	ClientId – integer, foreign key(required)
//•	Client – Client