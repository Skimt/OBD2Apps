using ELM327.Commands;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ELM327
{
	internal class Storager
	{

		private const int LOCAL_STORAGE_LIMIT = 40;

		private readonly Manager Manager;
		private readonly DataTable Data;
		private readonly string ConnectionString;
		private bool _init;

		internal Storager(Manager manager)
		{

			Manager = manager;
			//ConnectionString = config.ConnectionString;

			Data = new DataTable("LogEntry");

			Data.Columns.AddRange(new DataColumn[] {
				new DataColumn("Date", typeof(DateTime)),
				new DataColumn("PID", typeof(string)),
				new DataColumn("Value", typeof(double)),
				new DataColumn("SessionId", typeof(int))
			});

		}

		internal int CarId { get; set; } = 1;
		private int SessionId { get; set; }

		/// <summary>
		/// Returns true when the rows in storage have reached a specific limit. 
		/// <para>Uses <see cref="LOCAL_STORAGE_LIMIT"/></para> 
		/// </summary>
		private bool IsStorageFull => Data.Rows.Count > LOCAL_STORAGE_LIMIT;

		/// <summary>
		/// Save the string to local memory
		/// </summary>
		internal void Save(string response)
		{

			//// Setup session. 
			//if (!_init && Manager.RequestType == RequestType.DataStream) 
			//{
			//	SessionId = EstablishSession();
			//	_init = true;
			//}

			SaveToLocalStorage(response);

			//// Apply to database when the storage gets full. 
			//if (IsStorageFull)
			//{
			//	PostStorageToDb();
			//	Data.Rows.Clear();
			//}

		}

		//private int EstablishSession()
		//{

		//	object sessionId;

		//	using (SqlConnection connection = new SqlConnection(ConnectionString))
		//	{

		//		if (connection.State != ConnectionState.Open)
		//			connection.Open();

		//		using (SqlCommand command = new SqlCommand("spEstablishSession", connection))
		//		{

		//			command.CommandType = CommandType.StoredProcedure;
		//			command.Parameters.AddWithValue("@CarId", CarId);

		//			try
		//			{
		//				sessionId = command.ExecuteScalar();
		//			}
		//			catch (Exception e)
		//			{
		//				throw e;
		//			}

		//		}

		//	}

		//	return Convert.ToInt32(sessionId);

		//}

		/// <summary>
		/// Iterates through the different PID's in the given response, and convert before storing locally. 
		/// </summary>
		private void SaveToLocalStorage(string response)
		{

			for (int i = 0; i < response.Length / 2; i++)
			{

				int pidStartIndex = i * 2;

				string pid = response.Substring(pidStartIndex, 2);
				Command command = Manager.GetCommand(pid);

				// Get the hexadecimal values that follow the PID.
				string hexValues = response.Substring(pidStartIndex + 2, 2 * command.BytesCount);

				// Wrong byte size. 
				if (command.BytesCount != hexValues.Length / 2)
				{
					throw new Exception(string.Format("Expected {0} bytes for the PID '{1}', but found {2} bytes in the response.", command.BytesCount, pid, hexValues.Length / 2));
				}

				// Convert hexadecimals to bytes. 
				command.ConvertToBytes(hexValues);

				// Add to storage. 
				Data.Rows.Add(new object[] 
				{
					DateTime.Now,
					pid,
					command.Value,
					SessionId
				});

				// Count the number of rows that were stored. 
				Manager.StoredCount++;

				// Shift the iteration forward to where the next PID should be located.
				i += command.BytesCount;

			}

		}

		///// <summary>
		///// Copies bulk to database. 
		///// </summary>
		//private void PostStorageToDb()
		//{

		//	try
		//	{

		//		using (SqlConnection connection = new SqlConnection(ConnectionString))
		//		{

		//			if (connection.State != ConnectionState.Open)
		//				connection.Open();

		//			using (SqlBulkCopy copy = new SqlBulkCopy(connection))
		//			{

		//				// Set table to which the copy should be copied.  
		//				copy.DestinationTableName = Data.TableName;

		//				// Set the columns to which the copy should be copied. 
		//				foreach (object column in Data.Columns)
		//				{
		//					copy.ColumnMappings.Add(column.ToString(), column.ToString());
		//				}

		//				// Post copy to DB. 
		//				copy.WriteToServer(Data);

		//			}

		//		}

		//	}
		//	catch (Exception e)
		//	{
		//		throw e;
		//	}

		//}

	}
}