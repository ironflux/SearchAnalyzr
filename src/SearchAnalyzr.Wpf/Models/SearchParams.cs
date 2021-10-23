using System.ComponentModel.DataAnnotations;

namespace SearchAnalyzr.Wpf.Models
{
    public class SearchParams
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
    }
}
