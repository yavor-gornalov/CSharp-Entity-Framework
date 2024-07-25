using Medicines.Data.Models.Enums;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos;

[XmlType("Patient")]
public class ExportPatientDto
{
	[XmlAttribute("Gender")]
	public string Gender { get; set; } = null!;

	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[XmlElement("AgeGroup")]
	public AgeGroup AgeGroup { get; set; }

	[XmlArray("Medicines")]
	public ExportMedicineDto[] Medicines { get; set; } = null!;
}
