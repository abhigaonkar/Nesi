using System;
using nesi.core;

public partial class mobile_modules_signoff : shared.mobile_subpage
{
	Toolbox _tools;
	NeMember current_user;
	bool approve_pm;
	bool approve_bm;
	bool view_cost;
	bool can_create_locations;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools											= new Toolbox();
		current_user									= Toolbox.do_handle_authentication(1);
		}
    protected void Page_Load(object sender, EventArgs e)
		{
		if(this_woprog_id > 0)
			{
			approval_frame.Attributes["src"]		= "/sections/reports/invoice_preview/frame.aspx?id="+this_woprog_id+"&do_signature=false&is_signoff=true&from=" + from;
			}
		}
}