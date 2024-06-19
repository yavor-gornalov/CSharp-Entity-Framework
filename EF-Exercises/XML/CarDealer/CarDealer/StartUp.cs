using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer;

public class StartUp
{
	private const string _DatasetsPath = "../../../Datasets/";

	public static void Main()
	{
		CarDealerContext context = new CarDealerContext();

		// 09.Import Suppliers
		// string inputSuppliersXml = File.ReadAllText(_DatasetsPath + "suppliers.xml");
		// Console.WriteLine(ImportSuppliers(context, inputSuppliersXml));

		// 10.Import Parts
		// string inputPartsXml = File.ReadAllText(_DatasetsPath + "parts.xml");
		// Console.WriteLine(ImportParts(context, inputPartsXml));

		// 11.Import Cars
		// string inputCarsXml = File.ReadAllText(_DatasetsPath + "cars.xml");
		// Console.WriteLine(ImportCars(context, inputCarsXml));

		// 12. Import Customers
		// string inputCustomersXml = File.ReadAllText(_DatasetsPath + "customers.xml");
		// Console.WriteLine(ImportCustomers(context, inputCustomersXml));

		// 13. Import Sales 
		// string inputSalesXml = File.ReadAllText(_DatasetsPath + "sales.xml");
		// Console.WriteLine(ImportSales(context, inputSalesXml));

		// 14. Export Cars With Distance 
		// Console.WriteLine(GetCarsWithDistance(context));

		// 15. Export Cars From Make BMW
		// Console.WriteLine(GetCarsFromMakeBmw(context));

		// 16. Export Local Suppliers
		// Console.WriteLine(GetLocalSuppliers(context));

		// 17. Export Cars with Their List of Parts
		// Console.WriteLine(GetCarsWithTheirListOfParts(context));

		// 18. Export Total Sales By Customer
		// Console.WriteLine(GetTotalSalesByCustomer(context));

		// 19. Export Sales With Applied Discount
		Console.WriteLine(GetSalesWithAppliedDiscount(context));
	}

	// 09.Import Suppliers
	public static string ImportSuppliers(CarDealerContext context, string inputXml)
	{
		var suppliers = DeserializeXml<ImportSupplierDTO>(inputXml, "Suppliers")
			.Select(s => new Supplier
			{
				Name = s.Name,
				IsImporter = s.IsImporter,
			})
			.ToList();

		context.Suppliers.AddRange(suppliers);
		context.SaveChanges();

		return $"Successfully imported {suppliers.Count}";
	}

	// 10.Import Parts
	public static string ImportParts(CarDealerContext context, string inputXml)
	{
		var validSupplierIds = context.Suppliers
			.AsNoTracking()
			.Select(s => s.Id)
			.ToList();

		var parts = DeserializeXml<ImportPartDTO>(inputXml, "Parts")
			.Where(p => validSupplierIds.Contains(p.SupplierId))
			.Select(p => new Part
			{
				Name = p.Name,
				Price = p.Price,
				Quantity = p.Quantity,
				SupplierId = p.SupplierId,
			})
			.ToList();

		context.Parts.AddRange(parts);
		context.SaveChanges();

		return $"Successfully imported {parts.Count}";
	}

	// 11.Import Cars
	public static string ImportCars(CarDealerContext context, string inputXml)
	{
		ImportCarDTO[] carsInfo =
			DeserializeXml<ImportCarDTO>(inputXml, "Cars");

		List<Car> cars = new();
		foreach (var c in carsInfo)
		{
			Car car = new Car
			{
				Make = c.Make,
				Model = c.Model,
				TraveledDistance = c.TraveledDistance,
			};

			foreach (var partId in c.Parts.Select(x => x.PartId).Distinct())
			{
				PartCar part = new PartCar
				{
					Car = car,
					PartId = partId,
				};
				car.PartsCars.Add(part);
			}
			cars.Add(car);
		}

		context.Cars.AddRange(cars);
		context.SaveChanges();

		return $"Successfully imported {cars.Count}";
	}

	// 12. Import Customers
	public static string ImportCustomers(CarDealerContext context, string inputXml)
	{
		var customers = DeserializeXml<ImportCustomerDTO>(inputXml, "Customers")
			.Select(c => new Customer
			{
				Name = c.Name,
				BirthDate = c.BirthDate,
				IsYoungDriver = c.IsYoungDriver,
			})
			.ToList();

		context.Customers.AddRange(customers);
		context.SaveChanges();

		return $"Successfully imported {customers.Count}";
	}

	// 13. Import Sales 
	public static string ImportSales(CarDealerContext context, string inputXml)
	{
		var validCarIds = context.Cars
			.Select(c => c.Id)
			.ToList();

		var sales = DeserializeXml<ImportSaleDTO>(inputXml, "Sales")
			.Where(s => validCarIds.Contains(s.CarId))
			.Select(s => new Sale
			{
				CarId = s.CarId,
				CustomerId = s.CustomerId,
				Discount = s.Discount,
			})
			.ToList();

		context.Sales.AddRange(sales);
		context.SaveChanges();

		return $"Successfully imported {sales.Count}";
	}

