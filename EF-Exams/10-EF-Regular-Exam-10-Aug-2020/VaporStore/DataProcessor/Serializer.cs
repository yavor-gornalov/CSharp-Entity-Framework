namespace VaporStore.DataProcessor
{
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using System.Globalization;
	using TeisterMask.Helpers;
	using VaporStore.Data.Models;
	using VaporStore.DataProcessor.ExportDto;

	public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var gamesByGenres = context.Genres
				.Include(x => x.Games)
				.Where(g => genreNames.Contains(g.Name))
				.Include(x => x.Games)
				.Select(g => new
				{
					g.Id,
					Genre = g.Name,
					Games = g.Games
						.Where(gm => gm.Purchases.Any())
						.AsEnumerable()
						.Select(gm => new
						{
							gm.Id,
							Title = gm.Name,
							Developer = gm.Developer.Name,
							Tags = string.Join(", ", gm.GameTags
											.Select(gmt => gmt.Tag.Name)),
							Players = gm.Purchases.Count
						})
						.OrderByDescending(gm => gm.Players)
						.ThenBy(gm => gm.Id)
						.ToArray(),
				})
				.ToArray();

			var gamesByGenresSorted = gamesByGenres
				.Select(g => new
				{
					g.Id,
					g.Genre,
					g.Games,
					TotalPlayers = g.Games.Sum(gm => gm.Players),
				})
				.OrderByDescending(g => g.TotalPlayers)
				.ThenBy(g => g.Id)
				.ToList();

			return JsonConvert.SerializeObject(gamesByGenresSorted, Formatting.Indented);
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
		{
			var purchasesByType = context.Users
				.Include(u => u.Cards)
				.ThenInclude(c => c.Purchases)
				.ThenInclude(p => p.Game)
				.ThenInclude(gm => gm.Genre)
				// Filter users with any purchases
				.Where(u => u.Cards.Any(c => c.Purchases.Any()))
				// Get data from database
				.AsEnumerable()
				// Filter users with purchases from the given type
				.Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
				.Select(u => new ExportUserDto
				{
					Username = u.Username,
					Purchases = u.Cards
						.SelectMany(c => c.Purchases
							// Only demanded purchase type should be selected
							.Where(p => p.Type.ToString() == purchaseType)
							.Select(p => new ExportPurchaseDto
							{
								CardNumber = c.Number,
								CardCvc = c.Cvc,
								Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
								Game = new ExportGameDto
								{
									Title = p.Game.Name,
									Genre = p.Game.Genre.Name,
									Price = p.Game.Price
								}
							}))
						.OrderBy(p => p.Date)
						.ToArray(),
					TotalSpent = u.Cards.Sum(c => c.Purchases
						.Where(p => p.Type.ToString() == purchaseType)
						.Sum(p => p.Game.Price))
				})
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.Username)
				.ToList();

			return XmlSerializationHelper.Serialize(purchasesByType, "Users");
		}
	}
}