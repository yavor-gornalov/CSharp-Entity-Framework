namespace Cadastre.DataProcessor
{
	using Boardgames.Helpers;
	using Cadastre.Data;
	using Cadastre.Data.Enumerations;
	using Cadastre.Data.Models;
	using Cadastre.DataProcessor.ImportDtos;
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
			var sb = new StringBuilder();

			var districts = new List<District>();

			var districtsInfo = XmlSerializationHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

			var usedPropertyIdentifiers = new List<string>();

			var usedPropertyAddresses = new List<string>();

			foreach (var districtDto in districtsInfo)
			{
				if (!IsValid(districtDto)
					|| districts.Any(d => d.Name == districtDto.Name))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}
				var district = new District
				{
					Region = districtDto.Region,
					Name = districtDto.Name,
					PostalCode = districtDto.PostalCode,
				};

				foreach (var propertyDto in districtDto.Properties)
				{
					if (!IsValid(propertyDto) ||
						usedPropertyIdentifiers.Contains(propertyDto.PropertyIdentifier) ||
						usedPropertyAddresses.Contains(propertyDto.Address))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var property = new Property
					{
						PropertyIdentifier = propertyDto.PropertyIdentifier,
						Area = propertyDto.Area,
						Details = propertyDto.Details,
						Address = propertyDto.Address,
						DateOfAcquisition = DateTime.ParseExact(propertyDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture)
					};
					usedPropertyIdentifiers.Add(property.PropertyIdentifier);
					usedPropertyAddresses.Add(property.Address);
					district.Properties.Add(property);
				}
				districts.Add(district);
				sb.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count));
			}

			dbContext.Districts.AddRange(districts);
			dbContext.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
		{
			var sb = new StringBuilder();

			var citizens = new List<Citizen>();

			var citizensInfo = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);

			var validPropertyIds = dbContext.Properties.Select(p => p.Id).ToList();


			foreach (var citizenDto in citizensInfo)
			{
				if (!IsValid(citizenDto) || !Enum.TryParse(citizenDto.MaritalStatus, out MaritalStatus maritialStatus))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var citizen = new Citizen
				{
					FirstName = citizenDto.FirstName,
					LastName = citizenDto.LastName,
					BirthDate = DateTime.ParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
					MaritalStatus = maritialStatus,
				};

				foreach (int propertyId in citizenDto.PropertyIds)
				{
					if (!validPropertyIds.Contains(propertyId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var propertyCitizen = new PropertyCitizen
					{
						PropertyId = propertyId,
						Citizen = citizen,
					};

					citizen.PropertiesCitizens.Add(propertyCitizen);
				}

				citizens.Add(citizen);
				sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));
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
