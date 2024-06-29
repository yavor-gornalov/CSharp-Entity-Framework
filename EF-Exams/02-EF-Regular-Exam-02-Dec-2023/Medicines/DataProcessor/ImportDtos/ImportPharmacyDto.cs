using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using static Medicines.Common.ValidationConstants;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType("Pharmacy")]
public class ImportPharmacyDto
{
	[Required]
	[XmlAttribute("non-stop")]
	public string IsNonStop { get; set; } = null!;

	[Required]
	[XmlElement("Name")]
	[StringLength(PharmacyNameMaxLength, MinimumLength = PharmacyNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("PhoneNumber")]
	[StringLength(PharmacyPhoneNumberLength, MinimumLength = PharmacyPhoneNumberLength)]
	[RegularExpression(PharmacyPhoneNumberRegex)]
	public string PhoneNumber { get; set; } = null!;

	[XmlArray("Medicines")]
	public ImportMedicineDto[] Medicines { get; set; } = null!;
}
