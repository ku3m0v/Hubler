using System;
using Microsoft.AspNetCore.Mvc;
using Hubler.BAL.Interfaces;
using Hubler.DAL.Interfaces;
using Hubler.Models;

namespace Hubler.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IEmployeeDAL _employeeDAL;

    public LoginController(IEmployeeDAL employeeDAL)
    {
        _employeeDAL = employeeDAL ?? throw new ArgumentNullException(nameof(employeeDAL));
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Email) ||
            string.IsNullOrWhiteSpace(loginModel.Password))
        {
            return BadRequest(new { message = "Invalid credentials." });
        }

        // Assuming the password stored in the database is hashed and you would need to hash the provided password.
        // For this example, we are using the plain password, but you should definitely use a hash function before calling Authenticate.
        int? employeeId = _employeeDAL.Authenticate(loginModel.Email, loginModel.Password);

        if (!employeeId.HasValue)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        // You might also want to generate a JWT token or any other authentication mechanism here.
        // For now, simply returning a success message and the ID of the authenticated employee.

        return Ok(new { message = "Logged in successfully.", id = employeeId.Value });
    }
}