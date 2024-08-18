using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MindbridgeWebAPI.Models;
using MindbridgeWebAPI.Repository;
using MindbridgeWebAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MindbridgeWebAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserService userService, IConfiguration configuration,IUserRepository userRepository)
        {
            _userService = userService;
            _configuration = configuration;
            _userRepository = userRepository;   
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {   
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                bool result = await _userService.Login(model);

                if (result) // Check the result of the login operation
                {
                    message = "login successully";
                    var token = GenerateJwtToken(model.username);
                    return Ok(new { token });
                }
                else
                {
                    message = "Invalid User or Password";
                    return Unauthorized(message);
                }
               
            }

            // Model is invalid, return the login view with validation errors
            return Content(message);
        }
        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            { 
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userId = await _userService.RegisterUser(model);

            // Assuming a route named "GetUserById" exists 
            // to retrieve a user by ID
            return Ok(userId);
        }
        [Authorize]
        [HttpPost("GetAllUser")]
        public async  Task<IActionResult> GetUser()
        {
            var users = _userService.GetUsers().Result;
            return Ok(users);
        }
    }
}
