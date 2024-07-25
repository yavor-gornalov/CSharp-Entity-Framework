using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.Data.Models;

public class District
{
	public District()
	{
		Properties = new HashSet<Property>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(DistrictNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(DistrictPostalCodeLength)]
	public string PostalCode { get; set; } = null!;

	[Required]
	public Region Region { get; set; }

	public virtual ICollection<Property> Properties { get; set; }
}
