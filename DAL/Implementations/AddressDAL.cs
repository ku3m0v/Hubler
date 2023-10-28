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
        public Address GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_street", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_house", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_city", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_postalcode", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_country", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);

                connection.Execute("GET_ADDRESS_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Address
                {
                    Id = id,
                    Street = parameters.Get<string>("p_street"),
                    House = parameters.Get<int>("p_house"),
                    City = parameters.Get<string>("p_city"),
                    PostalCode = parameters.Get<string>("p_postalcode"),
                    Country = parameters.Get<string>("p_country")
                };
            }
        }

        public void Insert(Address address)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_street", address.Street, OracleMappingType.Varchar2);
                parameters.Add("p_house", address.House, OracleMappingType.Int32);
                parameters.Add("p_city", address.City, OracleMappingType.Varchar2);
                parameters.Add("p_postalcode", address.PostalCode, OracleMappingType.Varchar2);
                parameters.Add("p_country", address.Country, OracleMappingType.Varchar2);

                connection.Execute("INSERT_ADDRESS", parameters, commandType: CommandType.StoredProcedure);
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
                return connection.Query<Address>("GET_ALL_ADDRESSES", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}