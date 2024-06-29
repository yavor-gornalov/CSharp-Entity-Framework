using System.ComponentModel.DataAnnotations;
using static Medicines.Common.ValidationConstants;

namespace Medicines.Data.Models;

public class Pharmacy
{
	public Pharmacy()
	{
		Medicines = new HashSet<Medicine>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(PharmacyNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(PharmacyPhoneNumberLength)]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	public bool IsNonStop { get; set; }

	public virtual ICollection<Medicine> Medicines { get; set; }

}
