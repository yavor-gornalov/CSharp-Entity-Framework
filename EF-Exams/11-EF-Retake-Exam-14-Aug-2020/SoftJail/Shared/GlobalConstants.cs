﻿namespace SoftJail.Shared;

public class GlobalConstants
{
	// Prisoner
	public const int PrisonerFullNameMinLength = 3;
	public const int PrisonerFullNameMaxLength = 20;
	public const string PrisonerNicknameRegex = @"^The [A-Z][a-z]{1,}$";
	public const int PrisonerAgeLowLimit = 18;
	public const int PrisonerAgeHighLimit = 65;
	public const double PrisonerBailLowLimit = 0;
	public const double PrisonerBailHighLimit = double.MaxValue;

	// Officer
	public const int OfficerFullNameMinLength = 3;
	public const int OfficerFullNameMaxLength = 30;
	public const double OfficerSalaryLowLimit = 0;
	public const double OfficerSalaryHighLimit = double.MaxValue;

	// Cell
	public const double CellNumberLowLimit = 1;
	public const double CellNumberHighLimit = 1000;

	// Mail
	public const string MailAddressRegex = @"^[A-Za-z0-9 ]+ str.$";

	// Department
	public const int DepartmentNameMinLength = 3;
	public const int DepartmentNameMaxLength = 25;
}
