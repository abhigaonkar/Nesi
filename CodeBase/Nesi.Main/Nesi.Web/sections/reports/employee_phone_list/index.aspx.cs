using System;
using nesi.core;
using nesi.core.print;

public partial class phonelist_report : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 76; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
    
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
    SqlBranches.SelectCommand = "SELECT id, ddl_name from business_unit  WHERE id in(" +
                                new Current_User().visible_business_units + ") order by ddl_name";
        if (!IsPostBack)
            {
            cboBranch.Value = myMember.business_unit.id32;
            }
        else
        {
        
        }
        bindreport();

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        
        
    }

    protected void cboBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindreport();
    }
    private void bindreport()
    {
        var selectcompany = -1;
        if (this.cboBranch.SelectedIndex >= 0)
            selectcompany = Convert.ToInt32(this.cboBranch.SelectedItem.Value);
        var report = new XtraRptEmployeePhoneList();
        report.Parameters[0].Value = selectcompany;
        this.ReportViewer1.Report = report;
        this.ReportViewer1.DataBind();


    }
  
}
