using System;
using System.Data;
using nesi.core;

public partial class sales_commision : System.Web.UI.Page
{
    public NeMember myMember;

    private const int _page_id = 74; // from Page table in DB
    private const string _privilege_id = "57";
    private const string _page_description = "Reports / Sales Commission";

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
            //ddlCompany.DataSource = NeBusinessUnit.units_active();
            ddlCompany.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);

            ddlCompany.DataBind();
            ddlCompany.SelectedValue = myMember.business_unit_id.ToString();

           // ddlCompany.Enabled = myMember.AuthenticatedForPrivilege(Convert.ToInt32(_privilege_id));

            var mlist = new NeMember();
            ddlPM.DataSource = mlist.LoadComMemberList(myMember.business_unit_id);
            ddlPM.DataTextField = "UserName";
            ddlPM.DataValueField = "id";
            ddlPM.DataBind();
            ddlPM.SelectedValue = myMember.id.ToString();


            txtSelectedDate.Text = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
            txtEndDate.Text = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        var mlist = new NeMember();
        ddlPM.DataSource = mlist.LoadComMemberList(Convert.ToInt32(ddlCompany.SelectedValue));
        ddlPM.DataTextField = "UserName";
        ddlPM.DataValueField = "id";
        ddlPM.DataBind();
        
    }
    protected void ddlPM_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        var strBVSQL1 = "";
        var strBVSQL2 = "";
        var strTransNo = "";
        var myDataTable = new DataTable();

        var strWONum = "";
        var strCustName = "";
        var strStatus = "";
        var strInvoiceNo = "";
        var strDateInvoice = "";
        var strInvoiceAmount = "";
        var strAmountPaid = "";
        var strTransNOSEL = "";
        var strFOLIO = "";

        var company = new NeBusinessUnit();
        var DSN = company.GetBUDSN(Convert.ToInt32(ddlCompany.SelectedValue));

        myDataTable.Columns.Add("WONum", typeof(string));
        myDataTable.Columns.Add("CustomerName", typeof(string));
        myDataTable.Columns.Add("Status", typeof(string));
        myDataTable.Columns.Add("InvoiceNo", typeof(string));
        myDataTable.Columns.Add("DateInvoiced", typeof(string));
        myDataTable.Columns.Add("InvoicedAmount", typeof(string));
        myDataTable.Columns.Add("AmountPaid", typeof(string));


        var tools = new Toolbox();

        var strWOSQL = "SELECT WOProg_BVWO, WOProg_Status, WOProg_InvoiceNo, WOProg_InvoiceDate, WOProg_CustomerName, WOPRog_InvoicedNetTotal FROM neintranet.woprog WHERE WOProg_SP_MemberID=" + ddlPM.SelectedValue;
        strWOSQL += " AND WOProg_InvoiceDate > '" + txtSelectedDate.Text + "' AND WOProg_InvoiceDate < '" + txtEndDate.Text + "'";
        var woprogdata = new DataTable();
        var arData = new DataTable();

        woprogdata = tools.getSQL_datatable(@"SELECT WOProg_BVWO, WOProg_Status, WOProg_InvoiceNo, WOProg_InvoiceDate, WOProg_CustomerName, WOPRog_InvoicedNetTotal FROM neintranet.woprog  WHERE WOProg_SP_MemberID=@v0", new object[] { ddlPM.SelectedValue });

        foreach (DataRow worow in woprogdata.Rows)
        {
            strWONum = worow[0].ToString();
            strCustName = worow[4].ToString();
            strStatus = worow[1].ToString();
            strInvoiceNo = "N/A";
            strDateInvoice = "N/A";
            strInvoiceAmount = "0.00";
            strAmountPaid = "0.00";
            if (worow[1].ToString() == "Invoiced")
            {
                strInvoiceNo = worow[2].ToString();
                strDateInvoice = worow[3].ToString().Substring(0,10);
                strInvoiceAmount = worow[5].ToString();
                strBVSQL1 = "SELECT TRANS_NO FROM SALES_HISTORY_HEADER WHERE NUMBER='" + worow[2] + "'";
               // try
               // {
               //     strTransNo = tools.getSQL_string(@"SELECT TRANS_NO FROM SALES_HISTORY_HEADER  WHERE NUMBER=?" , DSN, new object[] { worow[2].ToString() });
               // }
               // catch
               // {
               //     strTransNo = "";
               // }

               // if (strTransNo != "")
               // {
               //     strTransNOSEL = "SELECT FOLIO FROM AR_TRANSACTIONS WHERE TRANS_NO ='" + strTransNo + "'";
			   //
               //     try
               //     {
               //         strFOLIO = tools.getSQL_string(@"SELECT FOLIO FROM AR_TRANSACTIONS  WHERE TRANS_NO =?" , DSN, new object[] { strTransNo });
               //     }
               //     catch
               //     {
               //         strFOLIO = "";
               //     }
               //     strBVSQL2 = "SELECT D_AMOUNT, C_AMOUNT, BALANCE FROM AR_TRANSACTIONS WHERE FOLIO ='" + strFOLIO + "' AND CODE = 'P'";
               //     arData = tools.getSQL_datatable(@"SELECT D_AMOUNT, C_AMOUNT, BALANCE FROM AR_TRANSACTIONS  WHERE FOLIO =? AND CODE = 'P'", DSN, new object[] { strFOLIO });
               //     double fullammount = 0;
               //     foreach (DataRow bvrow in arData.Rows)
               //     {
               //         var Debit = Convert.ToDouble(bvrow[0].ToString());
               //         var Credit = Convert.ToDouble(bvrow[1].ToString());
               //         var ammount = Credit - Debit;
               //         fullammount += ammount;
               //         
               //     }
			   //
               //     strAmountPaid = fullammount.ToString();
               // }

            }

            myDataTable.Rows.Add(strWONum,
                                 strCustName,
                                 strStatus,
                                 strInvoiceNo,
                                 strDateInvoice,
                                 strInvoiceAmount,
                                 strAmountPaid);

        }

        GridView1.DataSource = myDataTable;
        GridView1.DataBind();
    }
}
