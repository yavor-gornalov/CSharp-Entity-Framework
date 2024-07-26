using AutoMapper.Configuration.Annotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos;

[XmlType("Patient")]
public class ExportPatientDto
{
	[XmlAttribute("Gender")]
	public string Gender { get; set; } = null!;

	[XmlElement("Name", Order = 1)]
	public string Name { get; set; } = null!;

	[XmlElement("AgeGroup", Order = 2)]
	public string AgeGroup = null!;

	[XmlArray("Medicines", Order = 3)]
	public ExportMedicineDto[] Medicines { get; set; } = null!;
}
