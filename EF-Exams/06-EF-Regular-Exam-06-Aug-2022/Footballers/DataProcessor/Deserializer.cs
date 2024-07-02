namespace Footballers.DataProcessor
{
	using Footballers.Data;
	using Footballers.Data.Models;
	using Footballers.Data.Models.Enums;
	using Footballers.DataProcessor.ImportDto;
	using Footballers.Helpers;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedCoach
			= "Successfully imported coach - {0} with {1} footballers.";

		private const string SuccessfullyImportedTeam
			= "Successfully imported team - {0} with {1} footballers.";

		public static string ImportCoaches(FootballersContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var coachesInfo = XmlSerializationHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

			var coaches = new List<Coach>();

			foreach (var c in coachesInfo)
			{
				if (!IsValid(c))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var coach = new Coach
				{
					Name = c.Name,
					Nationality = c.Nationality,
				};

				foreach (var f in c.Footballers)
				{
					if (!IsValid(f) ||
						!DateTime.TryParseExact(f.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var contractStartDate) ||
						!DateTime.TryParseExact(f.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var contractEndDate) ||
						contractStartDate >= contractEndDate)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var footballer = new Footballer
					{
						Name = f.Name,
						ContractStartDate = contractStartDate,
						ContractEndDate = contractEndDate,
						BestSkillType = (BestSkillType)f.BestSkillType,
						PositionType = (PositionType)f.PositionType,
					};

					coach.Footballers.Add(footballer);
				}

				coaches.Add(coach);
				sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
			}

			context.Coaches.AddRange(coaches);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportTeams(FootballersContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var validFootballersIds = context.Footballers.Select(f => f.Id).ToList();

			var teamsInfo = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);
			var teams = new List<Team>();

			foreach (var t in teamsInfo)
			{
				if (!IsValid(t))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var team = new Team
				{
					Name = t.Name,
					Nationality = t.Nationality,
					Trophies = t.Trophies,
				};

				foreach (int footballerId in t.FootballersIds)
				{
					if (!validFootballersIds.Contains(footballerId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var teamFootballer = new TeamFootballer
					{
						Team = team,
						FootballerId = footballerId,
					};

					team.TeamsFootballers.Add(teamFootballer);
				}

				teams.Add(team);
				sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
			}

			context.Teams.AddRange(teams);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}
