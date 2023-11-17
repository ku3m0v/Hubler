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

    [HttpGet("{id}")]
    public ActionResult<Employee> GetById(int id)
    {
        // Retrieve the employee's ID from the claims
        var employeeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(employeeIdClaim, out var employeeIdFromToken) || id != employeeIdFromToken)
        {
            return Unauthorized("Access denied - Invalid employee ID");
        }

        var employee = _employeeDAL.GetById(id);
        if (employee == null)
        {
            return NotFound($"No employee found with ID {id}");
        }

        return Ok(employee);
    }

    [HttpPost("{id}")]
    public IActionResult Update(int id, [FromBody] Employee updatedEmployee)
    {
        // Retrieve the employee's ID from the claims
        var employeeIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(employeeIdClaim, out var employeeIdFromToken) || id != employeeIdFromToken)
        {
            return Unauthorized("Access denied - Invalid employee ID");
        }

        if (id != updatedEmployee.Id)
        {
            return BadRequest("Mismatched employee ID");
        }

        // The controller action assumes that the employee is updating their own record
        // This needs to be secured appropriately in a real application
        _employeeDAL.Update(updatedEmployee);
        return Ok("Employee updated successfully");
    }
}