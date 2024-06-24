namespace Boardgames.DataProcessor
{
	using Boardgames.Data;
	using Boardgames.Data.Models.Enums;
	using Boardgames.DataProcessor.ExportDto;
	using Boardgames.Helpers;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;

	public class Serializer
	{
		public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
		{
			var creators = context.Creators
				.Where(c => c.Boardgames.Any())
				.Select(c => new ExportToXmlCreatorDTO
				{
					CreatorName = c.FirstName + ' ' + c.LastName,
					BoardgamesCount = c.Boardgames.Count(),
					Boardgames = c.Boardgames
						.Select(bg => new ExportToXmlBoardgameDTO {
							Name = bg.Name,
							YearPublished = bg.YearPublished,
						})
						.OrderBy(bg => bg.Name)
						.ToArray()
				})
				.OrderByDescending(c => c.Boardgames.Count())
				.ThenBy(c => c.CreatorName)
				.ToArray();

			return XmlSerializationHelper.Serialize(creators, "Creators");
		}

		public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
		{
			var sellers = context.Sellers
				.Where(s => s.BoardgamesSellers.Any(bgs => bgs.Boardgame.YearPublished >= year && bgs.Boardgame.Rating <= rating))
				.Select(s => new JsonExportSellerDTO
				{
					Name = s.Name,
					Website = s.Website,
					Boardgames = s.BoardgamesSellers
						.Where(bgs => bgs.Boardgame.YearPublished >= year && bgs.Boardgame.Rating <= rating)
						.Select(bgs => new ExportToJsonBoardgameDTO
						{
							Name = bgs.Boardgame.Name,
							Rating = bgs.Boardgame.Rating,
							CategoryType = bgs.Boardgame.CategoryType.ToString(),
							Mechanics = bgs.Boardgame.Mechanics,
						})
						.OrderByDescending(bg => bg.Rating)
						.ThenBy(bg => bg.Name)
						.ToArray(),
				})
				.OrderByDescending(s => s.Boardgames.Length)
				.ThenBy(s => s.Name)
				.Take(5)
				.AsNoTracking()
				.ToArray();

			return JsonConvert.SerializeObject(sellers, Formatting.Indented);
		}
	}
}