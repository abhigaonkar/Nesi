using System;
using System.Data;
using System.IO;
using nesi.core;

public partial class sections_purchaseorder_po_prog_scans2 : System.Web.UI.Page
{
	private NePOProg po;
	
    protected void Page_Load(object sender, EventArgs e)
    {
        var poprogid = Request.QueryString["poprogid"];
		if(poprogid == "")
			{
			Toolbox.FriendlyException(Response, "Purchase Order ID not supplied", "history.go(-1)");
			}
		po			=new  NePOProg(Convert.ToInt32(poprogid));
        CreateTabedPages(poprogid);
        var _Tools = new Toolbox();
        try
        {
            Title = "Scans for PO - " + _Tools.getSQL_string(@"Select ifnull((Select Concat(Concat(POprog_BVpo,' - '),POProg_order_description) from poprog_header  where poprog_id =@v0), 'Unknown')", new object[] { poprogid });
        }
        catch
        {
            Title = "Scans for Unknown PO";
        }
    }

    protected void CreateTabedPages(string poid)
    {        
		
        var PackslipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 1", new object[] { poid });
        var InvoiceSlipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 2", new object[] { poid });
        var QuoteSlipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 3", new object[] { poid });
        var ReturnAuthorizationsSlipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 4", new object[] { poid });
        var OrderConfirmationSlipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 5", new object[] { poid });
		var ShippingDocumentsSlipScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 6", new object[] { poid });
		var APProblemsScans = Toolbox.doSQL_dt(@"SELECT * FROM poprog_scans  WHERE poprog_scans_poprog_id =@v0 AND poprog_scans_type = 7", new object[] { poid });

        var tabs = "";

        if (PackslipScans.Rows.Count == 0 && InvoiceSlipScans.Rows.Count == 0 
            && QuoteSlipScans.Rows.Count == 0 && ReturnAuthorizationsSlipScans.Rows.Count == 0
			&& OrderConfirmationSlipScans.Rows.Count == 0 && ShippingDocumentsSlipScans.Rows.Count == 0 && APProblemsScans.Rows.Count==0)
        {

            tabs = "<ul id=\"scantabs\" class=\"shadetabs\">\n";
            var i = 0;
            
                i++;
                tabs += "<li><a href=\"#\" rel=\"noscan\"> No Scan </a></li>\n";


           

            tabs += "</ul>\n";
            tabs += "<div style=\"border:1px solid gray; height=100%; margin-bottom: 1em; padding: 10px\">\n";

            
                tabs += "<div id=\"noscan\" class=\"tabcontent\" style=\"height:100%;\">\n";
                tabs += "<div>No available scans</div>\n";
                //tabs += strBigData;
                tabs += "</div>\n";
            

            
            tabs += "</div>\n";
            
            //Label1.Font.Bold = true;
            //tabs = "There Are No Scanned Packng Slips Or Invoices Associated With this Purchase Order";

        }
        else
        {
            tabs = "<ul id=\"scantabs\" class=\"shadetabs\">\n";
            var i = 0;
            foreach (DataRow psrow in PackslipScans.Rows)
            {
                i++;
                tabs += "<li><a href=\"#\" rel=\"" + psrow[0] + "\"> Packing Slip " + i + "</a></li>\n";
            }


            i = 0;
            foreach (DataRow isrow in InvoiceSlipScans.Rows)
            {
                i++;
                tabs += "<li><a href=\"#\" rel=\"" + isrow[0] + "\"> Invoice Slip " + i + "</a></li>\n";
            }

            i = 0;

            foreach (DataRow qsrow in QuoteSlipScans.Rows)
            {
                i++;
                tabs += "<li><a href=\"#\" rel=\"" + qsrow[0] + "\"> Quotes  " + i + "</a></li>\n";
            }
            i = 0;
            foreach (DataRow rarow in ReturnAuthorizationsSlipScans.Rows)
            {
                i++;
                tabs += "<li><a href=\"#\" rel=\"" + rarow[0] + "\"> Return Authorizations  " + i + "</a></li>\n";
            }
            i = 0;
            foreach (DataRow ocrow in OrderConfirmationSlipScans.Rows)
            {
                i++;
                tabs += "<li><a href=\"#\" rel=\"" + ocrow[0] + "\"> Order Confirmations  " + i + "</a></li>\n";
            }
			foreach (DataRow sdrow in ShippingDocumentsSlipScans.Rows)
			{
				i++;
				tabs += "<li><a href=\"#\" rel=\"" + sdrow[0] + "\"> Shipping Documents  " + i + "</a></li>\n";
			}
			foreach (DataRow APProw in APProblemsScans.Rows)
			{
				i++;
				tabs += "<li><a href=\"#\" rel=\"" + APProw[0] + "\"> AP Problems  " + i + "</a></li>\n";
			}
            tabs += "</ul>\n";
            tabs += "<div style=\"border:1px solid gray; height=100vh; margin-bottom: 1em; padding: 10px\">\n";

            foreach (DataRow psrow in PackslipScans.Rows)
            {                
                var poprog = new NePOProg(int.Parse (psrow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);

                tabs += "<div id=\"" + psrow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + psrow[0] + ");} return false;\">Delete Scan</button>";
                tabs += "<embed src=\""+fileServer + @"/POs/named_scans/" + psrow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
                //tabs += strBigData;
                tabs += "</div>\n";
            }

            foreach (DataRow isrow in InvoiceSlipScans.Rows)
            {
                var poprog = new NePOProg(int.Parse(isrow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + isrow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + isrow[0] + ");} return false;\">Delete Scan</button>";
                tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + isrow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
                //tabs += strBigData;
                tabs += "</div>";
            }
            foreach (DataRow qsrow in QuoteSlipScans.Rows)
            {
                var poprog = new NePOProg(int.Parse(qsrow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + qsrow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + qsrow[0] + ");} return false;\">Delete Scan</button>";
                tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + qsrow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
                //tabs += strBigData;
                tabs += "</div>";

            }

            foreach (DataRow rarow in ReturnAuthorizationsSlipScans.Rows)
            {
                var poprog = new NePOProg(int.Parse(rarow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + rarow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + rarow[0] + ");} return false;\">Delete Scan</button>";
                tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + rarow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
                //tabs += strBigData;
                tabs += "</div>";

            }
			
			foreach (DataRow ocrow in OrderConfirmationSlipScans.Rows)
			{
                var poprog = new NePOProg(int.Parse(ocrow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + ocrow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + ocrow[0] + ");} return false;\">Delete Scan</button>";
				tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + ocrow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
				//tabs += strBigData;
				tabs += "</div>";

			}
			foreach (DataRow sdrow in ShippingDocumentsSlipScans.Rows)
			{
                var poprog = new NePOProg(int.Parse(sdrow["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + sdrow[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + sdrow[0] + ");} return false;\">Delete Scan</button>";
				tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + sdrow[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
				//tabs += strBigData;
				tabs += "</div>";
			}
			foreach (DataRow APProw in APProblemsScans.Rows)
			{
                var poprog = new NePOProg(int.Parse(APProw["poprog_scans_poprog_id"].ToString()));
                var fileServer = NeTaxEntity.BaseFolder(poprog.business_unit_id, true);
                tabs += "<div id=\"" + APProw[0] + "\" class=\"tabcontent\" style=\"height:100vh;\">\n";
				tabs += "<button type='button' onclick=\"if(confirm('Are you sure you want to delete this scan?')){DeleteScan(" + APProw[0] + ");} return false;\">Delete Scan</button>";
				tabs += "<embed src=\"" + fileServer + @"/POs/named_scans/" + APProw[0] + ".pdf\" width=\"100%\" height=\"150%\">\n";
				//tabs += strBigData;
				tabs += "</div>";
			}
            tabs += "</div>\n";
        }
        Label1.Text = tabs;
    }
	[System.Web.Services.WebMethod]
	public static void DeleteScan(string scanid)
		{
		var poprog_id		= Toolbox.doSQL_int(@"SELECT poprog_scans_poprog_id FROM poprog_scans  WHERE poprog_scans_id =@v0", new object[] { scanid });
		var poprog			= new NePOProg(poprog_id);
		var fileServer		= NeTaxEntity.BaseFolder(poprog.business_unit_id, false);
		var tools = new Toolbox();
		try
			{
			//Delete The Scan Entry in the database
			tools.getSQL_void(@"DELETE FROM poprog_scans WHERE poprog_scans_id = @v0  LIMIT 1", new object[] { scanid });
			var path		= fileServer + @"\POs\named_scans\" + scanid + ".pdf";
			if(File.Exists(path))
				{
				File.Delete(path);
				}
			}
		catch (Exception ex)
			{
			tools.catch_error(ex);
			throw;
			}
		}
}
