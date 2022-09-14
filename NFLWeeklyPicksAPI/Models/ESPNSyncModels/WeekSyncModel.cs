using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNSyncModels;

public class WeekSyncModel
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public int number { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string text { get; set; }
}