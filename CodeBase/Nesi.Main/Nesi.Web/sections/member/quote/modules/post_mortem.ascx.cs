using System;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_member_quote_modules_post_mortem : System.Web.UI.UserControl
	{
	public int quote_id  
		{
		get { var _quote_id = 0; if (ViewState["quote_id"] == null) { return _quote_id; } else { int.TryParse(ViewState["quote_id"].ToString(), out _quote_id); return _quote_id; } } 
		set {ViewState["quote_id"] = value.ToString();}
		}
	

	
	private const int _page_id			= 65;
	NeMember current_user;
	
	Toolbox _tools			= new Toolbox();
	quote quote;
	NeQuoteSchedule qs;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user			= Toolbox.do_handle_authentication(Convert.ToInt32(_page_id));
		
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		
		quote = new quote(Convert.ToInt32(hdn_quote_id.Value));
		hdn_cid.Value = quote.business_unit_id.ToString();
	
		if (!IsPostBack)
		{
			if (hdn_quote_id.Value != "0")
			{
			
		
			}
		}
		if (quote_id != 0)
		{
			fill_page();
		}
	}

	
	protected void fill_page()
	{
		qs = new NeQuoteSchedule(quote.QuoteID.ToString());
		if (quote.status == 8) // if we won the quote
		{
			lbl_whokilled.Text = "This quote was Converted by " + new NeMember(Convert.ToInt32(qs.convert_or_kill_mid)).FullName;
			lbl_datekilled.Text = "This quote was won " + (qs.convert_or_kill_date_completed == null ? "Unknown" : _tools.getSQL_string(@"Select fun_time(@v0)",new object[] { Convert.ToDateTime(qs.convert_or_kill_date_completed).ToString("yyyy-MM-dd HH:mm:ss") } ));
		}
		else
		{
			lbl_whokilled.Text = "This quote was killed by " + new NeMember(Convert.ToInt32(qs.convert_or_kill_mid)).FullName;
			lbl_datekilled.Text = "This quote was killed " + (qs.convert_or_kill_date_completed == null ? "Unknown" : _tools.getSQL_string(@"Select fun_time(@v0)",new object[] { Convert.ToDateTime(qs.convert_or_kill_date_completed).ToString("yyyy-MM-dd HH:mm:ss") } ));
		}
		lbl_whykilled.Text = "Reason: " + quote.why_killed;
		
		if (quote.quote_kill_stage != null)
		{
			if (quote.quote_kill_stage == "0")
			{
				lblstage_killed.Text = "This quote was killed at stage 1 (before any time was really spent)";
				gv.BackColor = System.Drawing.Color.LightCyan;
				
			}
			if (quote.quote_kill_stage == "1")
			{
				lblstage_killed.Text = "This quote was killed at stage 4 (After recon work was done)";
				gv.BackColor = System.Drawing.Color.PaleGoldenrod;
				
			}
			if (quote.quote_kill_stage == "2")
			{
				lblstage_killed.Text = "This quote was rejected by the customer after the quote was delivered";
				gv.BackColor = System.Drawing.Color.Pink;
				
			}
			if (quote.quote_kill_stage == "4")
			{
				lblstage_killed.Text = "Hey we won this quote! Why the int face?";
				gv.BackColor = System.Drawing.Color.LightGreen;
			}
		}
		if ((current_user.id == qs.post_mortem_complete_mid)||(NeMember.is_supervisor((int) qs.post_mortem_complete_mid,current_user.id)||(qs.post_mortem_complete_mid==0)))
		{
			if (quote.status == 13)
			{
				btnclose.ClientVisible = true;
			}
		}
	}

	
	protected void mem_result_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var qs = new NeQuoteSchedule(quote_id.ToString());
		var quote = new quote(quote_id);
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv.PerformCallback('m|{0}|' + s.GetValue()); }}", container.KeyValue);
		if ((current_user.id == qs.post_mortem_complete_mid) || (NeMember.is_supervisor((int) qs.post_mortem_complete_mid, current_user.id) || (qs.post_mortem_complete_mid == 0)))
		{
			if (quote.status == 13)
			{
				ddl.ClientEnabled = true;
			}
			
		}
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{

		if (e.Parameters[0].ToString() == "m")
		{
			var id = e.Parameters.Split('|').GetValue(1).ToString();
			var value = e.Parameters.Split('|').GetValue(2).ToString();


			try
			{
				_tools.getSQL_void(@"update quote_post_mortem set result = @v0 where id = @v1", new object[] {
						 value, id});
			}
			catch { }
		}
		

		fill_page();
	}
	
	protected void cb_Callback(object sender, CallbackEventArgsBase e)
	{
		var x = _tools.getSQL_int(@"Select count(id) from quote_post_mortem  where quote_id =@v0 and (ifnull(result,'') = '')", new object[] { quote.QuoteID });
		if (x > 0)
		{
			throw new Exception("You haven't answered all the questions");
		}
		else
		{
			
			_tools.getSQL_void(@"update quote_master set status_id = 6, 
killed_by=@v0 ,killed_date=curdate() where quote_id = @v1  and active_revision = 1", new object[] {
current_user.id,quote_id
			});
			cb.JSProperties["cp_close"] = "close";
			var qs = new NeQuoteSchedule(quote_id.ToString());
			qs.post_mortem_complete_date_completed = System.DateTime.Now;
			qs.save();
			var comp = new NeBusinessUnit(quote.business_unit_id);
			var email = new NeEMail();



			email.Subject = "Post Mortem Completed for Quote " + quote.QuoteID + " for " + quote.txtCustomerName;
			email.To = new NeMember(Convert.ToInt32(quote.quoted_by)).NEEmail + ";";
			email.CC = new NeMember(Convert.ToInt32(current_user.reports_to)).NEEmail;
			email.isHTML = true;
			email.Body = "<table style='width:900px; font-size:11px; font-family: Arial; border-collapse: collapse;' cellpadding='5'>";
			email.Body += "<tr bgcolor='DarkBlue' color='White'><td>Question</td><td>Answer</td><td>Answered by</td></tr>";
			var bgcolor = "LightGrey";

			#region set up post mortem questions
			var dt = _tools.getSQL_datatable(@"Select *,get_name(quote_post_mortem.memberid) _name from quote_post_mortem,quote_post_mortem_questions  where quote_post_mortem.question_id=quote_post_mortem_questions.id and quote_id =@v0", new object[] { quote_id });
			foreach (DataRow dr in dt.Rows)
			{
				email.Body += "<tr bgcolor='" + bgcolor + "' valign='top'><td>" + dr["question"] + "</td><td>" + dr["result"] + "</td><td>" + dr["_name"] + "</td></tr>";
				if (bgcolor == "LightGrey")
				{
					bgcolor = "White";
				}
				else
				{
					bgcolor = "LightGrey";
				}
			}
			email.Body += "</table>";
			#endregion
			if (qs.id > 0)
			{
				email.To += new NeMember(Convert.ToInt32(qs.rt1)).NEEmail + ";";
				email.To += new NeMember(Convert.ToInt32(qs.rt2)).NEEmail + ";";
				email.To += new NeMember(Convert.ToInt32(qs.rt3)).NEEmail + ";";
				email.To += new NeMember(Convert.ToInt32(qs.rt4)).NEEmail + ";";
			}
			email.Send();

			
		}
	}
}