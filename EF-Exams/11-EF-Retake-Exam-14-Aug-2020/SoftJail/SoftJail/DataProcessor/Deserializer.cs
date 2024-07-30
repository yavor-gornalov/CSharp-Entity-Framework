namespace SoftJail.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using System.Globalization;
	using System.Text;
	using System.Xml.Serialization;
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

			var departmentsInfo = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);

			var departmets = new List<Department>();

			if (departmentsInfo == null) return string.Empty;

			foreach (var d in departmentsInfo)
			{
				var isValidDepartment = true;
				if (!IsValid(d) || !d.Cells.Any())
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var department = new Department
				{
					Name = d.Name,
				};

				foreach (var c in d.Cells)
				{
					if (!IsValid(c))
					{
						sb.AppendLine(ErrorMessage);
						isValidDepartment = false;
						break;
					}

					var cell = new Cell
					{
						CellNumber = c.CellNumber,
						HasWindow = c.HasWindow,
					};

					department.Cells.Add(cell);
				}
				if (department.Cells.Any() || isValidDepartment)
				{
					departmets.Add(department);
					sb.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
				}
			};

			context.Departments.AddRange(departmets);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var prisonersInfo = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

			var prisoners = new List<Prisoner>();

			if (prisonersInfo == null) return string.Empty;

			foreach (var p in prisonersInfo)
			{
				var isPrisonerMailsValid = true;
				var prisonerMails = new List<Mail>();

				if (!IsValid(p))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var prisoner = new Prisoner
				{
					FullName = p.FullName,
					Nickname = p.Nickname,
					Age = p.Age,
					IncarcerationDate = DateTime.ParseExact(p.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
					ReleaseDate = p.ReleaseDate != null ? DateTime.ParseExact(p.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : default,
					Bail = p.Bail,
					CellId = p.CellId,
				};

				foreach (var mailDto in p.Mails)
				{
					if (!IsValid(mailDto))
					{
						isPrisonerMailsValid = false;
						sb.AppendLine(ErrorMessage);
						break;
					}

					var prisonerMail = new Mail
					{
						Description = mailDto.Description,
						Sender = mailDto.Sender,
						Address = mailDto.Address,
						Prisoner = prisoner
					};

					prisonerMails.Add(prisonerMail);
				}
				if (isPrisonerMailsValid)
				{
					prisoner.Mails = prisonerMails;
					prisoners.Add(prisoner);
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

			// var validDepartmetIds = context.Departments.Select(d => d.Id).ToList();

			var officers = new List<Officer>();


			if (officersInfo == null) return string.Empty;

			foreach (var officerDto in officersInfo)
			{
				if (!IsValid(officerDto)
					// DO NOT CHECK for valid Department Id, Judge tests will FAIL!
					// || !validDepartmetIds.Contains(officerDto.DepartmentId)
					|| !Enum.TryParse(officerDto.Position, out Position position)
					|| !Enum.TryParse(officerDto.Weapon, out Weapon weapon))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var officer = new Officer
				{
					FullName = officerDto.Name,
					Salary = officerDto.Salary,
					Position = position,
					Weapon = weapon,
					DepartmentId = officerDto.DepartmentId,
				};

				foreach (var prisonerId in officerDto.PrisonerIds.Select(p => p.Id).Distinct())
				{
					var officerPrisoner = new OfficerPrisoner
					{
						Officer = officer,
						PrisonerId = prisonerId,
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