using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.Data.Models;

public class Card
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(CardNumberMaxLength)]
	public string Number { get; set; } = null!;

	[Required]
	[MaxLength(CardCvcMaxLength)]
	public string Cvc { get; set; } = null!;

	[Required]
	public CardType Type { get; set; }

	[Required]
	[ForeignKey(nameof(User))]
	public int UserId { get; set; }
	public User User { get; set; } = null!;

	public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
}