using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class ScoreModel
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public double value { get; set; }
        public string displayValue { get; set; }
        public Source source { get; set; }
    }
  
    public class Source
    {
        public string id { get; set; }
        public string description { get; set; }
    }


}
