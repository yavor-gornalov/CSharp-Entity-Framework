using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.ValidationConstants;

namespace Cadastre.Data.Models;

public class Citizen
{
	public Citizen()
	{
		PropertiesCitizens = new HashSet<PropertyCitizen>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(CitizenFirstNameMaxLength)]
	public string FirstName { get; set; } = null!;

	[Required]
	[MaxLength(CitizenLastNameMaxLength)]
	public string LastName { get; set; } = null!;

	[Required]
	public DateTime BirthDate { get; set; }

	[Required]
	public MaritalStatus MaritalStatus { get; set; }

	public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; }
}