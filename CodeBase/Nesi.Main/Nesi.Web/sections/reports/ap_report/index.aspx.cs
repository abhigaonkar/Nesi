using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using nesi.core;

public partial class sections_reports_ap_report_index : System.Web.UI.Page
{
   
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 197;
	static string _page_name = "APReport";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	public string woprogid_notes;
    public string DSN;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = _page_name;
		layout.used_gv = Aspxgridview1;
		if (Session["working_business_unit_id"] != null)
		{
			Session.Remove("working_business_unit_id");
		}
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
    
    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Accounts Payable Report";

		if (!IsPostBack && !IsCallback)
       {
			Session["ap_report_gv"] = null;
			Session["ap_report_bv_notes"] = null;
			Session["ap_report_vend"] = "";

       var dtBusinessUnit = Toolbox.doSQL_dt(@"Select distinct tax_entity.id, tax_entity.ddl_name ddl_name, tax_entity.dsn 
from tax_entity 
inner join business_unit b on b.tax_entity_id = tax_entity.id and b.id in (" + new Current_User().visible_business_units + ") order by tax_entity.ddl_name", null);

       ddlCompany.DataSource = dtBusinessUnit;
       var lic = new ListItem("All Tax Entities", "0");
       ddlCompany.Items.Add(lic);
       ddlCompany.DataBind();
       ddlCompany.SelectedValue = current_user.business_unit.tax_entity_id.ToString();

            dteScope.Date = DateTime.Today.AddDays(14);

			if (current_user.AuthenticatedForPrivilege(182))
           {
                ddlCompany.Enabled = true;
           }
         
 
		   var gl = new NeGridLayouts(current_user.id, _page_name);
		   h.Set("gridview_id", "Aspxgridview1");
		   if (gl.GridLayoutID == 0)
		   {
			   Aspxgridview1.FilterExpression = string.Format("");
			   gl.GridLayout_Layout = Aspxgridview1.SaveClientLayout();
			   gl.member_id = current_user.id;
			   gl.GridLayout_Name = "Default";
			   gl.GridLayout_Gridid = _page_name;
			   gl.SaveGridLayout();

			   h.Set("ID", gl.GridLayoutID);
			   h.Set("NAME", gl.GridLayout_Name);
		   }
		   else
		   {
			   Aspxgridview1.LoadClientLayout(gl.GridLayout_Layout);
			   h.Set("ID", gl.GridLayoutID);
			   h.Set("NAME", gl.GridLayout_Name);
		   }
		   dde_filter.Text = gl.GridLayout_Name;
       }
		
		var comp2 = new NeTaxEntity(ddlCompany.SelectedValue);
		DSN = comp2.DSN;
		//populate_grid();

    }

	protected void gv_wo_line_grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_wo_line_grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
  

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comp = new NeTaxEntity(ddlCompany.SelectedValue);
        DSN = comp.DSN;
		Aspxgridview1.CancelEdit();
		Session["ap_report_gv"] = null;
		Session["ap_report_vend"] = "";
		Session["ap_report_bv_notes"] = null;

		
        //populate_grid();
       
    }

//    protected void populate_grid()
//    {
//		var dt = new DataTable();
//		if (Session["ap_report_gv"] == null)
//		{
//			var sql = @"
//SELECT 
//	vend, 
//	name, 
//	credit_line, 
//	SUM(IF(n_days <= 30, balance, 0)) as thirty,  
//	SUM(IF(n_days <= 60 AND n_days > 30, balance, 0)) as sixty,
//	SUM(IF(n_days <= 90 AND n_days > 60, balance, 0)) as ninety,
//	SUM(IF(n_days <= 120 AND n_days > 90, balance, 0)) as onetwenty,
//	SUM(IF(n_days > 120, balance, 0)) as onetwentyplus, 

//	SUM(IF(n_days <= 30, balance, 0))+ 
//	SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+
//	SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+
//	SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+
//	SUM(IF(n_days > 120, balance, 0)) as ebalance,

