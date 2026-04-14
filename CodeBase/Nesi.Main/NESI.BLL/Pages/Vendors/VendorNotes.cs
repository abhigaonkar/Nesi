using System;
using System.Data;
//using nesi.bv;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorNotes : VendorEditBase
	{
		public VendorNotes(Employee user, int id) : base(user, id)
		{

		}

		public new object Profile()
		{
			//var main_branch = new NeBusinessUnit(vendor.business_unit_id);
			//var bv_conn = BVDB.connect(main_branch.DSN);
			var notepadflag = "";
			var past_notes = "";
			//try
			//{
			//	notepadflag = BVDB.getSQL_string(bv_conn, @"Select ifnull((SELECT NOTEPAD FROM VENDOR WHERE VEN_NO = ? ),'X')", new object[] { vendor.Number });
			//}
			//catch (Exception)
			//{
			//	// 
			//}
			if (notepadflag == "X")
			{
				//var notes = BVDB.getSQL_dt(bv_conn,
				//	@"SELECT N_DATE, DETAIL, N_TIME FROM NOTES WHERE PROG='SUPP' AND ITEM=?  ORDER BY N_DATE DESC, N_TIME DESC",
				//	new object[] { vendor.Number });
				//foreach (DataRow NoteRow in notes.Rows)
				//{
				//	var strDate = NoteRow["N_DATE"].ToString();
				//	strDate = strDate.Insert(4, "-");
				//	strDate = strDate.Insert(7, "-");
				//	var strTime = NoteRow["N_TIME"].ToString();
				//	var timecount = strTime.Length;
				//	if (timecount == 8)
				//	{
				//		strTime = strTime.Substring(0, 4);
				//		strTime = strTime.Insert(2, ":");
				//	}
				//	else
				//	{
				//		strTime = strTime.Substring(0, 3);
				//		strTime = strTime.Insert(1, ":");
				//	}
				//	past_notes += $"[{strDate} {strTime}]\n{NoteRow["DETAIL"]}\n\n";
				//}
			}
			return new
			{
				entity = new
				{
					data = vendor.Vendor_Notes,
					data2 = ""
				},
				past_notes
			};
		}

		public DataExtra Save(DataString model)
		{
			var current_user = new NeMember(UserId);

			vendor.Vendor_Notes = model.Data;
			vendor.Save();
			//var notesadd = new bv_notes();
			//var nowdate = DateTime.Now;
			//var bvdate = nowdate.ToString("yyyyMMdd");
			//var bvtime = nowdate.ToString("HHmmssFF");
			//notesadd.prog = "SUPP";
			//notesadd.item = vendor.vendor_number;
			//notesadd.subject = vendor.vendor_number;
			//notesadd.n_date = bvdate;
			//notesadd.n_time = Convert.ToInt32(bvtime);
			//notesadd.n_user = current_user.Initials;
			//notesadd.detail = model.Data2.Replace("\"", "").Replace("'", "");

			//if (notesadd.detail.Trim() != "")
			//{
			//	var DSNS = bllToolbox.doSQL_dt(@"SELECT distinct t.dsn FROM tax_entity t inner join business_unit b on t.id=b.tax_entity_id  WHERE t.is_active = 1 AND t.is_test = 0");
			//	foreach (DataRow dsnrow in DSNS.Rows)
			//	{
			//		try
			//		{
			//			var dsn = dsnrow[0].ToString();
			//			notesadd.Save(dsn);
			//		}
			//		catch (Exception)
			//		{
			//			//
			//		}
			//	}
			//}
			return new DataExtra("Vendor Notes has been saved successfully.", Profile());
		}
	}
}