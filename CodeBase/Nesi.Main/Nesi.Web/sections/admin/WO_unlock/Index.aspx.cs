using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_admin_WO_unlock_Index : System.Web.UI.Page
{
	NeMember current_user;
	private const int _page_id = 63; // from Page table in DB
	private const string _page_description = "WO Unlock";

	protected void Page_Load(object sender, EventArgs e)
	{
		#region Variable Declaration
		var _tools = new Toolbox();
		var _q = Request.QueryString;
		current_user = Toolbox.do_handle_authentication(_page_id);
		var current_branch = new NeBusinessUnit(current_user.business_unit_id);
		var DSN = current_branch.DSN;
		var _response = "";
		var _locks = "";
		#endregion Variable Declaration
		#region DSN 
		if (_q["DSN"] != null)
		{
			DSN = _q["DSN"];
		}
		current_DSN.InnerHtml = "Current DSN:" + DSN;
		#endregion DSN
		//#region Lock Remover
		//if (_q["ID"] != null)
		//{
		//	var wo_id = _q["ID"];
		//	try
		//	{
		//		_tools.getSQL_void(@"DELETE from bvlock  WHERE BVLOCKFILEID =?", DSN, new object[] { wo_id });
		//		_response = "<div>Requested Lock Removed</div>";
		//	}
		//	catch
		//	{
		//		_response = "<div>Couldn't remove requested lock</div>";
		//	}
		//	server_response.InnerHtml = _response;
		//}
		//#endregion Lock Remover
		#region Branches Select Box

		var branches = _tools.getSQL_datatable("Select * from tax_entity order by ddl_name", null);
		var branches_select = @"<select onchange=""location.href='./index.aspx?DSN='+this.value"">";
		foreach (DataRow _dr in branches.Rows)
		{
			var this_dsn = _dr["dsn"].ToString();
			var this_name = _dr["ddl_name"].ToString();
			var this_selected = "";
			if (this_dsn == DSN)
			{
				this_selected = " SELECTED";
			}
			else
			{
				this_selected = "";
			}
			branches_select += string.Format("<option value='{1}'{0}>{2}</option>", this_selected, this_dsn, this_name);
		}
		branches_select += "</select>";
		company_selection.InnerHtml = branches_select;
		#endregion Branches Select Box
	//	#region Locks
	//	try
	//	{
	//		var current_locks = _tools.getSQL_datatable(@"SELECT BVLOCKFILEID, BVLOCKFILEKEY, LOCK_USER_ID FROM BVLOCK", DSN, null);
	//		foreach (DataRow _lock in current_locks.Rows)
	//		{
	//			var _id = _lock["BVLOCKFILEID"].ToString();
	//			var _invoice = _lock["BVLOCKFILEKEY"].ToString();
	//			var _user = _lock["LOCK_USER_ID"].ToString();
	//			_locks += string.Format(@"
	//<div>
	//	<button data-DSN='{3}' data-LOCKID='{2}' onclick='remove_lock(this)'>Remove Lock <b>{1}</b> put on Invoice # <b>{0}</b></button>
	//</div>", _invoice, _user, _id, DSN);
	//		}
	//	}
	//	catch (Exception ee)
	//	{
	//		_locks = ee.ToString();
	//	}
	//	locks.InnerHtml = _locks;
	//	#endregion Locks
	}
}
