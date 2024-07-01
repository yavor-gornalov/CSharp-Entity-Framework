namespace Trucks.Common;

public static class ValidationConstants
{
	// Truck
	public const int RegistrationNumberMaxLength = 8;
	public const string RegistrationNumberRegex = @"^[A-Z]{2}[0-9]{4}[A-Z]{2}\b";
	public const int VinNumberLength = 17;
	public const int TankCapacityLowLimit  = 950;
	public const int TankCapacityHighLimit  = 1420;
	public const int CargoCapacityLowLimit = 5000;
	public const int CargoCapacityHighLimit = 29000;

	// Client
	public const int ClientNameMinLength = 3;
	public const int ClientNameMaxLength = 40;
	public const int NationalityMinLength = 2;
	public const int NationalityMaxLength = 40;

	// Despatcher
	public const int DespatcherNameMinLength = 2;
	public const int DespatcherNameMaxLength = 40;


}
