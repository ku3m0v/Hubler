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
        
        
        Supermarket supermarket = _supermarketDAL.GetSupermarketByTitle(model.SupermarketTitle);
        
        var newEmployee = new Employee
        {
            Email = model.Email,
            PassHash = model.Password,
            FirstName = model.FirstName,
            LastName = model.LastName,
            SupermarketId = supermarket.Id,
            RoleId = 1
        };
        
        var validationMessage = _employeeDAL.ValidateRegistration(newEmployee);
        
        if (validationMessage != "Good")
        {
            return BadRequest(new { message = validationMessage });
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
        newEmployee.PassHash = hashedPassword;
        
        var result = _employeeDAL.Insert(newEmployee);
        
        if(result != "Employee was successfully created")
        {
            return BadRequest(result);
        }

        return Ok(new { message = "Registration successful." });
    }
    
    
    [HttpGet("titles")]
    public IActionResult GetSupermarketTitles()
    {
        try
        {
            var result = _supermarketDAL.GetAllTitles();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during getting supermarket titles.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpPost("impersonate")]
    [Authorize(Roles = "Admin")]
    public IActionResult Impersonate([FromBody] ImpersonateRequest impersonateRequest)
    {
        if (impersonateRequest == null || string.IsNullOrWhiteSpace(impersonateRequest.Email))
        {
            return BadRequest("Invalid impersonation request.");
        }

        try
        {
            Employee employeeToImpersonate = _employeeDAL.GetByEmail(impersonateRequest.Email);
            LkRole role = _lkRoleDAL.GetById(employeeToImpersonate.RoleId);

            if (employeeToImpersonate != null)
            {
                var tokenClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, employeeToImpersonate.Id.ToString()),
                    new Claim(ClaimTypes.Email, employeeToImpersonate.Email),
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
                
                _logger.LogInformation($"User {User.Identity.Name} impersonated {employeeToImpersonate.Email}");

                return Ok(new { Token = tokenString });
            }

            return NotFound("User to impersonate not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during impersonation.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
}