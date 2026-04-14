using System;
using System.Data;
using System.Web.UI;
using nesi.core;

public partial class pmwo_throughput : System.Web.UI.Page
{

    public NeMember myMember;

    private const int _page_id = 68; // from Page table in DB
    private const string _page_description = "Reports / PM WorkOrder Throughput";

    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();

        if (!IsPostBack)
        {
            var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);


            var cList2 = new NeBusinessUnit();
            ddlCompany.DataSource = NeBusinessUnit.units_active();
            ddlCompany.DataBind();
            ddlCompany.SelectedValue = myMember.business_unit_id.ToString();

            ddlCompany.Enabled = myMember.AuthenticatedForPrivilege(53);

            var mlist = new NeMember();
        ddlPM.DataSource = do_ddlPMDT(myMember.business_unit_id );
            
            ddlPM.DataBind();
            ddlPM.SelectedValue = myMember.id.ToString();
			
            txtSelectedDate.Text = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
            txtEndDate.Text = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
           
        
        }
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        var mlist = new NeMember();
    ddlPM.DataSource = do_ddlPMDT(Convert.ToInt32(ddlCompany.SelectedValue));
        ddlPM.DataBind();
		


    }

    private DataTable do_ddlPMDT(int buid)
        {
        return Toolbox.doSQL_dt(@" SELECT a.member_id id, a.member_fullname name FROM member a INNER JOIN memberpage b ON a.member_id = b.memberpage_member_id 
INNER JOIN memberpageprivilege c ON c.memberpageprivilege_memberpage_id = b.memberpage_id 
INNER JOIN page d ON b.memberpage_page_id = d.page_id 
INNER JOIN privilege e ON c.memberpageprivilege_privilege_id = e.privilege_id 
WHERE a.member_status = 'Active' AND a.business_unit_id = @v3  AND d.page_id = @v0  AND (c.MemberPagePrivilege_Privilege_ID = @v1  OR c.MemberPagePrivilege_Privilege_ID = @v2 ) ", new object[] { 12, 15, 16, buid });
    }

    protected void Button1_Click(object sender, ImageClickEventArgs e)
    {
		
        
        var strWO = "";
        var strCustomerName = "";
        var strDateInvoiced = "";
        var strInvoiceNumber = "";
        var strDollarsInvoiced = "";
        DateTime date;

        double dblDollarsInvoiced = 0;
        double dblTotalDollarsInvoiced = 0;
        double dblTimeAndMaterial = 0;

        var StrSelectedDate = txtSelectedDate.Text;
        var StrEndingDate = txtEndDate.Text;
        var total_days = 0;
        
        

        if (StrEndingDate == "")
        {
            StrEndingDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        if (StrSelectedDate == "")
        {
            StrSelectedDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        total_days = Convert.ToDateTime(StrEndingDate).Subtract(Convert.ToDateTime(StrSelectedDate)).Days;


        var myDataTable = new DataTable();

		myDataTable.Columns.Add("bvwo", typeof(string));
		myDataTable.Columns.Add("customer", typeof(string));
        myDataTable.Columns.Add("_date", typeof(string));
		myDataTable.Columns.Add("invoiced_net", typeof(string));
		myDataTable.Columns.Add("invoice_no", typeof(string));
      


       /* strCompInfo = "SELECT Company_ID, name, Company_DSNBV7, country "
                               + " from business_unit  where active = 'T' AND Company_ID=" + ddlCompany.SelectedValue.ToString();
       
        OdbcCommand comCompInfo = new OdbcCommand(strCompInfo, Conn);
        OdbcDataReader drCompInfo = comCompInfo.ExecuteReader();
        drCompInfo.Read();*/

       


        //DSN = drCompInfo["Company_DSNBV7"].ToString();
        //BVConn = NeDB.getCon(DSN);

		var strWOInvoiced =@"SELECT WOProg_BVWO bvwo, WOProg_InvoiceDate _date, WOProg_CustomerName customer, 
if(woprog_quoteid =0, WOProg_StillToBeBilled ,get_invoiced_of_all_wos(woprog_id)) invoiced_net,
WOProg_InvoiceNo invoice_no, benchmark_labor_sell LabourTotalSell, benchmark_material_sell MaterialTotalSell FROM WOProg
WHERE WOProg_InvoiceDate BETWEEN '" 
+ StrSelectedDate + "' AND '" 
+ StrEndingDate + "' "+ " AND WOProg_Status ='Invoiced' and WOProg_Associate_WOProg_ID = 0 AND WOProg_PM_MemberID=" 
+ ddlPM.SelectedValue+ " GROUP BY WOProg_BVWO, WOProg_InvoiceDate, WOProg_CustomerName";

		var dtWOInvoiced = Toolbox.doSQL_dt(@"SELECT WOProg_BVWO bvwo, WOProg_InvoiceDate _date, WOProg_CustomerName customer, 
if(woprog_quoteid =0, WOProg_StillToBeBilled ,get_invoiced_of_all_wos(woprog_id)) 
invoiced_net, WOProg_InvoiceNo invoice_no, benchmark_labor_sell LabourTotalSell, benchmark_material_sell MaterialTotalSell
FROM WOProg  WHERE WOProg_InvoiceDate BETWEEN @v0 AND @v1 AND WOProg_Status ='Invoiced' and WOProg_Associate_WOProg_ID = 0 AND WOProg_PM_MemberID=@v2  
GROUP BY WOProg_BVWO, WOProg_InvoiceDate, WOProg_CustomerName", new object[] { StrSelectedDate,StrEndingDate,ddlPM.SelectedValue });
        try
        {
            foreach (DataRow drInvoiced in dtWOInvoiced.Rows)
            {

                strWO = drInvoiced[0].ToString();
                strCustomerName = drInvoiced[2].ToString();
                try { date = Convert.ToDateTime(drInvoiced[1].ToString());
                      strDateInvoiced = date.ToString("yyyy-MM-dd");
                }
                catch { strDateInvoiced = ""; }

                try
                {
                    strInvoiceNumber = drInvoiced[4].ToString();
                }
                catch { strDateInvoiced = ""; }

                try
                {
                    dblDollarsInvoiced = Convert.ToDouble(drInvoiced[3].ToString());
                    dblTotalDollarsInvoiced += Convert.ToDouble(drInvoiced[3].ToString());
                }
                catch
                {
                    dblDollarsInvoiced = 0;
                    dblTotalDollarsInvoiced += 0;
                }

                try
                {
                    dblTimeAndMaterial += Convert.ToDouble(drInvoiced[5].ToString());
                }
                catch
                {
                    dblTimeAndMaterial += 0;
                }
                try
                {
                    dblTimeAndMaterial += Convert.ToDouble(drInvoiced[6].ToString());
                }
                catch
                {
                    dblTimeAndMaterial += 0;
                }

                strDollarsInvoiced = dblDollarsInvoiced.ToString("C2");

                myDataTable.Rows.Add(strWO,
                                     strCustomerName,
                                     strDateInvoiced,
                                     strDollarsInvoiced,
                                     strInvoiceNumber);


            }
        }
        catch (Exception ex) { throw new Exception(ex.ToString()); }

        myDataTable.Rows.Add("",
                             "",
                             "",
                             "");

        myDataTable.Rows.Add("Total Dollars Invoiced:",
                             "",
                             "",
                             dblTotalDollarsInvoiced.ToString("C2"));
        myDataTable.Rows.Add("Total Dollars Invoiced / day:",
                             "",
                             "",
                             (dblTotalDollarsInvoiced / total_days).ToString("C2"));
        myDataTable.Rows.Add("Total Dollars T&M:",
                     "",
                     "",
                      dblTimeAndMaterial.ToString("C2"));

     

       


        GridView1.DataSource = myDataTable;
        GridView1.DataBind();
    }
    
   
}

