namespace SoftJail.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using Castle.Core.Internal;
	using Data;
	using Newtonsoft.Json;
	using SoftJail.Data.Models;
	using SoftJail.Data.Models.Enums;
	using SoftJail.DataProcessor.ImportDto;
	using SoftJail.Helpers;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid Data";

		private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

		private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

		private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

		public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var departmentsInfo = JsonConvert.DeserializeObject<ImportDepartmetDto[]>(jsonString);

			var departments = new List<Department>();

			foreach (var d in departmentsInfo)
			{
				var isValidDepartment = true;
				if (!IsValid(d) || d.Cells.IsNullOrEmpty())
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var department = new Department
				{
					Name = d.Name,
				};

				var departmentCells = new List<Cell>();

				foreach (var c in d.Cells)
				{
					var isValidHasWindow = bool.TryParse(c.HasWindow, out bool hasWindow);

					if (!IsValid(c) || !isValidHasWindow)
					{
						isValidDepartment = false;
						break;
					}

					var cell = new Cell
					{
						CellNumber = c.CellNumber,
						HasWindow = hasWindow,
						Department = department,
					};

					departmentCells.Add(cell);
				};

				if (isValidDepartment)
				{
					department.Cells = departmentCells;
					departments.Add(department);
					sb.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
				}
				else
				{
					sb.AppendLine(ErrorMessage);
				}

			}

			context.Departments.AddRange(departments);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var prisonersInfo = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

			var prisoners = new List<Prisoner>();

			foreach (var p in prisonersInfo)
			{
				var isPrisonerValid = true;

				var isIncarcerationDateValid = DateTime.TryParseExact(
					p.IncarcerationDate,
					"dd/MM/yyyy",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime incarcerationDate);

				var isReleaseDateValid = DateTime.TryParseExact(
					p.ReleaseDate,
					"dd/MM/yyyy",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime releaseDate);



				if (!IsValid(p) ||
					!isIncarcerationDateValid)
				{
					sb.AppendLine(ErrorMessage);
					isPrisonerValid = false;
					continue;
				}

				var prisoner = new Prisoner
				{
					FullName = p.FullName,
					Nickname = p.Nickname,
					Age = p.Age,
					IncarcerationDate = incarcerationDate,
					Bail = p.Bail,
					CellId = p.CellId,
				};


				if (p.ReleaseDate != null)
				{
					if (isReleaseDateValid)
					{
						prisoner.ReleaseDate = releaseDate;
					}
					else
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}
				}

				var prisonerMails = new List<Mail>();

				foreach (var m in p.Mails)
				{
					if (!IsValid(m))
					{
						sb.AppendLine(ErrorMessage);
						isPrisonerValid |= false;
						break;
					}

					var mail = new Mail
					{
						Sender = m.Sender,
						Description = m.Description,
						Address = m.Address,
					};

					prisonerMails.Add(mail);
				}

				if (isPrisonerValid)
				{
					prisoners.Add(prisoner);
					prisoner.Mails = prisonerMails;
					sb.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));
				}
			}

			context.Prisoners.AddRange(prisoners);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var officersInfo = XmlSerializationHelper.Deserialize<ImportOfficerDto[]>(xmlString, "Officers");

			var officers = new List<Officer>();

			foreach (var o in officersInfo)
			{
				var isPositionValid = Enum.TryParse(typeof(Position), o.Position, out var position);
				var isWeaponValid = Enum.TryParse(typeof(Weapon), o.Weapon, out var weapon);

				if (!IsValid(0) ||
					!isPositionValid ||
					!isWeaponValid)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var officer = new Officer
				{
					FullName = o.FullName,
					Salary = o.Salary,
					Position = (Position)position,
					Weapon = (Weapon)weapon,
					DepartmentId = o.DepartmentId,
				};

				foreach (var prisonerId in o.Prisoners.Select(p => p.Id).Distinct())
				{
					var officerPrisoner = new OfficerPrisoner
					{
						Officer = officer,
						PrisonerId = prisonerId
					};

					officer.OfficerPrisoners.Add(officerPrisoner);
				}	

				officers.Add(officer);
				sb.AppendLine(string.Format(SuccessfullyImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count));
			}

			context.Officers.AddRange(officers);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		private static bool IsValid(object obj)
		{
			var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
			var validationResult = new List<ValidationResult>();

			bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
			return isValid;
		}
	}
}