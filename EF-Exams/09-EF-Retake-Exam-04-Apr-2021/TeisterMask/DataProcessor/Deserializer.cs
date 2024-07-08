// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
	using System.ComponentModel.DataAnnotations;
	using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

	using Data;
	using System.Text;
	using TeisterMask.Helpers;
	using TeisterMask.DataProcessor.ImportDto;
	using System.Globalization;
	using TeisterMask.Data.Models;
	using TeisterMask.Data.Models.Enums;
	using System.Text.Json;
	using Microsoft.VisualBasic;

	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data!";

		private const string SuccessfullyImportedProject
			= "Successfully imported project - {0} with {1} tasks.";

		private const string SuccessfullyImportedEmployee
			= "Successfully imported employee - {0} with {1} tasks.";

		public static string ImportProjects(TeisterMaskContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var projectsInfo = XmlSerializationHelper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

			var projects = new List<Project>();

			foreach (var p in projectsInfo)
			{
				DateTime projectOpenDate;
				DateTime projectDueDate;
				DateTime taskOpenDate;
				DateTime taskDueDate;

				bool isValidProjectOpenDate = DateTime.TryParseExact(
					p.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);



				if (!IsValid(p) || !isValidProjectOpenDate)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var project = new Project
				{
					Name = p.Name,
					OpenDate = projectOpenDate,
				};

				if (p.DueDate != null)
				{
					bool isValidProjectDueDate = DateTime.TryParseExact(
						p.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectDueDate);
					project.DueDate = projectDueDate;

				}
				else
				{
					projectDueDate = DateTime.MaxValue;
				}

				foreach (var t in p.Tasks)
				{
					bool isValidTaskOpenDate = DateTime.TryParseExact(
						t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);
					bool isValidTaskDueDate = DateTime.TryParseExact(
						t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

					if (!IsValid(t)
						|| !isValidTaskOpenDate
						|| !isValidTaskDueDate
						|| taskOpenDate < projectOpenDate
						|| taskDueDate > projectDueDate)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var task = new Task
					{
						Name = t.Name,
						OpenDate = taskOpenDate,
						DueDate = taskDueDate,
						ExecutionType = (ExecutionType)t.ExecutionType,
						LabelType = (LabelType)t.LabelType,
					};

					project.Tasks.Add(task);
				}
				projects.Add(project);
				sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
			}

			context.Projects.AddRange(projects);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportEmployees(TeisterMaskContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var employeesInfo = JsonSerializer.Deserialize<ImportEmployeeDto[]>(jsonString);

			var employees = new List<Employee>();

			var validTaskIds = context.Tasks.Select(t => t.Id).ToList();

			foreach (var e in employeesInfo)
			{
				if (!IsValid(e))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var employee = new Employee
				{
					Username = e.Username,
					Email = e.Email,
					Phone = e.Phone,
				};

				foreach (var taskId in e.TaskIds.Distinct())
				{
					if (!validTaskIds.Contains(taskId))
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var employeeTask = new EmployeeTask
					{
						Employee = employee,
						TaskId = taskId
					};

					employee.EmployeesTasks.Add(employeeTask);
				}

				employees.Add(employee);
				sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
			}

			context.Employees.AddRange(employees);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}