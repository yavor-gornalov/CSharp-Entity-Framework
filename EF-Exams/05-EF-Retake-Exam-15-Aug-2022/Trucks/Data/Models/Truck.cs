using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;
using static Trucks.Common.ValidationConstants;

namespace Trucks.Data.Models;

public class Truck
{
	public Truck()
	{
		ClientsTrucks = new HashSet<ClientTruck>();
	}

	[Key]
	public int Id { get; set; }

	[MaxLength(RegistrationNumberMaxLength)]
	public string? RegistrationNumber { get; set; }

	[Required]
	[MaxLength(VinNumberLength)]
	public string VinNumber { get; set; } = null!;

	[Required]
	public int TankCapacity { get; set; }

	[Required]
	public int CargoCapacity { get; set; }

	[Required]
	public CategoryType CategoryType { get; set; }

	[Required]
	public MakeType MakeType { get; set; }

	[Required]
	[ForeignKey(nameof(Despatcher))]
	public int DespatcherId { get; set; }
	public Despatcher Despatcher { get; set; } = null!;

	public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
}
