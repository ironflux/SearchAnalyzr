using System.ComponentModel.DataAnnotations;

namespace SearchAnalyzr.WebApi.Models
{
    public class SearchParams
    {
        [Required]
        public string Keywords { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
