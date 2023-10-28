using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class WarehouseDAL : IWarehouseDAL
    {
        public Warehouse GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_productid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_quantity", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_WAREHOUSE_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Warehouse
                {
                    Id = id,
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    ProductId = parameters.Get<int>("p_productid"),
                    Quantity = parameters.Get<int>("p_quantity")
                };
            }
        }

        public void Insert(Warehouse warehouse)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", warehouse.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", warehouse.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", warehouse.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantity", warehouse.Quantity, OracleMappingType.Int32);

                connection.Execute("INSERT_WAREHOUSE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(Warehouse warehouse)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", warehouse.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", warehouse.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", warehouse.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantity", warehouse.Quantity, OracleMappingType.Int32);

                connection.Execute("UPDATE_WAREHOUSE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_WAREHOUSE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Warehouse> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_WAREHOUSES", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<Warehouse>();
                }
            }
        }
    }