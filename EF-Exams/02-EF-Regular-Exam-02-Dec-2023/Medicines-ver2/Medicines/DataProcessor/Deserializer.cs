namespace Medicines.DataProcessor
{
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ImportDtos;
	using Medicines.Helpers;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;

	using static Medicines.Common.GlobalConstants;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid Data!";
		private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
		private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

		public static string ImportPatients(MedicinesContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var patientsInfo = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

			var patients = new List<Patient>();

			foreach (var p in patientsInfo)
			{
				if (!IsValid(p))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var patient = new Patient
				{
					FullName = p.FullName,
					AgeGroup = (AgeGroup)p.AgeGroup,
					Gender = (Gender)p.Gender,
				};

				var patientMedicinesIds = new List<int>();
				foreach (var medicineId in p.Medicines)
				{
					if (patientMedicinesIds.Contains(medicineId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var patientMedicine = new PatientMedicine
					{
						Patient = patient,
						MedicineId = medicineId
					};

					patientMedicinesIds.Add(medicineId);
					patient.PatientsMedicines.Add(patientMedicine);
				}

				patients.Add(patient);
				sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
			}

			context.Patients.AddRange(patients);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportPharmacies(MedicinesContext context, string xmlString)
		{
			var pharmaciesInfo = XmlSerializationHelper
				.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

			var sb = new StringBuilder();

			var pharmacies = new List<Pharmacy>();

			foreach (var p in pharmaciesInfo)
			{
				bool isValidNonStop = bool.TryParse(p.IsNonStop, out bool isNonStop);

				if (!IsValid(p) || !isValidNonStop)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var pharmacy = new Pharmacy
				{
					Name = p.Name,
					PhoneNumber = p.PhoneNumber,
					IsNonStop = isNonStop,
				};

				var pharmacyMedicines = new List<Medicine>();

				foreach (var m in p.Medicines)
				{
					bool isProductionDateValid = DateTime.TryParseExact(
							m.ProductionDate,
							DateTimeDefaultFormat,
							CultureInfo.InvariantCulture,
							DateTimeStyles.None,
							out DateTime productionDate
						);

					bool isExpiryDateValid = DateTime.TryParseExact(
							m.ExpiryDate,
							DateTimeDefaultFormat,
							CultureInfo.InvariantCulture,
							DateTimeStyles.None,
							out DateTime expiryDate
						);

					bool isMediciceUnique = !pharmacyMedicines.Any(pm => pm.Name == m.Name && pm.Producer == m.Producer);

					if (!IsValid(m) ||
						!isProductionDateValid ||
						!isExpiryDateValid ||
						!isMediciceUnique ||
						productionDate >= expiryDate)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var medicine = new Medicine
					{
						Category = (Category)m.Category,
						Name = m.Name,
						Price = m.Price,
						ProductionDate = productionDate,
						ExpiryDate = expiryDate,
						Producer = m.Producer,
					};

					pharmacyMedicines.Add(medicine);
				}

				pharmacy.Medicines = pharmacyMedicines;
				pharmacies.Add(pharmacy);
				sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
			}

			context.Pharmacies.AddRange(pharmacies);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}
