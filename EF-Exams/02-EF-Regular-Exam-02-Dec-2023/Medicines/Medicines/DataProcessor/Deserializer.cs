namespace Medicines.DataProcessor
{
	using Boardgames.Helpers;
	using Medicines.Data;
	using Medicines.Data.Models;
	using Medicines.Data.Models.Enums;
	using Medicines.DataProcessor.ImportDtos;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid Data!";
		private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
		private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

		public static string ImportPatients(MedicinesContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var patients = new List<Patient>();

			var info = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

			foreach (var patientDto in info)
			{
				if (!IsValid(patientDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var patient = new Patient
				{
					FullName = patientDto.FullName,
					AgeGroup = patientDto.AgeGroup,
					Gender = patientDto.Gender,
				};

				foreach (int medicineId in patientDto.MedicineIds)
				{
					if (patient.PatientsMedicines.Any(m => m.MedicineId == medicineId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var patientMedicine = new PatientMedicine
					{
						MedicineId = medicineId,
						Patient = patient,
					};

					patient.PatientsMedicines.Add(patientMedicine);
				}

				patients.Add(patient);
				sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count()));
			}

			context.Patients.AddRange(patients);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportPharmacies(MedicinesContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var info = XmlSerializationHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

			var pharmacies = new List<Pharmacy>();

			foreach (var pharmacyDto in info)
			{
				if (!IsValid(pharmacyDto)
					|| !bool.TryParse(pharmacyDto.IsNonStop, out bool isNonStop))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var pharmacy = new Pharmacy
				{
					Name = pharmacyDto.Name,
					PhoneNumber = pharmacyDto.PhoneNumber,
					IsNonStop = isNonStop,
				};

				foreach (var medicineDto in pharmacyDto.Medicines)
				{
					DateTime productionDate;
					DateTime expiryDate;

					try
					{
						productionDate = DateTime.ParseExact(medicineDto.ProductionDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
						expiryDate = DateTime.ParseExact(medicineDto.ExpiryDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (!IsValid(medicineDto)
						|| DateTime.Compare(productionDate, expiryDate) >= 0)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					if (pharmacy.Medicines.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var medicine = new Medicine
					{
						Category = (Category)medicineDto.Category,
						Name = medicineDto.Name,
						Price = medicineDto.Price,
						ProductionDate = productionDate,
						ExpiryDate = expiryDate,
						Producer = medicineDto.Producer,
					};

					pharmacy.Medicines.Add(medicine);
				}

				pharmacies.Add(pharmacy);
				sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count()));
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
