using System.ComponentModel.DataAnnotations;
using static SoftJail.Shared.GlobalConstants;

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