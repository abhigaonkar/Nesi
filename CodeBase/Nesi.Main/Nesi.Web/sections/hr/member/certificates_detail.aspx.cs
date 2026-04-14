using System;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_hr_member_certificates_detail : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 154;
	private bool can_edit = false;
	int certid = 0;
	Toolbox _tools;
	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);


		//	gvcert.DataBind();
	}
	protected void Page_Load(object sender, EventArgs e)
	{


		var _q = Request.QueryString;
		if (!IsPostBack)
		{
			certid = 0;
		}

		certid = string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);

		hdnqid.Value = certid.ToString();
		hdnid.Value = certid.ToString();


		if (!IsPostBack)
		{
			Session["cert_history"] = null;
			Session["gv_history_id"] = null;
			if (certid != 0)
			{
				populate_newform();
			}
			else
			{

			}


		}



		fill_history();

	}
	protected void populate_newform()
	{

		if (certid > 0)  // if you're editing
		{
			lblid.Text = certid.ToString();
			hdnid.Value = lblid.Text;
			var dt = _tools.getSQL_datatable(@"Select * from certificates  where id =@v0", new object[] { certid });
			txtname.Text = dt.Rows[0]["certificate_name"].ToString();
			txtexpiry.Text = dt.Rows[0]["expires"].ToString();
			txtnotes.Text = dt.Rows[0]["notes"].ToString();
			ddlstatus.Value = dt.Rows[0]["status"].ToString();
			txthow_to_acquire.Text = dt.Rows[0]["how_to_acquire"].ToString();
			chk_isinternal.Value = Convert.ToInt32(dt.Rows[0]["is_internal"].ToString());
			ASPxPageControl1.TabPages[1].ClientEnabled = true;
			ASPxPageControl1.TabPages[2].ClientEnabled = true;
			btnsave.Text = "Save";

			fill_history();
		}
		else
		{
			ASPxPageControl1.TabPages[1].ClientEnabled = false;
			ASPxPageControl1.TabPages[2].ClientEnabled = false;
			txtname.Text = "";
			txtexpiry.Text = "";
			txtnotes.Text = "";
			txthow_to_acquire.Text = "";

		}

	}

	protected void fill_history()
	{
		if (Session["cert_history"] == null)
		{
			Session["cert_history"] = _tools.getSQL_datatable(@"SELECT certificate_history.id, certificate_history.certificate_id, certificate_history.member_id, certificate_history.business_unit_id, certificate_history.date, certificate_history.next_date, certificate_history.notes, certificate_history.external_id, certificate_history.score FROM certificate_history  where certificate_id=@v0", new object[] { certid });
		}
		gv_history.DataSource = Session["cert_history"];
		gv_history.DataBind();

	}


	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}

		if (txtexpiry.Text == "")
		{
			txtexpiry.Text = "99999";
		}

		if (certid == 0)
		{
			_tools.getSQL_void(@"Insert into certificates 
(certificate_name,notes,is_internal,expires,status,how_to_acquire) 
values(@v0,@v1,@v2,@v3,@v4,@v5)", new object[] {
				 txtname.Text,
			txtnotes.Text ,
			Convert.ToInt32(chk_isinternal.Checked),
			Convert.ToInt32(txtexpiry.Text) , "Active",
			txthow_to_acquire.Text});

			lblid.Text = _tools.getSQL_int(@"Select last_insert_id() from certificates"  , null).ToString();
			ASPxWebControl.RedirectOnCallback("certificates_detail.aspx?id=" + lblid.Text);

			certid = Convert.ToInt32(lblid.Text);
			ASPxPageControl1.TabPages[1].ClientEnabled = true;
			ASPxPageControl1.TabPages[2].ClientEnabled = true;


		}
		else
		{
			_tools.getSQL_void(@"Update certificates 
set certificate_name=@v0, notes=@v1,is_internal=@v2, expires = @v3,status=@v4,how_to_acquire=@v5 where id =@v6 ", new object[]
				{txtname.Text,txtnotes.Text,Convert.ToInt32(chk_isinternal.Checked),Convert.ToInt32(txtexpiry.Text),ddlstatus.Value,txthow_to_acquire.Text,certid
				});

		}
	}

	protected void ASPxComboBox2_Init1(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_editq.PerformCallback('{0}|' + s.GetValue()+ '|p'); }}", container.KeyValue);

	}
	protected void ASPxTextBox2_Init(object sender, EventArgs e)
	{
		var txtq = sender as ASPxTextBox;
		var container = txtq.NamingContainer as GridViewDataItemTemplateContainer;
		txtq.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_editq.PerformCallback('{0}|' + s.GetText()+ '|c'); }}", container.KeyValue);

	}
	protected void gv_editq_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		if (e.Parameters == "a")
		{
			_tools = new Toolbox();
			var gv = (ASPxGridView)sender;
			var ddlprov = (ASPxComboBox)gv.FindTitleTemplateControl("ddl_prov");
			var addcred = (ASPxTextBox)gv.FindTitleTemplateControl("addcred");
			_tools.getSQL_void(@"insert into certificates_credential_link 
(credential,certificate_id,state_province) 
values (@v0,@v1,@v2)", new object[] {
				addcred.Text, hdnqid.Value, ddlprov.Value});
			gv.DataBind();
		}
		else
		{
			var p = e.Parameters.Split('|');
			try
			{
				if (p[2] == "c")
				{
					_tools.getSQL_void(@"update certificates_credential_link 
set credential = @v0  where id =@v1 ", new object[] {
						p[1], p[0]});
				}
				else if (p[2] == "p")
				{
					_tools.getSQL_void(@"update certificates 
set state_province = @v0 where id =@v1", new object[] {
					p[1], p[0]});
				}
			}
			catch { }
		}

	}
	protected void gv_editq_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var gv = (ASPxGridView)sender;
		_tools.getSQL_void(@"Delete from certificates_credential_link where id =@v0 limit 1", new object[] {
			e.Keys[0]});
		e.Cancel = true;
		gv.CancelEdit();
	}
	protected void gv_history_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		Session["cert_history"] = null;
		fill_history();

	}
	protected void gv_history_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{

	}
	protected void ddlmember_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var cb = (ASPxComboBox)sender;
		cb.DataBind();
	}
	protected void gv_history_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var cb = (ASPxCallbackPanel)gv_history.FindEditFormTemplateControl("cb0");
		var ddlmember = (ASPxComboBox)cb.FindControl("ddlmember");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");

		var txtscore = (ASPxTextBox)cb.FindControl("txtscore");
		var dte_date = (ASPxDateEdit)cb.FindControl("dte_date");
		var memnotes = (ASPxMemo)cb.FindControl("memnotes");
		if (!gv_history.IsNewRowEditing)
		{
			Session["gv_history_id"] = gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "id").ToString();
			ddlmember.Value = Convert.ToInt32(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "member_id"));
			ddlcompany.Value = Convert.ToInt32(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "business_unit_id"));

			txtscore.Text = gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "score").ToString();
			memnotes.Text = gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "notes").ToString();
			dte_date.Date = Convert.ToDateTime(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "date"));
			//dte_nextdate.Date = Convert.ToDateTime(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "next_date"));
		}
		else
		{
			Session["gv_history_id"] = null;

		}
	}
	protected void gv_history_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		Session["gv_history_id"] = null;
	}
	protected void gv_history_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		_tools.getSQL_void(@"delete from certificate_history where id =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		gv_history.CancelEdit();
		Session["cert_history"] = null;
		fill_history();
	}
	protected void gv_history_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		if (gv_history.IsEditing)
		{
			Session["gv_history_id"] = e.EditingKeyValue.ToString();
		}
		else
		{
			Session["gv_history_id"] = null;
		}
	}
	protected void cb_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}

		var cb = (ASPxCallbackPanel)sender;
		var ddlmember = (ASPxComboBox)cb.FindControl("ddlmember");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");


		var txtscore = (ASPxTextBox)cb.FindControl("txtscore");
		var dte_date = (ASPxDateEdit)cb.FindControl("dte_date");

		var memnotes = (ASPxMemo)cb.FindControl("memnotes");

		if (dte_date.Text == "")
		{
			throw new Exception("You must select a valid date");
		}

		if (ddlmember.Text == "")
		{
			throw new Exception("You must select a valid member");
		}
		if (ddlcompany.Text == "")
		{
			throw new Exception("You must select a valid branch");
		}

        //TODO: LL FIXING SQL Security

        if (Session["gv_history_id"] == null)
		{
			_tools.getSQL_void(@"Insert into certificate_history 
(date,certificate_id,member_id,business_unit_id,notes,score)
values (@v0,@v1,@v2,@v3,@v4,@v5)", new object[] {  dte_date.Date.ToString("yyyy-MM-dd") , hdnid.Value, ddlmember.Value ,
			ddlcompany.Value ,memnotes.Text,txtscore.Text});
		}
		else
		{
			_tools.getSQL_void(@"update certificate_history 
set date =@v0,member_id=@v1,business_unit_id =@v2,score=@v3,notes =@v4 where certificate_history.id =@v5 limit 1",
				new object[]
					{
					dte_date.Date.ToString("yyyy-MM-dd"), ddlmember.Value, ddlcompany.Value,txtscore.Text,memnotes.Text, Session["gv_history_id"]

					});
		}

	}
	protected void gv_q_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var ddl_skills0 = (ASPxComboBox)gv_q.FindTitleTemplateControl("ddl_skills0");
		if (_tools.getSQL_int(@"select count(id) from certificate_training_link  where training_header_id=@v0 and certificate_id =@v1 ", new object[] { ddl_skills0.Value,certid }) == 0)
		{
			_tools.getSQL_void(@"Insert into certificate_training_link (training_header_id,certificate_id) 
values (@v0,@v1)", new object[] {
				ddl_skills0.Value , certid });
			ddl_skills0.Text = "";
			gv_q.DataBind();
		}
	}
	protected void gv_q_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		_tools.getSQL_void("delete from certificate_training_link where id =@v0 limit 1", new object[] { e.Keys["id"] });
		e.Cancel = true;
		gv_q.CancelEdit();
		gv_q.DataBind();
	}
	protected void gv_certs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var ddlcerts = (ASPxComboBox)gv_certs.FindTitleTemplateControl("ddlcerts");
		if ((ddlcerts.Value != null) && (hdnid.Value != ""))
		{
			if (_tools.getSQL_int(@"Select count(id) from cr_certificates  where cr_id =@v0 and certificates_id =@v1 ", new object[] { ddlcerts.Value,hdnid.Value }) == 0)
			{
				_tools.getSQL_void(@"Insert into cr_certificates (cr_id,certificates_id)
values (@v0,@v1)", new object[] {
					 ddlcerts.Value , hdnid.Value });
				gv_certs.DataBind();
			}

		}
	}
	protected void gv_certs_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{

		_tools.getSQL_void(@"delete from cr_certificates where id =@v0", new object[] { e.Keys[0] });

		e.Cancel = true;
		gv_certs.CancelEdit();
		gv_certs.DataBind();
	}



}



