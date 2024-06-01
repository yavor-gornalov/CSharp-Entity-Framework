using P03_SalesDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models;

public class Customer
{
    public Customer()
    {
        this.Sales = new List<Sale>();
    }

    [Key]
    public int CustomerId { get; set; }

    [Required, MaxLength(GlobalConstants.CustomerNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(GlobalConstants.CustomerEmailMaxLength)]
    public string Email { get; set; } = null!;

    [Required]
    public string CreditCardNumber { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; }
}
