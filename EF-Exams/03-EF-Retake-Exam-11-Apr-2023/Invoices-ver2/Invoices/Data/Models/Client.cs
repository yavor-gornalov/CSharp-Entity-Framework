using System.ComponentModel.DataAnnotations;
using static Invoices.Common.GlobalConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.Data.Models;

public class Client
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(ClientNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(ClientNumberVatMaxLength)]
	public string NumberVat { get; set; } = null!;

	public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();

	public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

	public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
}

//•	Id – integer, Primary Key
//•	Name – text with length[10…25] (required)
//•	NumberVat – text with length[10…15] (required)
//•	Invoices – collection of type Invoicе
//•	Addresses – collection of type Address
//•	ProductsClients – collection of type ProductClient
