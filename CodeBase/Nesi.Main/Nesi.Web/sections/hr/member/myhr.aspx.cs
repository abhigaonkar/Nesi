using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class member_myhr : Page
	{
	NeMember current_user;
	private const int _page_id = 168; // from Page table in DB
	private const string _page_name = "myhr";
	private Toolbox _tools = new Toolbox();
    NameValueCollection _q;

    protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		fvr_listing1.mid = current_user.id32;
		if (!IsPostBack)
			{
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
       

        if (!IsPostBack)
		{
		}
		load_header();
		var acceptedOfferId = _tools.getSQL_int(@"Select ifnull((Select a.id from member_offers a  where a.memberid =@v0 and a.status = 'Accepted'),0)", new object[] { current_user.id });
		var isCurrentOfferSigned = NeMemberOffer.GetSignedOfferFileID(acceptedOfferId);
		ASPxButton3.Enabled = isCurrentOfferSigned > 0;
		

		}

	    protected void load_header()
	        {
	        NeMember m = new NeMember(current_user.id32);
	        NeTaxEntity te = new NeTaxEntity(m.business_unit.tax_entity_id);
            NeMember reports_to = new NeMember(m.reports_to);
            NeMember payroll_handler = new NeMember(m.payroll_handler);
            string year_start = new DateTime(DateTime.Today.Year,1,1).ToString("yyyy-MM-dd");
	        

            div_stuff.InnerHtml = "<b>Employer Information: </b></br>";
	        div_stuff.InnerHtml += te.public_name + "</br>";
	        div_stuff.InnerHtml += m.business_unit.name + "</br>";
            div_stuff.InnerHtml += m.business_unit.address + "</br>";
	        div_stuff.InnerHtml += m.business_unit.city + ", " + m.business_unit.provstate + ", " + m.business_unit.country + "</br>";
	        div_stuff.InnerHtml += m.business_unit.postal + "</br>";
	        div_stuff.InnerHtml +=  "</br>";
	        div_stuff.InnerHtml += "<b>My Information:</b></br>";
	        div_stuff.InnerHtml += "My Title: " + m.membertype.name + "</br>";
        div_stuff.InnerHtml += "My address of record: " + m.Address + ", " + m.City + ", " + m.Prov + ", " + m.Country + ", " + m.PostalCode + "</br>";
	        div_stuff.InnerHtml += "My Supervisor: " + reports_to.FullName + " of " + reports_to.business_unit.name + ", " + reports_to.NECellPhoneNumber + ", " + reports_to.NEEmail + "</br>";
	        div_stuff.InnerHtml += "My Payroll Approver: " + payroll_handler.FullName + " of " + payroll_handler.business_unit.name + "   " + payroll_handler.NECellPhoneNumber + ", " + payroll_handler.NEEmail + "</br>";
	        div_stuff.InnerHtml += "Hours worked this year: " +
	                               _tools.getSQL_double("select get_sum_of_hours_worked(@v0,@v1,@v2)",
	                                   new object[] {m.id32, year_start, DateTime.Today.ToString("yyyy-MM-dd")}) + "</br>";
            div_stuff.InnerHtml += "Hours worked since my start date: " + _tools.getSQL_double("select get_sum_of_hours_worked(@v0,@v1,@v2)",
                                       new object[] { m.id32, "1972-01-01", DateTime.Today.ToString("yyyy-MM-dd") }) + "  -  Thank you!</br>";



    }

    protected void ASPxButton3_Click(object sender, EventArgs e)
	{
		var id = _tools.getSQL_int(@"Select ifnull((Select a.id from member_offers a LEFT JOIN business_unit b on a.business_unit_id = b.id  where a.memberid =@v0 and a.status = 'Accepted' order by a.id desc limit 1),0)", new object[] { current_user.id });
		if (id != 0)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('offer_print_off.aspx?moid=" + id + "','mo',700,900)", true);
		}
		
	}
	protected void ASPxHyperLink1_Init(object sender, EventArgs e)
	{

		var ddl = sender as ASPxHyperLink;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		var _type = gv.GetRowValuesByKeyValue(container.KeyValue, "type").ToString();
		if (_type == "ea")
		{
			ddl.ClientSideEvents.Click = "function(s,e){boing('offer_print_off.aspx?moid=" + container.KeyValue + "','mo',700,900)}";
		}
		else if (_type == "er")
		{
			ddl.ClientSideEvents.Click = "function(s,e){boing('print_review.aspx?rid=" + container.KeyValue + "&locked=1&is_worksheet=0&mid=0', 'printrev', 900, 900)}";
		}
	}
	protected void ASPxButton6_Click(object sender, EventArgs e)
	{
		fvr_listing1.Visible = false;
		gv.Visible = true;
		gv.DataSource = _tools.getSQL_datatable(@"Select id,date,concat('Employee Agreement - ',(Select membertype.membertype_name from membertype  where membertype_id =mo.membertypeid)) description, 'ea' type from member_offers mo where mo.memberid =@v0 and mo.status IN ('Previous','Past') order by date desc", new object[] { current_user.id });
		gv.DataBind();
		pop.ShowOnPageLoad = true;
	}
	protected void ASPxButton5_Click(object sender, EventArgs e)
	{
		gv.Visible = false;
		fvr_listing1.Visible = true;
		fvr_listing1.mid = current_user.id32;
		pop.ShowOnPageLoad = true;
	}
	protected void ASPxButton7_Click(object sender, EventArgs e)
	{
		fvr_listing1.Visible = false;
		gv.Visible = true;
		gv.DataSource = _tools.getSQL_datatable(@"Select id,date,concat('Employee Review by ', get_name(emp_review.reviewed_by_id)) description, 'er' type from emp_review  where emp_review.member_id =@v0 and (emp_review.status = 'Delivered') order by date desc", new object[] { current_user.id });
		gv.DataBind();
		pop.ShowOnPageLoad = true;
	}
    protected void ASPxButton8_Click(object sender, EventArgs e)
    {
        var id = _tools.getSQL_int(@"Select ifnull((Select a.id from member_offers a  where a.memberid =@v0 and a.status = 'Released' order by id desc limit 1),0)", new object[] { current_user.id });
        if (id != 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('offer_print_off.aspx?moid=" + id + "','mo',700,900)", true);
        }
    }
	protected void ASPxButton4_Click(object sender, EventArgs e)
	{
					ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('print_review.aspx?rid=0&locked=0&is_worksheet=1&mid=" + current_user.id + "', 'printrev', 900, 900)", true);
			
	}
	protected void ASPxButton2_Click(object sender, EventArgs e)
	{
		var id = _tools.getSQL_int(@"Select ifnull((Select id from member_offers  where memberid =@v0 and status = 'Accepted' order by id desc limit 1),0)", new object[] { current_user.id });
		if (id != 0)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('offer_print_off.aspx?moid=" + id + "&jd=1','mo',700,900)", true);
		}
	}

    protected void btn_print_er(object sender, EventArgs e)
    {
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('../../reports/print_employment_record/index.aspx?id=" + current_user.id32 + @"&_details=false','E_R',700,900)", true);
        

    }

    protected void btn_print_er_detailed(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('../../reports/print_employment_record/index.aspx?id=" + current_user.id32 + @"&_details=true','E_R',700,900)", true);
    }
}