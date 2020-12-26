using System;
using System.Data;
using System.Data.SqlClient;

namespace ELM327.MSSQL
{
	public class OBDSql
	{

		private readonly SqlConnection sqlConnection;

		public OBDSql(string connectionString)
		{
			sqlConnection = new SqlConnection(connectionString);
			sqlConnection.Open();
		}

		/// <summary>
		/// Default threshold is 40. 
		/// </summary>
		public int StorageThreshold { get; set; } = 40;

		/// <summary>
		/// Total stored successions in the application's lifetime. 
		/// </summary>
		public int StoreTotal { get; internal set; }

		/// <summary>
		/// Save the string to local memory
		/// </summary>
		public void Insert(DataTable data)
		{

			try
			{

				if (sqlConnection.State != ConnectionState.Open)
					sqlConnection.Open();

				using (SqlBulkCopy copy = new SqlBulkCopy(sqlConnection))
				{

					// Set table to which the copy should be copied.  
					copy.DestinationTableName = data.TableName;

					// Set the columns to which the copy should be copied. 
					foreach (object column in data.Columns)
					{
						copy.ColumnMappings.Add(column.ToString(), column.ToString());
					}

					// Post copy to DB. 
					copy.WriteToServer(data);

					// Count. 
					StoreTotal += data.Rows.Count;

				}

			}
			catch (Exception e)
			{
				throw e;
			}

			data.Rows.Clear();

		}

		/// <summary>
		/// Returns a new session id. 
		/// </summary>
		public int GetSession(int carId)
		{

			object sessionId;

			try
			{

				if (sqlConnection.State != ConnectionState.Open)
					sqlConnection.Open();

				using (SqlCommand command = new SqlCommand("spEstablishSession", sqlConnection))
				{

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CarId", carId);
					sessionId = command.ExecuteScalar();

				}

			}
			catch (Exception e)
			{
				throw e;
			}

			return Convert.ToInt32(sessionId);

		}

		/// <summary>
		/// Returns the Logging application's configuration. 
		/// </summary>
		public OBDConfig GetConfiguration()
		{

			OBDConfig configuration = new OBDConfig();

			try
			{

				if (sqlConnection.State != ConnectionState.Open)
					sqlConnection.Open();

				using (SqlCommand command = new SqlCommand("SELECT TOP 1 CarId, IsLoggingToDb FROM Configuration", sqlConnection))
				{

					using (SqlDataReader reader = command.ExecuteReader())
					{
						
						if (!reader.Read())
						{
							throw new Exception("Evil Microsoft SQL doesn't let me read the configuration table in the OBD database and I don't know why.");
						}

						configuration = new OBDConfig()
						{
							CarId = Convert.ToInt32(reader["CarId"]),
							IsLoggingToDb = Convert.ToBoolean(reader["IsLoggingToDb"]) //GetBoolean(reader["IsLoggingToDb"])
						};

					}

				}

			}
			catch (Exception e)
			{
				throw e;
			}

			return configuration;

		}

		//private bool GetBoolean(object column)
		//{
		//	return Convert.ToInt32(column) == 1 ? true : false;
		//}

	}
}
