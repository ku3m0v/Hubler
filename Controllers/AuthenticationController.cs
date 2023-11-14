using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hubler.DAL.Implementations;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;

namespace Hubler.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly ISupermarketDAL _supermarketDAL;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IEmployeeDAL employeeDAL,
            ISupermarketDAL supermarketDAL,
            IConfiguration configuration)
        {
            _logger = logger;
            _employeeDAL = employeeDAL ?? throw new ArgumentNullException(nameof(employeeDAL));
            _supermarketDAL = supermarketDAL ?? throw new ArgumentNullException(nameof(supermarketDAL));
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid user request.");
            }

            Employee employee = _employeeDAL.GetByEmail(model.Username);
            
            if (employee != null && model.Username.Equals(employee.Email) &&
                BCrypt.Net.BCrypt.Verify(model.Password, employee.PassHash))
            {
                // DBConnection.SetContext(employee.Email);
                var tokenClaims = new[]
                {
                    new Claim("id", employee.Id.ToString()),
                    new Claim("email", employee.Email),
                };

                var jwtSettings = _configuration.GetSection("JWTSettings");
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]));
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

        
        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid registration request.");
            }
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            Supermarket supermarket = _supermarketDAL.GetSupermarketByTitle(model.SupermarketTitle);
            
            var newEmployee = new Employee
            {
                Email = model.Email,
                PassHash = hashedPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDate = DateTime.Now,
                SupermarketId = supermarket.Id,
                RoleId = 2
            };

            try
            {
                // Save the new employee record to the database
                var result = _employeeDAL.Insert(newEmployee);
                if (result == "Email already exists")
                {
                    return BadRequest(result);
                }
                return Ok("Registration successful.");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, "Error during registration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}