using System;
using nesi.core;

public partial class ext_locations : System.Web.UI.Page
	{
	NeMember myMember;
	Toolbox _tools = new Toolbox();
	int _page_id = 124;

	protected void Page_Load(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		((IntraDefault)this.Master).page_name = NePage.get_page_name(_page_id);
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		cb_branch.ClientEnabled	= myMember.AuthenticatedForPrivilege(90003);
		if (!IsPostBack)
			{
			cb_branch.Value = myMember.business_unit_id;
			}
		}

	protected void gv_default_locations_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		if (e.NewValues["location"] != null)
			{
			Toolbox.doSQL_void(@"UPDATE member SET member_default_location = @v0  WHERE member_id =@v1" ,
				new object[] { e.NewValues["location"],  e.Keys[0]});
			}
		e.Cancel = true;
		gv_default_locations.CancelEdit();
		}
	protected void gv_default_locations_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.FieldName == "location" && Toolbox.ReturnZeroIfNull_int(e.CellValue) == 0)
			{
			e.Cell.ForeColor			= System.Drawing.Color.Red;
			e.Cell.Style["font-weight"]	= "bold";
			}
		}
}
