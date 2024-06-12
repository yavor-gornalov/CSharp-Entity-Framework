using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Newtonsoft.Json;
using System.IO;

namespace CarDealer
{
	public class StartUp
	{
		private const string _DataPath = "../../../Datasets/";
		public static void Main()
		{
			var context = new CarDealerContext();

			// 09. Import Suppliers
			// string suppliersJson = File.ReadAllText(_DataPath + "suppliers.json");
			// Console.WriteLine(ImportSuppliers(context, suppliersJson));

			// 10. Import Parts
			// string partsJson = File.ReadAllText(_DataPath + "parts.json");
			// Console.WriteLine(ImportParts(context, partsJson));

			// 11.Import Cars
			// string carsJson = File.ReadAllText(_DataPath + "cars.json");
			// Console.WriteLine(ImportCars(context, carsJson));

			// 12. Import Customers
			// string customersJson = File.ReadAllText(_DataPath + "customers.json");
			// Console.WriteLine(ImportCustomers(context, customersJson));

			// 13. Import Sales
			string salesJson = File.ReadAllText(_DataPath + "sales.json");
			Console.WriteLine(ImportSales(context, salesJson));
		}

		// 09. Import Suppliers
		public static string ImportSuppliers(CarDealerContext context, string inputJson)
		{
			var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

			context.Suppliers.AddRange(suppliers);
			context.SaveChanges();

			return $"Successfully imported {suppliers.Count}.";
		}

		// 10. Import Parts
		public static string ImportParts(CarDealerContext context, string inputJson)
		{
			var validSuppliersIds = context.Suppliers
				.Select(x => x.Id)
				.ToList();

			var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson)
				.Where(p => validSuppliersIds.Contains(p.SupplierId))
				.ToList();

			context.Parts.AddRange(parts);
			context.SaveChanges();

			return $"Successfully imported {parts.Count}.";
		}

		// 11.Import Cars
		public static string ImportCars(CarDealerContext context, string inputJson)
		{
			var cars = new List<Car>();
			var partsCars = new List<PartCar>();

			var importedCarsData = JsonConvert.DeserializeObject<List<CarImportDTO>>(inputJson);

			foreach (var data in importedCarsData)
			{
				var car = new Car
				{
					Make = data.Make,
					Model = data.Model,
					TraveledDistance = data.TraveledDistance
				};
				cars.Add(car);

				foreach (var partId in data.PartsId)
				{
					partsCars.Add(new PartCar
					{
						Car = car,
						PartId = partId,
					});
				}
			}

			context.Cars.AddRange(cars);
			context.PartsCars.AddRange(partsCars);
			context.SaveChanges();

			return $"Successfully imported {importedCarsData.Count}.";
		}

		// 12. Import Customers
		public static string ImportCustomers(CarDealerContext context, string inputJson)
		{
			var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

			context.Customers.AddRange(customers);
			context.SaveChanges();

			return $"Successfully imported {customers.Count}.";
		}

		// 13. Import Sales
		public static string ImportSales(CarDealerContext context, string inputJson)
		{
			var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

			context.Sales.AddRange(sales);
			context.SaveChanges();

			return $"Successfully imported {sales.Count}.";
		}
	}
}