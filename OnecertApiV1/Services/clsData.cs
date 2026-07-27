using System.Data;
using System.Data.SqlClient;
namespace OnecertApiV1.Services
{

    public class clsData
    {
        private readonly IConfiguration _configuration;
        private SqlConnection sqlCon;

        public clsData(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlCon = new SqlConnection();
        }

        public SqlConnection Connection()
        {
            try
            {
                string conString = _configuration.GetConnectionString("SqlConnection");

                if (sqlCon == null || sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon = new SqlConnection(conString);
                    sqlCon.Open();
                }

                return sqlCon;
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return null;
            }
        }

        public void CloseConnection()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }

        public string TestConnection()
        {
            try
            {
                string conString = _configuration.GetConnectionString("SqlConnection");

                using (var testCon = new SqlConnection(conString))
                {
                    testCon.Open();
                    return "Successful";
                }
            }
            catch (Exception ex)
            {
                return "Failed. " + ex.Message;
            }
        }
    }

}



