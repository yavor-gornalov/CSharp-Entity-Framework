namespace Trucks.DataProcessor
{
	using Trucks.Helpers;
	using Data;
	using Newtonsoft.Json;
	using Trucks.Data.Models.Enums;
	using Trucks.DataProcessor.ExportDto;

	public class Serializer
	{
		public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
		{
			var despachersWithTheirTrucks = context.Despatchers
				.Where(d => d.Trucks.Any())
				.Select(d => new ExportDespatcherDto
				{
					TrucksCount = d.Trucks.Count(),
					Name = d.Name,
					Trucks = d.Trucks
						.OrderBy(t => t.RegistrationNumber)
						.Select(t => new ExportTruckDto
						{
							RegistrationNumber = t.RegistrationNumber,
							Make = t.MakeType.ToString()
						}).ToArray()
				})
				.OrderByDescending(d => d.TrucksCount)
				.ThenBy(d => d.Name)
				.ToList();

			return XmlSerializationHelper.Serialize(despachersWithTheirTrucks, "Despatchers");
		}

		public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
		{
			var clientsWithMostTrucks = context.Clients
				.Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
				.Select(c => new
				{
					Name = c.Name,
					Trucks = c.ClientsTrucks
						.Where(ct => ct.Truck.TankCapacity >= capacity)
						.OrderBy(ct => ct.Truck.MakeType)
						.ThenByDescending(ct => ct.Truck.CargoCapacity)
						.Select(ct => new
						{
							TruckRegistrationNumber = ct.Truck.RegistrationNumber,
							VinNumber = ct.Truck.VinNumber,
							TankCapacity = ct.Truck.TankCapacity,
							CargoCapacity = ct.Truck.CargoCapacity,
							CategoryType = ((CategoryType)ct.Truck.CategoryType).ToString(),
							MakeType = ((MakeType)ct.Truck.MakeType).ToString(),
						})
						.ToList()
				})
				.OrderByDescending(c => c.Trucks.Count)
				.ThenBy(c => c.Name)
				.Take(10)
				.ToList();

			return JsonConvert.SerializeObject(clientsWithMostTrucks, Formatting.Indented);
		}
	}
}
