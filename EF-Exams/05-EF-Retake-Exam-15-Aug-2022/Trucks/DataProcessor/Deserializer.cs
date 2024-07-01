namespace Trucks.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Text;
	using Trucks.Helpers;
	using Data;
	using Newtonsoft.Json;
	using Trucks.Data.Models;
	using Trucks.Data.Models.Enums;
	using Trucks.DataProcessor.ImportDto;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedDespatcher
			= "Successfully imported despatcher - {0} with {1} trucks.";

		private const string SuccessfullyImportedClient
			= "Successfully imported client - {0} with {1} trucks.";

		public static string ImportDespatcher(TrucksContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var despatcherInfo = XmlSerializationHelper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

			var despatchers = new List<Despatcher>();

			foreach (var despatcherDto in despatcherInfo)
			{
				if (!IsValid(despatcherDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var despatcher = new Despatcher
				{
					Name = despatcherDto.Name,
					Position = despatcherDto.Position,
				};

				foreach (var truckDto in despatcherDto.Trucks)
				{
					if (!IsValid(truckDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var truck = new Truck
					{
						RegistrationNumber = truckDto.RegistrationNumber,
						VinNumber = truckDto.VinNumber,
						TankCapacity = truckDto.TankCapacity,
						CargoCapacity = truckDto.CargoCapacity,
						CategoryType = (CategoryType)truckDto.CategoryType,
						MakeType = (MakeType)truckDto.MakeType,

					};

					despatcher.Trucks.Add(truck);
				}

				despatchers.Add(despatcher);
				sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
			}

			context.Despatchers.AddRange(despatchers);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportClient(TrucksContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var validTrucksIds = context.Trucks.Select(t => t.Id).ToList();

			var clientsInfo = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

			var clients = new List<Client>();

			foreach (var clientDto in clientsInfo)
			{
				if (!IsValid(clientDto) || clientDto.Type == "usual")
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var client = new Client
				{
					Name = clientDto.Name,
					Nationality = clientDto.Nationality,
					Type = clientDto.Type,
				};

				foreach (int truckId in clientDto.TruckIds.Distinct())
				{
					if (!validTrucksIds.Contains(truckId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var clientTruck = new ClientTruck
					{
						TruckId = truckId,
						Client = client
					};

					client.ClientsTrucks.Add(clientTruck);
				}

				clients.Add(client);
				sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
			}

			context.Clients.AddRange(clients);
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