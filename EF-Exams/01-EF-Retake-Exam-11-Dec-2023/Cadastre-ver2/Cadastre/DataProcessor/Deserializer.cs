namespace Cadastre.DataProcessor
{
	using Cadastre.Data;
	using Cadastre.Data.Enumerations;
	using Cadastre.Data.Models;
	using Cadastre.DataProcessor.ImportDtos;
	using Cadastre.Helpers;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;

	public class Deserializer
	{
		private const string ErrorMessage =
			"Invalid Data!";
		private const string SuccessfullyImportedDistrict =
			"Successfully imported district - {0} with {1} properties.";
		private const string SuccessfullyImportedCitizen =
			"Succefully imported citizen - {0} {1} with {2} properties.";

		public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
		{
			var districtsInfo = XmlSerializationHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

			if (districtsInfo == null)
			{
				return string.Empty;
			}

			var sb = new StringBuilder();

			var distrcts = new List<District>();

			var uniqueDistrictNames = new List<string>();

			foreach (var d in districtsInfo)
			{
				var isValidRegion = Enum.TryParse(d.Region, out Region region);

				if (!IsValid(d) || !isValidRegion || uniqueDistrictNames.Contains(d.Name))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var newDistrict = new District
				{
					Name = d.Name,
					PostalCode = d.PostalCode,
					Region = region,
				};

				var uniquePropertiesIdentifiers = new List<string>();
				var uniquePropertiesAddresses = new List<string>();
				foreach (var p in d.Properties)
				{
					var isDateValid = DateTime.TryParseExact(
						p.DateOfAcquisition,
						"dd/MM/yyyy",
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out DateTime dateOfAcquisition);

					if (!IsValid(p) ||
						!isDateValid ||
						uniquePropertiesIdentifiers.Contains(p.PropertyIdentifier) ||
						uniquePropertiesAddresses.Contains(p.Address))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var newProperty = new Property
					{
						PropertyIdentifier = p.PropertyIdentifier,
						Area = p.Area,
						Details = p.Details,
						Address = p.Address,
						DateOfAcquisition = dateOfAcquisition,
					};

					uniquePropertiesIdentifiers.Add(newProperty.PropertyIdentifier);
					uniquePropertiesAddresses.Add(newProperty.Address);
					newDistrict.Properties.Add(newProperty);
				}

				uniqueDistrictNames.Add(newDistrict.Name);
				distrcts.Add(newDistrict);
				sb.AppendLine(string.Format(SuccessfullyImportedDistrict, newDistrict.Name, newDistrict.Properties.Count));
			}

			dbContext.Districts.AddRange(distrcts);
			dbContext.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
		{
			var citizensInfo = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);

			if (citizensInfo == null)
			{
				return string.Empty;
			}

			var sb = new StringBuilder();

			var citizens = new List<Citizen>();

			foreach (var c in citizensInfo)
			{
				var isDateValid = DateTime.TryParseExact(
					c.BirthDate,
					"dd-MM-yyyy",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime birthDate);

				var isStatusValid = Enum.TryParse(c.MaritalStatus, out MaritalStatus maritalStatus);

				if (!IsValid(c) || !isDateValid || !isStatusValid)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var newCitizen = new Citizen
				{
					FirstName = c.FirstName,
					LastName = c.LastName,
					BirthDate = birthDate,
					MaritalStatus = maritalStatus
				};

				foreach (var propertyId in c.PropertiesIds.Distinct())
				{
					var newPropertyCitizen = new PropertyCitizen
					{
						PropertyId = propertyId,
						Citizen = newCitizen
					};

					newCitizen.PropertiesCitizens.Add(newPropertyCitizen);
				}

				citizens.Add(newCitizen);
				sb.AppendLine(string.Format(SuccessfullyImportedCitizen, newCitizen.FirstName, newCitizen.LastName, newCitizen.PropertiesCitizens.Count));
			}

			dbContext.Citizens.AddRange(citizens);
			dbContext.SaveChanges();

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
