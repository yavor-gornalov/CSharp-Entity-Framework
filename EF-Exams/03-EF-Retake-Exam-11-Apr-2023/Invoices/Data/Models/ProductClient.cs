namespace Invoices.Data.Models;

public class ProductClient
{
	public int ProductId { get; set; }
	public Product Product { get; set; } = null!;

	public int ClientId { get; set; }
	public Client Client { get; set; } = null!;
}