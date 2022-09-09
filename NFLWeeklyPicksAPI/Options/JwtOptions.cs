namespace NFLWeeklyPicksAPI.Options
{
    public class JwtOptions
    {
        public string Section { get; set; } = "JwtSettings";
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Expires { get; set; }
        public string SecretKey { get; set; }
    }
}