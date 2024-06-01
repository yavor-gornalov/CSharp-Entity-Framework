using P03_SalesDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models;

public class Product
{
    public Product()
    {
        this.Sales = new List<Sale>();
    }

    [Key]
    public int ProductId { get; set; }

    [Required, MaxLength(GlobalConstants.ProductNameMaxLength)]
    public string Name { get; set; } = null!;

    public double Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Sale> Sales { get; set; }
}
