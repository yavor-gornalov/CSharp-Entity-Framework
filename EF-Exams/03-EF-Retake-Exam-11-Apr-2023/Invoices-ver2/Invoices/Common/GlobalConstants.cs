namespace Invoices.Common;

public class GlobalConstants
{
	// Product
	public const int ProductNameMinLength = 9;
	public const int ProductNameMaxLength = 30;
	public const string ProductPriceLowLimit = "5.00";
	public const string ProductPriceHighLimit = "1000.00";

	// Address
	public const int AddressStreetNameMinLength = 10;
	public const int AddressStreetNameMaxLength = 20;
	public const int AddressCityMinLength = 5;
	public const int AddressCityMaxLength = 15;
	public const int AddressCountryMinLength = 5;
	public const int AddressCountryMaxLength = 15;

	// Invoice
	public const int InvoiceNumberLowLimit = 1_000_000_000;
	public const int InvoiceNumberHighLimit = 1_500_000_000;
	public const double InvoiceAmountLowLimit = 0;
	public const double InvoiceAmountHighLimit = double.MaxValue;


	// Client
	public const int ClientNameMinLength = 10;
	public const int ClientNameMaxLength = 25;
	public const int ClientNumberVatMinLength = 10;
	public const int ClientNumberVatMaxLength = 15;
}
