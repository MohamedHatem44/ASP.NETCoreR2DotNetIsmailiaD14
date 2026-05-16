using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP.NETCoreD14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestOptionsController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly JwtSettings _jwtSettings;
        /*------------------------------------------------------------------*/
        public TestOptionsController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public ActionResult Get()
        {
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;
            return Ok(_jwtSettings);
        }
        /*------------------------------------------------------------------*/
    }
}
