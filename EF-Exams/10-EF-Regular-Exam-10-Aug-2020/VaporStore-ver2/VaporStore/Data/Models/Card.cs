using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;
using static VaporStore.Data.Validations.GlobalConstants;

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
	public int UserId { get; set; }

	[ForeignKey(nameof(UserId))]
	public User User { get; set; } = null!;

	public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
}

//•	Id – integer, Primary Key
//•	Number – text, which consists of 4 pairs of 4 digits, separated by spaces(ex. "1234 5678 9012 3456") (required)
//•	Cvc – text, which consists of 3 digits(ex. "123") (required)
//•	Type – enumeration of type CardType, with possible values("Debit", "Credit") (required)
//•	UserId – integer, foreign key(required)
//•	User – the card's user (required)
//•	Purchases – collection of type Purchase
