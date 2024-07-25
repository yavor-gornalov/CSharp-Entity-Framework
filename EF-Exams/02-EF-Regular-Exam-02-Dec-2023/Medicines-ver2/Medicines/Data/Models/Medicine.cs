using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Medicines.Common.GlobalConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.Data.Models;

public class Medicine
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(MedicineNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	[Column(TypeName = "decimal(18,2)")]
	public decimal Price { get; set; }

	[Required]
	public Category Category { get; set; }

	[Required]
	public DateTime ProductionDate { get; set; }

	[Required]
	public DateTime ExpiryDate { get; set; }

	[Required]
	[MaxLength(MedicineProducerMaxLength)]
	public string Producer { get; set; } = null!;

	[Required]
	public int PharmacyId { get; set; }

	[ForeignKey(nameof(PharmacyId))]
	public Pharmacy Pharmacy { get; set; } = null!;

	public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new HashSet<PatientMedicine>();
}

//•	Id – integer, Primary Key
//•	Name – text with length[3, 150] (required)
//•	Price – decimal in range[0.01…1000.00] (required)
//•	Category – Category enum (Analgesic = 0, Antibiotic, Antiseptic, Sedative, Vaccine) (required)
//•	ProductionDate – DateTime(required)
//•	ExpiryDate – DateTime(required)
//•	Producer – text with length[3, 100] (required)
//•	PharmacyId – integer, foreign key(required)
//•	Pharmacy – Pharmacy
//•	PatientsMedicines - collection of type PatientMedicine
