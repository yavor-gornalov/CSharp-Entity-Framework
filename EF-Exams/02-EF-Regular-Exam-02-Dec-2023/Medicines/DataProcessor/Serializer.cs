namespace Medicines.DataProcessor
{
	using Boardgames.Helpers;
	using Medicines.Data;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ExportDtos;
	using Newtonsoft.Json;
	using System.Globalization;

	public class Serializer
	{
		public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
		{
			var targetDate = DateTime.Parse(date);

			var patientsInfo = context.Patients
				.Where(p => p.PatientsMedicines.Any(
					m => DateTime.Compare(m.Medicine.ProductionDate, targetDate) > 0))
				.Select(p => new ExportPatientDto
				{
					Gender = p.Gender.ToString().ToLower(),
					Name = p.FullName,
					AgeGroup = p.AgeGroup,
					Medicines = p.PatientsMedicines
						.Where(pm => DateTime.Compare(pm.Medicine.ProductionDate, targetDate) > 0)
						.Select(pm => new ExportMedicineDto
						{
							Category = pm.Medicine.Category.ToString().ToLower(),
							Name = pm.Medicine.Name,
							Price = pm.Medicine.Price.ToString("f2"),
							Producer = pm.Medicine.Producer,
							BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
							ExpiryDate = pm.Medicine.ExpiryDate,
							DecimalPrice = pm.Medicine.Price
						})
						.OrderByDescending(m => m.ExpiryDate)
						.ThenBy(m => m.DecimalPrice)
						.ToArray()
				})
				.OrderByDescending(p => p.Medicines.Length)
				.ThenBy(p => p.Name)
				.ToList();

			return XmlSerializationHelper.Serialize(patientsInfo, "Patients");
		}

		public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
		{
			var medicinesInfo = context.Medicines
				.Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop == true)
				.OrderBy(m => m.Price)
				.ThenBy(m => m.Name)
				.Select(m => new
				{
					m.Name,
					Price = m.Price.ToString("f2"),
					Pharmacy = new
					{
						m.Pharmacy.Name,
						m.Pharmacy.PhoneNumber,
					}
				})
				.ToList();

			return JsonConvert.SerializeObject(medicinesInfo, Formatting.Indented);
		}
	}
}
