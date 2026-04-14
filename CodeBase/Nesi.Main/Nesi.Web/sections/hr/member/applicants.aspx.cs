using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class member_applicants : Page
	{
	NeMember current_user;
	private const int _page_id = 138; // from Page table in DB
	private bool auth_for_edit = false;
	private bool auth_for_edit_all = false;
	private const string _page_name = "Applicants";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{
		Toolbox _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		NeMenu menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		layout.__page_name					= _page_name;
        layout.used_gv = gv_applicants;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;

		    sqlcompany.SelectCommand =
		        "Select id , ddl_name name from business_unit  where active = 'T'  and id in (" +
		        new Current_User().visible_business_units + ") order by ddl_name";
        //		frame.Attributes["src"]			= String.Format("./index.aspx");
        Label lbltemp = (Label)Page.Master.FindControl("lblHeading");
		if (!IsPostBack)
			{
			gv_applicants.FilterExpression = " [business_unit_id] = " + Convert.ToInt32(current_user.business_unit_id);
			Session["HR_applicant_edit"] = null;
			gv_applicants.JSProperties["cp_firstname"] = "";
			gv_applicants.JSProperties["cp_lastname"] = "";
			}
		// Hide the action column
		gv_applicants.Settings.ShowTitlePanel = current_user.AuthenticatedForPrivilege(5);
		auth_for_edit = current_user.AuthenticatedForPrivilege(32);
		auth_for_edit_all = current_user.AuthenticatedForPrivilege(152);

		h.Set("gridview_id", "gv_applicants");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		if (!gv_applicants.IsEditing)
		{
			fill_grid();
		}
//		gv_applicants.DataBind();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if (!IsPostBack)
			{
			NeGridLayouts gl	= new NeGridLayouts(Convert.ToInt32(current_user.id), _page_name);
			if(gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout		= gv_applicants.SaveClientLayout();
				gl.member_id	= Convert.ToInt32(current_user.id);
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_applicants.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			Session["HR_applicant_grid"] = null;
			Session["HR_applicant_edit"] = null;
			Session["HR_applicant_offers"] = null;
			
			hdnid.Value = null;
			
			}

		}
	protected void fill_grid()
	{
	
		bool chk_value = chk_all.Checked;
		if (auth_for_edit_all)
		{
			if (Session["HR_applicant_grid"] == null)
			{

				DataTable dt = _tools.getSQL_datatable(@"SELECT DISTINCT 
applicants.id,
applicants.addedbymemberid,
concat(applicants.firstname,' ',applicants.lastname) AS _name,
applicants.dateentered,
applicants.`status`,
applicants.membertypeid,
applicants.business_unit_id,

applicants.cellphone,
applicants.email,
ifnull((Select `status` from member_offers where member_offers.applicantid = applicants.id order by member_offers.id desc limit 1),'Unknown') offer_status,
applicants.notes
FROM
applicants where find_in_set(applicants.business_unit_id, @v0) " + (chk_all.Checked ? "" : "  and `status` !='Hired' and `status` !='Deleted' and `status` !='Rejected'") + @"
group by applicants.id
order by id 
",new object [] { new Current_User().visible_business_units });

				Session["HR_applicant_grid"] = dt;
			}
		}
		else if (current_user.AuthenticatedForPrivilege(33))
		{
			if (Session["HR_applicant_grid"] == null)
			{

				DataTable dt = _tools.getSQL_datatable(@"SELECT DISTINCT 
applicants.id id,
applicants.addedbymemberid,
concat(applicants.firstname,' ',applicants.lastname) AS _name,
applicants.dateentered,
applicants.`status`,
applicants.membertypeid,
applicants.business_unit_id,

applicants.cellphone,
applicants.email,
ifnull((Select `status` from member_offers where member_offers.applicantid = applicants.id order by member_offers.id desc limit 1),'Unknown') offer_status,
applicants.notes
FROM 
applicants where applicants.business_unit_id='" + current_user.business_unit_id + @"' OR applicants.addedbymemberid='" + current_user.id + @"'  
" + (chk_all.Checked ? "" : " and (`status` !='Hired' and `status` !='Deleted' and `status` !='Rejected')") + @" 
group by id 
order by id 
",null);


				Session["HR_applicant_grid"] = dt;
			}
		}
		else
		{

			if (Session["HR_applicant_grid"] == null)
			{

				DataTable dt = _tools.getSQL_datatable(@"SELECT DISTINCT 
applicants.id id,
applicants.addedbymemberid,
concat(applicants.firstname,' ',applicants.lastname) AS _name,
applicants.dateentered,
applicants.`status`,
applicants.membertypeid,
applicants.business_unit_id,

applicants.cellphone,
applicants.email,
ifnull((Select `status` from member_offers where member_offers.applicantid = applicants.id order by member_offers.id desc limit 1),'Unknown') offer_status,
applicants.notes
FROM 
applicants where applicants.addedbymemberid='" + current_user.id + @"'  
" + (chk_all.Checked ? "" : " and (`status` !='Hired' and `status` !='Deleted' and `status` !='Rejected')") + @" 
group by id 
order by id 
",null);


				Session["HR_applicant_grid"] = dt;
			}
		}
			gv_applicants.DataSource = Session["HR_applicant_grid"];
			gv_applicants.DataBind();
			
	}

	protected void fill_offer_grid()
	{
		if (Session["HR_applicant_offers"] == null)
		{
			Session["HR_applicant_offers"] = _tools.getSQL_datatable(@"SELECT a.id, a.date, a.memberid, a.enteredby, a.business_unit_id, a.wage, a.startdate, a.enddate, a.vacation_interval_1, a.vacation_interval_2, a.vacation_interval_3, a.vacation_amount_1, a.vacation_amount_2, a.vacation_amount_3, a.gets_vehicle, a.gets_phone, a.gets_laptop, a.has_comp, a.comp_details, a.notes, a.is_salary, a.is_signed, a.`status`, c.member_fullname reports_to, b.membertype_name membertypeid, a.isapplicant FROM member_offers a INNER JOIN membertype AS b ON a.membertypeid = b.membertype_id LEFT JOIN member c ON a.reports_to = c.member_id  WHERE a.applicantid =@v0", new object[] { Session["HR_applicant_edit"] });

		}

		ASPxGridView gv_offers = (ASPxGridView)gv_applicants.FindEditFormTemplateControl("gv_offers");
		gv_offers.DataSource = Session["HR_applicant_offers"];
		gv_offers.DataBind();

	}
	
	protected void gv_applicants_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!gv_applicants.IsNewRowEditing && gv_applicants.EditingRowVisibleIndex > -1)
		{
			hdnid.Value = gv_applicants.GetRowValues(gv_applicants.EditingRowVisibleIndex, new String[] { "id" }).ToString();
		}
        if (e.Parameters.Contains("|u"))
        {

            string new_note = e.Parameters.Split('|').GetValue(1).ToString();

            _tools.getSQL_void(@"update applicants  set notes=@v0  where id =@v1 limit 1 ", new object[] { new_note,e.Parameters.Split('|').GetValue(0).ToString() });
            Session["HR_applicant_grid"] = null;
            fill_grid();
            return;
        }
        if (e.Parameters.Contains("|n"))
        {
            int id = Convert.ToInt32(e.Parameters.Split('|').GetValue(0));
            string old_note = gv_applicants.GetRowValues(Convert.ToInt32(gv_applicants.FindVisibleIndexByKeyValue(id)), "notes").ToString();
            string new_note = System.DateTime.Today.ToString("yyyy-MM-dd") + "-" + current_user.FullName + " - " + e.Parameters.Split('|').GetValue(1) + System.Environment.NewLine + old_note;

            _tools.getSQL_void(@"update applicants  set notes=@v0  where id =@v1 limit 1 ", new object[] { new_note,e.Parameters.Split('|').GetValue(0).ToString() });
            Session["HR_applicant_grid"] = null;
            fill_grid();
            return;
        }
        else if (e.Parameters.Contains("|s"))
        {
            _tools.getSQL_void(@"update applicants  set status=@v0  where id =@v1 limit 1 ", new object[] { e.Parameters.Split('|').GetValue(1).ToString(),
				e.Parameters.Split('|').GetValue(0).ToString() });
            Session["HR_applicant_grid"] = null;
            fill_grid();
            return;
        }

		else if (e.Parameters.Contains("AddApp"))
		{
		
			
		
			gv_applicants.AddNewRow();
			gv_applicants.JSProperties["cp_hr_warning"] = e.Parameters.Split('|').GetValue(1).ToString();
			
		}
		else if (e.Parameters == "saveApp")
		{

			save_app();
		}
		else if (e.Parameters.Contains("open_app"))
		{
			string key = e.Parameters.Split('|').GetValue(1).ToString();
			Session["HR_applicant_grid"] = null;
			fill_grid();
			gv_applicants.FilterExpression = "id = " + key;
			gv_applicants.DataBind();
		}
		else
		{
			ASPxGridView gv = (ASPxGridView)sender;
			if (e.Parameters != "")
			{
				gv.LoadClientLayout(e.Parameters);
			}
			else
			{
				gv.FilterExpression = "";
				for (int i = 0; i < gv.Columns.Count; i++)
				{
					if (gv.Columns[i] is GridViewDataColumn)
					{
						GridViewDataColumn col = (GridViewDataColumn)gv.Columns[i];
						if (col.GroupIndex > -1)
						{
							gv.UnGroup(col);
						}
						col.Visible = true;
					}
				}
			}
		}
	}
	protected void gv_applicants_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{

		Object value1 = "0";
		ASPxGridView cb = (ASPxGridView)sender;
		HtmlContainerControl frame = (HtmlContainerControl)cb.FindEditFormTemplateControl("div_files");
		
		HiddenField hdnid1 = (HiddenField)cb.FindEditFormTemplateControl("hdnid1");
		ASPxButton btnsave = (ASPxButton)cb.FindEditFormTemplateControl("btnsave");
		if (!cb.IsNewRowEditing)  // if we are editing
		{
			int rowIndex = cb.EditingRowVisibleIndex;
			
			value1 = cb.GetRowValues(rowIndex, new String[] { "id" });
			if(value1 != null)
				{
			if (Session["HR_applicant_edit"]==null || ((value1.ToString() != Session["HR_applicant_edit"].ToString())))
			{
				Session["HR_applicant_offers"] = null;
				
			}

			Session["HR_applicant_edit"] = value1.ToString();
		
			btnsave.Text = "Save and Close";
hdnid.Value = value1.ToString();

		#region populate file tab
		frame.Attributes.Add("src", "/filemanager.aspx?parent_page=applicant_files&id=" + value1);
		frame.Attributes.Add("onload", "resizeIframe(this);");
		ASPxGridView g = (ASPxGridView)cb.FindEditFormTemplateControl("gv_offers");
		g.ClientVisible = true;
		HtmlControl	tr_app_files = (HtmlControl	)cb.FindEditFormTemplateControl("tr_app_files");
		tr_app_files.Visible = true;
		fill_offer_grid();

		NeApplicant a = new NeApplicant(Convert.ToInt32(value1));
		ASPxTextBox txtfirstname = (ASPxTextBox)cb.FindEditFormTemplateControl("txtfirstname");
		ASPxTextBox txtlastname = (ASPxTextBox)cb.FindEditFormTemplateControl("txtlastname");
		ASPxComboBox ddlcompany = (ASPxComboBox)cb.FindEditFormTemplateControl("ddlcompany");
		ASPxComboBox ddlmembertype = (ASPxComboBox)cb.FindEditFormTemplateControl("ddlmembertype");
		ASPxComboBox ddlstatus = (ASPxComboBox)cb.FindEditFormTemplateControl("ddlstatus");
		ASPxTextBox txtnotes = (ASPxTextBox)cb.FindEditFormTemplateControl("txtnotes");
		ASPxComboBox ddldept = (ASPxComboBox)cb.FindEditFormTemplateControl("ddldept");
		ASPxTextBox txtaddress = (ASPxTextBox)cb.FindEditFormTemplateControl("txtaddress");
		ASPxTextBox txtcity = (ASPxTextBox)cb.FindEditFormTemplateControl("txtcity");
		ASPxComboBox ddlprovince = (ASPxComboBox)cb.FindEditFormTemplateControl("ddlprovince");
		ASPxComboBox ddlcountry = (ASPxComboBox)cb.FindEditFormTemplateControl("ddlcountry");
		ASPxTextBox txtzip = (ASPxTextBox)cb.FindEditFormTemplateControl("txtzip");
		ASPxTextBox txtcell = (ASPxTextBox)cb.FindEditFormTemplateControl("txtcell");
		ASPxTextBox txtemail = (ASPxTextBox)cb.FindEditFormTemplateControl("txtemail");
		ASPxTextBox txthomephone = (ASPxTextBox)cb.FindEditFormTemplateControl("txthomephone");
		ASPxTextBox txtapt = (ASPxTextBox)cb.FindEditFormTemplateControl("txtapt");
		ASPxLabel lblid = (ASPxLabel)cb.FindEditFormTemplateControl("lblid");
		lblid.Text = a.id.ToString();
		txtfirstname.Text = a.firstname;
		txtlastname.Text = a.lastname;
		ddlcompany.Value = a.business_unit_id;
		
		ddlmembertype.Value = a.membertypeid;
		if (a.status == "Hired")
		{
			if (ddlstatus.Items.FindByText("Hired")==null)
			{
				ListEditItem li99 = new ListEditItem("Hired", "Hired");
				ddlstatus.Items.Add(li99);
			}
		}
		ddlstatus.Value = a.status;
		txtnotes.Text = _tools.value_from(a.notes);
		hdnid.Value = value1.ToString();
		hdnid1.Value = value1.ToString();
//		btnsave.Text = "Save and Close";
		txtaddress.Text = a.address;
		txtcity.Text = a.city;
		ddlprovince.Value = a.province;
		ddlcountry.Value = a.country;
		txtzip.Text = a.postal;
		txtcell.Text = a.cellphone;
		txtemail.Text = a.email;
		txthomephone.Text = a.homephone;
		txtapt.Text = a.apt;
		#endregion
				}
		}
		else
		{
		
			Session["HR_applicant_edit"] = null;
			hdnid.Value = "";
			hdnid1.Value = "";
			
		}
	}
	protected void gv_applicants_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		ASPxGridView gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		e.Properties["cp_warn_hr"] = null;
	}

	protected void gv_applicants_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		if (!gv_applicants.IsNewRowEditing)
		{
			gv_applicants.SettingsText.PopupEditFormCaption = "Editing " + gv_applicants.GetRowValuesByKeyValue(e.EditingKeyValue, "_name");
		}
		else
		{
			ASPxTextBox tb_firstname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("name_first");
			ASPxTextBox tb_lastname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("name_last");
			tb_firstname.Text = gv_applicants.JSProperties["cp_firstname"].ToString();
			tb_lastname.Text = gv_applicants.JSProperties["cp_lastname"].ToString();
			tb_firstname.ClientEnabled = false;
			tb_lastname.ClientEnabled = false;

			

		}
	}
	protected void gv_offers_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		ASPxGridView gv_offers = (ASPxGridView)sender;
	//	gv_offers.CancelEdit();
	//	e.Cancel = true;
		Session["HR_applicant_offers"] = null;
		fill_offer_grid();


	}
	protected void gv_offers_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		ASPxGridView gv_offers = (ASPxGridView)sender;
		if (e.Values["is_signed"].ToString() == "1")
		{
			throw new Exception("You can not delete offers that have already been signed");
		}
		_tools.getSQL_void(@"Delete from member_offers  where id =@v0", new object[] { e.Keys[0] });
		_tools.getSQL_void(@"Delete from memberoffer_milestones  where offerid =@v0", new object[] { e.Keys[0] });
		gv_offers.CancelEdit();
		e.Cancel = true;
		Session["HR_applicant_offers"] = null;
		fill_offer_grid();

	}
	protected void gv_offers_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{
	
		ASPxGridView gv = (ASPxGridView)sender;
		HiddenField hdnid1 = (HiddenField)gv_applicants.FindEditFormTemplateControl("hdnid1");
	
		HtmlContainerControl frame = (HtmlContainerControl)gv.FindEditFormTemplateControl("IFrame_Offer");
//		ASPxTextBox txtfirstname = (ASPxTextBox)gv.FindEditFormTemplateControl("txtfirstname");
//		ASPxTextBox txtlastname = (ASPxTextBox)gv.FindEditFormTemplateControl("txtlastname");
//		ASPxComboBox ddlcompany = (ASPxComboBox)gv.FindEditFormTemplateControl("ddlcompany");
//		ASPxComboBox ddlmembertype = (ASPxComboBox)gv.FindEditFormTemplateControl("ddlmembertype");
//		ASPxComboBox ddlstatus = (ASPxComboBox)gv.FindEditFormTemplateControl("ddlstatus");
//		ASPxTextBox txtnotes = (ASPxTextBox)gv.FindEditFormTemplateControl("txtnotes");
		
		
		if (!gv.IsNewRowEditing)  // are we editting here?
		{
			Session["offerid"] = null;
			
			int rowIndex = gv.EditingRowVisibleIndex;
			Object value1 = gv.GetRowValues(rowIndex, new String[] { "id" });
			if (value1 == null)
			{
				gv.CancelEdit();
				return;
			}
			else
			{
				frame.Attributes.Add("src", "member_offer.aspx?id=" + value1 + "&applicantid=" + hdnid1.Value + "&isapplicant=1");
				frame.Attributes.Add("onload", "resizeIframe(this);");
			}
			


		}
		else
		{
			
			Session["offerid"] = null;
			frame.Attributes.Add("src", "member_offer.aspx?applicantid=" + hdnid1.Value + "&isapplicant=1");
			frame.Attributes.Add("onload", "resizeIframe(this);");
//			ddlcompany.DataBind();
//			ddlcompany.Value = Convert.ToInt32(current_user.CompanyID);
//			ddlstatus.Value = "Active";

			
		}
		
		//	frame.Attributes.Add("onload", "resizeIframe(this);");
	}
	
	void save_app()
	{



		HiddenField hdnid1 = (HiddenField)gv_applicants.FindEditFormTemplateControl("hdnid1");
		ASPxGridView g = (ASPxGridView)gv_applicants.FindEditFormTemplateControl("gv_offers");
		HtmlContainerControl div_files = (HtmlContainerControl)gv_applicants.FindEditFormTemplateControl("div_files");
		ASPxTextBox txtfirstname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtfirstname");
		ASPxTextBox txtlastname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtlastname");
		ASPxComboBox ddlcompany = (ASPxComboBox)gv_applicants.FindEditFormTemplateControl("ddlcompany");
		ASPxComboBox ddlmembertype = (ASPxComboBox)gv_applicants.FindEditFormTemplateControl("ddlmembertype");
		ASPxComboBox ddlstatus = (ASPxComboBox)gv_applicants.FindEditFormTemplateControl("ddlstatus");
		ASPxTextBox txtnotes = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtnotes");
		
		ASPxButton btnsave = (ASPxButton)gv_applicants.FindEditFormTemplateControl("btnsave");
		ASPxTextBox txtaddress = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtaddress");
		ASPxTextBox txtcity = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtcity");
		ASPxComboBox ddlprovince = (ASPxComboBox)gv_applicants.FindEditFormTemplateControl("ddlprovince");
		ASPxComboBox ddlcountry = (ASPxComboBox)gv_applicants.FindEditFormTemplateControl("ddlcountry");
		ASPxTextBox txtzip = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtzip");
		ASPxTextBox txtcell = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtcell");
		ASPxTextBox txtemail = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtemail");
		ASPxTextBox txthomephone = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txthomephone");
		ASPxTextBox txtapt = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtapt");
		ASPxLabel lblid = (ASPxLabel)gv_applicants.FindEditFormTemplateControl("lblid");

		if (txtfirstname.Text == "")
		{
			throw new Exception("You must enter a first name");
		}
		if (txtlastname.Text == "")
		{
			throw new Exception("You must enter a last name");
		}
		if (ddlmembertype.Text == "")
		{
			throw new Exception("You must select a membertype");
		}
		if (ddlcompany.Text == "")
		{
			throw new Exception("You must select a business unit");
		}
	
        if (txtemail.Text == "")
        {
            throw new Exception("You must enter an email address");
        }
		if (!Toolbox.CheckEmail(txtemail.Text))
	        {
	        throw new Exception("You must enter a valid email address");
	        }
		if (ddlstatus.Text == "")
		{
			ddlstatus.Text = "New";
		}
		NeApplicant app;
		if (hdnid.Value == "")
		{
			app = new NeApplicant();
		}
		else
		{
			app = new NeApplicant(Convert.ToInt32(hdnid.Value));
		}
		app.firstname = txtfirstname.Text;
		app.lastname = txtlastname.Text;
		app.business_unit_id = Convert.ToInt32(ddlcompany.Value);
		
		app.membertypeid = Convert.ToInt32(ddlmembertype.Value);
		app.notes =txtnotes.Text;
		if (app.status != ddlstatus.Text)
		{
			if (ddlstatus.Text.Equals("Hired"))  // status is being manually to hired
			{
				if (app.becomes_memberid==0)
				{
					ddlstatus.Text = app.status;
					throw new Exception("You cannot manually set the status to HIRED unless they are already an employee in the employee table.  To convert them to an employee, you must an employment agreement and press the 'They Accepted' button.");
				}
			}
			if (app.status != "Hired")
			{
				app.status = ddlstatus.Text;
				
			}
			else
			{

			}
		}
		app.address = txtaddress.Text;
		app.city = txtcity.Text;
		app.province = ddlprovince.Value == null ? "": ddlprovince.Value.ToString();
		app.country = ddlcountry.Value == null ? "" : ddlcountry.Value.ToString();
		app.postal = txtzip.Text.Trim().Replace(" ","").ToUpper();
		app.cellphone = txtcell.Text;
		app.email = txtemail.Text;
		app.homephone = txthomephone.Text;
		app.apt = txtapt.Text;
		app.addedbymemberid = current_user.id32;
		
		if ((Session["HR_applicant_edit"] == null)||(app.dateentered.Year==1))
		{
			app.dateentered = System.DateTime.Today;
		}
		app.save();
		if (lbl_warning.Text=="dup_app")
		{
			shared.alert_payroll(string.Format(@"DUPLICATE APPLICANT WARNING - ""{0} {1}"" is being created by ""{2}"" - First & last Name matched", txtfirstname.Text, txtlastname.Text, current_user.FullName), "This is most likely a direct duplicate applicant IN THE MAKING... though it is a remote possiblity that there is a person with the same name as an existing applicant.");
		}
		else if (lbl_warning.Text=="dup_mem")
		{
			shared.alert_payroll(string.Format(@"DUPLICATE EMPLOYEE WARNING - ""{0} {1}"" is being created by ""{2}"" - First & last Name matched", txtfirstname.Text, txtlastname.Text, current_user.FullName), "This is most likely a direct duplicate employee IN THE MAKING... though it is a remote possiblity that there is a person with the same name as an existing employee.");
		}
		lblid.Text = app.id.ToString();

		if (Session["HR_applicant_edit"] == null)
		{
			Session["HR_applicant_grid"] = null;
			Session["HR_applicant_offers"] = null;
			Session["HR_applicant_edit"] = null;
			fill_grid();
//			btnsave.Text = "Save and Close";
			hdnid1.Value = app.id.ToString();
			Session["HR_applicant_edit"] = hdnid1.Value;
			gv_applicants.CancelEdit();
			gv_applicants.StartEdit(gv_applicants.FindVisibleIndexByKeyValue(lblid.Text));

		}
		else
		{
			gv_applicants.CancelEdit();
			Session["HR_applicant_grid"] = null;
			Session["HR_applicant_offers"] = null;
			Session["HR_applicant_edit"] = null;
			fill_grid();
		}
	}

	protected void gv_applicants_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		Session["HR_applicant_offers"] = null;

		ASPxTextBox tf = (ASPxTextBox)pop_new.FindControl("txt_check_first");
		ASPxTextBox tl = (ASPxTextBox)pop_new.FindControl("txt_check_last");
		
		
		gv_applicants.SettingsText.PopupEditFormCaption = "Enter New Applicant";
		ASPxTextBox tb_firstname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtfirstname");
		ASPxTextBox tb_lastname = (ASPxTextBox)gv_applicants.FindEditFormTemplateControl("txtlastname");
		tb_firstname.ClientEnabled = false;
		tb_lastname.ClientEnabled = false;
		tb_firstname.Text = tf.Text;
		tb_lastname.Text = tl.Text;
		
		
	}

	protected void gv_applicants_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
