using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class master_contact : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id = 120; // from Page table in DB
	private const string _page_name = "MasterContact";
	Toolbox _tools = new Toolbox();
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

    protected void Page_Init(object sender, EventArgs e)
    {
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
	//	myMember = Toolbox.do_handle_authentication("12");
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		layout.used_gv						= gv_MasterContacts;
		h.Set("gridview_id", "gv_MasterContacts");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
		_tools.dont_cache_page();
    }

	protected void Page_Load(object sender, EventArgs e)
		{
		_tools.dont_cache_page();
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Contacts Report";

		if (!IsPostBack)
			{
				Session["master_contact_grid"] = null;

			var gl	= new NeGridLayouts(myMember.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout		= "";
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_MasterContacts.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			}
		fill_grid();
		
		}


	protected void fill_grid()
	{
		if (Session["master_contact_grid"] == null)
		{
			Session["master_contact_grid"] = _tools.getSQL_datatable(@"
SELECT
	a.contact_id,
	a.contact_name,
	a.contact_email,
	a.contact_title,
	a.contact_cellphone,
	a.contact_status,
	a.contact_extension,
	a.contact_login,
	a.contact_password,
	a.contact_type,
	a.contact_status_id,
	a.contact_login_enabled,
	a.contact_directline,
	a.contact_cust_id,
	b.customer_name,
	c.vendor_name,
	a.stopsurveys,
	d.address_addr1,
	d.address_city,
	d.Address_Postal,
	d.address_prov,
	f.member_fullname project_mgr,
	g.ddl_name business_unit,
	b.customer_id,
(select count(woprog.WOProg_ID) from woprog where woprog.woprog_customer_id = b.customer_id and WOProg_Contact_ID = a.contact_id and a.contact_type='Customer') wos,
	(select count(quote_master.quote_id) from quote_master where quote_master.customer_id = b.customer_id and quote_master.Contact_ID = a.contact_id and a.contact_type='Customer') quotes
FROM
	contact a
LEFT JOIN 
	customer b ON 
		a.contact_cust_id = b.customer_id AND 
		a.contact_type = 'Customer'
LEFT JOIN 
	vendor c ON 
		a.contact_cust_id = c.vendor_id AND 
		a.contact_type = 'Vendor'
LEFT JOIN
	address d ON a.address_id = d.address_id 
LEFT JOIN
	customer_sales_properties e ON d.address_id = e.address_id
LEFT JOIN
	member f ON e.project_mgr_member_id = f.member_id
LEFT join
	business_unit g ON f.business_unit_id = g.id",null);
		}

		gv_MasterContacts.DataSource = Session["master_contact_grid"];
		gv_MasterContacts.DataBind();
	}

	protected void Button2_Click(object sender, EventArgs e)
		{
		}

	protected void Button1_Click(object sender, EventArgs e)
		{
		gv_MasterContacts.CancelEdit();
		}


	protected void Aspxgridview1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var _set = "";
		GridViewDataColumn gv;
		var memberid = 0;
		if ((e.NewValues["Account_Manager"]!=null) && (e.NewValues["Account_Manager"].ToString() != ""))
			{
				try
				{
					gv = (GridViewDataColumn)gv_MasterContacts.Columns[7];
					var ddl_AM = (ASPxComboBox)gv_MasterContacts.FindEditRowCellTemplateControl(gv, "ddlEditAccountManager");
					memberid = Convert.ToInt32(ddl_AM.Value);
					_set = "customer_account_manager = " + memberid + ",";
				}
				catch { }

			}
		var control_Memberid = 0;
		if ((e.NewValues["Controls"] != null)&&(e.NewValues["Controls"].ToString()!=""))
			{
				try
				{
					gv = (GridViewDataColumn)gv_MasterContacts.Columns["Controls Guy"];
					var ddl_CG = (ASPxComboBox)gv_MasterContacts.FindEditRowCellTemplateControl(gv, "ddlcustomer_controls");
					try { control_Memberid = Convert.ToInt32(ddl_CG.Value); }
					catch { control_Memberid = memberid; }
					//   memberid = Convert.ToInt32(e.NewValues["Account Manager"]);
					_set += "customer_controlsguy = " + control_Memberid + ",";
				}
				catch { }
			}

		var nextdate = 0;
		if (e.NewValues["Call Cycle"] != null)
			{
			try
				{
				nextdate = Convert.ToInt16(e.NewValues["Call Cycle"].ToString());
				}
			catch
				{
				}
			_set += "customer_frequency_days = '" + nextdate + "',";
			}
        double _discount = 0;
            if (e.NewValues["Discount"] != null)
            {
                _discount = Convert.ToDouble(e.NewValues["Discount"].ToString());
                _set += "customer_discount = " + _discount + ", ";
            }
			
		try
		{
			Toolbox.doSQL_void(@"Update customer " + _set.Remove(_set.Length - 1, 1) + " where customer_id =@v0 ", new object[] {e.Keys["ID"]});
		}
		catch { throw new Exception("could save for whatever reason"); }

			if ((e.NewValues["Action"] != null) && (e.NewValues["Action"].ToString() != ""))  // if there is a new action
			{
				if (((e.OldValues["Action"] == null) || (e.OldValues["Action"].ToString() == "")) || (e.NewValues["Action"].ToString() != e.OldValues["Action"].ToString()))  // if the old value is blank or it's changed
				{
					if (e.NewValues["Customer_LastDateTime"] != null)
					{
						try
						{
						
								var lastactionid = Convert.ToInt32(e.NewValues["Action"]);

								Toolbox.doSQL_void(@"Insert into customer_history (customer_history_memberID,customer_history_Action,customer_history_custID,customer_history_date)  Values (@v0,@v1,@v2,@v3)",new object[] { myMember.id,lastactionid,e.Keys["ID"],Convert.ToDateTime(e.NewValues["Customer_LastDateTime"]).ToString("yyyy-MM-dd") } );
						
						}
						catch
						{
							throw new Exception("could save for whatever reason, who knows... its a mystery.");
						}

					}
				}
			}
		e.Cancel = true;

		gv_MasterContacts.CancelEdit();
		Session["master_contact_gv"] = null;
		Session["master_contact_grid"] = null;
		fill_grid();
		}

	protected void Aspxgridview1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if ((e.DataColumn.FieldName == "Vendor_Name")||(e.DataColumn.FieldName == "Customer_Name"))
			{
				e.Cell.Text = _tools.value_from(e.CellValue.ToString());

			}
		}

	protected void Aspxgridview1_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		
		
		}

	

	protected void LinkButton1_Click(object sender, EventArgs e)
		{

		var index = (((LinkButton) sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
		var val = gv_MasterContacts.GetRowValues(index, "customer_id");
		var progress = new NECustomer(Convert.ToInt32(val));
		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('../../customer/index.aspx?Customer_id=" + progress.Customer_ID + "&business_unit_id=" + progress.business_unit_id + "','wo',950,800)", true);
		}

	protected void gv_MasterContacts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_MasterContacts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
		}


	protected void gv_MasterContacts_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (gv_MasterContacts.GetRowValues(e.VisibleIndex, "contact_login_enabled").ToString() == "1")
			{
				e.Row.ForeColor = System.Drawing.Color.Red;
				e.Row.Style.Add("font-weight", "bold");
			}
		}
	}
	protected void chk_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
		{
		var cb							= (ASPxCheckBox) sender;
		var tc	= (GridViewDataItemTemplateContainer) cb.NamingContainer;
		var vis_index							= tc.VisibleIndex;
		var dr								= gv_MasterContacts.GetDataRow(vis_index);
		e.Properties["cpID"]					= dr["contact_id"];
		}
	protected void cb_action_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras		= e.Parameter.Split('|');
			var action		= paras[0];
			var id				= Convert.ToInt32(paras[1]);
			var chked			= Convert.ToBoolean(paras[2]);
			var c			= new NEContact(id);
			switch(action)
				{
				case "stop_surveys":
					c.stopsurveys		= chked;
					c.save();
				break;
				case "login_enabled":
					if(chked && (c.email == "" || !Toolbox.CheckEmail(c.email) || c.password == "") )
						{
						e.Result			= "You need to first supply an email address and password before enabling login privileges";
						}
					else
						{
						c.login_enabled		= chked ? 1 : 0;
						c.save();
						}
				break;
				}
			}
		}
}
