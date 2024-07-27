namespace Invoices.DataProcessor
{
	using Boardgames.Helpers;
	using Invoices.Data;
	using Invoices.DataProcessor.ExportDto;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using System.Globalization;

	public class Serializer
	{
		public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
		{
			var clients = context.Clients
				.Where(c => c.Invoices.Any(i => DateTime.Compare(i.IssueDate, date) > 0))
				.Select(c => new ExportXmlClientDto
				{
					InvoicesCount = c.Invoices.Count(),
					Name = c.Name,
					VatNumber = c.NumberVat,
					Invoices = c.Invoices
						.OrderBy(i => i.IssueDate)
						.ThenByDescending(i => i.DueDate)
						.Select(i => new ExportXmlInvoiceDto
						{
							Number = i.Number,
							Amount = i.Amount,
							DueDate = i.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
							CurrencyType = i.CurrencyType.ToString(),
						})
						.ToArray()
				})
				.OrderByDescending(c => c.InvoicesCount)
				.ThenBy(c => c.Name)
				.ToList();

			return XmlSerializationHelper.Serialize(clients, "Clients");
		}

		public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
		{
			var products = context.Products
				.Where(p => p.ProductsClients.Any(
					pc => pc.Client.Name.Length >= nameLength))

				.Select(p => new ExportJsonProductDto
				{
					Name = p.Name,
					Price = p.Price,
					CategoryType = p.CategoryType.ToString(),
					Clients = p.ProductsClients
						.Where(pc => pc.Client.Name.Length >= nameLength)
						.Select(pc => new ExportJsonClientDto
						{
							Name = pc.Client.Name,
							NumberVat = pc.Client.NumberVat,
						})
						.OrderBy(c => c.Name)
						.ToList()
				})
				.OrderByDescending(p => p.Clients.Count)
				.ThenBy(p => p.Name)
				.Take(5)
				.AsNoTracking()
				.ToList();

			return JsonConvert.SerializeObject(products, Formatting.Indented);
		}
	}
}