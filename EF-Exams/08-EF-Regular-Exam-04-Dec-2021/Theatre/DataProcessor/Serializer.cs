namespace Theatre.DataProcessor
{
	using Newtonsoft.Json;
	using System.Globalization;
	using Theatre.Data;
	using Theatre.Data.Models.Enums;
	using Theatre.DataProcessor.ExportDto;
	using Theatre.Helpers;

	public class Serializer
	{
		public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
		{
			var theatres = context.Theatres
				.Where(th => th.NumberOfHalls >= numbersOfHalls && th.Tickets.Count >= 20)
				.Select(th => new
				{
					Name = th.Name,
					Halls = th.NumberOfHalls,
					TotalIncome = th.Tickets
						.Where(t => t.RowNumber <= 5)
						.Sum(t => t.Price),
					Tickets = th.Tickets
						.Where(t => t.RowNumber <= 5)
						.Select(t => new
						{
							Price = t.Price,
							RowNumber = t.RowNumber,
						})
						.OrderByDescending(t => t.Price)
						.ToArray()
				})
				.OrderByDescending(th => th.Halls)
				.ThenBy(th => th.Name)
				.ToList();

			return JsonConvert.SerializeObject(theatres, Formatting.Indented);
		}

		public static string ExportPlays(TheatreContext context, double raiting)
		{
			string mainCharacterInfo = "Plays main character in '{0}'.";

			var playsInfo = context.Plays
				.Where(p => p.Rating <= (float)raiting)
				.Select(p => new ExportPlayDto
				{
					Title = p.Title,
					Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
					Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
					Genre = p.Genre.ToString(),
					Actors = p.Casts
						.Where(a => a.IsMainCharacter == true)
						.Select(a => new ExportActorDto
						{
							FullName = a.FullName,
							MainCharacter = string.Format(mainCharacterInfo, p.Title)
						})
						.OrderByDescending(a => a.FullName)
						.ToArray()
				})
				.ToArray()
				.OrderBy(p => p.Title)
				.ThenByDescending(p => p.Genre)
				.ToList();

			return XmlSerializationHelper.Serialize(playsInfo, "Plays");
		}
	}
}
