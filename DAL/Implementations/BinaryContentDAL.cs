using System.Data;
using Dapper;
using Dapper.Oracle;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Hubler.DAL.Implementations;

public class BinaryContentDAL : IBinaryContentDAL
{
    public BinaryContent GetById(int? id)
    {
        using (var connection = DBConnection.GetConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "GET_BINARY_CONTENT_BY_ID";
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.Add(new OracleParameter("p_id", id));
                command.Parameters.Add(new OracleParameter("p_filename", OracleDbType.Varchar2, 255, null,
                    ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("p_filetype", OracleDbType.Varchar2, 50, null,
                    ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("p_fileextension", OracleDbType.Varchar2, 10, null,
                    ParameterDirection.Output));
                command.Parameters.Add(new OracleParameter("p_content", OracleDbType.Blob, ParameterDirection.Output));
                command.Parameters.Add(
                    new OracleParameter("p_uploaddate", OracleDbType.Date, ParameterDirection.Output));

                connection.Open();
                command.ExecuteNonQuery();

                byte[] blobData = null;
                if (command.Parameters["p_content"].Value != DBNull.Value)
                {
                    var blob = (OracleBlob)command.Parameters["p_content"].Value;
                    blobData = new byte[blob.Length];
                    blob.Read(blobData, 0, (int)blob.Length);
                }

                DateTime? uploadDate = null;
                if (command.Parameters["p_uploaddate"].Value != DBNull.Value)
                {
                    OracleDate oracleDate = (OracleDate)command.Parameters["p_uploaddate"].Value;
                    uploadDate = oracleDate.Value;
                }

                return new BinaryContent
                {
                    Id = id,
                    FileName = command.Parameters["p_filename"].Value.ToString(),
                    FileType = command.Parameters["p_filetype"].Value.ToString(),
                    FileExtension = command.Parameters["p_fileextension"].Value.ToString(),
                    Content = blobData,
                    UploadDate = uploadDate
                };
            }
        }
    }


    public int Insert(BinaryContent content)
    {
        using (var connection = DBConnection.GetConnection())
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_filename", content.FileName, OracleMappingType.Varchar2);
            parameters.Add("p_filetype", content.FileType, OracleMappingType.Varchar2);
            parameters.Add("p_fileextension", content.FileExtension, OracleMappingType.Varchar2);
            parameters.Add("p_content", content.Content, OracleMappingType.Blob);
            parameters.Add("p_uploaddate", content.UploadDate, OracleMappingType.Date);
            parameters.Add("p_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            connection.Execute("INSERT_BINARY_CONTENT", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("p_id"); // Retrieve the ID of the inserted record
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

    public void Delete(int? id)
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
            return connection.Query<BinaryContent>("GET_ALL_BINARY_CONTENTS", commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }
}