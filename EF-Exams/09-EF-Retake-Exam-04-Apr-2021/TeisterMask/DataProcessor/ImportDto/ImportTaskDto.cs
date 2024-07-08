using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;
using static TeisterMask.Common.GlobalConstants;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Task")]
public class ImportTaskDto
{
	[Required]
	[StringLength(TaskNameMaxLength, MinimumLength = TaskNameMinLength)]
	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[Required]
	[XmlElement("OpenDate")]
	public string OpenDate { get; set; } = null!;

	[Required]
	[XmlElement("DueDate")]
	public string DueDate { get; set; } = null!;

	[Required]
	[XmlElement("ExecutionType")]
	[EnumDataType(typeof(ExecutionType))]
	public int ExecutionType { get; set; }

	[Required]
	[XmlElement("LabelType")]
	[EnumDataType(typeof(LabelType))]
	public int LabelType { get; set; }
}