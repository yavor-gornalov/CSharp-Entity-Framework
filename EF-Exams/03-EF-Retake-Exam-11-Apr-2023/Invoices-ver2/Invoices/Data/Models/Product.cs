using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Invoices.Common.GlobalConstants;

namespace Invoices.Data.Models;

public class Product
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(ProductNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[Column(TypeName = "decimal(18,2)")]
	public decimal Price { get; set; }

	[Required]
	public CategoryType CategoryType { get; set; }

	public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
}

//•	Id – integer, Primary Key
//•	Name – text with length[9…30] (required)
//•	Price – decimal in range[5.00…1000.00] (required)
//•	CategoryType – enumeration of type CategoryType, with possible values(ADR, Filters, Lights, Others, Tyres) (required)
//•	ProductsClients – collection of type ProductClient
