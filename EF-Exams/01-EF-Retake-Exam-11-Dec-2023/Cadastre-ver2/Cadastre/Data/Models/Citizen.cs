using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.GlobalConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Cadastre.Data.Models;

public class Citizen
{
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

	public ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new HashSet<PropertyCitizen>();
}

//•	Id – integer, Primary Key
//•	FirstName – text with length[2, 30] (required)
//•	LastName – text with length[2, 30] (required)
//•	BirthDate – DateTime(required)
//•	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
//•	PropertiesCitizens - collection of type PropertyCitizen
