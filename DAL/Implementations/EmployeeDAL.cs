﻿using System.Data;
using Dapper;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Dapper.Oracle;

namespace Hubler.DAL.Implementations;

public class EmployeeDAL : IEmployeeDAL
    {
        public Employee GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_email", dbType: OracleMappingType.Varchar2, size: 100, direction: ParameterDirection.Output);
                parameters.Add("p_passhash", dbType: OracleMappingType.Varchar2, size: 100, direction: ParameterDirection.Output);
                parameters.Add("p_firstname", dbType: OracleMappingType.Varchar2, size: 64, direction: ParameterDirection.Output);
                parameters.Add("p_lastname", dbType: OracleMappingType.Varchar2, size: 64, direction: ParameterDirection.Output);
                parameters.Add("p_createddate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_roleid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_content_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_EMPLOYEE_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new Employee
                {
                    Id = id,
                    Email = parameters.Get<string>("p_email"),
                    PassHash = parameters.Get<string>("p_passhash"),
                    FirstName = parameters.Get<string>("p_firstname"),
                    LastName = parameters.Get<string>("p_lastname"),
                    CreatedDate = parameters.Get<DateTime>("p_createddate"),
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    RoleId = parameters.Get<int>("p_roleid"),
                    ContentId = parameters.Get<int>("p_content_id")
                };
            }
        }

        public void Insert(Employee employee)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", employee.Id, OracleMappingType.Int32);
                parameters.Add("p_email", employee.Email, OracleMappingType.Varchar2);
                parameters.Add("p_passhash", employee.PassHash, OracleMappingType.Varchar2);
                parameters.Add("p_firstname", employee.FirstName, OracleMappingType.Varchar2);
                parameters.Add("p_lastname", employee.LastName, OracleMappingType.Varchar2);
                parameters.Add("p_createddate", employee.CreatedDate, OracleMappingType.Date);
                parameters.Add("p_supermarketid", employee.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_roleid", employee.RoleId, OracleMappingType.Int32);
                parameters.Add("p_content_id", employee.ContentId, OracleMappingType.Int32);

                connection.Execute("INSERT_EMPLOYEE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(Employee employee)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", employee.Id, OracleMappingType.Int32);
                parameters.Add("p_email", employee.Email, OracleMappingType.Varchar2);
                parameters.Add("p_passhash", employee.PassHash, OracleMappingType.Varchar2);
                parameters.Add("p_firstname", employee.FirstName, OracleMappingType.Varchar2);
                parameters.Add("p_lastname", employee.LastName, OracleMappingType.Varchar2);
                parameters.Add("p_createddate", employee.CreatedDate, OracleMappingType.Date);
                parameters.Add("p_supermarketid", employee.SupermarketId, OracleMappingType.Int32);
                parameters.Add("p_roleid", employee.RoleId, OracleMappingType.Int32);
                parameters.Add("p_content_id", employee.ContentId, OracleMappingType.Int32);

                connection.Execute("UPDATE_EMPLOYEE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_EMPLOYEE", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                using (var multi = connection.QueryMultiple("GET_ALL_EMPLOYEES", commandType: CommandType.StoredProcedure))
                {
                    return multi.Read<Employee>();
                }
            }
        }
        
        public Employee GetByEmail(string email)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_email", email, OracleMappingType.Varchar2);
                parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_passhash", dbType: OracleMappingType.Varchar2, size: 100, direction: ParameterDirection.Output);
                parameters.Add("p_firstname", dbType: OracleMappingType.Varchar2, size: 64, direction: ParameterDirection.Output);
                parameters.Add("p_lastname", dbType: OracleMappingType.Varchar2, size: 64, direction: ParameterDirection.Output);
                parameters.Add("p_createddate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);
                parameters.Add("p_supermarketid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_roleid", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("p_content_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                connection.Execute("GET_EMPLOYEE_BY_EMAIL", parameters, commandType: CommandType.StoredProcedure);

                return new Employee
                {
                    Id = parameters.Get<int>("p_id"),
                    Email = email,
                    PassHash = parameters.Get<string>("p_passhash"),
                    FirstName = parameters.Get<string>("p_firstname"),
                    LastName = parameters.Get<string>("p_lastname"),
                    CreatedDate = parameters.Get<DateTime>("p_createddate"),
                    SupermarketId = parameters.Get<int>("p_supermarketid"),
                    RoleId = parameters.Get<int>("p_roleid"),
                    ContentId = parameters.Get<int>("p_content_id")
                };
            }
        }

    }