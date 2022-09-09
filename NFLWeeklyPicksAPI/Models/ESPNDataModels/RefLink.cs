using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class RefLink
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }
}
