using ASP.NETCoreD14.Data.Models;
using ASP.NETCoreD14.DTOs.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreD14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly RoleManager<ApplicationRole> _roleManager;
        /*------------------------------------------------------------------*/
        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody] RoleCreateDto roleCreateDto)
        {
            ApplicationRole applicationRole = new ApplicationRole
            {
                Name = roleCreateDto.RoleName
            };
            var result = await _roleManager.CreateAsync(applicationRole);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
    }
}
