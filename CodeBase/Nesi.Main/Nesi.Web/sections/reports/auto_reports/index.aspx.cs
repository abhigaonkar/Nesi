using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_auto_reports : Page
{
	public NeMember current_user;

	private const int _page_id = 180; // from Page table in DB
	private const string _page_description = "Auto Reports Settings";
	Toolbox _tools;
	ASPxHiddenField h;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = _page_description;
		hdn_mid.Value = current_user.id.ToString();
		hdn_mtid.Value = current_user.MemberTypeID.ToString();
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		_tools.leave_open_connection = true;
	
	}
	protected void fill_grid()
	{
	

	}
	protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		//try
		//{
			var interval = e.NewValues["interval"].ToString();
			var id = e.Keys[0].ToString();
			var business_unit_id = e.NewValues["business_unit_id"].ToString();
			var auto_reports_id = ASPxGridView1.GetRowValuesByKeyValue(e.Keys[0], "auto_reports_id").ToString();
		

			if (isDuplicate(business_unit_id, hdn_mid.Value, auto_reports_id))
			{
				e.Cancel = true;
				ASPxGridView1.CancelEdit();
				throw new Exception("This will result in a duplicate schedule.. please double check the data you are editing");
			}
			else
			{
				_tools.getSQL_void(@"update auto_reports_schedule  set `interval`=@v0,business_unit_id=@v1   where id =@v2", new object[] { interval,business_unit_id,id });
			}
		/*}
		catch (Exception ee)
		{
			throw new Exception("There is something wrong with the data you are trying to save");
		}*/
	/*	if (_tools.getSQL_int(@"Select count(id) from auto_reports_schedule  where auto_reports_id =@v0 and member_id =@v1 ", new object[] { autoreport_id,mid }); > 0)
		{
			_tools.getSQL_void(@"update auto_reports_schedule  set `interval`=@v0  where auto_reports_id =@v1 and member_id =@v2 ", new object[] { interval,autoreport_id,mid });
		}
		else
		{
			_tools.getSQL_void(@"insert into auto_reports_schedule (auto_reports_id,member_id,`interval`)  values(@v0,@v1,@v2)",new object[] { autoreport_id,mid,interval } );
		}
	*/
		
		e.Cancel = true;
		ASPxGridView1.CancelEdit();
		ASPxGridView1.DataBind();
	}


	protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		try
		{
		var interval = e.NewValues["interval"].ToString();
		var business_unit_id = e.NewValues["business_unit_id"].ToString();
		var auto_reports_id = e.NewValues["auto_reports_id"].ToString();
		var member_id = current_user.id.ToString();
		if (!isDuplicate(business_unit_id, member_id, auto_reports_id))
		{
			_tools.getSQL_void(@"Insert into auto_reports_schedule (business_unit_id, member_id,auto_reports_id,`interval`)  values (@v0,@v1,@v2,@v3)",new object[] { business_unit_id,member_id,auto_reports_id,interval } );
		}

			}
		catch
		{
			throw new Exception("There is something wrong with the data you are trying to Insert, check every value");
		}
		e.Cancel = true;
		ASPxGridView1.CancelEdit();
		
		ASPxGridView1.DataBind();
	}
    /*
     * 
     * Returns true of duplicate returns false if not.
     */
	protected bool isDuplicate(string business_unit_id, string member_id, string report_id)
	{
		if (_tools.getSQL_int(@"Select count(id) from auto_reports_schedule  where business_unit_id =@v0 and member_id =@v1  and auto_reports_id =@v2 ", new object[] { business_unit_id,member_id,report_id }) > 0)
		{
			return true;
		}
		return false;
	}
	protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from auto_reports_schedule  where id =@v0", new object[] { e.Keys[0] });

		e.Cancel = true;
		ASPxGridView1.CancelEdit();
		ASPxGridView1.DataBind();
	}
}
