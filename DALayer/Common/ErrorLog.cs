using SCMModels.SCMModels;
using System;
using System.Data.SqlClient;
using System.Web;

namespace DALayer.Common
{
	public class ErrorLog
	{
		public void ErrorMessage(string controllername, string methodname, string exception)
		{
			YSCMEntities DB = new YSCMEntities();
			string query = "insert into dbo.ApiErrorLog(ControllerName,MethodName,ExceptionMsg,OccuredDate,URL)values('" + controllername+"', '"+methodname+"', '"+exception+ "','"+ DateTime.Now + "','" + HttpContext.Current.Request.Url + "')";
			SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
			SqlCommand cmd = new SqlCommand(query, con);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
			

		}
	}
}