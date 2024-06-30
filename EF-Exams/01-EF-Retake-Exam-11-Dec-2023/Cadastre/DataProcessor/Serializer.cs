using Boardgames.Helpers;
using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
	public class Serializer
	{
		public static string ExportPropertiesWithOwners(CadastreContext dbContext)
		{
			var targetDate = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture);

			var propertiesInfo = dbContext.Properties
				.Where(p => DateTime.Compare(p.DateOfAcquisition, targetDate) >= 0)
				.OrderByDescending(p => p.DateOfAcquisition)
				.ThenBy(p => p.PropertyIdentifier)
				.Select(p => new
				{
					PropertyIdentifier = p.PropertyIdentifier,
					Area = p.Area,
					Address = p.Address,
					DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Owners = p.PropertiesCitizens
						.Select(ps => new
						{
							LastName = ps.Citizen.LastName,
							MaritalStatus = ps.Citizen.MaritalStatus.ToString()
						})
						.OrderBy(c => c.LastName)
						.ToList()
				})
				.ToList();

			return JsonConvert.SerializeObject(propertiesInfo, Formatting.Indented);
		}

		public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
		{
			var propertiesInfo = dbContext.Properties
				.Where(p => p.Area >= 100)
				.OrderByDescending(p => p.Area)
				.ThenBy(p => p.DateOfAcquisition)
				.Select(p => new ExportPropertyDto
				{
					PostalCode = p.District.PostalCode,
					PropertyIdentifier = p.PropertyIdentifier,
					Area = p.Area,
					DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
				})
				.ToList();

			return XmlSerializationHelper.Serialize(propertiesInfo, "Properties");
		}
	}
}
