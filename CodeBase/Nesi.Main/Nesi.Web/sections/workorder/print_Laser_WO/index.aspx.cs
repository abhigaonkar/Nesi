using System;
using System.Collections.Specialized;
using nesi.core;
using nesi.core.print;

public partial class print_Laser_WO : System.Web.UI.Page
	{
	NeMember myMember;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(12);
		_tools.dont_cache_page();

		bindreport();
		if (!IsPostBack)
			{
			}
		}

	private void bindreport()
		{
		var _q		= Request.QueryString;
		var wo = _q["woprog_id"] != null 
						? new NeWOProg(Convert.ToInt32(_q["woprog_id"]))
						: new NeWOProg();
		var cust		= new NECustomer((int) wo.WOProg_Customer_ID);
		var print_map		= _q["map"] == "True";
		var print_safety	= _q["safety"] == "True";
		var print_quote	= _q["quote"] == "True";
		var print_picklist = _q["picklist"] == "True";
		var report = new rpt_WOLaser_Header(wo.woprog_id);
		report.Name			= wo.OrderNumber+" - "+cust.Customer_Name;
		report.CreateDocument();

		if(print_map)
			{
			var map_addition = new Map();
			map_addition.Parameters["printmap"].Value = print_map.ToString();
			var add = new NEAddress(wo.woprog_Address_ID);
			var addfull = string.Format("{0},{1},{2},{3},{4},{5}", add.Addr1, add.Addr2, add.Addr3, add.City, add.Prov, NeCountry.get_name(add.Country));
			map_addition.Parameters["address"].Value = addfull;
			map_addition.CreateDocument();
			report.Pages.AddRange(map_addition.Pages);
			}
		if(print_safety)
			{
			var safety_addition = new SafetyForm();
			safety_addition.CreateDocument();
			report.Pages.AddRange(safety_addition.Pages);
			}
		if (wo.QuoteID != "0" && wo.woprog_id != 0 && print_quote)
			{
			var quote = new quote(Convert.ToInt32(wo.QuoteID));
			var quote_addition = new quotemaster();
			quote_addition.Parameters[0].Value = Convert.ToInt32(quote.QuoteID);
			quote_addition.Parameters[1].Value = Convert.ToInt32(quote.Revision);
			quote_addition.Parameters[2].Value = ("WO");
			quote_addition.Parameters[3].Value = Convert.ToInt16(0);
            quote_addition.remitToAddress.Value = this.getRemitAddress(quote.business_unit_id);
			quote_addition.CreateDocument();
			report.Pages.AddRange(quote_addition.Pages);
			}
		if (wo.QuoteID != "0" && wo.woprog_id != 0 && print_picklist)
		{
		
			var sc = new woshoppingcart(Convert.ToInt32(wo.woprog_id));
			sc.Parameters[0].Value = Convert.ToInt32(wo.woprog_id);
			sc.Parameters[1].Value = 0;
			sc.CreateDocument();
			report.Pages.AddRange(sc.Pages);
			
		}
		viewer.Report = report;
		viewer.DataBind();
		}

    private string getRemitAddress(int businessUnitId)
    {
        return quotemaster.getRemitAddress(businessUnitId);
    }
}