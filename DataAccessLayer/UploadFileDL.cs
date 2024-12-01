using ExcelDataReader;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using UploadFileProject.CommonLayer.Model;

namespace UploadFileProject.DataAccessLayer
{
    public class UploadFileDL : IUploadFileDL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        public UploadFileDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnectionString"]);
        }

        public async Task<ReadRecordResponse> ReadRecord()
        {
            ReadRecordResponse response = new ReadRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                string SqlQuery = @"SELECT * FROM sample.bulkuplaodtable";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                        if (sqlDataReader.HasRows)
                        {
                            response.data = new List<ReadRecord>();
                            while (await sqlDataReader.ReadAsync())
                            {
                                ReadRecord getdata = new ReadRecord();
                                getdata.UserID = sqlDataReader["UserID"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["UserID"]) : -1;
                                getdata.UserName = sqlDataReader["UserName"] != DBNull.Value ? Convert.ToString(sqlDataReader["UserName"]) : "-1";
                                getdata.EmailID = sqlDataReader["EmailID"] != DBNull.Value ? Convert.ToString(sqlDataReader["EmailID"]) : "-1";
                                getdata.MobileNumber = sqlDataReader["MobileNumber"] != DBNull.Value ? Convert.ToString(sqlDataReader["MobileNumber"]) : "-1";
                                getdata.Age = sqlDataReader["Age"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Age"]) : -1;
                                getdata.Salary = sqlDataReader["Salary"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Salary"]) : -1;
                                getdata.Gender = sqlDataReader["Gender"] != DBNull.Value ? Convert.ToString(sqlDataReader["Gender"]) : "-1";
                                response.data.Add(getdata);
                            }

                        }
                        else 
                        {
                            response.IsSuccess = false;
                            response.Message = "Record Not Found";
                            return response;
                        }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            List<ExcelBulkUploadParameter> Parameters = new List<ExcelBulkUploadParameter>();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                if (request.File.FileName.ToLower().Contains(".xlsx"))
                {
                    FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet dataset = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        ExcelBulkUploadParameter rows = new ExcelBulkUploadParameter();
                        rows.UserName = dataset.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[0]) : "-1";
                        rows.EmailID = dataset.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[1]) : "-1";
                        rows.MobileNumber = dataset.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[2]) : "-1";
                        rows.Age = dataset.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToInt32(dataset.Tables[0].Rows[i].ItemArray[3]) : -1;
                        rows.Salary = dataset.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToInt32(dataset.Tables[0].Rows[i].ItemArray[4]) : -1;
                        rows.Gender = dataset.Tables[0].Rows[i].ItemArray[5] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[5]) : "-1";
                        Parameters.Add(rows);
                    }
                    stream.Close();
                    if (Parameters.Count > 0)
                    {
                        string SqlQuery = @"INSERT INTO SAMPLE.bulkuplaodtable(UserName,EmailID,MobileNumber,Age,Salary,Gender) VALUES (@UserName,@EmailID,@MobileNumber,@Age,@Salary,@Gender)";

                        foreach (ExcelBulkUploadParameter rows in Parameters)
                        {
                            using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.CommandTimeout = 180;
                                sqlCommand.Parameters.AddWithValue("@UserName", rows.UserName);
                                sqlCommand.Parameters.AddWithValue("@EmailID", rows.EmailID);
                                sqlCommand.Parameters.AddWithValue("@MobileNumber", rows.MobileNumber);
                                sqlCommand.Parameters.AddWithValue("@Age", rows.Age);
                                sqlCommand.Parameters.AddWithValue("@Salary", rows.Salary);
                                sqlCommand.Parameters.AddWithValue("@Gender", rows.Gender);

                                int Status = await sqlCommand.ExecuteNonQueryAsync();
                                if (Status <= 0)
                                {
                                    response.IsSuccess = false;
                                    response.Message = "Query Not Executed";
                                    return response;
                                }
                                //sqlCommand.Parameters.Clear();
                            }

                        }
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect File";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
    }
}
