using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Theatre.Common.GlobalConstants;

namespace Theatre.Data.Models;

public class Cast
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(CastNameMaxLength)]
	public string FullName { get; set; } = null!;

	[Required]
	public bool IsMainCharacter { get; set; }

	[Required]
	[MaxLength(PhoneNumberMaxLength)]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	[ForeignKey(nameof(Play))]
	public int PlayId { get; set; }
	public Play Play { get; set; } = null!;
}