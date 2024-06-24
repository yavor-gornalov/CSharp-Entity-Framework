using Boardgames.Common;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Seller
{
    public Seller()
    {
        BoardgamesSellers = new List<BoardgameSeller>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.SellerNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.SellerAddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;

    [Required]
    public string Website { get; set; } = null!;

    public ICollection<BoardgameSeller> BoardgamesSellers  { get; set; }
}
