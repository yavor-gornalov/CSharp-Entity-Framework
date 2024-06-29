namespace Medicines.Common;

public static class ValidationConstants
{
	// Pharmacy
	public const int PharmacyNameMinLength = 2;
	public const int PharmacyNameMaxLength = 50;
	public const int PharmacyPhoneNumberLength = 14;
	public const string PharmacyPhoneNumberRegex = @"^\([0-9]{3}\) [0-9]{3}-[0-9]{4}\b";

	// Medicine
	public const int MedicineNameMinLength = 3;
	public const int MedicineNameMaxLength = 150;
	public const double MedicinePriceLowLimit = 0.01;
	public const double MedicinePriceHighLimit = 1000;
	public const int MedicineProducerNameMinLength = 3;
	public const int MedicineProducerNameMaxLength = 100;

	// Patient
	public const int PatientFullNameMinLength = 5;
	public const int PatientFullNameMaxLength = 100;


}
