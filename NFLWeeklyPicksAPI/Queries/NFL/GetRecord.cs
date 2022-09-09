using MediatR;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Models.ESPNDataModels;
using System.Net.Http.Json;

namespace NFLWeeklyPicksAPI.Queries.NFL
{
    public class GetRecord : IRequest<string>
    {
        public string RecordURL { get; set; }

        internal class Handler : IRequestHandler<GetRecord, string>
        {
            private readonly IHttpClientFactory _httpClient;

            public Handler(IHttpClientFactory httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<string> Handle(GetRecord request, CancellationToken cancellationToken)
            {
                var client = _httpClient.CreateClientWithUrl(request.RecordURL);

                var record = await client.GetFromJsonAsync<RecordModel>(request.RecordURL, cancellationToken);

                return record.count == 0 ? 
                    "0-0" : 
                    record.items
                        .Where(r => int.Parse(r.id) == 0)
                        .Select(r => r.summary)
                        .FirstOrDefault();
            }
        }
    }
}
