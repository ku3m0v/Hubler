using Dapper;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class LogDAL : ILogDAL
{
    public IEnumerable<Log> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<Log>("SELECT * FROM LOG_TABLE");
        }
    }
}