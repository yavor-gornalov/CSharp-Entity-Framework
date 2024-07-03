using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Artillery.Common.GlobalConstants;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Shell")]
public class ImportShellDto
{
	[Required]
	[XmlElement("ShellWeight")]
	[Range(ShellWeightLowLimit, ShellWeightHighLimit)]
	public double ShellWeight { get; set; }

	[Required]
	[XmlElement("Caliber")]
	[MinLength(ShellCaliberMinLength)]
	[MaxLength(ShellCaliberMaxLength)]
	public string Caliber { get; set; } = null!;
}
