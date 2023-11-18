using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;

namespace Hubler.Controllers;


[Route("api/employee")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeDAL _employeeDAL;
    private readonly ISupermarketDAL _supermarketDAL;
    private readonly ILkRoleDAL _lkRoleDAL;

    public EmployeeController(IEmployeeDAL employeeDAL, ISupermarketDAL supermarketDAL, ILkRoleDAL lkRoleDAL)
    {
        _employeeDAL = employeeDAL;
        _supermarketDAL = supermarketDAL;
        _lkRoleDAL = lkRoleDAL;
    }

    // GET: api/employee/list
    [HttpGet("list")]
    public IActionResult GetAll()
    {
        var employees = _employeeDAL.GetAll();
        var employeeModels = new List<EmployeeModel>();

        foreach (var employee in employees)
        {
            var supermarket = _supermarketDAL.GetById(employee.SupermarketId);
            var role = _lkRoleDAL.GetById(employee.RoleId);

            if (supermarket != null && role != null)
            {
                employeeModels.Add(new EmployeeModel
                {
                    Email = employee.Email,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    CreatedDate = employee.CreatedDate,
                    // Supermarket fields
                    SupermarketName = supermarket.Title,
                    // Role fields
                    Role = role.RoleName
                });
            }
        }
        
        if (!employeeModels.Any())
        {
            return NotFound("No supermarkets found.");
        }
        return Ok(employeeModels);
    }

    // POST: api/employee/insert
    [HttpPost("insert")]
    public IActionResult Insert([FromBody] EmployeeModel employeeModel)
    {
        var supermarket = _supermarketDAL.GetSupermarketByTitle(employeeModel.SupermarketName);
        var role = _lkRoleDAL.GetByRoleName(employeeModel.Role);
        var manager = _employeeDAL.GetByEmail(employeeModel.AdminEmail);

        if (supermarket == null || role == null)
        {
            return BadRequest("Invalid supermarket or role.");
        }

        var employee = new Employee
        {
            Email = employeeModel.Email,
            PassHash = BCrypt.Net.BCrypt.HashPassword(employeeModel.Password),
            FirstName = employeeModel.FirstName,
            LastName = employeeModel.LastName,
            CreatedDate = DateTime.UtcNow,
            SupermarketId = supermarket.Id,
            RoleId = role.Id,
            AdminId = manager.Id
        };

        var result = _employeeDAL.Insert(employee);
        return Ok(result);
    }
    
    // GET: api/employee/details/{email}
    [HttpGet("details/{email}")]
    public IActionResult Details(string email)
    {
        var employee = _employeeDAL.GetByEmail(email);
        var supermarket = _supermarketDAL.GetById(employee.SupermarketId);
        var role = _lkRoleDAL.GetById(employee.RoleId);
        var manager = _employeeDAL.GetById(employee.AdminId);
        if (employee == null)
        {
            return NotFound("Employee not found.");
        }

        var employeeModel = new EmployeeModel
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            CreatedDate = employee.CreatedDate,
            // Supermarket fields
            SupermarketName = supermarket.Title,
            // Role fields
            Role = role.RoleName,
            // Admin fields
            AdminEmail = manager.Email
        };

        return Ok(employeeModel);
    }

    // PUT: api/employee/edit
    [HttpPut("edit")]
    public IActionResult Edit([FromBody] EmployeeModel employeeModel)
    {
        var employee = _employeeDAL.GetByEmail(employeeModel.Email);
        var supermarket = _supermarketDAL.GetSupermarketByTitle(employeeModel.SupermarketName);
        var role = _lkRoleDAL.GetByRoleName(employeeModel.Role);
        var manager = _employeeDAL.GetByEmail(employeeModel.AdminEmail);
        if (employee == null)
        {
            return NotFound("Employee not found.");
        }

        employee.FirstName = employeeModel.FirstName;
        employee.LastName = employeeModel.LastName;
        employee.SupermarketId = supermarket.Id;
        employee.RoleId = role.Id;
        employee.AdminId = manager.Id;
        
        _employeeDAL.Update(employee);
        return Ok("Employee updated successfully.");
    }

    // DELETE: api/employee/delete/{email}
    [HttpDelete("delete/{email}")]
    public IActionResult Delete(string email)
    {
        var employee = _employeeDAL.GetByEmail(email);
        if (employee == null)
        {
            return NotFound("Employee not found.");
        }

        _employeeDAL.Delete(employee.Id);
        return Ok("Employee deleted successfully.");
    }
    
    // GET: api/employee/managers
    [HttpGet("managers")]
    public IActionResult GetManagers()
    {
        var employees = _employeeDAL.GetAll();
        var employeeModels = new List<EmployeeModel>();

        foreach (var employee in employees)
        {
            var supermarket = _supermarketDAL.GetById(employee.SupermarketId);
            var role = _lkRoleDAL.GetById(employee.RoleId);

            if (supermarket != null && role != null && role.RoleName.Equals("manager"))
            {
                employeeModels.Add(new EmployeeModel
                {
                    Email = employee.Email
                });
            }
        }
        
        if (!employeeModels.Any())
        {
            return NotFound("No managers found.");
        }
        return Ok(employeeModels);
    }
}