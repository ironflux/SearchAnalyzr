using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SearchAnalyzr.WebApi.Services
{
    public class AnalyzrService : IAnalyzrService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ISearchService _search;
        public AnalyzrService(ISearchService search)
        {
            _search = search;
        }
        public async Task<AnalyzrResult> RunAsync(SearchParams data)
        {
            var content = await _search.QueryAsync(data.Keywords, _cancellationTokenSource.Token);
            return Inspect(content, data.Url);
        }
        private static AnalyzrResult Inspect(string content, string url)
        {
            var result = new AnalyzrResult() { Positions = new List<int>() };
            var displayOrder = 0;

            var anchors = Regex.Matches(content, @"<a\s.*?>");

            if(anchors.Count > 0)
            {
                foreach (Match anchor in anchors)
                {
                    var anchorValue = anchor.Groups[0].Value;

                    if (anchorValue.Contains("/url?q="))
                    {
                        displayOrder++;
                        if (anchorValue.Contains(url.Trim().ToLower()))
                        {
                            result.Positions.Add(displayOrder);
                        }
                    }
                }
            }

            return result;
        }
    }
}
