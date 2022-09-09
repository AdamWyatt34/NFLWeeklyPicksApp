using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class WeeklyGameModel
    {
        [JsonPropertyName("$meta")]
        public Meta Meta { get; set; }
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<RefLink> items { get; set; }
    }

    public class Meta
    {
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public List<string> week { get; set; }
        public List<string> season { get; set; }
        public List<string> seasontypes { get; set; }
    }
}
