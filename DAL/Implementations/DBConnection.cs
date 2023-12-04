using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class DBConnection
{
    private static String ConnectionString { get; set; } = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));" +
                                                                           "user id=ST67058;password=qwerty;" +
                                                                           "Connection Timeout=120;Validate connection=true;Min Pool Size=4;";


    public static OracleConnection GetConnection()
    {
        return new OracleConnection(ConnectionString);
    }

}