using System;
using System.Data;
using DevExpress.Web;
using nesi.core;

public partial class pm_reports : System.Web.UI.Page
    {
    public NeMember myMember;

    private const int _page_id = 189; // from Page table in DB
    private const string _page_description = "Reports / Project Manager Overview";
    private ASPxGridView grid = new ASPxGridView();
    private string startdate = "";
    private string enddate = "";
    private int businessUnitId;
    private string OverViewProjectManagerID;
    private string OverViewCustomerID;

    Toolbox _tools;

    protected void Page_Init(object sender, EventArgs e)
        {
        _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        }

    protected void Page_Load(object sender, EventArgs e)
        {
        if (!IsPostBack)
            {
            var now = DateTime.Today;
            var DateToday = now.ToString("yyyy-MM-dd");
            now = now.AddDays(-30);
            var DateOneMonthAgo = now.ToString("yyyy-MM-dd");
            txtSelectedDate.Text = DateOneMonthAgo;
            txtEndDate.Text = DateToday;
            var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
            }

        startdate = txtSelectedDate.Text;
        var DTstartdate = Convert.ToDateTime(startdate);
        startdate = DTstartdate.ToString("yyyy-MM-dd");
        enddate = txtEndDate.Text;
        var DTenddate = Convert.ToDateTime(enddate);
        enddate = DTenddate.ToString("yyyy-MM-dd");

        BusinessUnitOverView();
        }



    protected void BusinessUnitOverView()
        {

        var table = Toolbox.doSQL_dt(@"SELECT
    a.business_unit_id,
    ddl_name name,
    COUNT(*) AS Customer_Count,
    SUM(WOProg_LabourTotalSell) AS LABSUM,
    SUM(WOProg_MaterialTotalSell) AS MATSUM,
    SUM(WOProg_InvoicedNetTotal) AS INVSUM,
    (
        SUM(WOProg_LabourTotalSell) + SUM(WOProg_MaterialTotalSell) - SUM(WOProg_InvoicedNetTotal)
    ) AS OVERAGESUM
FROM
    WOPRog a,
    customer b,
    business_unit c
WHERE
     WOProg_InvoiceDate BETWEEN @v0 AND @v1
    AND WOProg_Customer_ID = Customer_ID
    AND a.business_unit_id = c.ID
GROUP BY business_unit_id
ORDER BY business_unit_id", new object[] {startdate, enddate});
        ASPxGridView1.DataSource = table;
        ASPxGridView1.DataBind();
        table.Dispose();


        }

    protected void ASPxGridView2_BeforePerformDataSelect(object sender, EventArgs e)
        {
        businessUnitId = (int) (sender as ASPxGridView).GetMasterRowKeyValue();
        }

    protected void ASPxGridView1_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {

        }

    protected void ASPxGridView2_Init(object sender, EventArgs e)
        {
        businessUnitId = (int) (sender as ASPxGridView).GetMasterRowKeyValue();
        grid = (ASPxGridView) sender;
        ProjectManagerOverView(grid);
        }

    protected void ProjectManagerOverView(ASPxGridView grid)
        {

        grid.DataSource = Toolbox.doSQL_dt(@"SELECT
    WOProg_PM_MemberID,
    member_fullname AS Member_Name,
    COUNT(*) AS Cust_Count,
    SUM(WOProg_LabourTotalSell) AS LABSUM,
    SUM(WOProg_MaterialTotalSell) AS MATSUM,
    SUM(WOProg_InvoicedNetTotal) AS INVSUM,
    (
        SUM(WOProg_LabourTotalSell) + SUM(WOProg_MaterialTotalSell) - SUM(WOProg_InvoicedNetTotal)
    ) AS OVERAGESUM
FROM
    WOPRog a,
    customer b ,
    member c
WHERE a.business_unit_id = @v0
AND WOProg_InvoiceDate BETWEEN @v1 AND @v2
AND WOProg_PM_MemberID = member_id
AND WOProg_Customer_ID = Customer_ID 
GROUP BY WOProg_PM_MemberID 
ORDER BY Member_Name
", new object[] {businessUnitId, startdate, enddate});
        ;
        grid.DataBind();
        }

    protected void ASPxGridViewCustomer_Init(object sender, EventArgs e)
        {
        OverViewProjectManagerID = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
        grid = (ASPxGridView) sender;
        CustomerOverView(grid);
        }

    protected void CustomerOverView(ASPxGridView grid)
        {
        grid.DataSource = Toolbox.doSQL_dt(@"SELECT
    woprog_customer_id,
    COUNT(*) AS custer_count,
    SUM(WOProg_LabourTotalSell) AS LABSUM,
    SUM(WOProg_MaterialTotalSell) AS MATSUM,
    SUM(WOProg_InvoicedNetTotal) AS INVSSUM,
    (
        SUM(WOProg_LabourTotalSell) + SUM(WOProg_MaterialTotalSell) - SUM(WOProg_InvoicedNetTotal)
    ) AS OVERAGESUM,
    urldecode (WOPRog_CustomerName) WOProg_CustomerName
FROM
    WOPRog a,
    customer b
    WHERE WOProg_InvoiceDate BETWEEN @v0 AND @v1
AND WOProg_PM_MemberID = @v2
AND a.business_unit_id = @v3
AND WOProg_Customer_ID = Customer_ID 
GROUP BY WOProg_Customer_ID 
ORDER BY WOProg_CustomerName", new object[] {startdate, enddate, OverViewProjectManagerID, businessUnitId});
        grid.DataBind();
        }

    protected void ASPxGridViewCustomer_BeforePerformDataSelect(object sender, EventArgs e)
        {
        OverViewProjectManagerID = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
        }

    protected void ASPxGridView2_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
        OverViewProjectManagerID = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
        }

    protected void ASPxGridViewWorkOrders_Init(object sender, EventArgs e)
        {
        var hold = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
        OverViewCustomerID = (sender as ASPxGridView).GetMasterRowKeyValue().ToString();
        grid = (ASPxGridView) sender;
        WorkOrderOverView(grid);
        }

    protected void WorkOrderOverView(ASPxGridView grid)
    { 


grid.DataSource = Toolbox.doSQL_dt(@"SELECT
    woprog_id,
    woprog_bvwo,
    WOProg_LabourTotalSell,
    WOProg_MaterialTotalSell,
    WOProg_InvoicedNetTotal,
    woprog_invoiceno,
    woprog_quoteid,
    (
        WOProg_LabourTotalSell + WOProg_MaterialTotalSell - WOProg_InvoicedNetTotal
    ) AS OVERAGESUM
FROM
    WOPRog
WHERE WOProg_InvoiceDate BETWEEN @v0
    AND @v1
    AND WOProg_Customer_ID = @v2
    AND WOProg_PM_MemberID = @v3
ORDER BY WOProg_BVWO", new object[] {startdate, enddate,OverViewCustomerID , OverViewProjectManagerID});
        grid.DataBind();

    }
}
