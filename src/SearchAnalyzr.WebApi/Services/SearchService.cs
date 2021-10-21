using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SearchAnalyzr.WebApi.Services
{
    public class SearchService : ISearchService
    {
        private HttpClient _client;
        private const int numberOfResults = 100;
        public SearchService(HttpClient client)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 20);
            _client.DefaultRequestHeaders.Clear();
        }
        public async Task<string> QueryAsync(string keywords, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"?num={numberOfResults}&q={keywords}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using var response = await _client.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "";
        }
    }
}
