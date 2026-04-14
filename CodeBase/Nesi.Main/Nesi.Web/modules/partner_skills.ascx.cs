using System;
using nesi.core;

public partial class modules_partner_skills : System.Web.UI.UserControl
	{
	NeMember current_user;
//	public NECustomer this_customer  { get; set; }
//	public NEVendor this_vendor { get; set; }
	public int customer_id  
		{ 
		get {var _customer_id = 0; if(ViewState["customer_id"] == null){return _customer_id;}else{int.TryParse(ViewState["customer_id"].ToString(), out _customer_id); return _customer_id; }} 
		set {ViewState["customer_id"] = value.ToString();}
		}
	public int vendor_id  
		{
			get { var _vendor_id = 0; if (ViewState["vendor_id"] == null) { return _vendor_id; } else { int.TryParse(ViewState["vendor_id"].ToString(), out _vendor_id); return _vendor_id; } }
			set { ViewState["vendor_id"] = value.ToString(); }
		}
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.DataBinding += new EventHandler(this_databind);
	}
	protected void this_databind(object sender, EventArgs e)
	{
		this.init();
	}

	
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
	
		}
	protected void Page_Load(object sender, EventArgs e)
	{
		
	
	}

public void init()
		{
			if (customer_id != 0)
			{
		//		this_customer = this_customer == null ? new NECustomer((int)customer_id) : this_customer;
				hdn_member_id.Value = current_user.id.ToString();
				hdn_table_id.Value =  customer_id.ToString();
				hdn_table.Value = "Customer";
				gv.DataBind();
			}
			if (vendor_id != 0)
			{
		//		this_vendor = this_vendor == null ? new NEVendor((int)vendor_id) : this_vendor;
				hdn_member_id.Value = current_user.id.ToString();
				hdn_table_id.Value = vendor_id.ToString();
				hdn_table.Value = "Vendor";
				gv.DataBind();
			}
		}



protected void gv_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
{
	if (e.Parameters != null)
	{
		var parameters = e.Parameters.Split('|');
		if (parameters[0] == "DELETE")
		{
			var key = gv.GetRowValues(Convert.ToInt32(parameters[1]), "id").ToString();
			Toolbox.doSQL_void(@"Delete from partner_skills where id =@v0", key);
		}
		gv.DataBind();
	}
}
}