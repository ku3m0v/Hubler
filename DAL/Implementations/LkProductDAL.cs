using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class LkProductDAL : ILkProductDAL
{
    public LkProduct GetById(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_lk_product_id", id, OracleMappingType.Int32);
            parameters.Add("p_cursor", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            var result = connection.QueryFirstOrDefault<LkProduct>("GET_LK_PRODUCT_BY_ID", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }

    public void Insert(LkProduct lkProduct)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_title", lkProduct.Title, OracleMappingType.Varchar2);
            parameters.Add("p_currentprice", lkProduct.CurrentPrice, OracleMappingType.Decimal);
            parameters.Add("p_lk_product_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            connection.Execute("INSERT_LK_PRODUCT", parameters, commandType: CommandType.StoredProcedure);
            lkProduct.LkProductId = parameters.Get<int>("p_lk_product_id");
        }
    }

    public void Update(LkProduct lkProduct)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_lk_product_id", lkProduct.LkProductId, OracleMappingType.Int32);
            parameters.Add("p_title", lkProduct.Title, OracleMappingType.Varchar2);
            parameters.Add("p_currentprice", lkProduct.CurrentPrice, OracleMappingType.Decimal);

            connection.Execute("UPDATE_LK_PRODUCT", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public void Delete(int id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_lk_product_id", id, OracleMappingType.Int32);

            connection.Execute("DELETE_LK_PRODUCT", parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public IEnumerable<LkProduct> GetAll()
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_cursor", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            return connection.Query<LkProduct>("GET_ALL_LK_PRODUCTS", parameters, commandType: CommandType.StoredProcedure);
        }
    }
    
    public LkProduct GetByTitle(string title)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_title", title, OracleMappingType.Varchar2);
            parameters.Add("p_cursor", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            var result = connection.QueryFirstOrDefault<LkProduct>("GET_LK_PRODUCT_BY_TITLE", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }

}