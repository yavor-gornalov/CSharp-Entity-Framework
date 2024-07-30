using System.ComponentModel.DataAnnotations;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportPrisonerDto
{
	[Required]
	[StringLength(PrisonerFullNameMaxLength, MinimumLength = PrisonerFullNameMinLength)]
	public string FullName { get; set; } = null!;

	[Required]
	[RegularExpression(PrisonerNicknameRegex)]
	public string Nickname { get; set; } = null!;

	[Required]
	[Range(PrisonerAgeLowLimit, PrisonerAgeHighLimit)]
	public int Age { get; set; }

	[Required]
	public string IncarcerationDate { get; set; } = null!;

	public string? ReleaseDate { get; set; }

	[Range(PrisonerBailLowLimit, PrisonerBailHighLimit)]
	public decimal? Bail { get; set; }

	public int? CellId { get; set; }

	public ImportMailDto[] Mails { get; set; }
}
