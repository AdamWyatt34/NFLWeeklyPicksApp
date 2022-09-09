namespace NFLWeeklyPicksAPI.Models.ESPNDataModels
{
    public class Image
    {
        public string href { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string alt { get; set; }
        public List<string> rel { get; set; }
    }
}
