using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SoftJail.Common.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportMailDto
{
	[Required]
	public string Description { get; set; } = null!;

	[Required]
	public string Sender { get; set; } = null!;

	[Required]
	[RegularExpression(MailAddressRegex)]
	public string Address { get; set; } = null!;
}