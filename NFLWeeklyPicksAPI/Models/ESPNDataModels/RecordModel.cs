using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class RecordModel
    {
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<RecordItem> items { get; set; }
    }

    public class RecordItem
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string displayValue { get; set; }
        public double value { get; set; }
        public List<Stat> stats { get; set; }
        public string displayName { get; set; }
        public string shortDisplayName { get; set; }
        public string description { get; set; }
    }

    public class Stat
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string shortDisplayName { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
        public string type { get; set; }
        public double value { get; set; }
        public string displayValue { get; set; }
    }


}
