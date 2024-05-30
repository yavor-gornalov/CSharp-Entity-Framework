using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    public Game()
    {
        this.PlayersStatistics = new List<PlayerStatistic>();
        this.Bets = new List<Bet>();
    }

    [Key]
    public int GameId { get; set; }

    public int HomeTeamId { get; set; }
    [ForeignKey(nameof(HomeTeamId))]
    public virtual Team HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    [ForeignKey(nameof(AwayTeamId))]
    public virtual Team AwayTeam { get; set; }

    public byte HomeTeamGoals { get; set; }

    public byte AwayTeamGoals { get; set; }

    public double HomeTeamBetRate { get; set; }

    public double AwayTeamBetRate { get; set; }

    public double DrawBetRate { get; set; }

    public DateTime DateTime { get; set; }

    [MaxLength(ValidationConstants.ResultMaxLength)]
    public string? Result { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }
}
