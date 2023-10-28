using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class ProductOrderDAL : IProductOrderDAL
    {
        public ProductOrder GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_productid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_orderedquantity", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_orderdate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);

                connection.Execute("GET_PRODUCT_ORDER_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new ProductOrder
                {
                    Id = id,
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    ProductId = parameters.Get<int>("p_productid"),
                    OrderedQuantity = parameters.Get<int>("p_orderedquantity"),
                    OrderDate = parameters.Get<DateTime>("p_orderdate")
                };
            }
        }

        public void Insert(ProductOrder order)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", order.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", order.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", order.ProductId, OracleMappingType.Int32);
                parameters.Add("p_orderedquantity", order.OrderedQuantity, OracleMappingType.Int32);
                parameters.Add("p_orderdate", order.OrderDate, OracleMappingType.Date);

                connection.Execute("INSERT_PRODUCT_ORDER", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(ProductOrder order)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", order.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", order.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", order.ProductId, OracleMappingType.Int32);
                parameters.Add("p_orderedquantity", order.OrderedQuantity, OracleMappingType.Int32);
                parameters.Add("p_orderdate", order.OrderDate, OracleMappingType.Date);

                connection.Execute("UPDATE_PRODUCT_ORDER", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_PRODUCT_ORDER", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ProductOrder> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_PRODUCT_ORDERS", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<ProductOrder>();
                }
            }
        }
    }