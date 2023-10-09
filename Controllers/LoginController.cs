using System;
using Microsoft.AspNetCore.Mvc;
using Hubler.BAL.Interfaces;
using Hubler.Models;

namespace Hubler.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IEmployeeBAL _employeeBAL;

    public LoginController(IEmployeeBAL employeeBAL)
    {
        _employeeBAL = employeeBAL ?? throw new ArgumentNullException(nameof(employeeBAL));
    }

    [HttpPost("authenticate")]
    public ActionResult Authenticate(LoginModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("Invalid user credentials provided.");
        }

        var userId = _employeeBAL.Authenticate(model.Email, model.Password);
        if (userId.HasValue)
        {
            // Assuming you want to return the employee's Id for a successful login
            return Ok(new { UserId = userId.Value });
        }

        return Unauthorized();
    }
}