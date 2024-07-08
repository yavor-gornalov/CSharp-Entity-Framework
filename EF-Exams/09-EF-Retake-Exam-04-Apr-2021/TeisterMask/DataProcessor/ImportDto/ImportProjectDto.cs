using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models;
using static TeisterMask.Common.GlobalConstants;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Project")]
public class ImportProjectDto
{
	[Required]
	[StringLength(ProjectNameMaxLength, MinimumLength = ProjectNameMinLength)]
	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("OpenDate")]
	public string OpenDate { get; set; } = null!;

	[XmlElement("DueDate")]
	public string? DueDate { get; set; }

	[XmlArray("Tasks")]
	public ImportTaskDto[]? Tasks { get; set; }

}
