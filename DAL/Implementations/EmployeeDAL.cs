using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Dapper;

namespace Hubler.DAL.Implementations;

public class EmployeeDAL : IEmployeeDAL
{
    public IEnumerable<Employee> FindByEmail(string email)
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<Employee>("SELECT * FROM EMPLOYEE WHERE EMAIL = :Email", new { Email = email });
        }
    }

    public Employee FindById(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.QueryFirstOrDefault <Employee>("SELECT * FROM EMPLOYEE WHERE ID = :Id", new { Id = id });
        }
    }
}