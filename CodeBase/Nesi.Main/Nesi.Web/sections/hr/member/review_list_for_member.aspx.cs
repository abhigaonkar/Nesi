using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using System.Web.UI;
using nesi.core;
using NESI.Common.Models;

public partial class sections_hr_member_review_list_for_member : System.Web.UI.Page
{
	NeMember current_user;
	NeEmpReview review;
	Toolbox _tools;
	protected void Page_Load(object sender, EventArgs e)
	{

		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);  // if the user has access to the member page, they can add a review
		var _q = Request.QueryString;
		hdnmemberid.Value = _q["memberid"];
		

		ddloffers.DataSource = _tools.getSQL_datatable(@"Select id,concat(DATE_FORMAT(date,'%d %b %Y'),' - ',status) _name from member_offers  where memberid =@v0 and (status = 'Accepted') order by id desc", new object[] { Convert.ToInt32(hdnmemberid.Value) });
		ddloffers.DataBind();
var mem = new NeMember(Convert.ToInt32(hdnmemberid.Value));


		if (!IsPostBack)
		{
			Session["reviewid"] = null;
			Session["reviewid_locked"] = "0";
			
			ddlreviewed.DataSource = _tools.getSQL_datatable(@" call get_possible_reviewers(@v0,0)
 ", new object[] { mem.id });

            
		}
		ddlreviewed.DataBind();
		if (Session["reviewid"] != null)
		{
			hdnr_id.Value = Session["reviewid"].ToString();
			review = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));
		}
		else
		{
			Session["reviewid"] = _q["id"];
			hdnr_id.Value = _q["id"];
			review = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));
		}
		//		NeEmpReview review1 = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));
		if (hdnr_id.Value != "0")
		{
			if (
					! ((review.reviewed_by_id == current_user.id) ||
						(current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewAllReviews))||
					   (NeMember.is_supervisor(review.member_id, current_user.id)) || 
					  ((review.member_id == current_user.id) && (review.status == "Delivered"))))
			{
				Toolbox.FriendlyException(Response, "You are not allowed to see this review", "./index.aspx?id=");
			}
		}

		if (!IsPostBack)
		{
	

			if (hdnr_id.Value != "0")  // if its an edit 
			{
				Session["reviewid_locked"] = "0";
				review = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));

				lblid0.Text = mem.FullName;
				#region Build DDLReviewed datasource
				if (ddlreviewed.Items.IndexOfValue(review.reviewed_by.id) >= 0)
				{
					ddlreviewed.Value = review.reviewed_by.id;
				}
				else
				{
					ddlreviewed.DataBind();
					var li = new ListEditItem(review.reviewed_by.FullName, review.reviewed_by.id);
					ddlreviewed.Items.Add(li);
					ddlreviewed.Text = review.reviewed_by.FullName;
				}

				if (ddlreviewed.Items.IndexOfValue(review.offer.reports_to) >= 0)
				{
				}
				else
				{
					ddlreviewed.DataBind();
					var li = new ListEditItem(new NeMember(Convert.ToInt32(review.offer.reports_to)).FullName, review.offer.reports_to);
					ddlreviewed.Items.Add(li);
					ddlreviewed.Text = review.reviewed_by.FullName;
				}
				if (ddlreviewed.Items.IndexOfValue(mem.reports_to) >= 0)
				{
				}
				else
				{
					ddlreviewed.DataBind();
					var li = new ListEditItem(new NeMember(Convert.ToInt32(mem.reports_to)).FullName, mem.reports_to);
					ddlreviewed.Items.Add(li);
					ddlreviewed.Text = review.reviewed_by.FullName;
				}
				#endregion

				#region gv_milestones datasource
				gv_milestones.DataSource = _tools.getSQL_datatable(@"SELECT memberoffer_milestones.milestone, memberoffer_milestones.due, memberoffer_milestones.id, memberoffer_milestones.ticketid, memberoffer_milestones.notes, emp_review.id, memberoffer_milestones.completed FROM memberoffer_milestones INNER JOIN emp_review ON emp_review.offerid = memberoffer_milestones.offerid  where emp_review.id =@v0", new object[] { hdnr_id.Value });
				gv_milestones.DataBind();

				#endregion

				dtereview.Date = review.date;
				hdnmemberid.Value = review.member_id.ToString();
				chklocked.Checked = Convert.ToBoolean(review.locked);
				chklocked.ClientEnabled = !(chklocked.Checked);
				lblstatus.Text = review.status;
				ddloffers.ClientEnabled = false;
				ddloffers.Value = review.offer.id;
				lblmt.Text = review.mt.name;
				this.Title = "Employee Review for " + mem.FullName;
				btnprint_worksheet.ClientEnabled = true;
				setup_buttons();
				

			}
			else  // if its new
			{
				lblid0.Text = mem.FullName;
				lblmt.Text = mem.membertype.name;
				ddlreviewed.Value = current_user.id;
				dtereview.Date = System.DateTime.Today;
				lblstatus.Text = "Not Started Yet...";
				ddloffers.SelectedIndex = 0;
				btnsave.Text = "Next ->";
				try
				{
					lblmt.Text = new NeMemberType(new NeMemberOffer(Convert.ToInt32(ddloffers.Value)).membertypeid).name;
				}
				catch { }
			}
	

		}

		if (hdnr_id.Value != "0") // if its an edit, enable the review tabs
		{
			ASPxPageControl1.TabPages[0].ClientEnabled = true;  // standard employee setup
			ASPxPageControl1.TabPages[1].ClientEnabled = true;  // COre REsponsibilites questions
			ASPxPageControl1.TabPages[3].ClientEnabled = true;  // milesotnes from the review
			if (NeMember.is_supervisor(review.member_id, current_user.id))  // if the current user is somehow teu branch managers boss
			{
				ASPxPageControl1.TabPages[4].ClientEnabled = true;  // member history
				fill_history();  // fill the history grid.
			}
		}

		fill_gv_reviewitem();  // fil generic list
		fill_gv_reviewitem0(); // fil list from respomsibilites
		populate_details();   // populate top right corner with the score

	}

	protected void setup_buttons()
	{
		review = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));
		if (hdnr_id.Value != "0")
		{
			btnsave.Text = "Save";
			lblid.Text = hdnr_id.Value;
		}
		lblstatus.Text = review.status;
		if (review.locked == 1)
		{
			btn_print_final.ClientVisible = true;
			btnprint_worksheet.ClientVisible = false;
			btnclose.ClientVisible = false;
			
			chklocked.ClientEnabled = NeMember.is_supervisor(review.reviewed_by_id,current_user.id); // the supervisor of the reviewer can unlock a review
			Session["reviewid_locked"] = "1";
			ddlreviewed.ClientEnabled = false;
			dtereview.ClientEnabled = false;
			lbl_whocanunlock.Visible = true;
			btnsave.ClientEnabled = NeMember.is_supervisor(review.reviewed_by_id, current_user.id); // the supervisor of the reviewer can unlock a review
            if (review.was_printed == 1)
            {
                chklocked.ClientEnabled = false;
                btnsave.ClientEnabled = false;
                lbl_whocanunlock.Text = "Because the final printout was printed, this review can't be unlocked.";
            }
            else
            {
                if (new NeMember(review.reviewed_by_id).reports_to != 0)
                {

                    lbl_whocanunlock.Text = "This can be unlocked by " + new NeMember(new NeMember(review.reviewed_by_id).reports_to).FullName;
                }
            
			}
		}
		else
		{
			btn_print_final.ClientVisible = false;
			btnprint_worksheet.ClientVisible = true;
			btnclose.ClientVisible = true;
			chklocked.ClientEnabled = true;
			ddlreviewed.ClientEnabled = true;
			dtereview.ClientEnabled = true;
			lbl_whocanunlock.Visible = false;
			btnsave.ClientEnabled=true;
		}
		if ((review.status=="Closed") ||(review.status == "Delivered"))
		{
			
			btnsave.ClientEnabled=false;
		}

	}
	protected void populate_details()
	{
		lblid.Text = hdnr_id.Value;
		var str = "<table style='font-family: Arial; font-size: 10pt'>";
		var dt = _tools.getSQL_datatable(@"SELECT emp_review_group.group g, ifnull(avg(if(emp_review_history.score>0,emp_review_history.score,null)),0) score FROM emp_review_history INNER JOIN emp_review_items ON emp_review_history.emp_review_item_id = emp_review_items.id INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id  WHERE emp_review_history.emp_review_id =@v0", new object[] { Session["reviewid"] });
		foreach (DataRow dr in dt.Rows)
		{
			var x = Convert.ToDouble(dr[1]).ToString("n1") == "0.0"? "Not Scored" : Convert.ToDouble(dr[1]).ToString("n1");
			str += "<tr><td>General Review:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' style='font-family: Arial; font-size: 20pt'>" + x + "</td></tr>";

		}
		str += "<tr><td></td><td></td></tr>";
		str += "<tr><td></td><td></td></tr>";
		dt = _tools.getSQL_datatable(@"SELECT core_responsibilities.core_responsibility g, ifnull(avg(if(emp_review_history.score>0,emp_review_history.score,null)),0) score FROM emp_review_history INNER JOIN cr_review ON emp_review_history.cr_review_item_id = cr_review.cr_review_id INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id  WHERE emp_review_history.emp_review_id =@v0", new object[] { Session["reviewid"] });


		foreach (DataRow dr in dt.Rows)
		{
			var x = Convert.ToDouble(dr[1]).ToString("n1") == "0.0" ? "Not Scored" : Convert.ToDouble(dr[1]).ToString("n1");
			str += "<tr><td nowrap='nowrap'>Core Responsibilities:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align='center' style='font-family: Arial; font-size: 20pt'>" + x + "</td></tr>";

		}
		results.InnerHtml = str + "</table>";
		
	


	}

	protected void fill_history()
	{
		#region fill history
		if (review != null && review.id != 0)
		{
			var dt_history = new DataTable();
			dt_history.Columns.Add("date");
			dt_history.Columns.Add("action");
			dt_history.Columns.Add("added_by");
			dt_history.Columns.Add("notes");

			var mem = new NeMember(Convert.ToInt32(review.member_id));
			var dt = new DataTable();

			dt = _tools.getSQL_datatable(@"Select emp_review.date,emp_review.reviewed_by_id from emp_review  where emp_review.member_id =@v0 and locked = 1", new object[] { mem.id });
			foreach (DataRow dr in dt.Rows)
			{
				dt_history.Rows.Add(Convert.ToDateTime(dr[0]).ToString("yyyy-MM-dd"), "Employee Review", new NeMember(Convert.ToInt32(dr[1])).FullName, "");
			}
			dt = _tools.getSQL_datatable(@"SELECT mn.*, m.Member_fullname FROM MemberNote mn Inner join Member m on mn.Member_ID_Audit = m.Member_Id  WHERE MemberNote_Member_ID =@v0 order by Date desc", new object[] { mem.id });
			foreach (DataRow dr in dt.Rows)
			{
				dt_history.Rows.Add(Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd"), "Discipline", dr["Member_fullname"], dr["Comments"]);
			}

			dt_history.Rows.Add(Convert.ToDateTime(mem.StartDate).ToString("yyyy-MM-dd"), "Start Date", "", "");
			var dv = dt_history.DefaultView;
			dv.Sort = "date desc";
			var sortedDT = dv.ToTable();
			gv_history.DataSource = sortedDT;
			gv_history.DataBind();
		}

		#endregion

}

	protected void fill_gv_reviewitem()
	{
		gv_reviewitem.DataSource = _tools.getSQL_datatable(@"SELECT emp_review_history.id, emp_review_group.group, emp_review_history.emp_review_question reviewitem, emp_review_history.notes, emp_review_history.score, emp_review_items.description, emp_review_group.notes FROM emp_review_history INNER JOIN emp_review_items ON emp_review_history.emp_review_item_id = emp_review_items.id INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id  WHERE emp_review_history.emp_review_id =@v0", new object[] { Session["reviewid"] });
		if (!IsCallback)
		{
			if (!cb_expand.Checked)
			{
				gv_reviewitem.GroupBy(gv_reviewitem.Columns["group"]);
				gv_reviewitem.CollapseAll();
			}
			else
			{
				gv_reviewitem.GroupBy(gv_reviewitem.Columns["group"]);
				gv_reviewitem.ExpandAll();
			}
		}
		gv_reviewitem.DataBind();
	}
	protected void fill_gv_reviewitem0()
	{

		gv_reviewitem0.DataSource = _tools.getSQL_datatable(@"SELECT emp_review_history.id, emp_review_history.cr_review_question, emp_review_history.notes, emp_review_history.score, core_responsibilities.core_responsibility AS cr, cr_review.cr_review_cr_id cr_id FROM emp_review_history INNER JOIN cr_review ON emp_review_history.cr_review_item_id = cr_review.cr_review_id INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id  WHERE emp_review_history.emp_review_id =@v0", new object[] { Session["reviewid"] });
		if (!IsCallback)
		{
			if (!cb_expand0.Checked)
			{
				gv_reviewitem0.GroupBy(gv_reviewitem0.Columns["cr"]);
				gv_reviewitem0.CollapseAll();
			}
			else
			{
				gv_reviewitem0.GroupBy(gv_reviewitem0.Columns["cr"]);
				gv_reviewitem0.ExpandAll();
			}
		}
gv_reviewitem0.DataBind();
	}
	
	protected void gv_reviewitem_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		var _tools = new Toolbox();
		var gv = (ASPxGridView)sender;
		if (e.VisibleIndex >= 0)
		{
			if (e.RowType != GridViewRowType.Group)
			{
				if ((Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "score")) < 3) && (Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "score")) != 0))
				{
					e.Row.BackColor = System.Drawing.Color.LightSalmon;
				}
			}
			if (e.RowType == GridViewRowType.Group)
			{
				e.Row.Font.Size = FontUnit.Point(10);
				e.Row.Font.Bold = true;
				double x = 0;
				if (gv.ID == "gv_reviewitem")
				{
					x = _tools.getSQL_double(@"SELECT ifnull(avg(emp_review_history.score),0) FROM emp_review_history INNER JOIN emp_review_items ON emp_review_history.emp_review_item_id = emp_review_items.id INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id  WHERE emp_review_history.emp_review_id =@v0 and emp_review_history.score>0 and emp_review_group.group =@v1 ", new object[] { Session["reviewid"],e.GetValue("group") });
				}
				else
				{
					x = _tools.getSQL_double(@"SELECT ifnull(avg(emp_review_history.score),0) from emp_review_history INNER JOIN cr_review ON emp_review_history.cr_review_item_id = cr_review.cr_review_id INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id  WHERE emp_review_history.emp_review_id =@v0 and emp_review_history.score>0 and core_responsibilities.id =@v1 ", new object[] { Session["reviewid"],e.GetValue("cr_id") });
				}
					if (x != 0 && x < 3)
				{
					e.Row.BackColor = System.Drawing.Color.Red;
				}
			}
			
		}
	}
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		ddloffers.DataSource = _tools.getSQL_datatable(@"Select id,concat(DATE_FORMAT(date,'%d %b %Y'),' - ',status) _name from member_offers  where memberid =@v0 and (status = 'Accepted' or status = 'Previous')", new object[] { Convert.ToInt32(hdnmemberid.Value) });
		ddloffers.DataBind();
		if (dtereview.Text == "")
		{
			throw new Exception("You must enter a valid date.");
		}
		if (ddlreviewed.Text == "")
		{
			throw new Exception("You must select a valid reviewer.");
		}
		if (ddloffers.Text == "")
		{
			throw new Exception("You must select a valid employment agreement to review against.  If an employment agreement hasn't been created, please created one via the Employment Agreements Tab.");
		}

		

		if (Session["reviewid"] == null || hdnr_id.Value == "0")
		{
		var x=	NeEmpReview.create_review(Convert.ToInt32(ddloffers.Value), dtereview.Date, Convert.ToInt32(ddlreviewed.Value));
		Session["reviewid"] = x.ToString();

		cb.JSProperties["cp_parent"] = "1";
		cb.JSProperties["cp_url"] = "review_list_for_member.aspx?id=" + x + "&memberid=" + hdnmemberid.Value;

			
	/*		hdnr_id.Value = x.ToString();
			fill_gv_reviewitem();
			fill_gv_reviewitem0();

			//	gv_reviewitem.DataBind();
			//	gv_reviewitem0.DataBind();
			setup_buttons();
			
	 */
		}

		else  // save the review
		{

			var er = new NeEmpReview(Convert.ToInt32(hdnr_id.Value));

			if (chklocked.Checked)
			{
				if (er.was_printed == 1)
				{
					er.status = "Delivered";
				}
				else
				{
					er.status = "Ready to Deliver";
				}
			}
			else
			{
				er.status = "In Development";
			}
			
			er.id = Convert.ToInt32(hdnr_id.Value);
			er.date = dtereview.Date;
			er.locked = Convert.ToInt32(chklocked.Checked);
			er.reviewed_by_id = Convert.ToInt32(ddlreviewed.Value);
			er.save();
			
			setup_buttons();
		}
		
	}
	protected void ASPxRadioButtonList1_Init(object sender, EventArgs e)
	{
		var chk = sender as ASPxRadioButtonList;
		if (Convert.ToString(Session["reviewid_locked"]) == "0")
		{
			var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
			chk.ClientSideEvents.ValueChanged = string.Format(@"function (s, e) {{   cb_grid.PerformCallback('{0}|' + s.GetValue()+ '|g'); }}", container.KeyValue);
		}
		else
		{
			chk.Enabled = false;
		}

	}
	protected void gv_reviewitem_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var p = e.Parameters.Split('|');
	//	ASPxGridView gv = (ASPxGridView)sender;
		if (p.Length == 1)
		{
			
		}
		else
		{
			if (p[2] == "g")
			{

				_tools.getSQL_void(@"update emp_review_history 
set member_id=@v0,date = curdate(),score =@v1 where id =@v2",
					new object[] {
current_user.id,p[1],p[0]
						}
);
				
			}
			else if (p[2] == "n")
			{
				_tools.getSQL_void(@"update emp_review_history 
set member_id=@v0,date = curdate(),notes =@v1 where id =@v2 ",
					new object[] {
current_user.id,p[1],p[0]
						}
);
			}
			
			
		}
	//	fill_gv_reviewitem();
	//	fill_gv_reviewitem0();
	}

	protected void memreviewnotes_Init(object sender, EventArgs e)
	{
		var mem = sender as ASPxMemo;
		if (Convert.ToString(Session["reviewid_locked"]) == "0")
		{
			var container = mem.NamingContainer as GridViewDataItemTemplateContainer;
			mem.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb_grid.PerformCallback('{0}|' + s.GetText()+ '|n'); }}", container.KeyValue);
			mem.ID = "memreviewnotes" + container.KeyValue;
			mem.ClientInstanceName = mem.ID;
		}
		else
		{
			mem.Enabled = false;
		}
		
	}
	

	protected void btnprint_Click(object sender, EventArgs e)  // print the work sheet
	{
		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('print_review.aspx?rid=" + hdnr_id.Value + "&locked=" + chklocked.Value + "&is_worksheet=1','printrev" + hdnr_id.Value + "',900,900)", true);
	
	}

	protected void btnprint0_Click(object sender, EventArgs e)  // print the final
	{
		_tools.getSQL_void(@"update emp_review set locked = 1, was_printed=1, status = 'Delivered' where id = @v0", new object[] { hdnr_id.Value});
		chklocked.Checked = true;
		chklocked.ClientEnabled = false;
		lblstatus.Text = "Delivered";
		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('print_review.aspx?rid=" + hdnr_id.Value + "&locked=" + chklocked.Value + "&is_worksheet=0','printrev" + hdnr_id.Value + "',900,900)", true);
		setup_buttons();
	}
	protected void gv_reviewitem_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
	{
		
	}
	protected void gv_reviewitem_FocusedRowChanged(object sender, EventArgs e)
	{

	}
	protected void gv_reviewitem_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
	{
		var _tools = new Toolbox();
		
			if (e.Column.FieldName == "group")
			{
				var x = _tools.getSQL_double(@"SELECT ifnull(avg(emp_review_history.score),0) FROM emp_review_history INNER JOIN emp_review_items ON emp_review_history.emp_review_item_id = emp_review_items.id INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id  WHERE emp_review_history.emp_review_id =@v0 and emp_review_history.score>0 and emp_review_group.group =@v1 ", new object[] { Session["reviewid"],e.Value });

				if (x == 0)
				{
					e.DisplayText = e.Value + "- Not Scored";
				}
				else
				{
					e.DisplayText = e.Value + "- Score: " + x.ToString("n1");
				}
			}
		
	}
	protected void gv_reviewitem0_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
	{
		var _tools = new Toolbox();

		if (e.Column.FieldName == "cr")
		{
			double x = 0;
			x = _tools.getSQL_double(@"SELECT ifnull(avg(emp_review_history.score),0) from emp_review_history INNER JOIN cr_review ON emp_review_history.cr_review_item_id = cr_review.cr_review_id INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id  WHERE emp_review_history.emp_review_id =@v0 and emp_review_history.score>0 and core_responsibilities.id =@v1 ", new object[] { Session["reviewid"],e.GetFieldValue("cr_id") });

			if (x == 0)
			{
				e.DisplayText = e.Value + "- Not Scored";
			}
			else
			{
				e.DisplayText = e.Value + "- Score: " + x.ToString("n1");

			}
		}
	}
	protected void ASPxRadioButtonList2_Init(object sender, EventArgs e)
	{
		var chk = sender as ASPxRadioButtonList;
		if (Convert.ToString(Session["reviewid_locked"]) == "0")
		{
			var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
			chk.ClientSideEvents.ValueChanged = string.Format(@"function (s, e) {{   cb_grid.PerformCallback('{0}|' + s.GetValue()+ '|g'); }}", container.KeyValue);
		}
		else
		{
			chk.Enabled = false;
		}
	}
