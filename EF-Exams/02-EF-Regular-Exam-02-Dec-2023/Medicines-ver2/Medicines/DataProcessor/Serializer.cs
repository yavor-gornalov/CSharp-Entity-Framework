using static Medicines.Common.GlobalConstants;

namespace Medicines.DataProcessor
{
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ExportDtos;
	using Medicines.Helpers;
	using Newtonsoft.Json;
	using System.Globalization;

	public class Serializer
	{
		public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
		{
			var targetDate = DateTime.ParseExact(date, DateTimeDefaultFormat, CultureInfo.InvariantCulture);

			var patients = context.Patients
				.Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > targetDate))
				.Select(p => new ExportPatientDto
				{
					Gender = p.Gender.ToString().ToLower(),
					Name = p.FullName,
					AgeGroup = p.AgeGroup.ToString(),
					Medicines = p.PatientsMedicines
						.Where(pm => pm.Medicine.ProductionDate > targetDate)
						.OrderByDescending(pm => pm.Medicine.ExpiryDate)
						.ThenBy(pm => pm.Medicine.Price)
						.Select(pm => new ExportMedicineDto
						{
							Category = pm.Medicine.Category.ToString().ToLower(),
							Name = pm.Medicine.Name,
							Price = pm.Medicine.Price.ToString("f2"),
							BestBefore = pm.Medicine.ExpiryDate.ToString(DateTimeDefaultFormat, CultureInfo.InvariantCulture),
							Producer = pm.Medicine.Producer
						})
						.ToArray()
				})
				.OrderByDescending(p => p.Medicines.Count())
				.ThenBy(p => p.Name)
				.ToList();

			return XmlSerializationHelper.Serialize(patients, "Patients");
		}

		public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
		{
			var medicines = context.Medicines
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
						m.Pharmacy.PhoneNumber
					}
				})
				.ToList();

			return JsonConvert.SerializeObject(medicines, Formatting.Indented);
		}
	}
}
