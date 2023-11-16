using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations
{
    public class AddressDAL : IAddressDAL
    {
        public Address? GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var addressParams = new OracleDynamicParameters();
                addressParams.Add("p_id", id, OracleMappingType.Int32);
                addressParams.Add("o_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                return connection.Query<Address>(
                    "GET_ADDRESS_BY_ID",
                    addressParams,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public int Insert(Address address)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_street", address.Street, OracleMappingType.Varchar2);
                parameters.Add("p_house", address.House, OracleMappingType.Int32);
                parameters.Add("p_city", address.City, OracleMappingType.Varchar2);
                parameters.Add("p_postalcode", address.PostalCode, OracleMappingType.Varchar2);
                parameters.Add("p_country", address.Country, OracleMappingType.Varchar2);
                parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("INSERT_ADDRESS", parameters, commandType: CommandType.StoredProcedure);
        
                return parameters.Get<int>("p_id");
            }
        }


        public void Update(Address address)
        {
            using (var connection = DBConnection.GetConnection())
            {
                
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", address.Id, OracleMappingType.Int32);
                parameters.Add("p_street", address.Street, OracleMappingType.Varchar2);
                parameters.Add("p_house", address.House, OracleMappingType.Int32);
                parameters.Add("p_city", address.City, OracleMappingType.Varchar2);
                parameters.Add("p_postalcode", address.PostalCode, OracleMappingType.Varchar2);
                parameters.Add("p_country", address.Country, OracleMappingType.Varchar2);

                connection.Execute("UPDATE_ADDRESS", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_ADDRESS", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Address> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("o_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                return connection.Query<Address>("GET_ALL_ADDRESSES", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}