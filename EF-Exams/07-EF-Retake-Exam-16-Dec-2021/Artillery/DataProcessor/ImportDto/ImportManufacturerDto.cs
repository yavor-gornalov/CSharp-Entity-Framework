using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Artillery.Common.GlobalConstants;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Manufacturer")]
public class ImportManufacturerDto
{
	[Required]
	[XmlElement("ManufacturerName")]
	[MinLength(ManufacturerNameMinLength)]
	[MaxLength(ManufacturerNameMaxLength)]
	public string ManufacturerName { get; set; } = null!;

	[Required]
	[XmlElement("Founded")]
	[MinLength(ManufacturerFoundedMinLength)]
	[MaxLength(ManufacturerFoundedMaxLength)]
	public string Founded { get; set; } = null!;
}
