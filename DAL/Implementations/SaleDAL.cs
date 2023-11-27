using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class SaleDAL : ISaleDAL
    {
        public Sale GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_dateandtime", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);

                connection.Execute("GET_SALE_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Sale
                {
                    Id = id,
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    DateAndTime = parameters.Get<DateTime>("p_dateandtime")
                };
            }
        }

        public int Insert(Sale sale)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_supermarketid", sale.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_dateandtime", sale.DateAndTime, OracleMappingType.Date);
                parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("INSERT_SALE", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("p_id");
            }
        }

        public void Update(Sale sale)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", sale.Id, OracleMappingType.Int32);
                parameters.Add("p_supermarketid", sale.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_dateandtime", sale.DateAndTime, OracleMappingType.Date);

                connection.Execute("UPDATE_SALE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_SALE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Sale> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_SALES", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<Sale>();
                }
            }
        }
    }