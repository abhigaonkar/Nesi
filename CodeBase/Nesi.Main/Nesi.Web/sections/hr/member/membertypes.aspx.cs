using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_hr_member_membertypes : System.Web.UI.Page
	{
	NeMember current_user;
	private int page_id = 134;
	bool can_see_wage;
	int memberid = 0;
	int id = 0;
	int comp_id = 0;
	Toolbox _tools;
	protected void Page_Load(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		var _q = Request.QueryString;
		memberid = string.IsNullOrEmpty(_q["memberid"]) || _q["memberid"] == "0" ? 0 : Convert.ToInt32(_q["memberid"]);
		id = string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);
		var user = new NeMember(id);
		comp_id = user.business_unit_id;


		//		frame.Attributes["src"]			= String.Format("./index.aspx");
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));

		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		can_see_wage = current_user.AuthenticatedForPrivilege(101);

		gvtypes.Columns["avg_wage"].Visible = true;
		if (user.business_unit_id != current_user.business_unit_id && user.business_unit_id != 0 && !current_user.AuthenticatedForPrivilege(6))
			{
			Toolbox.FriendlyException(Response, "You can only access users from your branch", "./index.aspx?id=");
			}
		if (!IsPostBack)
			{
			gvtypes.FilterExpression = "[active] = True";
			gvtypes.SortBy(gvtypes.Columns["ratesheet_order"], DevExpress.Data.ColumnSortOrder.Ascending);
			ddlbranch.Value = current_user.business_unit_id;

			}

		}


	protected void gvtypes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		e.Cancel = true;
		gvtypes.CancelEdit();
		gvtypes.DataBind();
		}
	protected void gvtypes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		e.Cancel = true;
		gvtypes.CancelEdit();
		gvtypes.DataBind();
		}
	protected void gvtypes_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
		{
		var frame = (HtmlContainerControl)gvtypes.FindEditFormTemplateControl("IFrame_type");
		if (!gvtypes.IsNewRowEditing)  // are we editting here?
			{
			var rowIndex = gvtypes.EditingRowVisibleIndex;
			var value1 = gvtypes.GetRowValues(rowIndex, new string[] { "membertype_id" });
			frame.Attributes.Add("src", "m_type_detail.aspx?mt_id=" + value1);
			}
		else
			{

			frame.Attributes.Add("src", "m_type_detail.aspx?mt_id=0");
			}
		}
	protected void gvtypes_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		if (e.DataColumn.FieldName == "avg_wage")
			{
			if (can_see_wage)
				{
				int mtid = Convert.ToInt16(gvtypes.GetRowValues(e.VisibleIndex, "membertype_id"));
				e.Cell.Text = _tools.getSQL_double(@"select (ifnull((SELECT avg(m.WAGE) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.business_unit_id =@v0 and m.mt =@v1 group by m.mt), 0)) a", new object[] { ddlbranch.Value,mtid }).ToString("C2");
				}
			}
		}
	protected void gvtypes_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
		gvtypes.DataBind();
		}
	protected void gvtypes_CustomJSProperties1(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gvtypes_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv						= (ASPxGridView) sender;
		if(e.Parameters.Contains("|"))
			{
			var p						= e.Parameters.Split('|');
			var action					= p[0];
			switch(action)
				{
				case "init_sort":
					if(p[1] == "true")
						{
						gv.FilterExpression					= "[active] = True AND [show_on_ratesheet] = TRUE";
						gv.Columns[0].CellStyle.CssClass	+= " hidden";
						gv.Styles.FilterBar.CssClass		+= " hidden";
						gv.Styles.Header.CssClass			+= " hidden";
						gv.Styles.TitlePanel.CssClass		+= " hidden";
						gv.Styles.FilterRow.CssClass		+= " hidden";
						}
					else
						{
						gv.FilterExpression					= "[active] = True";
						}
				break;
				case "sort":
					var this_id							= p[1];
					var prev_id							= p[2];
					Toolbox.doSQL_void(@"CALL REORDER_MEMBERTYPES(@v0,@v1)", new object[] {
						prev_id, this_id});
					gv.DataBind();
					gv.Columns[0].CellStyle.CssClass	+= " hidden";
					gv.Styles.FilterBar.CssClass		+= " hidden";
					gv.Styles.Header.CssClass			+= " hidden";
					gv.Styles.TitlePanel.CssClass		+= " hidden";
					gv.Styles.FilterRow.CssClass		+= " hidden";
				break;
				}

			}
		}
}


