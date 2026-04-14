using System;
using System.Linq;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_hr_member_m_type_detail : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id  = 134;

	int mtid = 0;
	private bool can_edit = false;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(page_id);
		}
	protected void Page_Load(object sender, EventArgs e)
	{
		can_edit = current_user.AuthenticatedForPrivilege(155);  // they must have CAP edit 
		var _q = Request.QueryString;
		mtid = string.IsNullOrEmpty(_q["mt_id"]) || _q["mt_id"] == "0" ? 0 : Convert.ToInt32(_q["mt_id"]);
		if (!IsPostBack)
		{
			if (mtid != 0)
			{
				PopulateNewForm();
			}
			else
			{
				clearform();
			}
		}

	}
	protected void PopulateNewForm()
	{
		var mt = new NeMemberType(mtid);
		txtbasechargeout.Text = mt.GetChargeoutforbranch(current_user.business_unit_id).ToString();
		txtexisting.Text = Toolbox.doSQL_int(@"Select ifnull((select count(member.Member_MemberType_ID) from member  where member.business_unit_id != 8 and member.Member_Status = 'Active' and member.business_unit_id in( " + new Current_User().visible_business_units + " ) and member.Member_MemberType_ID =@v0),0) ", new object[] { mt.id }).ToString();
		ddlstatus.Value = Convert.ToInt32(mt.active);
        
		txtname.Text = mt.name;
		hdnid.Value = mtid.ToString();
		lblid.Text = mtid.ToString();
		ddlreportsto.Value = Convert.ToInt32(mt.reports_to);
		ASPxMemo1.Text = mt.objective;
		gv_jr.Enabled = true;
		ddlbenefits.Text = mt.benefitplan;
		gv_q.Enabled = true;
		chkelevated.Checked = mt.is_elevated;
		chkrates.Checked = mt.show_on_ratesheet;
		chk_is_scheduled.Checked = mt.is_scheduled;
		chk_is_teamlead.Checked = mt.is_team_leader;
		spn_severance.Value = mt.exec_severance_package;
        spn_benefits.Value = mt.benefits_start_default;
		chkConsideredPM.Checked = mt.considered_pm;
		chk_is_oncall.Checked = mt.is_oncall;
		chkChargeoutOnly.Checked = mt.IsChargeoutOnly;
        chk_considered_field_staff.Checked = mt.considered_field_staff;

        ASPxComboBox1.Value = new NeBusinessUnit(current_user.business_unit_id).Prov;
		pc.TabPages[1].ClientVisible	= current_user.AuthenticatedForPrivilege(126); // never use visible.. always use client visible

		if (current_user.AuthenticatedForPrivilege(155))
		{
			btnSave.ClientEnabled = true;
		}
		DataTable dt;
		
		try
		{
			dt = Toolbox.doSQL_dt(@"Select concat(member_fullname,' - ',ddl_name) _name from member,business_unit  where member.business_unit_id = business_unit.id and member_membertype_id =@v0 and member_status = 'Active' and business_unit.id <>8 and member.business_unit_id in( " + new Current_User().visible_business_units + " ) order by business_unit.ddl_name,concat(member_firstname,' ',member_lastname,' - ',name)", new object[] { mt.id });
		}
		catch
		{

			dt = new DataTable();
			dt.Columns.Add("_name");
		}

		ASPxListBox1.DataSource = dt;
		ASPxListBox1.DataBind();
		
		
	}
	protected void clearform()
	{
		txtbasechargeout.Text = "";
		txtexisting.Text = "0";
		txtname.Text = "";
		ddlreportsto.Value = 5;
		ddlbenefits.SelectedIndex = 0;
		ASPxMemo1.Text = "";
		chkrates.Checked = false;
		chkelevated.Checked = false;
		gv_jr.Enabled = false;
		gv_q.Enabled = false;
        spn_severance.Value = 0;
		chkConsideredPM.Checked = false;
       spn_benefits.Value = 12;
		ASPxComboBox1.Value = new NeBusinessUnit(current_user.business_unit_id).Prov;
		ddlstatus.Value = 1;
		chkChargeoutOnly.Checked = true;
        chk_considered_field_staff.Checked = true;
	if (current_user.AuthenticatedForPrivilege(155))
		{
			btnSave.ClientEnabled = true;
		}
	}
	protected void btnSave0_Click(object sender, EventArgs e)
	{
		reset_chargeout();
	}
	protected void reset_chargeout()
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		if (txtbasechargeout.Text != "")
		{
			var co = Convert.ToDouble(txtbasechargeout.Text);
            NeMemberType.save_chargeout_rate(Convert.ToInt32(hdnid.Value), current_user.id32, Convert.ToDouble(txtbasechargeout.Text), Convert.ToInt32(current_user.business_unit_id));
        }
	}
	protected void save()
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		if (string.IsNullOrWhiteSpace(txtname.Text))
		{
			lblerror.Text = "You must enter a valid membertype name";
			return;
		}

		NeMemberType mt = new NeMemberType(mtid)
			{
			name = txtname.Text,
			reports_to = Convert.ToInt32(ddlreportsto.Value),
			active = string.IsNullOrWhiteSpace(txtbasechargeout.Text) ? false : Convert.ToBoolean(ddlstatus.Value),
			show_on_ratesheet = chkrates.Checked,
			is_elevated = chkelevated.Checked,
			objective = ASPxMemo1.Text,
			benefitplan = ddlbenefits.Text,
			is_scheduled = chk_is_scheduled.Checked,
			considered_pm = chkConsideredPM.Checked,
			is_team_leader = chk_is_teamlead.Checked,
			exec_severance_package = Convert.ToInt32(spn_severance.Value),
			benefits_start_default = Convert.ToInt32(spn_benefits.Value),
			is_oncall = chk_is_oncall.Checked,
			IsChargeoutOnly = chkChargeoutOnly.Checked,
			considered_field_staff = chk_considered_field_staff.Checked
			};

		mt.save();

		if (mtid == 0)
		{
			mtid = mt.MemberTypeID;
			if (mtid != 0)
			{
				hdnid.Value = mtid.ToString();
				reset_chargeout();
				gv_jr.Enabled = true;
				gv_q.Enabled = true;
				lblid.Text = mtid.ToString();
				hdnid.Value = mtid.ToString();
				Response.Redirect("m_type_detail.aspx?mt_id=" + mtid);
			}
		}
		else
		{
			PopulateNewForm();
		}

	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		save();

	}
	protected void gv_jr_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var cb = (ASPxComboBox)gv_jr.FindTitleTemplateControl("ddlcr");
		var spnpriority = (ASPxSpinEdit)gv_jr.FindTitleTemplateControl("spnpriority");
		if (cb.Text != "")
		{
			if (Toolbox.doSQL_int(@"Select count(id) from membertype_responsibilities  where membertype_id =@v0 and core_responsibility_id =@v1 ", new object[] { mtid,cb.Value }) == 0)
			{
				Toolbox.doSQL_void(@"Insert into membertype_responsibilities (membertype_id, core_responsibility_id, mt_cr_priority)  values(@v0,@v1,@v2)",new object[] { mtid,cb.Value,spnpriority.Value } );
			}
			gv_jr.DataBind();
		}
	}
	protected void gv_jr_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		Toolbox.doSQL_void(@"delete from membertype_responsibilities  where id =@v0", new object[] { e.Keys[0] });
		gv_jr.CancelEdit();
		e.Cancel = true;
		gv_jr.DataBind();

	}
	protected void gv_jr_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "core_responsibility")
			{
				e.Cell.ToolTip = gv_jr.GetRowValues(e.VisibleIndex, "description").ToString();

			}
		}
	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		save();
		//ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('mt_print_off.aspx?mtid=" + mtid + "&prov=" + ASPxComboBox1.Value + "','mt',900,900)", true);
	}
	protected void cb_mt_cr_priority_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameter.Split('|');
		Toolbox.doSQL_void(@"update membertype_responsibilities  set mt_cr_priority =@v0  where id =@v1", new object[] { p[1],p[0] });

	}
	protected void ASPxSpinEdit1_Init(object sender, EventArgs e)
	{
		var ASPxSpinEdit1 = sender as ASPxSpinEdit;
		var container = ASPxSpinEdit1.NamingContainer as GridViewDataItemTemplateContainer;
		ASPxSpinEdit1.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb_mt_cr_priority.PerformCallback('{0}|' + s.GetValue()+ '|p'); }}", container.KeyValue);

	}
	protected void ASPxButton2_Init(object sender, EventArgs e)
	{
		var b = (ASPxButton)sender;
		b.ClientSideEvents.Click = @"function(s, e) {print_mreview(" + hdnid.Value + @");}";

	}
	protected void gv_comps_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{

		Toolbox.doSQL_void(@"delete from bonus_membertype_link  where id =@v0", new object[] { e.Keys[0] });
		gv_comps.DataBind();
		e.Cancel = true;
		gv_comps.CancelEdit();
	}
	protected void gv_comps_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters != "")
		{
			var x = Toolbox.doSQL_int(@"Select count(id) from bonus_membertype_link  where membertype_id =@v0 and bonus_type_id =@v1 ", new object[] { hdnid.Value,e.Parameters });
			if (x == 0)
			{
				Toolbox.doSQL_void(@"insert into bonus_membertype_link (bonus_type_id,membertype_id)  values(@v0,@v1)",new object[] { e.Parameters,hdnid.Value } );
				gv_comps.DataBind();
			}
		}
	}
	protected void cbp_dg_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		}
	protected void list_dg_left_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var dg_name		= "";
		var lb		= (ASPxListBox) sender;
		var _users	= new DataTable();
		Toolbox.boolstr bs;
		if(e.Parameter != "refresh")
			{
			var ids						= e.Parameter.TrimEnd(',').Split(',').Select(int.Parse).ToList();
			foreach(var dg_id in ids)
				{
				dg_name		= Toolbox.doSQL_string(@"SELECT name FROM distribution_group WHERE id = @v0 ", new object[] {  dg_id } );
				Toolbox.doSQL_void(@"INSERT INTO distribution_group_link (distribution_group_id, membertype_id) VALUES (@v0 , @v1 )", new object[] {  dg_id, mtid } );
				_users		= Toolbox.doSQL_dt(@"SELECT member_ldap_user FROM member WHERE member.business_unit_id in( " + new Current_User().visible_business_units + " ) and member_membertype_id = @v0  AND member_status = 'Active' AND member_ldap_user != ''", new object[] {  mtid } );
				foreach(DataRow dr in _users.Rows)
					{
					var ldap_user	= dr["member_ldap_user"].ToString();
					bs					= NeMember.ldap_group_admin("add", dg_name, ldap_user);
					}
				}
			}
		lb.DataBind();
		}
	protected void list_dg_right_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var dg_name		= "";
		var lb		= (ASPxListBox) sender;
		var _users	= new DataTable();
		Toolbox.boolstr bs;
		if(e.Parameter != "refresh")
			{
			var ids						= e.Parameter.TrimEnd(',').Split(',').Select(int.Parse).ToList();
			foreach(var dg_id in ids)
				{
				dg_name		= Toolbox.doSQL_string(@"SELECT name FROM distribution_group WHERE id = @v0 ", new object[] {  dg_id } );
				Toolbox.doSQL_void(@"DELETE FROM distribution_group_link WHERE distribution_group_id = @v0  AND membertype_id = @v1  LIMIT 1", new object[] {  dg_id, mtid } );
				_users		= Toolbox.doSQL_dt(@"SELECT member_ldap_user FROM member WHERE member.business_unit_id in( " + new Current_User().visible_business_units + " ) and member_membertype_id = @v0  AND member_status = 'Active' AND member_ldap_user != ''", new object[] {  mtid } );
				foreach(DataRow dr in _users.Rows)
					{
					var ldap_user	= dr["member_ldap_user"].ToString();
					bs					= NeMember.ldap_group_admin("remove", dg_name, ldap_user);
					}
				}
			}
		lb.DataBind();
		}
	
}
	

	