namespace Invoices.DataProcessor
{
	using Invoices.Data;
	using Invoices.DataProcessor.ExportDto;
	using Invoices.Helpers;
	using Newtonsoft.Json;
	using System.Diagnostics;
	using System.Globalization;
	using System.Xml.Linq;

	public class Serializer
	{
		public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
		{
			var clients = context.Clients
				.Where(c => c.Invoices.Any(i => i.IssueDate > date))
				.Select(c => new ExportClientDto
				{
					InvoicesCount = c.Invoices.Count(),
					ClientName = c.Name,
					VatNumber = c.NumberVat,
					Invoices = c.Invoices
					.OrderBy(i => i.IssueDate)
					.ThenByDescending(i => i.DueDate)
					.Select(i => new ExportInvoiceDto
					{
						InvoiceNumber = i.Number,
						InvoiceAmount = i.Amount,
						DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
						Currency = i.CurrencyType.ToString(),
					})
					.ToArray()
				})
				.OrderByDescending(c => c.InvoicesCount)
				.ThenBy(c => c.ClientName)
				.ToList();

			return XmlSerializationHelper.Serialize(clients, "Clients");
		}

		public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
		{
			var products = context.Products
				.Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
				.Select(p => new
				{
					Name = p.Name,
					Price = p.Price,
					Category = p.CategoryType.ToString(),
					Clients = p.ProductsClients
					.Where(pc => pc.Client.Name.Length >= nameLength)
					.AsEnumerable()
					.Select(pc => new
					{
						Name = pc.Client.Name,
						NumberVat = pc.Client.NumberVat,
					})
					.OrderBy(c => c.Name)
					.ToArray(),
				})
				.OrderByDescending(p => p.Clients.Count())
				.ThenBy(p => p.Name)
				.Take(5)
				.ToList();

			return JsonConvert.SerializeObject(products, Formatting.Indented);
		}
	}
}