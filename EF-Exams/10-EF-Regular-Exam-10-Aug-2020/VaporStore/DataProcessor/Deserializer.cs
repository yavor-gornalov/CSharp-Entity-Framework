namespace VaporStore.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using System.Xml.Serialization;
	using Castle.Core.Internal;
	using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
	using Data;
	using Newtonsoft.Json;
	using TeisterMask.Helpers;
	using VaporStore.Data.Models;
	using VaporStore.Data.Models.Enums;
	using VaporStore.DataProcessor.ImportDto;

	public static class Deserializer
	{
		public const string ErrorMessage = "Invalid Data";

		public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

		public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

		public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var existingDeveloperNames = context.Developers.Select(d => d.Name);
			var existingTagNames = context.Tags.Select(d => d.Name);
			var existingGenreNames = context.Genres.Select(d => d.Name);

			var gamesInfo = JsonConvert.DeserializeObject<List<ImportGameDto>>(jsonString);

			var games = new List<Game>();
			var developers = new List<Developer>();
			var genres = new List<Genre>();
			var tags = new List<Tag>();

			if (gamesInfo == null) return string.Empty;

			foreach (var gameDto in gamesInfo)
			{
				bool isReleaseDateValid = DateTime.TryParseExact(
					gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

				if (!IsValid(gameDto)
						|| gameDto.Name.IsNullOrEmpty()
						|| gameDto.Developer.IsNullOrEmpty()
						|| gameDto.Genre.IsNullOrEmpty()
						|| gameDto.Tags.IsNullOrEmpty()
						|| !isReleaseDateValid)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				if (!developers.Any(d => d.Name == gameDto.Developer))
					developers.Add(new Developer { Name = gameDto.Developer });

				if (!genres.Any(g => g.Name == gameDto.Genre))
					genres.Add(new Genre { Name = gameDto.Genre });

				var game = new Game
				{
					Name = gameDto.Name,
					Price = gameDto.Price,
					ReleaseDate = releaseDate,
					Developer = developers.First(d => d.Name == gameDto.Developer),
					Genre = genres.First(g => g.Name == gameDto.Genre)
				};

				foreach (var tagName in gameDto.Tags)
				{
					if (!tags.Any(t => t.Name == tagName))
						tags.Add(new Tag { Name = tagName });

					var gameTag = new GameTag
					{
						Game = game,
						Tag = tags.First(t => t.Name == tagName)
					};

					game.GameTags.Add(gameTag);
				}

				games.Add(game);
				sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
			}

			context.Games.AddRange(games);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var usersInfo = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

			var users = new List<User>();

			if (usersInfo == null) return string.Empty;

			foreach (var userDto in usersInfo)
			{
				if (!IsValid(userDto) || !userDto.Cards.Any())
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var user = new User
				{
					Username = userDto.Username,
					FullName = userDto.FullName,
					Email = userDto.Email,
					Age = userDto.Age,
				};

				foreach (var cardDto in userDto.Cards)
				{
					if (!IsValid(cardDto)
						|| !Enum.TryParse(cardDto.Type, out CardType cardType))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var card = new Card
					{
						Number = cardDto.Number,
						Cvc = cardDto.Cvc,
						Type = cardType,
					};

					user.Cards.Add(card);
				}

				users.Add(user);
				sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
			}

			context.Users.AddRange(users);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var purchasesInfo = XmlSerializationHelper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");

			var cards = context.Cards;

			var games = context.Games;

			var purchases = new List<Purchase>();

			if (purchasesInfo == null) return string.Empty;

			foreach (var purdchaseDto in purchasesInfo)
			{
				if (!IsValid(purdchaseDto)
					|| !Enum.TryParse(purdchaseDto.CardType, out PurchaseType purchaseType)
					|| !cards.Select(c => c.Number).Contains(purdchaseDto.CardNumer))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var purchase = new Purchase
				{
					Game = games.First(g => g.Name == purdchaseDto.GameName),
					Type = purchaseType,
					ProductKey = purdchaseDto.Key,
					Card = cards.First(c => c.Number == purdchaseDto.CardNumer),
					Date = DateTime.ParseExact(purdchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
				};

				purchases.Add(purchase);
				sb.AppendLine(string.Format(SuccessfullyImportedPurchase, purchase.Game.Name, purchase.Card.User.Username));
			}

			context.Purchases.AddRange(purchases);
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