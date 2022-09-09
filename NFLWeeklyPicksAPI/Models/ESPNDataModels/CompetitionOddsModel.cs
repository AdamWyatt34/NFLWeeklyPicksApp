using System.Text.Json.Serialization;

namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class CompetitionOddsModel
    {
        public int count { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public List<Item> items { get; set; }
    }

    public class AwayTeamOdds
    {
        public bool favorite { get; set; }
        public bool underdog { get; set; }
        public int moneyLine { get; set; }
        public double spreadOdds { get; set; }
        public RefLink team { get; set; }
        public double? winPercentage { get; set; }
    }

    public class BettingOdds
    {
        public RefLink homeTeam { get; set; }
        public RefLink awayTeam { get; set; }
        public TeamOdds teamOdds { get; set; }
    }


    public class HomeTeamOdds
    {
        public bool favorite { get; set; }
        public bool underdog { get; set; }
        public int moneyLine { get; set; }
        public double spreadOdds { get; set; }
        public RefLink team { get; set; }
        public double? winPercentage { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public Provider provider { get; set; }
        public string details { get; set; }
        public double overUnder { get; set; }
        public double spread { get; set; }
        public double overOdds { get; set; }
        public double underOdds { get; set; }
        public AwayTeamOdds awayTeamOdds { get; set; }
        public HomeTeamOdds homeTeamOdds { get; set; }
        public List<Link> links { get; set; }
        public bool moneylineWinner { get; set; }
        public bool spreadWinner { get; set; }
        public BettingOdds bettingOdds { get; set; }
    }

    public class PreMatchMoneyLineAway
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchMoneyLineHome
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchSpreadAway
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchSpreadHandicapAway
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchSpreadHandicapHome
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchSpreadHome
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchTotalHandicap
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchTotalOver
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class PreMatchTotalUnder
    {
        public string oddId { get; set; }
        public string value { get; set; }
        public string betSlipUrl { get; set; }
    }

    public class Provider
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int priority { get; set; }
    }

    public class TeamOdds
    {
        public PreMatchMoneyLineAway preMatchMoneyLineAway { get; set; }
        public PreMatchMoneyLineHome preMatchMoneyLineHome { get; set; }
        public PreMatchSpreadHandicapAway preMatchSpreadHandicapAway { get; set; }
        public PreMatchSpreadHome preMatchSpreadHome { get; set; }
        public PreMatchTotalOver preMatchTotalOver { get; set; }
        public PreMatchSpreadAway preMatchSpreadAway { get; set; }
        public PreMatchTotalUnder preMatchTotalUnder { get; set; }
        public PreMatchTotalHandicap preMatchTotalHandicap { get; set; }
        public PreMatchSpreadHandicapHome preMatchSpreadHandicapHome { get; set; }
    }


}
