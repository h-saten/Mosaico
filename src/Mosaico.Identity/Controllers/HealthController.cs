using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;

namespace Mosaico.Identity.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/healthz")]
    [Route("api/v{version:apiVersion}/healthz")]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Produces(typeof(SuccessResult<bool>))]
        public IActionResult GetHealthAsync()
        {
            return new SuccessResult(true);
        }
    }
}