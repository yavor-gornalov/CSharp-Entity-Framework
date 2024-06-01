using P03_SalesDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models;

public class Store
{
    public Store()
    {
        this.Sales = new List<Sale>();
    }

    [Key]
    public int StoreId { get; set; }

    [Required, MaxLength(GlobalConstants.StoreNameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; }

}
