using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class CompetitionModel
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string uid { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public RefLink season { get; set; }
        public RefLink seasonType { get; set; }
        public RefLink week { get; set; }
        public bool timeValid { get; set; }
        public List<Competition> competitions { get; set; }
        public List<Link> links { get; set; }
        public List<Venue> venues { get; set; }
        public RefLink league { get; set; }
    }

    public class BoxscoreSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Competition
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string guid { get; set; }
        public string uid { get; set; }
        public string date { get; set; }
        public int attendance { get; set; }
        public Type type { get; set; }
        public bool necessary { get; set; }
        public bool timeValid { get; set; }
        public bool neutralSite { get; set; }
        public bool divisionCompetition { get; set; }
        public bool conferenceCompetition { get; set; }
        public bool previewAvailable { get; set; }
        public bool recapAvailable { get; set; }
        public bool boxscoreAvailable { get; set; }
        public bool lineupAvailable { get; set; }
        public bool gamecastAvailable { get; set; }
        public bool playByPlayAvailable { get; set; }
        public bool conversationAvailable { get; set; }
        public bool commentaryAvailable { get; set; }
        public bool pickcenterAvailable { get; set; }
        public bool summaryAvailable { get; set; }
        public bool liveAvailable { get; set; }
        public bool ticketsAvailable { get; set; }
        public bool shotChartAvailable { get; set; }
        public bool timeoutsAvailable { get; set; }
        public bool possessionArrowAvailable { get; set; }
        public bool onWatchESPN { get; set; }
        public bool recent { get; set; }
        public bool bracketAvailable { get; set; }
        public GameSource gameSource { get; set; }
        public BoxscoreSource boxscoreSource { get; set; }
        public PlayByPlaySource playByPlaySource { get; set; }
        public LinescoreSource linescoreSource { get; set; }
        public StatsSource statsSource { get; set; }
        public Venue venue { get; set; }
        public List<Competitor> competitors { get; set; }
        public List<object> notes { get; set; }
        public RefLink situation { get; set; }
        public RefLink status { get; set; }
        public RefLink odds { get; set; }
        public RefLink broadcasts { get; set; }
        public RefLink tickets { get; set; }
        public List<Link> links { get; set; }
        public RefLink predictor { get; set; }
        public RefLink powerIndexes { get; set; }
        public Format format { get; set; }
        public RefLink drives { get; set; }
    }

    public class Competitor
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string uid { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public string homeAway { get; set; }
        public RefLink team { get; set; }
        public RefLink score { get; set; }
        public RefLink record { get; set; }
        public RefLink previousCompetition { get; set; }
        public RefLink nextCompetition { get; set; }
    }

    public class Format
    {
        public Regulation regulation { get; set; }
        public Overtime overtime { get; set; }
    }

    public class GameSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class LinescoreSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Overtime
    {
        public int periods { get; set; }
        public string displayName { get; set; }
        public string slug { get; set; }
        public double clock { get; set; }
    }

    public class PlayByPlaySource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Regulation
    {
        public int periods { get; set; }
        public string displayName { get; set; }
        public string slug { get; set; }
        public double clock { get; set; }
    }

    public class StatsSource
    {
        public string id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string text { get; set; }
        public string abbreviation { get; set; }
        public string slug { get; set; }
        public string type { get; set; }
    }

    public class Venue
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string fullName { get; set; }
        public Address address { get; set; }
        public int capacity { get; set; }
        public bool grass { get; set; }
        public bool indoor { get; set; }
        public List<Image> images { get; set; }
    }
}
