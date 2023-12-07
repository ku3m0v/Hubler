using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class ProductDAL : IProductDAL
    {
        public Product GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_title", dbType: OracleMappingType.Varchar2, size: 255, direction: ParameterDirection.Output);
                parameters.Add("p_currentprice", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_producttype", dbType: OracleMappingType.Varchar2, size: 15, direction: ParameterDirection.Output);

                connection.Execute("GET_PRODUCT_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Product
                {
                    Id = id,
                    Title = parameters.Get<string>("p_title"),
                    CurrentPrice = parameters.Get<decimal>("p_currentprice"),
                    ProductType = parameters.Get<string>("p_producttype")
                };
            }
        }

        public int Insert(ProductNew item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
        
                parameters.Add("p_lkproduct_id", item.LkProduct_Id, OracleMappingType.Int32);
                parameters.Add("p_producttype", item.ProductType, OracleMappingType.Varchar2);
                parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("INSERT_PRODUCT", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("p_id");
            }
        }



        public void Update(Product item)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", item.Id, OracleMappingType.Int32);
                parameters.Add("p_title", item.Title, OracleMappingType.Varchar2);
                parameters.Add("p_currentprice", item.CurrentPrice, OracleMappingType.Int32);
                parameters.Add("p_producttype", item.ProductType, OracleMappingType.Varchar2);

                connection.Execute("UPDATE_PRODUCT", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_PRODUCT", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);
            
                return connection.Query<Product>("GET_ALL_PRODUCTS", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        
        public IEnumerable<Product> GetProductsBySupermarket(int supermarketId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_supermarket_id", supermarketId, OracleMappingType.Int32);
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var result = connection.Query<Product>("GET_PRODUCTS_BY_SUPERMARKET", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }