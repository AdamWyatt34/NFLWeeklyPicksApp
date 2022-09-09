namespace NFLWeeklyPicksAPI.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateClientWithUrl(this IHttpClientFactory httpClient, string url)
        {
            var client = httpClient.CreateClient();
            client.BaseAddress = new Uri(url);
            return client;
        }
    }
}
