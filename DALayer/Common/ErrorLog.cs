﻿using SCMModels.SCMModels;
using System.Data.SqlClient;

namespace DALayer.Common
{
	public class ErrorLog
	{
		public void ErrorMessage(string controllername, string methodname, string exception)
		{
			YSCMEntities DB = new YSCMEntities();
			string query = "insert into dbo.ApiErrorLog(ControllerName,MethodName,ExceptionMsg)values('"+controllername+"', '"+methodname+"', '"+exception+"')";
			SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
			SqlCommand cmd = new SqlCommand(query, con);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
			

		}
	}
}