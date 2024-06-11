using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop;

public class StartUp
{
	private const string _DatasetsPath = "../../../Datasets/";
	public static void Main()
	{
		ProductShopContext context = new();

		// 01. Import Users
		// var usersJson = File.ReadAllText(_DatasetsPath + "users.json");
		// Console.WriteLine(ImportUsers(context, usersJson));

		// 02.Import Products
		// var productsJson = File.ReadAllText(_DatasetsPath + "products.json");
		// Console.WriteLine(ImportProducts(context, productsJson));

		// 03. Import Categories
		// var categoriesJson = File.ReadAllText(_DatasetsPath + "categories.json");
		// Console.WriteLine(ImportCategories(context, categoriesJson));

		// 04. Import Categories and Products
		// var categoriesProductsJson = File.ReadAllText(_DatasetsPath + "categories-products.json");
		// Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));

		// 05. Export Products In Range
		// Console.WriteLine(GetProductsInRange(context));

		// 06. Export Sold Products
		// Console.WriteLine(GetSoldProducts(context));

		// 07. Export Categories By Products Count 
		// Console.WriteLine(GetCategoriesByProductsCount(context));

		// 08. Export Users and Products
		Console.WriteLine(GetUsersWithProducts(context));

	}



	// 01. Import Users
	public static string ImportUsers(ProductShopContext context, string inputJson)
	{
		var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

		context.Users.AddRange(users);
		context.SaveChanges();

		return $"Successfully imported {users.Count}";
	}

	// 02. Import Products
	public static string ImportProducts(ProductShopContext context, string inputJson)
	{
		var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

		context.Products.AddRange(products);
		context.SaveChanges();

		return $"Successfully imported {products.Count}";
	}

	// 03. Import Categories
	public static string ImportCategories(ProductShopContext context, string inputJson)
	{
		var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
			.Where(c => c.Name is not null)
			.ToList();

		context.Categories.AddRange(categories);
		context.SaveChanges();

		return $"Successfully imported {categories.Count}";
	}

	// 04. Import Categories and Products
	public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
	{
		var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

		context.CategoriesProducts.AddRange(categoryProducts);
		context.SaveChanges();

		return $"Successfully imported {categoryProducts.Count}";
	}

	// 05. Export Products In Range
	public static string GetProductsInRange(ProductShopContext context)
	{
		var productsInRange = context.Products
			.Where(p => p.Price >= 500 && p.Price <= 1000)
			.Select(p => new
			{
				name = p.Name,
				price = p.Price,
				seller = $"{p.Seller.FirstName} {p.Seller.LastName}",
			})
			.ToList()
			.OrderBy(x => x.price);

		return JsonConvert.SerializeObject(productsInRange, Formatting.Indented);
	}

	// 06. Export Sold Products
	public static string GetSoldProducts(ProductShopContext context)
	{
		var users = context.Users
			.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
			.Select(u => new
			{
				firstName = u.FirstName,
				lastName = u.LastName,
				soldProducts = u.ProductsSold
					.Select(p => new
					{
						name = p.Name,
						price = p.Price,
						buyerFirstName = p.Buyer!.FirstName,
						buyerLastName = p.Buyer.LastName
					})
					.ToList()
			})
			.ToList()
			.OrderBy(x => x.lastName)
			.ThenBy(x => x.firstName);

		return JsonConvert.SerializeObject(users, Formatting.Indented);
	}

	// 07. Export Categories By Products Count 
	public static string GetCategoriesByProductsCount(ProductShopContext context)
	{
		var categoriesByproductsCount = context.Categories
			.OrderByDescending(c => c.CategoriesProducts.Count)
			.Select(c => new
			{
				category = c.Name,
				productsCount = c.CategoriesProducts.Count,
				averagePrice = c.CategoriesProducts
					.Select(cp => cp.Product.Price).Average().ToString("f2"),
				totalRevenue = c.CategoriesProducts
					.Select(cp => cp.Product.Price).Sum().ToString("f2")
			})
			.ToList();

		return JsonConvert.SerializeObject(categoriesByproductsCount, Formatting.Indented);
	}

	// 08. Export Users and Products
	public static string GetUsersWithProducts(ProductShopContext context)
	{
		var usersAndProducts = context.Users
			.Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
			.Select(u => new
			{
				firstName = u.FirstName,
				lastName = u.LastName,
				age = u.Age,
				soldProducts = u.ProductsSold
					.Where(p => p.BuyerId != null)
					.Select(p => new
					{
						name = p.Name,
						price = p.Price
					})
					.ToArray()
			})
			.ToArray()
			.OrderByDescending(x => x.soldProducts.Length);

		var usersWithUsersCount = new
		{
			usersCount = usersAndProducts.Count(),
			users = usersAndProducts
				.Select(u => new { 
					u.firstName,
					u.lastName,
					u.age,
					soldProducts = new
					{
						count = u.soldProducts.Length,
						products = u.soldProducts
					}
				})
		};

		return JsonConvert.SerializeObject(usersWithUsersCount, new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented
		});
	}


}