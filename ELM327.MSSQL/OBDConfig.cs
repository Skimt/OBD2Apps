namespace ELM327.MSSQL
{
	public class OBDConfig
	{
		public int ConfigurationId { get; set; }
		public int CarId { get; set; }
		public bool IsLoggingToDb { get; set; }
	}
}