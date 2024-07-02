namespace Footballers.Common;

public static class ValidationConstants
{
	// Footballer
	public const int FootballerNameMaxLength = 40;
	public const int FootballerNameMinLength = 2;

	// Team
	public const int TeamNameMaxLength = 40;
	public const int TeamNameMinLength = 3;
	public const string TeamNameRegex = @"^[a-zA-Z0-9 .-]{3,40}$";
	public const int TeamNationalityMaxLength = 40;
	public const int TeamNationalityMinLength = 2;
	public const int TeamTrophiesLowLimit = 1;
	public const int TeamTrophiesHighLimit = int.MaxValue;

	// Coach
	public const int CoachNameMaxLength = 40;
	public const int CoachNameMinLength = 2;
}
