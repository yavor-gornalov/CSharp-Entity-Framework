using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Trucks.Common.ValidationConstants;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Despatcher")]
public class ImportDespatcherDto
{
	[Required]
	[XmlElement("Name")]
	[StringLength(DespatcherNameMaxLength, MinimumLength = DespatcherNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("Position")]
	public string Position { get; set; } = null !;

	[XmlArray("Trucks")]
	public ImportTruckDto[] Trucks { get; set; } = null!;
}
