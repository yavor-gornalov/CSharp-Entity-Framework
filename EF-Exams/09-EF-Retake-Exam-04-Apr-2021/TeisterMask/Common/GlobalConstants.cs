namespace TeisterMask.Common;

public static class GlobalConstants
{
	// Employee
	public const int UsernameMaxLength = 40;
	public const int UsernameMinLength = 3;
	public const string UsernameRegex = @"^[A-Za-z0-9]+$";
	public const int EmailMaxLength = 255;
	public const int PhoneNumberLength = 12;
	public const string PhoneNumberRegex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";

	// Project
	public const int ProjectNameMaxLength = 40;
	public const int ProjectNameMinLength = 2;

	// Task
	public const int TaskNameMaxLength = 40;
	public const int TaskNameMinLength = 2;



}
