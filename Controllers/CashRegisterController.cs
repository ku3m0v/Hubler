using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.Controllers;

[Route("api/cashregister")]
[ApiController]
public class CashRegisterController : ControllerBase
{
    private readonly ICashRegisterDAL _cashRegisterDAL;
    private readonly IEmployeeDAL _employeeDAL;

    public CashRegisterController(ICashRegisterDAL cashRegisterDAL, IEmployeeDAL employeeDAL)
    {
        _cashRegisterDAL = cashRegisterDAL;
        _employeeDAL = employeeDAL;
    }

    [HttpGet("list"), Authorize]
    public IActionResult GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        int userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);

        try
        {
            switch (role)
            {
                case "admin":
                {
                    var cashRegisters = _cashRegisterDAL.GetAll();
                    return Ok(cashRegisters);
                }
                case "manager":
                {
                    var manager = _employeeDAL.GetById(userId);
                    var cashRegisters = _cashRegisterDAL.GetAll()
                        .Where(cr => cr.SupermarketId == manager.SupermarketId);
                    return Ok(cashRegisters);
                }
                case "cashier":
                {
                    var cashier = _employeeDAL.GetById(userId);
                    var cashRegisters = _cashRegisterDAL.GetAll()
                        .Where(cr => cr.EmployeeId == cashier.Id);
                    return Ok(cashRegisters);
                }
                default:
                    return BadRequest("You do not have permission to view this resource.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    
    [HttpPut]
    public IActionResult Update(CashRegister cashRegister)
    {
        try
        {
            _cashRegisterDAL.Update(cashRegister);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the cash register.");
        }
    }

    
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try
        {
            _cashRegisterDAL.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the cash register.");
        }
    }

    
    [HttpPost("insert")]
    public IActionResult Insert(CashRegister cashRegister)
    {
        try
        {
            _cashRegisterDAL.Insert(cashRegister);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while inserting the cash register.");
        }
    }

}