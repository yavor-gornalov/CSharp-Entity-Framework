namespace VaporStore.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
	using Castle.Core.Internal;
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

			var gamesInfo = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

			var games = new List<Game>();

			var genres = new List<Genre>();

			var developers = new List<Developer>();

			var tags = new List<Tag>();

			foreach (var g in gamesInfo)
			{
				var isDateValid = DateTime.TryParseExact(g.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

				if (!IsValid(g) ||
					!isDateValid ||
					g.Tags.IsNullOrEmpty())
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var developer = developers.FirstOrDefault(d => d.Name == g.Developer);
				if (developer == null)
				{
					developer = new Developer { Name = g.Developer };
					developers.Add(developer);
				}

				var genre = genres.FirstOrDefault(gr => gr.Name == g.Genre);
				if (genre == null)
				{
					genre = new Genre { Name = g.Genre };
					genres.Add(genre);
				}

				var game = new Game
				{
					Name = g.Name,
					Price = g.Price,
					ReleaseDate = releaseDate,
					Genre = genre,
					Developer = developer,
				};

				foreach (var tagName in g.Tags)
				{
					var tag = tags.FirstOrDefault(tg => tg.Name == tagName);
					if (tag == null)
					{
						tag = new Tag { Name = tagName };
						tags.Add(tag);
					}
					var gameTag = new GameTag
					{
						Game = game,
						Tag = tag,
					};
					game.GameTags.Add(gameTag);
				}

				sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));

				games.Add(game);
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

			foreach (var u in usersInfo)
			{
				bool isValidUser = IsValid(u);

				bool isValidUserCards = true;

				if (u.Cards != null)
				{
					foreach (var c in u.Cards)
					{
						if (!IsValid(c))
						{
							isValidUserCards = false;
							break;
						}
					}
				}
				else
				{
					isValidUserCards = false;
				}

				if (!isValidUser || !isValidUserCards)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var user = new User
				{
					Username = u.Username,
					FullName = u.FullName,
					Email = u.Email,
					Age = u.Age,
				};

				foreach (var c in u.Cards)
				{
					var card = new Card
					{
						Number = c.Number,
						Cvc = c.Cvc,
						Type = (CardType)Enum.Parse(typeof(CardType), c.Type),
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

			var purchases = new List<Purchase>();

			var cards = context.Cards.ToList();

			var games = context.Games.ToList();

			foreach (var p in purchasesInfo)
			{
				var isDateValid = DateTime.TryParseExact(p.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime purchaseDate);

				var card = cards.FirstOrDefault(c => c.Number == p.CardNumber);

				var game = games.FirstOrDefault(g => g.Name == p.GameTitle);

				if (!IsValid(p) || !isDateValid || game == null || card == null)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var purchase = new Purchase
				{
					Type = (PurchaseType)Enum.Parse((typeof(PurchaseType)), p.Type),
					ProductKey = p.ProductKey,
					Card = card,
					Game = game,
					Date = purchaseDate,
				};

				var gameName = purchase.Game.Name;
				var username = purchase.Card.User.Username;

				purchases.Add(purchase);
				sb.AppendLine(string.Format(SuccessfullyImportedPurchase, gameName, username));
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