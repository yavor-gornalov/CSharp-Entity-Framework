using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Common.GlobalConstants;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType("Medicine")]
public class ImportMedicineDto
{
	[XmlAttribute("category")]
	[Required]
	[EnumDataType(typeof(Category))]
	public int Category { get; set; }

	[XmlElement("Name")]
	[Required]
	[StringLength(MedicineNameMaxLength, MinimumLength = MedicineNameMinLength)]
	public string Name { get; set; } = null!;

	[XmlElement("Price")]
	[Required]
	[Range(typeof(decimal), MedicinePriceLowLimit, MedicinePriceHighLimit)]
	public decimal Price { get; set; }

	[XmlElement("ProductionDate")]
	[Required]
	public string ProductionDate { get; set; } = null!;


	[XmlElement("ExpiryDate")]
	[Required]
	public string ExpiryDate { get; set; } = null!;

	[XmlElement("Producer")]
	[Required]
	[StringLength(MedicineProducerMaxLength, MinimumLength = MedicineProducerMinLength)]
	public string Producer { get; set; }
}