namespace Invoices.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Diagnostics;
	using System.Diagnostics.Metrics;
	using System.Globalization;
	using System.Text;
	using System.Xml.Linq;
	using AutoMapper.Execution;
	using Boardgames.Helpers;
	using Invoices.Data;
	using Invoices.Data.Models;
	using Invoices.Data.Models.Enums;
	using Invoices.DataProcessor.ImportDto;
	using Microsoft.VisualBasic;
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

			var clientsInfo = XmlSerializationHelper.Deserialize<ImportXmlClientDto[]>(xmlString, "Clients");

			var clients = new List<Client>();

			foreach (var clientDto in clientsInfo)
			{
				if (!IsValid(clientDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var client = new Client
				{
					Name = clientDto.Name,
					NumberVat = clientDto.NumberVat,
				};

				foreach (var addressDto in clientDto.Addresses)
				{
					if (!IsValid(addressDto))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var address = new Address
					{
						StreetName = addressDto.StreetName,
						StreetNumber = addressDto.StreetNumber,
						PostCode = addressDto.PostCode,
						City = addressDto.City,
						Country = addressDto.Country,
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

			var invoicesInfo = JsonConvert.DeserializeObject<ImportJsonInvoiceDto[]>(jsonString);
			var invoices = new List<Invoice>();

			var validClietIds = context.Clients.Select(c => c.Id).ToList();


			foreach (var invoiceDto in invoicesInfo)
			{

				DateTime.TryParse(invoiceDto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate);
				DateTime.TryParse(invoiceDto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

				if (!IsValid(invoiceDto)
						|| DateTime.Compare(issueDate, dueDate) >= 0
						|| !validClietIds.Contains(invoiceDto.ClientId))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var invoice = new Invoice
				{
					Number = invoiceDto.Number,
					IssueDate = issueDate,
					DueDate = dueDate,
					Amount = invoiceDto.Amount,
					CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
					ClientId = invoiceDto.ClientId,
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

			var validClietIds = context.Clients.Select(c => c.Id).ToList();

			var productsInfo = JsonConvert.DeserializeObject<ImportJsonProductDto[]>(jsonString);

			var products = new List<Product>();

			foreach (var productDto in productsInfo)
			{
				if (!IsValid(productDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var product = new Product
				{
					Name = productDto.Name,
					Price = productDto.Price,
					CategoryType = (CategoryType)productDto.CategoryType,
				};

				foreach (var clientId in productDto.ClientIds.Distinct())
				{
					if (!validClietIds.Contains(clientId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
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
