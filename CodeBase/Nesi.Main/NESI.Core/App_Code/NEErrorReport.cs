namespace nesi.core
	{
	/// <summary>
	/// Summary description for ErrorReport
	/// </summary>
	public class NEErrorReport
		{
		public void WriteErrorReport(string MemberID, string dsn, string error_message)
			{
			Toolbox.doSQL_void(@"INSERT INTO error
(error_member_id, error_dsn, error_desc, error_datetime) 
VALUES (@v0,@v1,@v2, NOW())", new object[] { MemberID, dsn, error_message});
			}
		}
	}