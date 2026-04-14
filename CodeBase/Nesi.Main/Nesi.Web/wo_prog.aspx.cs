using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.Services;
using NESI.Common.Models;
using nesi.core;

public partial class wo_prog : System.Web.UI.Page
{
	NeMember myMember;

	private const int _page_id = 12; // from Page table in DB
	private const string _page_name = "WO_Summary_Grid";

	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static string default_filter = "";
	SqlDataSource ds_templates;
	ASPxHiddenField h;
	Toolbox _tools;
	string dsn = "";
	bool can_view_margin = false;
	bool can_view_total_TM = false;
	bool can_cut_WO = false;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();


	    Response.Redirect("#/home/12/workorder");

        var _q = Request.QueryString;
		myMember = Toolbox.do_handle_authentication(_page_id);
		ods_companies.SelectParameters.Add(new Parameter("member_id", DbType.Int32, myMember.id32.ToString()));

		((IntraDefault)this.Master).page_name = NePage.get_page_name(_page_id);
		dsn = new NeBusinessUnit(myMember.business_unit_id).DSN;
		layout.used_gv = gv_wosummary;
		if (!string.IsNullOrEmpty(_q["a"]))
		{
			switch (_q["a"])
			{
				case "set_active_branch":
					Session["working_business_unit_id"] = _q["business_unit_id"];
					Toolbox.QuickReponse(Response, "SUCCESS");
					Response.End();
					break;
			}
		}
		_tools.add_css("/css/wo_prog.css?idx="+Toolbox.do_RandomString(10));
		if (Session["working_business_unit_id"] == null)
		{
			Session["working_business_unit_id"] = myMember.business_unit_id.ToString();
		}

