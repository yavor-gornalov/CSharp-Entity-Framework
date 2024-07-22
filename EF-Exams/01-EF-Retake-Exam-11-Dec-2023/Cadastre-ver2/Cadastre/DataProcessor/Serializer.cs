using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Helpers;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
	public class Serializer
	{
		public static string ExportPropertiesWithOwners(CadastreContext dbContext)
		{
			var targetDate = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture);

			var owners = dbContext.Properties
				.Where(p => p.DateOfAcquisition >= targetDate)
				.OrderByDescending(p => p.DateOfAcquisition)
				.ThenBy(p => p.PropertyIdentifier)
				.AsEnumerable()
				.Select(p => new
				{
					p.PropertyIdentifier,
					p.Area,
					p.Address,
					DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Owners = p.PropertiesCitizens
						.Select(pc => new
						{
							pc.Citizen.LastName,
							MaritalStatus = pc.Citizen.MaritalStatus.ToString()
						})
						.OrderBy(c => c.LastName)
						.ToList()
				})
				.ToList();


			return JsonConvert.SerializeObject(owners, Formatting.Indented);
		}

		public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
		{
			var properties = dbContext.Properties
				.Where(p => p.Area >= 100)
				.OrderByDescending(p => p.Area)
				.ThenBy(p => p.DateOfAcquisition)
				.AsEnumerable()
				.Select(p => new ExportPropertyDto
				{
					PostalCode = p.District.PostalCode,
					PropertyIdentifier = p.PropertyIdentifier,
					Area = p.Area,
					DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
				})
				.ToList();

			return XmlSerializationHelper.Serialize(properties, "Properties");
		}
	}
}
