namespace TeisterMask.DataProcessor
{
	using Data;
	using Microsoft.EntityFrameworkCore;
	using System.Globalization;
	using System.Text.Json;
	using TeisterMask.DataProcessor.ExportDto;
	using TeisterMask.Helpers;

	public class Serializer
	{
		public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
		{
			var projects = context.Projects
				.Where(p => p.Tasks.Any())
				.Select(p => new ExportProjectDto
				{
					TasksCount = p.Tasks.Count(),
					ProjectName = p.Name,
					HasEndDate = p.DueDate != null ? "Yes" : "No",
					Tasks = p.Tasks
						.Select(t => new ExportTaskDto
						{
							Name = t.Name,
							Label = t.LabelType.ToString()
						})
						.OrderBy(t => t.Name)
						.ToArray()
				})
				.OrderByDescending(p => p.TasksCount)
				.ThenBy(p => p.ProjectName)
				.ToList();

			return XmlSerializationHelper.Serialize(projects, "Projects");
		}

		public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
		{
			var employees = context.Employees
				.Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
				.AsNoTracking()
				.Select(e => new
				{
					Username = e.Username,
					Tasks = e.EmployeesTasks
						.Where(et => et.Task.OpenDate >= date)
						.OrderByDescending(et => et.Task.DueDate)
						.ThenBy(et => et.Task.Name)
						.Select(et => new
						{
							TaskName = et.Task.Name,
							OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
							DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
							LabelType = et.Task.LabelType.ToString(),
							ExecutionType = et.Task.ExecutionType.ToString(),

						})
						.ToList()
				})
				.OrderByDescending(e => e.Tasks.Count)
				.ThenBy(e => e.Username)
				.Take(10)
				.ToList();

			return JsonSerializer.Serialize(employees, new JsonSerializerOptions
			{
				WriteIndented = true
			});
		}
	}
}