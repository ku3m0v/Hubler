using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hubler.DAL.Implementations;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;

namespace Hubler.Controllers;

[Route("api/cashregister")]
[ApiController]
public class CashRegisterController : ControllerBase
{
    private readonly ICashRegisterDAL _cashRegisterDAL;
    private readonly IEmployeeDAL _employeeDAL;
    private readonly ISupermarketDAL _supermarketDAL;
    private readonly ILkStatusDAL _lkStatusDAL;

    public CashRegisterController(ICashRegisterDAL cashRegisterDAL, IEmployeeDAL employeeDAL,
        ISupermarketDAL supermarketDAL, LkStatusDAL lkStatusDAL)
    {
        _cashRegisterDAL = cashRegisterDAL;
        _employeeDAL = employeeDAL;
        _supermarketDAL = supermarketDAL;
        _lkStatusDAL = lkStatusDAL;
    }

    [HttpGet("list"), Authorize]
    public IActionResult GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        int userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);

        IEnumerable<CashRegister> cashRegisters;

        if (role == "admin")
        {
            cashRegisters = _cashRegisterDAL.GetAll();
        }
        else if (role == "manager")
        {
            var manager = _employeeDAL.GetById(userId);
            cashRegisters = _cashRegisterDAL.GetAll()
                .Where(e => e.SupermarketId == manager.SupermarketId);
        }
        else
        {
            return BadRequest("You do not have permission to view this resource.");
        }

        var cashRegisterModels = new List<CashRegisterModel>();

        foreach (var cashRegister in cashRegisters)
        {
            var supermarket = _supermarketDAL.GetById(cashRegister.SupermarketId);
            var status = _lkStatusDAL.GetById(cashRegister.StatusId);

            if (supermarket != null && status != null)
            {
                cashRegisterModels.Add(new CashRegisterModel
                {
                    Id = cashRegister.Id,
                    SupermarketName = supermarket.Title,
                    RegisterNumber = cashRegister.RegisterNumber,
                    StatusName = status.StatusName,
                    EmployeeId = cashRegister.EmployeeId
                });
            }
        }

        return Ok(cashRegisterModels);
    }
    
    [HttpGet("get")]
    public ActionResult<CashRegisterModel> GetById(int id)
    {
        var cashRegister = _cashRegisterDAL.GetById(id);
        if (cashRegister == null) return NotFound("Cash register not found.");

        var supermarket = _supermarketDAL.GetById(cashRegister.SupermarketId);
        var status = _lkStatusDAL.GetById(cashRegister.StatusId);

        var cashRegisterModel = new CashRegisterModel
        {
            Id = cashRegister.Id,
            SupermarketName = supermarket.Title,
            RegisterNumber = cashRegister.RegisterNumber,
            StatusName = status.StatusName,
            EmployeeId = cashRegister.EmployeeId
        };

        return Ok(cashRegisterModel);
    }
    
    [HttpPost("edit")]
    public IActionResult Update(CashRegisterModel model)
    {
        var cashRegister = _cashRegisterDAL.GetById(model.Id);
        var status = _lkStatusDAL.GetByName(model.StatusName);
        
        if (cashRegister == null) return NotFound("Cash register not found.");
        
        cashRegister.RegisterNumber = model.RegisterNumber;
        cashRegister.StatusId = status.Id;
        cashRegister.EmployeeId = model.EmployeeId;
        
        _cashRegisterDAL.Update(cashRegister);
        
        return Ok();
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
    public IActionResult Insert(CashRegisterModel model)
    {
        var supermarket = _supermarketDAL.GetSupermarketByTitle(model.SupermarketName);
        var status = _lkStatusDAL.GetByName(model.StatusName);
        
        
        var cashRegister = new CashRegister
        {
            SupermarketId = supermarket.Id,
            RegisterNumber = model.RegisterNumber,
            StatusId = status.Id,
            EmployeeId = model.EmployeeId
        };
        
        
        _cashRegisterDAL.Insert(cashRegister);
        
        return Ok();
    }
    
    [HttpGet("statuses")]
    public IActionResult GetStatuses()
    {
        var result = _lkStatusDAL.GetAll();
        IEnumerable<string> statuses = result.Select(s => s.StatusName);
        return Ok(statuses);
    }
    
    [HttpGet("employees"), Authorize]
    public ActionResult<IEnumerable<Employee>> GetEmployees()
    {
        int userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);

        var employee = _employeeDAL.GetById(userId);
        var employees = _employeeDAL.GetAll()
            .Where(e => e.SupermarketId == employee.SupermarketId);
        
        return Ok(employees);
    }

    [HttpGet("employee")]
    public ActionResult<Employee> GetEmployees(int id)
    {
        var employee = _employeeDAL.GetById(id);
        return Ok(employee);
    }
}