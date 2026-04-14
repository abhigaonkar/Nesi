using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using nesi.bv;
using nesi.core;

public partial class sections_accounting_terms : System.Web.UI.Page
	{
	NeMember current_user;
	int _page_id = 224;
	protected  void Page_Init(object sender, EventArgs e)
		{
		xpo_ds.Session = XpoHelper.GetNewSession();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		}

	protected void gv_term_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
		{
		//using (var bvConn = BVDB.connect("NSNEEI"))
			{
	  //		NeAccounting.Terms.sync_bv(bvConn);
			}
		}

	protected void gv_term_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
		{
		//using (var bvConn = BVDB.connect("NSNEEI"))
			{
	//		NeAccounting.Terms.sync_bv(bvConn);
			}
		}

    protected void gv_term_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
    {
        e.Editor.ReadOnly = true;
    }
}