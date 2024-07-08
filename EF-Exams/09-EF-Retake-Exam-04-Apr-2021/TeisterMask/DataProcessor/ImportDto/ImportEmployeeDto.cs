using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static TeisterMask.Common.GlobalConstants;

namespace TeisterMask.DataProcessor.ImportDto;

public class ImportEmployeeDto
{
	[Required]
	[JsonPropertyName("Username")]
	[StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
	[RegularExpression(UsernameRegex)]
	public string Username { get; set; } = null!;

	[Required]
	[JsonPropertyName("Email")]
	[StringLength(EmailMaxLength)]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	[JsonPropertyName("Phone")]
	[StringLength(PhoneNumberLength, MinimumLength = PhoneNumberLength)]
	[RegularExpression(PhoneNumberRegex)]
	public string Phone { get; set; } = null!;

	[JsonPropertyName("Tasks")]
	public int[]? TaskIds { get; set; }
}
