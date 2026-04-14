using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using nesi.core;



public partial class sections_hr_member_review_history : System.Web.UI.Page
	{
	NeMember current_user;
	NeEmpReview review;
	Toolbox _tools;
	ASPxDropDownEdit dde_filter;
	private const string _page_name = "Reviews";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	Panel panel_export;
	bool can_add_review		= false;
	bool can_see_all		= false;
	NameValueCollection _q;
    DataTable options_dt;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(1);  // if the user has access to the member page, they can add a review
		can_add_review	= current_user.AuthenticatedForPrivilege(128);
		can_see_all		= current_user.AuthenticatedForPrivilege(144);
		_q				= Request.QueryString;
		layout.used_gv	= gv_review;
		
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		pnl_create_review.Visible		= can_add_review;
		lbl_select_user.InnerText		= can_see_all ? "Create a new review for a user:" : lbl_select_user.InnerText;

        combo_select_user.DataSource = can_see_all
											? Toolbox.doSQL_dt(@"SELECT a.member_id, CONCAT(b.name,' - ', a.member_fullname) member_fullname FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id  WHERE a.member_status = 'Active' and a.member_id !=@v0 and find_in_set(b.id,@v1) ORDER BY b.name, a.member_lastname, a.member_nickname", new object[] { current_user.id,new Current_User().visible_business_units })
											: Toolbox.doSQL_dt(@"SELECT a.member_id, CONCAT(b.name,' - ', a.member_fullname) member_fullname FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id  WHERE a.member_status = 'Active' and (scheduled_by =@v0 or reports_to =@v1  )  and find_in_set(b.id,@v3) and a.member_id !=@v2  ORDER BY b.name, a.member_lastname, a.member_nickname", new object[] { current_user.id,current_user.id,current_user.id, new Current_User().visible_business_units });
		combo_select_user.DataBind();
		js_handlers.Visible			= can_add_review;
        fill_grid();
    }
	protected void Page_Load(object sender, EventArgs e)
		{
		layout.Visible = true;
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.__page_name = _page_name;
		var _q = Request.QueryString;
		hdnmemberid.Value = _q["memberid"];
		if (!IsPostBack)
			{
            Session["review_grid"] = null;

            if (!string.IsNullOrEmpty(_q["a"]))
				{
				var action		= _q["a"];
				var is_error		= false;
				var error		= "";
				var c				= 0;
				switch(action)
					{
					case "update_reviewer":
						var review_id		= 0;
						int.TryParse(_q["id"], out review_id);
						var reviewer_id		= 0;
						int.TryParse(_q["rid"], out reviewer_id);
						if(review_id == 0)
							{
							is_error	= true;
							error		= "Invalid review ID";
							}
						if(!is_error && reviewer_id == 0)
							{
							is_error	= true;
							error		= "Invalid Reviewer ID";
							}
						c					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM emp_review WHERE id = @v0", review_id);
						if(!is_error && c > 0)
							{
							review				= new NeEmpReview(Convert.ToInt32(review_id));
							if(review.status == "Delivered")
								{
								is_error	= true;
								error		= "This review has already been delivered";
								}
							if(!is_error && review.member.Status == "Not Active")
								{
								is_error	= true;
								error		= "This user is no longer active in the system";
								}
							if(!is_error && !can_see_all)
								{
								var this_reports_to		= NeMember.get_allreports(current_user.id);
								if(this_reports_to.Select(string.Format("member_id = '{0}'", review.member_id)).Count() == 0)
									{
									is_error	= true;
									error		= "This employee does not report to you.";
									}
								}
							}
						else if(!is_error)
							{
							is_error	= true;
							error		= "This is not a valid review ID";
							}
						if(is_error)
							{
							Toolbox.QuickReponse(Response, error);
							}
						else
							{
							review.reviewed_by_id			= Convert.ToInt32(reviewer_id);
							review.save();
							Toolbox.QuickReponse(Response, "SUCCESS");
							}
					break;
					}
				}
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout = gv_review.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_review.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;
			ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
			ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
			h.Set("gridview_id", "gv_review");
			}
      
           
      
    }

	protected void fill_grid()
		{
        options_dt = _tools.getSQL_datatable(@"
Select distinct member.member_id, CONCAT(b.ddl_name, ' - ', member_fullname) name ,b.id business_unit_id
from member,business_unit b  
where 

b.id = member.business_unit_id 

order by CONCAT(b.ddl_name, ' - ', member_fullname) ", new object[] { current_user.id });

        Sqlmem.SelectCommand = @"Select distinct member.member_id id, CONCAT(b.ddl_name, ' - ', member_fullname) _name ,b.id business_unit_id
from member,business_unit b  
where 
b.id = member.business_unit_id and find_in_set(b.id,'" + new Current_User().visible_business_units + @"')
order by CONCAT(b.ddl_name, ' - ', member_fullname) ";
        Sqlmem.DataBind();

        if (Session["review_grid"] == null)
        {

            var where_clause = "";
            if (!can_see_all)
            {
                if (dt_reports_to == null)
                {
                    dt_reports_to = NeMember.get_allreports(current_user.id);
                }
                var report_list = new List<string>();
                foreach (DataRow dr in dt_reports_to.Rows)
                {
                    report_list.Add(dr["member_id"].ToString());
                }
                if (report_list.Count > 0)
                {
                    where_clause = string.Format(@"
WHERE 
	a.member_id > 0 AND
	(
	(a.member_id = {1} AND a.locked = true) OR
	a.member_id IN ({0}) or 
	(a.member_id = '{1}' and a.locked = true) OR 
	a.reviewed_by_id = '{1}'
	)", string.Join(",", report_list.Select(x => x.ToString()).ToArray()), current_user.id);
                }
                else
                {
                    where_clause = string.Format(@"
WHERE 
	a.member_id > 0 AND
	((a.member_id = '{0}' and a.locked = true) OR a.reviewed_by_id = '{0}') ", current_user.id);
                }
            }
            else
            {
                where_clause = @"
WHERE 
	a.member_id > 0";
            }
            Session["review_grid"] = _tools.getSQL_datatable(string.Format(@"
SELECT
	a.id,
	a.date,
	a.status,
	b.member_fullname AS member_id,
	a.reviewed_by_id,
	a.member_id memberid,
	a.locked,
	IFNULL(f.member_id,0) AS reviewedby,
	c.ddl_name,
	a.offerid,
	d.membertype_name,
	a.was_printed,
	e.reports_to,
	CONCAT(c.ddl_name, ' - ', g.member_fullname) rt_name,
	CONCAT(h.ddl_name, ' - ', f.member_fullname) rb_name,
f.member_id rb_id,
	d.membertype_id,
round(datediff(curdate(),b.member_startdate)/365,0) year,
(SELECT 
round(ifnull(avg(if(emp_review_history.score>0,emp_review_history.score,null)),0),2) y
FROM
emp_review_history
INNER JOIN emp_review_items ON emp_review_history.emp_review_item_id = emp_review_items.id
INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id
WHERE
emp_review_history.emp_review_id = a.id) score,
(SELECT 
round(ifnull(avg(if(emp_review_history.score>0,emp_review_history.score,null)),0),2) x
FROM
emp_review_history
INNER JOIN cr_review ON emp_review_history.cr_review_item_id = cr_review.cr_review_id
INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id
WHERE
emp_review_history.emp_review_id = a.id) _score, 

i.status hrstatus
FROM
	emp_review a
LEFT JOIN 
	member b ON a.member_id = b.Member_ID
LEFT JOIN 
	business_unit c ON b.business_unit_id = c.id
LEFT JOIN 
	membertype d ON a.mt = d.membertype_id
LEFT JOIN 
	member_offers e ON a.offerid = e.id
LEFT JOIN
	member f ON a.reviewed_by_id = f.member_id
LEFT JOIN
	member g ON e.reports_to = g.member_id
LEFT JOIN
	member_hrstatus i ON b.member_hrstatus_id = i.id
LEFT JOIN
	business_unit h ON f.business_unit_id = h.id 
{0}
ORDER BY 
	a.member_id, a.date", where_clause),null);

        }
        gv_review.DataSource = Session["review_grid"];
        gv_review.DataBind();
        }
	

	protected void btnprint0_Click(object sender, EventArgs e)
	{
	//	_tools.getSQL_void(@"update emp_review set locked = 1, was_printed=1  where id =@v0", new object[] { hdnr_id });
	//	chklocked.Checked = true;
	//	chklocked.ClientEnabled = false;
	//	ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('print_review.aspx?rid=" + hdnr_id.Value + "&locked=" + chklocked.Value + "&is_worksheet=0','printrev',1200,900)", true);
	}
	
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}

	protected void gv_review_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var frame = (HtmlContainerControl)gv_review.FindEditFormTemplateControl("if_detail");
		var rowIndex = gv_review.EditingRowVisibleIndex;
		var value1 = gv_review.GetRowValues(rowIndex, new string[] { "id" });
		var value2 = gv_review.GetRowValues(rowIndex, new string[] { "memberid" });
		frame.Attributes.Add("src", "review_list_for_member.aspx?id=" + value1 + "&memberid=" + value2);
	//	frame.Attributes.Add("onload", "resizeIframe(this);");
		
	}
	protected void gv_review_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gv_review.SettingsText.PopupEditFormCaption = "Review Details for " + gv_review.GetRowValuesByKeyValue(e.EditingKeyValue, "member_id");
        Session["review_grid"] = null;
    }

	protected void gv_review_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			
			if (gv_review.GetRowValuesByKeyValue(e.KeyValue, "status").ToString()!="Closed" && gv_review.GetRowValuesByKeyValue(e.KeyValue, "locked") != null && gv_review.GetRowValuesByKeyValue(e.KeyValue, "locked").ToString() == "0")
			{
				e.Row.ForeColor = System.Drawing.Color.Red;
			}
		}
	}
	protected void ASPxButton1_Init(object sender, EventArgs e)
	{
		var b	= (ASPxButton)sender;
		var c			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM emp_review WHERE member_id = @v0 AND locked = FALSE", current_user.id);
		if(c > 0)
			{
			b.ClientSideEvents.Click = @"function(s, e) {print_mreview(" + current_user.id + @");}";			
			}
		else
			{
			b.Visible	= false;
			}

	}
	protected void gv_review_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_review_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	
	
	DataTable dt_reports_to;
	protected void gv_review_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
        if (e.VisibleIndex >= 0 )
        {
            var gv = (ASPxGridView)sender;
            var review_id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["id"]);
            var member_id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["memberid"]);
            var reviewedby_id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["reviewedby"]);
            var member_name = gv.GetDataRow(e.VisibleIndex)["member_id"].ToString();
            var membertype_name = gv.GetDataRow(e.VisibleIndex)["membertype_name"].ToString();
            var offer_name = gv.GetDataRow(e.VisibleIndex)["offerid"].ToString();
            var reports_to = 0;
            int.TryParse(gv.GetDataRow(e.VisibleIndex)["reports_to"].ToString(), out reports_to);
            if (e.DataColumn.FieldName == "rb_name1")
            {
                var cell_value = reviewedby_id;
                var sb = new StringBuilder();
                if (dt_reports_to == null)
                {
                    // Build options based on the user
                    dt_reports_to = !can_see_all
                                            ? NeMember.get_allreports(current_user.id)
                                            : options_dt;
                }
                if (can_see_all || (dt_reports_to.Rows.Count > 0 && dt_reports_to.Select(string.Format("member_id = '{0}'", member_id)).Count() > 0)) // They are in the reports to list, show drop down
                {
                    if (options_dt.Rows.Count > 0)
                    {
                        sb.AppendFormat(@"<select onchange='change_reviewer(this);' style='width:100%;font-weight:bold;font-size:11px;' data-original_id='{0}' data-review_id='{1}'>", reviewedby_id, review_id);
                        string _possible_bu = _tools.getSQL_string("select get_visible_business_units_group_concat(@v0)", new object[] { member_id });
                        foreach (DataRow dr in options_dt.Rows)
                        {
                            var id = Convert.ToInt32(dr["member_id"]);
                            var name = dr["name"];
                            var business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
                            if (!_possible_bu.Contains(business_unit_id.ToString()))
                            {
                                continue;
                            }
                            var selected = id == cell_value ? " SELECTED" : "";
                            sb.AppendFormat(@"<option value='{0}' {1}>{2}", id, selected, name);
                        }
                        sb.Append(@"</select>");
                    }
                }
                else // They aren't the reports to, only show name
                {
                    var drs = options_dt.Select("member_id = " + cell_value);
                    var dr = drs.Count() > 0 ? options_dt.Select("member_id = " + cell_value)[0] : null;
                    if (dr != null)
                    {
                        sb.Append(dr["name"]);
                    }
                }
                e.Cell.Text = sb.ToString();
            }
            else if (e.DataColumn.FieldName == "member_id" && !can_add_review)
            {
                e.Cell.Text = member_name;
            }
            else if (e.DataColumn.FieldName == "membertype_name" && !can_add_review)
            {
                e.Cell.Text = membertype_name;
            }
            else if (e.DataColumn.FieldName == "offerid" && !can_add_review)
            {
                e.Cell.Text = offer_name;
            }
        }
		}
	protected void gv_review_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
		{
        if (e.VisibleIndex >= 0)
        {
            var gv = (ASPxGridView)sender;
            var reviewer_id = 0;
            var dr = gv.GetDataRow(e.VisibleIndex);
            if (dr != null)
            {
                int.TryParse(dr["reviewed_by_id"].ToString(), out reviewer_id);
                if (!can_add_review && e.Cell.Controls.Count > 0 && current_user.id != reviewer_id)
                {
                    e.Cell.Controls[0].Visible = false;
                }
            }
        }
		}
    protected void gv_review_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        if (e.VisibleIndex >= 0)
        {
            if (e.ButtonType == ColumnCommandButtonType.Delete)
            {
                var was_printed = gv_review.GetRowValues(e.VisibleIndex, "was_printed").ToString();
                var status = gv_review.GetRowValues(e.VisibleIndex, "status").ToString();
                if ((was_printed == "1") && (status != "In Development"))
                {
                    e.Visible = false;
                }
            }
        }
    }
	protected void gv_review_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		new NeEmpReview().delete(Convert.ToInt32(e.Keys[0]));
		e.Cancel = true;
		gv_review.CancelEdit();
		gv_review.DataBind();

	}

    

    protected void gv_review_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues[0]!=e.OldValues[0])
        {
      //      _tools.getSQL_void("update emp_review set reviewed_by = " + e.NewValues[0] + " where id = " + e.Keys[0] + " limit 1");

        }
       
        

    }

    protected void gv_review_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        if (e.UpdateValues.Count > 0)
        {
            foreach (var args in e.UpdateValues)
            {
               if (new NeMember(Convert.ToInt32(args.NewValues[0])).Status=="Active")
                {
                    _tools.getSQL_void("update emp_review set reviewed_by_id = @v0 where id = @v1 limit 1", new object[] { args.NewValues[0], args.Keys[0] });
                }
               else
                {
                    throw new Exception("You have selected an employee that is not active");
                }




            }

            Session["review_grid"] = null;
            fill_grid();
        }
        e.Handled = true;

    }

  
}
	

