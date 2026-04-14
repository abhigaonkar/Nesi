using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class modules_stuff_i_did : System.Web.UI.UserControl
{
	public NeMember myMember;
	Toolbox _tools;
    protected void Page_Load(object sender, EventArgs e)
    {
		_tools = new Toolbox();
		myMember							= Toolbox.do_handle_authentication(1);
//		gv_journal.DataSource = Toolbox.doSQL_dt(@"call get_journal(@v0,curdate())",new object[] { myMember.id } );
//		gv_journal.DataBind();
    }
}