/*	protected void btnqc_Click(object sender, EventArgs e)
	{
			try
			{
				NeEMail email = new NeEMail();
				email.From = "administrator@newelectric.com";
				email.To = new NeMember(Convert.ToInt32(review.member.reports_to)).NEEmail;
				email.Subject = current_user.FullName + " has just completed the review for " + review.member.FullName + " (" + review.member.business_unit_name + ")";
				string body = "<font face='arial' size='2'>";
				email.isHTML = true;
				body += "<b>Review:</b> <a href='https://www.nesi.ca/sections/hr/member/review_list_for_member.aspx?memberid=" + hdnmemberid.Value + "&id=" + hdnr_id.Value + "' target = 'review'>" + hdnr_id.Value + "</a><br/>";
				email.Body = body;
				email.Send();
			}
			catch { }
			_tools.getSQL_void(@"update emp_review set status = 'Waiting for Approval'  where id =@v0", new object[] { hdnr_id.Value });
			lblstatus.Text = "Waiting for Approval";
			setup_buttons();
	}
	protected void btnapproved_Click(object sender, EventArgs e)
	{
		try
		{
			NeEMail email = new NeEMail();
			email.From = "administrator@newelectric.com";
			email.To = review.reviewed_by.NEEmail;
			email.Subject = current_user.FullName + " has just APPROVED the review for " + review.member.FullName + " (" + review.member.business_unit_name + ")";
			email.isHTML = true;
			string body = "<font face='arial' size='2'>";

			body += "<b>Review:</b> <a href='https://www.nesi.ca/sections/hr/member/review_list_for_member.aspx?memberid=" + hdnmemberid.Value + "&id=" + hdnr_id.Value + "' target = 'review'>" + hdnr_id.Value + "</a><br/>";
			email.Body = body;
			email.Send();
		}
		catch { }
		_tools.getSQL_void(@"update emp_review set status = 'Ready to Deliver'  where id =@v0", new object[] { hdnr_id.Value });
		lblstatus.Text = "Ready to Deliver";
		
		setup_buttons();
	}
	protected void btnapproved0_Click(object sender, EventArgs e)
	{
		try
		{
			NeEMail email = new NeEMail();
			email.From = "administrator@newelectric.com";
			email.To = review.reviewed_by.NEEmail;
			email.Subject = current_user.FullName + " has sent your review for " + review.member.FullName + " (" + review.member.business_unit_name + ") back into development.";
			email.isHTML = true;
			string body = "<font face='arial' size='2'>";

			body += "<b>Review:</b> <a href='https://www.nesi.ca/sections/hr/member/review_list_for_member.aspx?memberid=" + hdnmemberid.Value + "&id=" + hdnr_id.Value + "' target = 'review'>" + hdnr_id.Value + "</a><br/>";
			email.Body = body;
			email.Send();
		}
		catch { }
		_tools.getSQL_void(@"update emp_review set status = 'In Development'  where id =@v0", new object[] { hdnr_id.Value });
		lblstatus.Text = "In Development";

		setup_buttons();
	}
 */ 
	protected void cbnote_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
	//	ASPxMemo txtaddnote = (ASPxMemo)cbnote.FindControl("txtaddnote");
	//	ASPxMemo memnote = (ASPxMemo)cbnote.FindControl("memnote");
	//	memnote.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + current_user.FullName2 + System.Environment.NewLine + txtaddnote.Text + System.Environment.NewLine + System.Environment.NewLine + memnote.Text;

	//	_tools.getSQL_void(@"update emp_review  set notes =@v0  where id =@v0 limit 1 ", new object[] { memnote.Text,hdnr_id.Value });
	//	txtaddnote.Text = "";
	}
	
	
	protected void cb_grid_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var p = e.Parameter.Split('|');
		//	ASPxGridView gv = (ASPxGridView)sender;
		if (p.Length == 1)
		{

		}
		else
		{
			if (p[2] == "g")
			{

				_tools.getSQL_void(@"update emp_review_history 
set member_id=@v0,date = curdate(),score =@v1 where id =@v2 " ,
					new object[] {
current_user.id, p[1],p[0]
						}
);

			}
			else if (p[2] == "n")
			{
				_tools.getSQL_void(@"update emp_review_history 
set member_id=@v0,date = curdate(),notes = @v1 where id = @v2",
					new object[] {
						current_user.id,  p[1],p[0]
					}
				);
			}


		}
		//	fill_gv_reviewitem();
		//	fill_gv_reviewitem0();
	}
	protected void btn_refresh_Click(object sender, EventArgs e)
	{

	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		if (_tools.getSQL_int(@"select ifnull((Select count(id) from emp_review  where offerid =@v0),0) ", new object[] { ddloffers.Value }) == 1)
		{
			_tools.getSQL_void(@"delete from emp_review_history where
member_id = @v0 and emp_review_id =@v1 and cr_review_item_id is not null",
				new object[] {
					hdnmemberid.Value,hdnr_id.Value

					});
			

			var dt = _tools.getSQL_datatable(@"SELECT cr_review.cr_review_id,cr_review.cr_review_question FROM cr_review,memberoffer_cr,core_responsibilities  WHERE memberoffer_cr.memberoffer_crid = cr_review.cr_review_cr_id and core_responsibilities.id = memberoffer_cr.memberoffer_crid and core_responsibilities.`status` = 'Active' and memberoffer_cr.memberoffer_moid = ifnull((Select member_offers.id from member_offers where member_offers.`status`='Accepted' and member_offers.memberid =@v0 order by member_offers.id desc limit 1),0) and cr_review.cr_review_status = 'Active'", new object[] { hdnmemberid.Value });
			foreach (DataRow dr in dt.Rows)
			{
				_tools.getSQL_void(@"insert into emp_review_history 
(member_id, date, cr_review_item_id,emp_review_id,score,cr_review_question) 
values (@v0,curdate(),@v1,@v2,@v3,@v4)",
					new object[] { hdnmemberid.Value , dr["cr_review_id"] , hdnr_id.Value,0, dr["cr_review_question"]});
			}
		}
	}

	protected void ddlreviewed_DataBound(object sender, EventArgs e)
	{
		if (ddlreviewed.Items.FindByValue(current_user.id)==null)
		{
			if (NeMember.is_supervisor(Convert.ToInt32(hdnmemberid.Value), current_user.id))
			{
				ddlreviewed.Items.Add(current_user.FullName, current_user.id);
				//ddlreviewed.DataBind();
			}
		}

	}

	protected void mem_milestones_Init(object sender, EventArgs e)
	{
		var mem = sender as ASPxMemo;
		if (Convert.ToString(Session["reviewid_locked"]) == "0")
		{
			var container = mem.NamingContainer as GridViewDataItemTemplateContainer;
			mem.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_milestones.PerformCallback('{0}|' + s.GetText()+ '|n'); }}", container.KeyValue);
			mem.ID = "memmilestonesnotes" + container.KeyValue;
			mem.ClientInstanceName = mem.ID;
		}
		else
		{
			mem.Enabled = false;
		}
	}
	protected void milestone_complete_Init(object sender, EventArgs e)
	{
		var chk = sender as ASPxCheckBox;
		if (Convert.ToString(Session["reviewid_locked"]) == "0")
		{
			var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
			chk.ClientSideEvents.ValueChanged = string.Format(@"function (s, e) {{   gv_milestones.PerformCallback('{0}|' + s.GetValue()+ '|g'); }}", container.KeyValue);
		}
		else
		{
			chk.Enabled = false;
		}
	}
	protected void gv_milestones_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var p = e.Parameters.Split('|');
		//	ASPxGridView gv = (ASPxGridView)sender;
		if (p.Length == 1)
		{

		}
		else
		{
			if (p[2] == "g")
			{

				_tools.getSQL_void(@"update memberoffer_milestones set completed=@v0 where id =@v1", new object[] { p[1]  ,p[0]});

			}
			else if (p[2] == "n")
			{
				_tools.getSQL_void(@"update  memberoffer_milestones set notes =@v0 where id =@v1 ", new object[] { p[1], p[0]});
			}


		}
	}
	protected void btnclose_Click(object sender, EventArgs e)
	{

		new NeEmpReview().delete(Convert.ToInt32(lblid.Text));
		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "opener.location.href = opener.location.href; window.close();", true);

		
	}
}
	

	