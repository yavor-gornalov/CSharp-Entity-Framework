using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    [Key]
    public int TeamId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.LogoUrlMaxLength)]
    public string? LogoUrl { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TeamInitialsMaxLength)]
    public string Initials { get; set; } = null!;

    public decimal Budget { get; set; }

    public int PrimaryKitColorId { get; set; }
    [ForeignKey(nameof(Color.PrimaryKitTeams))]
    public Color PrimaryKitColor { get; set; } = null!;

    public int SecondaryKitColorId { get; set; }
    [ForeignKey(nameof(Color.SecondaryKitTeams))]
    public Color SecondaryKitColor { get; set; } = null!;

    public int TownId { get; set; }
    [ForeignKey(nameof(TownId))]
    public Town Town { get; set; } = null!;



}
