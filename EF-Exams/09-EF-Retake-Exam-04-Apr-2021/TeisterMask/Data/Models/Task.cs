using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeisterMask.Data.Models.Enums;
using static TeisterMask.Common.GlobalConstants;

namespace TeisterMask.Data.Models;

public class Task
{
	public Task()
	{
		EmployeesTasks = new HashSet<EmployeeTask>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(TaskNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	public DateTime OpenDate { get; set; }

	[Required]
	public DateTime DueDate { get; set; }

	[Required]
	public ExecutionType ExecutionType { get; set; }

	[Required]
	public LabelType LabelType { get; set; }

	[Required]
	[ForeignKey(nameof(Project))]
	public int ProjectId { get; set; }
	public Project Project { get; set; } = null!;

	public ICollection<EmployeeTask> EmployeesTasks { get; set; }
}
