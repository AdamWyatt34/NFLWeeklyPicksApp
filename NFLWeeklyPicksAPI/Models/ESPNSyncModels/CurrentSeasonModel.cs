using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNSyncModels;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Athletes
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Calendar
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Coaches
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Corrections
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Draft
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Events
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Franchises
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class FreeAgents
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Futures
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Group
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Groups
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Item
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public string id { get; set; }
    public int type { get; set; }
    public string name { get; set; }
    public string abbreviation { get; set; }
    public int year { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public bool hasGroups { get; set; }
    public bool hasStandings { get; set; }
    public bool hasLegs { get; set; }
    public Groups groups { get; set; }
    public Weeks weeks { get; set; }
    public Corrections corrections { get; set; }
    public string slug { get; set; }
    public Week week { get; set; }
    public Leaders leaders { get; set; }
}

public class Leaders
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Link
{
    public string language { get; set; }
    public List<string> rel { get; set; }
    public string href { get; set; }
    public string text { get; set; }
    public string shortText { get; set; }
    public bool isExternal { get; set; }
    public bool isPremium { get; set; }
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

public class Notes
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Qbr
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Rankings
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class CurrentSeasonModel
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public string id { get; set; }
    public string guid { get; set; }
    public string uid { get; set; }
    public string name { get; set; }
    public string abbreviation { get; set; }
    public string shortName { get; set; }
    public string slug { get; set; }
    public bool isTournament { get; set; }
    public Season season { get; set; }
    public Seasons seasons { get; set; }
    public Franchises franchises { get; set; }
    public Teams teams { get; set; }
    public Group group { get; set; }
    public Groups groups { get; set; }
    public Events events { get; set; }
    public Notes notes { get; set; }
    public Rankings rankings { get; set; }
    public Draft draft { get; set; }
    public List<Link> links { get; set; }
    public List<Logo> logos { get; set; }
    public Athletes athletes { get; set; }
    public FreeAgents freeAgents { get; set; }
    public Calendar calendar { get; set; }
    public Transactions transactions { get; set; }
    public Talentpicks talentPicks { get; set; }
    public Leaders leaders { get; set; }
    public string gender { get; set; }
}

public class Season
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public int year { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string displayName { get; set; }
    public Type type { get; set; }
    public Types types { get; set; }
    public Rankings rankings { get; set; }
    public Coaches coaches { get; set; }
    public Athletes athletes { get; set; }
    public Futures futures { get; set; }
    public Leaders leaders { get; set; }
}

public class Seasons
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Talentpicks
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Teams
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Transactions
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}

public class Type
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public string id { get; set; }
    public int type { get; set; }
    public string name { get; set; }
    public string abbreviation { get; set; }
    public int year { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public bool hasGroups { get; set; }
    public bool hasStandings { get; set; }
    public bool hasLegs { get; set; }
    public Groups groups { get; set; }
    public Week week { get; set; }
    public Weeks weeks { get; set; }
    public Corrections corrections { get; set; }
    public Leaders leaders { get; set; }
    public string slug { get; set; }
}

public class Types
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public int count { get; set; }
    public int pageIndex { get; set; }
    public int pageSize { get; set; }
    public int pageCount { get; set; }
    public List<Item> items { get; set; }
}

public class Week
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
    public int number { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string text { get; set; }
    public Rankings rankings { get; set; }
    public Events events { get; set; }
    public Talentpicks talentpicks { get; set; }
    public Qbr qbr { get; set; }
}

public class Weeks
{
    [JsonPropertyName("$ref")] public string Ref { get; set; }
}