//	IF(
//		(	(
//			SUM(IF(n_days <= 30,balance,0))+ 
//			SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+ 
//			SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+ 
//			SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+ 
//			SUM(IF(n_days > 120, balance, 0))
//			)-credit_line
//		)<0,
//		0, 
//		(
//			(
//			SUM(IF(n_days <= 30, balance, 0))+ 
//			SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+ 
//			SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+ 
//			SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+ 
//			SUM(IF(n_days > 120, balance, 0 ))
//			)-credit_line
//		)) as overage, 
//	0.00 as pos, 
//	'' as Invoices, 
//	'' vendor_notes,
//	'' as status_ , 
//	'' as vendor_id,
//	0 as avg5,
//'' OnHold 
//FROM (select 
//	AP.vend as vend, 
//	V.NAME as name, 
//	V.CREDIT_LINE as credit_line , 
//	DATEDIFF(day, TO_DATE(APF_DATE), CURDATE())  n_days,
//	balance,
	
//	0.00 as wos, 
//	'' as Invoices, 
//	'' vendor_notes,
//	CODE as status_ , 
//	'' as vendor_id,
//	90.0 as nextexpectedpay,
//	0 as avg5, 
//	'' as arnotes , 
//	0.0 as next14, 
//	'' as note_date ,
//'' OnHold 
//	from AP_TRANSACTIONS as AP, VENDOR AS V 
//	where AP.vend = V.VEN_NO  AND CODE in('I','C','D','S','P') AND Balance<>0 and TO_DATE(APF_DATE)<=CURDATE()
//	) asdf
//GROUP BY vend,name,credit_line";


//			dt = _tools.getSQL_datatable(sql, DSN,null);
			
//				foreach (DataRow dr in dt.Rows)
//				{
//					var venddt = _tools.getSQL_dataset(@"Select vendor_id,vendor_notes,Vendor_hold from vendor where vendor_number = @v0 ", new object[] {  dr["vend"].ToString().Trim() } );
//					var result1 = _tools.getSQL_double(@"Select ifnull(sum(poprog_total_recCost),0) from poprog_header inner join business_unit b on b.id = poprog_header.business_unit_id where poprog_vendor_id = @v0  and poprog_status <> 6 and poprog_status <> 7 and poprog_status <> 8 and poprog_status <> 9 and poprog_status <> 1 and poprog_status is not null and b.tax_entity_id = @v1 ", new object[] {  venddt.Tables[0].Rows[0].ItemArray[0], ddlCompany.SelectedValue } );

