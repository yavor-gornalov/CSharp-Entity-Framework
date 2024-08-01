namespace VaporStore.DataProcessor
{
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using System.Globalization;
	using TeisterMask.Helpers;
	using VaporStore.Data.Models;
	using VaporStore.Data.Models.Enums;
	using VaporStore.DataProcessor.ExportDto;

	public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var genresInfo = context.Genres
				.Where(genre => genreNames.Contains(genre.Name))
				.Select(genre => new
				{
					genre.Id,
					Genre = genre.Name,
					Games = genre.Games
					.Where(g => g.Purchases.Any())
					.Select(g => new
					{
						g.Id,
						Title = g.Name,
						Developer = g.Developer.Name,
						Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
						Players = g.Purchases.Count
					})
					.OrderByDescending(g => g.Players)
					.ThenBy(g => g.Id)
					.ToList(),
				})
				.ToList();

			var genres = genresInfo
				.Select(genre => new
				{
					genre.Id,
					genre.Genre,
					genre.Games,
					TotalPlayers = genre.Games.Sum(g => g.Players)
				})
				.OrderByDescending(g => g.TotalPlayers)
				.ThenBy(g => g.Id)
				.ToList();

			return JsonConvert.SerializeObject(genres, Formatting.Indented);
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
		{
			var users = context.Users
				.Include(u => u.Cards)
				.ThenInclude(c => c.Purchases)
				.ThenInclude(p => p.Game)
				.ThenInclude(gm => gm.Genre)
				.Where(u => u.Cards.Any(c => c.Purchases.Any()))
				.AsEnumerable()
				.Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == purchaseType)))
				.Select(u => new ExportUserDto
				{
					Username = u.Username,
					Purchases = u.Cards
						.SelectMany(c => c.Purchases
							.Where(p => p.Type.ToString() == purchaseType)
							.Select(p => new ExportPurchaseDto
							{
								CardNumber = c.Number,
								CardCvc = c.Cvc,
								PurchaseDate = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
								Game = new ExportGameDto
								{
									GameTitle = p.Game.Name,
									Genre = p.Game.Genre.Name,
									Price = p.Game.Price
								}
							}))
						.OrderBy(p => p.PurchaseDate)
						.ToArray(),
					TotalSpent = u.Cards.Sum(c => c.Purchases
						.Where(p => p.Type.ToString() == purchaseType)
						.Sum(p => p.Game.Price))
				})
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.Username)
				.ToList();

			return XmlSerializationHelper.Serialize(users, "Users");
		}
	}
}