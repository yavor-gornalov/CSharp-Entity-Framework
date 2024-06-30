using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.Data.Models;

public class Property
{
	public Property()
	{
		PropertiesCitizens = new HashSet<PropertyCitizen>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(PropertyIdentifierMaxLength)]
	public string PropertyIdentifier { get; set; } = null!;

	[Required]
	public int Area { get; set; }

	[MaxLength(PropertyDetailsMaxLength)]
	public string? Details { get; set; }

	[Required]
	[MaxLength(PropertyAddressMaxLength)]
	public string Address { get; set; } = null!;

	[Required]
	public DateTime DateOfAcquisition { get; set; }

	[Required]
	[ForeignKey(nameof(District))]
	public int DistrictId { get; set; }
	public virtual District District { get; set; } = null!;

	public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; }

}