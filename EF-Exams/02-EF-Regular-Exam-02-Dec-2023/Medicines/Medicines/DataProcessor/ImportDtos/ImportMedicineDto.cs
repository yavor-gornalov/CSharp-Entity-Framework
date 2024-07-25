using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using static Medicines.Common.ValidationConstants;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType("Medicine")]
public class ImportMedicineDto
{
	[Required]
	[XmlAttribute("category")]
	[EnumDataType(typeof(Category))]
	public int Category { get; set; }

	[Required]
	[XmlElement("Name")]
	[StringLength(MedicineNameMaxLength, MinimumLength = MedicineNameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("Price")]
	[Range(MedicinePriceLowLimit, MedicinePriceHighLimit)]
	public decimal Price { get; set; }

	[Required]
	[XmlElement("ProductionDate")]
	public string? ProductionDate { get; set; } = null!;

	[Required]
	[XmlElement("ExpiryDate")]
	public string? ExpiryDate { get; set; } = null!;

	[Required]
	[StringLength(MedicineProducerNameMaxLength, MinimumLength = MedicineProducerNameMinLength)]
	public string Producer { get; set; } = null!;
}
