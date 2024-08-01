using System.ComponentModel.DataAnnotations;
using static VaporStore.Common.GlobalConstants;

namespace VaporStore.Data.Models;

public class User
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(UsernameMaxLength)]
	public string Username { get; set; } = null!;

	[Required]
	public string FullName { get; set; } = null!;

	[Required]
	public string Email { get; set; } = null!;

    [Required]
    public int Age { get; set; }

	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}
