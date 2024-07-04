namespace Theatre.Common;

public static class GlobalConstants
{
	// Theatre
	public const int TheatreNameMaxLength = 30;
	public const int TheatreNameMinLength = 4;
	public const int TheatreDirectorMaxLength = 30;
	public const int TheatreDirectorMinLength = 4;
	public const int NumberOfHallsHighLimit = 10;
	public const int NumberOfHallsLowLimit = 1;

	// Play
	public const int PlayTitleMaxLength = 50;
	public const int PlayTitleMinLength = 4;
	public const float PlayRatingHighLimit = 10;
	public const float PlayRatingLowLimit = 0;
	public const int PlayDescriptionMaxLength = 700;
	public const int PlayDescriptionMinLength = 1;
	public const int PlayScreenwriterMaxLength = 30;
	public const int PlayScreenwriterMinLength = 4;

	// Cast
	public const int CastNameMaxLength = 30;
	public const int CastNameMinLength = 4;
	public const int PhoneNumberMaxLength = 15;
	public const string PhoneNumberRegex = @"^\+44-\d{2}-\d{3}-\d{4}$";

	// Ticket
	public const double TicketPriceHighLimit = 100;
	public const double TicketPriceLowLimit = 1;
	public const sbyte TicketRowNumberHighLimit = 10;
	public const sbyte TicketRowNumberLowLimit = 1;
}
