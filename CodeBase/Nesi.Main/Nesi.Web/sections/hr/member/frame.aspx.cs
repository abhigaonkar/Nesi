using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using nesi.core;

public partial class member_frame : Page
{
	NeMember current_user;
	private const int _page_id = 127; // from Page table in DB
	private bool auth_for_edit = false;
	private bool auth_for_edit_all = false;
	private bool is_dept_hr = false;
	private bool is_branch_hr = false;
	private bool is_branch_daysoff = false;
    private bool can_see_wage = false;
    private bool isowner = false;
    private const string _page_name = "Employees";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	private Toolbox _tools = new Toolbox();
	List<int> reports_to_list;

	protected void Page_Init(object sender, EventArgs e)
	{
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		layout.__page_name = _page_name;
		layout.used_gv = gv_members;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		var temp_list = Toolbox.doSQL_string(@"SELECT IFNULL(reports_to_all(@v0 ), '')", new object[] { current_user.id });
		reports_to_list = temp_list.Contains(",")
												? temp_list.Split(',').Select(int.Parse).ToList()
												: temp_list != ""
													? new List<int> { Convert.ToInt32(temp_list) }
													: new List<int>();

		//		frame.Attributes["src"]			= String.Format("./index.aspx");
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		if (!IsPostBack)
		{
			//	gv_members.FilterExpression = "[loginstatus] = 'Active' and [branch] = " + business_unit_id;

		}


		// Hide the action column
		gv_members.Settings.ShowTitlePanel = current_user.AuthenticatedForPrivilege(5);
		auth_for_edit = current_user.AuthenticatedForPrivilege(32);
		is_branch_hr = current_user.AuthenticatedForPrivilege(33);
		is_dept_hr = current_user.AuthenticatedForPrivilege(35);
		btnMemberAccess.Visible = current_user.AuthenticatedForPrivilege(36);
		auth_for_edit_all = current_user.AuthenticatedForPrivilege(6);
		is_branch_daysoff = current_user.AuthenticatedForPrivilege(166);
	    can_see_wage = current_user.AuthenticatedForPrivilege(101);

        sqlcomp.SelectCommand = "Select id business_unit_id, ddl_name name from business_unit where id in (" +
								new Current_User().visible_business_units + ") order by ddl_name";
		sqlcomp.DataBind();

		h.Set("gridview_id", "gv_members");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		if (!IsPostBack)
		{
			Session["HR_employee_grid"] = null;
		}
		fill_grid();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		sqlcomp.DataBind();
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gv_members.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_members.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;

		}

	}
	protected void fill_grid()
	{
		if (Session["HR_employee_grid"] == null)
		{
	//		var dt_last_mobile = _tools.getSQL_datatable(@"SELECT max(dt) dt,member_id FROM log_page_asax  WHERE dt > '2014-01-01' AND log_page_asax.url = '/mobile/index.aspx' GROUP BY log_page_asax.member_id", null);


			var dt = _tools.getSQL_datatable(string.Format(@" 
CALL sp_employee_grid({0})", current_user.id), null);

			//ne_session.snap_collection(_page_id, "HR_employee_grid", ne_session.obj_size(dt), dt.Rows.Count, dt.Columns.Count);
//			foreach (DataRow dr in dt.Rows)
//			{

		//		dr.BeginEdit();

//				if (dt_last_mobile.Select("member_id=" + dr["memberid"]).Length > 0)
//				{
//					dr["last_mobile_login"] = Convert.ToDateTime(dt_last_mobile.Select("member_id = " + dr["memberid"])[0][0]).ToString("yyyy-MM-dd HH:mm:ss");
//				}
//				else
//				{
//					dr["last_mobile_login"] = "Unknown";
//				}
//				dr.AcceptChanges();
//			}

			Session["HR_employee_grid"] = dt;

		}

		gv_members.DataSource = Session["HR_employee_grid"];
		gv_members.DataBind();




	}

	protected void gv_members_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{

		var frame = (HtmlContainerControl)gv_members.FindEditFormTemplateControl("editframe");
		if (!gv_members.IsNewRowEditing)
		{
			var rowIndex = gv_members.EditingRowVisibleIndex;
			var value1 = gv_members.GetRowValues(rowIndex, new string[] { "memberid" });
			if (value1 != null)
			{
				frame.Attributes.Add("src", "index.aspx?id=" + value1);
			}
			else
			{
				Session["HR_employee_grid"] = null;
				fill_grid();
			}

		}
		else
		{

			frame.Attributes.Add("src", "index.aspx?id=0");
		}

	//	frame.Attributes.Add("onload", "resizeIframe(this);");
	}
	protected void gv_members_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
	{
		if (e.ButtonID == "bc")
		{
			var memid = Convert.ToInt32(gv_members.GetRowValues(e.VisibleIndex, "memberid"));
			try
			{
				NeMember _member;
				_member = new NeMember(memid);
				_member.print_barcode_label(1);
			}
			catch
			{
			}
		}
	}

	protected void gv_members_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters.Contains("ss|"))
		{
			_tools.getSQL_void(@"update member  set include_in_mobile_contactlist =@v0  where member_id =@v1 limit 1 ", new object[] { e.Parameters.Split('|').GetValue(2), e.Parameters.Split('|').GetValue(1) });

		}
		else if (e.Parameters == "AddEmp")
		{
			gv_members.AddNewRow();

		}
		else if (e.Parameters == "AddApp")
		{


		}
		else
		{
			var gv = (ASPxGridView)sender;
			if (e.Parameters != "")
			{
				gv.LoadClientLayout(e.Parameters);
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
	}
	protected void gv_members_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		if (!gv_members.IsNewRowEditing)
		{

			gv_members.SettingsText.PopupEditFormCaption = "Editing " + gv_members.GetRowValuesByKeyValue(e.EditingKeyValue, "_name");
		}
	}
	protected void print_bc_Click(object sender, EventArgs e)
	{
		try
		{
		}
		catch
		{

		}
	}
	protected void print_bc_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{
		var button = (ASPxButton)sender;
		var c = (GridViewDataItemTemplateContainer)button.NamingContainer;
		e.Properties.Add("cp_ID", c.KeyValue);
	}
	protected void cb_printbc_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		var _member = new NeMember(Convert.ToInt32(e.Parameter));
		_member.print_barcode_label(1);
		e.Result = "SUCCESS";
	}

	protected void gv_members_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		var gv = (ASPxGridView)sender;

		e.Visible = false;
		var kv = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "memberid"));
		if (kv > 0)
		{
			var business_unit_id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "branch"));

            //			if (!auth_for_edit_all)
            //				{
            //				if (!auth_for_edit)
            //					{
            //					e.Visible = false;
            //					}
            //				else
            //					{
            //					if (auth_for_edit && business_unit_id != current_user.business_unit_id)
            //						{
            //						e.Visible = false;
            //						}
            //					}
            //				}
            if ((is_branch_hr || auth_for_edit) && (current_user.business_unit_id == business_unit_id))
			{
				e.Visible = true;
			}

			if (auth_for_edit_all)
			{
				e.Visible = true;
			}
			if (reports_to_list.Contains(Convert.ToInt32(kv)))
			{
				e.Visible = true;
			}
		}
	}
	protected void gv_members_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void cb_apps_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["HR_employee_grid"] = null;
	}
	public static string FormatPhone(string num)
	{
		//first we must remove all non numeric characters
		num = num.Replace("(", "").Replace(")", "").Replace("-", "");
		var results = string.Empty;
		var formatPattern = @"(\d{3})(\d{3})(\d{4})";
		results = Regex.Replace(num, formatPattern, "($1) $2-$3");
		//now return the formatted phone number
		return results;
	}

	protected void ASPxLabel1_PreRender(object sender, EventArgs e)
	{
		var l = (ASPxLabel)sender;
		l.Text = FormatPhone(l.Text);
	}
	protected void gv_members_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		Session["HR_employee_grid"] = null;
		fill_grid();
	}
	protected void gv_members_HtmlDataCellPrepared1(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			switch (e.DataColumn.FieldName)
			{
				case "day_off":
				case "gen_score":
				case "cr_score":
				case "last_agreement":
			
				case "morale":
				case "wage":
				case "address":

				    e.Cell.Text = "";
                    var member_id = Convert.ToInt32(gv_members.GetRowValues(e.VisibleIndex, "memberid"));
					var business_unit_id = Convert.ToInt32(gv_members.GetRowValues(e.VisibleIndex, "branch"));
				    var te_id = Convert.ToInt32(gv_members.GetRowValues(e.VisibleIndex, "te_id"));
                    var reports_to = reports_to_list.Contains(member_id);
				    isowner = NeMember.is_owner(current_user.id, te_id);
                    switch (e.DataColumn.FieldName)
					{
						case "day_off":
							if (!reports_to && !auth_for_edit_all && !isowner && (!is_branch_daysoff || (is_branch_daysoff && business_unit_id != current_user.business_unit_id)))
							{
								e.Cell.Text = "";
							}
							break;

						case "gen_score":
						case "cr_score":
						case "member_payroll_id":
						case "last_agreement":
						case "morale":
                        case "address":
						
                            if (isowner|| reports_to || (auth_for_edit_all && auth_for_edit))
                                {
								var type = e.CellValue.GetType();
								if(e.DataColumn.Name == "last_agreement" && e.CellValue != DBNull.Value)
									{
									e.Cell.Text = Toolbox.MySQL_shortdt((DateTime) e.CellValue);
									}
								else
									{
									e.Cell.Text = e.CellValue.ToString();
									}
                                }
							break;
					        case "wage":
					            if (((isowner || reports_to || (auth_for_edit_all && auth_for_edit))&&can_see_wage))
					            {
					            e.Cell.Text = e.CellValue.ToString();
					            }
                            break;
                    }

					break;
			}

		}
	}

	protected void ASPxHyperLink1_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var c = (GridViewDataItemTemplateContainer)hl.NamingContainer;
	    if (c.Column.FieldName == "mo_status")
	        {
	        if (hl.Text == "In Development")
	            {

	            hl.NavigateUrl = string.Format("javascript:boing('member_offer.aspx?id={0}','mo',1100,800);",
	                gv_members.GetRowValuesByKeyValue(c.KeyValue, "mo_id"));
	            }
	        else
	            {
	            hl.NavigateUrl = string.Format("javascript: boing('offer_print_off.aspx?moid={0}', 'mo', 800, 900);",
	                gv_members.GetRowValuesByKeyValue(c.KeyValue, "mo_id"));

	            }

	        }

        if (hl.Text.Equals("0"))
		{
			hl.NavigateUrl = "";
			hl.Text = "";
		}
		else if (!auth_for_edit)
		{
			hl.NavigateUrl = "";
			hl.Text = "";
		}
		else if ((!auth_for_edit_all) && (!auth_for_edit))
		{
			if (hl.Text == "In Development")
			{

				var mo_id = gv_members.GetRowValuesByKeyValue(c.KeyValue, "mo_id").ToString();
                
				if (_tools.getSQL_string(@"Select reports_to from member_offers  where id =@v0", new object[] { mo_id }) == current_user.id.ToString())
				{
				    

                }
				else
				{
					hl.NavigateUrl = "";
					hl.Text = "";
				}
			}
			else
			{

				hl.NavigateUrl = "";
				hl.Text = "";
			}
		}
		else
		{
			var kv = Convert.ToInt32(gv_members.GetRowValuesByKeyValue(c.KeyValue, "memberid"));
            var te_id = Convert.ToInt32(gv_members.GetRowValuesByKeyValue(c.KeyValue, "te_id"));
		    bool is_owner = NeMember.is_owner(current_user.id32, te_id);
            if (kv > 0)
			{
				var business_unit_id = Convert.ToInt32(gv_members.GetRowValuesByKeyValue(c.KeyValue, "branch"));


				if ((!auth_for_edit_all) && (!is_owner))
				{
					hl.NavigateUrl = "";
					hl.Text = "";
				}


				if (!auth_for_edit_all && !is_owner && !reports_to_list.Contains(Convert.ToInt32(kv)))
				{
					hl.NavigateUrl = "";
					hl.Text = "";
				}

			}
		}

	}
	protected void ASPxCheckBox1_Init(object sender, EventArgs e)
	{
		var button = (ASPxCheckBox)sender;
		var c = (GridViewDataItemTemplateContainer)button.NamingContainer;
		button.ClientSideEvents.CheckedChanged = string.Format("function(s,e){{gv_members.PerformCallback('ss|{0}|' + s.GetValue());}}", c.KeyValue);

		if (gv_members.GetRowValuesByKeyValue(c.KeyValue, "_in_mobile_contact_list").ToString() == "1")
		{
			button.Checked = true;
		}
		else
		{
			button.Checked = false;
		}
	}


   
    
}