namespace Artillery.DataProcessor
{
	using Artillery.Data;
	using Artillery.Data.Models;
	using Artillery.Data.Models.Enums;
	using Artillery.DataProcessor.ImportDto;
	using Artillery.Helpers;
	using Newtonsoft.Json;
	using System.ComponentModel.DataAnnotations;
	using System.Text;

	public class Deserializer
	{
		private const string ErrorMessage =
			"Invalid data.";
		private const string SuccessfulImportCountry =
			"Successfully import {0} with {1} army personnel.";
		private const string SuccessfulImportManufacturer =
			"Successfully import manufacturer {0} founded in {1}.";
		private const string SuccessfulImportShell =
			"Successfully import shell caliber #{0} weight {1} kg.";
		private const string SuccessfulImportGun =
			"Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

		public static string ImportCountries(ArtilleryContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var countriesInfo = XmlSerializationHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

			var countries = new List<Country>();

			foreach (var countryDto in countriesInfo)
			{
				if (!IsValid(countryDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var country = new Country
				{
					CountryName = countryDto.CountryName,
					ArmySize = countryDto.ArmySize,
				};

				countries.Add(country);
				sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
			}

			context.Countries.AddRange(countries);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportManufacturers(ArtilleryContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var manufacturersInfo = XmlSerializationHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

			var manufacturers = new List<Manufacturer>();
			var uniqueManufacturersNames = new List<string>();

			foreach (var manufacturerDto in manufacturersInfo)
			{
				if (!IsValid(manufacturerDto) ||
					uniqueManufacturersNames.Contains(manufacturerDto.ManufacturerName))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				string[] foundedInfo;
				try
				{
					foundedInfo = manufacturerDto.Founded
						.Split(", ", StringSplitOptions.RemoveEmptyEntries)
						.TakeLast(2)
						.ToArray();
				}
				catch (Exception ex)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var manufacturer = new Manufacturer
				{
					ManufacturerName = manufacturerDto.ManufacturerName,
					Founded = manufacturerDto.Founded,
				};

				uniqueManufacturersNames.Add(manufacturer.ManufacturerName);
				manufacturers.Add(manufacturer);
				sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturerDto.ManufacturerName, $"{foundedInfo[0]}, {foundedInfo[1]}"));
			}

			context.Manufacturers.AddRange(manufacturers);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportShells(ArtilleryContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var shellsInfo = XmlSerializationHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

			var shells = new List<Shell>();
			foreach (var shellDto in shellsInfo)
			{
				if (!IsValid(shellDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var shell = new Shell
				{
					ShellWeight = shellDto.ShellWeight,
					Caliber = shellDto.Caliber,
				};

				shells.Add(shell);
				sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
			}

			context.Shells.AddRange(shells);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportGuns(ArtilleryContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var gunsInfo = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

			var guns = new List<Gun>();

			foreach (var gunDto in gunsInfo)
			{
				GunType gunType;

				if (!IsValid(gunDto) || !Enum.TryParse(gunDto.GunType, true, out gunType))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var gun = new Gun
				{
					ManufacturerId = gunDto.ManufacturerId,
					GunWeight = gunDto.GunWeight,
					BarrelLength = gunDto.BarrelLength,
					NumberBuild = gunDto.NumberBuild,
					Range = gunDto.GunRange,
					GunType = gunType,
					ShellId = gunDto.ShellId,
				};

				foreach (var countryId in gunDto.CountriesIds.Select(i => i.Id).Distinct())
				{
					var countryGun = new CountryGun
					{
						CountryId = countryId,
						Gun = gun
					};

					gun.CountriesGuns.Add(countryGun);
				}

				guns.Add(gun);
				sb.AppendLine(string.Format(SuccessfulImportGun, gunType, gun.GunWeight, gun.BarrelLength));
			}

			context.Guns.AddRange(guns);
			context.SaveChanges();

			return sb.ToString().Trim();
		}
		private static bool IsValid(object obj)
		{
			var validator = new ValidationContext(obj);
			var validationRes = new List<ValidationResult>();

			var result = Validator.TryValidateObject(obj, validator, validationRes, true);
			return result;
		}
	}
}