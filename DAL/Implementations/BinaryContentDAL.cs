using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.DAL.Implementations;

public class BinaryContentDAL : IBinaryContentDAL
    {
        public BinaryContent GetById(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);
                parameters.Add("p_filename", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_filetype", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_fileextension", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);
                parameters.Add("p_content", dbType: OracleMappingType.Blob, direction: ParameterDirection.Output);
                parameters.Add("p_uploaddate", dbType: OracleMappingType.Date, direction: ParameterDirection.Output);

                connection.Execute("GET_BINARY_CONTENT_BY_ID", parameters, commandType: CommandType.StoredProcedure);

                return new BinaryContent
                {
                    Id = id,
                    FileName = parameters.Get<string>("p_filename"),
                    FileType = parameters.Get<string>("p_filetype"),
                    FileExtension = parameters.Get<string>("p_fileextension"),
                    Content = parameters.Get<byte[]>("p_content"),
                    UploadDate = parameters.Get<DateTime>("p_uploaddate")
                };
            }
        }

        public void Insert(BinaryContent content)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_filename", content.FileName, OracleMappingType.Varchar2);
                parameters.Add("p_filetype", content.FileType, OracleMappingType.Varchar2);
                parameters.Add("p_fileextension", content.FileExtension, OracleMappingType.Varchar2);
                parameters.Add("p_content", content.Content, OracleMappingType.Blob);
                parameters.Add("p_uploaddate", content.UploadDate, OracleMappingType.Date);

                connection.Execute("INSERT_BINARY_CONTENT", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(BinaryContent content)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", content.Id, OracleMappingType.Int32);
                parameters.Add("p_filename", content.FileName, OracleMappingType.Varchar2);
                parameters.Add("p_filetype", content.FileType, OracleMappingType.Varchar2);
                parameters.Add("p_fileextension", content.FileExtension, OracleMappingType.Varchar2);
                parameters.Add("p_content", content.Content, OracleMappingType.Blob);
                parameters.Add("p_uploaddate", content.UploadDate, OracleMappingType.Date);

                connection.Execute("UPDATE_BINARY_CONTENT", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DBConnection.GetConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_id", id, OracleMappingType.Int32);

                connection.Execute("DELETE_BINARY_CONTENT", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<BinaryContent> GetAll()
        {
            using (var connection = DBConnection.GetConnection())
            {
                return connection.Query<BinaryContent>("GET_ALL_BINARY_CONTENTS", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }