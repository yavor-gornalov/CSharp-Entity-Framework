using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;
using static Trucks.Common.ValidationConstants;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Truck")]
public class ImportTruckDto
{
	[Required]
	[RegularExpression(RegistrationNumberRegex)]
	public string RegistrationNumber { get; set; } = null!;

	[Required]
	[StringLength(VinNumberLength, MinimumLength = VinNumberLength)]
	public string VinNumber { get; set; } = null!;

	[Required]
	[Range(TankCapacityLowLimit, TankCapacityHighLimit)]
	public int TankCapacity { get; set; }

	[Required]
	[Range(CargoCapacityLowLimit, CargoCapacityHighLimit)]
	public int CargoCapacity { get; set; }

	[Required]
	[EnumDataType(typeof(CategoryType))]
	public int CategoryType { get; set; }

	[Required]
	[EnumDataType(typeof(MakeType))]
	public int MakeType { get; set; }
}