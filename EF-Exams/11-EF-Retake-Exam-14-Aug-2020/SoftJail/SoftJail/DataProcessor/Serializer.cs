namespace SoftJail.DataProcessor
{
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using SoftJail.Data.Models;
	using SoftJail.DataProcessor.ExportDto;
	using SoftJail.Helpers;
	using System.Xml.Linq;

	public class Serializer
	{
		public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
		{
			var prisoners = context.Prisoners
				.Where(p => ids.Contains(p.Id) && p.CellId != null)
				.Select(p => new
				{
					Id = p.Id,
					Name = p.FullName,
					CellNumber = p.Cell.CellNumber,
					Officers = p.PrisonerOfficers
						.Select(po => new
						{
							OfficerName = po.Officer.FullName,
							Department = po.Officer.Department.Name,
						})
						.OrderBy(o => o.OfficerName)
						.ToArray(),
					TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary),2)

				})
				.OrderBy(p => p.Name)
				.ThenBy(p => p.Id)
				.ToList();

			return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
		}

		public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
		{
			var targetNames = prisonersNames
				.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.ToList();

			var prisoners = context.Prisoners
				.Where(p => targetNames.Contains(p.FullName))
				.Select(p => new ExportPrisonerDto
				{
					Id = p.Id,
					Name = p.FullName,
					IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
					EncryptedMessages = p.Mails
						.Select(m => new ExportMessageDto {
							Description = new string(m.Description.Reverse().ToArray()),
						})
						.ToArray(),
				})
				.OrderBy(p => p.Name)
				.ThenBy(p => p.Id)
				.ToList();

			return XmlSerializationHelper.Serialize(prisoners, "Prisoners");
		}
	}
}