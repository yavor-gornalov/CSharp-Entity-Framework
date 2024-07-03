namespace Artillery.Common;

public static class GlobalConstants
{
	// Country
	public const int CountryNameMaxLength = 60;
	public const int CountryNameMinLength = 4;
	public const int CountryArmySizeHighLimit = 10_000_000;
	public const int CountryArmySizeLowLimit = 50_000;

	// Manufacturer
	public const int ManufacturerNameMaxLength = 40;
	public const int ManufacturerNameMinLength = 4;
	public const int ManufacturerFoundedMaxLength = 100;
	public const int ManufacturerFoundedMinLength = 10;

	// Shell
	public const double ShellWeightHighLimit = 1_680;
	public const double ShellWeightLowLimit = 2;
	public const int ShellCaliberMaxLength = 30;
	public const int ShellCaliberMinLength = 4;

	// Gun
	public const int GunWeightHighLimit = 1_350_000;
	public const int GunWeightLowLimit = 100;
	public const double GunBarrelLengthHighLimit = 35;
	public const double GunBarrelLengthLowLimit = 2;
	public const double GunRangeHighLimit = 100_000;
	public const double GunRangeLowLimit = 1;
}
