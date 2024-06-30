using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using static Cadastre.Common.ValidationConstants;

namespace Cadastre.DataProcessor.ImportDtos;

public class ImportCitizenDto
{
	public ImportCitizenDto()
	{
		PropertyIds = new HashSet<int>();
	}

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
	public string MaritalStatus { get; set; } = null!;

	[JsonProperty("Properties")]
	public ICollection<int> PropertyIds { get; set; }
}