	// 14. Export Cars With Distance 
	public static string GetCarsWithDistance(CarDealerContext context)
	{
		var cars = context.Cars
			.Where(c => c.TraveledDistance > 2_000_000)
			.OrderBy(c => c.Make)
			.ThenBy(c => c.Model)
			.Take(10)
			.AsNoTracking()
			.Select(c => new ExportCarWithDistanceDTO
			{
				Make = c.Make,
				Model = c.Model,
				TraveledDistance = c.TraveledDistance
			})
			.ToList();

		return SerializeToXml(cars, "cars");
	}

	// 15. Export Cars From Make BMW
	public static string GetCarsFromMakeBmw(CarDealerContext context)
	{
		var cars = context.Cars
			.Where(c => c.Make == "BMW")
			.OrderBy(c => c.Model)
			.ThenByDescending(c => c.TraveledDistance)
			.Select(c => new ExportBmwCarDTO
			{
				Id = c.Id,
				Model = c.Model,
				TraveledDistance = c.TraveledDistance
			})
			.ToList();

		return SerializeToXml(cars, "cars");
	}

	// 16. Export Local Suppliers
	public static string GetLocalSuppliers(CarDealerContext context)
	{
		var supliers = context.Suppliers
			.Where(s => !s.IsImporter)
			.Select(s => new ExportLocalSupplierDTO
			{
				Id = s.Id,
				Name = s.Name,
				PartsCount = s.Parts.Count,
			})
			.ToList();

		return SerializeToXml(supliers, "suppliers");
	}

	// 17. Export Cars With Their List Of Parts
	public static string GetCarsWithTheirListOfParts(CarDealerContext context)
	{
		var cars = context.Cars
			.OrderByDescending(c => c.TraveledDistance)
			.ThenBy(c => c.Model)
			.Take(5)
			.AsNoTracking()
			.Select(c => new ExportCarWithPartsDTO
			{
				Make = c.Make,
				Model = c.Model,
				TraveledDistance = c.TraveledDistance,
				Parts = c.PartsCars
					.Select(pc => new ExportCarPartDTO
					{
						Name = pc.Part.Name,
						Price = pc.Part.Price,
					})
					.OrderByDescending(p => p.Price)
					.ToArray()
			})
			.ToList();

		return SerializeToXml(cars, "cars");
	}

	// 18. Export Total Sales By Customer
	public static string GetTotalSalesByCustomer(CarDealerContext context)
	{
		var customers = context.Customers
			.Where(c => c.Sales.Any(s => s.Car != null))
			.Select(c => new ExportCustomerWithCarDTO
			{
				FullName = c.Name,
				CarsBoughtCount = c.Sales.Count,
				TotalMoneySpent = c.Sales
					.Sum(s => s.Car.PartsCars
					.Sum(p => Math.Round(c.IsYoungDriver
							? p.Part.Price * 0.95m
							: p.Part.Price, 2)))
			})
			.OrderByDescending(c => c.TotalMoneySpent)
			.ToList();

		return SerializeToXml(customers, "customers");
	}

	// 19. Export Sales With Applied Discount
	public static string GetSalesWithAppliedDiscount(CarDealerContext context)
	{
		var sales = context.Sales
			.Select(s => new ExportSaleDTO
			{
				Car = new ExportCarDTO
				{
					Make = s.Car.Make,
					Model = s.Car.Model,
					TraveledDistance = s.Car.TraveledDistance
				},
				Discount = s.Discount,
				CustomerName = s.Customer.Name,
				Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
				PriceWithDiscount = ((100 - s.Discount) * s.Car.PartsCars.Sum(pc => pc.Part.Price)) / 100
			})
			.ToList();

		return SerializeToXml(sales, "sales");
	}

	// Helpers
	private static Mapper GetMapper()
		=> new(new MapperConfiguration(c => c.AddProfile<CarDealerProfile>()));

	private static T[] DeserializeXml<T>(string inputXml, string rootElement)
	{
		// Create root element
		XmlRootAttribute xmlRootAttribute = new(rootElement);

		// Create xml serializer with type of dto and root element
		XmlSerializer xmlSerializer = new(typeof(T[]), xmlRootAttribute);

		// Create string reader
		using var xmlReader = new StringReader(inputXml);

		// Deserialize inputXml and cast it to dto type
		return (T[])xmlSerializer.Deserialize(xmlReader);
	}

	private static string SerializeToXml<T>(T obj, string xmlRootAttribute, string prefix = "", string ns = "")
	{
		XmlRootAttribute rootAttribute = new(xmlRootAttribute);

		XmlSerializer xmlSerializer = new(typeof(T), rootAttribute);

		StringBuilder stringBuilder = new();

		StringWriter stringWriter = new(stringBuilder, CultureInfo.InvariantCulture);

		using (stringWriter)
		{
			XmlSerializerNamespaces xmlSerializerNamespaces = new();
			xmlSerializerNamespaces.Add(prefix, ns);

			try
			{
				xmlSerializer.Serialize(stringWriter, obj, xmlSerializerNamespaces);
			}
			catch (Exception)
			{
				throw;
			}
		}

		return stringBuilder.ToString();
	}
}