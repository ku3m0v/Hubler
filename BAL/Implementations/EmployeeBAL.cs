using Hubler.BAL.Interfaces;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.BAL.Implementations;

public class EmployeeBAL : IEmployeeBAL
{
    
    private readonly IEmployeeDAL _employeeDAL;
    
    public EmployeeBAL(IEmployeeDAL employeeDAL)
    {
        _employeeDAL = employeeDAL;
    }
    
    // public int? Authenticate(string email, string passHash)
    // {
    //     string encryptedPassword = EncryptPassword(passHash);
    //     foreach (Employee employee in _employeeDAL.FindByEmail(email))
    //     {
    //         if (employee.PassHash == passHash)
    //         {
    //             return employee.Id;
    //         }
    //     }
    //     return null;
    // }
    
    public string EncryptPassword(string passHash)
    {
        return passHash;
    }
}