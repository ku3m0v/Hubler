using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class PerishableDAL : IPerishableDAL
    {
        public Perishable GetByProductId(int productId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", productId, OracleMappingType.Int32);
                parameters.Add("p_expirydate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);
                parameters.Add("p_storagetype", dbType: OracleMappingType.Varchar2, size: 255, direction: ParameterDirection.Output);

                connection.Execute("GET_PERISHABLE_BY_PRODUCTID", parameters, commandType: CommandType.StoredProcedure);

                return new Perishable
                {
                    ProductId = productId,
                    ExpiryDate = parameters.Get<DateTime>("p_expirydate"),
                    StorageType = parameters.Get<string>("p_storagetype")
                };
            }
        }

        public void Insert(Perishable item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", item.ProductId, OracleMappingType.Int32);
                parameters.Add("p_expirydate", item.ExpiryDate, OracleMappingType.Date);
                parameters.Add("p_storagetype", item.StorageType, OracleMappingType.Varchar2);

                connection.Execute("INSERT_PERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(Perishable item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", item.ProductId, OracleMappingType.Int32);
                parameters.Add("p_expirydate", item.ExpiryDate, OracleMappingType.Date);
                parameters.Add("p_storagetype", item.StorageType, OracleMappingType.Varchar2);

                connection.Execute("UPDATE_PERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int productId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", productId, OracleMappingType.Int32);

                connection.Execute("DELETE_PERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Perishable> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);
            
                return connection.Query<Perishable>("GET_ALL_PERISHABLES", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
