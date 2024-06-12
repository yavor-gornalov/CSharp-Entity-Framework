namespace CarDealer.DTOs.Import;

public class CarImportDTO
{
	public CarImportDTO()
	{
		PartsId = new HashSet<int>();
	}

	public int Id { get; set; }

	public string Make { get; set; } = null!;

	public string Model { get; set; } = null!;

	public long TraveledDistance { get; set; }

	public HashSet<int> PartsId { get; set; }
}
