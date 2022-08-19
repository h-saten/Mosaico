using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;

namespace Mosaico.Tools.HangfireDashboard.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/healthz")]
    public class HealthzController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new SuccessResult(true);
        }
    }
}