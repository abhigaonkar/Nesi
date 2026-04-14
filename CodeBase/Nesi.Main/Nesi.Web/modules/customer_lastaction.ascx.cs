using System;
using System.Web.Script.Serialization;
using nesi.core;

public partial class modules_customer_lastaction : System.Web.UI.UserControl
	{
	JavaScriptSerializer JSON				= new JavaScriptSerializer();
	private const int _page_id			= 10;
	NeMember current_user;
	Toolbox _tools			= new Toolbox();
    protected override void OnInit(EventArgs e)
        {
        base.OnInit(e);
        this.DataBinding += new EventHandler(this_databind);
		}
	protected void this_databind(object sender, EventArgs e)
		{
		this.init();
		}
	public int address_id  
		{ 
		get {var _address_id = 0; int.TryParse(hid_address_id.Value, out _address_id); return _address_id; } 
		set {hid_address_id.Value = value.ToString();}
		}
	public int customer_id  
		{ 
		get {var _customer_id = 0; int.TryParse(hid_customer_id.Value, out _customer_id); return _customer_id; } 
		set {hid_customer_id.Value = value.ToString();}
		}
	public string javascript_closeaction
		{ 
		get {return hid_js_closeaction.Value; } 
		set {hid_js_closeaction.Value = value;}
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user			= Toolbox.do_handle_authentication(Convert.ToInt32(_page_id));
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	public void init()
		{
		if(customer_id != 0)
			{
			var c						= new NECustomer((int) customer_id);
			var a							= new NEAddress(address_id);
			lastaction_customername.Text		= c.Customer_Name;
			lastaction_customerlocation.Text	= a.Addr1;
			lastaction_dt.Date					= DateTime.Now;
			lastaction_employee.Value			= current_user.id;
			lastaction_action.Value				= null;
			lastaction_notes.Text				= "";
			lastaction_origin.Value				= null;
			}
		}
	protected void cb_customer_lastaction_action_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var js				= JSON.Deserialize<from_json>(e.Parameter);
		var ch			= new customer_history();
		ch.member_id				= current_user.id;
		ch.action_id				= js.action;
		ch.date						= Convert.ToDateTime(js.date);
		ch.customer_id				= js.customer_id;
		ch.notes					= js.note;
		ch.origin					= js.origin;
		ch.address_id				= (int) js.address_id;
		try
			{
			ch.save();
			e.Result					= javascript_closeaction;
			}
		catch (Exception ee)
			{
			e.Result					= "alert('There was an error saving - "+ee.Message+"')";
			}
		}
		struct from_json
			{
			public int customer_id  { get; set; }
			public int address_id  { get; set; }
			public string date  { get; set; }
			public int action  { get; set; }
			public int origin  { get; set; }
			public int employee  { get; set; }
			public string note  { get; set; }
			}
}