using System;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Collections.Generic;
using nesi.core;
using nesi.core.print;
using System.Web;
using System.Linq;

public partial class sections_hr_member_member_offer : System.Web.UI.Page
{
    NeMember current_user;
    private int page_id = 127;
    bool can_see_wage;
    bool can_see_all_offers;
    bool can_edit_offers;
    private bool isowner = false;

    int memberid;
    int id;
    int comp_id;
    int isapplicant;
    int applicantid;
    Toolbox _tools;
    NeMember user;
    NeApplicant applicant;
    NeMemberOffer m;
    NameValueCollection _q;

    protected void Page_Init(object sender, EventArgs e)
    {
        _q = Request.QueryString;
        if (!string.IsNullOrEmpty(_q["id"]))  // if a q value is passed
        {
            Session["offerid"] = Convert.ToInt32(_q["id"]);
        }
        else
        {
            if (Session["offerid"] == null)
            {
                Session["offerid"] = 0;
            }
        }
        id = Convert.ToInt32(Session["offerid"]);
        if (id != 0) // if we're editing an existing offer
        {
            m = new NeMemberOffer(id);
        }
        hid_moid.Value = id.ToString();
        lblid.Text = id.ToString();
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(page_id);
        can_see_all_offers = current_user.AuthenticatedForPrivilege(152);
        can_edit_offers = current_user.AuthenticatedForPrivilege(129);
        SqlDataSource1.SelectCommand =
            "Select id business_unit_id, ddl_name name from business_unit  where active = 'T' and id in (" +
            new Current_User().visible_business_units + ") order by ddl_name";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_q["save_file_moid"]))
        {
            var mo_savefile_id = Convert.ToInt32(_q["save_file_moid"]);
            upload_offer_file(mo_savefile_id);
            Response.Clear();
            Response.Write("success");
            Response.Flush();
            return;
        }

        ASPxCallbackPanel1.JSProperties["cpServerMessage"] = null;

        memberid = string.IsNullOrEmpty(_q["memberid"]) || _q["memberid"] == "0" ? 0 : Convert.ToInt32(_q["memberid"]);
        applicantid = string.IsNullOrEmpty(_q["applicantid"]) || _q["applicantid"] == "0" ? 0 : Convert.ToInt32(_q["applicantid"]);

        if (!string.IsNullOrEmpty(_q["isapplicant"]) && (_q["isapplicant"].ToLower() == "true" || _q["isapplicant"].ToLower() == "false"))
        {
            isapplicant = _q["isapplicant"].ToLower() == "true" ? 1 : 0;
        }
        else
        {
            isapplicant = string.IsNullOrEmpty(_q["isapplicant"]) || _q["isapplicant"] == "0" ? 0 : Convert.ToInt32(_q["isapplicant"]);
        }
        btnaccepted.JSProperties["cp_warning"] = "WARNING! This will overwrite their existing employee data with the contents of this agreement, as well as close any employee reviews that are In Development.  It will also create a new employee review 1 month before the end date of this agreement";
        if (id != 0) // if we're editing an existing offer
        {
            m = new NeMemberOffer(id);
            txtstatus.Text = m.status;
            hid_moid.Value = m.id.ToString();
            if (!m.isapplicant)
            {
                dteStart.CalendarDayCellPrepared += new EventHandler<CalendarDayCellPreparedEventArgs>(dteStart_CalendarDayCellPrepared);
                btnreset.ClientEnabled = true;
                memberid = m.memberid;
                user = new NeMember(memberid);
                isowner = NeMember.is_owner(current_user.id, user.business_unit.tax_entity_id);
                comp_id = user.business_unit_id;


                //				if (user.business_unit_id != current_user.business_unit_id && user.business_unit_id != 0 && !can_see_all_offers)
                if (user.business_unit_id != current_user.business_unit_id && !isowner && !NeMember.is_supervisor(user.id, current_user.id) && current_user.id != user.id && !can_see_all_offers)
                {
                    Toolbox.FriendlyException(Response, "You can only access users from your branch", "./index.aspx?id=");
                }
            }
            else
            {
                btnaccepted.JSProperties["cp_warning"] = "";
                applicant = new NeApplicant(m.applicantid);
                comp_id = applicant.business_unit_id;
                hdnmtype.Value = m.membertypeid.ToString();
                isapplicant = 1;
                applicantid = applicant.id;
                memberid = 0;
            }


            if (!m.isapplicant && !NeMember.is_supervisor(user.id, current_user.id))  // if the current user is over the offer_user, let them through no matter what
            {
                if (current_user.id != new NeBusinessUnit(m.business_unit_id).branch_manager.id && m.enteredby != current_user.id && !can_see_all_offers && m.memberid != current_user.id && m.reports_to != current_user.id && user.reports_to != current_user.id && !isowner)
                {
                    Toolbox.FriendlyException(Response, "Sorry, you do not have the credentials to view this page", "./index.aspx?id=");
                }
                else
                {
                    if (!can_see_all_offers && m.memberid == current_user.id && current_user.reports_to != 0)
                    {
                        Toolbox.FriendlyException(Response, "Sorry, you do not have the credentials to view this page", "./index.aspx?id=");
                    }

                }

            }
            if (!can_edit_offers)
            {
                Toolbox.FriendlyException(Response, "Sorry, you do not have the credentials to view this page", "./index.aspx?id=");
            }

        }
        else
        {
            hid_moid.Value = "0";
            if (applicantid != 0)
            {
                btnaccepted.JSProperties["cp_warning"] = "";
                applicant = new NeApplicant(applicantid);
            }
            else
            {
                btnreset.ClientEnabled = true;
                user = new NeMember(memberid);
            }
        }



        var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
        can_see_wage = current_user.AuthenticatedForPrivilege(101);

        if (!IsPostBack)
        {
            populate_newform();
        }
        setup_buttons();

        sqlmember.SelectCommand = user == null ? @"call get_visible_users(" + current_user.id + ")" : @"call get_visible_users(" + user.id + ")";
        ddlreportsto.DataBind();

        ddlreportsto.Items.Add(new ListEditItem("Board of Directors", 0));




    }
    protected void populate_newform()
    {

        var chkcomp = (ASPxCheckBox)pnl_compplan.FindControl("chkhascomp");

        #region EDIT OFFER
        //	btnaction.AutoPostBack = false;
        if (hid_moid.Value != "0" && (applicant != null || user != null))  // if it's an existing offer.
        {
            var mo = new NeMemberOffer(id);
            m = mo;
            lblid.Text = id.ToString();
            Session["offerid"] = id;
            btnSave.Text = "Save";
            var branch = new NeBusinessUnit(mo.business_unit_id);
            ddlmembertype.Value = Convert.ToInt32(mo.membertypeid);
            var mt = new NeMemberType(mo.membertypeid);
            ddlcompany.Value = mo.business_unit_id;
            if (mo.isapplicant)
            {
                txtEmpName.Text = applicant.firstname + " " + applicant.lastname;

            }
            else
            {


                txtEmpName.Text = user != null ? user.FullName : "";
            }


            txtstatus.Text = mo.status;
            ASPxPageControl1.TabPages[1].ClientEnabled = true;
            ASPxPageControl1.TabPages[2].ClientEnabled = true;
            ASPxPageControl1.TabPages[3].ClientEnabled = true;
            ASPxPageControl1.TabPages[4].ClientEnabled = true;
            gv_mocr.DataBind();
            hdnmtype.Value = mo.membertypeid.ToString();
            dteStart.Date = mo.startdate;
            if (user != null && !string.IsNullOrEmpty(user.StartDate))
            {
                //	dteStart.MinDate = Convert.ToDateTime(user.StartDate);
            }
            dteEnd.Date = mo.enddate;


            memdevnotes.Text = _tools.getSQL_string(@"select member_offers.redo_notes from member_offers  where id =@v0", new object[] { Session["offerid"] });
            lblauthor.Text = new NeMember(Convert.ToInt32(mo.enteredby)).FullName;
            #region report-to
            ddlreportsto.Value = Convert.ToInt32(mo.reports_to);
            #endregion

            #region wage section


            txtwage.Text = mo.wage.ToString();
            ddlpaytype.Value = mo.paytype_id == 0 ? 1 : mo.paytype_id;
            //	chksalary.Checked = mo.is_salary;



            //	lbllabourmargin.Text = "Margin: " + Math.Round((mt.GetChargeoutforbranch(mo.business_unit_id) - mo.wage * 1.15) / mt.GetChargeoutforbranch(mo.business_unit_id) * 100, 0) + "% on $ " + mt.GetChargeoutforbranch(mo.business_unit_id) + " chargeout";
            lblprevious_wage.Text = "";
            try
            {
                if (user != null)
                {
                    var wage = new NeWage(user.id);
                    if (wage.current_wage != 0 && mo.wage != 0)
                    {
                        lblprevious_wage.Text = wage.current_wage.ToString("C2");
                    }
                }
            }
            catch { }

            update_wage_note(ddlmembertype.Value + ":" + mo.wage + ":" + mo.business_unit_id);

            lblprevious_wage0.Text = user == null ? "" : Convert.ToDateTime(user.StartDate).ToString("yyyy-MM-dd") + " at " + _tools.getSQL_double(@"select ifnull((Select currentwage from memberwage  where memberwage_memberid =@v0 order by date limit 1),0)", new object[] { user.id }).ToString("C2");

            var minwage = _tools.getSQL_double(@"SELECT ifnull(min(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", new object[] { mo.membertypeid, mo.business_unit_id });
            var maxwage = _tools.getSQL_double(@"SELECT ifnull(max(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", new object[] { mo.membertypeid, mo.business_unit_id });
            var avgwage = _tools.getSQL_double(@"SELECT ifnull(avg(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", new object[] { mo.membertypeid, mo.business_unit_id });

            lblwagerange.Text = minwage.ToString("C2") + " - " + maxwage.ToString("C2") + "....avg (" + avgwage.ToString("C2") + ")";
            minwage = _tools.getSQL_double(@"SELECT ifnull(min(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", new object[] { mo.membertypeid });
            maxwage = _tools.getSQL_double(@"SELECT ifnull(max(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", new object[] { mo.membertypeid });
            avgwage = _tools.getSQL_double(@"SELECT ifnull(avg(m.WAGE),0) from currentwage m,membertype  where m.member_status = 'Active' and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", new object[] { mo.membertypeid });
            lblwagerange0.Text = minwage.ToString("C2") + " - " + maxwage.ToString("C2") + "....avg (" + avgwage.ToString("C2") + ")";




            #endregion
            #region vacation

            if (new NeBusinessUnit(mo.business_unit_id).country == "CDN")
            {
                txtvac1amt.Text = mo.vacation_amount_1.ToString("P1");
                txtvac2amt.Text = mo.vacation_amount_2.ToString("P1");
                txtvac3amt.Text = mo.vacation_amount_3.ToString("P1");
                lbl_vac_1int.Text = "Vacation Level 1 Percentage of Earning";
                lbl_vac_2int.Text = "Vacation Level 2 Percentage of Earning";
                lbl_vac_3int.Text = "Vacation Level 3 Percentage of Earning";
            }
            else
            {
                txtvac1amt.Text = mo.vacation_amount_1.ToString();
                txtvac2amt.Text = mo.vacation_amount_2.ToString();
                txtvac3amt.Text = mo.vacation_amount_3.ToString();
            }
            txtvac1int.Text = mo.vacation_interval_1.ToString() == "" ? "0" : mo.vacation_interval_1.ToString();
            txtvac2int.Text = mo.vacation_interval_2.ToString() == "" ? "60" : mo.vacation_interval_2.ToString();
            txtvac3int.Text = mo.vacation_interval_3.ToString() == "" ? "120" : mo.vacation_interval_3.ToString();
            #endregion
            #region comp
            memcompdetails.Text = mo.comp_details;
            chk_part_time.Checked = mo.part_time;
            if (mo.status == "In Development")
            {
                spn_bonus_amt.ClientEnabled = true;
                ddlbonus.ClientEnabled = true;
                pnl_compplan.Enabled = true;
            }

            if (mo.paytype_id <= 3)
            {
                pnl_compplan.ClientVisible = true;
                pnl_vacation.ClientVisible = true;
                pnl_subcontractor_stuff.ClientVisible = false;
                pnl_not_subcontractor_stuff.ClientVisible = true;
            }
            else
            {
                pnl_compplan.ClientVisible = false;
                pnl_vacation.ClientVisible = false;
                pnl_subcontractor_stuff.ClientVisible = true;
                pnl_not_subcontractor_stuff.ClientVisible = false;
                ddlsubcontractor.Value = mo.vendor_id;
                mem_contract_details.Text = mo.contract_details;
                txtwage_sub.Text = mo.wage.ToString();
            }
            var dt_bonus = Toolbox.doSQL_dt(@"Select 0 id,'Not  Set' bonus_type union SELECT bonus_type.id, bonus_type.bonus_type FROM bonus_type INNER JOIN bonus_membertype_link ON bonus_membertype_link.bonus_type_id = bonus_type.id and bonus_membertype_link.membertype_id =@v0  and bonus_type.status = 1", new object[] { ddlmembertype.Value });
            ddlbonus.DataSource = dt_bonus;
            ddlbonus.DataBind();
            if (dt_bonus.Rows.Count > 0)
            {
                ddlbonus.Value = mo.bonus_type;
                if (ddlbonus.Value.ToString() == "3")
                {


                    var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
                    row_bonus.Visible = true;
                    var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
                    row_rev.Visible = true;
                    var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
                    row_mar.Visible = true;
                    var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
                    row_net.Visible = false;
                    var row_net_hw = (HtmlControl)cb_bonus.FindControl("row_net_hw");
                    row_net_hw.Visible = false;

                    spn_bonus_amt.Value = Convert.ToDecimal(mo.bonus_amount);
                    txt_bonus_margin_threshold.Text = mo.bonus_margin_threshold.ToString("C2");
                    txt_bonus_revenue_threshold.Text = mo.bonus_revenue_threshold.ToString("C2");

                    memcompdetails.Text = mo.comp_details;
                    memcompdetails.ReadOnly = true;
                    if (Convert.ToString(ddlbonus.Value) != "" && (int)ddlbonus.Value != 0)
                    {
                        spn_bonus_amt.MinValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(min_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
                        spn_bonus_amt.MaxValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(max_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
                        spn_bonus_amt.ToolTip = _tools.getSQL_string(@"select tooltip from bonus_type  where id =@v0", new object[] { ddlbonus.Value });
                        //	lbl_default_bonus_verbiage.InnerHtml = _tools.getSQL_string(@"select offer_verbiage from bonus_type  where id =@v0" , Convert.ToDouble(spn_bonus_amt.Value).ToString("P2"), new object[] { ddlbonus.Value).Replace("zzz });
                    }

                }
                else if (ddlbonus.Value.ToString() == "10")
                {
                    var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
                    row_bonus.Visible = false;
                    var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
                    row_rev.Visible = false;
                    var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
                    row_mar.Visible = false;
                    var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
                    row_net.Visible = true;
                    var row_net_hw = (HtmlControl)cb_bonus.FindControl("row_net_hw");
                    row_net_hw.Visible = true;
                    memcompdetails.ReadOnly = true;
                    txt_bonus_netincome_threshold.Text = mo.bonus_netincome_threshold.ToString("C2");
                    txt_bonus_hw.Text = mo.bonus_netincome_highwater.ToString("C2");
                }
                update_comp_verbiage();
            }
            else
            {
                bonus1.Visible = false;

            }
            dte_benefits_startdate.Text = mo.benefits_startdate == null ? "" : Convert.ToDateTime(mo.benefits_startdate).ToString("yyyy-MM-dd");

            #endregion
            #region milestones
            fill_milestones();


            #endregion
            #region other details
            chkcell.Checked = mo.gets_phone;
            chklaptop.Checked = mo.gets_laptop;
            chkvehicle.Checked = mo.gets_vehicle;

            chkemail.Checked = mo.gets_neemail;
            chkbarcode.Checked = mo.gets_barcodescanner;
            chkbusinesscards.Checked = mo.gets_businesscards;
            chkdirectdeposit.Checked = mo.gets_directdeposit;
            chkemail.Checked = mo.gets_neemail;
            chkphoneext.Checked = mo.gets_phoneext;

            memnotes.Text = mo.notes;
            #endregion




            #region buttons
            /*
			if (mo.status == "In Development")
			{
				btnaccepted.ClientVisible = false;
				if (lblid.Text == "0")
				{
					btnexpire.ClientVisible = false;
				}
				else
				{
					btnSave.ClientEnabled = true;
					btnprint.ClientEnabled = false;
					spn_bonus_amt.ClientEnabled = true;
					ddlbonus.ClientEnabled = true;
					pnl_compplan.Enabled = true;
					if ((memcompdetails.Text == "") ||
						(current_user.AuthenticatedForPrivilege(130)) ||
						NeMember.is_supervisor(mo.reports_to, current_user.id))  // if the offer can go straight through...
					{
						btnSave.Text = "Save";
						btnaction.Text = "Revoke";
						btnaction.ClientVisible = true;
						btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('r');}";
						btnSave.ClientEnabled = true;
						btnprint.Text = "Print and Release";
						btnprint.ClientEnabled = true;
					}
					else
					{
						btnaction.Text = "Send for Approval";
						btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('a');}";
						btnprint.Text = "Print and Release";
						btnprint.ClientEnabled = false;
						btnaction.ClientVisible = true;
					}
					btnexpire.ClientVisible = true;
				}

			}
			else if (mo.status == "Waiting for Approval")
			{
				btnaccepted.ClientVisible = false;
				btnSave.Text = "Save";

				btnprint.Text = "Print";
				btnaccepted.ClientVisible = false;

				// if its a nesi member or a supervisor of the branch manager of the branch allow th approval

				if (current_user.AuthenticatedForPrivilege(130) ||
						NeMember.is_supervisor(mo.reports_to, current_user.id))
				{
					spn_bonus_amt.ClientEnabled = true;
					ddlbonus.ClientEnabled = true;
					btnaction.Text = "Approve";
					btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('p');}";
					btnaction.ClientVisible = true;
					btnredo.ClientVisible = true;
					btnprint.ClientEnabled = true;
				}
				else
				{
					btnSave.ClientEnabled = false;
					btnaction.ClientVisible = false;
					pnl_compplan.Enabled = false;
				}


			}
			else if ((mo.status == "Approved"))
			{
				btnaccepted.ClientVisible = false;
				btnSave.Text = "Save";
				btnaction.Text = "Revoke";
				btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('r');}";
				btnaction.ClientVisible = true;
				btnSave.ClientEnabled = false;
				btnprint.Text = "Print and Release";
				btnprint.ClientEnabled = true;
				pnl_compplan.Enabled = false;
			}
			else if (mo.status == "Released")
			{
				btnSave.Text = "Save";
				if (isapplicant == 0)
				{
					btnaccepted.Text = "Accepted";
				}
				btnaccepted.ClientVisible = true;
				btnprint.Text = "Print";
				btnprint.ClientEnabled = true;
				btnSave.ClientEnabled = false;
				pnl_compplan.Enabled = false;
				ASPxPageControl1.TabPages[5].ClientEnabled = true;
				iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant;

			}
			else if (mo.status == "Revoked")
			{
				btnaccepted.ClientVisible = false;
				btnSave.Text = "Save";
				btnSave.ClientEnabled = false;
				btnaction.Text = "Revive";
				btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('v');}";
				btnaction.ClientVisible = true;
				btnprint.ClientEnabled = false;
			}
			else if (mo.status == "Closed")
			{
				btnSave.Text = "Save";
				btnSave.ClientEnabled = false;
				btnaccepted.ClientVisible = false;
				btnexpire.ClientVisible = false;
				btnaction.Text = "Revive";
				btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('v');}";
				btnaction.ClientVisible = true;
				btnprint.ClientEnabled = false;
				pnl_compplan.Enabled = false;
			}
			else if (mo.status == "Accepted" || mo.status == "Awaiting Start Date")
			{
				btnaccepted.ClientVisible = false;
				btnexpire.ClientVisible = false;
				//btnaction.Text = "Revive";
				btnaction.ClientVisible = false;
				btnprint.ClientEnabled = true;
				btnSave.Text = "Save";
				btnSave.ClientEnabled = false;
				ASPxPageControl1.TabPages[5].ClientEnabled = true;
				pnl_compplan.Enabled = false;
				iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant;
			}
			else if (mo.status == "Previous")
			{
				btnaccepted.ClientVisible = false;
				btnexpire.ClientVisible = false;
				//btnaction.Text = "Revive";
				btnaction.ClientVisible = false;
				btnprint.ClientEnabled = true;
				btnSave.Text = "Save";
				btnSave.ClientEnabled = false;
				ASPxPageControl1.TabPages[5].ClientEnabled = true;
				pnl_compplan.Enabled = false;
				iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant;
			}
			*/
            #endregion


        }
        #endregion
        #region NEW OFFER
        else  // if its a new offer
        {

            var f_date = new DateTime();
            #region existing employee
            if (isapplicant == 0)// if its a member
            {
                var company = new NeBusinessUnit(user.business_unit_id);
                //	var business_id		= Toolbox.doSQL_int(@"SELECT IFNULL(MIN(id),0) FROM business WHERE number = @v0 ", new object[] {  company.BusinessNumber } );
                if (memberid != 0)  // if its a member... and a new offer.
                {
                    lblid.Text = "0";
                    ddlmembertype.Value = Convert.ToInt32(user.MemberTypeID);
                    ddlcompany.Value = user.business_unit_id;
                    txtstatus.Text = "In Development";
                    btnexpire.ClientVisible = false;
                    dteStart.MinDate = Convert.ToDateTime(user.StartDate);


                    f_date = Convert.ToDateTime(_tools.getSQL_string(@"SELECT get_fiscal_year_start_date(@v0)", new object[] { user.business_unit_id })).AddYears(1);

                    txtEmpName.Text = user.FullName;
                    lblauthor.Text = current_user.FullName;
                    btnSave.Text = "Next ->";
                    #region report-to
                    if (user.reports_to != 0)
                    {
                        ddlreportsto.Value = user.reports_to;
                    }

                    var x = _tools.getSQL_int(@"Select ifnull((Select reports_to from member_offers  where memberid =@v0 order by id desc limit 1),0)", new object[] { memberid });
                    if (x != 0)
                    {
                        ddlreportsto.Value = x;
                    }
                    else
                    {
                        var comp = new NeBusinessUnit(user.business_unit_id);
                        ddlreportsto.Value = comp.branch_manager.id;
                    }
                    #endregion
                    #region wage section
                    var wage = new NeWage(user.id);
                    txtwage.Text = wage.current_wage.ToString();
                    txtwage_sub.Text = wage.current_wage.ToString();
                    //	chksalary.Checked = user.PayType_Name == "Salary" ? true : false;
                    lbllabourmargin.Text = "Margin: " + Math.Round((user.chargeout - wage.current_wage * 1.15) / user.chargeout * 100, 0) + "% on $ " + user.chargeout + " per hour";
                    ddlbonus.Value = wage.bonus_type;
                    spn_bonus_amt.Value = wage.bonus_amount;
                    #endregion
                    #region vacation
                    if (user.business_unit.country == "USA")
                    {
                        txtvac1amt.Text = user.vacation_amount_1.ToString();
                        txtvac2amt.Text = user.vacation_amount_2.ToString();
                        txtvac3amt.Text = user.vacation_amount_3.ToString();
                    }
                    else
                    {
                        txtvac1amt.Text = user.vacation_amount_1.ToString("P1");
                        txtvac2amt.Text = user.vacation_amount_2.ToString("P1");
                        txtvac3amt.Text = user.vacation_amount_3.ToString("P1");
                        lbl_vac_1int.Text = "Vacation Level 1 Percentage of Earning";
                        lbl_vac_2int.Text = "Vacation Level 2 Percentage of Earning";
                        lbl_vac_3int.Text = "Vacation Level 3 Percentage of Earning";
                    }
                    txtvac1int.Text = user.vacation_interval_1.ToString();
                    txtvac2int.Text = user.vacation_interval_2.ToString();
                    txtvac3int.Text = user.vacation_interval_3.ToString();
                    #endregion
                    #region comp
                    ddlpaytype.Value = user.paytype_id == 0 ? 1 : user.paytype_id;
                    chk_part_time.Checked = user.part_time;

                    if (user.paytype_id > 3)
                    {
                        dte_benefits_startdate.Date = System.DateTime.Today.AddYears(5);
                        dte_benefits_startdate.ClientEnabled = false;
                    }
                    else
                    {
                        dte_benefits_startdate.ClientEnabled = true;
                        dte_benefits_startdate.Date = Convert.ToDateTime(user.benefits_startdate);
                    }

                    var old_moid = _tools.getSQL_int(@"Select ifnull((Select id from member_offers  where memberid =@v0 and (status = 'Accepted' or status = 'Previous') order by id desc limit 1),0)", new object[] { user.id });
                    if (old_moid != 0)
                    {
                        var old_mo = new NeMemberOffer(old_moid);
                        var pp_id = NePayPeriod.get_payperiod_id(old_mo.enddate);
                        var pp = new NePayPeriod(pp_id);
                        dteStart.Date = Convert.ToDateTime(pp.Enddate).AddDays(1);

                        if (old_mo.has_comp == 1)
                        {
                            ddlbonus.Value = old_mo.bonus_type;
                            if (chkcomp != null)
                            {
                                chkcomp.Checked = true;
                            }

                            if (user.paytype_id <= 3)
                            {
                                pnl_compplan.ClientVisible = true;
                                pnl_vacation.ClientVisible = true;
                                pnl_subcontractor_stuff.ClientVisible = false;
                                pnl_not_subcontractor_stuff.ClientVisible = true;
                            }
                            else  // if the user is a subcontractor
                            {
                                pnl_compplan.ClientVisible = false;
                                pnl_vacation.ClientVisible = false;
                                pnl_subcontractor_stuff.ClientVisible = true;
                                pnl_not_subcontractor_stuff.ClientVisible = false;
                            }

                            txt_bonus_margin_threshold.Text = old_mo.bonus_margin_threshold.ToString("C2");
                            txt_bonus_netincome_threshold.Text = old_mo.bonus_netincome_threshold.ToString("C2");
                            txt_bonus_revenue_threshold.Text = old_mo.bonus_revenue_threshold.ToString("C2");
                            memcompdetails.Text = old_mo.comp_details;

                            if (ddlbonus.Value.ToString() == "3")
                            {
                                var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
                                row_bonus.Visible = true;
                                var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
                                row_rev.Visible = true;
                                var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
                                row_mar.Visible = true;
                                var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
                                row_net.Visible = false;

                                spn_bonus_amt.Value = Convert.ToDecimal(old_mo.bonus_amount);
                                memcompdetails.ReadOnly = true;
                                if (Convert.ToString(ddlbonus.Value) != "" && (int)ddlbonus.Value != 0)
                                {
                                    spn_bonus_amt.MinValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(min_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
                                    spn_bonus_amt.MaxValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(max_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
                                    spn_bonus_amt.ToolTip = _tools.getSQL_string(@"select tooltip from bonus_type  where id =@v0", new object[] { ddlbonus.Value });
                                    //	lbl_default_bonus_verbiage.InnerHtml = _tools.getSQL_string(@"select offer_verbiage from bonus_type  where id =@v0" , Convert.ToDouble(spn_bonus_amt.Value).ToString("P2"), new object[] { ddlbonus.Value).Replace("zzz });
                                }

                            }
                            else if (ddlbonus.Value.ToString() == "10")
                            {
                                var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
                                row_bonus.Visible = false;
                                var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
                                row_rev.Visible = false;
                                var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
                                row_mar.Visible = false;
                                var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
                                row_net.Visible = true;
                                memcompdetails.ReadOnly = true;
                                txt_bonus_netincome_threshold.Text = old_mo.bonus_netincome_threshold.ToString("C2");

                            }
                        }
                        else
                        {
                            if (chkcomp != null)
                            {
                                chkcomp.Checked = false;
                            }

                            txt_bonus_margin_threshold.Text = "$0.00";
                            txt_bonus_netincome_threshold.Text = "$0.00";
                            txt_bonus_revenue_threshold.Text = "$0.00";
                            memcompdetails.Text = "";
                        }

                    }
                    #endregion

                    #region milestones

                    #endregion

                    #region other details

                    chkcell.Checked = user.gets_phone;
                    chklaptop.Checked = user.gets_laptop;
                    chkvehicle.Checked = user.gets_vehicle;
                    chkbarcode.Checked = user.gets_barcodescanner;
                    chkbusinesscards.Checked = user.gets_businesscards;
                    chkdirectdeposit.Checked = user.gets_directdeposit;
                    chkemail.Checked = user.gets_neemail;
                    chkphoneext.Checked = user.gets_phoneext;


                    #endregion
                }
            }
            #endregion
            #region new applicant
            else// if its an applicant
            {
                var company = new NeBusinessUnit(applicant.business_unit_id);
                //	var business_id		= Toolbox.doSQL_int(@"SELECT IFNULL(MIN(id),0) FROM business WHERE number = @v0 ", new object[] {  company.BusinessNumber } );
                lblid.Text = "0";
                txtEmpName.Text = applicant.firstname + " " + applicant.lastname;
                var mt = new NeMemberType(applicant.membertypeid);

                ddlmembertype.Value = Convert.ToInt32(applicant.membertypeid);
                ddlcompany.Value = Convert.ToInt32(applicant.business_unit_id);
                txtstatus.Text = "In Development";
                btnexpire.ClientVisible = false;
                dteStart.Date = System.DateTime.Today;

                f_date = Convert.ToDateTime(_tools.getSQL_string(@"SELECT get_fiscal_year_start_date(@v0)", new object[] { applicant.business_unit_id })).AddYears(1);


                #region report-to
                var x = Toolbox.doSQL_int(@"Select ifnull((Select reports_to from membertype where membertype_id =@v0 limit 1),0) ", new object[] {
                    ddlmembertype.Value});
                if (x != 0)
                {
                    var y = Toolbox.doSQL_int(@"Select ifnull((Select member_id from member where 
member_membertype_id = @v0 and business_unit_id = @v1 and member_status ='Active' limit 1),0)",
                        new object[] {
 x,applicant.business_unit_id
}
);
                    if (y == 0)
                    {
                        y = current_user.id;
                    }
                    ddlreportsto.Value = y;
                }
                else
                {
                    var comp = new NeBusinessUnit(applicant.business_unit_id);
                    ddlreportsto.Value = comp.branch_manager.id;
                }
                #endregion
                #region wage section
                //	NeWage wage = new NeWage(user.id);
                txtwage.Text = mt.GetAvgBranchCost(applicant.membertypeid, applicant.business_unit_id).ToString("C2");
                //		chksalary.Checked = false;
                lbllabourmargin.Text = "";
                ddlbonus.Value = 0;
                spn_bonus_amt.Value = 0;
                dte_benefits_startdate.Date = dteStart.Date.AddDays(mt.benefits_start_default * 7);

                #endregion
                #region vacation
                if (new NeBusinessUnit(applicant.business_unit_id).country == "CDN")
                {
                    txtvac1amt.Text = "4.0%";
                    txtvac2amt.Text = "6.0%";
                    txtvac3amt.Text = "8.0%";
                    txtvac1int.Text = "0";
                    txtvac2int.Text = "60";
                    txtvac3int.Text = "120";
                    lbl_vac_1int.Text = "Vacation Level 1 Percentage of Earning";
                    lbl_vac_2int.Text = "Vacation Level 2 Percentage of Earning";
                    lbl_vac_3int.Text = "Vacation Level 3 Percentage of Earning";
                }
                else
                {
                    txtvac1amt.Text = "40";
                    txtvac2amt.Text = "80";
                    txtvac3amt.Text = "120";
                    txtvac1int.Text = "12";
                    txtvac2int.Text = "36";
                    txtvac3int.Text = "60";
                }

                #endregion
                #region comp
                if (applicant.country == "USA")
                {
                    dte_benefits_startdate.Date = new DateTime(dteStart.Date.AddDays(90).AddMonths(1).Year, dteStart.Date.AddDays(90).AddMonths(1).Month, 1);
                }
                else
                {
                    dte_benefits_startdate.Date = dteStart.Date.AddMonths(3);
                }
                /*			if (applicant. > 3)
                            {
                                dte_benefits_startdate.Date = System.DateTime.Today.AddYears(5);
                                dte_benefits_startdate.ClientEnabled = false;
                            }
                            else
                            {
                                dte_benefits_startdate.ClientEnabled = true;
                                if (applicant.country == "USA")
                                {
                                    dte_benefits_startdate.Date = new DateTime(dteStart.Date.AddDays(60).AddMonths(1).Year, dteStart.Date.AddDays(60).AddMonths(1).Month, 1);
                                }
                                else
                                {
                                    dte_benefits_startdate.Date = dteStart.Date.AddMonths(6);
                                }
                            }
            */

                #endregion
                #region other details
                #endregion

            }

            if (f_date.Subtract(System.DateTime.Today).TotalDays < 45 || f_date < dteStart.Date)
            {
                var temp_f_date = f_date;
                f_date = Convert.ToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(f_date.AddYears(1))).Enddate);
                if (f_date < dteStart.Date)
                {
                    f_date = Convert.ToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(temp_f_date.AddYears(2))).Enddate);
                }

                dteEnd.Date = f_date;

            }
            else
            {
                f_date = Convert.ToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(f_date)).Enddate);
                dteEnd.Date = f_date;
            }


            ddlmembertype.DataBind();

        }
        #endregion
        #endregion
        setup_buttons();

    }
    protected void cb1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {



        if (e.Parameter != null)
        {
            update_wage_note(e.Parameter);
        }
    }

    protected void update_wage_note(string parameter)
    {
        if (parameter.Contains(":"))
        {
            var mtype = parameter.Split(':').GetValue(0).ToString();
            var _wage = parameter.Split(':').GetValue(1).ToString();
            var _cid = parameter.Split(':').GetValue(2).ToString();
            try
            {

                var _chargeout =
                    _tools.getSQL_double(
                        @"Select ifnull((Select chargeout from membertype_chargeout  where paytype_id = 1 and membertype_id =@v0 and business_unit_id =@v1),0) ",
                        new object[] { mtype, _cid });
                if (_chargeout > 0)
                {
                    if (ddlpaytype.Value.ToString() == "2" || ddlpaytype.Value.ToString() == "3")
                    {
                        var salarywage = Math.Round(Convert.ToDouble(_wage) * 20.80, 0) * 100;
                        lbllabourmargin.Text = "Salary of " + salarywage.ToString("C0") + System.Environment.NewLine +
                                               "Margin: " +
                                               Math.Round((_chargeout - Convert.ToDouble(_wage) * 1.15) / _chargeout * 100, 0) +
                                               "% on $ " + _chargeout + " per hour";
                        if (lblprevious_wage.Text != "")
                        {
                            lbllabourmargin.Text += System.Environment.NewLine +
                                                    ((Convert.ToDouble(_wage) -
                                                      Convert.ToDouble(lblprevious_wage.Text.Replace("$", ""))) /
                                                     Convert.ToDouble(_wage)).ToString("P2") + " Increase";
                        }
                        //+ "  " + ((mo.wage - wage.current_wage) / mo.wage).ToString("P2") + " Increase";	
                    }
                    else
                    {
                        lbllabourmargin.Text = "Margin: " +
                                               Math.Round((_chargeout - Convert.ToDouble(_wage) * 1.15) / _chargeout * 100, 0) +
                                               "% on $ " + _chargeout + " per hour";
                        if (lblprevious_wage.Text != "")
                        {
                            lbllabourmargin.Text += System.Environment.NewLine +
                                                    ((Convert.ToDouble(_wage) -
                                                      Convert.ToDouble(lblprevious_wage.Text.Replace("$", ""))) /
                                                     Convert.ToDouble(_wage)).ToString("P2") + " Increase";
                        }
                    }
                }

            }
            catch
            {
                lbllabourmargin.Text = "";
            }

        }

    }

    protected void ddlmembertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        var cb = (ASPxComboBox)sender;




        // Matt: This was just setting the text, not the value. 
        // When setting the text and not the value, the value becomes the text if the text doesn't exist in the items.
        // This was throwing an "Input String Exception" when the title was changed, and the next button pressed, per ticket 4663
        // I've switched this over to using the value, versus the text... 
        var x = Toolbox.doSQL_int(@"select ifnull((SELECT 
member.member_id 
FROM
membertype
INNER JOIN member ON membertype.reports_to = member.Member_MemberType_ID
INNER JOIN business_unit ON member.business_unit_id = business_unit.id 
WHERE
membertype.membertype_id =@v0  AND
member.business_unit_id =@v1  AND
member.Member_Status = 'Active' limit 1),0)",
            new object[] {cb.Value,comp_id
}
);
        if (x != 0)
        {

            ddlreportsto.Value = x;

        }
        else
        {
            if (isapplicant == 0)
            {
                var branch = new NeBusinessUnit(user.business_unit_id);
                ddlreportsto.Value = branch.branch_manager.id;
            }
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        var user = new NeMember(memberid);
        memberid = user.id;
        lblid.Text = id.ToString();
        txtEmpName.Text = user.FullName2;
        ddlmembertype.Value = Convert.ToInt32(user.MemberTypeID);
        ddlcompany.Value = user.business_unit_id;
        txtstatus.Text = "In Development";
        dteStart.Date = System.DateTime.Today;
        dteEnd.Date = System.DateTime.Today.AddYears(1);

        #region report-to

        ddlreportsto.Value = user.reports_to;


        #endregion
        #region wage section

        var wage = new NeWage(user.id);
        txtwage.Text = wage.current_wage.ToString();

        //	chksalary.Checked = ddlpaytype.Value.ToString() == "2" || ddlpaytype.Value.ToString() == "3" ? true : false;
        lbllabourmargin.Text = "Margin: " + Math.Round((user.chargeout - wage.current_wage * 1.15) / user.chargeout * 100, 0) + "% on $ " + user.chargeout + " per hour";

        #endregion
        #region vacation
        if (user.business_unit.country == "USA")
        {
            txtvac1amt.Text = user.vacation_amount_1.ToString();
            txtvac2amt.Text = user.vacation_amount_2.ToString();
            txtvac3amt.Text = user.vacation_amount_3.ToString();
        }
        else
        {
            txtvac1amt.Text = user.vacation_amount_1.ToString("P1");
            txtvac2amt.Text = user.vacation_amount_2.ToString("P1");
            txtvac3amt.Text = user.vacation_amount_3.ToString("P1");
        }
        txtvac1int.Text = user.vacation_interval_1.ToString();
        txtvac2int.Text = user.vacation_interval_2.ToString();
        txtvac3int.Text = user.vacation_interval_3.ToString();
        #endregion
        #region comp
        memcompdetails.Text = user.comp_details;
        //		ASPxCheckBox chk = (ASPxCheckBox)pnl_compplan.FindControl("chkhascomp");
        //		chk.Checked = Convert.ToBoolean(user.has_comp);


        #endregion
        #region other details
        chkcell.Checked = user.gets_phone;
        chklaptop.Checked = user.gets_laptop;
        chkvehicle.Checked = user.gets_vehicle;
        chk_part_time.Checked = user.part_time;
        chkbarcode.Checked = user.gets_barcodescanner;
        chkbusinesscards.Checked = user.gets_businesscards;
        chkdirectdeposit.Checked = user.gets_directdeposit;
        chkemail.Checked = user.gets_neemail;
        chkemail.Checked = user.gets_neemail;
        chkphoneext.Checked = user.gets_phoneext;

        memnotes.Text = user.offer_notes;
        #endregion

    }
    protected void save()
    {
        validate();
        NeMemberOffer mo;
        var new_offer_trigger = false;
        var new_app_offer = false;

        if (id != 0)  // if its an existing offer
        {
            mo = new NeMemberOffer(Convert.ToInt32(id));

        }
        else
        {
            new_offer_trigger = true;
            mo = new NeMemberOffer();
            mo.enteredby = current_user.id;
            if (isapplicant == 1)
            {
                new_app_offer = true;
                mo.isapplicant = true;
            }


        }
        mo.id = id;

        if (mem_contract_details.Text == "")
        {
            //	cb_bonus.FindControl("memcompdetails")
            mo.comp_details = memcompdetails.Text;
        }
        else
        {
            mo.comp_details = "";
        }

        //		ASPxCheckBox chk = (ASPxCheckBox)pnl_compplan.FindControl("chkhascomp");

        mo.has_comp = mo.comp_details == "" ? 0 : 1;
        mo.business_unit_id = Convert.ToInt32(ddlcompany.Value);
        var branch = new NeBusinessUnit(mo.business_unit_id);

        mo.date = System.DateTime.Today.Date;
        mo.enddate = dteEnd.Date;
        mo.gets_laptop = chklaptop.Checked;
        mo.gets_phone = chkcell.Checked;
        mo.gets_vehicle = chkvehicle.Checked;

        mo.gets_barcodescanner = chkbarcode.Checked;
        mo.gets_businesscards = chkbusinesscards.Checked;
        mo.gets_directdeposit = chkdirectdeposit.Checked;
        mo.gets_neemail = chkemail.Checked;
        mo.gets_phoneext = chkphoneext.Checked;

        mo.is_salary = ddlpaytype.Value == null ? false : ddlpaytype.Value.ToString() == "2" || ddlpaytype.Value.ToString() == "3" ? true : false;
        mo.paytype_id = ddlpaytype.Value == null || Convert.ToInt32(ddlpaytype.Value) == 0 ? 1 : Convert.ToInt32(ddlpaytype.Value);
        mo.part_time = chk_part_time.Checked;
        mo.membertypeid = Convert.ToInt32(ddlmembertype.Value);
        mo.notes = memnotes.Text;
        mo.reports_to = Convert.ToInt32(ddlreportsto.Value);
        mo.startdate = dteStart.Date;
        mo.status = txtstatus.Text;

        if (new NeBusinessUnit(mo.business_unit_id).country == "USA")
        {
            mo.vacation_amount_1 = Convert.ToDouble(txtvac1amt.Text);
            mo.vacation_amount_2 = Convert.ToDouble(txtvac2amt.Text);
            mo.vacation_amount_3 = Convert.ToDouble(txtvac3amt.Text);
        }
        else
        {
            mo.vacation_amount_1 = Convert.ToDouble(txtvac1amt.Text.Replace(",", "").Replace("%", "").Trim()) / 100;
            mo.vacation_amount_2 = Convert.ToDouble(txtvac2amt.Text.Replace(",", "").Replace("%", "").Trim()) / 100;
            mo.vacation_amount_3 = Convert.ToDouble(txtvac3amt.Text.Replace(",", "").Replace("%", "").Trim()) / 100;
        }
        mo.vacation_interval_1 = Convert.ToInt32(txtvac1int.Text);
        mo.vacation_interval_2 = Convert.ToInt32(txtvac2int.Text);
        mo.vacation_interval_3 = Convert.ToInt32(txtvac3int.Text);




        if ((mo.paytype_id <= 3) && !mo.part_time)
        {
            mo.benefits_startdate = dte_benefits_startdate.Date;
            mo.wage = double.Parse(txtwage.Text.Trim().Replace("$", ""));
        }
        else
        {
            if (mo.paytype_id != 7)
            {
                mo.benefits_startdate = mo.startdate.AddYears(50);
            }
            if (mo.paytype_id == 4 || mo.paytype_id == 6)
            {
                mo.wage = double.Parse(txtwage_sub.Text.Trim().Replace("$", ""));
            }
            else if (mo.paytype_id == 1)
            {
                mo.wage = double.Parse(txtwage.Text.Trim().Replace("$", ""));
            }
            else if (mo.paytype_id == 5)
            {
                mo.wage = double.Parse(txtwage_sub.Text.Trim().Replace("$", ""));
            }
        }
        mo.vendor_id = Convert.ToInt32(ddlsubcontractor.Value);
        mo.contract_details = mem_contract_details.Text;

        mo.bonus_amount = Convert.ToDouble(spn_bonus_amt.Value);
        mo.bonus_type = Convert.ToString(ddlbonus.Value) == "" ? 0 : Convert.ToInt32(ddlbonus.Value);
        mo.bonus_margin_threshold = Convert.ToString(txt_bonus_margin_threshold.Text) == "" ? 0 : Convert.ToDouble(txt_bonus_margin_threshold.Text.Replace("$", "").Replace(",", ""));
        mo.bonus_netincome_threshold = Convert.ToString(txt_bonus_netincome_threshold.Text) == "" ? 0 : Convert.ToDouble(txt_bonus_netincome_threshold.Text.Replace("$", "").Replace(",", ""));
        mo.bonus_revenue_threshold = Convert.ToString(txt_bonus_revenue_threshold.Text) == "" ? 0 : Convert.ToDouble(txt_bonus_revenue_threshold.Text.Replace("$", "").Replace(",", ""));
        mo.bonus_netincome_highwater = Convert.ToString(txt_bonus_hw.Text) == "" ? 0 : Convert.ToDouble(txt_bonus_hw.Text.Replace("$", "").Replace(",", ""));
        mo.contract_details = mem_contract_details.Text;
        mo.vendor_id = Convert.ToInt32(ddlsubcontractor.Value);
        try
        {
            var wage = new NeWage(Convert.ToInt32(mo.memberid));
            if (wage.current_wage != 0 && mo.wage != 0)
            {
                if ((mo.wage - wage.current_wage) / mo.wage > 0.1)
                {
                    var email = new NeEMail();
                    email.To = new NeMember(Convert.ToInt32(new NeMember(Convert.ToInt32(mo.reports_to)).reports_to)).NEEmail;
                    email.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
                    email.Subject = "Large wage increase detected on offer for " + user.FullName + " in " + ddlcompany.Text;
                    email.Body = "Wage was: " + wage.current_wage.ToString("C2") + " to " + mo.wage.ToString("C2");
                    //				email.Send();
                }
            }
        }
        catch { }



        if (isapplicant == 0)
        {
            mo.memberid = memberid;
            mo.isapplicant = false;
            mo.applicantid = 0;
        }
        else
        {
            mo.applicantid = applicant.id;
            mo.isapplicant = true;
            mo.memberid = 0;
        }
        // reset all other open offers to Expired

        mo.save();

        btnSave.Text = "Save";
        id = mo.id;
        m = new NeMemberOffer(id);
        hid_moid.Value = id.ToString();
        Session["offerid"] = id;

        if (new_app_offer)
        {
            var dtmm = _tools.getSQL_datatable(@"Select * from new_milestone", null);
            foreach (DataRow dr in dtmm.Rows)
            {
                Memberoffer_Milestones mm;
                mm = new Memberoffer_Milestones();
                mm.milestone = dr["new_milestone"].ToString();
                mm.due = mo.startdate.AddDays(Convert.ToInt32(dr["days_from_startdate"]));
                mm.addedby = mo.enteredby;
                mm.offerid = mo.id;
                mm.save();
            }

        }
        if (new_offer_trigger)
        {
            hdnmtype.Value = m.membertypeid.ToString();
            prepopulate_crs();
            ASPxCallbackPanel1.JSProperties["cpServerMessage"] = id.ToString();
        }

        //	Session["offerid"] = id;
        //	hid_moid.Value = id.ToString();
        //	hdnmtype.Value = mo.membertypeid.ToString();
        //	SqlDataSource3.DataBind();
        //	ASPxPageControl1.TabPages[1].ClientEnabled = true;
        //	ASPxPageControl1.TabPages[2].ClientEnabled = true;
        //	ASPxPageControl1.TabPages[3].ClientEnabled = true;
        //	ASPxPageControl1.TabPages[4].ClientEnabled = true;
        Session["HR_applicant_grid"] = null;


    }
    protected void validate()
    {


        if (ddlcompany.Value == null || ddlcompany.Value.ToString() == "0")
        {
            throw new Exception("You must select a valid branch.");
        }
        if (dteEnd.Text == "")
        {
            throw new Exception("You must select a valid End Date.");
        }
        if (ddlmembertype.Value == null || ddlmembertype.Value.ToString() == "")
        {
            throw new Exception("You must select a valid membertype.");
        }
        if (ddlreportsto.Value == null || ddlreportsto.Value.ToString() == "")
        {
            throw new Exception("You must select who the person reports to.");
        }
        if (dteStart.Text == "")
        {
            throw new Exception("You must select a valid Start Date.");
        }
        if (dteStart.Date > dteEnd.Date)
        {
            throw new Exception("The end date must be after the start date.");
        }

        if (ASPxPageControl1.TabPages[1].ClientEnabled == true)
        {
            if (txtvac1amt.Text == null || txtvac1amt.Text == "" || txtvac2amt.Text == null || txtvac2amt.Text == "" || (txtvac3amt.Text == null || txtvac3amt.Text == ""))
            {
                throw new Exception("You are missing a vacation amount parameter.");
            }
            if (txtvac1int.Text == null || txtvac1int.Text == "" || txtvac2int.Text == null || txtvac2int.Text == "" || (txtvac3int.Text == null || txtvac3int.Text == ""))
            {
                throw new Exception("You are missing a vacation interval parameter.");
            }

            if (new NeBusinessUnit(ddlcompany.Value).country.Equals("CDN"))
            {
                try
                {
                    if (Convert.ToDouble(txtvac1amt.Text.Replace(",", "").Replace("%", "").Trim()) > 10)
                    {
                        throw new Exception("Vacation Level 1 percentage holidays is showing more than 10%!!");
                    }

                    if (Convert.ToDouble(txtvac2amt.Text.Replace(",", "").Replace("%", "").Trim()) > 10)
                    {
                        throw new Exception("Vacation Level 2 percentage holidays is showing more than 10%!!");
                    }

                    if (Convert.ToDouble(txtvac3amt.Text.Replace(",", "").Replace("%", "").Trim()) > 10)
                    {
                        throw new Exception("Vacation Level 3 percentage holidays is showing more than 10%!!");
                    }
                }
                catch { throw new Exception("Please review all the vacation settings.. because something is not right there."); }
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(txtvac1amt.Text) > 240)
                    {
                        throw new Exception("Vacation Level 1 Hours off is showing MORE THAN 240 Hours!!");
                    }

                    if (Convert.ToInt32(txtvac2amt.Text) > 240)
                    {
                        throw new Exception("Vacation Level 2 Hours off is showing MORE THAN 240 Hours!!");
                    }

                    if (Convert.ToInt32(txtvac3amt.Text) > 240)
                    {
                        throw new Exception("Vacation Level 3 Hours off is showing MORE THAN 240 Hours!!");
                    }
                }
                catch { throw new Exception("Please review all the vacation settings.. because something is not right there."); }

            }
            if (Convert.ToInt32(ddlpaytype.Value) <= 3)
            {
                if (txtwage.Text == null || txtwage.Text == "")
                {
                    throw new Exception("You are missing a wage.");
                }
                if (txtwage.Text == "$0.00")
                {
                    throw new Exception("You are missing a wage.");
                }
            }
            else
            {
                if (txtwage_sub.Text == null || txtwage_sub.Text == "")
                {
                    throw new Exception("You are missing a wage.");
                }
                if (txtwage_sub.Text == "$0.00")
                {
                    throw new Exception("You are missing a wage.");
                }
            }
        }

        if (m != null && m.id != 0 && m.isapplicant == false)
        {
            if (ddlreportsto.Text == "" || NeMember.Check_for_circular_org_chart(m.memberid, Convert.ToInt32(ddlreportsto.Value)) == true)
            {
                throw new Exception("Reporting Lines are Circular.  Somewhere between this person and the CEO, the reporting lines become circular.");
            }
        }

    }
    protected void prepopulate_crs()
    {
        var dt = _tools.getSQL_datatable(@"SELECT * FROM core_responsibilities INNER JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id  WHERE core_responsibilities.`status` = 'Active' AND membertype_responsibilities.membertype_id =@v0", new object[] { hdnmtype.Value });
        foreach (DataRow dr in dt.Rows)
        {
            if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { dr["id"], hid_moid.Value }) == 0)
            {
                _tools.getSQL_void(@"insert into memberoffer_cr 
(memberoffer_crid, memberoffer_moid, cr_wording,daily,weekly,monthly,quarterly,annually,as_required) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)", new object[] {
                    dr["id"],
                    hid_moid.Value,
                dr["core_responsibility"],
                dr["daily"],
                dr["weekly"],
                dr["monthly"],
                dr["quarterly"],
                dr["annually"],
                dr["as_required"] });
            }
        }
    }
    protected void gv_mocr_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        var p = e.Parameters.Split('|');

        if (p[0] == "dd")  // Select all.
        {
            if (p[1] == "true")
            {
                var dt = _tools.getSQL_datatable(@"SELECT * FROM core_responsibilities INNER JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id  WHERE core_responsibilities.`status` = 'Active' AND membertype_responsibilities.membertype_id =@v0", new object[] { hdnmtype.Value });
                foreach (DataRow dr in dt.Rows)
                {
                    if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { dr["id"], hid_moid.Value }) == 0)
                    {
                        _tools.getSQL_void(@"insert into memberoffer_cr 
(memberoffer_crid, memberoffer_moid, cr_wording,daily,weekly,monthly,quarterly,annually,as_required)
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)", new object[] {
                                dr["id"],
                                hid_moid.Value,
                                dr["core_responsibility"],
                                dr["daily"],
                                dr["weekly"],
                                dr["monthly"],
                                dr["quarterly"],
                                dr["annually"],
                                dr["as_required"] });
                    }
                }
            }
            else
            {
                _tools.getSQL_void(@"delete from memberoffer_cr where memberoffer_moid =@v0 ", new object[] {
                    hid_moid.Value});
            }
            gv_mocr.DataBind();
            var gvc = (GridViewColumn)gv_mocr.Columns["Applies"];
            var c = (ASPxCheckBox)gv_mocr.FindHeaderTemplateControl(gvc, "ASPxCheckBox2");
            c.Checked = Convert.ToBoolean(p[1]);
        }
        else if (p[0] == "xx")  // insert from a previous offer
        {
            if (p[1] == "true")
            {
                if (_tools.getSQL_int(@"Select count(memberid) from member_offers  where memberid =@v0 and member_offers.status = 'Accepted'", new object[] { m.memberid }) > 0)
                {
                    var old_moid = _tools.getSQL_int(@"select id from member_offers  where memberid =@v0 and member_offers.status = 'Accepted' order by member_offers.enddate desc limit 1", new object[] { m.memberid });
                    var old_mo = new NeMemberOffer(old_moid);
                    var dt = _tools.getSQL_datatable(@"SELECT core_responsibilities.id FROM core_responsibilities INNER JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id  WHERE core_responsibilities.`status` = 'Active' AND membertype_responsibilities.membertype_id =@v0", new object[] { hdnmtype.Value });
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { dr[0], hid_moid.Value }) == 0)
                        {

                            if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { dr[0], old_moid }) == 0)
                            {
                                _tools.getSQL_void(@"insert into memberoffer_cr (memberoffer_crid, memberoffer_moid)
values (@v0,@v1)", new object[] {
                                    dr[0],hid_moid.Value });
                            }
                        }
                    }
                }

            }
            else
            {
                _tools.getSQL_void(@"delete from memberoffer_cr where memberoffer_moid =@v0", new object[] { hid_moid.Value });
            }
            gv_mocr.DataBind();
            var gvc = (GridViewColumn)gv_mocr.Columns["Applies"];
            var c = (ASPxCheckBox)gv_mocr.FindHeaderTemplateControl(gvc, "chkprev_offer");
            c.Checked = Convert.ToBoolean(p[1]);
        }
        else
        {
            if (p[2] == "u")
            {
                if (Convert.ToBoolean(p[1]) == false)
                {
                    _tools.getSQL_void("delete from memberoffer_cr where memberoffer_crid =@v0  and memberoffer_moid =@v1 limit 1", new object[] { p[0], p[3] });
                }
                else
                {
                    if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { p[0], p[3] }) == 0)
                    {
                        var dt = _tools.getSQL_datatable(@"SELECT * FROM core_responsibilities INNER JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id  WHERE core_responsibilities.id =@v0 limit 1 ", new object[] { p[0] });
                        if (dt.Rows.Count > 0)
                        {
                            _tools.getSQL_void(@"insert into memberoffer_cr 
(memberoffer_crid, memberoffer_moid, cr_wording,daily,weekly,monthly,quarterly,annually,as_required) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)",
                                new object[] {
                                p[0],
                                p[3],
                                dt.Rows[0]["core_responsibility"],
                                dt.Rows[0]["daily"],
                                dt.Rows[0]["weekly"],
                                dt.Rows[0]["monthly"],
                                dt.Rows[0]["quarterly"],
                                dt.Rows[0]["annually"],
                                dt.Rows[0]["as_required"]});
                        }
                        //_tools.getSQL_void(@"insert into memberoffer_cr (memberoffer_crid, memberoffer_moid)  values (@v0,@v1)",new object[] { p[0].ToString(),p[3].ToString() } );
                    }

                }
            }
        }
    }
    protected void ASPxCheckBox1_Init(object sender, EventArgs e)
    {

        var chk = sender as ASPxCheckBox;
        var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
        chk.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ gv_mocr.PerformCallback('{0}|' + s.GetValue()+ '|u|{1}'); }}", container.KeyValue, id);
        if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { container.KeyValue, id }) >= 1)
        {
            chk.Checked = true;
        }

        if (m != null && id != 0)
        {
            if (m.status == "In Development")
            {
                chk.ClientEnabled = true;
            }
            else
            {
                chk.ClientEnabled = false;
            }
        }
        else
        {
            chk.ClientEnabled = false;
        }
    }
    protected void gv_mocr_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.VisibleIndex >= 0)
        {
            if (e.DataColumn.Caption == "use")
            {
                if (_tools.getSQL_int(@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ", new object[] { gv_mocr.GetRowValuesByKeyValue(e.KeyValue, "id"), id }) >= 1)
                {
                    var chk = (ASPxCheckBox)gv_mocr.FindDetailRowTemplateControl(e.VisibleIndex, "ASPxCheckBox1");
                    chk.Checked = true;
                }
            }
        }
    }
    protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {

        // check that the membertype ahs a chargeout

        NeMemberType mt = new NeMemberType(ddlmembertype.Value);
        double co = mt.GetChargeoutforbranch(ddlcompany.Value);
        if (co <= 0)
        {
            ASPxCallbackPanel1.JSProperties["cp_chargeout"] = "1";
            return;
        }







        if (e.Parameter[0].ToString() == "x")
        {
            //			setup_buttons();
            return;

        }
        else if (e.Parameter[0].ToString() == "y")
        {
            if (id != 0)
            {
                var xx = System.DateTime.Today.ToString("yyyy-MM-dd") + " - " + current_user.FullName + System.Environment.NewLine + mem_addnote.Text + System.Environment.NewLine + memdevnotes.Text;
                _tools.getSQL_void(@"update member_offers 
set redo_notes = @v0 
where id = @v1 ", new object[] {
                    xx, id});
                memdevnotes.Text = _tools.getSQL_string(@"select member_offers.redo_notes from member_offers  where id =@v0", new object[] { id });
            }
            mem_addnote.Text = "";
        }
        else if (e.Parameter[0].ToString() == "d")  // save button
        {
            if (id != 0 && m.status != "In Development")
            {
                try
                {
                    var mo = new NeMemberOffer(Convert.ToInt32(id));
                    mo.status = "In Development";
                    txtstatus.Text = "In Development";
                    mo.save();

                    redo_pop.ShowOnPageLoad = false;
                    var me = (ASPxMemo)redo_pop.FindControl("memredo");
                    btnredo.ClientVisible = false;
                    btnprint.ClientEnabled = false;

                    var xx = System.DateTime.Today.ToString("yyyy-MM-dd") + " - " + current_user.FullName + System.Environment.NewLine + memdevnotes.Text + System.Environment.NewLine + me.Text;
                    _tools.getSQL_void(@"update member_offers set redo_notes =@v0  where id = @v1", new object[] {
                            xx, id});
                    memdevnotes.Text = _tools.getSQL_string(@"select member_offers.redo_notes from member_offers  where id =@v0", new object[] { id });
                    var redo_email = new NeEMail();
                    redo_email.To = new NeMember(mo.enteredby).NEEmail;
                    //	redo_email.To = "aketelaars@newelectric.com";
                    //			redo_email.CC = new NeMember(mo.reports_to).NEEmail;
                    redo_email.From = current_user.NEEmail;
                    redo_email.Subject = "Suggested Edits for Employment Agreement: " + mo.id;
                    redo_email.isHTML = true;
                    var hl = "<a href ='" + Toolbox.app_setting("Domain")+"/sections/hr/member/member_offer.aspx?page_id=138&id=" + mo.id + "&memberid=" + mo.memberid + "&applicantid=" + mo.applicantid + "&isapplicant=" + (mo.isapplicant ? 1 : 0) + "' target='blank' >Click Here</a>";


                    var body = @"<style type='text/css'>
table.stat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }
table.sstat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }

</style><table class='stat'><tr><td colspan='2'><b>Employment Offer Change Request:</b></td></tr>";
                    body += "<tr><td><b>Offer ID:</b></td><td>" + mo.id + "</td></tr>";
                    body += "<tr><td><b>Author:</b></td><td>" + new NeMember(mo.enteredby).FullName2 + "</td></tr>";
                    if (mo.isapplicant == true)
                    {
                        body += "<tr><td><b>Applicant:</b></td><td>" + new NeApplicant(mo.applicantid).firstname + " " + new NeApplicant(mo.applicantid).lastname + "</td></tr>";

                    }
                    else
                    {
                        body += "<tr><td><b>Applicant:</b></td><td>" + new NeMember(mo.memberid).FullName2 + "</td></tr>";
                    }
                    body += "<tr><td><b>Requested Changes:</b></td><td> " + me.Text + "</td></tr>";
                    body += "<tr><td></td><td></td></tr>";
                    body += "<tr><td></td><td>Please review the changes by clicking the link above or somehow find your way to the member offer page, make the changes and re-submit it for approval</td></tr>";
                    body += "</td></tr></table><table class='sstat'><tr>";


                    redo_email.Body += body;
                    redo_email.Send();
                    me.Text = "";

                }
                catch { }
            }

        }

        else if (e.Parameter[0].ToString() == "s")  // save button
        {
            //	validate();
            save();
            //	populate_newform();
            populate_newform();
            //			throw new Exception("This offer has been saved.");
            ASPxCallbackPanel1.JSProperties["cpalert"] = "This offer has been saved";
            //			Context.ApplicationInstance.CompleteRequest();

        }
        else if (e.Parameter[0].ToString() == "a")  // send for approval
        {
            if (id != 0)
            {
                //		validate();
                save();

                try
                {
                    var mo = new NeMemberOffer(Convert.ToInt32(id));
                    var hl = "<a href ='" + Toolbox.app_setting("Domain")+"/sections/hr/member/member_offer.aspx?page_id=138&id=" + mo.id + "&memberid=" + mo.memberid + "&applicantid=" + mo.applicantid + "&isapplicant=" + (mo.isapplicant ? 1 : 0) + "' target='blank' >Click Here</a>";
                    var em = new NeEMail();
                    em.isHTML = true;
                    em.Body = "<div font-face='Arial'>";
                    var final_review = 0;
                    if (mo.isapplicant)
                    {
                        em.Subject = "Applicant Offer Awaiting Your Approval";
                        em.Body += "Please review the offer for " + new NeApplicant(mo.applicantid).firstname + " " + new NeApplicant(mo.applicantid).lastname + " in " + new NeBusinessUnit(mo.business_unit_id).name + ".";
                    }
                    else
                    {
                        em.Subject = "Employment agreement Awaiting Your Approval";
                        em.Body += "Please review the agreement for " + new NeMember(mo.memberid).FullName2 + " in " + new NeBusinessUnit(mo.business_unit_id).name + ".";
                    }
                    try
                    {
                        if (mo.reports_to == mo.enteredby)
                        {
                            final_review = Convert.ToInt32(new NeMember(Convert.ToInt32(mo.reports_to)).reports_to);
                        }
                        else
                        {
                            if (NeMember.is_supervisor(mo.reports_to, mo.enteredby))
                            {
                                final_review = Convert.ToInt32(new NeMember(Convert.ToInt32(mo.enteredby)).reports_to);
                            }
                            else
                            {
                                final_review = Convert.ToInt32(new NeMember(Convert.ToInt32(mo.reports_to)).reports_to);
                            }
                        }

                        if (final_review == 0)
                        {
                            if (mo.business_unit_id == 3)
                            {
                                final_review = 18;
                            }
                            else if (System.DateTime.Today < new DateTime(2016, 1, 1))
                            {
                                final_review = 19;
                            }
                            else
                            {
                                final_review = _tools.getSQL_int(@"Select ifnull((Select member_id from member  where member_membertype_id = 35 limit 1),0)", null);
                            }
                        }
                        em.To = new NeMember(final_review == 0 ? mo.reports_to : final_review).NEEmail;
                    }
                    catch
                    {
                        em.To = "debug@" + Toolbox.app_setting("DomainForEmail");
                        em.Subject = "Offer Awaiting Your Approval - because something is broke";
                    }
                    em.From = current_user.NEEmail;
                    em.Body += System.Environment.NewLine + hl;
                    em.Send();

                    mo.status = "Waiting for Approval";
                    txtstatus.Text = "Waiting for Approval";
                    mo.save();

                    populate_newform();
                    ASPxCallbackPanel1.JSProperties["cpalert"] = "This offer has been sent to " + new NeMember(final_review == 0 ? mo.reports_to : final_review).FullName + " for approval";


                }
                catch { }
            }

        }
        else if (e.Parameter[0].ToString() == "p")  // approved button pressed
        {
            if (id != 0)
            {
                try
                {
                    var mo = new NeMemberOffer(id);
                    var hl = "<a href ='" + Toolbox.app_setting("Domain")+"/sections/hr/member/member_offer.aspx?page_id=138&id=" + id + "&memberid=" + mo.memberid + "&applicantid=" + mo.applicantid + "&isapplicant=" + (mo.isapplicant ? 1 : 0) + "' target='blank' >Please click review the agreement for this person</a>";


                    var em = new NeEMail();
                    em.isHTML = true;
                    em.To = new NeMember(mo.enteredby).NEEmail;
                    em.CC = "debug@" + Toolbox.app_setting("DomainForEmail");
                    em.Body = "<div font-face='Arial'>";
                    em.From = current_user.NEEmail;
                    if (mo.isapplicant)
                    {
                        em.Subject = "Offer Has Been Approved!";
                        em.Body += "The offer for " + new NeApplicant(mo.applicantid).firstname + " " + new NeApplicant(mo.applicantid).lastname + " in " + new NeBusinessUnit(mo.business_unit_id).name + " has been approved by " + current_user.FullName + "!";
                    }
                    else
                    {
                        em.Subject = "Agreement Has Been Approved!";

                        em.Body += "The agreement for " + new NeMember(mo.memberid).FullName2 + " in " + new NeBusinessUnit(mo.business_unit_id).name + " has been approved " + current_user.FullName + "!";
                    }
                    em.Body += "<br/>" + hl;
                    em.Body += "</div>";
                    em.Send();
                    mo.status = "Approved";
                    txtstatus.Text = "Approved";
                    mo.save();
                    populate_newform();
                    ASPxCallbackPanel1.JSProperties["cpalert"] = "Thank you for approving this offer.";

                }
                catch { }
            }

        }
        else if (e.Parameter[0].ToString() == "r")  // revoke button pressed
        {
            if (id != 0)
            {
                try
                {
                    var mo = new NeMemberOffer(Convert.ToInt32(id));
                    mo.status = "Revoked";
                    txtstatus.Text = "Revoked";
                    mo.save();
                    populate_newform();
                    ASPxCallbackPanel1.JSProperties["cpalert"] = "This offer has been revoked.";
                }
                catch { }
            }

        }
        else if (e.Parameter[0].ToString() == "v")   // revive button pressed
        {
            if (id != 0)
            {
                try
                {
                    var mo = new NeMemberOffer(Convert.ToInt32(id));
                    mo.status = "In Development";
                    txtstatus.Text = "In Development";
                    prepopulate_crs();
                    mo.save();
                    //					populate_newform();
                    ASPxCallbackPanel1.JSProperties["cpServerMessage"] = mo.id.ToString();
                    ASPxCallbackPanel1.JSProperties["cpalert"] = "This offer has been revived.";

                }
                catch { }
            }

        }

        populate_newform();

    }
    protected void btnaccepted_Click(object sender, EventArgs e)
    {
        //		return;
        save();
        var mo = new NeMemberOffer(id);
        if (mo.status != "Released")
        {

            ASPxCallbackPanel1.JSProperties["cpalert"] = "The offer must bear the status 'Released' in order for the offer to be accepted";
        }
        if (mo.isapplicant)
        {
            try
            {
                #region created the user here.. not on the index page

                // Does username exist in any other users?
                var fname = NeMember.CleanUserPass(applicant.firstname).ToLower();
                var lname = NeMember.CleanUserPass(applicant.lastname).ToLower();
                var proposedUserName = fname.Substring(0, 1) + lname;
                var proposedPassword = fname.Substring(0, 1) + lname.First().ToString().ToUpper() + lname.Substring(1) + mo.id;
                var userExists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_user = @v0 AND member_id != @v1", new object[] { proposedUserName, id }) > 0;
                var isSubContractor = mo.paytype_id > 3 && mo.paytype_id != 7;
                var n_info = new NeMember();

                n_info.Username = userExists ? proposedUserName + mo.id : proposedUserName;
                n_info.Password = proposedPassword;


                n_info.FirstName = applicant.firstname;
                n_info.LastName = applicant.lastname;
                n_info.Address = applicant.address;
                n_info.City = applicant.city;
                n_info.Prov = applicant.province;
                n_info.Country = applicant.country;
                n_info.PostalCode = applicant.postal;
                n_info.Email = applicant.email;
                n_info.empnotes = applicant.notes;
                n_info.hrstatus_id = 1;
                n_info.NEEmail = "nomail@thatsnew.com";
                n_info.benefits_startdate = isSubContractor ? mo.startdate.AddYears(50) : mo.benefits_startdate;
                n_info.timetostat = isSubContractor ? 9999 : n_info.Country == "CAN" ? 0 : 90;
                n_info.receive_stat_pay = isSubContractor ? 0 : 1;

                n_info.vacation_amount_1 = (decimal)mo.vacation_amount_1;
                n_info.vacation_amount_2 = (decimal)mo.vacation_amount_2;
                n_info.vacation_amount_3 = (decimal)mo.vacation_amount_3;
                n_info.vacation_interval_1 = mo.vacation_interval_1;
                n_info.vacation_interval_2 = mo.vacation_interval_2;
                n_info.vacation_interval_3 = mo.vacation_interval_3;
                n_info.reports_to = mo.reports_to;
                n_info.payroll_handler = mo.reports_to > 0 && new NeMember(mo.reports_to).AuthenticatedForPage(47)
                                            ? mo.reports_to
                                            : new NeBusinessUnit(mo.business_unit_id).branch_manager.id;
                n_info.PhoneAreaCode = "";
                n_info.PhoneFirst = "";
                n_info.PhoneLast = "";
                n_info.StartDate = Toolbox.MySQL_shortdt(mo.startdate);
                n_info.TerminateDate = "2099-12-31";
                n_info.business_unit_id = mo.business_unit_id;


                n_info.Status = "Active";
                n_info.MemberTypeID = mo.membertypeid;
                n_info.is_CAN_boardmember = false;
                n_info.is_US_boardmember = false;
                n_info.Nickname = applicant.firstname;
                n_info.part_time = mo.part_time;
                n_info.paytype_id = mo.paytype_id == 0 ? 1 : mo.paytype_id;


                if (applicant.cellphone.Length == 10)
                {
                    n_info.pecell_area = applicant.cellphone.Substring(0, 3);
                    n_info.pecell_pref = applicant.cellphone.Substring(3, 3);
                    n_info.pecell_suff = applicant.cellphone.Substring(6, 4);
                }




                if (n_info.id == 0)  // if it's a new user
                {

                    try
                    {
                        n_info.save();
                    }
                    catch (Exception ee)
                    {
                        _tools.catch_error(ee);

                        ASPxCallbackPanel1.JSProperties["cpalert"] = "Something went wrong converting this applicant into an employee";
                        Context.ApplicationInstance.CompleteRequest();
                        Toolbox.doSQL_void(@"INSERT INTO matt (jumble, dt) VALUES (@v0, NOW())", ee.StackTrace);
                        return;
                    }


                    #region Kick off PreLive function
                    mo.memberid = n_info.id;
                    mo.save();
                    NESI.BLL.Pages.Employees.NeMemberOffer.go_prelive(mo.id, current_user, "hired");

                    #endregion
                    ASPxCallbackPanel1.JSProperties["cpalert"] = "New USER has been created, HR will now send them their NEW HIRE fvr package.";

                    Context.ApplicationInstance.CompleteRequest();

                }


                #endregion


            }
            catch { ASPxCallbackPanel1.JSProperties["cpalert"] = "Could not create new member"; }
        }
        else
        {
            var mo_m = new NeMember(Convert.ToInt32(mo.memberid));
            if (mo_m.hrstatus_id == 5 || mo_m.hrstatus_id == 4)
            {
                NESI.BLL.Pages.Employees.NeMemberOffer.go_prelive(mo.id, current_user, "reinstated");
            }

            var em = new NeEMail();
            em.isHTML = true;
            em.Bcc = "payroll@" + Toolbox.app_setting("DomainForEmail");


            if (mo.startdate > System.DateTime.Today)
            {
                mo.status = "Awaiting Start Date";
                em.Subject = "Employment agreement for " + mo_m.FullName + " has been accepted. But will not go into effect until " + mo.startdate.ToString("yyyy-MM-dd");
                em.isHTML = true;
                var str_body = @"<table style='width: 100%; font-family: Arial; font-size: small;'>
				<tr>
					<td bgcolor='Lime' colspan='2'>
						Employment Agreement Accepted</td>
				</tr>
				<tr>
					<td colspan='2'>
						The system will update their details automatically on " + mo.startdate.ToString("yyyy-MM-dd") + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Employee:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='"+Toolbox.app_setting("Domain")+"/sections/hr/member/index.aspx?id=" + mo_m.id + "'>" + mo_m.FullName2 + @"  (" + mo_m.id + @")</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Branch:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + new NeBusinessUnit(mo.business_unit_id).name + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='" + Toolbox.app_setting("Domain") + "/sections/hr/member/member_offer.aspx?id=" + mo.id + "'>" + mo.id + @"</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement Start Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.startdate.ToLongDateString() + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement End Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.enddate.ToLongDateString() + @"</td>
				</tr>
			</table>";


                em.Body = str_body;
                try
                {
                    em.To = new NeBusinessUnit(mo.business_unit_id).branch_manager.NEEmail;
                }
                catch (Exception ee)
                {
                    Toolbox.do_errorLog(ee);
                    em.To = "debug@" + Toolbox.app_setting("DomainForEmail");
                    em.Body = "This Email has been sent to Debug because The system couldnt find a branch manager.<br>" + str_body;
                }
                em.From = current_user.NEEmail;
                em.Send();

            }
            else  // if the offer is already to be put in effect starting now
            {
                mo.status = "Accepted";
                NESI.BLL.Pages.Employees.NeMemberOffer.go_live(mo.id);
            }

            mo.save();

        }


        populate_newform();
        ASPxCallbackPanel1.JSProperties["cpalert"] = "This offer has been accepted and "+ Toolbox.app_setting("Domain") + " files have been updated.";
        upload_offer_file(mo.id);
    }


    public void upload_offer_file(int moid)
    {
        var mo = new NeMemberOffer(moid);
        #region save a version into the filestore
        var stream = new MemoryStream();
        if (mo.vendor_id != 0)
        {
            var e_info = new subcontract_employment();
            var eo = (subcontract_employment)NeMemberOffer.fill_report_sub(e_info, mo);
            e_info.CreateDocument();
            e_info.PrintingSystem.ExportToPdf(stream);
        }
        else
        {
            var e_info = new emp_offer();
            var eo = (emp_offer)NeMemberOffer.fill_report(e_info, mo.id, 0);
            e_info.CreateDocument();
            e_info.PrintingSystem.ExportToPdf(stream);
        }

        stream.Seek(0, SeekOrigin.Begin);
        var fileLength = (int)stream.Length;
        var rawdata = new byte[fileLength];
        stream.Read(rawdata, 0, (int)fileLength);
        var f = new file_store.fileObj();
        f.page_id = Convert.ToInt32(127);
        f.folder_id = 4;
        f.sub_folder_id = mo.id;
        if (Toolbox.doSQL_int(@"Select count(id) from filestore.files where (folder_id=4 and sub_folder_id=@v0)", mo.id) > 0)
        {
            Toolbox.doSQL_void(@"delete from filestore.files where (folder_id=4 and sub_folder_id=@v0)", mo.id);
        }
        f.name = "Employment Agreement " + mo.id;
        f.ext = "pdf";
        f.mime = "application/pdf";

        f.content = rawdata;
        f.save();
        #endregion

    }

    protected void setup_buttons()
    {
        if (m != null)
        {
            switch (m.status)
            {
                #region In Development
                case "In Development":
                    btnaccepted.ClientVisible = false;
                    if (lblid.Text == "0")
                    {
                        btnexpire.ClientVisible = false;
                    }
                    else
                    {
                        btnSave.ClientEnabled = true;
                        btnprint.ClientEnabled = false;
                    }
                    if (memcompdetails.Text == "" ||
                            current_user.AuthenticatedForPrivilege(130) ||
                            NeMember.is_supervisor(m.enteredby, current_user.id) && NeMember.is_supervisor(m.reports_to, current_user.id))  // if the offer can go straight through...
                    {
                        btnSave.Text = "Save";

                        btnaction.ClientVisible = false;

                        btnSave.ClientEnabled = true;
                        btnprint.Text = "Print and Release";
                        btnprint.ClientEnabled = true;
                    }
                    else
                    {
                        btnaction.Text = "Send for Approval";
                        btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('a');}";
                        btnprint.Text = "Print and Release";
                        btnprint.ClientEnabled = false;
                        btnaction.ClientVisible = true;
                    }
                    break;
                #endregion
                #region Approved
                case "Approved":
                    btnaccepted.ClientVisible = false;
                    btnSave.Text = "Save";
                    btnaction.Text = "Revoke";
                    btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('r');}";
                    btnaction.ClientVisible = true;
                    btnexpire.ClientVisible = true;
                    btnSave.ClientEnabled = false;
                    btnprint.Text = "Print and Release";
                    btnprint.ClientEnabled = true;
                    pnl_compplan.Enabled = false;
                    ddlpaytype.ClientEnabled = false;

                    break;
                #endregion
                #region Waiting for Approval
                case "Waiting for Approval":

                    btnSave.Text = "Save";
                    btnprint.Text = "Print";
                    btnaccepted.ClientVisible = false;
                    btnexpire.ClientVisible = true;
                    // if its a nesi member or a supervisor of the branch manager of the branch allow th approval

                    if (current_user.AuthenticatedForPrivilege(130) ||
                            NeMember.is_supervisor(m.enteredby, current_user.id) && NeMember.is_supervisor(m.reports_to, current_user.id))
                    {
                        spn_bonus_amt.ClientEnabled = true;
                        ddlbonus.ClientEnabled = true;
                        btnaction.Text = "Approve";
                        btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('p');}";
                        btnaction.ClientVisible = true;
                        btnredo.ClientVisible = true;
                        btnprint.ClientEnabled = true;

                    }
                    else
                    {
                        btnSave.ClientEnabled = false;
                        btnaction.ClientVisible = false;
                        pnl_compplan.Enabled = false;
                    }
                    break;
                #endregion

                #region Released
                case "Released":
                    btnSave.Text = "Save";
                    if (isapplicant == 0)
                    {
                        btnaccepted.Text = "Accepted";
                    }
                    btnaccepted.ClientVisible = true;
                    btnexpire.ClientVisible = true;
                    btnaction.Text = "Revoke";
                    btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('r');}";
                    btnprint.Text = "Print";
                    btnprint.ClientEnabled = true;
                    btnSave.ClientEnabled = false;
                    pnl_compplan.Enabled = false;
                    ddlpaytype.ClientEnabled = false;
                    ASPxPageControl1.TabPages[5].ClientEnabled = true;
                    iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant + "&offerid=" + m.id;

                    break;
                #endregion
                case "Accepted":
                case "Awaiting Start Date":
                    btnaccepted.ClientVisible = false;
                    btnexpire.ClientVisible = true;
                    //btnaction.Text = "Revive";
                    btnaction.ClientVisible = false;
                    btnprint.ClientEnabled = true;
                    btnSave.Text = "Save";
                    btnSave.ClientEnabled = false;
                    ddlpaytype.ClientEnabled = false;
                    ASPxPageControl1.TabPages[5].ClientEnabled = true;
                    pnl_compplan.Enabled = false;
                    iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant + "&offerid=" + m.id;

                    break;
                case "Previous":
                    btnaccepted.ClientVisible = false;
                    btnexpire.ClientVisible = false;
                    //btnaction.Text = "Revive";
                    btnaction.ClientVisible = false;
                    btnprint.ClientEnabled = true;
                    btnSave.Text = "Save";
                    btnSave.ClientEnabled = false;
                    ddlpaytype.ClientEnabled = false;
                    ASPxPageControl1.TabPages[5].ClientEnabled = true;
                    pnl_compplan.Enabled = false;
                    iframe_upload.Attributes["src"] = "./upload_signback.aspx?pageid=" + page_id + "&memberid=" + memberid + "&applicantid=" + applicantid + "&isapplicant=" + isapplicant + "&offerid=" + m.id;

                    break;
                case "Revoked":
                    btnaccepted.ClientVisible = false;
                    btnSave.Text = "Save";
                    btnSave.ClientEnabled = false;
                    btnaction.Text = "Revive";
                    btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('v');}";
                    btnaction.ClientVisible = true;
                    btnprint.ClientEnabled = false;
                    btnaccepted.ClientVisible = false;
                    break;


                case "Closed":
                    btnSave.Text = "Save";
                    btnSave.ClientEnabled = false;
                    btnaccepted.ClientVisible = false;
                    btnexpire.ClientVisible = false;
                    btnaction.Text = "Revive";
                    btnaction.ClientSideEvents.Click = "function (s, e) {cb.PerformCallback('v');}";
                    btnaction.ClientVisible = true;
                    btnprint.ClientEnabled = false;
                    pnl_compplan.Enabled = false;
                    ddlpaytype.ClientEnabled = false;

                    break;
            }

        }

    }

    protected void btnexpire_Click1(object sender, EventArgs e)
    {
        _tools.getSQL_void(@"Delete from memberoffer_cr where memberoffer_cr.memberoffer_moid =@v0 ", new object[] {
            id});
        var mo = new NeMemberOffer(id);
        mo.status = "Closed";
        mo.save();
        populate_newform();


    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (id != 0)
        {
            NeMemberOffer mo;
            mo = new NeMemberOffer(Convert.ToInt32(id));
            txtstatus.Text = mo.status;
            if (mo.status == "In Development")
            {
                /*			if((current_user.AuthenticatedForPrivilege(130)) ||
                                        NeMember.is_supervisor(m.enteredby, current_user.id))
                            {
                                mo.status = "Released";
                                validate();
                                save();
                            }
                 */

                NeMemberType mt = new NeMemberType(ddlmembertype.Value);
                double co = mt.GetChargeoutforbranch(ddlcompany.Value);
                if (co <= 0)
                {
                    pop_chargeout.ShowOnPageLoad = true;
                    return;
                }

                validate();
                save();

                if (mo.comp_details == "" || current_user.AuthenticatedForPrivilege(130) || NeMember.is_supervisor(m.enteredby, current_user.id))
                {

                    if (mo.isapplicant)
                    {
                        _tools.getSQL_void(@"UPDATE member_offers SET status ='Closed' WHERE status !='Accepted' AND status !='Previous' 
AND applicantid = @v0  AND isapplicant=1 AND id != @v1", new object[] {
                            mo.applicantid, mo.id});
                        _tools.getSQL_void(@"UPDATE applicants SET status ='Offered' WHERE status ='New' AND id = @v0", new object[] {
                            mo.applicantid});
                    }
                    else
                    {
                        _tools.getSQL_void(@"UPDATE member_offers SET status ='Closed' WHERE status !='Accepted' AND status !='Previous'
AND memberid = @v0 AND isapplicant=0 AND id != @v1", new object[] {
                            mo.memberid,  mo.id});
                    }
                    mo.status = "Released";
                    mo.save();

                }

            }

            if (mo.status == "Approved" || mo.status == "Waiting for Approval" || mo.status == "Released" || mo.status == "Accepted")
            {

                if (mo.status == "Approved" || mo.status == "Released")
                {
                    populate_newform();
                    if (mo.isapplicant)
                    {
                        _tools.getSQL_void(@"UPDATE member_offers SET status ='Closed' 
WHERE status !='Accepted' AND status !='Previous' AND applicantid = @v0  AND isapplicant=1 AND id != @v1", new object[] {
                            mo.applicantid,  mo.id});
                    }
                    else
                    {
                        _tools.getSQL_void(@"UPDATE member_offers SET status ='Closed'
WHERE status !='Accepted' AND status !='Previous' AND memberid = @v0 AND isapplicant=0  AND id != @v1", new object[] {
                            mo.memberid,  mo.id});
                    }
                    mo.status = "Released";
                    mo.save();
                }


            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('offer_print_off.aspx?moid=" + id + "','mo',700,900)", true);
            txtstatus.Text = mo.status;
        }
        populate_newform();
    }
    protected void chkprev_offer_Init(object sender, EventArgs e)
    {
        var c = (ASPxCheckBox)sender;
        c.ClientVisible = false;
        if (hid_moid.Value == "")
        {


        }
        else
        {
            var mo = new NeMemberOffer(Convert.ToInt32(hid_moid.Value));
            if (!mo.isapplicant)
            {
                if (_tools.getSQL_int(@"Select count(memberid) from member_offers  where memberid =@v0 and member_offers.status = 'Accepted'", new object[] { mo.memberid }) > 0)
                {
                    c.ClientVisible = true;
                }
                else
                {
                    c.ClientVisible = false;
                }

                if (mo.status == "In Development")
                {
                    c.ClientEnabled = true;
                }
                else
                {
                    c.ClientEnabled = false;
                }
            }
        }
    }

    protected void ASPxCheckBox2_Init(object sender, EventArgs e)
    {
        var c = (ASPxCheckBox)sender;
        c.ClientEnabled = id != 0 && m.status == "In Development";
    }
    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        var m = new Memberoffer_Milestones();
        m.update_milstones_from_old_offers();

    }
    protected void cb_milestones_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        if (e.Parameter.Length > 1)
        {
            if (e.Parameter[0].ToString() == "a")
            {
                var q = e.Parameter.Split('|');
                var mm = new Memberoffer_Milestones();
                mm.offerid = id;
                mm.addedby = current_user.id;
                mm.added = System.DateTime.Today;
                try
                {
                    mm.due = Convert.ToDateTime(q[2]);
                }
                catch
                {
                    mm.due = dteEnd.Date;
                }
                mm.milestone = q[1];
                mm.save();
            }
            else if (e.Parameter[0].ToString() == "d")
            {
                var q = e.Parameter.Split('|');
                var mm = new Memberoffer_Milestones(Convert.ToInt32(q[1]));
                mm.due = Convert.ToDateTime(q[2]);
                mm.save();
            }
            if (e.Parameter[0].ToString() == "m")
            {
                var q = e.Parameter.Split('|');
                var mm = new Memberoffer_Milestones(Convert.ToInt32(q[1]));
                mm.milestone = q[2];
                mm.save();
            }
        }
        fill_milestones();
    }
    protected void gv1_m_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        _tools.getSQL_void(@"Delete from memberoffer_milestones where id =@v0 limit 1 ", new object[] {
            e.Keys[0] });
        fill_milestones();
        e.Cancel = true;
        gv1_m.CancelEdit();
    }
    protected void fill_milestones()
    {
        gv1_m.DataSource = _tools.getSQL_datatable(@"Select * from memberoffer_milestones  where memberoffer_milestones.offerid =@v0", new object[] { id });
        gv1_m.DataBind();

    }
    protected void ASPxDateEdit1_Init(object sender, EventArgs e)
    {
        var dtemilestone = sender as ASPxDateEdit;
        var container = dtemilestone.NamingContainer as GridViewDataItemTemplateContainer;
        dtemilestone.ClientSideEvents.DateChanged = string.Format("function (s, e) {{ cb_milestones.PerformCallback('d|{0}|' + s.GetText()); }}", container.KeyValue);
        if (m != null)
        {
            if (m.id != 0)
            {
                if (m.status != "In Development" && m.status != "Waiting for Approval")
                {
                    dtemilestone.ClientEnabled = false;
                }
            }

        }
    }
    protected void mem1st_Init(object sender, EventArgs e)
    {
        var dtemilestone = sender as ASPxMemo;
        var container = dtemilestone.NamingContainer as GridViewDataItemTemplateContainer;
        dtemilestone.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb_milestones.PerformCallback('m|{0}|' + s.GetText()); }}", container.KeyValue);
        if (m != null)
        {
            if (m.id != 0)
            {
                if (m.status != "In Development" && m.status != "Waiting for Approval")
                {
                    dtemilestone.ClientEnabled = false;
                }
            }

        }
    }

    protected void gv1_m_PreRender(object sender, EventArgs e)
    {
        //		if (m != null)
        //		{
        //			if (m.id != 0)
        //			{
        //				if ((m.status != "In Development")&&(m.status != "Waiting for Approval"))
        //				{
        //					gv1_m.Enabled = false;
        //				}
        //			}
        //			
        //		}
    }


    protected void ddlreportsto_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {




    }

    protected void cb_bonus_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        if (e.Parameter == "clear")
        {
            memcompdetails.Text = "";
            spn_bonus_amt.Value = 0;
            txt_bonus_revenue_threshold.Text = "0";
            txt_bonus_netincome_threshold.Text = "0";
            txt_bonus_margin_threshold.Text = "0";
            ddlbonus.SelectedIndex = 0;

        }
        else if (e.Parameter == "pull_rev")
        {
            var fiscal_year_of_mo = new NeBusinessUnit(comp_id).fiscal_start_current.Month;

            //				double	dolled = _tools.getSQL_double(@"Select ifnull((select sum(ifnull(fytarget_employees.m@v0,0)) from fytarget_employees,new object[] { cb.Text,hid_business_unit_id.Value,spn.Value,y }  where fytarget_employees.target_name =@v0 and fytarget_employees.business_unit_id =@v1  and fytarget_employees.fy =@v2 ),0) ", new object[] { cb.Text,hid_business_unit_id.Value,spn.Value });

            //	double x = _tools.gesst_double("Select Sum(
        }
        else if (e.Parameter == "pull_margin")
        {

        }
        else if (e.Parameter == "pull_net")
        {

        }
        else if (e.Parameter == "update")
        {
            update_comp_verbiage();


        }
        else if (e.Parameter != "0")
        {
            if (ddlbonus.SelectedIndex > 0)
            {
                spn_bonus_amt.MinValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(min_amount,0) from bonus_type  where id =@v0),0) ", new object[] { e.Parameter }));
                spn_bonus_amt.MaxValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(max_amount,0) from bonus_type  where id =@v0),0) ", new object[] { e.Parameter }));
                spn_bonus_amt.ToolTip = _tools.getSQL_string(@"select tooltip from bonus_type  where id =@v0", new object[] { e.Parameter });

                //lbl_default_bonus_verbiage.InnerHtml = _tools.getSQL_string(@"select offer_verbiage from bonus_type  where id =@v0" ,Convert.ToDouble(spn_bonus_amt.Value).ToString("P2"), new object[] { e.Parameter).Replace("zzz });
                memcompdetails.Text = _tools.getSQL_string("select IFNULL(MAX(offer_verbiage), '') from bonus_type where id =@v0", new object[] { e.Parameter }).Replace("zzz", Convert.ToDouble(spn_bonus_amt.Value).ToString("P2") + "),'')");
                memcompdetails.ReadOnly = true;

            }
            else
            {

            }

        }
        else
        {
            spn_bonus_amt.Value = 0;
            memcompdetails.Text = "";
            spn_bonus_amt.Value = 0;
            txt_bonus_revenue_threshold.Text = "0";
            txt_bonus_netincome_threshold.Text = "0";
            txt_bonus_margin_threshold.Text = "0";
            memcompdetails.ReadOnly = false;
        }
        if (ddlbonus.Value.ToString() == "3")
        {
            var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
            row_bonus.Visible = true;
            var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
            row_rev.Visible = true;
            var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
            row_mar.Visible = true;
            var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
            row_net.Visible = false;
            var row_net_hw = (HtmlControl)cb_bonus.FindControl("row_net_hw");
            row_net_hw.Visible = false;
            memcompdetails.ReadOnly = true;

        }
        else if (ddlbonus.Value.ToString() == "10")
        {
            var row_bonus = (HtmlControl)cb_bonus.FindControl("row_bonus");
            row_bonus.Visible = false;
            var row_rev = (HtmlControl)cb_bonus.FindControl("row_rev");
            row_rev.Visible = false;
            var row_mar = (HtmlControl)cb_bonus.FindControl("row_margin");
            row_mar.Visible = false;
            var row_net = (HtmlControl)cb_bonus.FindControl("row_net");
            row_net.Visible = true;
            var row_net_hw = (HtmlControl)cb_bonus.FindControl("row_net_hw");
            row_net_hw.Visible = true;
            memcompdetails.ReadOnly = true;
        }
    }

    protected void update_comp_verbiage()
    {
        if (ddlbonus.Value == null)
            return;
        if (ddlbonus.Value.ToString() != "0")
        {
            if (ddlbonus.Value.ToString() == "3")
            {
                #region calculate high water mark
                // figure out the net income of the previous 3 12 month periods and pick the highest.



                #endregion
            }
            memcompdetails.Text = _tools.getSQL_string(@"select IFNULL(MAX(offer_verbiage), '') from bonus_type  where id =@v0", new object[] { ddlbonus.Value });
            memcompdetails.Text = memcompdetails.Text.Replace("{branch}", new NeBusinessUnit(ddlcompany.Value).name);
            if (txt_bonus_margin_threshold.Text != "0" && txt_bonus_margin_threshold.Text != "")
            {
                memcompdetails.Text = memcompdetails.Text.Replace("{margin}", Convert.ToDouble(txt_bonus_margin_threshold.Text.Replace("$", "").Replace(",", "")).ToString("C2"));
            }

            if (txt_bonus_netincome_threshold.Text != "0" && txt_bonus_netincome_threshold.Text != "")
            {
                memcompdetails.Text = memcompdetails.Text.Replace("{netincome}", Convert.ToDouble(txt_bonus_netincome_threshold.Text.Replace("$", "").Replace(",", "")).ToString("C2"));

            }

            if (txt_bonus_revenue_threshold.Text != "0" && txt_bonus_revenue_threshold.Text != "")
            {
                memcompdetails.Text = memcompdetails.Text.Replace("{revenue}", Convert.ToDouble(txt_bonus_revenue_threshold.Text.Replace("$", "").Replace(",", "")).ToString("C2"));
            }
            if (txt_bonus_hw.Text != "0" && txt_bonus_hw.Text != "")
            {
                memcompdetails.Text = memcompdetails.Text.Replace("{highwatermark}", Convert.ToDouble(txt_bonus_hw.Text.Replace("$", "").Replace(",", "")).ToString("C2"));
            }
            if (Convert.ToDouble(spn_bonus_amt.Value) > 0)
            {
                memcompdetails.Text = memcompdetails.Text.Replace("{bonus_amount}", Convert.ToDouble(spn_bonus_amt.Value).ToString("P2"));

            }
        }
    }

    protected void hl_ticket_Init(object sender, EventArgs e)
    {
        var hl = sender as ASPxHyperLink;
        hl.ClientEnabled = true;
        hl.Enabled = true;
        if (hl.Text != "0" && hl.Text != "")
        {
            hl.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('/sections/member/tickets/ticketpage.aspx?issue={0}','ticket',950,800) }}", hl.Text);
        }
        else
        {
            hl.ClientEnabled = false;
            hl.Enabled = false;
        }
    }
    protected void btn_create_ticket_Init(object sender, EventArgs e)
    {
        var btn_ticket = sender as ASPxButton;
        var container = btn_ticket.NamingContainer as GridViewDataItemTemplateContainer;
        var index = gv1_m.FindVisibleIndexByKeyValue(container.KeyValue);
        if (gv1_m.GetRowValuesByKeyValue(container.KeyValue, "ticketid").ToString() == "0")
        {

            btn_ticket.ClientEnabled = true;
            btn_ticket.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('/sections/member/tickets/CreateIssue.aspx?from=milestone&ms_id={0}','ticket',1030,800) }}", container.KeyValue);
        }
        else
        {
            btn_ticket.ClientEnabled = false;
        }

    }
    protected void gv1_m_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        if (m != null)
        {
            if (m.id != 0)
            {
                if (m.status != "In Development" && m.status != "Waiting for Approval")
                {
                    e.Enabled = false;
                }
            }
        }
    }
    protected void hl_ticket_PreRender(object sender, EventArgs e)
    {
        var hl = sender as ASPxHyperLink;
        hl.ClientEnabled = true;
        hl.Enabled = true;
        if (hl.Text != "0" && hl.Text != "")
        {
            hl.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('/sections/member/tickets/ticketpage.aspx?issue={0}','ticket',950,800) }}", hl.Text);
        }
        else
        {
            hl.ClientEnabled = false;
        }
    }
    protected void ddlpaytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (applicant != null)
        {
            if (Convert.ToInt16(ddlpaytype.Value) <= 3) // if its not a subcontractor.
            {
                pnl_compplan.ClientVisible = true;
                pnl_vacation.ClientVisible = true;
                pnl_subcontractor_stuff.ClientVisible = false;
                pnl_not_subcontractor_stuff.ClientVisible = true;
                ddlsubcontractor.Value = 0;
            }
            else
            {
                pnl_compplan.ClientVisible = false;
                pnl_vacation.ClientVisible = false;
                pnl_subcontractor_stuff.ClientVisible = true;
                pnl_not_subcontractor_stuff.ClientVisible = false;

            }
        }
        else if (user != null)
        {
            if (Convert.ToInt16(ddlpaytype.Value) <= 3) // if its not a subcontractor.
            {
                pnl_compplan.ClientVisible = true;
                pnl_vacation.ClientVisible = true;
                pnl_subcontractor_stuff.ClientVisible = false;
                pnl_not_subcontractor_stuff.ClientVisible = true;
                ddlsubcontractor.Value = 0;
                if (txtvac1amt.Text == "0")
                {
                    #region vacation
                    if (user.business_unit.country == "USA")
                    {
                        txtvac1amt.Text = user.vacation_amount_1.ToString();
                        txtvac2amt.Text = user.vacation_amount_2.ToString();
                        txtvac3amt.Text = user.vacation_amount_3.ToString();
                    }
                    else
                    {
                        txtvac1amt.Text = user.vacation_amount_1.ToString("P1");
                        txtvac2amt.Text = user.vacation_amount_2.ToString("P1");
                        txtvac3amt.Text = user.vacation_amount_3.ToString("P1");
                    }
                    #endregion
                }
                ddlsubcontractor.Value = 0;
            }
            else
            {
                memcompdetails.Text = "";
                ddlbonus.Value = 0;
                spn_bonus_amt.Value = 0;
                txt_bonus_revenue_threshold.Text = "0";
                txt_bonus_margin_threshold.Text = "0";
                txt_bonus_netincome_threshold.Text = "0";
                pnl_compplan.ClientVisible = false;
                pnl_vacation.ClientVisible = false;
                pnl_subcontractor_stuff.ClientVisible = true;
                pnl_not_subcontractor_stuff.ClientVisible = false;
                txtvac1amt.Text = "0";
                txtvac2amt.Text = "0";
                txtvac3amt.Text = "0";
                txtvac1int.Text = "0";
                txtvac2int.Text = "0";
                txtvac3int.Text = "0";
            }
        }
    }

    private DataTable _payperiod_array;
    private NePayPeriod get_associated_payperiod(DateTime _date)
    {
        if (_payperiod_array == null)
        {
            _payperiod_array = Toolbox.doSQL_dt(@"SELECT payperiodid,startdate, enddate FROM payperiods", null);
        }
        var pp = new NePayPeriod();
        var payperiods = _payperiod_array.Select(string.Format("startdate <= '{0}' AND enddate >= '{0}'", Toolbox.MySQL_shortdt(_date)));
        if (payperiods.Length > 0)
        {
            pp.id = (int)payperiods[0]["payperiodid"];
            pp.start_date = (DateTime)payperiods[0]["startdate"];
            pp.end_date = (DateTime)payperiods[0]["enddate"];
        }
        return pp;
    }
    protected void dteStart_CalendarDayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
    {
        var pp = get_associated_payperiod(e.Date);

        //	if (m!=null && m.id!=0)
        //	{

        if (user != null && isapplicant == 0 && user.hrstatus_id != 5 && user.hrstatus_id != 4)
        {
            //		lbl_pp_note.ClientVisible = true;
            if (e.Date != pp.start_date || e.Date < Convert.ToDateTime(user.StartDate))
            {
                e.Cell.Enabled = false;
                e.Cell.Attributes["style"] = "pointer-events: none; background-color: lightgray";
            }
        }
        else
        {
            //		lbl_pp_note.ClientVisible = false;
        }
        //}
    }

    protected void dteEnd_CalendarDayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
    {
        var pp = get_associated_payperiod(e.Date);
        if (e.Date != pp.start_date.AddDays(13))
        {
            e.Cell.Enabled = false;
            e.Cell.Attributes["style"] = "pointer-events: none; background-color: lightgray";
        }
    }
    protected void btnprint0_Click(object sender, EventArgs e)
    {
        if (id != 0)
        {
            //			NeMemberOffer mo;
            //			mo = new NeMemberOffer(Convert.ToInt32(id));
            //			populate_newform();
            //			txtstatus.Text = mo.status;
            if (txtstatus.Text == "In Development")
            {
                NeMemberType mt = new NeMemberType(ddlmembertype.Value);
                double co = mt.GetChargeoutforbranch(ddlcompany.Value);
                if (co <= 0)
                {
                    pop_chargeout.ShowOnPageLoad = true;
                    return;
                }
                validate();
                save();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('offer_print_off.aspx?moid=" + id + "','mo',700,900)", true);
        }

    }
    protected void ASPxButton7_Click(object sender, EventArgs e)
    {
        var mo = new NeMemberOffer(Convert.ToInt32(hid_moid.Value));
        #region save a version into the filestore
        var stream = new MemoryStream();
        if (mo.vendor_id != 0)
        {
            var e_info = new subcontract_employment();
            var eo = (subcontract_employment)NeMemberOffer.fill_report_sub(e_info, mo);
            e_info.CreateDocument();
            e_info.PrintingSystem.ExportToPdf(stream);
        }
        else
        {
            var e_info = new emp_offer();
            var eo = (emp_offer)NeMemberOffer.fill_report(e_info, mo.id, 0);
            e_info.CreateDocument();
            e_info.PrintingSystem.ExportToPdf(stream);
        }

        stream.Seek(0, SeekOrigin.Begin);
        var fileLength = (int)stream.Length;
        var rawdata = new byte[fileLength];
        stream.Read(rawdata, 0, (int)fileLength);
        var f = new file_store.fileObj();
        f.page_id = Convert.ToInt32(127);
        f.folder_id = 4;
        f.sub_folder_id = mo.id;
        if (Toolbox.doSQL_int(@"Select count(id) from filestore.files where (folder_id=4 and sub_folder_id=@v0)", mo.id) > 0)
        {
            Toolbox.doSQL_void(@"delete from filestore.files where (folder_id=4 and sub_folder_id=@v0) ", mo.id);
        }
        f.name = "Employment Agreement " + mo.id;
        f.ext = "pdf";
        f.mime = "application/pdf";

        f.content = rawdata;
        f.save();


        #endregion
    }

    protected void ASPxPageControl1_Callback(object sender, CallbackEventArgsBase e)
    {

        if (e.Parameter == "benefits_check")
        {
            var mo = new NeMemberOffer(Convert.ToInt32(hid_moid.Value));
            if (mo.id != 0)
            {
                if (mo.isapplicant)
                {
                    NeMemberType mt = new NeMemberType(ddlmembertype.Value);
                    dte_benefits_startdate.Date = dteStart.Date.AddDays(mt.benefits_start_default * 7);
                }
            }
        }
        if (e.Parameter == "benefits_check_mt")
        {
            var mo = new NeMemberOffer(Convert.ToInt32(hid_moid.Value));
            if (mo.id != 0)
            {
                if (mo.isapplicant)
                {
                    NeMemberType mt = new NeMemberType(ddlmembertype.Value);
                    dte_benefits_startdate.Date = dteStart.Date.AddDays(mt.benefits_start_default * 7);
                }

            }
        }



        else
        {
            if (ddlreportsto.SelectedIndex >= 0 && user != null)
            {

                if (NeMember.Check_for_circular_org_chart(user.id, Convert.ToInt32(ddlreportsto.Value)))
                {
                    var rr = ddlreportsto.Text;
                    ddlreportsto.SelectedIndex = -1;
                    ddlreportsto.DataBind();
                    ASPxPageControl1.JSProperties["cp_alert"] = string.Format("Setting " + txtEmpName.Text + "'s supervisor to " + rr + " will result in a circular reporting structure.  Please pick another supervisor, or review your org chart.");

                }
                else
                {
                    ASPxPageControl1.JSProperties["cp_alert"] = "";
                }

            }
        }
    }

    protected void btn_save_chargeout_Click(object sender, EventArgs e)
    {

        NeMemberType.save_chargeout_rate(Convert.ToInt32(ddlmembertype.Value), current_user.id32, Convert.ToDouble(txt_chargeout.Text), Convert.ToInt32(ddlcompany.Value));

        NeMemberType mt = new NeMemberType(ddlmembertype.Value);
        double co = mt.GetChargeoutforbranch(ddlcompany.Value);
        ASPxCallbackPanel1.JSProperties["cp_chargeout"] = null;
        if (co <= 0)
        {
            ASPxCallbackPanel1.JSProperties["cp_chargeout"] = "1";
        }
        pop_chargeout.ShowOnPageLoad = false;
    }

    protected void pop_chargeout_WindowCallback(object source, PopupWindowCallbackArgs e)
    {

    }
}


