using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class NonPerishableDAL : INonPerishableDAL
    {
        public NonPerishable GetByProductId(int productId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", productId, OracleMappingType.Int32);
                parameters.Add("p_shelflife", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_NONPERISHABLE_BY_PRODUCTID", parameters, commandType: CommandType.StoredProcedure);

                return new NonPerishable
                {
                    ProductId = productId,
                    ShelfLife = parameters.Get<int>("p_shelflife")
                };
            }
        }

        public void Insert(NonPerishable item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", item.ProductId, OracleMappingType.Int32);
                parameters.Add("p_shelflife", item.ShelfLife, OracleMappingType.Int32);

                connection.Execute("INSERT_NONPERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(NonPerishable item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", item.ProductId, OracleMappingType.Int32);
                parameters.Add("p_shelflife", item.ShelfLife, OracleMappingType.Int32);

                connection.Execute("UPDATE_NONPERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int productId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_productid", productId, OracleMappingType.Int32);

                connection.Execute("DELETE_NONPERISHABLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<NonPerishable> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);
                
                return connection.Query<NonPerishable>("GET_ALL_NONPERISHABLES", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }