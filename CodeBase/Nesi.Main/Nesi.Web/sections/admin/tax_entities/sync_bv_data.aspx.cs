using System;
using System.Data;
using nesi.core;
using System.Collections.Specialized;
//using nesi.bv;
using DevExpress.Xpo;
using System.Linq;

public partial class sync_bv_data : System.Web.UI.Page
{
    NeMember myMember;
    public Toolbox _tools = new Toolbox();
    private const int _page_id = 9; // from Page table in DB
    NameValueCollection _q;
    DataTable dt_glhistory;
    protected void Page_Init(object sender, EventArgs e)
    {
        _q = Request.QueryString; 
      
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        myMember = Toolbox.do_handle_authentication(_page_id);
        
    }

    protected void Sync_Taxes(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);


        Response.Write("Tax Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();
        using (var uow = new UnitOfWork())
        {
            var taxList = from t in new XPQuery<ne_xpo.cs.tax>(uow)
                          select t;
            foreach (var t in taxList)
            {
                var tax = new NeTax(t.tax_id);
        //        tax.Save_to_bv_from_MySQL(te.DSN);
            }
        }
        Response.Write("Tax Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();
    }

    protected void Sync_Terms(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);


        Response.Write("Terms Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();
      //  using (var bvConn = BVDB.connect(te.DSN))
        {
      //      NeAccounting.Terms.sync_bv(bvConn);
        }
        Response.Write("Terms Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();
    }

    protected void Sync_Customers(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);

        Response.Write("Customer Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();

     //   NECustomer.sync_bv_from_mysql(te.DSN, user);

        Response.Write("Customer Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

    }

    protected void Sync_Vendors(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);

        Response.Write("Vendor Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();

       // NEVendor.sync_bv_from_mysql(te.DSN, user);

        Response.Write("Vendor Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

    }

    protected void Sync_Addresses(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);

        Response.Write("Address Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();

      //  NEAddress.sync_S_adds_bv_from_mysql(te.DSN, user);

        Response.Write("Address Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

    }



    protected void Sync_All_Data(object sender, EventArgs e)
    {

        var te = new NeTaxEntity(_q[0]);
        var user = new NeMember(1316);


        Response.Write("Tax Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();
        using (var uow = new UnitOfWork())
        {
            var taxList = from t in new XPQuery<ne_xpo.cs.tax>(uow)
                          select t;
            foreach (var t in taxList)
            {
                var tax = new NeTax(t.tax_id);
               // tax.Save_to_bv_from_MySQL(te.DSN);
            }
        }

        Response.Write("Tax Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

        Response.Write("Terms Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();
     //   using (var bvConn = BVDB.connect(te.DSN))
        {
     //       NeAccounting.Terms.sync_bv(bvConn);
        }
        Response.Write("Terms Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

        Response.Write("Customer Sync Started:"+DateTime.Now+"<br />");
        Response.Flush();

     //   NECustomer.sync_bv_from_mysql(te.DSN, user);        

        Response.Write("Customer Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

        Response.Write("Vendor Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();

     //   NEVendor.sync_bv_from_mysql(te.DSN, user);

        Response.Write("Vendor Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();

        Response.Write("Address Sync Started:" + DateTime.Now + "<br />");
        Response.Flush();

      //  NEAddress.sync_S_adds_bv_from_mysql(te.DSN, user);

        Response.Write("Address Sync Completed:" + DateTime.Now + "<br /><br />");
        Response.Flush();
    }
}