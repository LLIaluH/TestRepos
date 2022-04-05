using DataHSMforWeb.App;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
//using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataHSMforWeb
{
    public partial class Autentification : System.Web.UI.Page
    {
        OracleConnection connect;
        OracleCommand cmd;

        string query;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public bool GetCon()
        {
            dbConnection conn = new dbConnection();
            if (conn.OpenDbConnection(out connect))
            {
                return true;
            }
            return false;
        }

        public void TryLogin(object sender, EventArgs e)
        {
            string login = UserName.Text;
            string password = Password.Text;
            query = "SELECT 1 FROM USERS WHERE" +
                 " LOGIN = :login and PASSWORD = :password";
            try
            {
                GetCon();
                cmd = connect.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("login", login);
                cmd.Parameters.Add("password", password);
                string HasUser = cmd.ExecuteScalar().ToString();
                if (HasUser == "1")
                {
                    //такой пользователь есть, вход разрешён
                    FormsAuthentication.RedirectFromLoginPage(login, RememberMe.Checked);//второй параметр для запоминания куков
                }
                else
                {
                    
                }
            }
            catch (Exception)
            {

            }
        }
    }
}