//		Session["HR_applicant_grid"] = null;
		fill_grid();
	}
	protected void gv_offers_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		ASPxGridView gv = (ASPxGridView)sender;
		if (e.VisibleIndex >= 0)
		{
		object this_is_applicant		= gv.GetRowValues(e.VisibleIndex, "isapplicant");
		object app_status = gv_applicants.GetRowValues(gv_applicants.EditingRowVisibleIndex, "status");
		object this_status				= gv.GetRowValues(e.VisibleIndex, "status");
			if ((this_is_applicant != null && this_is_applicant.Equals("0")) ||app_status.Equals("Hired")|| (this_status != null && this_status.Equals("Accepted")))
			{
					e.Visible = false;
			}
			else if (this_status != null && this_status.Equals("Released"))
			{
				if (e.ButtonType == ColumnCommandButtonType.Delete)
				{
					e.Visible = false;
				}
			}

		}
	}

	protected void gv_applicants_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (e.ButtonType == ColumnCommandButtonType.Edit)

		{
			int app = Convert.ToInt32(gv_applicants.GetRowValues(e.VisibleIndex, "addedbymemberid"));
			int branchid = Convert.ToInt32(gv_applicants.GetRowValues(e.VisibleIndex, "business_unit_id"));
			int x = _tools.getSQL_int(@"Select ifnull((Select ifnull(reports_to,0) from member_offers  where applicantid =@v0 and isapplicant = 1 order by id desc limit 1),0)", new object[] { app });
			if (NeMember.is_supervisor(app, current_user.id32) || (current_user.id32 == app)||(auth_for_edit_all)||(current_user.id32 == x)||((current_user.business_unit_id == branchid)&&(current_user.MemberTypeID==5)))
			{
				e.Visible = true;
				if (e.ButtonType == ColumnCommandButtonType.Delete)
				{
					if (gv_applicants.GetRowValues(e.VisibleIndex,"status").ToString()=="New")
					{
						e.Visible = true;
					}
					else
					{
						e.Visible = false;
					}
				}
			}
			else
			{
				e.Visible = false;
			}
		}
	}
	protected void cb_name_check_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		if(e.Parameter.Contains("|"))
			{
			string[] firstlast		= e.Parameter.Split('|');
			string name_first		= firstlast[0].Trim();
			string name_last		= firstlast[1].Trim();
			// Check direct - First Name / Last Name
			int c_first				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_firstname = @v0  AND member_lastname = @v1 ", new object[] {  name_first, name_last } );
			// Check nick - Nickname / Last Name
			int c_nick				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_nickname = @v0  AND member_lastname = @v1 ", new object[] {  name_first, name_last } );
			// Check last - Last Name
			int c_last				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_lastname like CONCAT(@v0 ,'%') ", new object[] {  name_last } );
			string info_list		= "";
			string this_type		= c_first > 0 ? "first" : c_nick > 0 ? "nick" : c_last > 0 ? "last" : "nomatch";
			string used_column		= c_first > 0 ? "first" : c_nick > 0 ? "nick" : c_last > 0 ? "last" : "";
			DataTable _matches		= new DataTable();
			if(c_first > 0 || c_nick > 0)
				{
				_matches			= Toolbox.doSQL_dt(string.Format(@"SELECT a.member_fullname, b.name,
a.member_status FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id
WHERE a.member_{0} name = @v0  AND a.member_lastname = @v1  LIMIT 10", used_column), new object[] {  name_first, name_last } );
				foreach(DataRow dr in _matches.Rows)
					{
					var full_name		= dr["member_fullname"];
					var company_name	= dr["name"];
					var status			= dr["member_status"];
					info_list			+= string.Format(@"{{""fullname"":""{0}"", ""branch"":""{1}"", ""status"":""{2}""}},", full_name, company_name, status);
					}
			//	NeShared.alert_hr(string.Format(@"Most likely a duplicate applicant ""{0} {1}"" is being created by ""{2}"" - First & last Name matched", name_first, name_last, current_user.FullName), "This is most likely a direct duplicate employee en' route... though it is a remote possiblity that there is a person with the same name as an existing employee.");
				}
			else if(c_last > 0)
				{
				_matches			= Toolbox.doSQL_dt(@"SELECT a.member_fullname, b.name, a.member_status FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_lastname like CONCAT(@v0 ,'%')  LIMIT 10", new object[] {  name_last } );
				foreach(DataRow dr in _matches.Rows)
					{
					var full_name		= dr["member_fullname"];
					var company_name	= dr["name"];
					var status			= dr["member_status"];
					info_list			+= string.Format(@"{{""fullname"":""{0}"", ""branch"":""{1}"", ""status"":""{2}""}},", full_name, company_name, status);
					}
			//	NeShared.alert_hr(string.Format(@"Possible duplicate applicant ""{0} {1}"" being created by ""{2}"" - Last Name matched", name_first, name_last, current_user.FullName), "This may or may not be a valid match, but we are alerting the manager of possible people with the last name, just in case.");
				}
			else
				{
				info_list		= "{},";
				}
			info_list			= info_list.TrimEnd(new char[]{','});
			e.Result			= string.Format(@"{{""type"":""{0}"",""info"":[{1}]}}", this_type, info_list);
			}
		}

	protected void gv_applicants_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from applicants  where id =@v0 limit 1 ", new object[] { e.Keys[0] });
		_tools.getSQL_void(@"Delete from member_offers  where isapplicant = 1 and applicantid =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		fill_grid();
	}
	protected void chk_all_CheckedChanged(object sender, EventArgs e)
	{
		
		Session["HR_applicant_grid"] = null;
		fill_grid();
	}
	protected void pop_new_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		
		if (e.Parameter.Contains("proceed"))
		{

			check_results.InnerHtml = "";
			btn_add_final.ClientVisible = false;
			pop_new.JSProperties["cp_close"] = "proceed";

		}
		bool add_app_note = false;
		bool add_mem_note = false;
		string name_first = txt_check_first.Text.Trim();
		string name_last = txt_check_last.Text.Trim();
		string stuff = "";
		DataTable _matches = new DataTable();
		check_results.InnerHtml = "";
		string mem_avoid_mist = "(";

		_matches = Toolbox.doSQL_dt(@"SELECT applicants.id, Concat(applicants.firstname,' ',applicants.lastname, ' of ', applicants.city) app_name, if (member.Member_ID is not null, (Concat(membertype.membertype_name ,' on ', date(applicants.dateentered), ' Status:',applicants.`status`,' - Employee Status: ',member_hrstatus.status)), (Concat(membertype.membertype_name ,' on ', date(applicants.dateentered), ' Status:',applicants.`status`))) name, member.Member_Status, emp_trm.term_date, emp_trm.reason_roe, member.Member_ID, member_hrstatus.status hr_status, business_unit.name, membertype.membertype_name, applicants.dateentered, applicants.`status` app_status FROM applicants INNER JOIN business_unit ON applicants.business_unit_id = business_unit.ID INNER JOIN membertype ON applicants.membertypeid = membertype.membertype_id LEFT JOIN member ON applicants.becomes_memberid = member.Member_ID LEFT JOIN emp_trm ON member.Member_ID = emp_trm.trm_member_id left join member_hrstatus on member_hrstatus.id = member.member_hrstatus_id where applicants.business_unit_id!=8 and (applicants.firstname like  CONCAT('%',@v0,'%')  and applicants.lastname like CONCAT('%',@v1,'%') and (member.member_status is null)) ", 
			new object[] {  name_first, name_last } );
		foreach (DataRow dr in _matches.Rows)
		{
	//		check_results.InnerHtml += string.Format("<tr><td><a href='javascript:boing(&quot;applicants.aspx?id={0}&quot;,&quot;hello&quot;,900,900)'>* Applicant: {1}</a></td><td>{2}</td><td>{3}</td></tr>", dr["id"], dr["name"], "", "");
			//check_results.InnerHtml += string.Format("<tr><td><a href='javascript:gv_applicants.PerformCallback(&quot;open_app|{0}&quot;);'>* Applicant: {1}</a></td><td>{2}</td><td>{3}</td></tr>", dr["id"], dr["name"], "", "");
			check_results.InnerHtml += string.Format("<tr><td><a href='javascript:gv_applicants.PerformCallback(&quot;open_app|{0}&quot;);'>* Applicant: {1}</a></td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["id"], dr["app_name"], dr["name"], dr["membertype_name"], dr["app_status"], dr["hr_status"], dr["reason_roe"]);
			add_app_note = true;
			
			if (dr["Member_ID"] != null)
			{
				mem_avoid_mist += dr["Member_ID"].ToString();
			}
		}
		if (mem_avoid_mist.Length>1)
		{
			mem_avoid_mist=mem_avoid_mist.TrimEnd(',') + ")";
			stuff = @"SELECT 
if((date(a.Member_TermDate)>curdate()),
Concat(a.member_fullname, ' of ', a.member_city , '-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' Status:',member_hrstatus.status,' and ', Member_Status),
Concat(a.member_fullname, ' of ', a.member_city , '-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' and their status was terminated on ',a.Member_TermDate)) _stuff,
a.member_fullname,
business_unit.name,
a.member_status,
emp_trm.term_date,
emp_trm.reason_roe,
a.member_id,
member_hrstatus.status hr_status,
membertype.membertype_name
FROM member a 
LEFT JOIN business_unit ON a.business_unit_id = business_unit.id 
left JOIN membertype ON a.member_membertype_id = membertype.membertype_id
LEFT JOIN emp_trm ON a.Member_ID = emp_trm.trm_member_id
left join member_hrstatus on member_hrstatus.id = a.member_hrstatus_id
WHERE a.member_id not in @v2 and ((a.member_firstname like CONCAT('%',@v0,'%') AND a.member_lastname Like CONCAT('%',@v1,'%')) ) order by a.member_id desc LIMIT 10";
		_matches = Toolbox.doSQL_dt(stuff, new object[] { name_first, name_last, mem_avoid_mist });

		}
		else
		{
			stuff = @"SELECT 
if((date(a.Member_TermDate)>curdate()),
Concat(a.member_fullname, ' of ', a.member_city ,'-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' Status:',member_hrstatus.status,' and ', Member_Status),
Concat(a.member_fullname, ' of ', a.member_city ,'-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' and their status was terminated on ',a.Member_TermDate)) _stuff,
a.member_fullname,
business_unit.name,
a.member_status,
emp_trm.term_date,
emp_trm.reason_roe,
a.member_id,
member_hrstatus.status hr_status,
membertype.membertype_name
FROM member a 
LEFT JOIN business_unit ON a.business_unit_id = business_unit.id 
left JOIN membertype ON a.member_membertype_id = membertype.membertype_id
LEFT JOIN emp_trm ON a.Member_ID = emp_trm.trm_member_id
left join member_hrstatus on member_hrstatus.id = a.member_hrstatus_id
WHERE  (a.member_firstname like CONCAT('%',@v0,'%') AND a.member_lastname Like CONCAT('%',@v1,'%')) order by a.member_id desc  LIMIT 10";
		_matches = Toolbox.doSQL_dt(stuff, new object[] { name_first, name_last });

		}
		foreach (DataRow dr in _matches.Rows)
		{
//			check_results.InnerHtml += string.Format("<tr><td><a href='javascript:boing(&quot;index.aspx?id={0}&quot;,&quot;hello&quot;,900,900)'>** Employee: {1}</a></td><td>{2}</td><td>{3}</td></tr>", dr["member_id"], dr["_stuff"], "", "");
			check_results.InnerHtml += string.Format("<tr><td><a href='javascript:boing(&quot;index.aspx?id={0}&quot;,&quot;hello&quot;,900,900)'>** Employee:  {1}</a></td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["member_id"], dr["member_fullname"], dr["name"], dr["membertype_name"], "", dr["hr_status"], dr["reason_roe"]);
			
			add_mem_note = true;
		}
		//			NeShared.alert_hr(string.Format(@"Possible duplicate applicant ""{0} {1}"" being created by ""{2}"" - Last Name matched", name_first, name_last, current_user.FullName), "This may or may not be a valid match, but we are alerting the manager of possible people with the last name, just in case.");
		pop_new.JSProperties["cp_warning"] = null;
	
		if (check_results.InnerHtml.Length>5)
		{
		
			check_results.InnerHtml = "Previous or Existing Employees/Applicants with similar names:</br><table width='100%'><tr bgcolor='#EBEBEB' color='white'><th align='left'>Name</th><th align='left'>Branch</th><th align='left'>Title</th><th align='left'>App. Status</th><th align='left'>HR Status</th><th align='left'>ROE Note</th></tr>" + check_results.InnerHtml;
			check_results.InnerHtml += "</table></br>";
			if (add_app_note)
			{
				pop_new.JSProperties["cp_warning"] = "dup_app";
				check_results.InnerHtml += "</br>*If you are wishing to add this applicant, please be sure it is not a duplicate, click the link.  If the page is not accessible, please contact the branch manager of the associated branch to discuss the applicant BEFORE you add them.</br>";
			}
			if (add_mem_note)
			{
				pop_new.JSProperties["cp_warning"] = "dup_mem";
				
				check_results.InnerHtml += "</br>**If you are wishing to hire a past employee, an email will be automatically sent to the HR administrator to contact you.";
			}

		}

		btn_add_final.ClientVisible = true;
	}
    protected void mem_notes_Init(object sender, EventArgs e)
    {
        ASPxMemo ddl = sender as ASPxMemo;
        GridViewDataItemTemplateContainer container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
        ddl.ClientSideEvents.TextChanged = String.Format("function (s, e) {{ gv_applicants.PerformCallback('{0}|' + s.GetValue()+ '|u'); }}", container.KeyValue);
    }
    protected void ddl_status_Init(object sender, EventArgs e)
    {
        ASPxComboBox ddl = sender as ASPxComboBox;
        GridViewDataItemTemplateContainer container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
        ddl.ClientSideEvents.SelectedIndexChanged = String.Format("function (s, e) {{ gv_applicants.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);
    }
    

        protected void mem_newnotes_Init(object sender, EventArgs e)
    {
        ASPxMemo ddl = sender as ASPxMemo;
        GridViewDataItemTemplateContainer container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
               ddl.ClientInstanceName = String.Format("mm_{0}", container.KeyValue);
    }

    protected void mem_btn_add_notes_Init(object sender, EventArgs e)
    {
        ASPxButton ddl = sender as ASPxButton;

        GridViewDataItemTemplateContainer container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
        ASPxMemo m = (ASPxMemo)container.FindControl("mem_newnote");

        ddl.ClientSideEvents.Click = String.Format("function (s, e) {{ gv_applicants.PerformCallback('{0}|' + mm_{0}.GetText() + '|n'); }}", container.KeyValue);
    }
}