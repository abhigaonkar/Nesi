using System;
using System.Web.Script.Serialization;
using nesi.core;

public partial class modules_reviewed_by_admin : System.Web.UI.UserControl
	{
	public NeMember myMember;
	Toolbox _tools;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(1);

		}
    protected void Page_Load(object sender, EventArgs e)
		{
			
				fill();
			
		}
	protected void fill()
	{
		
	}
}
