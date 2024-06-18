using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
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

		StringBuilder stringBuilder = new StringBuilder();

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