		layout.__page_name = _page_name;
		default_filter = string.Format("page1|conditions2|1|3|4|3|visible14|t0|t1|t2|t3|t5|t6|t7|t8|t10|t11|t12|t13|t4|t9|width14|25px|100px|100px|50px|150px|75px|100px|100px|70px|75px|100px|100px|75px|50px");
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_wosummary");
		can_view_margin = myMember.AuthenticatedForPrivilege(81);
		can_view_total_TM = myMember.AuthenticatedForPrivilege(60);
		can_cut_WO = myMember.AuthenticatedForPrivilege(25);
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		gv_wosummary.Columns["woprog_stilltobebilled"].Visible = can_view_total_TM;
		gv_wosummary.Columns["woprog_stilltobebilled"].ShowInCustomizationForm = can_view_total_TM;
		gv_wosummary.Columns["margin"].Visible = can_view_margin;
		gv_wosummary.Columns["margin"].ShowInCustomizationForm = can_view_margin;
		if (!can_view_margin)
		{
			layout.ShowExcelExport = false;
			layout.ShowPDFExport = false;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		using (var conn = Toolbox.connect())
		{
			var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			fill_gv_summary();
			if (menu.PageDescription != null)
			{
				lbltemp.Text = menu.PageDescription;
				//	lblcb_branches.Visible = false;
				//	cb_branches.ClientVisible = false;

			}
			var custid = "%";
			if (myMember.customerID != 0)
			{
				custid = myMember.customerID.ToString();
			}
			ddlcompany.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);
			ddlcompany.DataBind();
			ddlcompany.Items.Add("All", 0);

			if (!IsPostBack)
			{
				ddlcompany.Value = 0;

				#region Load Summary Gridview
				if (!IsCallback && !IsPostBack)
				{
					var gl = new NeGridLayouts(myMember.id, _page_name);
					var temp_company = new NeBusinessUnit(Convert.ToInt32(Session["working_business_unit_id"]));
					if (gl.GridLayoutID != 0)
					{
						gv_wosummary.LoadClientLayout(gl.GridLayout_Layout);
						h.Set("ID", gl.GridLayoutID);
						h.Set("NAME", gl.GridLayout_Name);
						dde_filter.Text = gl.GridLayout_Name;
						gv_wosummary.SettingsPager.Mode = GridViewPagerMode.ShowPager;
					}
					else
					{
						gv_wosummary.LoadClientLayout(default_filter);
						gl.GridLayout_Layout = default_filter;
						gl.member_id = myMember.id;
						gl.GridLayout_Name = "Default";
						gl.GridLayout_Gridid = string.Format("{0}", _page_name);
						gl.SaveGridLayout();

						h.Set("ID", gl.GridLayoutID);
						h.Set("NAME", gl.GridLayout_Name);
						dde_filter.Text = gl.GridLayout_Name;
					}

				}
				#endregion

			    if ((myMember.business_unit.is_backoffice)||(myMember.business_unit.is_corporate))
			        {
			        ddlcompany.Value = 0;
			        }

			    }
			if (myMember.isContact)
			{
				panel_export.Visible = false;
				gv_wosummary.Columns["Customer Name"].Visible = false;
				gv_wosummary.Columns["Still to be Billed"].Visible = false;

				gv_wosummary.Columns[0].Visible = false;
				var cust = new NECustomer(Convert.ToInt32(myMember.customerID)).Customer_Name;
				gv_wosummary.FilterExpression = "woprog_customername = '" + cust + "' and woprog_vis_to_cust=1 ";

			}
			fill_top_summary();
		}


	}

	private void fill_top_summary()
	{
		var custid = "%";
		if (myMember.customerID != 0)
		{
			custid = myMember.customerID.ToString();
		}
		using (var conn = Toolbox.connect())
		{
			#region load summary table
			if (can_cut_WO)  // if member can cut a wo, show them the top table
			{

				var visible_companies = ddlcompany.Value.ToString() == "0" ? new Current_User().visible_business_units : ddlcompany.Value.ToString();
                // TODO : May need to add another privilege to add WO
				var _companyies = Toolbox.doSQL_dt(conn, @"
SELECT
	a.id,
	a.name,
	a.ddl_name ,
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
	b.dsn AS dsn,
	a.wopath AS path,
	IFNULL((SELECT COUNT(b.woprog_id) FROM WOProg b WHERE  b.business_unit_id = a.id AND b.WOProg_CloseDateTime IS NULL AND b.woprog_bvwo != 'Not Entered' AND b.woprog_status NOT IN ('Invoiced', 'Deleted') and b.woprog_customer_id like '" + custid + @"'), 0) AS total,
	IFNULL((SELECT SUM(c.WOProg_StillToBeBilled) FROM WOProg c WHERE c.business_unit_id = a.id AND c.WOProg_CloseDateTime IS NULL AND c.woprog_bvwo != 'Not Entered' AND c.woprog_status NOT IN ('Invoiced','Deleted')), 0) AS dollars,
	0 orderby
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.public_name NOT LIKE 'Master %' AND
	a.tax_entity_id = @v1
UNION
SELECT
	a.id,
	a.name,
	a.ddl_name ,
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
	b.dsn AS dsn,
	a.wopath AS path,
	IFNULL((SELECT COUNT(b.woprog_id) FROM WOProg b WHERE  b.business_unit_id = a.id AND b.WOProg_CloseDateTime IS NULL AND b.woprog_bvwo != 'Not Entered' AND b.woprog_status NOT IN ('Invoiced', 'Deleted') and b.woprog_customer_id like '" + custid + @"'), 0) AS total,
	IFNULL((SELECT SUM(c.WOProg_StillToBeBilled) FROM WOProg c WHERE c.business_unit_id = a.id AND c.WOProg_CloseDateTime IS NULL AND c.woprog_bvwo != 'Not Entered' AND c.woprog_status NOT IN ('Invoiced','Deleted')), 0) AS dollars,
	1 orderby
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.public_name NOT LIKE 'Master %' AND
	a.tax_entity_id != @v1
ORDER BY  
	orderby, tax_entity_id, ddl_name", new object[] { visible_companies, myMember.business_unit.tax_entity_id,myMember.business_unit_id });


				double[] GrandTotal = { 0, 0 };
				double[] GrandOpenTotal = { 0, 0 };
				double[] GrandOnHold = { 0, 0 };
				double[] GrandPMTotals = { 0, 0 };
				double[] GrandBMTotals = { 0, 0 };
				double[] GrandWInvTotals = { 0, 0 };
				double[] GrandQuestTotals = { 0, 0 };
				double[] GrandRework = { 0, 0 };
				double[] GrandWaitingForPO = { 0, 0 };
				double[] GrandInProgress = { 0, 0 };
				double[] GrandOpenPO = { 0, 0 };
				var grandjustscan = 0;
				var summary = new StringBuilder();
				summary.Append(@"
												<table cellpadding='0' cellspacing='0' id='wo_prog_summary'>
													<thead>
														<tr>
															<th>&nbsp;</th>
<th colspan='1'>Just<br/>Scanned</th>
															<th style='text-align:left;'>Business Unit</th>
															<th colspan='2' class='total'>Total</th>
															<th colspan='2'>Open</th>
															<th colspan='2'>On Hold</th>
															<th colspan='2'>Being<br/>Processed</th>
															
															<th colspan='2'>Init<br/>Prep</th>
															<th colspan='2'>Open<br/>PO's</th>
															<th colspan='2'>Rework</th>
															<th colspan='2'>Questions</th>
															<th colspan='2'>PM<br/>Approval</th>
															<th colspan='2'>BM<br/>Approval</th>
															<th colspan='2'>To Be<br/>Invoiced</th>
															<th colspan='2'>Waiting<br/>Cust PO</th>
														</tr>
													</thead>
													<tbody>");

				var strClass = "";
				var temp_comp = "";
				var show_comp_header = true;
				var last_te_name = "";
				foreach (DataRow _company in _companyies.Rows)
				{
					var business_unit_id = _company["id"].ToString();
					if (business_unit_id != temp_comp)
					{
						show_comp_header = true;
						temp_comp = business_unit_id;
					}
					else
					{
						show_comp_header = false;
					}

					strClass = business_unit_id == myMember.business_unit_id.ToString() ? "selected" : "";
					if ((business_unit_id == "8" || business_unit_id == "11") && myMember.isContact)
					{
						continue;
					}

					var name = _company["ddl_name"].ToString();
                    var te_name = _company["tax_entity_name"].ToString();
					if(te_name != last_te_name)
						{
						summary.AppendFormat(@"
<tr>
	<td colspan='40' class='tax_entity'>
		{0}
	</td>
</tr>
", te_name);
						last_te_name = te_name;
						}
					var company_path = _company["path"].ToString();
					var c = Convert.ToInt32(business_unit_id);
					double[] Total = { 0, 0 };
					double[] OpenPO = { 0, 0 };
					var po_list = "";
					var open_pos = new DataTable();
					try
					{
						//open_pos = _tools.getSQL_datatable(@"SELECT DISTINCT(CONVERT(VEND_PO, SQL_INTEGER)) VEND_PO FROM PURCHASE_ORDER_HEADR  WHERE STATUS IN ('H', '0', 'S') AND VEND_PO >= '0' AND VEND_PO < ':'", company_dsn , null);
					}
					catch { }
					var NewOpenPOs = string.Format(@"
SELECT 
	DISTINCT(CAST(woprog_bvwo AS SIGNED)) 
FROM 
	poprog_header, 
	po_details_current, 
	woprog 
WHERE 
	poprog_status IN (1,2,5,3,9) AND 
	poprog_header.business_unit_id = {0} AND 
	poprog_id = po_details_poprog_id AND 
	po_details_line_active = TRUE AND 
	po_details_woprog_id = woprog_id and
    po_details_current.is_gl_account = false
    and
	woprog_customer_id Like '{1}' ", business_unit_id, custid);
					var NewOpenPOsTable = Toolbox.doSQL_dt(conn, NewOpenPOs, null);
					if (open_pos.Rows.Count > 0 || NewOpenPOsTable.Rows.Count > 0)
					{
						//foreach (DataRow _po in open_pos.Rows)
						//	{
						//	string po = _po["VEND_PO"].ToString();
						//	po_list += po + ",";
						//	}

						foreach (DataRow _newpo in NewOpenPOsTable.Rows)
						{
							var newpo = _newpo[0].ToString();
							po_list += newpo + ",";
						}

						po_list = po_list.TrimEnd(',');
						//SELECT COUNT(*) n, IFNULL(SUM(WOProg_StillToBeBilled), 0) d FROM WOProg WHERE business_unit_id =2 AND WOProg_CloseDateTime IS NULL AND WOProg_Status NOT IN ('Open', 'Hold') AND CAST(WOProg_BVWO as DECIMAL(10)) IN (18767, 18910, 18910, 18910, 18910, 18910, 18910, 18910, 18910, 18910, 18910, 18913, 18927, 18947, 18947, 18952, 18955, 18964, 18965, 18966, 18966, 18968, 18973, 18976, 18980, 18980, 18980, 18982)
						open_pos = Toolbox.doSQL_dt(conn, string.Format(@"SELECT COUNT(*) n, IFNULL(SUM(WOProg_StillToBeBilled), 0) d 
FROM WOProg WHERE business_unit_id = @v0  AND WOProg_CloseDateTime IS NULL AND WOProg_Status != 'Open' 
AND WOProg_Hold = 0 AND CAST(WOProg_BVWO as DECIMAL(10)) IN ({0})", po_list), new object[] { business_unit_id });
						foreach (DataRow _po in open_pos.Rows)
						{

							OpenPO[0] = Convert.ToDouble(_po["n"]);
							OpenPO[1] = Convert.ToDouble(_po["d"]);
							GrandOpenPO[0] += c != 8 ? Convert.ToDouble(_po["n"]) : 0;
							GrandOpenPO[1] += c != 8 ? Convert.ToDouble(_po["d"]) : 0;
						}
					}

					Total[0] = Convert.ToDouble(_company["total"]);
					Total[1] = Convert.ToDouble(_company["dollars"]);
					GrandTotal[0] += c != 8 ? Convert.ToDouble(_company["total"]) : 0;
					GrandTotal[1] += c != 8 ? Convert.ToDouble(_company["dollars"]) : 0;
					DirectoryInfo dirJust = null;
					string strJustScanned;
					FileInfo[] arrFiles;
					if (!myMember.isContact && show_comp_header)
					{
						try
						{
							if(Directory.Exists(company_path))
								{
								dirJust = new DirectoryInfo(company_path);
								arrFiles = dirJust.GetFiles("*.pdf");
								strJustScanned = arrFiles.Length.ToString();
								grandjustscan += arrFiles.Length;
								}
							else
								{
								strJustScanned = "N/A";
								}

						}
						catch (Exception ee)
						{
							strJustScanned = "N/A";
							Toolbox.do_debug_note(ee);
						}
					}
					else
					{
						strJustScanned = "";
					}

					var dtTotals = Toolbox.doSQL_dt(conn, @" SELECT COUNT(*) AS n, if(woprog_hold = 1, 'Hold',woprog_status) AS status, 
SUM(woprog_stilltobebilled) d FROM woprog WHERE woprog.business_unit_id = @v0  and woprog_closedatetime IS NULL AND woprog_bvwo != 'Not Entered' 
AND woprog_customer_id Like @v1  AND  woprog_status NOT IN ('Deleted', 'Waiting Parent PM Approval', 'Waiting for Parts') GROUP BY status", new object[] { business_unit_id, custid });
					/* Current statuses: 
                    -- Not used -- 'Deleted',
                    'Initial Prep',
                    'Invoiced',
                    'Open',
                    'Questions For PM',
                    'Rework',
                    'Waiting BM Approval',
                    'Waiting for Parts',
                    'Waiting For PO',
                    'Waiting PM Approval',
                    'Waiting Parent BM Approval',
                    'Waiting To Be Invoiced' 
                     */
					double[] PMTotals = { 0, 0 };
					double[] BMTotals = { 0, 0 };
					double[] WInvTotals = { 0, 0 };
					double[] QuestTotals = { 0, 0 };
					double[] OpenTotal = { 0, 0 };
					double[] OnHold = { 0, 0 };
					double[] Rework = { 0, 0 };
					double[] WaitingForPO = { 0, 0 };
					double[] InProgress = { 0, 0 };
					double[] open_vendor_pos = { 0, 0 };
					foreach (DataRow _dr in dtTotals.Rows)
					{
						var status = _dr["status"].ToString();
						var total = Convert.ToDouble(_dr["n"]);
						var dollars = Convert.ToDouble(_dr["d"]);
						switch (status)
						{
							case OpsWOStatus.Open:
								OpenTotal[0] = total;
								OpenTotal[1] = dollars;
								GrandOpenTotal[0] += c != 8 ? total : 0;
								GrandOpenTotal[1] += c != 8 ? dollars : 0;
								break;
							case "Waiting PM Approval":
								PMTotals[0] = total;
								PMTotals[1] = dollars;
								GrandPMTotals[0] += c != 8 ? total : 0;
								GrandPMTotals[1] += c != 8 ? dollars : 0;
								break;
							case "Waiting BM Approval":
								BMTotals[0] = total;
								BMTotals[1] = dollars;
								GrandBMTotals[0] += c != 8 ? total : 0;
								GrandBMTotals[1] += c != 8 ? dollars : 0;
								break;
							case OpsWOStatus.WaitingToBeInvoiced:
								WInvTotals[0] = total;
								WInvTotals[1] = dollars;
								GrandWInvTotals[0] += c != 8 ? total : 0;
								GrandWInvTotals[1] += c != 8 ? dollars : 0;
								break;
							case "Questions For PM":
								QuestTotals[0] = total;
								QuestTotals[1] = dollars;
								GrandQuestTotals[0] += c != 8 ? total : 0;
								GrandQuestTotals[1] += c != 8 ? dollars : 0;
								break;
							case "Hold":
								OnHold[0] = total;
								OnHold[1] = dollars;
								GrandOnHold[0] += c != 8 ? total : 0;
								GrandOnHold[1] += c != 8 ? dollars : 0;
								break;
							case "Rework":
								Rework[0] = total;
								Rework[1] = dollars;
								GrandRework[0] += c != 8 ? total : 0;
								GrandRework[1] += c != 8 ? dollars : 0;
								break;
							case "Waiting For PO":
								WaitingForPO[0] = total;
								WaitingForPO[1] = dollars;
								GrandWaitingForPO[0] += c != 8 ? total : 0;
								GrandWaitingForPO[1] += c != 8 ? dollars : 0;
								break;
							case "Initial Prep":
								InProgress[0] = total;
								InProgress[1] = dollars;
								GrandInProgress[0] += c != 8 ? total : 0;
								GrandInProgress[1] += c != 8 ? dollars : 0;
								break;
							case "In Progress":
								InProgress[0] = total;
								InProgress[1] = dollars;
								GrandInProgress[0] += c != 8 ? total : 0;
								GrandInProgress[1] += c != 8 ? dollars : 0;
								break;
						}
					}
					//if(business_unit_id != "8")
					summary.AppendFormat(@"
										<tr class='{11}'>
											<td class='action' style='width: 75px;' ><button type='button' style='width: 60px; visibility:{28}'  title='Start new work order' onclick=""location.href='sections/workorder/index.aspx?woprog_id=0&business_unit_id={0}'""><img src='/images/icon/icon[add].gif' width='16' height='16' />Add</button></td>
                                            <td data-colname='scanned' class='{11}rd'>{5}</td>
											<td class='branch'><a href='wo_prog_edit.aspx?business_unit_id={0}'>{1}</a></td>
											<td class='{11}total_n'>{2}</td>
											<td class='{11}total_d'>{19}</td>
											<td data-colname='open' class='{11}n'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Open'>{3}</td>
											<td data-colname='open' class='{11}d'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Open'>{12}</td>
											<td data-colname='onhold' class='{11}hold_n'>{10}</td>
											<td data-colname='onhold' class='{11}hold_d'>{18}</td>
											<td data-colname='process' class='{11}rn'>{4}</td>
											<td data-colname='process' class='{11}rd'>{13}</td>
											
											<td data-colname='inprogress' class='{11}rn'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=In%20Progress'>{24}</a></td>
											<td data-colname='inprogress' class='{11}rd'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=In%20Progress'>{25}</a></td>
											<td data-colname='openpo' class='{11}rn'>{26}</td>
											<td data-colname='openpo' class='{11}rd'>{27}</td>
											<td data-colname='rework' class='{11}rn'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Rework'>{20}</a></td>
											<td data-colname='rework' class='{11}rd'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Rework'>{21}</a></td>
											<td data-colname='questions' class='{11}rn'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Questions%20For%20PM'>{9}</a></td>
											<td data-colname='questions' class='{11}rd'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Questions%20For%20PM'>{17}</a></td>
											<td data-colname='pmapproval' class='{11}rn'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Waiting%20PM%20Approval'>{6}</a></td>
											<td data-colname='pmapproval' class='{11}rd'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Waiting%20PM%20Approval'>{14}</a></td>
											<td data-colname='bmapproval' class='{11}rn'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Waiting%20BM%20Approval'>{7}</a></td>
											<td data-colname='bmapproval' class='{11}rd'><a href='/sections/reports/master_workorders/index.aspx?business_unit_id={0}&status=Waiting%20BM%20Approval'>{15}</a></td>
											<td data-colname='invoiced' class='{11}rn'>{8}</td>
											<td data-colname='invoiced' class='{11}rd'>{16}</td>
											<td data-colname='custpo' class='{11}rn'>{22}</td>
											<td data-colname='custpo' class='{11}rd'>{23}</td>
										</tr>",
											  business_unit_id, //  {0}
											 show_comp_header ? name : "", //  {1}
											r(Total[0], false, can_view_total_TM), //  {2}
											r(OpenTotal[0], false, can_view_total_TM), //  {3}
											r(Total[0] - OpenTotal[0] - OnHold[0], false, can_view_total_TM), //  {4}
											strJustScanned, //  {5}
											r(PMTotals[0], false, can_view_total_TM), //  {6}
											r(BMTotals[0], false, can_view_total_TM), //  {7}
											r(WInvTotals[0], false, can_view_total_TM), //  {8}
											r(QuestTotals[0], false, can_view_total_TM), //  {9}
											r(OnHold[0], false, can_view_total_TM), // {10}
											strClass, // {11}
											r(OpenTotal[1], true, can_view_total_TM), // {12}
											r(Total[1] - OpenTotal[1] - OnHold[1], true, can_view_total_TM), // {13}
											r(PMTotals[1], true, can_view_total_TM), // {14}
											r(BMTotals[1], true, can_view_total_TM), // {15}
											r(WInvTotals[1], true, can_view_total_TM), // {16}
											r(QuestTotals[1], true, can_view_total_TM), // {17}
											r(OnHold[1], true, can_view_total_TM), // {18}
											r(Total[1], true, can_view_total_TM), // {19}
											r(Rework[0], false, can_view_total_TM), // {20}
											r(Rework[1], true, can_view_total_TM), // {21}
											r(WaitingForPO[0], false, can_view_total_TM), // {22}
											r(WaitingForPO[1], true, can_view_total_TM), // {23}
											r(InProgress[0], false, can_view_total_TM), // {24}
											r(InProgress[1], true, can_view_total_TM), // {25}
											r(OpenPO[0], false, can_view_total_TM), // {26}
											r(OpenPO[1], true, can_view_total_TM),  //27

											show_comp_header ? "visible" : "hidden"//28

											);

				}
				summary.AppendFormat(@"
										<tr class='{11}' style='height:25px; font-weight: bold;background-color:#e8f1f9;'>

											<td style='background-color:#e2e9ef;'>&nbsp;</td>
<td style='background-color:#e2e9ef;' data-colname='scanned' >{5}</td>
											<td class='branch' style='background-color:#e2e9ef;'>{1}</td>

											<td style='background-color:#e2e9ef;'>{2}</td>
											<td >{19}</td>
											<td data-colname='open' style='background-color:#e2e9ef;'>{3}</td>
											<td data-colname='open' >{12}</td>
											<td data-colname='onhold' style='background-color:#e2e9ef;'>{10}</td>
											<td data-colname='onhold' >{18}</td>
											<td data-colname='process' style='background-color:#e2e9ef;'>{4}</td>
											<td data-colname='process' >{13}</td>
											
											<td data-colname='inprogress' style='background-color:#e2e9ef;'>{24}</td>
											<td data-colname='inprogress' >{25}</td>
											<td data-colname='openpo' style='background-color:#e2e9ef;'>{26}</td>
											<td data-colname='openpo' >{27}</td>
											<td data-colname='rework' style='background-color:#e2e9ef;'>{20}</td>
											<td data-colname='rework' >{21}</td>
											<td data-colname='questions' style='background-color:#e2e9ef;'>{9}</td>
											<td data-colname='questions' >{17}</td>
											<td data-colname='pmapproval' style='background-color:#e2e9ef;'>{6}</td>
											<td data-colname='pmapproval'>{14}</td>
											<td data-colname='bmapproval' style='background-color:#e2e9ef;'>{7}</td>
											<td data-colname='bmapproval' >{15}</td>
											<td data-colname='invoiced' style='background-color:#e2e9ef;'>{8}</td>
											<td data-colname='invoiced' >{16}</td>
											<td data-colname='custpo' style='background-color:#e2e9ef;'>{22}</td>
											<td data-colname='custpo' >{23}</td>
										</tr>
													</tbody>
												</table>",
											"", //  {0}
											"", //  {1}
											r(GrandTotal[0], false, can_view_total_TM), //  {2}
											r(GrandOpenTotal[0], false, can_view_total_TM), //  {3}
											r(GrandTotal[0] - GrandOpenTotal[0] - GrandOnHold[0], false, can_view_total_TM), //  {4}
											grandjustscan, //  {5}
											r(GrandPMTotals[0], false, can_view_total_TM), //  {6}
											r(GrandBMTotals[0], false, can_view_total_TM), //  {7}
											r(GrandWInvTotals[0], false, can_view_total_TM), //  {8}
											r(GrandQuestTotals[0], false, can_view_total_TM), //  {9}
											r(GrandOnHold[0], false, can_view_total_TM), // {10}
											strClass, // {11}
											r(GrandOpenTotal[1], true, can_view_total_TM), // {12}
											r(GrandTotal[1] - GrandOpenTotal[1] - GrandOnHold[1], true, can_view_total_TM), // {13}
											r(GrandPMTotals[1], true, can_view_total_TM), // {14}
											r(GrandBMTotals[1], true, can_view_total_TM), // {15}
											r(GrandWInvTotals[1], true, can_view_total_TM), // {16}
											r(GrandQuestTotals[1], true, can_view_total_TM), // {17}
											r(GrandOnHold[1], true, can_view_total_TM), // {18}
											r(GrandTotal[1], true, can_view_total_TM), // {19}
											r(GrandRework[0], false, can_view_total_TM), // {20}
											r(GrandRework[1], true, can_view_total_TM), // {21}
											r(GrandWaitingForPO[0], false, can_view_total_TM), // {22}
											r(GrandWaitingForPO[1], true, can_view_total_TM), // {23}
											r(GrandInProgress[0], false, can_view_total_TM), // {24}
											r(GrandInProgress[1], true, can_view_total_TM), // {25}
											r(GrandOpenPO[0], false, can_view_total_TM), // {26}
											r(GrandOpenPO[1], true, can_view_total_TM), // {27}
											""//{28}
											);
				divCoSummary.InnerHtml = summary.ToString();
			}
			else
			{
				divCoSummary.Visible = false;
			}

			#endregion
		}
	}

	private void fill_gv_summary()
	{
		var dt = Toolbox.doSQL_dt(@" SELECT a.woprog_id, a.woprog_bvwo, a.business_unit_id, a.woprog_vis_to_cust, a.woprog_invoiceno, a.woprog_customer_id customer_id, a.woprog_customername, a.woprog_status status, a.woprog_custpo woprog_custpo, a.woprog_opendatetime open_dt, d.member_fullname cutby, a.woprog_stilltobebilled, IF(CAST(a.woprog_quoteid AS UNSIGNED) > 100000, LEFT(a.woprog_quoteid, 6), NULL) quote_id, IF(CAST(a.woprog_quoteid AS UNSIGNED) > 100000, SUBSTRING(a.woprog_quoteid, 7, 2), NULL) revision, a.woprog_description, e.member_fullname acct_manager, f.member_fullname pm, a.woprog_cutdatetime, g.Contact_Name as Contact_Name, h._date, woprog_hold hold, woprog_ts last_modified, woprog_grossmargin margin FROM woprog a LEFT JOIN customer b ON a.woprog_customer_id = b.customer_id and a.business_unit_id = @v0  LEFT JOIN customer_sales_properties csp ON csp.customer_id = a.woprog_customer_id AND csp.address_id = a.woprog_address_id LEFT JOIN business_unit c ON a.business_unit_id = c.id and a.business_unit_id = @v0  LEFT JOIN member d ON a.woprog_cutby_memberid = d.member_id and a.business_unit_id = @v0  LEFT JOIN member e ON csp.account_manager = e.member_id LEFT JOIN member f ON a.woprog_pm_memberid = f.member_id and a.business_unit_id = @v0  LEFT JOIN contact g ON a.woprog_contact_id = g.contact_id and g.contact_type = 'Customer' and a.business_unit_id = @v0  LEFT JOIN (SELECT MIN(hh.startdate) _date, hh.woprog_id FROM appointments hh WHERE hh.business_unit_id = @v0  AND hh.startdate >= CURDATE() GROUP BY hh.woprog_id) h ON h.woprog_id = a.woprog_id WHERE a.business_unit_id = @v0  AND a.woprog_status != 'Deleted' and (a.WOProg_InvoiceDate >(curdate()-interval 5 year) or (a.woprog_status!='Invoiced' )) ORDER BY a.woprog_id DESC ", new object[] { Session["working_business_unit_id"] });
		gv_wosummary.DataSource = dt;
		gv_wosummary.DataBind();
	}
	private string r(double number, bool is_dollar, bool can_view_total_TM)
	{
		var _number = is_dollar ? number.ToString("C0") : number.ToString();
		var formatted_number = "";
		if (number > 0)
		{
			formatted_number = _number;
		}
		else if (number < 0)
		{
			formatted_number = "<font style='color:#f00;'>" + _number + "</font>";
		}
		else
		{
			formatted_number = "<font style='weight: normal'>" + _number + "</font>";
		}
		if (can_view_total_TM || !is_dollar)
		{
			return formatted_number;
		}
		else
		{
			return "";
		}
	}
	protected void ds_summary_Init(object sender, EventArgs e)
	{
		var ds = (SqlDataSource)sender;
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);

		var totalSummary = new ASPxSummaryItem
		{
			FieldName = "woprog_stilltobebilled",
			ShowInColumn = "woprog_stilltobebilled",
			SummaryType = DevExpress.Data.SummaryItemType.Sum,
			DisplayFormat = "C2"
		};

		gv_wosummary.TotalSummary.Add(totalSummary);
		gv_wosummary.DataBind();

	}
	protected void hl_quote_Init(object sender, EventArgs e)
	{
		var hl = (HyperLink)sender;
		if (hl.Text == "0")
		{
			throw new Exception(hl.Text);
		}
	}
	protected void cb_branches_DataBound(object sender, EventArgs e)
	{
		var c = (ASPxComboBox)sender;
		c.Value = Convert.ToString(Session["working_business_unit_id"]);

	}
	protected void cb_company_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		Session["working_business_unit_id"] = e.Parameter;
	}
	protected void text_container_Init(object sender, EventArgs e)
	{
		var l = (ASPxLabel)sender;
		l.Attributes.Add("data-title", l.Text.Replace("\"", ""));
		l.CssClass = "ttip";
	}

	protected void gv_wosummary_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
	{


		try
		{
			var wo = Convert.ToInt32(gv_wosummary.GetRowValues(e.VisibleIndex, "woprog_id"));


			var wwo = new NeWOProg(Convert.ToInt32(wo));

			wwo.print_barcode_label(1);

		}
		catch
		{
		}
	}
	protected void cb_branches_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["working_business_unit_id"] = cb_branches.Value.ToString();
		gv_wosummary.DataBind();
	}
	protected void gv_wosummary_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_wosummary_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_wosummary_ClientLayout(object sender, DevExpress.Web.ASPxClientLayoutArgs e)
	{
		gv_wosummary.Columns["woprog_stilltobebilled"].Visible = can_view_total_TM;

	}
	[WebMethod]
	public static string print_barcode(int woprog_id)
	{
		var wo = new NeWOProg(woprog_id);
		wo.print_barcode_label(1);
		return wo.OrderNumber;
	}
	protected void gv_wosummary_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			var this_date = gv_wosummary.GetRowValues(e.VisibleIndex, "_date");
			var date_string = this_date == null ? "" : this_date.ToString();
			if (date_string != "")
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");
			}
		}
	}



	protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
	{
		fill_top_summary();
	}

}