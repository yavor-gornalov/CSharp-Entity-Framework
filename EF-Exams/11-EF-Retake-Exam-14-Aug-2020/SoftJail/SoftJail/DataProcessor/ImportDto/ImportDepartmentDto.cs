using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportDepartmentDto
{
	[Required]
	[StringLength(DepartmentNameMaxLength, MinimumLength = DepartmentNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	public ICollection<ImportCellDto> Cells { get; set; }
}
