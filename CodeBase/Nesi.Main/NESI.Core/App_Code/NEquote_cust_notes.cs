using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for customer notes on quotes
	/// </summary>
	public class NEquote_cust_notes
		{
		Toolbox _Tools = new Toolbox();

		#region Variable Declaration
		private int _quote_cust_notes_id = 0;
		private int _quote_cust_notes_by = 0;
		private string _quote_cust_notes_notes = "";
		private DateTime _quote_cust_notes_ts;
		private int _quote_cust_notes_quoteid = 0;

		#endregion
   
		#region Get Set Variables
		public int quote_cust_notes_id { get { return _quote_cust_notes_id; } set { _quote_cust_notes_id = value; } }
		public int quote_cust_notes_by { get { return _quote_cust_notes_by; } set { _quote_cust_notes_by = value; } }
		public string quote_cust_notes_notes { get { return _quote_cust_notes_notes; } set { _quote_cust_notes_notes = value; } }
		public DateTime quote_cust_notes_ts { get { return _quote_cust_notes_ts; } set { _quote_cust_notes_ts = value; } }
		public int quote_cust_notes_quoteid { get { return _quote_cust_notes_quoteid; } set { _quote_cust_notes_quoteid = value; } }

   
		#endregion 


		public NEquote_cust_notes()
			{
	
			}


		public NEquote_cust_notes(int quote_id)
			{
			if (quote_id > 0)
				{
				var dt = Toolbox.doSQL_dt(@"Select * from quote_cust_notes  where quote_cust_notes_quoteid = @v0", new object[] { quote_id });
				if (dt.Rows.Count > 0)
					{
					var dr = dt.Rows[0];
					_quote_cust_notes_by = Convert.ToInt32(dr["quote_cust_notes_by"]);
					_quote_cust_notes_id = Convert.ToInt32(dr["quote_cust_notes_id"]);
					_quote_cust_notes_quoteid = Convert.ToInt32(dr["quote_cust_notes_quoteid"]);
					_quote_cust_notes_notes = _Tools.value_from(dr["quote_cust_notes_notes"].ToString());
					_quote_cust_notes_ts = Convert.ToDateTime(dr["quote_cust_notes_ts"]);
					}
				}
			}

		public void Save()
			{
			try
				{
				if (_quote_cust_notes_id >0)  // if its an update
					{
					_Tools.getSQL_void(@"Update quote_cust_notes 
set quote_cust_notes_by = @v0,  
quote_cust_notes_quoteid= @v1  
quote_cust_notes_notes = @v2 
where quote_cust_notes_id = @v3",
new object[]
{
	_quote_cust_notes_by,
	_quote_cust_notes_quoteid,
	_Tools.value_to(_quote_cust_notes_notes),
	_quote_cust_notes_id
}
);
					}
				else
					{
					_Tools.getSQL_void(@"Insert into quote_cust_notes 
(quote_cust_notes_by,quote_cust_notes_quoteid,quote_cust_notes_notes) 
Values (@v0,@v1,@v2)",
new object[] {
_quote_cust_notes_by ,_quote_cust_notes_quoteid ,_Tools.value_to(_quote_cust_notes_notes)});
					}
			
				}
			catch
				{

				}
			}

		}
	}