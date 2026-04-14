using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.IO;
using nesi.core;
using NESI.Common.Models;

public partial class sections_purchaseorder_company_credit_card_frame : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id		= 194; // from Page table in DB
	private const string _page_name = "Company Credit Card Expense";
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static string default_filter = "";
	SqlDataSource ds_templates;
	ASPxHiddenField h;
    Toolbox _tools;
	string dsn = "";
    bool is_admin = false;


	protected void Page_Init(object sender, EventArgs e)
	{
        _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(1);
        is_admin = myMember.AuthenticatedForPrivilege(175);
        _tools.dont_cache_page();

		if ((_tools.getSQL_int(@"select count(id) from credit_cards  where member_id =@v0", new object[] { myMember.id })==0)&& myMember.business_unit_id!=11 && !myMember.AuthenticatedForPage(194))
		{
			Toolbox.FriendlyException(this.Response, "Sorry, only credit card holders have access to this page", @"\sections\purchaseorder\po_prog_edit.aspx");

		}

		if (Session["working_business_unit_id"] == null)
		{
			Session["working_business_unit_id"] = myMember.business_unit_id.ToString();
		}
		
		layout.__page_name = _page_name;
		layout.used_gv = gv;
		SqlDataSource1.SelectParameters[0].DefaultValue	= myMember.id.ToString();
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = string.Format("{0}", _page_name);
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
	}



	protected void Page_Load(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		myMember				= Toolbox.do_handle_authentication(_page_id);
		var menu				= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml		= menu.MenuHTML;
		_tools.dont_cache_page();
		# region Load Summary Gridview
		if (!IsCallback && !IsPostBack)
		{
			
			var gl = new NeGridLayouts(myMember.id, string.Format("{0}", _page_name));
			var temp_company = new NeBusinessUnit(Session["working_business_unit_id"]);
			if (gl.GridLayoutID != 0)
			{
				gv.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;

			}
			else
			{
				gv.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = default_filter;
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = Session["working_business_unit_id"] != null ? string.Format("{0}", _page_name) : string.Format("{0}", _page_name);
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
			}

		}
		#endregion
	


		divSide.InnerHtml		= shared.PrintSidePanelHTML(myMember);

		
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
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.Caption == "Receipt")
		{
			// Get Expense ID
			var row_id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "id"));
			// Load Expense
			if (row_id != null)
			{
				
				var exp = new NECredit_card_purchase(row_id);
				// Check if it has a file
				if (exp.has_file)
				{
					// Check if file exists
					var fileServer		= NeTaxEntity.BaseFolder(exp.business_unit_id, false);
					var path = fileServer + @"\credit_card_receipts\" + exp.id + "." + exp.file_ext;
                    var pathExt = NeTaxEntity.BaseFolder(exp.business_unit_id, true) + @"/credit_card_receipts/" + exp.id + "." + exp.file_ext;
                    if (File.Exists(path))
					{
						// Create link to file
						e.Cell.Text = string.Format(@"<a href='javascript:void(0)' onmousedown=""get_attachment(event, {0}, '{1}');""><img src='/images/icon/icon[attachment].gif' /></a>", exp.id, pathExt);
					}
				}
			}
			else
			{
				throw new Exception("Could not retrieve cc purchase information");
			}
		}
	}
	protected void gv_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
        if (e.VisibleIndex > -1)
			{
		var status = gv.GetRowValues(e.VisibleIndex, "status").ToString();
		var buyer =  gv.GetRowValues(e.VisibleIndex, "member_id").ToString();
        var sub_users = Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(" + myMember.id + "), '')");
       
            if (e.ButtonID.Equals("Approve"))
		{
			e.Visible = DevExpress.Utils.DefaultBoolean.False;
			if ((status.Equals("Waiting for Approval"))&&(NeMember.is_supervisor(Convert.ToInt32(buyer),myMember.id)))
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.True;
			}
		}
		else if (e.ButtonID.Equals("Deny"))
		{
			e.Visible = DevExpress.Utils.DefaultBoolean.False;
			if ((status.Equals("Waiting for Approval")) && (NeMember.is_supervisor(Convert.ToInt32(buyer), myMember.id)))
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.True;
			}
		}
		else if (e.ButtonID.Equals("Approved"))
		{
			e.Visible = DevExpress.Utils.DefaultBoolean.False;
			if (myMember.business_unit_id==11)
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.True;
			}
		}
            else if (e.ButtonID.Equals("Edit"))
            {
                e.Visible = DevExpress.Utils.DefaultBoolean.False;
                var row_id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "id"));
                var exp = new NECredit_card_purchase(row_id);
                if(exp.woprog_id != 0)
                {
                    var woProgStatus = Toolbox.doSQL_string(@"SELECT woprog_status from woprog WHERE woprog_id = @v0", new object[] { exp.woprog_id });
                    if ((woProgStatus == "Open" || woProgStatus == "Rework") && (is_admin || sub_users != ""))
                    {
                        e.Visible = DevExpress.Utils.DefaultBoolean.True;
                    }
                }
                else if (is_admin || sub_users != "")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.True;
                }
            }
        }
      
	}
   /* protected void gv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        var dr_exp = Toolbox.doSQL_dt(@"SELECT * FROM credit_card_purchase WHERE id = @v0 ", new object[] {  e.Keys[0] } ).Rows[0];
        var woprog_id = 0;
        int.TryParse(dr_exp["woprog_id"].ToString(), out woprog_id);
        var description = dr_exp["item_text"].ToString();
        var id = dr_exp["id"].ToString();
		
        var seller_id = Convert.ToInt32(dr_exp["seller_id"]);
        var affected_workorder_result = "";

        var originCount = Toolbox.doSQL_int(@"SELECT COUNT(origin_id) FROM credit_card_purchase WHERE origin_id = @v0", new object[] { id });

        if (originCount > 0)
        {
            throw new Exception("You cannot delete this expense as it is the origin of another.");
        }

        if (woprog_id > 0)
        {
            var wo = new NeWOProg(woprog_id);
            if ((wo.Status == "Invoiced")||(wo.Status == OpsWOStatus.WaitingToBeInvoiced)||(wo.Status == "Waiting For PO")||(wo.Status == "Waiting Parent BM Approval"))
            {
                throw new Exception("This work order is no longer open, you cannot delete an expense through this interface, it must be manually denied");
            }
            var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current 
WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_consignment_id = @v1", new object[] { woprog_id,  e.Keys[0]});
            if (c == 1)
            {
                var dr_wo = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and  wo_detail_current_consignment_id = @v1", new object[] {  woprog_id,  e.Keys[0] } ).Rows[0];
                NeWODetailCurrent.delete_workorder_line(Convert.ToInt32(dr_wo["wo_detail_current_id"]), OpsSpecialPart.CompanyCreditCardExpense, myMember);
                NeWOProg.update_header_totals(woprog_id.ToString(), myMember.business_unit_id, wo.OrderNumber);
                affected_workorder_result = string.Format("Work order #{0} has been updated.", wo.OrderNumber);
            }
            else if (c > 1)
            {
                throw new Exception("There are more than one entries on that work order matching this expense, it might need to be manually denied.");
            }
        }
        else
        {
            affected_workorder_result = "This was a shop expense, no work order needed to be updated.";
        }
        Toolbox.doSQL_void(@"DELETE FROM credit_card_purchase WHERE id = @v0 LIMIT 1", new object[] {  e.Keys[0] } );
    
        gv.DataBind();
        e.Cancel = true;
       
    }*/

}

