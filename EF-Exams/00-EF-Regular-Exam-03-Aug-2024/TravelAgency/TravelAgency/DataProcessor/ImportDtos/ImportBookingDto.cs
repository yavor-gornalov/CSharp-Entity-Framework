using System.ComponentModel.DataAnnotations;
using static TravelAgency.Data.Validations.GlobalConstants;

namespace TravelAgency.DataProcessor.ImportDtos;

public class ImportBookingDto
{
	[Required]
	public string BookingDate { get; set; } = null!;

	[Required]
	[StringLength(CustomerFullNameMaxLength, MinimumLength = CustomerFullNameMinLength)]
	public string CustomerName { get; set; } = null!;

	[Required]
	[StringLength(TourPackageNameMaxLength, MinimumLength = TourPackageNameMinLength)]
	public string TourPackageName { get; set; } = null!;
}

// BookingDate
// CustomerName
// TourPackageName
