using System.ComponentModel.DataAnnotations;
using static VaporStore.Data.Validations.GlobalConstants;

namespace VaporStore.DataProcessor.ImportDto;

public class ImportUserDto
{
	[Required]
	[RegularExpression(UserFullnameRegex)]
	public string FullName { get; set; } = null!;

	[Required]
	[StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
	public string Username { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Range(UserAgeLowLimit, UserAgeHighLimit)]
	public int Age { get; set; }

    public ImportCardDto[] Cards { get; set; }
}
