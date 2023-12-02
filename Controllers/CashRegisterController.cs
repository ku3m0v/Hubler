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
        ISupermarketDAL supermarketDAL, ILkStatusDAL lkStatusDAL)
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
            var employee = _employeeDAL.GetById(cashRegister.Employee_Id);

            if (supermarket != null && status != null)
            {
                cashRegisterModels.Add(new CashRegisterModel
                {
                    Id = cashRegister.Id,
                    SupermarketName = supermarket.Title,
                    RegisterNumber = cashRegister.RegisterNumber,
                    StatusName = status.StatusName,
                    EmployeeId = cashRegister.Employee_Id,
                    EmployeeName = employee.FirstName + " " + employee.LastName
                });
            }
        }

        return Ok(cashRegisterModels);
    }
    
    // GET: api/cashregister/{supermarketName}/{registerNumber}
    [HttpGet("getDetails/{supermarketName}/{registerNumber}")]
    public ActionResult<CashRegisterModel> GetBySupermarketNameAndRegisterNumber(string supermarketName, int registerNumber)
    {
        var supermarket = _supermarketDAL.GetSupermarketByTitle(supermarketName);
        var cashRegister = _cashRegisterDAL.GetBySupermarketIdAndRegisterNumber(supermarket.Id, registerNumber);
        
        if (cashRegister != null)
        {
            var status = _lkStatusDAL.GetById(cashRegister.StatusId);
            var employee = _employeeDAL.GetById(cashRegister.Employee_Id);

            var cashRegisterModel = new CashRegisterModel
            {
                Id = cashRegister.Id,
                SupermarketName = supermarket.Title,
                RegisterNumber = cashRegister.RegisterNumber,
                StatusName = status.StatusName,
                EmployeeId = cashRegister.Employee_Id,
                EmployeeName = employee.FirstName + " " + employee.LastName
            };

            return Ok(cashRegisterModel);
        }
        
        return NotFound("Cash register not found.");
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
            EmployeeId = cashRegister.Employee_Id
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
        if (model.EmployeeId == 0)
        {
            cashRegister.Employee_Id = null;
        }
        else
        {
            cashRegister.Employee_Id = model.EmployeeId;
        }
        
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
            
        };
        
        if (model.EmployeeId == 0)
        {
            cashRegister.Employee_Id = null;
        }
        else
        {
            cashRegister.Employee_Id = model.EmployeeId;
        }
        
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
        var userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        IEnumerable<Employee> employees;
        
        if (role == "admin")
        {
            employees = _employeeDAL.GetAll();
            return Ok(employees);
        }
        else if(role == "manager")
        {
            var employee = _employeeDAL.GetById(userId);
            employees = _employeeDAL.GetAll()
                .Where(e => e.SupermarketId == employee.SupermarketId);
        }
        else
        {
            return BadRequest("You do not have permission to view this resource.");
        }
        return Ok(employees);
    }

    [HttpGet("employee")]
    public ActionResult<Employee> GetEmployee(int id)
    {
        var employee = _employeeDAL.GetById(id);
        return Ok(employee);
    }
    
    [HttpGet("supermarkets"), Authorize]
    public ActionResult<IEnumerable<Supermarket>> GetSupermarkets()
    {
        var userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        IEnumerable<Supermarket> supermarkets;
        
        if (role == "admin")
        {
            supermarkets = _supermarketDAL.GetAll();
            return Ok(supermarkets);
        }
        else if(role == "manager")
        {
            var employee = _employeeDAL.GetById(userId);
            supermarkets = _supermarketDAL.GetAll()
                .Where(e => e.Id == employee.SupermarketId);
        }
        else
        {
            return BadRequest("You do not have permission to view this resource.");
        }
        return Ok(supermarkets);
    }
}