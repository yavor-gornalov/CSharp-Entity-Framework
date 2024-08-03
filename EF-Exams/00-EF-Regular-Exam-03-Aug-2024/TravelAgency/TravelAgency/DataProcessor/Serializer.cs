using Newtonsoft.Json;
using System.Globalization;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Helpers;

namespace TravelAgency.DataProcessor
{
	public class Serializer
	{
		public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
		{
			var guides = context.Guides
				.Where(g => g.Language == Language.Spanish)
				.OrderByDescending(g => g.TourPackagesGuides.Count)
				.ThenBy(g => g.FullName)
				.Select(g => new ExportGuideDto
				{
					FullName = g.FullName,
					TourPackages = g.TourPackagesGuides
					.Select(tpg => new ExportTourPackageDto
					{
						PackageName = tpg.TourPackage.PackageName,
						Description = tpg.TourPackage.Description,
						Price = tpg.TourPackage.Price,
					})
					.OrderByDescending(tp => tp.Price)
					.ThenBy(tp => tp.PackageName)
					.ToArray()
				})
				.ToList();

			return XmlSerializationHelper.Serialize(guides, "Guides");
		}

		public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
		{
			var customers = context.Customers
				.Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
				.Select(c => new
				{
					FullName = c.FullName,
					PhoneNumber = c.PhoneNumber,
					Bookings = c.Bookings
						.Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
						.OrderBy(b => b.BookingDate)
						.Select(b => new
						{
							TourPackageName = b.TourPackage.PackageName,
							Date = b.BookingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

						})
						.ToArray()

				})
				.ToList()
				.OrderByDescending(c => c.Bookings.Count())
				.ThenBy(c => c.FullName);

			return JsonConvert.SerializeObject(customers, Formatting.Indented);
		}
	}
}
