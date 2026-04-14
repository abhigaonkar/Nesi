using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.Text;
using nesi.core;
using System.IO;

public partial class sections_purchaseorder_po_prog : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id = 92; // from Page table in DB
	private const string _page_name = "PO_Summary_Grid";
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static string default_filter = "";
	SqlDataSource ds_templates;
	ASPxHiddenField h;
	Toolbox _tools;


	protected void Page_Init(object sender, EventArgs e)
	{



	    Response.Redirect("#/1/" + _page_id);
        Toolbox.do_debug("Page Init start");
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		_tools.add_css("/css/wo_prog.css");
		if (Session["po_working_business_unit_id"] == null)
		{
			Session["po_working_business_unit_id"] = myMember.business_unit_id.ToString();
		}

		layout.__page_name = _page_name;
		layout.used_gv = gv_posummary;
		//	default_filter = string.Format("page1|conditions2|1|3|4|3|visible14|t0|t1|t2|t3|t5|t6|t7|t8|t10|t11|t12|t13|t4|t9|width14|25px|100px|100px|50px|150px|75px|100px|100px|70px|75px|100px|100px|75px|50px");
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_posummary");
		ds_templates.SelectParameters["@page_name"].DefaultValue = string.Format("{0}", _page_name);
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		ods_companies.SelectParameters.Add(new Parameter("member_id", DbType.Int32, myMember.id32.ToString()));
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}
		Toolbox.do_debug("Page Init stop");
	}



	protected void Page_Load(object sender, EventArgs e)
	{
		Toolbox.do_debug("Page Load start");
		Response.Cache.SetCacheability(HttpCacheability.NoCache);
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
	    ddlcompany.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);
	    ddlcompany.DataBind();
	    ddlcompany.Items.Add("All", 0);
	    if (!IsPostBack)
	        {
	        ddlcompany.Value = 0;
     
        if ((myMember.business_unit.is_backoffice) || (myMember.business_unit.is_corporate))
	        {
	        ddlcompany.Value = 0;
	        }
   }
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		_tools.add_css("/css/wo_prog.css");
		divMenu.InnerHtml = menu.MenuHTML;
		_tools.dont_cache_page();
		SqlDataSource1.FilterExpression = myMember.business_unit_id == 11 ? "" : "nesi_cut_po = false";
        #region Load Summary Gridview
        if (!IsCallback && !IsPostBack)
		{

			var gl = new NeGridLayouts(myMember.id, string.Format("{0}", _page_name));
			var temp_company = new NeBusinessUnit(Session["po_working_business_unit_id"]);
			if (gl.GridLayoutID != 0)
			{
				gv_posummary.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
				gv_posummary.SettingsPager.Mode = GridViewPagerMode.ShowPager;
			}
			else
			{
				gv_posummary.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = default_filter;
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = Session["po_working_business_unit_id"] != null ? string.Format("{0}", _page_name) : string.Format("{0}", _page_name);
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
			}

		}
		#endregion



	
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = menu.PageDescription ?? "";

	    fill_top_summary();



        Toolbox.do_debug("Page Load stop");
	}


    protected void fill_top_summary()
        {
        var can_see = true;//myMember.AuthenticatedForPrivilege(_page_id, "59");
        double[] open_total = { 0, 0 };
        var removed_nesi_cut_po = myMember.business_unit_id == 11 ? "" : " AND nesi_cut_po = false";

        string bus =  ddlcompany.Value.ToString()=="0" ? new Current_User().visible_business_units : ddlcompany.Value.ToString();

        var poproginfo = _tools.getSQL_datatable(string.Format(@" 
SELECT 
	a.id, 
	a.ddl_name, 
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
	(SELECT COUNT(*) FROM poprog_header WHERE business_unit_id = a.id AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {0} ) total_n, 
	(
	SELECT 
		IFNULL(SUM(poprog_total_cost), 0)
	FROM 
		poprog_header 
	WHERE 
		business_unit_id = a.id AND 
		poprog_check_sent_date IS NULL 
	AND
		poprog_status NOT IN (4,6,7,8) {0} 
	) total_d,
	0 orderby,
 POPath ,
a.gl_div
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.id = @v1 AND
	b.public_name NOT LIKE 'Master %' 
UNION
SELECT 
	a.id, 
	a.ddl_name, 
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
	(SELECT COUNT(*) FROM poprog_header WHERE business_unit_id = a.id AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {0} ) total_n, 
	(
	SELECT 
		IFNULL(SUM(poprog_total_cost), 0)
	FROM 
		poprog_header 
	WHERE 
		business_unit_id = a.id AND 
		poprog_check_sent_date IS NULL 
	AND
		poprog_status NOT IN (4,6,7,8) {0} 
	) total_d, 
	1 orderby,
 POPath,
a.gl_div
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.id != @v1 AND
	b.public_name NOT LIKE 'Master %' 
ORDER BY 
	orderby, tax_entity_id, gl_div, ddl_name", removed_nesi_cut_po), new object[] { bus, myMember.business_unit.tax_entity_id , myMember.business_unit_id});
        if (!IsCallback)
            {
            var strList = new StringBuilder();
            double[] Topen_total = { 0, 0 };
            double[] TJustCut = { 0, 0 };
            double[] TQuestions = { 0, 0 };
            double[] TWaitingPS = { 0, 0 };
            double[] TWaitingIS = { 0, 0 };
            double[] TWaitingApproval = { 0, 0 };
            double[] TWaitingToBeIssued = { 0, 0 };
            double[] TProblems = { 0, 0 };
            double[] TWaitingPaid = { 0, 0 };
            double[] TWaitingConfirmation = { 0, 0 };
            var last_te_name = "";
            var popath = "";
            foreach (DataRow drCo in poproginfo.Rows)
                {
                
                var business_unit_id = drCo["id"].ToString();
                var name = drCo["ddl_name"].ToString();
                open_total[0] = Convert.ToDouble(drCo["total_n"]);
                open_total[1] = Convert.ToDouble(drCo["total_d"]);
                if (business_unit_id != "8")
                    {
                    Topen_total[0] += Convert.ToDouble(drCo["total_n"]);
                    Topen_total[1] += Convert.ToDouble(drCo["total_d"]);
                    }
                var te_name = drCo["tax_entity_name"].ToString();
                var dtTotals = _tools.getSQL_datatable(string.Format(@"SELECT COUNT(*) n, poprog_status po_status, SUM(poprog_total_cost) d FROM poprog_header WHERE business_unit_id = @v0 
AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {0}  GROUP BY po_status ", removed_nesi_cut_po), new object[] { business_unit_id });


                if (te_name != last_te_name)
                    {
                    strList.AppendFormat(@"
<tr>
	<td colspan='40' class='tax_entity'>
		{0}
	</td>
</tr>
", te_name);
                    last_te_name = te_name;

                }
                double[] JustCut = { 0, 0 };
                double[] Questions = { 0, 0 };
                double[] WaitingPS = { 0, 0 };
                double[] WaitingIS = { 0, 0 };
                double[] WaitingApproval = { 0, 0 };
                double[] WaitingToBeIssued = { 0, 0 };
                double[] WaitingPaid = { 0, 0 };
                double[] Problems = { 0, 0 };
                double[] WaitingConfirmation = { 0, 0 };

                foreach (DataRow _subdr in dtTotals.Rows)
                    {
                    var po_status = _subdr["po_status"].ToString();
                    var _n = Convert.ToDouble(_subdr["n"]);
                    var _d = Convert.ToDouble(_subdr["d"]);
                    switch (po_status)
                        {
                            case "1":
                                JustCut[0] = _n;
                                JustCut[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TJustCut[0] += _n;
                                    TJustCut[1] += _d;
                                    }
                                break;
                            case "2":
                                WaitingApproval[0] = _n;
                                WaitingApproval[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TWaitingApproval[0] += _n;
                                    TWaitingApproval[1] += _d;
                                    }
                                break;
                            case "3":
                                WaitingPS[0] = _n;
                                WaitingPS[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TWaitingPS[0] += _n;
                                    TWaitingPS[1] += _d;
                                    }
                                break;
                            case "5":
                                WaitingToBeIssued[0] = _n;
                                WaitingToBeIssued[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TWaitingToBeIssued[0] += _n;
                                    TWaitingToBeIssued[1] += _d;
                                    }
                                break;
                            case "9":
                                Questions[0] = _n;
                                Questions[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TQuestions[0] += _n;
                                    TQuestions[1] += _d;
                                    }
                                break;
                        case "10":
                            Problems[0] = _n;
                            Problems[1] = _d;
                            if (business_unit_id != "8")
                            {
                                TProblems[0] += _n;
                                TProblems[1] += _d;
                            }
                            break;
                        case "11":
                                WaitingConfirmation[0] = _n;
                                WaitingConfirmation[1] = _d;
                                if (business_unit_id != "8")
                                    {
                                    TWaitingConfirmation[0] += _n;
                                    TWaitingConfirmation[1] += _d;
                                    }
                                break;
                        }
                    }

                int x = 0;

                if (popath != drCo["POPath"].ToString())
                {
                    popath = drCo["POPath"].ToString();
                    try
                    {
                        DirectoryInfo dirJust = null;
                        dirJust = new DirectoryInfo(drCo["POPath"].ToString());
                        // For catching my test environment, otherwise I can't load this page due to not being able to access this scan path. -- Matt
                        var from_localhost = HttpContext.Current.Request.Url.Port == 80 &&
                                             HttpContext.Current.Request.Url.Host == "localhost";
                        x = dirJust.GetFiles("PO_NEEDED*.pdf").Length;

                        TProblems[0] += x;
                    }
                    catch { }
                }

                var strClass = business_unit_id == myMember.business_unit_id.ToString() ? "selected" : "";
                var their_branch = business_unit_id == myMember.business_unit_id.ToString() ? true : false;
                strList.AppendFormat(@"
<tr class='{0}'>
	<td class='action' style='width: 100px;'><button type='button' style='width: 60px' title='Start new purchase order' onclick=""location.href='po_prog_add.aspx?action=add&woprog_id=0&business_unit_id={1}'""><img src='/images/icon/icon[add].gif' width='16' height='16' />Add</button></td>
	<td class='branch'><a href=po_prog_edit.aspx?business_unit_id={1}>{2}</a></td>
	<td  class='total_n'>{3}</td>
	<td  class='total_d'>{16}</td>
	<td  class='{0}rn'>{4}</td>
	<td  class='{0}rd'>{10}</td>
	<td  class='{0}rn'>{17}</td>
	<td  class='{0}rd'>{18}</td>
	
	<td  class='{0}rn'>{6}</td>
	<td  class='{0}rd'>{12}</td>
<td  class='{0}rn'>{19}</td>
	<td  class='{0}rd'>{20}</td>
	<td  class='{0}rn'>{7}</td>
	<td  class='{0}rd'>{13}</td>
<td  class='{0}rn'>{5}</td>
	<td  class='{0}rd'>{11}</td>
	{8}
	{14}
	{9}
	{15}
<td  class='{0}rn'>{21}</td>
	<td  class='{0}rd'>{22}</td>
</tr>",
                    strClass, // {0}
                    business_unit_id, // {1}
                    name, // {2}
                    r(open_total[0], false, can_see, their_branch), // {3}
                    r(JustCut[0], false, can_see, their_branch), // {4}
                    r(Questions[0], false, can_see, their_branch), // {5}
                    r(WaitingToBeIssued[0], false, can_see, their_branch), // {6}
                    r(WaitingPS[0], false, can_see, their_branch), // {7}
                    "", // {8}
                    "", // {9}
                    r(JustCut[1], true, can_see, their_branch), // {10}
                    r(Questions[1], true, can_see, their_branch), // {11}
                    r(WaitingToBeIssued[1], true, can_see, their_branch), // {12}
                    r(WaitingPS[1], true, can_see, their_branch), // {13}
                    "", // {14}
                    "", // {15}
                    r(open_total[1], true, can_see, their_branch), // {16}
                    r(WaitingApproval[0], false, can_see, their_branch), // {17}
                    r(WaitingApproval[1], true, can_see, their_branch), //{18}
                    r(WaitingConfirmation[0], false, can_see, their_branch), // {19}
                    r(WaitingConfirmation[1], true, can_see, their_branch), //{20}
                    r(Problems[0] + x,false,can_see,their_branch),  // 21
                    r(Problems[1], true, can_see, their_branch)  //22
                );

                }

            var strtotalList = string.Format(@"
<tr style='font-weight: bold;height:30px;background-color:#BDEDFF;'>
	<td style='background-color:#fff;'></td>
	<td style='background-color:#fff;'></td>
	<td style='background-color:#69a;color:#fff;' >{3}</td>
	<td  >{16}</td>
	<td style='background-color:#69a;color:#fff;' >{4}</td>
	<td  >{10}</td>
<td style='background-color:#69a;color:#fff;'  >{17}</td>
	<td >{18}</td>
	
	<td style='background-color:#69a;color:#fff;' >{6}</td>
	<td >{12}</td>
<td style='background-color:#69a;color:#fff;' >{19}</td>
	<td  >{20}</td>
	<td style='background-color:#69a;color:#fff;' >{7}</td>
	<td  >{13}</td>
<td style='background-color:#69a;color:#fff;' >{5}</td>
	<td  >{11}</td>
	{8}
	{14}
	{9}
	{15}
<td style='background-color:#69a;color:#fff;' >{21}</td>
	<td  >{22}</td>
</tr>",
                "",                                                 // {0}
                0,                                                  // {1}
                0,                                              // {2}
                r(Topen_total[0], false, can_see, false),               // {3}
                r(TJustCut[0], false, can_see, false),              // {4}
                r(TQuestions[0], false, can_see, false),                // {5}
                r(TWaitingToBeIssued[0], false, can_see, false),        // {6}
                r(TWaitingPS[0], false, can_see, false),                // {7}
                "",                                                         // {8}
                "",         // {9}
                r(TJustCut[1], true, can_see, false),               // {10}
                r(TQuestions[1], true, can_see, false),                 // {11}
                r(TWaitingToBeIssued[1], true, can_see, false),         // {12}
                r(TWaitingPS[1], true, can_see, false),                 // {13}
                "",                                                         // {14}
                "",             // {15}
                r(Topen_total[1], true, can_see, false),                // {16}
                r(TWaitingApproval[0], false, can_see, false),
                r(TWaitingApproval[1], true, can_see, false),           //{18}
                r(TWaitingConfirmation[0], false, can_see, false),      // {19}
                r(TWaitingConfirmation[1], true, can_see, false),            //{20}
                r(TProblems[0], false, can_see, false),      // {19}
                r(TProblems[1], true, can_see, false)            //{20}
            );



            divCoSummary.InnerHtml = string.Format(@"
								<table cellpadding='0' cellspacing='0' id='wo_prog_summary'>
									<thead>
										<tr>
											<th  class='branch'></th>
											<th align='center'>Business Unit</th>
											<th  class='total' colspan='2'>Total</th>
											<th  colspan='2'>Just Cut</th>
											<th colspan='2'>Waiting for Approval</th>
											<th  colspan='2'>Waiting to be Issued</th>
                                            <th  colspan='2'>Waiting for Confirmation</th>
											<th  colspan='2'>Issued Waiting For Parts</th>
											<th  colspan='2'>Questions</th>
                                            <th  colspan='2'>AP Problems</th>
										</tr>
									</thead>
									<tbody>
										{0}
										{1}
									</tbody>
								</table>", strList, strtotalList);

            }
    }


	private static string r(double number, bool is_dollar, bool can_see, bool their_branch)
	{
		var _number = is_dollar ? number.ToString("C0") : number.ToString();
		var formatted_number = "";
		if (number > 0)
		{
			formatted_number = _number;
		}
		else if (number < 0)
		{
			formatted_number = string.Format("<font style='color:#f00;'>{0}</font>", _number);
		}
		else
		{
			formatted_number = string.Format("<font style='color:#999;'>{0}</font>", _number);
		}
		if (can_see || is_dollar == false || their_branch)
		{
			return formatted_number;
		}
		else
		{
			return "<font style='color:#999;'>--</font>";
		}
	}
	protected void cb_branches_DataBound(object sender, EventArgs e)
	{
		var c = (ASPxComboBox)sender;
		if (c.Items.FindByValue(0) == null)
		{
			c.Items.Insert(0, new ListEditItem("All Branches", 0));
		}
		c.SelectedItem = c.Items.FindByValue(Session["po_working_business_unit_id"]);
		Session["po_working_business_unit_id"] = c.Value;
	}
	protected void cb_company_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		Session["po_working_business_unit_id"] = e.Parameter;
	}
	protected void gv_posummary_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
	{


		try
		{
			var wo = Convert.ToInt32(gv_posummary.GetRowValues(e.VisibleIndex, "poprog_id"));


			var wwo = new NeWOProg(Convert.ToInt32(wo));

			wwo.print_barcode_label(1);

		}
		catch
		{
		}
	}
	protected void cb_branches_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["po_working_business_unit_id"] = cb_branches.Value.ToString();
		gv_posummary.DataBind();
	}
	protected void gv_posummary_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}

	}
	protected void gv_posummary_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_posummary_ClientLayout(object sender, DevExpress.Web.ASPxClientLayoutArgs e)
	{

	}
	protected void text_container_Init(object sender, EventArgs e)
	{
		var l = (ASPxLabel)sender;
		l.Attributes.Add("data-title", l.Text.Replace("\"", ""));
		l.CssClass = "ttip";
	}
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		try
		{
			var sql = "";
			var p = e.Parameter.Split('|');
			if (p[2] == "p")
			{
				_tools.getSQL_void(@"Update poprog_header set poprog_apstatus=@v0 where poprog_id =@v1", new object[] { Convert.ToInt32(p[1]), p[0] });
			}
			else if (p[2] == "n")
			{
				_tools.getSQL_void(@"Update poprog_header set poprog_hasproblem_notes=@v0 where poprog_id =@v1", new object[] { p[1], p[0] });
			}



		}
		catch
		{
			throw new Exception("Couldn't Update PO Header Table");
		}
	}
	protected void mem_Init(object sender, EventArgs e)
	{
		var dtecomp = sender as ASPxMemo;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;
		//dtecomp.Text = _tools.value_from(container.KeyValue);
		dtecomp.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetText() + '|n'); }}", container.KeyValue);


	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{

		var dtecomp = sender as ASPxComboBox;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;
		dtecomp.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetValue() + '|p'); }}", container.KeyValue);
	}

    protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
        {
        fill_top_summary();
        }

}

