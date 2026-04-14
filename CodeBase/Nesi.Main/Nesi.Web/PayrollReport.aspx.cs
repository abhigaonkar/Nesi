using System;
using System.Data;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.Diagnostics;
using nesi.core;

public partial class PayrollReport : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id			= 31; // from Page table in DB
	private const string _page_description	= "Payroll Reports by Selected User and Date";
	
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools			= new Toolbox();
		var _q	= Request.QueryString;
		myMember				= Toolbox.do_handle_authentication(_page_id);
		_tools.page_author		= new NeMember(711);
		
		var menu				= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml		= menu.MenuHTML;
		divSide.InnerHtml		= shared.PrintSidePanelHTML(myMember);

		var lbltemp			= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text			= _page_description;
		var business_unit_id			= myMember.business_unit_id;
		var Payroll_Report	= new Report();
		if(_q["payperiod_id"] != null)
			{
			Payroll_Report.payperiod_id		= _q["payperiod_id"];
			}
		else
			{
			Payroll_Report.payperiod_id		= _tools.getSQL_string(@"CALL _payperiod()"  , null);
			}
		Payroll_Report._payperiod			= new NePayPeriod(Convert.ToInt32(Payroll_Report.payperiod_id));
		if(_q["start_date"] != null && _q["end_date"] != null && _q["start_date"] != "" && _q["end_date"] != "" )
			{
			Payroll_Report.start_date		= _q["start_date"];
			Payroll_Report.end_date			= _q["end_date"];
			text_start_date.Text			= Payroll_Report.start_date.ToString();
			text_end_date.Text				= Payroll_Report.end_date.ToString();
			}
		else
			{
			Payroll_Report.start_date		= Payroll_Report._payperiod.StartDate;
			Payroll_Report.end_date			= Payroll_Report._payperiod.Enddate;
			text_start_date.Text			= Payroll_Report._payperiod.StartDate;
			text_end_date.Text				= Payroll_Report._payperiod.Enddate;
			}
		ddl_payperiod.DataSource			= Payroll_Report.build_payperiods();
		ddl_payperiod.DataValueField		= "value";
		ddl_payperiod.DataTextField			= "name";
		ddl_payperiod.SelectedValue			= Payroll_Report.payperiod_id.ToString();
		ddl_payperiod.Attributes.Add("onchange", "$('.text_start_date').val('');$('.text_end_date').val('');");
		ddl_payperiod.DataBind();
		
		if(_q["business_unit_id"] != null)
			{
			business_unit_id						= Convert.ToInt32(_q["business_unit_id"]);
			}
		Payroll_Report.business_unit_id			= business_unit_id;
			
		ddl_branch.DataSource				= Payroll_Report.build_branches();
		ddl_branch.DataValueField			= "value";
		ddl_branch.DataTextField			= "name";
		ddl_branch.SelectedValue			= Payroll_Report.business_unit_id.ToString();
		ddl_branch.DataBind();
		text_start_date.Attributes.Add("onfocus", "$(this).datepicker({ dateFormat: 'yy-mm-dd' });");
		text_end_date.Attributes.Add("onfocus", "$(this).datepicker({ dateFormat: 'yy-mm-dd' });");
					Debug.WriteLine(DateTime.Now.ToString("u")+" -- build Start");
		div_payroll_report.InnerHtml		= Payroll_Report.buildreport();
					Debug.WriteLine(DateTime.Now.ToString("u")+" -- build End");
		}


	private class Report
		{
		private Toolbox _tools			= new Toolbox();
		private object _business_unit_id		= null;
		private object _payperiod_id	= null;
		private object _start_date		= null;
		private object _end_date		= null;
		public NePayPeriod _payperiod;
		public object business_unit_id
			{
			get {return _business_unit_id;}
			set {_business_unit_id = value;}
			}
		public object payperiod_id
			{
			get {return _payperiod_id;}
			set {_payperiod_id = value;}
			}
		public object start_date
			{
			get {return _start_date;}
			set {_start_date = value;}
			}
		public object end_date
			{
			get {return _end_date;}
			set {_end_date = value;}
			}
		public string buildreport()
			{
			// get divisions for active company
			var builder				= new StringBuilder();
			    bool timesheet_lite = new NeBusinessUnit(_business_unit_id).allow_unlinked_timesheet;

            builder.Append("<div align='right'>");
			builder.Append("</div>");
			if(_payperiod_id != null)
				{
				var total_all				= new hours();
				var _employees		= Toolbox.doSQL_dt(@"CALL REPORT_PAYROLL(@v0 , @v1 , @v2 )", new object[] {  start_date, end_date, _business_unit_id } );
				var bus					= (from DataRow dr in _employees.Rows
												select new {	id		= dr["buid"],
																name	= dr["bu_name"]}).Distinct();
				var divs_c					= bus.Count();
				for(var d = 0; d < divs_c; d++)
					{
					#region go through each business unit
					var total_div				= new hours();
					var bu_id					= (int)bus.ElementAt(d).id;
					var bu_name				= bus.ElementAt(d).name;
				
					var ids						= (from DataRow dr in _employees.Rows
													where (int) dr["buid"] == bu_id 
													select new {	id		= dr["employee_id"], 
																	name	= dr["employee_name"], 
																	type	= dr["employee_type"]
																	}).Distinct();
					var ids_c					= ids.Count();
					builder.AppendFormat("<a name='{0}' /><div style='background-color:#48B;padding:5px;color:#fff;font-family:'Segoe UI',Helvetica,'Droid Sans',Tahoma,Geneva,sans-serif;font-size:20px;font-weight:bold;border-bottom:solid 1px #fff;'>{1} ({0})</div>", bu_id, bu_name);
					for(var i = 0; i < ids_c; i++)
						{
						#region cycle through each employee
						var member_id				= ids.ElementAt(i).id;
						var member_type				= ids.ElementAt(i).type;
						var member_full_name		= ids.ElementAt(i).name;
						var _employee_data	= _employees.Select("employee_id = "+member_id);
						var total					= new hours();
						var w1					= new hours();
						var w2					= new hours();
						builder.AppendFormat(@"
						<div style='padding:5px 5px 2px 5px;font-size:20px;border-left:solid 1px #48B;border-right:solid 1px #48B;border-top:solid 1px #48B;'><b>{0} ({2})</b></div>
						<div style='padding:0px 0px 2px 5px;border-left:solid 1px #48B;border-right:solid 1px #48B;color:#777;'><i><b>{1}</b></i></div>
						<table width='100%' style='margin-bottom:10px;' cellpadding='2' cellspacing='0' class='member'>
							<thead style='background-color:#48B;color:#fff;font-weight:bold;cursor:pointer;'>
								<tr>
									<th width='100'>DATE</th>
									<th width='300'>CUSTOMER NAME</th>
									<th width='100'>WO#</th>
									<th width='100'>WOTYPE</th>
									<th width='50'>RT</th>
									<th width='50'>OT</th>
									<th width='50'>DT</th>
									<th width='50'>RTsp</th>
									<th width='50'>OTsp</th>
									<th width='50'>DTsp</th>
									<th style='min-width:350px;overflow-wrap:break-word;'>DESCRIPTION</th>
                                    <th style='min-width:70px'>Job Type</th>
                                    <th style='min-width:250px;overflow-wrap:break-word;'>WO Description</th>
								</tr>
							</thead>
							<tbody>", member_full_name, member_type, member_id);
						var previous_week			= 0;
						double vac_total			= 0;
						foreach(var _data in _employee_data)
							{
							#region Cycle through hour data
							var week_n				= (int) _data["week_n"];
                            DateTime date				= (DateTime) _data["mydate"];
                            DateTime endtd = Convert.ToDateTime(end_date.ToString());
                            TimeSpan diff = (endtd - date);
                            if (diff.Days>=7)
                            {
                                previous_week = week_n;
                            }
                            else
                            {
                                previous_week = week_n-1;
                            }


                            var background_color	= "#eee";
							//if(previous_week == 0)
							//	{
							//	previous_week		= week_n-1;
							//	}
									
							var customer_name		= _data["customer_name"];
							var wotype				= _data["wotype"].ToString();
							int wo_number;
							int.TryParse(_data["workorder_id"].ToString(), out wo_number);
							var r = new hours
										{
										rt = (double) _data["rt"],
										ot = (double) _data["ot"],
										dt = (double) _data["dt"],
										rtsp = (double) _data["rtsp"],
										otsp = (double) _data["otsp"],
										dtsp = (double) _data["dtsp"],
										vac	= wotype == "VAC" ? (double) _data["rt"] : 0
										};
							var description		= _data["description"].ToString();
                            var wo_description = _data["WO_Description"].ToString();
                            var job_type = _data["jobType"].ToString();
							if(wotype == "VAC")
								{
								vac_total += (double) _data["rt"];
								}
							var str_wo_number			= wo_number.ToString();
							var wo_number_length		= str_wo_number.Length;
							var quote_id				= wotype == "Quote" && str_wo_number != "0" ? str_wo_number.Substring(0,6) : "";
							var rev						= wotype == "Quote" && str_wo_number != "0" && str_wo_number.Substring(6, wo_number_length-6) != "0" 
															? str_wo_number.Substring(6, wo_number_length-6) 
															: "1";

							var number_link				= timesheet_lite? str_wo_number: wotype == "WO" 
															? string.Format(@"<a href='javascript:void(0);' onclick=""boing('/wo_prog_frame.aspx?action=show&woprog_id={0}', 'wo_popup{0}', 1024, 768);"">{0}</a>", wo_number) 
															: wotype == "Quote" && str_wo_number.Length >= 7
																? string.Format(@"<a href='javascript:void(0);' onclick=""boing('/#/opens/65/quotes/{0}/{1}', 'quote_popup{0}', 1024, 768);"">{0} v{1}</a>", quote_id, rev) 
																: wo_number.ToString();
							total.rt					+= r.rt;
							total.ot					+= r.ot;
							total.dt					+= r.dt;
							total.rtsp					+= r.rtsp;
							total.otsp					+= r.otsp;
							total.dtsp					+= r.dtsp;
							total.vac					+= wotype == "VAC" ? r.rt : 0;
									
							total_all.rt				+= r.rt;
							total_all.ot				+= r.ot;
							total_all.dt				+= r.dt;
							total_all.rtsp				+= r.rtsp;
							total_all.otsp				+= r.otsp;
							total_all.dtsp				+= r.dtsp;
							total_all.vac				+= wotype == "VAC" ? r.rt : 0;
									
							total_div.rt				+= wotype == "VAC" ? 0 : r.rt;
							total_div.ot				+= r.ot;
							total_div.dt				+= r.dt;
							total_div.rtsp				+= r.rtsp;
							total_div.otsp				+= r.otsp;
							total_div.dtsp				+= r.dtsp;
							total_div.vac				+= wotype == "VAC" ? r.rt : 0;
									
							if(week_n != previous_week)
								{
								w2.rt				+= wotype == "VAC" ? 0 : r.rt;
								w2.ot				+= r.ot;
								w2.dt				+= r.dt;
								w2.rtsp				+= r.rtsp;
								w2.otsp				+= r.otsp;
								w2.dtsp				+= r.dtsp;
								w2.vac				+= wotype == "VAC" ? r.rt : 0;
								background_color	= "#ccc";
								}
							else
								{
								w1.rt				+= wotype == "VAC" ? 0 : r.rt;
								w1.ot				+= r.ot;
								w1.dt				+= r.dt;
								w1.rtsp				+= r.rtsp;
								w1.otsp				+= r.otsp;
								w1.dtsp				+= r.dtsp;
								w1.vac				+= wotype == "VAC" ? r.rt : 0;
								}
									
							builder.AppendFormat(@"
								<tr style='background-color:{11};'>
									<td align='center' style='border-left:solid 1px #48B;'><b>{0}</b></td>
									<td>{1}</td>
									<td align='center'>{2}</td>
									<td align='center'>{3}</td>
									<td align='center'>{4}</td>
									<td align='center'>{5}</td>
									<td align='center'>{6}</td>
									<td align='center'>{7}</td>
									<td align='center'>{8}</td>
									<td align='center'>{9}</td>
									<td style='min-width:350px;overflow-wrap:break-word;'>{10}</td> 
                                    <td align='center'>{12}</td>
                                    <td style='border-right:solid 1px #48B;min-width:250px;overflow-wrap:break-word;'>{13}</td>  
								</tr>	
							", Toolbox.MySQL_shortdt(date), customer_name, number_link, wotype, format_hour(r.rt), format_hour(r.ot), format_hour(r.dt), format_hour(r.rtsp), format_hour(r.otsp), format_hour(r.dtsp), description, background_color,job_type,wo_description);
							#endregion Cycle through hour data
							}
						builder.AppendFormat(@"
							</tbody>
							<tfoot style='color:#fff;font-weight:bold;'>
								<tr style='background-color:#fff;color:#000;'>
									<td colspan='4' align='right' style='border-left:solid 1px #48B;'>Week 1 Total:</td>
									<td align='center'>{6}</td>
									<td align='center'>{7}</td>
									<td align='center'>{8}</td>
									<td align='center'>{9}</td>
									<td align='center'>{10}</td>
									<td align='center'>{11}</td>
                                    <td align='center' style='min-width:350px;'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center' style='border-right:solid 1px #48B;min-width:250px;'>&nbsp;</td>
								</tr>
								<tr style='background-color:#fff;color:#000;'>
									<td colspan='4' align='right' style='border-left:solid 1px #48B;'>Week 2 Total:</td>
									<td align='center'>{12}</td>
									<td align='center'>{13}</td>
									<td align='center'>{14}</td>
									<td align='center'>{15}</td>
									<td align='center'>{16}</td>
									<td align='center'>{17}</td>
                                    <td align='center' style='min-width:350px;'>&nbsp;</td>
									<td align='center'>&nbsp;</td>      
									<td align='center' style='border-right:solid 1px #48B;min-width:250px;'>&nbsp;</td>
								</tr>
								<tr style='background-color:#fff;color:#000;{19}'>
									<td colspan='4' align='right' style='border-left:solid 1px #48B;'>Vacation Total:</td>
									<td align='center'>{18}</td>
									<td align='center'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
                                    <td align='center' style='min-width:350px;'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center' style='border-right:solid 1px #48B;min-width:250px;'>&nbsp;</td>
								</tr>
								<tr style='background-color:#48B;color:#fff;'>
									<td colspan='4' align='right' style='border-left:solid 1px #48B;'>Total:</td>
									<td align='center'>{0}</td>
									<td align='center'>{1}</td>
									<td align='center'>{2}</td>
									<td align='center'>{3}</td>
									<td align='center'>{4}</td>
									<td align='center'>{5}</td>
                                    <td align='center' style='min-width:350px;'>&nbsp;</td>
									<td align='center'>&nbsp;</td>
									<td align='center' style='border-right:solid 1px #48B;min-width:250px;'>&nbsp;</td>
								</tr>
							</tfoot>
						</table>",
							total.rt, total.ot, total.dt, total.rtsp, total.otsp, total.dtsp, // {0} - {5}
							w1.rt, w1.ot, w1.dt, w1.rtsp, w1.otsp, w1.dtsp, // {6} - {11}
							w2.rt, w2.ot, w2.dt, w2.rtsp, w2.otsp, w2.dtsp, // {12 - {17}
							vac_total, vac_total == 0 ? "display:none;" : ""
							);
						#endregion cycle through each employee
						}
					builder.AppendFormat(@"
					<div style='border:dashed 1px #888; padding:5px;width:350px;margin:5px;margin-bottom:50px;'>
						<div style='border-bottom:solid 1px #ccc;'><b>Hour Type Totals For Business Unit: {9}</b></div>
						<div><span style='{8}'>RT:</span> {0}</div>
						<div><span style='{8}'>OT:</span> {1}</div>
						<div><span style='{8}'>DT:</span> {2}</div>
						<div><span style='{8}'>RTsp:</span> {3}</div>
						<div><span style='{8}'>OTsp:</span> {4}</div>
						<div><span style='{8}'>DTsp:</span> {5}</div>
						<div><span style='{8}'>Vacation:</span> {6}</div>
						<div><span style='{8}'>Total of All Hour Types:</span> {7}</div>
					</div>
						",
						format_hour(total_div.rt),		// {0}
						format_hour(total_div.ot),		// {1}
						format_hour(total_div.dt),		// {2}
						format_hour(total_div.rtsp),		// {3}
						format_hour(total_div.otsp),		// {4}
						format_hour(total_div.dtsp),		// {5}
						format_hour(total_div.vac),		// {6}
						format_hour(total_div.rt + total_div.ot + total_div.dt + total_div.rtsp + total_div.otsp + total_div.dtsp+ total_div.vac),	// {7}
						"width:75%;font-weight:bold;float:left;",		// {8}
						bu_name
						 ); 
					#endregion go through each division
					}
				    if (divs_c > 1)
				        {
				        builder.AppendFormat(@"
					<div align='right'>
					<div style='border:solid 3px #060; padding:5px;width:350px;margin:5px;'>
						<div style='border-bottom:solid 1px #ccc;'><b>Hour Type Totals For This Business Unit</b></div>
						<div><span style='{8}'>RT:</span> {0}</div>
						<div><span style='{8}'>OT:</span> {1}</div>
						<div><span style='{8}'>DT:</span> {2}</div>
						<div><span style='{8}'>RTsp:</span> {3}</div>
						<div><span style='{8}'>OTsp:</span> {4}</div>
						<div><span style='{8}'>DTsp:</span> {5}</div>
						<div><span style='{8}'>Vacation:</span> {6}</div>
						<div><span style='{8}'>Total of All Hour Types:</span> {7:N2}</div>
					</div>
					</div>
					",
				            format_hour(total_all.rt), // {0}
				            format_hour(total_all.ot), // {1}
				            format_hour(total_all.dt), // {2}
				            format_hour(total_all.rtsp), // {3}
				            format_hour(total_all.otsp), // {4}
				            format_hour(total_all.dtsp), // {5}
				            format_hour(total_all.vac), // {6}
				            format_hour(total_all.rt + total_all.ot + total_all.dt + total_all.rtsp + total_all.otsp +
				                        total_all.dtsp + total_all.vac), // {6}
				            "width:75%;font-weight:bold;float:left;" // {7}
				        );
				        }
				    return builder.ToString();
				}
			else
				{
				return "";
				}
			}
		private string format_hour(double _hours)
			{
			var color		= _hours == 0 ? "#aaa" : _hours < 0 ? "#f00" : "#000";
			return "<span style='color:"+color+";'>"+_hours.ToString("N2")+"</span>";
			}
		public DataTable build_branches()
			{
			return _tools.getSQL_datatable(@"SELECT id value, ddl_name name from business_unit  WHERE enable_timesheet = 1 and id in(" + new Current_User().visible_business_units + ") order by ddl_name" , null);
			}
		public DataTable build_payperiods()
			{
			return _tools.getSQL_datatable(@"SELECT payperiodid value, CAST(CONCAT(payperiodid,' - ',DATE_FORMAT(startdate, '%Y-%m-%d'), ' - ', DATE_FORMAT(enddate, '%Y-%m-%d')) AS CHAR) name FROM payperiods ORDER BY startdate"  , null);
			}
		private struct hours
			{
			public double rt;
			public double ot;
			public double dt;
			public double rtsp;
			public double otsp;
			public double dtsp;
			public double vac;
			}
		}

}
