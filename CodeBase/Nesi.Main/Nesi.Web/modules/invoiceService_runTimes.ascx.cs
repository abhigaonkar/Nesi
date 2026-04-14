using System;
using System.Data;
using DevExpress.Web;
using nesi.core;
using NESI.BLL.Pages.HomePage;

public partial class modules_invoiceService_runTimes : System.Web.UI.UserControl
{


    NeMember current_user;
    private Toolbox _tools;

    protected void Page_Load(object sender, EventArgs e)
    {
        invoiceServiceTimer.Enabled = true;
        _tools = new Toolbox();
        
        if (!this.Page.IsCallback)
        {
            fill_grid();
        }
    }
    protected void fill_grid()
    {
        var dt = new HomePageBase(null).GetInvoiceServiceRunTime();

        gv_invoice.DataSource = dt;
        gv_invoice.DataBind();
    }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
      
        if (e.Parameters != "")
        {
            if (e.Parameters == "refresh")
            {
                fill_grid();
            }
        }
        
    }
    
}