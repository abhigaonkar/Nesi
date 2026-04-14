using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeDB
	/// </summary>
	public class NeDB
		{
		// can be used for ACCESS
		public static OleDbConnection getConAccess(string strPath)
			{
			var myConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;data source=" + strPath;
			var conn = new OleDbConnection(myConnectionString);
			conn.Open();        
			return conn;
			}
		// Use for Alitgen Connection
		public static SqlConnection getAltigenConnection()
			{
			var myConnectionString = @"Data Source=NE-VSERVER-05\SQLEXPRESS;Initial Catalog=ExternalCDR;User ID=sa;Password=N3p@$$20!1";
			var conn = new SqlConnection(myConnectionString);
			conn.Open();
			return conn;
			}

		public static OdbcConnection get_altigen_Connection2()
			{
			var myConnectionString = @"Dsn=CDR;uid=sa;pwd=N3p@$$20!1";
			var conn = new OdbcConnection(myConnectionString);
			conn.Open();
			return conn;
			}
		}
	}