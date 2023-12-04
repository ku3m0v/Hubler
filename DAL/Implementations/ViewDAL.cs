using Dapper;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class ExpiredInventoryDAL : IViewDAL<ExpiredInventory>
{
    public IEnumerable<ExpiredInventory> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<ExpiredInventory>("SELECT * FROM EXPIRED_INVENTORY");
        }
    }
}

public class ExpiredWarehouseDAL : IViewDAL<ExpiredWarehouse>
{
    public IEnumerable<ExpiredWarehouse> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<ExpiredWarehouse>("SELECT * FROM EXPIRED_WAREHOUSE");
        }
    }
}

public class Top5ProductsBySupermarketDAL : IViewDAL<Top5ProductsBySupermarket>
{
    public IEnumerable<Top5ProductsBySupermarket> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<Top5ProductsBySupermarket>("SELECT * FROM TOP_5_PRODUCTS_BY_SUPERMARKET");
        }
    }
}

public class SupermarketSalesSummaryDAL : IViewDAL<SupermarketSalesSummary>
{
    public IEnumerable<SupermarketSalesSummary> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            return connection.Query<SupermarketSalesSummary>("SELECT * FROM V_SUPERMARKET_SALES_SUMMARY");
        }
    }
}