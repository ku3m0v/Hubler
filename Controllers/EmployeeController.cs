using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.Controllers;


[Route("api/employee")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeDAL _employeeDAL;

    public EmployeeController(IEmployeeDAL employeeDAL)
    {
        _employeeDAL = employeeDAL;
    }

    [HttpGet("get")]
    public ActionResult<Employee> GetById()
    {
        var employeeIdClaim = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var employee = _employeeDAL.GetById(employeeIdClaim);
        if (employee == null)
        {
            return NotFound($"No employee found with ID {employeeIdClaim}");
        }

        return Ok(employee);
    }

    [HttpPost("{id}")]
    public IActionResult Update(int id, [FromBody] Employee updatedEmployee)
    {
        var employeeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(employeeIdClaim, out var employeeIdFromToken) || id != employeeIdFromToken)
        {
            return Unauthorized("Access denied - Invalid employee ID");
        }

        if (id != updatedEmployee.Id)
        {
            return BadRequest("Mismatched employee ID");
        }
        
        _employeeDAL.Update(updatedEmployee);
        return Ok("Employee updated successfully");
    }
}