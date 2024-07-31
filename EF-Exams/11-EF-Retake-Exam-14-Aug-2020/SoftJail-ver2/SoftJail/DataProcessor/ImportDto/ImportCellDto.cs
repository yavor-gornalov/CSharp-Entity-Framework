using System.ComponentModel.DataAnnotations;
using static SoftJail.Common.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportCellDto
{
	[Required]
	[Range(CellNumberLowLimit, CellNumberHighLimit)]
	public int CellNumber { get; set; }

	[Required]
	public string HasWindow { get; set; } = null!;
}