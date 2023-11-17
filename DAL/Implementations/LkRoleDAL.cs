using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class LkRoleDAL : ILkRoleDAL
    {
        public LkRole GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32, direction: ParameterDirection.Input);
                parameters.Add("p_rolename", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 50);

                connection.Execute("GET_ROLE_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new LkRole
                {
                    Id = id,
                    RoleName = parameters.Get<string>("p_rolename")
                };
            }
        }

        public void Insert(LkRole role)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_rolename", role.RoleName, OracleMappingType.Varchar2);

                connection.Execute("INSERT_ROLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(LkRole role)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", role.Id, OracleMappingType.Int32);
                parameters.Add("p_rolename", role.RoleName, OracleMappingType.Varchar2);

                connection.Execute("UPDATE_ROLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_ROLE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<LkRole> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_ROLES", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<LkRole>();
                }
            }
        }
    }