using SearchAnalyzr.WebApi.Models;
using System.Threading.Tasks;

namespace SearchAnalyzr.WebApi.Interfaces
{
    public interface IAnalyzrService
    {
        public Task<AnalyzrResult> RunAsync(SearchParams data);
    }
}
