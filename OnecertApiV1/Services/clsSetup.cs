using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnecertApiV1.Services
{
    public class clsSetup
    {
        private readonly clsData _db;
        public clsSetup(clsData db)
        {
                _db = db;
        }
        private string _prevConValue, _Code, _Category, _Description, _Remarks, _OnlyRead, _AuthStatus, _CreatedBy, _AuthorisedBy, _ModifiedBy, _CreatedDt, _AuthorisedDt, _ModifiedDt, _AuthCnt;

        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }

        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                _Remarks = value;
            }
        }

        public string OnlyRead
        {
            get
            {
                return _OnlyRead;
            }
            set
            {
                _OnlyRead = value;
            }
        }

        public string AuthStatus
        {
            get
            {
                return _AuthStatus;
            }
            set
            {
                _AuthStatus = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                _CreatedBy = value;
            }
        }

        public string AuthorisedBy
        {
            get
            {
                return _AuthorisedBy;
            }
            set
            {
                _AuthorisedBy = value;
            }
        }

        public string ModifiedBy
        {
            get
            {
                return _ModifiedBy;
            }
            set
            {
                _ModifiedBy = value;
            }
        }

        public string CreatedDt
        {
            get
            {
                return _CreatedDt;
            }
            set
            {
                _CreatedDt = value;
            }
        }

        public string AuthorisedDt
        {
            get
            {
                return _AuthorisedDt;
            }
            set
            {
                _AuthorisedDt = value;
            }
        }

        public string ModifiedDt
        {
            get
            {
                return _ModifiedDt;
            }
            set
            {
                _ModifiedDt = value;
            }
        }

        public string AuthCnt
        {
            get
            {
                return _AuthCnt;
            }
            set
            {
                _AuthCnt = value;
            }
        }

        public string prevConValue
        {
            get
            {
                return _prevConValue;
            }
            set
            {
                _prevConValue = value;
            }
        }


        public clsSetup()
        {

        }

        public clsSetup(string Category, string Code)
        {
            var resDS = new DataSet();
            var sqlCommand = new SqlCommand();
            var objData = _db;

            sqlCommand.Connection = objData.Connection();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "spSetupGet";
            sqlCommand.Parameters.Add(new  SqlParameter("mCategory", DbType.String)).Value = Category;
            sqlCommand.Parameters.Add(new  SqlParameter("mCode", DbType.String)).Value = Code;

            var DA = new  SqlDataAdapter(sqlCommand);

            DA.Fill(resDS);

            var myRow = resDS.Tables[0].Rows[0];

            this.Category = myRow["Category"].ToString();
            this.Code = myRow["Code"].ToString();
            Description = myRow["Description"].ToString();
            Remarks = myRow["Remarks"].ToString();
            OnlyRead = myRow["OnlyRead"].ToString();
            AuthStatus = myRow["AuthStatus"].ToString();
            CreatedBy = myRow["CreatedBy"].ToString();
            AuthorisedBy = myRow["AuthorisedBy"].ToString();
            ModifiedBy = myRow["ModifiedBy"].ToString();
            CreatedDt = myRow["CreatedDt"].ToString();
            AuthorisedDt = myRow["AuthorisedDt"].ToString();
            ModifiedDt = myRow["ModifiedDt"].ToString();
            AuthCnt = myRow["AuthorisedCnt"].ToString();
            prevConValue = myRow["prevConValue"].ToString();

            sqlCommand.Dispose();
            resDS.Dispose();
            DA.Dispose();
            objData.CloseConnection();
            objData = null;
        }

        public  DataTable listSetup(string Category)
        {
            DataTable listSetupRet;
            var resDS = new DataSet();
            var sqlCommand = new  SqlCommand();
            var objData = _db;

            sqlCommand.Connection = objData.Connection();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "spSetupList";

            sqlCommand.Parameters.Add(new  SqlParameter("mCategory", DbType.String)).Value = Category;

            var DA = new  SqlDataAdapter(sqlCommand);
            DA.Fill(resDS);

            listSetupRet = resDS.Tables[0];
            sqlCommand.Dispose();
            resDS.Dispose();
            DA.Dispose();
            objData.CloseConnection();
            objData = null;
            return listSetupRet;
        }

        public  DataTable listSetupALL()
        {
            DataTable listSetupALLRet;
            var resDS = new DataSet();
            var sqlCommand = new  SqlCommand();
            var objData = _db;

            sqlCommand.Connection = objData.Connection();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "spSetupListALL";

            var DA = new  SqlDataAdapter(sqlCommand);
            DA.Fill(resDS);

            listSetupALLRet = resDS.Tables[0];
            sqlCommand.Dispose();
            resDS.Dispose();
            DA.Dispose();
            objData.CloseConnection();
            objData = null;
            return listSetupALLRet;
        }

        public  DataTable listSomeSetup(string where_Str)
        {
            DataTable listSomeSetupRet;
            var resDS = new DataSet();
            var sqlCommand = new  SqlCommand();
            var objData = _db;

            sqlCommand.Connection = objData.Connection();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "spSetupGetList";
            sqlCommand.Parameters.Add(new  SqlParameter("p_join_str", DbType.String)).Value = DBNull.Value;
            sqlCommand.Parameters.Add(new  SqlParameter("p_where_str", DbType.String)).Value = where_Str;
            sqlCommand.Parameters.Add(new  SqlParameter("p_sort_str", DbType.String)).Value = DBNull.Value;
            sqlCommand.Parameters.Add(new  SqlParameter("p_page_number", DbType.Int32)).Value = 1;
            sqlCommand.Parameters.Add(new  SqlParameter("p_batch_size", DbType.Int32)).Value = 10000;


            var DA = new  SqlDataAdapter(sqlCommand);
            DA.Fill(resDS);

            listSomeSetupRet = resDS.Tables[1];
            sqlCommand.Dispose();
            resDS.Dispose();
            DA.Dispose();
            objData.CloseConnection();
            objData = null;
            return listSomeSetupRet;
        }

        public void DeleteSetup(string Category, string Code)
        {
            var sqlCommand = new  SqlCommand();
            var objData = _db;

            sqlCommand.Connection = objData.Connection();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "spSetupDelete";
            sqlCommand.Parameters.Add(new  SqlParameter("mCategory", DbType.String)).Value = Category;
            sqlCommand.Parameters.Add(new  SqlParameter("mCode", DbType.String)).Value = Code;
            sqlCommand.ExecuteNonQuery();

            sqlCommand.Dispose();
            objData.CloseConnection();
            objData = null;
        }

     
       
    }
}