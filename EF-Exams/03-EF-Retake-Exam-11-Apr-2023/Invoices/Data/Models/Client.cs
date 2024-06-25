using System.ComponentModel.DataAnnotations;
using static Invoices.Common.ValidationConstants;

namespace Invoices.Data.Models;

public class Client
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(ClientNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(ClientVatMaxLength)]
	public string NumberVat { get; set; } = null!;

	public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

	public ICollection<Address> Addresses { get; set; } = new List<Address>();

	public ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();
}