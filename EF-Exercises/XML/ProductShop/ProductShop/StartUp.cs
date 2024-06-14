using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop;

public class StartUp
{
	private const string _DatasetsPath = "../../../Datasets/";
	public static void Main()
	{
		// Setup DB context
		ProductShopContext context = new();

		// 01.Import Users
		// string inputUsersXml = File.ReadAllText(_DatasetsPath + "users.xml");
		// Console.WriteLine(ImportUsers(context, inputUsersXml));

		// 02. Import Products
		// string inputProductsXml = File.ReadAllText(_DatasetsPath + "products.xml");
		// Console.WriteLine(ImportProducts(context, inputProductsXml));

		// 03. Import Categories
		// string inputCategoriessXml = File.ReadAllText(_DatasetsPath + "categories.xml");
		// Console.WriteLine(ImportCategories(context, inputCategoriessXml));

		// 04. Import Categories and Products 
		string inputCategoriessAndProductsXml = File.ReadAllText(_DatasetsPath + "categories-products.xml");
		Console.WriteLine(ImportCategoryProducts(context, inputCategoriessAndProductsXml));
	}

	// 01.Import Users
	public static string ImportUsers(ProductShopContext context, string inputXml)
	{
		var mapper = GetMapper();

		var importedUserDTOs = DeserializeXml<ImportUserDTO>(inputXml, "Users");

		List<User> users = mapper.Map<List<User>>(importedUserDTOs);

		context.Users.AddRange(users);
		context.SaveChanges();

		return $"Successfully imported {users.Count}";
	}

	// 02. Import Products
	public static string ImportProducts(ProductShopContext context, string inputXml)
	{
		var mapper = GetMapper();

		var importedProductDTOs = DeserializeXml<ImportProductDTO>(inputXml, "Products");

		List<Product> products = mapper.Map<List<Product>>(importedProductDTOs);

		context.Products.AddRange(products);
		context.SaveChanges();

		return $"Successfully imported {products.Count}";
	}

	// 03. Import Categories
	public static string ImportCategories(ProductShopContext context, string inputXml)
	{
		var mapper = GetMapper();

		var importedCategoryDTOs = DeserializeXml<ImportCategoryDTO>(inputXml, "Categories");

		List<Category> categories = mapper.Map<List<Category>>(importedCategoryDTOs);

		context.Categories.AddRange(categories);
		context.SaveChanges();

		return $"Successfully imported {categories.Count}";
	}

	// 04. Import Categories and Products 
	public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
	{
		var mapper = GetMapper();

		var importedCategoryProductDTOs = DeserializeXml<ImportCategoryProductDTO>(inputXml, "CategoryProducts");

		List<CategoryProduct> categoryProducts = mapper.Map<List<CategoryProduct>>(importedCategoryProductDTOs);

		context.CategoryProducts.AddRange(categoryProducts);
		context.SaveChanges();

		return $"Successfully imported {categoryProducts.Count}";
	}

	// Helpers
	private static Mapper GetMapper()
		=> new(new MapperConfiguration(c => c.AddProfile<ProductShopProfile>()));

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