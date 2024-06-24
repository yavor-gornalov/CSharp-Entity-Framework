using Boardgames.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

public class ExportToJsonBoardgameDTO
{
	[JsonProperty("Name")]
	public string Name { get; set; } = null!;

	[JsonProperty("Rating")]
	public double Rating { get; set; }

	[JsonProperty("Mechanics")]
	public string Mechanics { get; set; } = null!;

	[JsonProperty("Category")]
	public string CategoryType { get; set; } = null!;
}
