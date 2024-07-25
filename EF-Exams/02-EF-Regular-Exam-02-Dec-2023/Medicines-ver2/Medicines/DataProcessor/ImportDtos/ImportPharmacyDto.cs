using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Common.GlobalConstants;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType("Pharmacy")]
public class ImportPharmacyDto
{
	[XmlAttribute("non-stop")]
	[Required]
	public string IsNonStop { get; set; } = null!;

	[XmlElement("Name")]
	[Required]
	[StringLength(PharmacyNameMaxLength, MinimumLength = PharmacyNameMinLength)]
	public string Name { get; set; } = null!;

	[XmlElement("PhoneNumber")]
	[Required]
	[RegularExpression(PharmacyPhoneNumberRegex)]
	public string PhoneNumber { get; set; } = null!;

	[XmlArray("Medicines")]
	public ImportMedicineDto[] Medicines { get; set; } = null!;
}
