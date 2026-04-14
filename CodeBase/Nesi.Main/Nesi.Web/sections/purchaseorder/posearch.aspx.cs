using System;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using log4net;
using nesi.core;

public partial class sections_purchaseorder_posearch : System.Web.UI.Page
{
    NeMember myMember;
    Toolbox _tools = new Toolbox();
    private const int _page_id = 93; // from Page table in DB
    private const string _page_description = "Purchase Order Search";

    protected void Page_Load(object sender, EventArgs e)
    {

        
        myMember = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = _page_description;

        if (!IsPostBack)
        {
            txtSearch.Focus();
            var strCompany = "SELECT id,ddl_name name"
                                    + " from business_unit where id in (" + new Current_User().visible_business_units + ") order by ddl_name";

            var drCompany = _tools.getSQL_datatable(strCompany,null);

            var lic = new ListItem("All Business Units", "0");

            ddlCompany.DataSource = drCompany;
            ddlCompany.DataTextField = "name";
            ddlCompany.DataValueField = "ID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, lic);

			ddlCompany.SelectedIndex = 0;

        }
		else
			{
        var searchstring = txtSearch.Text;
        if (searchstring != "")
        {
		fill_grid();
        }

			}
    }
	private void fill_grid()
		{
        InfoLabel.Text = "";
        var POSearchInfo = new DataTable();
        var POSql = "";
        var POSqlAddOn = "";
        var searchstring = txtSearch.Text;
        if (ddlCompany.SelectedValue != "0")
        {
            POSqlAddOn = " AND a.business_unit_id = " + ddlCompany.SelectedValue + " ";
        }

        //Numeric Search
        var Number = 0;
		int.TryParse(searchstring, out Number);

        if (Number != 0)
        {
           //Search for PO Number or ID
            POSql = @"SELECT a.poprog_id, a.poprog_bvpo, a.poprog_vendor_id, URLDECODE(poprog_order_description) AS poprog_order_description, a.poprog_cutdate,a.poprog_total_cost,
                            b.Vendor_Name, c.status_type AS PO_Status, business_unit.ddl_name 
                            FROM poprog_header AS a, vendor AS b, poprog_status AS c, business_unit
                            WHERE a.poprog_vendor_id = b.Vendor_ID " + POSqlAddOn + @"
							and a.business_unit_id = business_unit.id
                            AND c.poprog_status_id = a.poprog_status
                            AND (poprog_id = " + Number + @"
                            OR poprog_bvpo LIKE '%" + Number + @"%')";

            

        }
        else
        {
            //Search For Vendor Name
			POSql = @"SELECT a.poprog_id, a.poprog_bvpo, a.poprog_vendor_id, URLDECODE(poprog_order_description) AS poprog_order_description, a.poprog_cutdate,a.poprog_total_cost,
                            b.Vendor_Name, c.status_type AS PO_Status, business_unit.ddl_name 
                            FROM poprog_header AS a, vendor AS b, poprog_status AS c, business_unit
                            WHERE a.poprog_vendor_id = b.Vendor_ID " + POSqlAddOn + @"
							and a.business_unit_id = business_unit.id
                            AND c.poprog_status_id = a.poprog_status
                            AND b.Vendor_name LIKE '%" + searchstring + @"%'";

        }

        try
        {
            POSearchInfo = _tools.getSQL_datatable(POSql,null);
            if (POSearchInfo.Rows.Count != 0)
            {
                GridViewPO.Visible = true;
                ResultLabel.Visible = false;
                GridViewPO.DataSource = POSearchInfo;
                GridViewPO.DataBind();
            }
            else
            {
                GridViewPO.Visible = false;
                ResultLabel.Visible = true;
                ResultLabel.Text = "Your Search Returned No Results.";
            }
            
        }
        catch { }
        POSearchInfo.Dispose();
		}
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var searchstring = txtSearch.Text;
        if (searchstring == "")
        {
            InfoLabel.Text = "Please Enter A Value to Search By";
            return;
        }
    }
}
