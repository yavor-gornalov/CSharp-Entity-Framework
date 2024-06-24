namespace Boardgames.Common;

public static class ValidationConstants
{
	// Boargame
	public const int BoargameNameMinLength = 10;
	public const int BoargameNameMaxLength = 20;
	public const double BoargameRatingLowLimit = 1;
	public const double BoargameRatingHighLimit = 10;
	public const double BoargameYearLowLimit = 2018;
	public const double BoargameYearHighLimit = 2023;

	// Seller
	public const int SellerNameMinLength = 5;
	public const int SellerNameMaxLength = 20;
	public const int SellerAddressMinLength = 2;
	public const int SellerAddressMaxLength = 30;
	public const string SellerWebsiteRegex = @"www\.[a-zA-Z0-9\-]+\.com";

	// Creator
	public const int CreatorFirstNameMinLength = 2;
	public const int CreatorFirstNameMaxLength = 7;
	public const int CreatorLastNameMinLength = 2;
	public const int CreatorLastNameMaxLength = 7;
}
