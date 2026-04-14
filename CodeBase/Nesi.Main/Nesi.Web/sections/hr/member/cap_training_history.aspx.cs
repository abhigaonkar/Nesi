using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_cap_training_history : System.Web.UI.Page
{
	

	NeMember current_user;
	private int page_id = 177;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	string _page_name = "cap_training_history";

	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);

		LayoutControl1.__page_name = _page_name;
		LayoutControl1.used_gv		= gv;
		h = (ASPxHiddenField)LayoutControl1.FindControl("h");
		ds_templates = (SqlDataSource)LayoutControl1.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)LayoutControl1.FindControl("dde_filter");
		panel_export = (Panel)LayoutControl1.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	    Session["visibleBU"] = new Current_User().visible_business_units;

    }
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		if (!IsPostBack)
		{
			Session["gvcap_training_history"] = null;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gv.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
		
	

	}
	
	

	
	

	protected void ASPxButton2_Init(object sender, EventArgs e)
	{
		var b = (ASPxButton)sender;
		var container = b.NamingContainer as GridViewDataItemTemplateContainer;
		var historyid = container.Grid.GetRowValues(container.VisibleIndex, "id").ToString();

		//		b.ClientSideEvents.Click = String.Format("function (s, e) {{ gv.PerformCallback('xcs|{0}|' + s.GetText()); }}", cert_historyid);
		b.ClientVisible = (Convert.ToDateTime(gv.GetRowValues(container.VisibleIndex, "Training Date")) < System.DateTime.Today);
		b.ClientSideEvents.Click = string.Format("function (s, e) {{ gv.PerformCallback('issue|{0}|' + s.GetText()); }}", historyid);
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;

		if (e.Parameters != "")
		{

			if (e.Parameters.Contains("issue"))
			{
				var thistory = new NeCapTraining_History(Convert.ToInt32(e.Parameters.Split('|').GetValue(1)));
				NeCapTraining.issue_certs(thistory.id);
				gv.JSProperties["cp_alert"] = "Certifications Issued";
				gv.DataBind();
			}
			else
			{
				gv.LoadClientLayout(e.Parameters);
			}
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
	protected void gv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from training_header_history where id = @v0", new object[] {
			e.Keys[0]});
		e.Cancel = true;
		gv.CancelEdit();
	}
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		

	}
	protected void gv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
if (Convert.ToDateTime(gv.GetRowValues(e.VisibleIndex, "Training Date")) > System.DateTime.Today)
		{
			e.Visible = true;
			e.Text = "Cancel";
		}
		else
		{
			e.Visible = false;
		}
	}
}
	

	