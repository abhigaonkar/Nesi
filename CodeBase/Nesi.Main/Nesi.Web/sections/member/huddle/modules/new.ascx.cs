using System;
using nesi.core;

public partial class sections_member_huddle_modules_new : System.Web.UI.UserControl
	{
	public NeMember current_user { get; set; }
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!IsPostBack)
			{
			new_captain.Value		= current_user.id;
			}
		}
	protected void cbp_new_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		if(e.Parameter.StartsWith("save"))
			{
			var h			= new huddle();
			h.name				= new_name.Text;
			h.description		= new_description.Text;
			h.created_by		= (int) current_user.id;
			h.captain			= Convert.ToInt32(new_captain.Value);
			h.date_of_huddle = date_of_huddle.Date;
			h.save();
			// Add captain as huddle user
			var hu		= new huddle.user();
			hu.member			= h.captain;
			hu.huddle			= h.id;
			hu.save();
			clear();
			if(e.Parameter.EndsWith("close"))
				{
				cbp_new.JSProperties.Add("cpClose", "true");
				}
			else
				{
					date_of_huddle.Focus();
				}
			}
		}
	public void clear()
		{
		new_name.Text			= "";
		new_description.Text	= "";
		new_captain.Value		= current_user.id;
		date_of_huddle.Text = "";
		}
	
}