using NFLWeeklyPicksAPI.Models.ESPNDataModels;

namespace NFLWeeklyPicksAPI.Models.ESPNSyncModels;

public class TeamSyncModel
{
    public int count { get; set; }
    public int pageIndex { get; set; }
    public int pageSize { get; set; }
    public int pageCount { get; set; }
    public List<RefLink> items { get; set; }
}