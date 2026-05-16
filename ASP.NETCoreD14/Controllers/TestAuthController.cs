using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreD14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        [HttpGet]
        [Route("TestV01")]
        [Authorize]
        public ActionResult TestV01()
        {
            return Ok("You are authenticated");
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        [Route("TestV02")]
        [Authorize(Policy = "UserOnly")]
        public ActionResult TestV02()
        {
            return Ok("You are authenticated");
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        [Route("TestV03")]
        [Authorize(Policy = "AdminOnly")]
        public ActionResult TestV03()
        {
            return Ok("You are authenticated");
        }
        /*------------------------------------------------------------------*/
    }
}
