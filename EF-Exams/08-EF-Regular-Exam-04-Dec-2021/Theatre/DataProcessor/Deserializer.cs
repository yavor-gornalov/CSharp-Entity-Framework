namespace Theatre.DataProcessor
{
	//using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using System.Text.Json;
	using Theatre.Data;
	using Theatre.Data.Models;
	using Theatre.Data.Models.Enums;
	using Theatre.DataProcessor.ImportDto;
	using Theatre.Helpers;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfulImportPlay
			= "Successfully imported {0} with genre {1} and a rating of {2}!";

		private const string SuccessfulImportActor
			= "Successfully imported actor {0} as a {1} character!";

		private const string SuccessfulImportTheatre
			= "Successfully imported theatre {0} with #{1} tickets!";



		public static string ImportPlays(TheatreContext context, string xmlString)
		{
			var plays = new List<Play>();
			var sb = new StringBuilder();

			var playsInfo = XmlSerializationHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

			foreach (var p in playsInfo)
			{
				TimeSpan duration;
				Genre genre;

				bool isDurationValid = TimeSpan.TryParseExact(p.Duration, "c", CultureInfo.InvariantCulture, out duration);
				bool isGenreValid = Enum.TryParse(p.Genre, out genre);

				if (!IsValid(p) || !isDurationValid || !isGenreValid || duration.TotalHours < 1)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var play = new Play
				{
					Title = p.Title,
					Duration = duration,
					Rating = p.Rating,
					Genre = genre,
					Description = p.Description,
					Screenwriter = p.Screenwriter,
				};

				plays.Add(play);
				sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
			}

			context.Plays.AddRange(plays);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportCasts(TheatreContext context, string xmlString)
		{
			var casts = new List<Cast>();
			var sb = new StringBuilder();

			var castsInfo = XmlSerializationHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

			foreach (var c in castsInfo)
			{
				if (!IsValid(c))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var cast = new Cast
				{
					FullName = c.FullName,
					IsMainCharacter = c.IsMainCharacter,
					PhoneNumber = c.PhoneNumber,
					PlayId = c.PlayId,
				};

				string characterType = cast.IsMainCharacter == true ? "main" : "lesser";

				sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, characterType));
				casts.Add(cast);
			}

			context.Casts.AddRange(casts);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
		{
			var theatres = new List<Theatre>();
			var sb = new StringBuilder();

			//var theatresInfo = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);
			var theatresInfo = JsonSerializer.Deserialize<ImportTheatreDto[]>(jsonString);

			foreach (var th in theatresInfo)
			{
				if (!IsValid(th))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var theatre = new Theatre
				{
					Name = th.Name,
					NumberOfHalls = th.NumberOfHalls,
					Director = th.Director,
				};

				foreach (var t in th.Tickets)
				{
					if (!IsValid(t))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var ticket = new Ticket
					{
						Price = t.Price,
						RowNumber = t.RowNumber,
						PlayId = t.PlayId,
					};

					theatre.Tickets.Add(ticket);
				}

				theatres.Add(theatre);
				sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
			}

			context.Theatres.AddRange(theatres);
			context.SaveChanges();

			return sb.ToString().Trim();
		}


		private static bool IsValid(object obj)
		{
			var validator = new ValidationContext(obj);
			var validationRes = new List<ValidationResult>();

			var result = Validator.TryValidateObject(obj, validator, validationRes, true);
			return result;
		}
	}
}
