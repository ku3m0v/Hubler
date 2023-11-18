using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class SupermarketDAL : ISupermarketDAL
    {
        public Supermarket GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32, ParameterDirection.Input);
                parameters.Add("p_title", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 100);
                parameters.Add("p_phone", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 20);
                parameters.Add("p_addressid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_SUPERMARKET_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Supermarket
                {
                    Id = id,
                    Title = parameters.Get<string>("p_title"),
                    Phone = parameters.Get<string>("p_phone"),
                    AddressId = parameters.Get<int>("p_addressid")
                };
            }
        }

        public void Insert(Supermarket supermarket)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", supermarket.Id, OracleMappingType.Int32);
                parameters.Add("p_title", supermarket.Title, OracleMappingType.Varchar2);
                parameters.Add("p_phone", supermarket.Phone, OracleMappingType.Varchar2);
                parameters.Add("p_addressid", supermarket.AddressId, OracleMappingType.Int32);

                connection.Execute("INSERT_SUPERMARKET", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(Supermarket supermarket)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", supermarket.Id, OracleMappingType.Int32);
                parameters.Add("p_title", supermarket.Title, OracleMappingType.Varchar2);
                parameters.Add("p_phone", supermarket.Phone, OracleMappingType.Varchar2);
                parameters.Add("p_addressid", supermarket.AddressId, OracleMappingType.Int32);

                connection.Execute("UPDATE_SUPERMARKET", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(string title)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_title", title, OracleMappingType.Varchar2);

                connection.Execute("DELETE_SUPERMARKET", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Supermarket> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                return connection.Query<Supermarket>("GET_ALL_SUPERMARKETS", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        
        public Supermarket GetSupermarketByTitle(string title)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_title", title, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_phone", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 20);
                parameters.Add("p_addressid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_SUPERMARKET_BY_TITLE", parameters, commandType: CommandType.StoredProcedure);

                var supermarket = new Supermarket
                {
                    Id = parameters.Get<int>("p_id"),
                    Title = title,
                    Phone = parameters.Get<string>("p_phone"),
                    AddressId = parameters.Get<int>("p_addressid")
                };

                return supermarket;
            }
        }
        
        public IEnumerable<string> GetAllTitles()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                return connection.Query<string>("GET_ALL_SUPERMARKET_TITLES", parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }