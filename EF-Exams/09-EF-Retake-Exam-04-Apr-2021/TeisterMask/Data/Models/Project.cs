using System.ComponentModel.DataAnnotations;
using static TeisterMask.Common.GlobalConstants;

namespace TeisterMask.Data.Models;

public class Project
{
	public Project()
	{
		Tasks = new HashSet<Task>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(TaskNameMaxLength)]
	public string Name { get; set; } = null!;

	[Required]
	public DateTime OpenDate { get; set; }

	public DateTime? DueDate { get; set; }

	public ICollection<Task> Tasks { get; set; }
}
