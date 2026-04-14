using System;
using System.Data;
using System.Linq;
using DevExpress.Web;
using nesi.core;

public partial class modules_signpad : System.Web.UI.UserControl
	{
	public bool is_mobile  { get; set; }
	public string cancel_event  { get; set; }
	public string js_action  { get; set; }
	public int woprog_id  { get; set; }
	
	public bool show_po  { get; set; }
	public bool show_printed_name  { get; set; }
	public bool show_contact_ddl  { get; set; }
	public int customer_id  { get; set; }
	public int address_id  { get; set; }

	protected void Page_Init(object _sender, EventArgs _e)
		{
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		text_printed_name.Visible				= true;
		text_printed_name.Style["display"]		= show_printed_name ? "block" : "none";
		text_po.Visible							= show_po;
		//contact_table.Visible					= show_contact_ddl;
		if (show_contact_ddl && customer_id > 0)
		{
			var wo = new NeWOProg(woprog_id);
			fill_contacts();
			if (show_po)
			{

				text_po.Text = wo.PONumber;
			}
			if (!IsPostBack)
			{
				combo_contact.Value = wo.woprog_Contact_ID;
			}

		}
		if(is_mobile)
			{
			button_cancel.Style["padding"]		= "10px 0px 10px 0px";
			button_clear.Style["padding"]		= "10px 0px 10px 0px";
			button_save.Style["padding"]		= "10px 0px 10px 0px";
			}
		}
	private void fill_contacts()
		{
		var dt						= Toolbox.doSQL_dt(@"SELECT 0 id, 'No Email' name, '' email UNION SELECT 1 id, 'Multiple Recipients' name, '' email UNION (SELECT contact_id id, IFNULL(contact_name, '') name, IFNULL(contact_email, '') email FROM contact WHERE contact_type = 'Customer' AND contact_cust_id = @v0  ORDER BY contact_name ASC)", new object[] {  customer_id } );
		foreach(DataRow dr in dt.Rows)
			{
			var x = dr[0].GetType();
			var y = dr[1].GetType();
			var z = dr[2].GetType();
			}
		combo_contact.DataSource			= (from d in dt.AsEnumerable() 
												select new
													{
													id = Convert.ToInt32(d["id"]),
													name = d["email"].ToString().Trim() == "" && Convert.ToInt32(d["id"]) > 1 ? d["name"]+" (EMAIL?) " : d["name"].ToString()
													}).ToList();
		combo_contact.DataBind();
		recipient_list.DataSource			= (from d in dt.AsEnumerable() 
												where Convert.ToInt32(d["id"]) != 0 && Convert.ToInt32(d["id"]) != 1
												select new
													{
													id = Convert.ToInt32(d["id"]),
													name = d["email"].ToString().Trim() == "" ? d["name"]+" (EMAIL?) " : d["name"]
													}).ToList();
		recipient_list.DataBind();
		}
	protected void cb_newcontact_Callback(object _source, CallbackEventArgs _e)
		{
		if(_e.Parameter.Contains("|"))
			{
			var paras			= _e.Parameter.Split('|');
			var contact_name	= paras[0];
		
			var contact_email	= paras[1];
			if(!Toolbox.CheckEmail(contact_email))
				{
				throw new Exception("Sorry, that is not a valid email address. Please try again.");
				}
			var c = new NEContact
									{
										customer_id = customer_id,
										type = "Customer",
										status = "Active",
										name = contact_name,
										address_id = address_id,
										email = contact_email
									};

			c.save();
			_e.Result		= c.id.ToString();
			}
		}
	protected void cb_existingcontact_Callback(object _source, CallbackEventArgs _e)
		{
		if(_e.Parameter.Contains("|"))
			{
			var paras			= _e.Parameter.Split('|');
			var contact_name	= paras[0];
		
			var contact_email	= paras[1];
			var contact_id			= Convert.ToInt32(paras[2]);
			if(!Toolbox.CheckEmail(contact_email))
				{
				throw new Exception("Sorry, that is not a valid email address. Please try again.");
				}
			var c = new NEContact(contact_id)
						{
							name = contact_name,
							email = contact_email
						};

			c.save();
			_e.Result		= c.id.ToString();
			}
		}
	protected void combo_contact_Callback(object _sender, CallbackEventArgsBase _e)
		{
		}
	protected void popup_newcontact_WindowCallback(object _source, PopupWindowCallbackArgs _e)
		{
		if(_e.Parameter != "" && _e.Parameter != "0")
			{
			var contact_id			= Convert.ToInt32(_e.Parameter);
			var c				= new NEContact(contact_id);

			tb_name.Text = c.name;
			tb_email.Text			= c.email;
			hid_contact_id.Value	= _e.Parameter;
			}
		else
			{
			
			tb_name.Text			= "";
			tb_email.Text			= "";
			hid_contact_id.Value	= "";
			}
		}
	protected void cbp_recipient_list_Callback(object sender, CallbackEventArgsBase e)
		{
		fill_contacts();
		recipient_list.ClientVisible = true;
		}
}