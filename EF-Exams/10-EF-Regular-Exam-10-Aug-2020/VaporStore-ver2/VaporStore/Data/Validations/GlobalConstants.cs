namespace VaporStore.Data.Validations;

public static class GlobalConstants
{
	// Game
	public const double GamePriceLowLimit = 0.0;
	public const double GamePriceHighLimit = double.MaxValue;

	// User
	public const int UsernameMinLength = 3;
	public const int UsernameMaxLength = 20;
	public const string UserFullnameRegex = @"^[A-Z][a-z]+ [A-Z][a-z]+$";
	public const int UserAgeLowLimit = 3;
	public const int UserAgeHighLimit = 103;

	// Card
	public const int CardNumberMaxLength = 19;
	public const string CardNumberRegex = @"^\d{4} \d{4} \d{4} \d{4}$";
	public const int CardCvcMaxLength = 3;
	public const string CardCvcRegex = @"^\d{3}$";

	// Purchase
	public const int PurchaseProductKeyMaxLength = 14;
	public const string PurchaseProductKeyRegex = @"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$";


}
