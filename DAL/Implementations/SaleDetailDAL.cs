using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class SaleDetailDAL : ISaleDetailDAL
    {
        public SaleDetail GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_saleid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_productid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_quantitysold", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_totalprice", dbType: OracleMappingType.Decimal, direction: ParameterDirection.Output);

                connection.Execute("GET_SALE_DETAIL_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new SaleDetail
                {
                    Id = id,
                    SaleId = parameters.Get<int>("p_saleid"),
                    ProductId = parameters.Get<int>("p_productid"),
                    QuantitySold = parameters.Get<int>("p_quantitysold"),
                    TotalPrice = parameters.Get<decimal>("p_totalprice")
                };
            }
        }

        public void Insert(SaleDetail saleDetail)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", saleDetail.Id, OracleMappingType.Int32);
                parameters.Add("p_saleid", saleDetail.SaleId, OracleMappingType.Int32);
                parameters.Add("p_productid", saleDetail.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantitysold", saleDetail.QuantitySold, OracleMappingType.Int32);
                parameters.Add("p_totalprice", saleDetail.TotalPrice, OracleMappingType.Decimal);

                connection.Execute("INSERT_SALE_DETAIL", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(SaleDetail saleDetail)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", saleDetail.Id, OracleMappingType.Int32);
                parameters.Add("p_saleid", saleDetail.SaleId, OracleMappingType.Int32);
                parameters.Add("p_productid", saleDetail.ProductId, OracleMappingType.Int32);
                parameters.Add("p_quantitysold", saleDetail.QuantitySold, OracleMappingType.Int32);
                parameters.Add("p_totalprice", saleDetail.TotalPrice, OracleMappingType.Decimal);

                connection.Execute("UPDATE_SALE_DETAIL", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_SALE_DETAIL", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<SaleDetail> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_SALE_DETAILS", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<SaleDetail>();
                }
            }
        }
    }