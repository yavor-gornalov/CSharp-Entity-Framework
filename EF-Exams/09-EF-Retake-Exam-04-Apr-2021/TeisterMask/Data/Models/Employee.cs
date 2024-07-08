namespace TeisterMask.Data.Models;

using System.ComponentModel.DataAnnotations;
using static TeisterMask.Common.GlobalConstants;

public class Employee
{
	public Employee()
	{
		EmployeesTasks = new HashSet<EmployeeTask>();
	}

	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(UsernameMaxLength)]
	public string Username { get; set; } = null!;

	[Required]
	[MaxLength(EmailMaxLength)]
	public string Email { get; set; } = null!;

	[Required]
	[MaxLength(PhoneNumberLength)]
	public string Phone { get; set; } = null!;

	public ICollection<EmployeeTask> EmployeesTasks { get; set; }
}
