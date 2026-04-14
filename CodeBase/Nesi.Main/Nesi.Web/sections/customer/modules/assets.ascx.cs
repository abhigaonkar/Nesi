using System;
using nesi.core;

public partial class sections_customer_modules_assets : System.Web.UI.UserControl
	{
	NeMember current_user;
	public NECustomer this_customer  { get; set; }
	public NEAddress this_address  { get; set; }
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
	public void init()
		{
		Toolbox.do_debug("Assets UC Init Start");
		this_customer	= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
		this_address = new NEAddress(address_id);
		assets_frame.Attributes["src"] = "/sections/reports/customer_assets/index.aspx?customer_id=" + customer_id + "&address_id=" + address_id;
		Toolbox.do_debug("Assets UC Init End");
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		}
	}