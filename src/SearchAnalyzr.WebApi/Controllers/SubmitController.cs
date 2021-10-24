using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SearchAnalyzr.WebApi.Controllers
{
    [Route("api/submit")]
    [ApiController]
    public class SubmitController : ControllerBase
    {
        private readonly IAnalyzrService _analyzrService;
        private readonly ILogger<SubmitController> _logger;
        public SubmitController(IAnalyzrService analyzrService, ILogger<SubmitController> logger)
        {
            _analyzrService = analyzrService;
            _logger = logger;
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Submit search query for analysis")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        public async Task<ActionResult<AnalyzrResult>> PostRequest([FromBody] SearchParams data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _analyzrService.RunAsync(data));
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(503, new { code = 503, message = ex.Message });
                }
            }
            else
            {
                _logger.LogWarning("Mising or invalid search parameters {data}", data);
                return BadRequest(ModelState);
            }
        }
    }
}
