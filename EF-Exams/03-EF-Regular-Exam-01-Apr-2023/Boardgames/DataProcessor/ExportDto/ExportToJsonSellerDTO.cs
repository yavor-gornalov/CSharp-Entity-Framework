using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ExportDto;

public class JsonExportSellerDTO
{
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;
    
    [JsonProperty("Website")]
    public string Website { get; set; } = null!;

    [JsonProperty("Boardgames")]
	public ExportToJsonBoardgameDTO[] Boardgames { get; set; }

}
