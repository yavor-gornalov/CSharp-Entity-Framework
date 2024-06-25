namespace Invoices.Common;

public static class ValidationConstants
{
	// Product
	public const int ProductNameMinLength = 9;
	public const int ProductNameMaxLength = 30;
	public const decimal ProductPriceLowLimit = 5m;
	public const decimal ProductPriceHighLimit = 1000m;

	// Address
	public const int StreetNameMinLength = 10;
	public const int StreetNameMaxLength = 20;
	public const int CityNameMinLength = 5;
	public const int CityNameMaxLength = 15;
	public const int CountryNameMinLength = 5;
	public const int CountryNameMaxLength = 15;

	// Invoice
	public const int InvoiceNumberLowLimit = 1_000_000_000;
	public const int InvoiceNumberHighLimit = 1_500_000_000;

	// Client
	public const int ClientNameMinLength = 10;
	public const int ClientNameMaxLength = 25;
	public const int ClientVatMinLength = 10;
	public const int ClientVatMaxLength = 15;
}
