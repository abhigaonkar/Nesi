using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_modules_fvr_listing : System.Web.UI.UserControl
	{
    public int _id {get;set;}
	public int _comp_id {get;set;}
	public NeMember user {get;set;}
	public int mid { get; set; }
	protected void Page_Load(object sender, EventArgs e)
		{
			HiddenField1.Value = mid.ToString();
			gv_past_fvrs.DataBind();
		}
	protected void gv_past_fvrs_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		var gv		= (ASPxGridView) sender;
		var type			= gv.GetDataRow(e.VisibleIndex)["type"].ToString();
		var cat_id			= Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["tab_index"]);
		switch(e.DataColumn.Index)
			{
			case 1:
				// Type
				switch(type)
					{
					case "NEWHIRE":
						e.Cell.Text			= "New Hire";
					break;
					case "RENEW":
						e.Cell.Text			= "Renewal";
					break;
					case "EXIT":
						e.Cell.Text			= "Exit Interview";
					break;
					}
			break;
			case 2:
				e.Cell.Text		= Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab WHERE id = @v0", cat_id);
			break;
			}
		//string type			= gv.GetDataRow(e.VisibleIndex)["confirmed"].ToString();
		}
		//string type			= gv.GetDataRow(e.VisibleIndex)["confirmed"].ToString();
	
	
	}