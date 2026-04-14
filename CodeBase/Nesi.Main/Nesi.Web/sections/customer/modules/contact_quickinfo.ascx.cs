using System;
using nesi.core;

public partial class sections_customer_modules_contact_quickinfo : System.Web.UI.UserControl
	{
	public int contact_id  { get; set; }
	NeMember current_user;
	Toolbox _tools			= new Toolbox();
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user			= (NeMember) Session["profile"];
		}
	protected void Page_Load(object sender, EventArgs e)
	{
		fill_page();

	}
	public void fill_page()
	{
		if (contact_id != 0)
		{
			var c = new NEContact(contact_id);
			txt_popcontact_cell.Text = c.cellphone;
			txt_popcontact_email.Text = c.Contact_Email;
			txt_popcontact_facebook.Text = c.facebook;
			txt_popcontact_linkedin.Text = c.linkedin;
			txt_popcontact_name.Text = c.name;
			txt_popcontact_password.Text = c.password;
			txt_popcontact_phone.Text = c.direct_line;
			txt_popcontact_twitter.Text = c.twitter;
			txt_popcontact_username.Text = c.Contact_Email;
			dte_popcontact_birthday.Date = c.Contact_Birthday;
			ddl_popcontact_loginstatus.Value = Convert.ToInt32(c.Contact_Login_enabled).ToString();
			ddl_popcontact_status.Value = c.Contact_Status_ID;
			dd_popcontact_title.Text = c.Contact_Title;
			
		}
	}
}