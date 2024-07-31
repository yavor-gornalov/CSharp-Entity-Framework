namespace SoftJail.DataProcessor
{
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using SoftJail.Data.Models;
	using SoftJail.DataProcessor.ExportDto;
	using SoftJail.Helpers;
	using System.Globalization;

	public class Serializer
	{
		public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
		{
			var prisonersInfo = context.Prisoners
				.Where(p => ids.Contains(p.Id))
				.Select(p => new
				{
					Id = p.Id,
					Name = p.FullName,
					CellNumber = p.Cell.CellNumber,
					Officers = p.PrisonerOfficers
						.Select(po => new
						{
							OfficerName = po.Officer.FullName,
							OfficerSalary = po.Officer.Salary,
							Department = po.Officer.Department.Name,

						})
						.ToArray()
				})
				.ToArray();

			var prisoners = prisonersInfo
				.Select(p => new
				{
					p.Id,
					p.Name,
					p.CellNumber,
					Officers = p.Officers
						.Select(o => new
						{
							o.OfficerName,
							o.Department
						})
						.OrderBy(o => o.OfficerName)
						.ToArray(),
					TotalOfficerSalary = Math.Round(p.Officers.Sum(o => o.OfficerSalary), 2)
				})
				.OrderBy(p => p.Name)
				.ThenBy(p => p.Id)
				.ToArray();


			return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
		}

		public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
		{
			var searchedPrisoners = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();

			var prisoners = context.Prisoners
				.Where(p => searchedPrisoners.Contains(p.FullName))
				.Select(p => new ExportPrisonerDto
				{
					Id = p.Id,
					FullName = p.FullName,
					IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
					Messages = p.Mails
						.Select(m => new ExportMessageDto
						{
							Description = new string(m.Description.Reverse().ToArray()),
						})
						.ToArray(),
				})
				.OrderBy(p => p.FullName)
				.ThenBy(p => p.Id)
				.ToArray();

			return XmlSerializationHelper.Serialize(prisoners, "Prisoners");
		}
	}
}