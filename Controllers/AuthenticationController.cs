using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hubler.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IEmployeeDAL _employeeDAL;
    private readonly ISupermarketDAL _supermarketDAL;
    private readonly ILkRoleDAL _lkRoleDAL;
    private readonly IConfiguration _configuration;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IEmployeeDAL employeeDAL,
        ISupermarketDAL supermarketDAL,
        ILkRoleDAL lkRoleDAL,
        IConfiguration configuration)
    {
        _logger = logger;
        _employeeDAL = employeeDAL ?? throw new ArgumentNullException(nameof(employeeDAL));
        _supermarketDAL = supermarketDAL ?? throw new ArgumentNullException(nameof(supermarketDAL));
        _lkRoleDAL = lkRoleDAL ?? throw new ArgumentNullException(nameof(lkRoleDAL));
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
        LkRole role = _lkRoleDAL.GetById(employee.RoleId);
        
        
        if (employee != null && model.Username.Equals(employee.Email) &&
            BCrypt.Net.BCrypt.Verify(model.Password, employee.PassHash))
        {
            // DBConnection.SetContext(employee.Email);
            var tokenClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, role.RoleName)
            };

            var jwtSettings = _configuration.GetSection("JWTSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: tokenClaims,
                expires: DateTime.Now.AddMinutes(60),
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
            //FIXME: Add email confirmation to registration @returnT0
            return Ok(new { message = "Registration successful." });
            
            
        }
        catch (Exception ex)
        {
            // Log the exception details for debugging purposes
            _logger.LogError(ex, "Error during registration.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
}