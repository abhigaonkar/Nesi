using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_customer_rates_index : Page
{
	Toolbox _tools;
	NeMember current_user;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static int _page_id = 125;
	static string _page_name = "CustomerRates";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = _page_name;
		if (Session["working_business_unit_id"] != null)
		{
			Session.Remove("working_business_unit_id");
		}
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv_customer_rates;
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsCallback && !IsPostBack)
		{
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			h.Set("gridview_id", "gv_customer_rates");
			if (gl.GridLayoutID == 0)
			{
				gv_customer_rates.FilterExpression = string.Format("[business_unit] = '{0}' ", current_user.business_unit.ddl_name);

				gl.GridLayout_Layout = gv_customer_rates.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_customer_rates.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;

			//		DataTable dt = _tools.getSQL_datatable(@"Select * from customer_rates"  , null);

			//		gv_customer_rates.DataSource = dt;

			//		Session["gv_customer_rates"] = dt;
		}
		else
		{
			//		gv_customer_rates.DataSource = Session["gv_customer_rates"];
		}
		gv_customer_rates.DataSource = Toolbox.doSQL_dt(string.Format(@"SELECT customer.customer_id, 
d.ddl_name business_unit, customer.customer_name customer_name, membertype_chargeout.membertype_id,
membertype_chargeout.business_unit_id, customer_rate.chargeout, membertype_chargeout.Chargeout AS normal, 
customer_rate.last_updated, customer_rate.from_date, customer_rate.to_date FROM customer_rate 
INNER JOIN customer ON customer_rate.customer_id = customer.Customer_ID 
INNER JOIN membertype_chargeout ON customer_rate.base_chargeout_id = membertype_chargeout.id 
LEFT JOIN business_unit d ON d.id = `membertype_chargeout`.`business_unit_id`
WHERE membertype_chargeout.paytype_id = 1 AND membertype_id NOT IN (10,11,14,15,16,3) AND d.id IN ({0}) ORDER BY customer_name, chargeout", new Current_User().visible_business_units), null);


		gv_customer_rates.DataBind();
	}

	protected void gv_customer_rates_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}

	protected void gv_customer_rates_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
	{
		try
		{
			var dt = (DateTime)gv_customer_rates.GetRowValues(e.VisibleIndex, "to_date");
			if (System.DateTime.Today.Date > dt)
			{
				e.Row.BackColor = System.Drawing.Color.Red;

			}
		}
		catch { }

	}
	protected void gv_customer_rates_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "chargeout")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(gv_customer_rates.GetRowValues(e.VisibleIndex, "normal")))
			{
				e.Cell.BackColor = System.Drawing.Color.LightPink;
			}
			else if (Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(gv_customer_rates.GetRowValues(e.VisibleIndex, "normal")))
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
			}
		}
	}


    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
