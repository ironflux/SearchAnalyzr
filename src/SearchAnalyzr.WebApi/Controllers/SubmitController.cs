using Microsoft.AspNetCore.Mvc;
using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace SearchAnalyzr.WebApi.Controllers
{
    [Route("submit")]
    [ApiController]
    public class SubmitController : ControllerBase
    {
        private readonly IAnalyzrService _analyzrService;
        public SubmitController(IAnalyzrService analyzrService)
        {
            _analyzrService = analyzrService;
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Submit search query for analysis")]
        public async Task<ActionResult<AnalyzrResult>> PostRequest([FromBody] SearchParams data)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _analyzrService.RunAsync(data));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
