//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;

namespace DataHSMforWeb.App
{
    public class dbConnection
    {
        public bool OpenDbConnection(out OracleConnection conn)
        {
            try
            {
                string _connect = ConfigurationManager.ConnectionStrings["L2"].ToString();
                conn = new OracleConnection(_connect);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
                conn = null;
                return false;
            }
        }
    }
}