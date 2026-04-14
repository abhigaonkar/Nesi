using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using nesi.core;

public partial class sections_hr_member_modules_wage : System.Web.UI.UserControl
	{
	Toolbox	_tools		= new Toolbox();
	public int _id {get;set;}
	public int _comp_id {get;set;}
	public NeMember user {get;set;}
	protected void Page_Load(object sender, EventArgs e)
		{
		BindWageGrid();
		

		}
	#region wage tab
	public void populate()
		{
		BindWageGrid();
		pnlView.Visible				= false;
		if (user.business_unit.country.Equals("CDN"))
		{
		vacation_amount_1.Text		= (user.vacation_amount_1*100).ToString();
		vacation_amount_2.Text		= (user.vacation_amount_2*100).ToString();
		vacation_amount_3.Text		= (user.vacation_amount_3*100).ToString();


		}
		else
		{
					vacation_amount_1.Text		= user.vacation_amount_1.ToString();
		vacation_amount_2.Text		= user.vacation_amount_2.ToString();
		vacation_amount_3.Text		= user.vacation_amount_3.ToString();

		}
			vacation_interval_1.Text	= user.vacation_interval_1.ToString();
		vacation_interval_2.Text	= user.vacation_interval_2.ToString();
		vacation_interval_3.Text	= user.vacation_interval_3.ToString();

			tb_tillstat.Text = user.timetostat.ToString();
			chk_nostat.Checked = user.receive_stat_pay == 0;
		
		




		}
	protected void BindWageGrid()
		{
		WageGrid.DataSource = _tools.getSQL_datatable(@" SELECT mw.memberwage_id, mw.date, mw.currentwage, mw.nextraise, IFNULL(mw.comment,'') comment, m.member_fullname user, mw.bonus_type_id, mw.bonus_amount, bt.bonus_type bonus_name FROM memberwage mw INNER JOIN member m ON mw.member_id_audit = m.member_id left JOIN bonus_type bt ON mw.bonus_type_id = bt.id WHERE memberwage_memberid = @v0  ORDER BY mw.date desc,mw.memberwage_id DESC", new object[] {  _id } );
		WageGrid.DataBind();
		
		if (!IsPostBack)
		{
			var mymember = new NeMember(Session["session"].ToString());
			if (mymember.is_backoffice)
			{
				btnInsertWage.Enabled = true;
				vacation_amount_1.Enabled = mymember.AuthenticatedForPrivilege(37);
				vacation_amount_2.Enabled = mymember.AuthenticatedForPrivilege(37);
				vacation_amount_3.Enabled = mymember.AuthenticatedForPrivilege(37);
				vacation_interval_1.Enabled = mymember.AuthenticatedForPrivilege(37);
				vacation_interval_2.Enabled = mymember.AuthenticatedForPrivilege(37);
				vacation_interval_3.Enabled = mymember.AuthenticatedForPrivilege(37);
				bt_savepanel.Enabled = mymember.AuthenticatedForPrivilege(37);
	//			pnl_vacation.Enabled = mymember.AuthenticatedForPrivilege(37);

			}
		}
		}

	protected void WageGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
			var mymember = new NeMember(Session["session"].ToString());
			if (!mymember.AuthenticatedForPrivilege(101))
			{
				return;
			}
			if (user.hrstatus_id >= 4)
			{
				return;
			}

		pnlView.Visible = true;
		bt_wage_add.Visible = false;
		bt_wage_update.Visible = true;
        btnInsertWage.Visible = false;
		//errorlabel.Text = "";
		txtComment.Text = "";
		var to_member = new NeMember(Convert.ToInt32(_id));
		var dt_bonus = _tools.getSQL_dataset(@"Select 0 id,'Not  Set' bonus_type union SELECT bonus_type.id, bonus_type.bonus_type FROM bonus_type INNER JOIN bonus_membertype_link ON bonus_membertype_link.bonus_type_id = bonus_type.id and bonus_membertype_link.membertype_id =@v0" , new object[] { to_member.MemberTypeID });

		ddlbonus.DataSource = dt_bonus;
		ddlbonus.DataBind();
		var gvr			= WageGrid.Rows[WageGrid.SelectedIndex];
        txt_id.Text = (gvr.Cells[8]).Text;
        txt_id.Enabled = false;
        var bonus_value			= 0;
 		int.TryParse((gvr.Cells[5]).Text, out bonus_value);
			ddlbonus.Value = bonus_value;
		spn_bonusamt.Value = (gvr.Cells[6]).Text;
		if ((ddlbonus.Value!=null)&&(ddlbonus.Value.ToString() !="0"))
		{
			spn_bonusamt.MinValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(min_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
			spn_bonusamt.MaxValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(max_amount,0) from bonus_type  where id =@v0),0) ", new object[] { ddlbonus.Value }));
			spn_bonusamt.ToolTip = _tools.getSQL_string(@"select tooltip from bonus_type  where id =@v0", new object[] { ddlbonus.Value });
			spn_bonusamt.ClientEnabled = true;
		}
		txtDateJoined.Text = (gvr.Cells[0]).Text;
		txtCurrentWage.Text = (gvr.Cells[1]).Text;
		txtNextRaise.Text = (gvr.Cells[2]).Text;
		txtComment.Text = (gvr.Cells[3]).Text.Trim();
		}
	protected void btnUpdate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		BindWageGrid();
		var id			= Convert.ToInt32(WageGrid.DataKeys[WageGrid.SelectedIndex].Value);
		var w	= new NeWage(id.ToString());
		w.date = Convert.ToDateTime(txtDateJoined.Text).ToString("yyyy-MM-dd");
		w.current_wage = Convert.ToDouble(txtCurrentWage.Text);
		w.date_next_raise = Convert.ToDateTime(txtNextRaise.Text).ToString("yyyy-MM-dd");
		w.comment = txtComment.Text;
		w.business_unit_id = Convert.ToInt32(_comp_id);
		w.save();
		Complete();
		}

	protected void btnInsertWage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		//errorlabel.Text = "";
		pnlView.Visible = true;
		bt_wage_add.Visible = true;
		bt_wage_update.Visible = false;
		ClearwageTextBoxes();
		}

	

	protected void Complete()
		{
		bt_wage_add.Visible = false;
		bt_wage_update.Visible = false;
		ClearwageTextBoxes();
        btnInsertWage.Visible = true;
        pnlView.Visible = false;
        txt_id.Text = "";

        //errorlabel.Text = "The Infomation was updated Successfully.";

        BindWageGrid();
		}

	protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{

		ClearwageTextBoxes();
		pnlView.Visible = false;
		//errorlabel.Text = "";
		BindWageGrid();
		}

	protected void ClearwageTextBoxes()
		{
		txtDateJoined.Text = "";
		txtCurrentWage.Text = "";
		txtNextRaise.Text = "";
		txtComment.Text = "";
		}

	protected void WageGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
		WageGrid.PageIndex = e.NewPageIndex;
		WageGrid.DataSource = Toolbox.doSQL_dt(@"SELECT mw.memberwage_id,mw.Date,mw.CurrentWage,mw.NextRaise,IFNULL(mw.comment,'') Comment, m.Member_User as User FROM MemberWage mw Inner join Member m on mw.Member_ID_Audit = m.Member_Id  WHERE MemberWage_MemberID =@v0", new object[] { _id });
		WageGrid.DataBind();
		}
	
	protected void WageGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
		var WageDet = new NeWage();
		if(WageGrid.DataKeys.Count > 0)
			{
			WageDet.id = Convert.ToInt32(WageGrid.DataKeys[e.RowIndex].Value);

			try
				{
				WageDet.delete();
				}
			catch (Exception ex)
				{
				_tools.catch_error(ex);
				}

			finally
				{
				Complete();
				}
			}
		}
	
	protected void bt_wage_cancel_Click(object sender, EventArgs e)
		{
		ClearwageTextBoxes();
		pnlView.Visible = false;
	//	errorlabel.Text = "";
		BindWageGrid();
		}
	protected void bt_wage_add_Click(object sender, EventArgs e)
		{
		var w = new NeWage();
		var mymember	= new NeMember(Session["session"].ToString());
		var to_member	= new NeMember(Convert.ToInt32(_id));
		w.member_id	= user.id;
		w.date = Convert.ToDateTime(txtDateJoined.Text).ToString("yyyy-MM-dd");
		w.current_wage = Convert.ToDouble(txtCurrentWage.Text);
		var next_raise_dt	= txtNextRaise.Text.Trim() == "" ? DateTime.Now.AddYears(1) :  Convert.ToDateTime(txtNextRaise.Text);
		w.date_next_raise = next_raise_dt.ToString("yyyy-MM-dd");
		w.comment = txtComment.Text;
		w.member_id_audit = Convert.ToInt32(mymember.id);
		w.member_id_added_by = Convert.ToInt32(mymember.id);
		w.business_unit_id = Convert.ToInt32(_comp_id);
		w.membertype_id = Convert.ToInt32(to_member.MemberTypeID);
		if ((ddlbonus.Text != "") && (spn_bonusamt.Text != ""))
		{
			w.bonus_type = Convert.ToInt32(ddlbonus.Value);
			w.bonus_amount = Convert.ToDouble(spn_bonusamt.Value);
		}
		w.save();
		shared.alert_payroll("Employee Wage Inserted for "+to_member.FullName, "This is an automated message to let you know that a wage ("+w.current_wage.ToString("C2")+") has been added to "+to_member.FullName);
		Complete();
		}
	protected void bt_wage_update_Click(object sender, EventArgs e)
		{
		BindWageGrid();
  
            var admin = new NeMember(Session["session"].ToString());
            var to_member = new NeMember(Convert.ToInt32(_id));
            var id = Convert.ToInt32(txt_id.Text);
            var w = new NeWage(id.ToString());
            w.date = Convert.ToDateTime(txtDateJoined.Text).ToString("yyyy-MM-dd");
            w.current_wage = Convert.ToDouble(txtCurrentWage.Text);
            var next_raise_dt = txtNextRaise.Text.Trim() == "" ? DateTime.Now.AddYears(1) : Convert.ToDateTime(txtNextRaise.Text);
            w.date_next_raise = next_raise_dt.ToString("yyyy-MM-dd");
            w.comment = txtComment.Text;
            w.business_unit_id = Convert.ToInt32(_comp_id);
            if ((ddlbonus.Text != "") && (spn_bonusamt.Text != ""))
            {
                w.bonus_type = Convert.ToInt32(ddlbonus.Value);
                w.bonus_amount = Convert.ToDouble(spn_bonusamt.Value);
            }
          w.save();
            shared.alert_payroll("Employee Wage Updated for " + to_member.FullName, "This is an automated message to let you know that " + admin.FullName + " has updated " + to_member.FullName + "'s wage (" + w.current_wage.ToString("C2") + ")");
            Complete();
       
		}
	#endregion
	protected void bt_savepanel_Click(object sender, EventArgs e)
		{
		#region prep
		var time_till_stat			= 0;
		var vac_int_1				= 0;
		var vac_int_2				= 0;
		var vac_int_3				= 0;
		decimal vac_amt_1			= 0;
		decimal vac_amt_2			= 0;
		decimal vac_amt_3			= 0;
		int.TryParse(tb_tillstat.Text, out time_till_stat);
		int.TryParse(vacation_interval_1.Text, out vac_int_1);
		int.TryParse(vacation_interval_2.Text, out vac_int_2);
		int.TryParse(vacation_interval_3.Text, out vac_int_3);
		decimal.TryParse(vacation_amount_1.Text, out vac_amt_1);
		decimal.TryParse(vacation_amount_2.Text, out vac_amt_2);
		decimal.TryParse(vacation_amount_3.Text, out vac_amt_3);
		#endregion prep

		if (user.paytype_id > 3 && user.paytype_id != 7)
		{
			user.receive_stat_pay =  0;
			user.timetostat = 9999;
		}
		else
		{
			user.receive_stat_pay = chk_nostat.Checked ? 0 : 1;
			user.timetostat = time_till_stat;
		}

			if (user.business_unit.country.Equals("CDN"))
			{
				user.vacation_amount_1 = vac_amt_1/100;
				user.vacation_amount_2 = vac_amt_2/100;
				user.vacation_amount_3 = vac_amt_3/100;
			}
			else
			{
				user.vacation_amount_1 = vac_amt_1;
				user.vacation_amount_2 = vac_amt_2;
				user.vacation_amount_3 = vac_amt_3;
			}

			user.vacation_interval_1	= vac_int_1;
		user.vacation_interval_2	= vac_int_2;
		user.vacation_interval_3	= vac_int_3;
		user.save();
		var csm = Page.ClientScript;
		csm.RegisterClientScriptBlock(this.GetType(), "", "<script> please_wait('stop');</script>");


		}

	protected void cb_bonus_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if ((ddlbonus.Value != null) && (ddlbonus.Value.ToString() != "0"))
		{
			spn_bonusamt.ClientEnabled = true;
			spn_bonusamt.MinValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(min_amount,0) from bonus_type  where id =@v0),0) ", new object[] { e.Parameter }));
			spn_bonusamt.MaxValue = Convert.ToDecimal(_tools.getSQL_double(@"select ifnull((Select ifnull(max_amount,0) from bonus_type  where id =@v0),0) ", new object[] { e.Parameter }));
			spn_bonusamt.ToolTip = _tools.getSQL_string(@"select tooltip from bonus_type  where id =@v0", new object[] { e.Parameter });
		}
	}
	protected void btnInsertWage_Click(object sender, EventArgs e)
	{
		pnlView.Visible = true;
		bt_wage_add.Visible = true;
		bt_wage_update.Visible = false;
		ClearwageTextBoxes();
	}
}