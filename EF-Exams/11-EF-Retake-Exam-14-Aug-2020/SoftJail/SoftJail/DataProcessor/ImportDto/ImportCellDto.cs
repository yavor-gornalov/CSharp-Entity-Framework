using System.ComponentModel.DataAnnotations;
using static SoftJail.Shared.GlobalConstants;

namespace SoftJail.DataProcessor.ImportDto;

public class ImportCellDto
{
    [Required]
    [Range(CellNumberLowLimit, CellNumberHighLimit)]
    public int CellNumber { get; set; }

    [Required]
    public bool HasWindow { get; set; }
}