using Newtonsoft.Json;

namespace Artillery.DataProcessor.ImportDto;

public class ImportCoutryIdDto
{
	[JsonProperty("Id")]
	public int Id { get; set; }
}