using ASP.NETCoreD14.Data.Models;
using ASP.NETCoreD14.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASP.NETCoreD14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        /*------------------------------------------------------------------*/
        public AuthController
            (
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IOptions<JwtSettings> jwtSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                // To Do General Error Handling
                return BadRequest();
            }

            // Map from Dto to Domain Model
            ApplicationUser user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            IdentityResult identityResult = await _userManager.AddToRoleAsync(user, "User");
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult);
            }

            // Success => Login
            return Ok(new { Message = "User registered successfully" });
        }
        /*------------------------------------------------------------------*/
        //[HttpPost]
        //public ActionResult Login(LoginDto loginDto)
        //{
        //    if (loginDto.Email != "Admin" || loginDto.Password != "123")
        //    {
        //        return Unauthorized();
        //    }

        //    #region Claims
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, loginDto.Email),
        //        new Claim(ClaimTypes.NameIdentifier, "Test User"),
        //        new Claim(ClaimTypes.Role, "User"),
        //    };
        //    #endregion

        //    #region Secret Key
        //    var key = "Welcome to ASP.NET Core Welcome to ASP.NET Core";
        //    var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        //    #endregion

        //    #region Signing Credentials
        //    // We Need Signing Credentials
        //    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //    #endregion

        //    #region Generate Token
        //    var tokenString = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        signingCredentials: signingCredentials
        //        );

        //    // Encode the token
        //    var token = new JwtSecurityTokenHandler().WriteToken(tokenString);
        //    #endregion

        //    return Ok(token);
        //}
        /*------------------------------------------------------------------*/
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                // To Do General Error Handling
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Check Password
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!),
                };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = GenerateJwtToken(claims);

            return Ok(token);
        }
        /*------------------------------------------------------------------*/
        private TokenDto GenerateJwtToken(List<Claim> claims)
        {
            var keyFromConfig = _jwtSettings.SecretKey;
            var KeyInBytes = Convert.FromBase64String(keyFromConfig);
            var key = new SymmetricSecurityKey(KeyInBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);


            var tokenString = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: expireDate
                );

            // Encode the token
            var token = new JwtSecurityTokenHandler().WriteToken(tokenString);

            var tokenDto = new TokenDto(token, _jwtSettings.DurationInMinutes);
            return tokenDto;
        }
        /*------------------------------------------------------------------*/
    }
}
