using System.ComponentModel.DataAnnotations;
using static SoftJail.Common.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportDepartmetDto
{
	[Required]
	[StringLength(DepartmentNameMaxLength, MinimumLength = DepartmentNameMinLength)]
	public string Name { get; set; } = null!;

	public ImportCellDto[] Cells { get; set; } = null!;
}