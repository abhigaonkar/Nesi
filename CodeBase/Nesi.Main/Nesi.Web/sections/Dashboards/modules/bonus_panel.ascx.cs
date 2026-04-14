using System;
using System.Data;
using DevExpress.Web;
using nesi.core;

public partial class sections_Dashboards_modules_bonus_panel : System.Web.UI.UserControl
	{
	public NeMember current_user;
	public NeMember loaded_user { get; set; }
	public string _type;

	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(1);
		_company_wide = current_user.AuthenticatedForPrivilege(146);
		_region_wide = current_user.AuthenticatedForPrivilege(147);
		_branch_wide = current_user.AuthenticatedForPrivilege(149);
		_base_level = current_user.AuthenticatedForPrivilege(150);

		}

	protected void Page_Load(object sender, EventArgs e)
		{

		var myqtr_date = (ASPxDateEdit)mypanel.FindControl("myqtr_date");
		var ddlpm = (ASPxComboBox)mypanel.FindControl("ddlpm");
		if (!IsPostBack)
			{
			Session["dashboard2_ddlpm"] = null;
			ddlpm.Value = current_user.id;
			myqtr_date.Date = System.DateTime.Today;

			}
		if (_company_wide || _region_wide || _branch_wide)
			{
			ddlpm.ClientVisible = true;
			}
		else
			{
			ddlpm.ClientVisible = false;
			}
		fill_grid();
		if (loaded_user != null)
			{
			fill_details();
			}
		else
			{
			loaded_user = new NeMember(Convert.ToInt32(ddlpm.Value));
			}


		}



	public void fill_details()
		{
		using (var conn = Toolbox.connect())
			{
			var bonus_text = "";
			var bonus_text_2 = "";
			double bonus = 0;
			var myqtr_date = (ASPxDateEdit)mypanel.FindControl("myqtr_date");
			var ddlpm = (ASPxComboBox)mypanel.FindControl("ddlpm");
			var btn_agreement = (ASPxButton)mypanel.FindControl("btn_agreement");
			var moid = NeMemberOffer.get_current_agreement_at_date(Convert.ToInt32(ddlpm.Value), myqtr_date.Date);
			if (moid == 0)
				{
				btn_agreement.ClientSideEvents.Click = string.Format(@"function (s, e) {{alert('There is no agreement in place for this period'); }}", moid);
				}
			else
				{
				btn_agreement.ClientSideEvents.Click = string.Format(@"function (s, e) {{open_offer({0}); }}", moid);
				}


			ddlpm.Value = loaded_user.id;
			var wage = new NeWage(Convert.ToInt32(ddlpm.Value));
			var currentpm = new NeMember(Convert.ToInt32(ddlpm.Value));
			var mo = new NeMemberOffer(NeMemberOffer.get_current_agreement_at_date(currentpm.id, myqtr_date.Date));

			var lbl_bonus_type = "";
			var lbl_bonus_type_sum = "";
			var fiscal_start = myqtr_date.Date.ToString("yyyy-01-01");
			var fiscal_end = myqtr_date.Date.ToString("yyyy-12-31");
			ASPxMemo1.Text = Toolbox.doSQL_string(conn, @"Select ifnull((Select ifnull(bonus_notes,'') from member where member_id = @v0),'')",new object[] { currentpm.id});
			var rev_todate = Toolbox.doSQL_double(conn,@"select ifnull((SELECT sum(get_invoiced_of_all_wos(ryw.WOProg_ID)) benchmark FROM woprog ryw  WHERE ryw.WOProg_Associate_WOProg_ID = 0 and ryw.WOProg_Status = 'Invoiced' and ryw.business_unit_id =@v0 and ryw.woprog_pm_memberid =@v1  AND ryw.WOProg_InvoiceDate<=@v2  and ( ryw.woprog_invoicebalance<=0 || (ryw.WOProg_InvoiceDate>curdate() - interval 180 day)) and ryw.WOProg_InvoiceDate>=@v3 ) ,0) ", new object[] { currentpm.business_unit_id,currentpm.id,fiscal_end,fiscal_start });
			var rev_todate_uncollected = Toolbox.doSQL_double(conn,@"select ifnull((SELECT sum(get_invoiced_of_all_wos(ryw.WOProg_ID)) benchmark FROM woprog ryw  WHERE ryw.WOProg_Associate_WOProg_ID = 0 and ryw.WOProg_Status = 'Invoiced' and ryw.business_unit_id =@v0 and ryw.woprog_pm_memberid =@v1  AND ryw.WOProg_InvoiceDate<=@v2  and ( ryw.woprog_invoicebalance>0 && (ryw.WOProg_InvoiceDate<curdate() - interval 180 day)) and ryw.WOProg_InvoiceDate>=@v3) ,0) ", new object[] { currentpm.business_unit_id,currentpm.id,fiscal_end,fiscal_start });

			var bench_todate = Toolbox.doSQL_double(conn,@"select ifnull((SELECT sum(get_benchmark_of_all_wos(ryw.WOProg_ID)) benchmark FROM woprog ryw  WHERE ryw.WOProg_Associate_WOProg_ID = 0 and ryw.WOProg_Status = 'Invoiced' and ryw.business_unit_id =@v0 and ryw.woprog_pm_memberid =@v1  AND ryw.WOProg_InvoiceDate<=@v2  and ( ryw.woprog_invoicebalance<=0 || (ryw.WOProg_InvoiceDate>curdate() - interval 180 day)) and ryw.WOProg_InvoiceDate>=@v3 ),0) ", new object[] { currentpm.business_unit_id,currentpm.id,fiscal_end,fiscal_start });
			var unbillable_hours = Toolbox.doSQL_double(conn,@"Select ifnull(sum(numberofhours),0) from membertime  where date >=@v0 and date <=@v1  and business_unit_id =@v2 AND membertime_memberid = @v3  and (wotype = 'Quote' or wotype='Shop' or wotype='Telem')", new object[] { fiscal_start,fiscal_end,currentpm.business_unit_id, currentpm.id }) ;
			var unbillable = unbillable_hours * currentpm.chargeout;
			var margin_todate = Toolbox.doSQL_double(conn,@"select ifnull((SELECT sum(get_margin_dollars_of_all_wos(ryw.WOProg_ID)) benchmark FROM woprog ryw  WHERE ryw.WOProg_Associate_WOProg_ID = 0 and ryw.WOProg_Status = 'Invoiced' and ryw.business_unit_id =@v0 and ryw.woprog_pm_memberid =@v1  AND ryw.WOProg_InvoiceDate<=@v2  and ( ryw.woprog_invoicebalance<=0 || (ryw.WOProg_InvoiceDate>curdate() - interval 180 day)) and ryw.WOProg_InvoiceDate>=@v3 ),0) ", new object[] { currentpm.business_unit_id,currentpm.id,fiscal_end,fiscal_start });
			var wo_credits = Toolbox.doSQL_double(conn,@"select ifnull((SELECT sum(benchmark_credit) FROM woprog ryw  WHERE ryw.WOProg_Associate_WOProg_ID = 0 and ryw.WOProg_Status = 'Invoiced' and ryw.business_unit_id =@v0 and ryw.woprog_pm_memberid =@v1  AND ryw.WOProg_InvoiceDate<=@v2  and ryw.WOProg_InvoiceDate>=@v3 ),0) ", new object[] { currentpm.business_unit_id,currentpm.id,fiscal_end,fiscal_start });
			var benchmark_overage = (rev_todate - bench_todate - unbillable);
			bonus = (margin_todate - unbillable) * mo.bonus_amount;

			if (wage.bonus_type == 3)  // margin dollars 
				{
				lbl_bonus_type = "Total Margin:";
				lbl_bonus_type_sum = margin_todate.ToString("C2");
				bonus_text_2 = "Potential Incentive : " + margin_todate.ToString("C0") + " - " + unbillable.ToString("C0") + " * " + mo.bonus_amount.ToString("p2") + " = " + bonus.ToString("C2");

				if (rev_todate < mo.bonus_revenue_threshold)
					{
					bonus_text = "HOWEVER your Revenue is below your target of  " + mo.bonus_revenue_threshold.ToString("c2") + " so you are not yet qualifying.";
					}
				else if (margin_todate < (mo.bonus_margin_threshold * 0.75))
					{
					bonus_text = "HOWEVER your Margin is below your target of  " + (mo.bonus_margin_threshold * 0.75).ToString("C2") + " so you are not yet qualifying.";
					}
				else
					{
					bonus = Convert.ToDouble((margin_todate - unbillable) * mo.bonus_amount);

						{
						bonus_text = "** Unofficial / Undeclared Incentive : " + bonus.ToString("C2") + ".  There are likely still adjustments to be made at year end, so this is not a declared incentive or bonus.";
						}

					}
				}


			else if (wage.bonus_type == 10)  // bm comp
				{
				lbl_bonus_type = "BM Compensation plan:";
				lbl_bonus_type_sum = margin_todate.ToString("C2");
				}





			div_details.InnerHtml = string.Format(@"<table style='width: 100%; font-family: Arial;'>
		
		<tr>
		<td><b>{7}</b></td>
		<td></td>
		<td></td>
		</tr>
		<tr>

		<tr>
		<td>{1}</td>
		<td></td>
		<td></td>
		</tr>

			<td>Revenue Target:</td>
			<td style='text-align: right;'>{0}</td>
			<td style='text-align: right; font-weight: bold; width:100%;'></td>
		</tr>
<tr>
			<td>Margin Target:</td>
			<td style='text-align: right;'>{6}</td>
			<td style='text-align: right; font-weight: bold; width:100%;'></td>
		</tr>
		<tr>
			<td>Revenue:</td>
			<td style='text-align: right;'>{2}</td>
			<td></td>
		</tr>
<tr>
			<td>Bad Debts: (unpaid after 180 days)</td>
			<td style='text-align: right;'>{8}</td>
			<td></td>
		</tr>
		<tr>
			<td nowrap='nowrap'>Unbillable Time:<div style='font-size:0.75em'>(base RT chargeout * all non-WO related time entries: {9:C2} * {10:N2})</div></td>
			<td style='text-align: right;'>{5}</td>
			<td></td>
		</tr>
		<tr>
			<td nowrap='nowrap'>{3}</td>
			<td style='text-align: right;'>{4}</td>
			<td></td>
		</tr>
	</table>", mo.bonus_revenue_threshold.ToString("C2"),
				 bonus_text,
				 rev_todate.ToString("C2"),
				 lbl_bonus_type,
				 lbl_bonus_type_sum,
				 unbillable.ToString("C2"),
				 mo.bonus_margin_threshold.ToString("c2"),
				 bonus_text_2,
				 rev_todate_uncollected.ToString("C2"),
				 currentpm.chargeout,
				 unbillable_hours);

			}
		}


	protected void fill_grid()
		{
		var myqtr_date = (ASPxDateEdit)mypanel.FindControl("myqtr_date");
		var ddlpm = (ASPxComboBox)mypanel.FindControl("ddlpm");
		var wage = new NeWage(Convert.ToInt32(ddlpm.Value));
		var currentpm = new NeMember(Convert.ToInt32(ddlpm.Value));
		if (Session["dashboard2_ddlpm"] == null)
			{
			var all_reports = NeMember.get_allreports(current_user.id);
			var is_member = "";
			if (all_reports.Rows.Count > 0)
				{
				foreach (DataRow dr in all_reports.Rows)
					{
					is_member += dr[0] + ",";
					}
				is_member = is_member.TrimEnd(',');

				Session["dashboard2_ddlpm"] = Toolbox.doSQL_dt(@"select member_id,member_fullname _name from member  where Member_Status = 'Active' and find_in_set(member_id,@v0 ) and member.member_membertype_id in (4,5,11,12,17,30,38) order by member_fullname", new object[] { is_member });
				}
			}
		ddlpm.DataSource = Session["dashboard2_ddlpm"];
		ddlpm.DataBind();
		if (ddlpm.Items.FindByValue(current_user.id) == null)
			{
			var li = new ListEditItem(current_user.FullName2, current_user.id);
			ddlpm.Items.Add(li);
			}


		}


	protected void btn_save_note_Click(object sender, EventArgs e)
		{
		var ddlpm = (ASPxComboBox)mypanel.FindControl("ddlpm");
		if (ddlpm.Value != null)
			{
			Toolbox.doSQL_void(@"update member set bonus_notes = @v0 where member_id = @v1", new object[] { ASPxMemo1.Text , ddlpm.Value});
			}

		}

	protected void myqtr_date_DateChanged(object sender, EventArgs e)
		{
		Session["gv_benchmark_dashboard"] = null;
		fill_details();
		fill_grid();
		}
	protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
		{
		Session["gv_benchmark_dashboard"] = null;
		fill_details();
		fill_grid();
		}

	}