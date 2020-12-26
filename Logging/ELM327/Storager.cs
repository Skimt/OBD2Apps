using Logging.ELM327.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Logging.ELM327
{
	public class Storager
	{

		private const int LOCAL_STORAGE_LIMIT = 20;

		/// <summary>
		/// The total amount of stored successions. 
		/// </summary>
		public int DataCount = 0;

		private readonly DataManager Manager;
		private readonly DataTable Storage;
		private readonly string ConnectionString;
		private readonly int CarId = 1; // TODO: WHEN LAUNCHING SET CAR ID. 

		public Storager(DataManager manager)
		{

			Manager = manager;
			ConnectionString = ConfigurationManager.ConnectionStrings["OBDConnectionString"].ConnectionString;
			Storage = new DataTable("LogEntry");

			Storage.Columns.AddRange(new DataColumn[] {
				new DataColumn("Date", typeof(DateTime)),
				new DataColumn("PID", typeof(string)),
				new DataColumn("Value", typeof(double)),
				new DataColumn("CarId", typeof(int))
			});

		}

		/// <summary>
		/// Returns true when the rows in storage have reached a specific limit. 
		/// See CONST LOCAL_STORAGE_LIMIT. 
		/// </summary>
		private bool IsStorageFull => Storage.Rows.Count > LOCAL_STORAGE_LIMIT;

		/// <summary>
		/// Save the string to local memory
		/// </summary>
		public void Save(string response)
		{

			SaveToStorage(response);

			// Apply to database when the storage gets full. 
			if (IsStorageFull)
			{
				PostStorageToDb();
				Storage.Rows.Clear();
			}

		}

		/// <summary>
		/// Iterates through the different PID's in the given response, and convert before storing locally. 
		/// </summary>
		private void SaveToStorage(string response)
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
				Storage.Rows.Add(new object[] 
				{
					DateTime.Now,
					pid,
					command.Value,
					CarId
				});

				// Count the number of rows that were stored. 
				DataCount++;

				// Shift the iteration forward to where the next PID should be located.
				i += command.BytesCount;

			}

		}

		/// <summary>
		/// Copies bulk (storage) to database. 
		/// </summary>
		private void PostStorageToDb()
		{

			try
			{

				using (SqlConnection connection = new SqlConnection(ConnectionString))
				{

					if (connection.State != ConnectionState.Open)
						connection.Open();

					using (SqlBulkCopy copy = new SqlBulkCopy(connection))
					{

						// Set table to which the copy should be copied.  
						copy.DestinationTableName = Storage.TableName;

						// Set the columns to which the copy should be copied. 
						foreach (object column in Storage.Columns)
						{
							copy.ColumnMappings.Add(column.ToString(), column.ToString());
						}

						// Post copy to DB. 
						copy.WriteToServer(Storage);

					}

				}


			}
			catch (Exception e)
			{
				throw e;
			}

		}

	}
}