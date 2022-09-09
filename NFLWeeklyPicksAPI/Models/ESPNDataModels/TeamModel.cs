using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class TeamModel
    {
        //[JsonPropertyName("$ref")]
        //public string Ref { get; set; }
        //public string id { get; set; }
        //public string guid { get; set; }
        //public string uid { get; set; }
        //public AlternateIds alternateIds { get; set; }
        //public string slug { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string abbreviation { get; set; }
        public string displayName { get; set; }
        public string shortDisplayName { get; set; }
        //public string color { get; set; }
        //public string alternateColor { get; set; }
        //public bool isActive { get; set; }
        //public bool isAllStar { get; set; }
        public List<Logo> logos { get; set; }
        public RefLink record { get; set; }
        public RefLink athletes { get; set; }
        //public Venue venue { get; set; }
        //public RefLink groups { get; set; }
        //public RefLink ranks { get; set; }
        //public List<Link> links { get; set; }
        //public RefLink injuries { get; set; }
        //public RefLink notes { get; set; }
        //public RefLink againstTheSpreadRecords { get; set; }
        //public RefLink franchise { get; set; }
        //public RefLink depthCharts { get; set; }
        //public RefLink events { get; set; }
        //public RefLink coaches { get; set; }
    }

    public class AlternateIds
    {
        public string sdr { get; set; }
    }

    public class Logo
    {
        public string href { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string alt { get; set; }
        public List<string> rel { get; set; }
        public string lastUpdated { get; set; }
    }
}
