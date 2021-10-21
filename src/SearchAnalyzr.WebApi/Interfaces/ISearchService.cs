using SearchAnalyzr.WebApi.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SearchAnalyzr.WebApi.Interfaces
{
    public interface ISearchService
    {
        public Task<string> QueryAsync(string keywords, CancellationToken cancellationToken);
    }
}
