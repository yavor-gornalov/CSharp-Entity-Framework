namespace Boardgames.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Diagnostics.Metrics;
	using System.Net;
	using System.Text;
	using System.Xml.Linq;
	using Boardgames.Data;
	using Boardgames.Data.Models;
	using Boardgames.Data.Models.Enums;
	using Boardgames.DataProcessor.ImportDto;
	using Boardgames.Helpers;
	using Newtonsoft.Json;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedCreator
			= "Successfully imported creator – {0} {1} with {2} boardgames.";

		private const string SuccessfullyImportedSeller
			= "Successfully imported seller - {0} with {1} boardgames.";

		public static string ImportCreators(BoardgamesContext context, string xmlString)
		{
			var creators = XmlSerializationHelper.Deserialize<ImportCreatorDTO[]>(xmlString, "Creators");

			var sb = new StringBuilder();
			var validCreators = new List<Creator>();

			foreach (var creator in creators)
			{
				if (!IsValid(creator))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var validCreator = new Creator
				{
					FirstName = creator.FirstName,
					LastName = creator.LastName,
				};

				foreach (var boardgame in creator.Boardgames)
				{
					if (!IsValid(boardgame))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var validBoardgame = new Boardgame
					{
						Name = boardgame.Name,
						Rating = boardgame.Rating,
						YearPublished = boardgame.YearPublished,
						CategoryType = (CategoryType)boardgame.CategoryType,
						Mechanics = boardgame.Mechanics
					};
					validCreator.Boardgames.Add(validBoardgame);
				}
				sb.AppendLine(string.Format(SuccessfullyImportedCreator, validCreator.FirstName, validCreator.LastName, validCreator.Boardgames.Count));
				validCreators.Add(validCreator);
			}

			context.Creators.AddRange(validCreators);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportSellers(BoardgamesContext context, string jsonString)
		{
			var sellers = JsonConvert.DeserializeObject<ImportSellerDTO[]>(jsonString).ToList();
			var validBoardgameIds = context.Boardgames.Select(b => b.Id).ToList();

			var sb = new StringBuilder();
			var validSellers = new List<Seller>();

			foreach (var seller in sellers)
			{
				if (!IsValid(seller))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var validSeller = new Seller
				{
					Name = seller.Name,
					Address = seller.Address,
					Country = seller.Country,
					Website = seller.Website,
				};

				foreach (var boardgameId in seller.GameIds.Distinct())
				{
					if (!validBoardgameIds.Contains(boardgameId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var boardgame = new BoardgameSeller
					{
						Seller = validSeller,
						BoardgameId = boardgameId,
					};

					validSeller.BoardgamesSellers.Add(boardgame);
				}

				validSellers.Add(validSeller);
				sb.AppendLine(string.Format(SuccessfullyImportedSeller, validSeller.Name, validSeller.BoardgamesSellers.Count));
			}

			context.Sellers.AddRange(validSellers);
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
