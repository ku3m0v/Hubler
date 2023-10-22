using System.Data;
using Dapper;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Dapper.Oracle;

namespace Hubler.DAL.Implementations;

public class EmployeeDAL : IEmployeeDAL
{
    // public IEnumerable<Employee> FindByEmail(string email)
    // {
    //     using (var connection = DBConnection.GetConnection())
    //     {
    //         return connection.Query<Employee>("SELECT * FROM EMPLOYEE WHERE EMAIL = :Email", new { Email = email });
    //     }
    // }

    public Employee FindById(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_employeeId", id, OracleMappingType.Int32);
            parameters.Add("p_email", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
            parameters.Add("p_passHash", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
            parameters.Add("p_firstName", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
            parameters.Add("p_lastName", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
            parameters.Add("p_createdDate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);
            parameters.Add("p_supermarketId", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_roleId", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_contentId", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            connection.Execute("FindByIdEmployee", parameters, commandType: CommandType.StoredProcedure);

            return new Employee
            {
                Id = id,
                Email = parameters.Get<string>("p_email"),
                PassHash = parameters.Get<string>("p_passHash"),
                FirstName = parameters.Get<string>("p_firstName"),
                LastName = parameters.Get<string>("p_lastName"),
                CreatedDate = parameters.Get<DateTime>("p_createdDate"),
                SupermarketId = parameters.Get<int>("p_supermarketId"),
                RoleId = parameters.Get<int>("p_roleId"),
                ContentId = parameters.Get<int>("p_contentId")
            };
        }
    }

    public int? Authenticate(string email, string passHash)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_email", email, OracleMappingType.Varchar2);
            parameters.Add("p_passHash", passHash, OracleMappingType.Varchar2);
            parameters.Add("p_employeeId", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
    
            connection.Execute("AuthenticateEmployee", parameters, commandType: CommandType.StoredProcedure);

            int? employeeId = parameters.Get<int?>("p_employeeId");
            return employeeId;
        }
    }
}