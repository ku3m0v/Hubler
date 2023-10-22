using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hubler.BAL.Interfaces;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.Extensions.Logging;

namespace Hubler.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IEmployeeDAL employeeDAL,
            IConfiguration configuration)
        {
            _logger = logger;
            _employeeDAL = employeeDAL ?? throw new ArgumentNullException(nameof(employeeDAL));
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid user request.");
            }

            Employee employee = _employeeDAL.FindByEmail(model.Email);

            // Assuming the password in your Employee model is hashed.
            // If it's not hashed, you'll want to hash it using a library like BCrypt before comparing.
            if (employee != null && model.Email.Equals(employee.Email) && 
                BCrypt.Net.BCrypt.Verify(model.Password, employee.PassHash))
            {
                var tokenClaims = new[]
                {
                    new Claim("id", employee.Id.ToString()),
                    new Claim("email", employee.Email),
                    // Assuming role or other claims you might want to include.
                };

                var jwtSettings = _configuration.GetSection("JWTSettings");
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: tokenClaims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiryInMinutes"])),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized("Invalid email or password.");
        }

        // If you have other methods for registration, profile retrieval, etc., add them here similar to the example.
    }
}