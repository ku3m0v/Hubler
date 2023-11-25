using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class CashRegisterDAL : ICashRegisterDAL
{
    public CashRegister GetById(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_id", id, OracleMappingType.Int32);
            parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_registernumber", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_statusid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("p_employee_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            connection.Execute("GET_CASHREGISTER_BY_ID", parameters, commandType: CommandType.StoredProcedure);

            return new CashRegister
            {
                Id = id,
                SupermarketId = parameters.Get<int>("p_supermarketid"),
                RegisterNumber = parameters.Get<int>("p_registernumber"),
                StatusId = parameters.Get<int>("p_statusid"),
                EmployeeId = parameters.Get<int?>("p_employee_id")
            };
        }
    }

    public void Insert(CashRegister cashRegister)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_supermarketid", cashRegister.SupermarketId, OracleMappingType.Int32);
            parameters.Add("p_registernumber", cashRegister.RegisterNumber, OracleMappingType.Int32);
            parameters.Add("p_statusid", cashRegister.StatusId, OracleMappingType.Int32);
            parameters.Add("p_employee_id", cashRegister.EmployeeId, OracleMappingType.Int32);

            connection.Execute("INSERT_CASHREGISTER", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public void Update(CashRegister cashRegister)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_id", cashRegister.Id, OracleMappingType.Int32);
            parameters.Add("p_supermarketid", cashRegister.SupermarketId, OracleMappingType.Int32);
            parameters.Add("p_registernumber", cashRegister.RegisterNumber, OracleMappingType.Int32);
            parameters.Add("p_statusid", cashRegister.StatusId, OracleMappingType.Int32);
            parameters.Add("p_employee_id", cashRegister.EmployeeId, OracleMappingType.Int32);

            connection.Execute("UPDATE_CASHREGISTER", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public void Delete(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_id", id, OracleMappingType.Int32);

            connection.Execute("DELETE_CASHREGISTER", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public IEnumerable<CashRegister> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            
            var parameters = new OracleDynamicParameters();
            parameters.Add("o_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);
            
            return connection.Query<CashRegister>("GET_ALL_CASHREGISTERS", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}