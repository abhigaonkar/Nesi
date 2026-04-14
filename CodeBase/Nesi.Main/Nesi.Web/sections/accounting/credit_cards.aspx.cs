using System;
using DevExpress.Web;
using nesi.core;

public partial class credit_cards : System.Web.UI.Page
{
    NeMember myMember;
	Toolbox _tools;
    private const int _page_id = 190; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        
		_tools		= new Toolbox();
   		myMember	= Toolbox.do_handle_authentication(_page_id);
		((IntraDefault)this.Master).page_name		= NePage.get_page_name(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

		
	
        
    }



	protected void gv_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
	{
		


	}
	
	protected void gv_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
	{

		#region Page

		if (!NEUserPage.exists(myMember.id, 194, 1))
		{
	//		NEUserPage NE_up = new NEUserPage();
	//		NE_up.admin = myMember;
//			NE_up.user = new NeMember(Convert.ToInt32(e.Keys[0]));
	//		NE_up.page_id = 192;
//			NE_up.user_id = user_id;
	//		NE_up.type_id = 1;
	//		NE_up.save();
		}
		#endregion Page

	}

	protected void gv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		// Validate the data is correctly arranged.
		// Verify the employee belongs to the selected branch.
		var empId = Toolbox.ReturnZeroIfNull_int(e.NewValues["member_id"]);
		if(empId == 0)
			{
			e.Cancel = true;
			return;
			}
		var businessUnitId = Toolbox.ReturnZeroIfNull_int(e.NewValues["business_unit_id"]);
		if(businessUnitId == 0)
			{
			e.Cancel = true;
			return;
			}
		var emp = new NeMember(empId);
		if(emp.business_unit_id != businessUnitId)
			{
			throw new Exception("Employee's business unit is different than the selected business unit.");
			}
		}
	}
