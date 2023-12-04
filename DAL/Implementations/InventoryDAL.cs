using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;

namespace Hubler.DAL.Implementations;

public class InventoryDAL : IInventoryDAL
    {
        public Inventory GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_productid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_quantity", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_INVENTORY_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Inventory
                {
                    Id = id,
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    ProductId = parameters.Get<int>("p_productid"),
                    Quantity = parameters.Get<int>("p_quantity")
                };
            }
        }

        public void Insert(Inventory inventory)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_supermarketid", inventory.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", inventory.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantity", inventory.Quantity, OracleMappingType.Int32);

                connection.Execute("INSERT_INVENTORY", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(Inventory inventory)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", inventory.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", inventory.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_productid", inventory.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantity", inventory.Quantity, OracleMappingType.Int32);

                connection.Execute("UPDATE_INVENTORY", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_INVENTORY", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Inventory> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_cursor", dbType: (OracleMappingType?)OracleDbType.RefCursor, direction: ParameterDirection.Output);

                return connection.Query<Inventory>("GET_ALL_INVENTORIES", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public string OrderProduct(int supermarketId)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_supermarketid", supermarketId, OracleMappingType.Int32);
                parameters.Add("resultMsg", dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.ReturnValue, size: 1000);

                connection.Execute("BEGIN :resultMsg := AutoOrderProductsFromInventory(:p_supermarketid); END;", parameters);

                return parameters.Get<string>("resultMsg");
            }
        }

    }