//					dr["pos"] = result1;
//					var result3 = Convert.ToDouble(dr["ebalance"]) + Convert.ToDouble(dr["pos"]) - Convert.ToDouble(dr["credit_line"]);
//					if (result3 < 0)
//						result3 = 0;
//					dr["overage"] = result3;
//					dr["vendor_notes"] = _tools.value_from(venddt.Tables[0].Rows[0].ItemArray[1].ToString());
//					dr["vendor_id"] = venddt.Tables[0].Rows[0].ItemArray[0].ToString();
//					dr["OnHold"] = venddt.Tables[0].Rows[0].ItemArray[2].ToString();
//				}
//				Session["ap_report_gv"] = dt;
//		}
//		Aspxgridview1.DataSource = Session["ap_report_gv"];
//        Aspxgridview1.DataBind();
//    }
   
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
   
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {

        exporter.WritePdfToResponse();
    }
    protected void btnExportXLS_Click(object sender, EventArgs e)
    {
        exporter.WriteXlsToResponse();
    }

	protected void Aspxgridview1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void Aspxgridview1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void Aspxgridview1_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		
		pop_invoiceeditform();
		
		
	}
	protected void pop_invoiceeditform()
	{
		var comp2 = new NeBusinessUnit(ddlCompany.SelectedValue);
		DSN = comp2.DSN;
	//	ASPxGridView gv2 = (ASPxGridView)Aspxgridview1.FindEditFormTemplateControl("gv_invoices");

		var cust = Aspxgridview1.GetRowValues(Aspxgridview1.EditingRowVisibleIndex, "vendor_id").ToString();
		if (cust != Session["ap_report_vend"].ToString())
		{
			Session["ap_report_avgage"] = NECustomer.GetAveragePaymentDelay(Convert.ToInt32(cust)).ToString();
		}
		var lbltypical = (ASPxLabel)Aspxgridview1.FindEditFormTemplateControl("lbltypicalPay");
		lbltypical.Text = string.Format("Statistical Days Outstanding for this vendor: {0} days ", Session["ap_report_avgage"]); 

		Session["ap_report_vend"] = cust;
	}


	protected void text_container_Init(object sender, EventArgs e)
	{
		var l = (ASPxLabel)sender;
		l.Attributes.Add("data-tooltip", l.Text.Replace("\"", ""));
		l.CssClass = "ttip";
	}


	protected void Aspxgridview1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{

		var pc = (ASPxPageControl)Aspxgridview1.FindEditFormTemplateControl("pc");
		var mem1 = (ASPxMemo)pc.FindControl("customer_notes");
		var mem2 = (ASPxMemo)pc.FindControl("customer_memo");
		var mem3 = (ASPxMemo)pc.FindControl("customer_salesnotes");
		var mem4 = (ASPxMemo)pc.FindControl("customer_arnotes");
		try
		{
			var onhold = e.NewValues["OnHold"].ToString() == "T" ? "T" : "F";
			var creditdays = e.NewValues["creditdays"] == null ? 30 : Convert.ToInt32(e.NewValues["creditdays"]);
			var creditlimit = e.NewValues["credit_limit"] == null ? 0 : Convert.ToInt32(e.NewValues["credit_limit"]);
			var customer_notes = mem1.Text == null ? "" : mem1.Text;
			var customer_memo = mem2.Text == null ? "" : mem2.Text;
			var customer_salesnotes = mem3.Text == null ? "" : mem3.Text;
			var customer_arnotes = mem4.Text == null ? "" : mem4.Text;
			var bv_cust_notes = e.NewValues["bv_cust_notes"] == null ? "" : e.NewValues["bv_cust_notes"].ToString();
			var status = e.NewValues["status_"] == null ? 0 : Convert.ToInt32(e.NewValues["status_"]);

			var customer_whyhold = e.NewValues["cust_whyhold"] == null ? "" : e.NewValues["cust_whyhold"].ToString();
			var customer_whohold = e.NewValues["cust_whohold"] == null ? "" : e.NewValues["cust_whohold"].ToString();
			var old_customer_whyhold = e.OldValues["cust_whyhold"] == null ? "" : e.OldValues["cust_whyhold"].ToString();
		
			var cust1 = new NECustomer(Convert.ToInt32(e.Keys["cust"]));

			if (onhold == "F")
			{
				customer_whyhold = "";
				customer_whohold = "0";
			}

			if ( (customer_whyhold != old_customer_whyhold) && onhold == "T")
			{
				customer_whohold = current_user.id.ToString();
			}
			if ((e.NewValues["OnHold"].ToString() != e.OldValues["OnHold"].ToString()) && (e.NewValues["OnHold"].ToString()=="T"))
			{
				if ((customer_whyhold == null) || (customer_whyhold == ""))
				{
					throw(new Exception("You must enter a reason for putting the customer on hold"));
				}
			}
			
			
			cust1.Hold = onhold;
			cust1.customer_creditdays = creditdays;
			cust1.CreditLimit = creditlimit;
			cust1.Notes = customer_notes;
			cust1.Memo = customer_memo;
			cust1.salesnotes = customer_salesnotes.Replace("'", "");
			cust1.customer_collection_status = status;
			cust1.customer_arnotes = customer_arnotes.Replace("'","");
			cust1.customer_whyhold = customer_whyhold.Replace("'", "");
			cust1.customer_whohold =  (char.IsNumber(customer_whohold,0))==true? Convert.ToInt32(customer_whohold):0;
			cust1.Member_ID = current_user.id;
			cust1.customer_whyhold = customer_whyhold.Replace("'", "");
			cust1.Save();

			Session["ap_report_gv"] = null;
			//populate_grid();
		}
		catch (Exception ee)
		{
			throw (ee);
		}
		e.Cancel = true;
		Aspxgridview1.CancelEdit();
	}


	protected void dteScope_DateChanged(object sender, EventArgs e)
	{
		Session["ap_report_gv"] = null;
		//populate_grid();
	}


 

	protected void btnUpdate_Click(object sender, EventArgs e)
	{

	}
	protected void Aspxgridview1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{

	}
}
