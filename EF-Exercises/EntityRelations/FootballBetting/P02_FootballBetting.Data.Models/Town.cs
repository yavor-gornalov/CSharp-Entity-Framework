using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    public Town()
    {
        this.Teams = new List<Team>();
        this.Players = new List<Player>();
    }

    [Key]
    public int TownId { get; set; }
    [Required]
    [MaxLength(ValidationConstants.TownNameMaxLength)]
    public string Name { get; set; } = null!;

    public int CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; }

    public virtual ICollection<Player> Players { get; set; }
}
