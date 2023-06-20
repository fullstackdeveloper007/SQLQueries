using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;


namespace OPA.Business.SQLHandler
{
    public class SqlHelper
    {      
       
        public static  string GetConnectionString(int dbId) 
        {            
            string ConnString = ConfigurationManager.ConnectionStrings["connectionPath"].ConnectionString;           
            return ConnString;
        }
        

        //public static void ExcuteNonQuery(string query, IDataParameter[] parameters, int dbId   )
        //{
        //    string connString;
        //    try
        //    {
        //        //List<SqlParameter> dbParams = new List<SqlParameter>();
        //        //dbParams.Add(new SqlParameter() { ParameterName = "@PromptID", SqlDbType = SqlDbType.DateTime2, Value = "" });

        //        connString = GetConnectionString(dbId);
        //        SqlConnection conn = new SqlConnection(connString);
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();
        //        //conn.ConnectionTimeout = 0;
        //        SqlCommand cmd = new SqlCommand(query, conn);

        //        if (parameters != null)
        //        {
        //            foreach (SqlParameter parameter in parameters)
        //            {
        //                cmd.Parameters.Add(parameter.ParameterName, parameter.SqlDbType).Value = parameter.Value;
        //            }
        //        }
        //        //cmd.Parameters.AddRange(dbParams.ToArray());

        //        cmd.CommandTimeout = 0;
        //        cmd.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}

        public static string CreateInsertStatement(string tableName, dynamic opaResponse, out List<SqlParameter> dbParams)
        {
            StringBuilder sqlStringValues = new StringBuilder();
            StringBuilder sqlStringKeys = new StringBuilder();
            dbParams = new List<SqlParameter>();

            sqlStringKeys.Append("INSERT INTO ");
            sqlStringKeys.Append(tableName + " ( ");

            foreach (KeyValuePair<string, object> kvp in opaResponse)
            {
                sqlStringKeys.Append(string.Format("{0},", kvp.Key));
                sqlStringValues.Append(String.Format("@{0},", kvp.Key));
                if (kvp.Value.GetType() == typeof(DateTime))
                {
                    dbParams.Add(new SqlParameter() { ParameterName = "@" + kvp.Key, SqlDbType = SqlDbType.DateTime2, Value = kvp.Value });
                }
                else
                {
                    dbParams.Add(new SqlParameter() { ParameterName = "@" + kvp.Key, SqlDbType = SqlDbType.VarChar, Value = kvp.Value });
                }
            }
            sqlStringKeys.Remove(sqlStringKeys.Length - 1, 1);
            sqlStringKeys.Append(" ) VALUES ( ");

            sqlStringValues.Remove(sqlStringValues.Length - 1, 1);
            sqlStringValues.Append(")");

            sqlStringKeys.Append(sqlStringValues.ToString());
            return sqlStringKeys.ToString();
        }

        public static int ExcuteNonQuery(string query, List<SqlParameter> dbParams, int dbId)
        {
            int noOfRowsEffected;
            SqlCommand cmd;
            SqlConnection conn;

            conn = new SqlConnection(GetConnectionString(dbId));

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddRange(dbParams.ToArray());
            noOfRowsEffected = cmd.ExecuteNonQuery();
            conn.Close();

            return noOfRowsEffected;
        }

        public static DataTable GetDataTableFromProc(string procName, IDataParameter[] parameters, int dbId)
        {
            DataTable dt;
            using (var conn = new SqlConnection(GetConnectionString(dbId)))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = procName;
                dt = new DataTable();
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        com.Parameters.Add(parameter);
                    }
                }

                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                var dataset = new DataSet();
                adapt.Fill(dataset);
                if (dataset.Tables.Count > 0)
                    dt = dataset.Tables[0];

            }
            return dt;
        }

        public static DataTable GetDataTable(string sqlString,int dbId )
        {
            var table = new DataTable();
            using (var da = new SqlDataAdapter(sqlString, GetConnectionString(dbId)))
            {
                da.Fill(table);
            }
            return table;
        }

        public static string ExecuteScalar(string sqlString, IDataParameter[] parameters, int dbId)
        {
            SqlConnection cnn;
            SqlCommand cmd;            
            string result = string.Empty;

            using (cnn = new SqlConnection(GetConnectionString(dbId)))
            {
                cnn.Open();
                cmd = new SqlCommand(sqlString, cnn);
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                result = Convert.ToString(cmd.ExecuteScalar());
                cmd.Dispose();
                cnn.Close();
            }
            return result;
        }
    }
}