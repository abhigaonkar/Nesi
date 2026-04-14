using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_customer_modules_locations : System.Web.UI.UserControl
	{
	NeMember current_user;
	
	public int address_id  
		{ 
		get {var _address_id = 0; if(ViewState["address_id"] == null){return _address_id;}else{int.TryParse(ViewState["address_id"].ToString(), out _address_id); return _address_id; }} 
		set {ViewState["address_id"] = value.ToString();}
		}
	public int customer_id  
		{ 
		get {var _customer_id = 0; if(ViewState["customer_id"] == null){return _customer_id;}else{int.TryParse(ViewState["customer_id"].ToString(), out _customer_id); return _customer_id; }} 
		set {ViewState["customer_id"] = value.ToString();}
		}
	public NECustomer this_customer  { get; set; }

	public bool is_initialized	{get;set;}
	protected void Page_Init(object sender, EventArgs e)
		{
		Page.RegisterRequiresControlState(this);
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		}
	public void init()
		{
		if(customer_id > 0 && !IsPostBack)
			{
			var _tools											= new Toolbox();
			var customer										= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
			ds_locations.SelectParameters[0].DefaultValue			= customer_id.ToString();
			gv_locations.DataBind();
			var uc_location_detail	= (sections_customer_modules_location) gv_locations.FindEditFormTemplateControl("uc_location_detail");
			if(uc_location_detail != null)
				{
				uc_location_detail.customer_id						= customer_id;
				uc_location_detail.this_customer					= this_customer;
				if(gv_locations.IsEditing && !gv_locations.IsNewRowEditing)
					{
					uc_location_detail.address_id						= Convert.ToInt32(gv_locations.GetRowValues(gv_locations.EditingRowVisibleIndex, "address_id"));
					var pc_location							= (ASPxPageControl) uc_location_detail.FindControl("pc_location");
					if(pc_location != null)
						{
						var uc_contacts		= (sections_customer_modules_contact) pc_location.FindControl("uc_contacts");
						if(uc_contacts != null)
							{
							uc_contacts.address_id							= uc_location_detail.address_id;
							uc_contacts.this_customer						= this_customer;
							}
						var uc_sales			= (sections_customer_modules_sales) pc_location.FindControl("uc_sales");
						if(uc_sales != null)
							{
							uc_sales.customer_id							= customer_id;
							uc_sales.address_id								= (int) uc_location_detail.address_id;
							uc_sales.this_customer							= this_customer;
							//uc_sales.init();
							}
						}
					}
				}
			is_initialized			= true;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var index				= gv_locations.EditingRowVisibleIndex;
		var c					= (sections_customer_modules_location) gv_locations.FindEditFormTemplateControl("uc_location_detail");
		
		hdn_address_id.Value	= address_id.ToString();
		hdn_customer_id.Value	= customer_id.ToString();

		if(gv_locations.IsEditing && index > -1 && c != null)
			{
			c.customer_id											= customer_id;
			c.address_id											= Convert.ToInt32(gv_locations.GetRowValues(gv_locations.EditingRowVisibleIndex, "address_id"));
			c.this_customer											= this_customer;
			c.init();
			}
		}
	protected void gv_locations_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var grid										= (ASPxGridView) sender;
		var c					= (sections_customer_modules_location) grid.FindEditFormTemplateControl("uc_location_detail");
		c.customer_id											= customer_id;
		c.business_unit_id											= current_user.business_unit_id;
		c.address_id											= Convert.ToInt32(grid.GetRowValues(grid.EditingRowVisibleIndex, "address_id"));
		c.this_customer											= this_customer;
		c.this_address = new NEAddress(c.address_id);
		c.init();
		}
}