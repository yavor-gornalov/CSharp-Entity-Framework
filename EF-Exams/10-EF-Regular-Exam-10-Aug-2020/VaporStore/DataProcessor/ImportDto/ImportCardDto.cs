using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.DataProcessor.ImportDto;

public class ImportCardDto
{
	[Required]
	[RegularExpression(CardNumberRegex)]
	public string Number { get; set; } = null!;

	[Required]
	[RegularExpression(CardCvcRegex)]
	public string Cvc { get; set; } = null!;

	[Required]
	[EnumDataType(typeof(CardType))]
	public string Type { get; set; } = null!;
}