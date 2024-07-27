namespace Invoices.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using Invoices.Data;
	using Invoices.Data.Models;
	using Invoices.Data.Models.Enums;
	using Invoices.DataProcessor.ImportDto;
	using Invoices.Helpers;
	using Newtonsoft.Json;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedClients
			= "Successfully imported client {0}.";

		private const string SuccessfullyImportedInvoices
			= "Successfully imported invoice with number {0}.";

		private const string SuccessfullyImportedProducts
			= "Successfully imported product - {0} with {1} clients.";


		public static string ImportClients(InvoicesContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var clientsInfo = XmlSerializationHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");

			var clients = new List<Client>();

			foreach (var c in clientsInfo)
			{
				if (!IsValid(c))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var client = new Client
				{
					Name = c.Name,
					NumberVat = c.NumberVat,
				};

				foreach (var a in c.Addresses)
				{
					if (!IsValid(a))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var address = new Address
					{
						StreetName = a.StreetName,
						StreetNumber = a.StreetNumber,
						City = a.City,
						PostCode = a.PostCode,
						Country = a.Country,
					};

					client.Addresses.Add(address);
				}

				clients.Add(client);
				sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));

			}

			context.Clients.AddRange(clients);
			context.SaveChanges();

			return sb.ToString().Trim();
		}


		public static string ImportInvoices(InvoicesContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var invoicesInfo = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

			var validClientsIds = context.Clients.Select(c => c.Id).ToList();

			var invoices = new List<Invoice>();

			foreach (var i in invoicesInfo)
			{
				bool isIssueDateValid = DateTime.TryParseExact(i.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);
				bool isDueDateValid = DateTime.TryParseExact(i.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);
				bool isClientValid = validClientsIds.Contains(i.ClientId);

				if (!IsValid(i) ||
					!isIssueDateValid ||
					!isDueDateValid ||
					!isClientValid ||
					dueDate < issueDate)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var invoice = new Invoice
				{
					Number = i.Number,
					IssueDate = issueDate,
					DueDate = dueDate,
					Amount = i.Amount,
					CurrencyType = (CurrencyType)i.CurrencyType,
					ClientId = i.ClientId,
				};

				invoices.Add(invoice);
				sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
			}

			context.Invoices.AddRange(invoices);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportProducts(InvoicesContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var productsInfo = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

			var validClientsIds = context.Clients.Select(c => c.Id).ToList();

			var products = new List<Product>();

			foreach(var p in productsInfo)
			{
				if(!IsValid(p))
				{
					sb.AppendLine(ErrorMessage); 
					continue;
				}

				var product = new Product
				{
					Name = p.Name,
					Price = p.Price,
					CategoryType = (CategoryType)p.CategoryType,
				};

				foreach (var clientId in p.Clients	)
				{
					if (!validClientsIds.Contains(clientId))
					{
						sb.AppendLine(ErrorMessage);
						continue ;
					}

					var productClient = new ProductClient
					{
						Product = product,
						ClientId = clientId,
					};

					product.ProductsClients.Add(productClient);
				}

				products.Add(product);
				sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
			}

			context.Products.AddRange(products);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}
