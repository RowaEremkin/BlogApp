using System;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine;
using System.Text;

namespace Rowa.Blog
{
	public class SqlAccess
	{
		public static MySqlConnection dbConnection;

		//If it is only local, just write localhost.
		// static string host = "localhost";  
		//If it is a local area network, then write the local area network IP of the machine
		static string host = "localhost";
		static string port = "3306";
		static string username = "BlogApp";
		static string pwd = "a10p59p14";
		static string database = "blogapp";

		public SqlAccess()
		{
			OpenSql();
		}

		/// <summary>
		/// Connect to the database
		/// </summary>
		public static void OpenSql()
		{
			try
			{
				string connectionString =
					string.Format("server = {0};port={1};database = {2};user = {3};password = {4};", host, port,
						database, username, pwd);
				Debug.Log(connectionString);
				dbConnection = new MySqlConnection(connectionString);
				dbConnection.Open();
				Debug.Log("Connection established ");
			}
			catch (Exception e)
			{
				throw new Exception("Server connection failed, please recheck whether to open MySql service." + e.Message.ToString());
			}
		}

		/// <summary>
		/// Close the database connection
		/// </summary>
		public void Close()
		{
			if (dbConnection != null)
			{
				dbConnection.Close();
				dbConnection.Dispose();
				dbConnection = null;
			}
		}

		/// <summary>
		/// Inquire
		/// </summary>
		///<param name="tableName"> Table Name</param>
		/// <param name="items"></param>
		///<param name="col"> field name</param>
		///<param name="operation"> operator</param>
		///<param name="values"> field value</param>
		/// <returns>DataSet</returns>
		public DataSet SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
		{

			if (col.Length != operation.Length || operation.Length != values.Length)
				throw new Exception("col.Length != operation.Length != values.Length");

			StringBuilder query = new StringBuilder();
			query.Append("SELECT ");
			query.Append(items[0]);

			for (int i = 1; i < items.Length; ++i)
			{
				query.Append(", ");
				query.Append(items[i]);
			}

			query.Append(" FROM ");
			query.Append(tableName);
			query.Append(" WHERE 1=1");

			for (int i = 0; i < col.Length; ++i)
			{
				query.Append(" AND ");
				query.Append(col[i]);
				query.Append(operation[i]);
				query.Append("'");
				query.Append(values[0]);
				query.Append("' ");
			}

			Debug.Log(query.ToString());
			return ExecuteQuery(query.ToString());
		}

		/// <summary>
		/// Execute the sql statement
		/// </summary>
		///<param name="sqlString"> sql statement</param>
		/// <returns></returns>
		public static DataSet ExecuteQuery(string sqlString)
		{
			if (dbConnection.State == ConnectionState.Open)
			{
				DataSet ds = new DataSet();
				try
				{
					MySqlDataAdapter da = new MySqlDataAdapter(sqlString, dbConnection);
					da.Fill(ds);
				}
				catch (Exception ee)
				{
					throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
				}
				finally
				{
				}

				return ds;
			}

			return null;
		}
	}
}
