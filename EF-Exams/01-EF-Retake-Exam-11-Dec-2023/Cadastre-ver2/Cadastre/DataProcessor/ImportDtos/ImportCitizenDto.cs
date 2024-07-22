using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Cadastre.Common.GlobalConstants;

namespace Cadastre.DataProcessor.ImportDtos;

public class ImportCitizenDto
{
	[Required]
	[JsonProperty("FirstName")]
	[StringLength(CitizenFirstNameMaxLength, MinimumLength = CitizenFirstNameMinLength)]
	public string FirstName { get; set; } = null!;

	[Required]
	[JsonProperty("LastName")]
	[StringLength(CitizenLastNameMaxLength, MinimumLength = CitizenLastNameMinLength)]
	public string LastName { get; set; } = null!;

	[Required]
	[JsonProperty("BirthDate")]
	public string BirthDate { get; set; } = null!;

	[Required]
	[JsonProperty("MaritalStatus")]
	[EnumDataType(typeof(MaritalStatus))]
	public string MaritalStatus { get; set; } = null!;

	[JsonProperty("Properties")]
	public ICollection<int> PropertiesIds { get; set; } = new HashSet<int>();
}

//•	Id – integer, Primary Key
//•	FirstName – text with length[2, 30] (required)
//•	LastName – text with length[2, 30] (required)
//•	BirthDate – DateTime(required)
//•	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
//•	PropertiesCitizens - collection of type PropertyCitizen