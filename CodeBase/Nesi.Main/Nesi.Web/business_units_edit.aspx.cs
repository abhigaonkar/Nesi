using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;
using NESI.Common.Models;
using nesi.core;

public class Record
	{
	int id;
	double _reg;
	double _ot;
	double _dt;
	double _regSP;
	double _otSP;
	double _dtSP;

	public Record(int id, double _reg, double _ot, double _dt, double _regSP, double _otSP, double _dtSP)
		{
		this.id = id;
		this._reg = _reg;
		this._ot = _ot;
		this._dt = _dt;
		this._regSP = _regSP;
		this._otSP = _otSP;
		this._dtSP = _dtSP;
		}
	public int Id { get { return id; } }
	public double Reg { get { return _reg; } }
	public double OT { get { return _ot; } }
	public double DT { get { return _dt; } }
	public double RegSP { get { return _regSP; } }
	public double OTSP { get { return _otSP; } }
	public double DTSP { get { return _dtSP; } }

	}

public partial class business_units_edit : Page
	{
	NeMember myMember;
	private List<TextBox> m_dynamicTextBoxes;
	private List<string> m_dynamicFieldNames_tb;
	private List<string> m_dynamicFieldNames_ddl;
	private List<DropDownList> m_dynamicDDLs;
	private int _qty;

	private List<string> M_fieldtype_ddl;
	private List<string> M_fieldtype_tb;
	protected List<Record> list = new List<Record>();
    public string business_unit_id = "";
	protected void Page_Init(object sender, EventArgs e)
		{
		
		myMember = Toolbox.do_handle_authentication(OpsPage.BusinessUnits);
		ASPxPageControl1.TabPages.FindByName("options").Visible = myMember.AuthenticatedForPrivilege(OpsPrivilege.POApprovalSettingsTab);

		if (!myMember.AuthenticatedForPrivilege(OpsPrivilege.EditBranch))
			{
			ASPxPageControl1.TabPages[0].Enabled = false;
			ASPxPageControl1.ActiveTabIndex      = 1;
			}
		if (!myMember.AuthenticatedForPrivilege(OpsPrivilege.EditTargets))
			{
			ASPxPageControl1.TabPages[5].ClientVisible = false;
			}
		batchUpdatePanel.Visible = myMember.AuthenticatedForPrivilege(OpsPrivilege.AllowVisibilityToResetCopyChargeouts);

		using (var conn = Toolbox.connect())
			{
			divSearchResults.InnerHtml = "";
			divSearchResults.Controls.Clear();

			
			if (string.IsNullOrEmpty(Request.QueryString["id"]))
				{
				Toolbox.FriendlyException(Response, "You have not supplied a business unit id.", "/business_units.aspx");
				}
			else
				{
				business_unit_id = Request.QueryString["id"]; // Request.QueryString["business_unit_id"]
				}
			Session["bu_id"] = business_unit_id;
			if (business_unit_id.Equals("999"))
				{
				business_unit_id = "1";
				ASPxPageControl1.TabPages[3].Enabled = false;
				}
			SqlDataSource1.SelectCommand = "Select id, ddl_name name from business_unit where id in (" + new Current_User().visible_business_units + ") ORDER BY ddl_name";
			SqlDataSource1.DataBind();
			DataTable dt_emp = Toolbox.doSQL_dt(
				@"call get_grouped_bu_with_parent(@v0)",
				new object[] { business_unit_id });
			c_PlaceHolder.Controls.Clear();
			hdnid.Value = business_unit_id;
			c_button.Click += ButtonClick;
			var dtBusinessUnits = Toolbox.doSQL_dt(conn,
@" SELECT * FROM business_unit  WHERE ID =@v0",
				new object[] { business_unit_id });
			if (dtBusinessUnits.Rows.Count == 0) throw new Exception("Invalid Business Unit");

			DataRow drBusinessUnit = dtBusinessUnits.Rows[0];
			m_dynamicTextBoxes = new List<TextBox>();
			m_dynamicFieldNames_tb = new List<string>();
			m_dynamicFieldNames_ddl = new List<string>();
			m_dynamicDDLs = new List<DropDownList>();
			M_fieldtype_tb = new List<string>();
			M_fieldtype_ddl = new List<string>();

			var fieldno = 0;


			c_PlaceHolder.Controls.Add(new LiteralControl("<table>"));
			while (fieldno < dtBusinessUnits.Columns.Count)
				{

				var tb = new TextBox();
				var ddl = new DropDownList();
				ddl.ID = "ddl" + fieldno;
				tb.ID = "txt" + fieldno;
				tb.Width = 350;
				ddl.Width = 350;
				ddl.DataTextField = "_NAME";
				ddl.DataValueField = "id";
				c_PlaceHolder.Controls.Add(new LiteralControl("<tr>"));
				c_PlaceHolder.Controls.Add(new LiteralControl("<td style='width: 250px'>"));
				c_PlaceHolder.Controls.Add(new LiteralControl(dtBusinessUnits.Columns[fieldno].ColumnName));
				c_PlaceHolder.Controls.Add(new LiteralControl("</td>"));
				c_PlaceHolder.Controls.Add(new LiteralControl("<td>"));
				var columnName = dtBusinessUnits.Columns[fieldno].ColumnName;
				var stringlength = columnName.Length;
				var val = drBusinessUnit[fieldno].ToString();
				var isDdl = false;
				var add = false;
				switch (columnName.ToLower())
					{
					case "id":
						tb.ReadOnly = true;
						tb.Text = val;
						c_PlaceHolder.Controls.Add(tb);
						isDdl = false;
						add = false;
					break;
					case "acting_manager":
					case "acting_right_hand":					
					case "acting_safety_officer":
						ddl.DataSource = dt_emp;
						ddl.DataBind();
						ddl.SelectedValue = val;
						c_PlaceHolder.Controls.Add(ddl);
						isDdl = true;
						add = true;
					break;
					case "country":
						ddl.DataSource = Toolbox.doSQL_dt(@"SELECT country_code id, country_name _name FROM country", null);
						ddl.DataBind();
						ddl.SelectedValue = val;
						c_PlaceHolder.Controls.Add(ddl);
                        ddl.Enabled = false;
                        isDdl = true;
						add = true;
					break;
                    case "old_div":
                    case "old_company_id":
                    case "gl_div":
                    case "old_dsn":
                    case "use_er_wo_logic":
                    case "gl_default_material_revenue":
                    case "gl_default_material_expense":
                    case "default_fvr_template_ids":
                    case "default_fvr_template_id":
                    case "gl_default_labour_revenue":
                    case "gl_default_labour_expense":
                    case "gl_default_subcontract_revenue":
                    case "gl_default_subcontract_expense":                 
                    case "esa_id":
                    case "csa_id":
                    case "web_domain":
                    case "netsuite_bu_internal_id":
                    case "name":
                    case "address":
                    case "city":
                    case "prov":
                    case "state":
                    case "postal":
                    case "active":
                    case "default_currency":
                    case "tax_entity_id":
                        tb.ReadOnly = true;
                        tb.Text = val;
                        c_PlaceHolder.Controls.Add(tb);
                        isDdl = false;
                        add = false;
                        break;    
                    case "acting_purchaser":
                        ddl.DataSource = dt_emp;
                        ddl.DataBind();
                        ddl.SelectedValue = val;
                        ddl.Enabled = true;
                        c_PlaceHolder.Controls.Add(ddl);
                        isDdl = true;
                        add = true;
                        break;
                    default:
						tb.Text = val;
						c_PlaceHolder.Controls.Add(tb);
						isDdl = false;
						add = true;
					break;
					}
				c_PlaceHolder.Controls.Add(new LiteralControl("</td>"));
				c_PlaceHolder.Controls.Add(new LiteralControl("</tr>"));
				if(add)
					{
					if(isDdl)
						{
						m_dynamicFieldNames_ddl.Add(columnName);
						m_dynamicDDLs.Add(ddl);
						M_fieldtype_ddl.Add(dtBusinessUnits.Columns[fieldno].DataType.ToString());
						}
					else
						{
						m_dynamicFieldNames_tb.Add(columnName);
						m_dynamicTextBoxes.Add(tb);
						M_fieldtype_tb.Add(dtBusinessUnits.Columns[fieldno].DataType.ToString());
						}
					}
				fieldno++;
				}

			c_PlaceHolder.Controls.Add(new LiteralControl("</table>"));


			}
		}


	protected void Page_Load(object sender, EventArgs e)
		{




		    sql_pm.SelectParameters[0].DefaultValue = new Current_User().visible_business_units;
		var x = 0;
		var business_unit_id = "";
		if (string.IsNullOrEmpty(Request.QueryString["id"]))
			{
			Toolbox.FriendlyException(Response, "You have not supplied a business unit id.", "/business_units.aspx");
			}
		else
			{
			business_unit_id = Request.QueryString["id"]; // Request.QueryString["business_unit_id"]
			}
		hid_business_unit_id.Value = business_unit_id;
		Session["bu_id"] = business_unit_id;

		var comp = new NeBusinessUnit(Session["bu_id"]);
		Title = "Editing " + comp.name;
		if (!IsPostBack)
			{

			var se = (ASPxSpinEdit)gv_targets.FindTitleTemplateControl("spnfy");
			se.Value = DateTime.Today.Year;
			var se0 = (ASPxSpinEdit)gv_targets0.FindTitleTemplateControl("spnfy0");
			se0.Value = DateTime.Today.Year;
			spnfy1.Value = DateTime.Today.Year;
			if (myMember.business_unit_id == 11)
				{
				btnadd.ClientEnabled = true;
				}


			GetMemberTypeInformation(Session["bu_id"].ToString());
            BindPaytypes(Convert.ToInt32(business_unit_id));

            }


		var start = ASPxGridView2.PageIndex * ASPxGridView2.SettingsPager.PageSize;
		var end = ASPxGridView2.SettingsPager.PageSize;


		var column1 = ASPxGridView2.Columns["Reg"] as GridViewDataColumn;
		//        GridViewDataColumn column2 = ASPxGridView2.Columns["OT"] as GridViewDataColumn;
		//        GridViewDataColumn column3 = ASPxGridView2.Columns["DT"] as GridViewDataColumn;
		//        GridViewDataColumn column4 = ASPxGridView2.Columns["SP"] as GridViewDataColumn;
		//        GridViewDataColumn column5 = ASPxGridView2.Columns["OTSP"] as GridViewDataColumn;
		//        GridViewDataColumn column6 = ASPxGridView2.Columns["DTSP"] as GridViewDataColumn;

		for (var i = 0; i < end; i++)
			{
			var txtBox1 = (ASPxTextBox)ASPxGridView2.FindRowCellTemplateControl(i, column1, "txtReg");

			var ot = Convert.ToDouble(ASPxGridView2.GetRowValues(i, "OT"));
			var dt = Convert.ToDouble(ASPxGridView2.GetRowValues(i, "DT"));
			var sp = Convert.ToDouble(ASPxGridView2.GetRowValues(i, "SP"));
			var otsp = Convert.ToDouble(ASPxGridView2.GetRowValues(i, "OTSP"));
			var dtsp = Convert.ToDouble(ASPxGridView2.GetRowValues(i, "DTSP"));
			if (txtBox1 == null)
				continue;
			var id = Convert.ToInt32(ASPxGridView2.GetRowValues(i, ASPxGridView2.KeyFieldName));
			list.Add(new Record(id, Convert.ToDouble(txtBox1.Text), ot, dt, sp, otsp, dtsp));

			}




		}
	protected void load_org()
		{
		var c = new NeBusinessUnit(hid_business_unit_id.Value);

		var datarows = "";
		var ceo_level = new NeMember(Convert.ToInt32(
			Toolbox.doSQL_int(
				@"Select ifnull((select member_id from member  where member_status = 'Active' and reports_to = 0 and substring(member.member_country,1,1) =@v0 limit 1),0) ",
				new object[] { c.country.Substring(0, 1) })));
		var img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
						  ceo_level.FullName2 +
						  @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" + ceo_level.id +
						  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
						  ceo_level.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
		datarows = "[{v:'" + ceo_level.id + "', f:'" + img_builder + "'},'',''],";
		var dt = Toolbox.doSQL_dt(
			@"Select member_id id, get_name(member_id) _name, member.reports_to, membertype_name from member,membertype  where member_membertype_id = membertype_id and member_status = 'Active' and member_hrstatus_id !=5 and business_unit_id =@v0 order by member_membertype_id",
			new object[] { hid_business_unit_id.Value });
		var s = "";

		foreach (DataRow dr in dt.Rows)
			{
			var dr1 = dt.Select("id = " + dr["reports_to"]);
			if ((dr1.Length == 0) && (dr["reports_to"].ToString() != "0")
			) // if the reports to is not in the list of people form this company... start adding single levels above
				{
				var man1 = new NeMember(Convert.ToInt32(dr["reports_to"]));
				img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
							  man1.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
							  man1.id +
							  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
							  man1.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
				s = @"{v:'" + man1.id + @"', f:'" + img_builder + "'}";
				datarows += @"[" + s + ",'" + man1.reports_to + "','" + man1.membertype.name + "'],";
				if (man1.reports_to != 0)
					{
					var man2 = new NeMember(Convert.ToInt32(man1.reports_to));
					img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
								  man2.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
								  man2.id +
								  @""" width=""60""></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
								  man2.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
					s = @"{v:'" + man2.id + @"', f:'" + img_builder + "'}";
					datarows += @"[" + s + ",'" + man2.reports_to + "','" + man2.membertype.name + "'],";
					if (man2.reports_to != 0)
						{
						var man3 = new NeMember(Convert.ToInt32(man2.reports_to));
						img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
									  man3.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
									  man3.id +
									  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
									  man3.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
						s = @"{v:'" + man3.id + @"', f:'" + img_builder + "'}";
						datarows += @"[" + s + ",'" + man3.reports_to + "','" + man3.membertype.name + "'],";
						if (man3.reports_to != 0)
							{
							var man4 = new NeMember(Convert.ToInt32(man3.reports_to));
							img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
										  man4.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
										  man4.id +
										  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
										  man4.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
							s = @"{v:'" + man4.id + @"', f:'" + img_builder + "'}";
							datarows += @"[" + s + ",'" + man4.reports_to + "','" + man4.membertype.name + "'],";
							if (man4.reports_to != 0)
								{
								var man5 = new NeMember(Convert.ToInt32(man4.reports_to));
								img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
											  man5.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
											  man5.id +
											  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
											  man5.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
								s = @"{v:'" + man5.id + @"', f:'" + img_builder + "'}";
								datarows += @"[" + s + ",'" + man5.reports_to + "','" + man5.membertype.name + "'],";
								if (man5.reports_to != 0)
									{
									var man6 = new NeMember(Convert.ToInt32(man5.reports_to));
									img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
												  man6.FullName2 +
												  @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" + man6.id +
												  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
												  man6.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";
									s = @"{v:'" + man6.id + @"', f:'" + img_builder + "'}";
									datarows += @"[" + s + ",'" + man6.reports_to + "','" + man6.membertype.name + "'],";

									}
								}
							}
						}
					}
				}
			img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" +
						  dr["_name"] + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" +
						  dr["id"] +
						  @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" +
						  dr["membertype_name"] + @"</td></tr><tr><td></td></tr></tbody></table>";

			s = @"{v:'" + dr["id"] + @"', f:'" + img_builder + "'}";
			datarows += @"[" + s + ",'" + dr["reports_to"] + "','" + dr["membertype_name"] + "'],";

			}

		div_org.InnerHtml = @"<html>
  <head>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script type='text/javascript'>
      google.load('visualization', '1', {packages:['orgchart']});
      google.setOnLoadCallback(drawChart);
      function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Name');
        data.addColumn('string', 'Manager');
        data.addColumn('string', 'ToolTip');
        data.addRows([
          " + datarows.TrimEnd(',') + @"
        ]);
        var chart = new google.visualization.OrgChart(document.getElementById('chart_div'));
        chart.draw(data, {allowHtml:true});
      }
    </script>
  </head>

  <body>
    <div id='chart_div'></div>
  </body>
</html>";

		}
	protected void ButtonClick(object sender, EventArgs e)
		{

		c_label.Text = "";
		var strtemp = "";

		var strtemp1 = "";
		int stringlength;
		var inserttext = "";
		var insertfield = "";
		int x;
		x = 0;
		foreach (var tb in m_dynamicTextBoxes)
			{
			var fieldtype = M_fieldtype_tb[x];
			var field =  m_dynamicFieldNames_tb[x];
			if (tb != null)
				{
				strtemp1 = tb.Text;
				if (x < m_dynamicTextBoxes.Count)
					{
					if (tb.Text == "" && fieldtype != "System.String" && fieldtype != "System.DateTime")
						{
						strtemp1 = Convert.ToString(0);
						}
					}
				if (x > 0)
					{
					insertfield += field;
					stringlength = field.Length;
					if (stringlength > 4 && field.Substring(stringlength - 4, 3) == "Tax")
						{
						strtemp += field + "=" + m_dynamicDDLs[x].SelectedValue;
						inserttext += m_dynamicDDLs[x].SelectedValue;
						}
					else
						{
						if (fieldtype == "System.String")
							{

							    switch (field)
							    {
                                    case "wopath":
                                        strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\")  + "\" \n";
                                    break;
							        case "popath":
							            strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
                                    break;
							        case "rootdirectory":
							            strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
							            break;
							        case "workorderprinter":
							            strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
							            break;
							        case "laserprinter":
							            strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
							            break;
							        case "barcodeprinter":
							            strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
							            break;
                                case "netsuite_bu_internal_id":
                                        if(strtemp1.Trim()=="")
                                        {
                                         strtemp += field + "= null \n";
                                        }
                                        else
                                       {
                                        strtemp += field + "= \"" + strtemp1.Replace("\\", "\\\\") + "\" \n";
                                       }                                       
                                    break;
                                default:
                                        strtemp += field + "='" + Toolbox.AddSlashes(strtemp1) + "'\n";
                                    break;
							    }



							
							inserttext += "'" + strtemp1 + "'\n";
							}
						else if (fieldtype == "System.DateTime")
							{
							if (strtemp1.Trim() != "")
								{
								strtemp += field + "='" + Convert.ToDateTime(strtemp1).ToString("yyyy-MM-dd HH:mm:ss") + "'\n";
								inserttext += "'" + Convert.ToDateTime(strtemp1).ToString("yyyy-MM-dd HH:mm:ss") + "'\n";
								}
							else
								{
								strtemp += field + "='" + Toolbox.MySQLNow_long() + "'\n";
								inserttext += "'" + Toolbox.MySQLNow_long() + "'\n";
								}
							}
						else if (fieldtype.Contains("Int"))
							{
							strtemp1 = strtemp1 == "" ? "0" : strtemp1;
							strtemp += field + "='" + strtemp1 + "'\n";
							inserttext += strtemp1;
							}
						else if (fieldtype == "System.Boolean")
							{
							strtemp1 = strtemp1 == "" ? "0" : strtemp1;
							strtemp += field + "=" + strtemp1;
							inserttext += strtemp1;
							}
						else
							{
							strtemp += field + "='" + strtemp1 + "'\n";
							inserttext += "'" + strtemp1 + "'\n";
							}
						}
					}
				}
			x++;
			if (x <= m_dynamicTextBoxes.Count + 1 && x > 1)
				{
				strtemp += ",\n";
				inserttext += ",";
				insertfield += ",";
				}
			}
		int y = x;
		x = 0;
		foreach (var ddl in m_dynamicDDLs)
			{
			var fieldtype = M_fieldtype_ddl[x];
			var field =  m_dynamicFieldNames_ddl[x];
			if (ddl != null)
				{
				strtemp1 = ddl.SelectedValue;
				strtemp1 = strtemp1 == "" ? field == "country" ? "CDN" : "0" : strtemp1;
				strtemp += field + "='" + strtemp1 + "' \n ";
				inserttext += strtemp1;

				}
			x++;

			if (x < m_dynamicDDLs.Count && x > 0)
				{
				strtemp += ",\n";
				inserttext += ",";
				insertfield += ",";
				}
			}


		strtemp = "Update business_unit Set " + strtemp + " where id = " + Session["bu_id"];
		//   strtemp = "Update Company Set Company_WOPath='\\\\\\\\ne-server-06\\\\XDrive\\\\BV7' where Company_id = 8";
		Toolbox.doSQL_void(strtemp);
		//	sync_godaddy_companies();
		//div_sql.InnerText = strtemp;
		if (Session["bu_id"].Equals("new"))
			{
			c_label.Text = "Successfully Added Business Unit in Mysql";
			//Response.Redirect("company.aspx");
			}
		else
			c_label.Text = "Successfully Updated Business Unit in Mysql";


		}



    private void BindPaytypes(int _business_unit_id)
    {
        var available = @"SELECT a.Description,a.PayTypeHours_ID FROM paytypehours a WHERE a.PayTypeHours_ID NOT IN (SELECT paytype_id FROM bu_paytype_link WHERE Business_unit_id = @v0)";
        var inUse = @"SELECT b.Description,a.paytype_id  FROM bu_paytype_link a LEFT JOIN paytypehours b ON a.paytype_id = b.PayTypeHours_ID WHERE a.business_unit_id = @v0";
        var inUseDt = Toolbox.doSQL_dt(inUse, new object[] { _business_unit_id });
        var availableDt = Toolbox.doSQL_dt(available,new object[] { _business_unit_id});
        if(availableDt.Rows.Count > 0)
        {
            
            LeftPaytype.DataSource = availableDt;
            LeftPaytype.DataTextField = "Description";
            LeftPaytype.DataValueField = "PayTypeHours_ID"; 
            LeftPaytype.DataBind();
        }
        if (inUseDt.Rows.Count > 0)
        {

            RightPaytype.DataSource = inUseDt;
            RightPaytype.DataTextField = "Description";
            RightPaytype.DataValueField = "paytype_id";
            RightPaytype.DataBind();
        }
    }

    protected void MoveRight(object sender, EventArgs e)
    {
        
        while (LeftPaytype.Items.Count>0 && LeftPaytype.SelectedItem != null)
        {
            ListItem selectedPaytype = LeftPaytype.SelectedItem;
            selectedPaytype.Selected = false;
            RightPaytype.Items.Add(selectedPaytype);
            LeftPaytype.Items.Remove(selectedPaytype);
            Toolbox.doSQL_void(@"INSERT INTO bu_paytype_link (business_unit_id,paytype_id) VALUE(@v0,@v1)",new object []{business_unit_id,selectedPaytype.Value});
        }
    }
 
    protected void MoveLeft(object sender, EventArgs e)
    {
        while (RightPaytype.Items.Count > 0 && RightPaytype.SelectedItem != null)
        {
            ListItem selectedPaytype = RightPaytype.SelectedItem;
            var test = selectedPaytype.Value;
            var paytypeUsage = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM membertime a WHERE a.business_unit_id = @v0 AND a.MemberTime_PayTypeHours_ID= @v1", new object[] { hid_business_unit_id.Value, test});
            if (paytypeUsage > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(UpdatePanel), "alert", "alert('This pay type can not be removed as it is already being used');", true);

            }
            else
            {
             Toolbox.doSQL_void(@"DELETE FROM bu_paytype_link WHERE business_unit_id = @v0 AND paytype_id = @v1", new object[] { business_unit_id, selectedPaytype.Value });
            selectedPaytype.Selected = false;
            LeftPaytype.Items.Add(selectedPaytype);
            RightPaytype.Items.Remove(selectedPaytype);
            

            }
        }
    }

    protected void GetMemberTypeInformation(string CompanyID)
		{
		//CompanyID = "8";

		if (myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewEditBranchChargeoutRates))
			{



			var SQL = @"
SELECT
	0 o,
	membertype.MemberType_Name,
	membertype.membertype_ID AS ID,
	ptreg.Description,
	reg.Chargeout AS Reg,
	ot.Chargeout AS OT,
	dt.Chargeout AS DT,
	regsp.Chargeout AS SP,
	otsp.Chargeout AS OTSP,
	dtsp.Chargeout AS DTSP
FROM
	membertype
INNER JOIN membertype_chargeout AS reg ON membertype.MemberType_ID = reg.membertype_id
INNER JOIN paytypehours AS ptreg ON ptreg.PayTypeHours_ID = reg.paytype_id AND ptreg.PayTypeHours_ID = 1
INNER JOIN membertype_chargeout AS ot ON ot.membertype_id = membertype.MemberType_ID
INNER JOIN paytypehours AS ptot ON ptot.PayTypeHours_ID = ot.paytype_id AND ptot.PayTypeHours_ID = 2
INNER JOIN membertype_chargeout AS dt ON membertype.MemberType_ID = dt.membertype_id
INNER JOIN paytypehours AS ptdt ON ptdt.PayTypeHours_ID = dt.paytype_id AND ptdt.PayTypeHours_ID = 3
INNER JOIN membertype_chargeout AS regsp ON membertype.MemberType_ID = regsp.membertype_id
INNER JOIN paytypehours AS ptregsp ON ptregsp.PayTypeHours_ID = regsp.paytype_id AND ptregsp.PayTypeHours_ID = 4
INNER JOIN membertype_chargeout AS otsp ON membertype.MemberType_ID = otsp.membertype_id
INNER JOIN paytypehours AS ptotsp ON ptotsp.PayTypeHours_ID = otsp.paytype_id AND ptotsp.PayTypeHours_ID = 5
INNER JOIN membertype_chargeout AS dtsp ON membertype.MemberType_ID = dtsp.membertype_id
INNER JOIN paytypehours AS ptdtsp ON ptdtsp.PayTypeHours_ID = dtsp.paytype_id AND ptdtsp.PayTypeHours_ID = 6
WHERE
	reg.business_unit_id = @v0 AND
	ot.business_unit_id =  @v0 and
	dt.business_unit_id =  @v0 AND
	regsp.business_unit_id =  @v0 AND
	otsp.business_unit_id =  @v0 AND
	dtsp.business_unit_id =  @v0 and membertype.active =1
UNION
	SELECT 
		1 o,
		membertype_name,
		membertype_id ID,
		'' description,
		0 Reg,
		0 OT,
		0 DT,
		0 SP,
		0 OTSP,
		0 DTSP
	FROM
		membertype a
	WHERE 
		membertype_id NOT IN (SELECT DISTINCT(membertype_id) FROM membertype_chargeout WHERE business_unit_id = @v0 AND paytype_id = 1) AND
		membertype_name NOT LIKE '%*%' AND TRIM(membertype_name) != 'BLANK' and a.active = 1
	order by o,MemberType_Name";
			var dt = Toolbox.doSQL_dt(SQL, new object[] { CompanyID });
			_qty = dt.Rows.Count;
			ASPxGridView2.DataSource = dt;
			ASPxGridView2.SettingsPager.PageSize = _qty;
			ASPxGridView2.DataBind();

			}


		}


	protected void ASPxGridView1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
		{
		double oldchargerout;
		double newchargeout;
		var editingkey = e.Keys[0];
		var newtableid = Convert.ToInt32(editingkey.ToString());
		oldchargerout = Convert.ToDouble(e.OldValues["Chargeout"].ToString());
		newchargeout = Convert.ToDouble(e.NewValues["Chargeout"].ToString());
		var SQL = "UPDATE membertype_chargeout SET Chargeout = " + newchargeout + " WHERE id = " + editingkey;
		var tools = new Toolbox();
		tools.getSQL_void(@"UPDATE membertype_chargeout  SET Chargeout =@v0  WHERE id =@v1",
			new object[] { newchargeout, editingkey });
		e.Cancel = true;
		//   ASPxGridView1.CancelEdit();
		GetMemberTypeInformation(Session["bu_id"].ToString());
		Toolbox.doSQL_void(
			@"Insert into membertype_chargeout_history (membertype_chargeout_history_memberid,membertype_chargeout_history_rate,membertype_chargeout_history_date,membertype_chargeout_history_membertype_id,business_unit_id)  values (@v0,@v1,curdate(),@v2,@v3)",
			new object[] { myMember.id, newchargeout, editingkey, Session["bu_id"] });

		}


	protected void ASPxGridView2_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
		{
		if (e.CommandArgs.CommandName == "Save")
			{
			var index = e.VisibleIndex;
			var membertype_id = e.KeyValue;
			var business_unit_id = hid_business_unit_id.Value;
			var rt = list[index].Reg;
			if (exists(membertype_id, business_unit_id, 1))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id =@v2  and paytype_id = 1",
					new object[] { rt, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 1, rt });
				}
			if (exists(membertype_id, business_unit_id, 2))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id =@v2  and paytype_id = 2",
					new object[] { rt * 1.5, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 2, rt * 1.5 });
				}
			if (exists(membertype_id, business_unit_id, 3))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id=@v2  and paytype_id = 3",
					new object[] { rt * 2, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 3, rt * 2 });
				}
			if (exists(membertype_id, business_unit_id, 4))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id =@v2  and paytype_id = 4",
					new object[] { rt * 1.1, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 4, rt * 1.1 });
				}
			if (exists(membertype_id, business_unit_id, 5))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id =@v2  and paytype_id = 5",
					new object[] { rt * 1.5 * 1.1, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 5, rt * 1.5 * 1.1 });
				}
			if (exists(membertype_id, business_unit_id, 6))
				{
				Toolbox.doSQL_void(
					@"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_id =@v1 and business_unit_id=@v2  and paytype_id = 6",
					new object[] { rt * 2 * 1.1, membertype_id, business_unit_id });
				}
			else
				{
				Toolbox.doSQL_void(
					@"INSERT INTO membertype_chargeout (business_unit_id, membertype_id, paytype_id, chargeout) VALUES (@v0 , @v1 , @v2 , @v3 )",
					new object[] { business_unit_id, membertype_id, 6, rt * 2 * 1.1 });
				}
			GetMemberTypeInformation(business_unit_id);
			Toolbox.doSQL_void(
				@"Insert into membertype_chargeout_history (membertype_chargeout_history_memberid,membertype_chargeout_history_rate,membertype_chargeout_history_date,membertype_chargeout_history_membertype_id,business_unit_id)  values (@v0,@v1,now(),@v2,@v3)",
				new object[] { myMember.id, rt, membertype_id, business_unit_id });

			}

		}

	private bool exists(object _membertype_id, object _business_unit_id, int _paytype_id)
		{
		return Toolbox.doSQL_int(
				   @"SELECT COUNT(id) FROM membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ",
				   new object[] { _membertype_id, _business_unit_id, _paytype_id }) == 1;
		}

	protected void ASPxGridView2_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var gv = (ASPxGridView)sender;

		if (e.DataColumn.Caption == "Reg")
			{
			var tb = (ASPxTextBox)gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "txtReg");
			tb.ClientInstanceName = "txtReg" + e.VisibleIndex;
			}
		if (e.DataColumn.Caption == "Member Type")
			{
			var membertype_id = gv.GetRowValues(e.VisibleIndex, "ID");
			var membertype_name = gv.GetRowValues(e.VisibleIndex, "MemberType_Name").ToString();
			var dt = Toolbox.doSQL_dt(
				@"SELECT member_fullname FROM member WHERE member_membertype_id = @v0  AND business_unit_id = @v1  AND member_status = 'Active'",
				new object[] { membertype_id, hid_business_unit_id.Value });
			if (dt.Rows.Count > 0)
				{
				var tooltip = new StringBuilder();
				tooltip.Append("<table><thead><th>Employee(s) in with this title</th></thead><tbody>");
				foreach (DataRow dr in dt.Rows)
					{
					tooltip.AppendFormat("<tr><td>{0}</td></tr>", HttpUtility.HtmlEncode(dr["member_fullname"]));
					}
				tooltip.Append("</tbody></table>");
				e.Cell.CssClass += " opt1";
				e.Cell.Attributes["data-tooltip"] = tooltip.ToString();
				e.Cell.Attributes["data-title"] = membertype_name;
				}
			}

		}

	protected void btnadd_Click(object sender, EventArgs e)
		{
		if (ASPxTextBox1.Text != "" && ASPxTextBox2.Text != "")
			{
			Toolbox.doSQL_void(@"Insert into membertype (membertype_name,active,is_elevated)  values (@v0,1,0)",
				new object[] { ASPxTextBox1.Text });
			var id = Toolbox.doSQL_int(@"Select max(membertype_id) from membertype", new object[] { });

			var dt = Toolbox.doSQL_dt(@"Select id from business_unit  where business_unit.active = 'T'", null);
			foreach (DataRow dr in dt.Rows)
				{
				var business_unit_id = dr["id"];
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,1,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) });
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,2,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) * 1.5 });
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,3,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) * 2 });
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,4,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) * 1.1 });
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,5,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) * 1.65 });
				Toolbox.doSQL_void(
					@"Insert into membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  values (@v0,@v1,6,@v2)",
					new object[] { business_unit_id, id, Convert.ToDouble(ASPxTextBox2.Text) * 2.2 });
				}
			}
		}

	protected void btnupdate_Click(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			if (ASPxComboBox3.SelectedItem.Text != "")
				{
				var dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM membertype_chargeout WHERE business_unit_id = @v0", new object[] { ASPxComboBox3.SelectedItem.Value });
				if (dt.Rows.Count > 0)
					{
					Toolbox.doSQL_void(conn, @"DELETE FROM membertype_chargeout  WHERE business_unit_id = @v0", new object[] { Session["bu_id"] });
					foreach (DataRow dr in dt.Rows)
						{
						Toolbox.doSQL_void(conn, @"INSERT INTO membertype_chargeout (business_unit_id,membertype_id,paytype_id,chargeout)  VALUES (@v0,@v1,@v2,@v3)",
							new object[] { Session["bu_id"], dr["membertype_id"], dr["paytype_id"], dr["chargeout"] });
						}
					}
				}
			}
		GetMemberTypeInformation(Session["bu_id"].ToString());
		}

	protected void btnprint_rates_Click(object sender, EventArgs e)
		{
		ScriptManager.RegisterStartupScript(this, GetType(), "open_",
			"boing('/sections/reports/rates_sheet/index.aspx?business_unit_id=" + Session["bu_id"] +
			"&customerid=0','rates',950,800)", true);

		}

	[WebMethod]
	public static int dist_exists(int business_unit_id, int start, int end, int save_id)
		{
		var c = 0;
		if (save_id != 0)
			{
			c = Toolbox.doSQL_int(
				@"SELECT IFNULL(MAX(id),0) from business_unit_po_dist WHERE business_unit_id = @v0  AND (amount_from BETWEEN @v1  AND @v2  OR amount_to BETWEEN @v1  AND @v2 ) AND id != @v3 ",
				new object[] { business_unit_id, start, end, save_id });
			}
		else
			{
			c = Toolbox.doSQL_int(
				@"SELECT IFNULL(MAX(id),0) from business_unit_po_dist WHERE business_unit_id = @v0  AND (amount_from BETWEEN @v1  AND @v2  OR amount_to BETWEEN @v1  AND @v2 )",
				new object[] { business_unit_id, start, end });
			}
		return c;
		}

	[WebMethod]
	public static void dist_save(int business_unit_id, int start, int end, int member_id, int save_id)
		{
		var c = dist_exists(business_unit_id, start, end, save_id);
		if (save_id != 0)
			{
			if (c != save_id && c != 0)
				{
				throw new Exception("This range has already been set");
				}
			Toolbox.doSQL_void(@"UPDATE business_unit_po_dist SET amount_from = @v0 , amount_to = @v1  WHERE id = @v2 ",
				new object[] { start, end, save_id });
			}
		else
			{
			if (c > 0)
				{
				throw new Exception("This range has already been set");
				}
			Toolbox.doSQL_void(
				@" INSERT INTO business_unit_po_dist ( business_unit_id, amount_from, amount_to, member_id ) VALUES ( @v0 , @v1 , @v2 , @v3  )",
				new object[] { business_unit_id, start, end, member_id });
			}
		}

	protected void gv_dist_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var tb = (ASPxTrackBar)gv.FindEditFormTemplateControl("trackbar");
		var lb_range_min = (ASPxLabel)gv.FindEditFormTemplateControl("lb_range_min");
		var lb_range_max = (ASPxLabel)gv.FindEditFormTemplateControl("lb_range_max");
		var lb_error = (ASPxLabel)gv.FindEditFormTemplateControl("lb_error");
		var business_unit_id = Convert.ToInt32(Session["bu_id"]);
		if (gv.IsEditing && gv.EditingRowVisibleIndex >= 0)
			{
			tb.PositionStart = (int)gv.GetRowValues(gv.EditingRowVisibleIndex, "amount_from");
			tb.PositionEnd = (int)gv.GetRowValues(gv.EditingRowVisibleIndex, "amount_to");
			lb_range_min.Text = Convert.ToDouble(tb.PositionStart).ToString("C0");
			lb_range_max.Text = Convert.ToDouble(tb.PositionEnd).ToString("C0");
			}
		else
			{
			tb.PositionStart = 0;
			tb.PositionEnd = 500;
			lb_range_min.Text = "$0";
			lb_range_max.Text = "$500";
			lb_error.Text = dist_exists(business_unit_id, 0, 500, 0) > 0
				? "A distribution already exists in this range"
				: "";
			}
		}

	protected void gv_dist_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var id = 0;
		if (gv.IsEditing)
			{
			id = Convert.ToInt32(gv.GetRowValues(gv.EditingRowVisibleIndex, "id"));
			}
		e.Properties.Add("cp_editid", id);
		}

	protected void gv_targets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var tb1 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb1");
		var tb2 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb2");
		var tb3 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb3");
		var tb4 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb4");
		var tb5 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb5");
		var tb6 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb6");
		var tb7 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb7");
		var tb8 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb8");
		var tb9 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb9");
		var tb10 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb10");
		var tb11 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb11");
		var tb12 = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb12");
		var cb = (ASPxComboBox)gv_targets.FindTitleTemplateControl("ddltarget");
		var spn = (ASPxSpinEdit)gv_targets.FindTitleTemplateControl("spnfy");

		if (e.Parameters[0].ToString() == "a")
			{
			var x = 1;
			var insert = "";
			List<object> paramObjects = new List<object>();
			while (x <= 12)
				{
				var tb = (ASPxTextBox)gv_targets.FindTitleTemplateControl("tb" + x);
				if ((tb.Text == null) || (tb.Text == ""))
					{
					throw new Exception("You must enter a valid target for Month " + x);
					}
				insert += tb.Text + ",";
				paramObjects.Add(tb.Text);
				x++;
				}
			insert = insert.TrimEnd(',');


			if (cb.Text != "")
				{
				if (Toolbox.doSQL_int(
						@"Select ifnull(sum(id),0) from fytarget  where fy =@v0 and business_unit_id =@v1  and target_name =@v2 ",
						new object[] { spn.Value, hdnid.Value, cb.Text }) == 0)
					{
					paramObjects.Add(cb.Text);
					paramObjects.Add(hdnid.Value);
					paramObjects.Add(spn.Value);
					Toolbox.doSQL_void(
						"Insert into fytarget (target_name,business_unit_id,fy,m1,m2,m3,m4,m5,m6,m7,m8,m9,m10,m11,m12) values(@v12,@v13,@v14,@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11)",
						paramObjects.ToArray());
					gv_targets.DataBind();
					}

				else
					{
					Toolbox.doSQL_void(@"update fytarget  set m1=@v0,m2=@v1 ,m3=@v2 ,m4=@v3 ,m5=@v4 ,
m6=@v5 ,m7=@v6 ,m8=@v7 ,m9=@v8 ,m10=@v9 ,m11=@v10 ,m12=@v11   where fy =@v12 and business_unit_id=@v13  and target_name =@v14 ",
						new object[]
							{
						tb1.Text, tb2.Text, tb3.Text, tb4.Text, tb5.Text, tb6.Text, tb7.Text, tb8.Text, tb9.Text, tb10.Text, tb11.Text,
						tb12.Text, spn.Value, hdnid.Value, cb.Text
							});
					gv_targets.DataBind();

					}
				}
			else
				{
				throw new Exception("You must select a valid target name");
				}
			gv_targets.DataBind();
			load_totals();
			}
		else if (e.Parameters[0].ToString() == "s")
			{
			var ee = e.Parameters.Split('|');
			var _value = ee.GetValue(3).ToString().Split('_');
			Toolbox.doSQL_void(
						@"update fytarget set fytarget.m" + (Convert.ToInt32(_value.GetValue(1)) - 3) + "=@v0 where id =@v1 ", new object[] {
					ee.GetValue(1), ee.GetValue(2)
							});
			gv_targets.DataBind();
			load_totals();
			}
		else if (e.Parameters[0].ToString() == "z")
			{
			if (cb.Text == "Margin")
				{
				tb1.Text = "50";
				tb2.Text = "50";
				tb3.Text = "50";
				tb4.Text = "50";
				tb5.Text = "50";
				tb6.Text = "50";
				tb7.Text = "50";
				tb8.Text = "50";
				tb9.Text = "50";
				tb10.Text = "50";
				tb11.Text = "50";
				tb12.Text = "50";
				}
			else if (cb.Text == "Revenue")
				{
				tb1.Text = "";
				tb2.Text = "";
				tb3.Text = "";
				tb4.Text = "";
				tb5.Text = "";
				tb6.Text = "";
				tb7.Text = "";
				tb8.Text = "";
				tb9.Text = "";
				tb10.Text = "";
				tb11.Text = "";
				tb12.Text = "";
				}
			else if (cb.Text == "Days to Invoice")
				{
				tb1.Text = "3";
				tb2.Text = "3";
				tb3.Text = "3";
				tb4.Text = "3";
				tb5.Text = "3";
				tb6.Text = "3";
				tb7.Text = "3";
				tb8.Text = "3";
				tb9.Text = "3";
				tb10.Text = "3";
				tb11.Text = "3";
				tb12.Text = "3";
				}
			load_totals();

			}
		//	gv_targets.DataBind();

		}
	protected void load_totals()
		{
		var cb = (ASPxComboBox)gv_targets.FindTitleTemplateControl("ddltarget");
		var spn = (ASPxSpinEdit)gv_targets.FindTitleTemplateControl("spnfy");
		var y = 1;
		if (spn.Text == null)
			{
			spn.Text = spnfy1.Text;
			}
		double ttt = 0;
		double uuu = 0;
		double yyy = 0;
		double _total_target_margin = 0;
		while (y <= 12)
			{
			var lb2 = (ASPxLabel)gv_targets.FindTitleTemplateControl("t" + y);
			var target = Toolbox.doSQL_double(@"select ifnull((Select ifnull(fytarget.m" + y + ",0) from fytarget  " +
											  "where fytarget.target_name =@v0 and fytarget.business_unit_id =@v1  and fytarget.fy =@v2 ),0) ",
				new object[] { cb.Text, hid_business_unit_id.Value, spn.Value });

			_total_target_margin += Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y + " from" +
														 " fytarget where business_unit_id =@v0 " +
														 " and target_name = 'Revenue' and fy = a.fy)) from fytarget a where a.business_unit_id =@v0 "
														 + " and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
																hid_business_unit_id.Value,spn.Value
															 });
			double dolled = 0;
			if (cb.Text == "Revenue")
				{
				dolled = Toolbox.doSQL_double("Select ifnull(sum(ifnull(fytarget_employees.m" + y + ",0)),0) " +
										   "from fytarget_employees where fytarget_employees.target_name = @v0 " +
										   "and fytarget_employees.business_unit_id = @v1 and fytarget_employees.fy =@v2 ", new object[] {
					cb.Text,hid_business_unit_id.Value,spn.Value
					});
				}
			else if (cb.Text == "Margin")
				{
				dolled = Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y
					+ " from fytarget_employees where member_id = a.member_id and business_unit_id = @v0"
					+ " and target_name = 'Revenue' and fy = a.fy)) " +
											  "from fytarget_employees a where a.business_unit_id = @v0"
											  + " and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					});
				uuu += Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y
					+ " from fytarget_employees where member_id = a.member_id and business_unit_id = @v0"
					+ " and target_name = 'Revenue' and fy = a.fy)) " +
											"from fytarget_employees a where a.business_unit_id = @v0"
											+ " and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					});
				dolled = 100 * (dolled / Toolbox.doSQL_double("select ifnull((Select ifnull(fytarget.m" + y
					+ ",0) from fytarget where fytarget.target_name = 'Revenue' and fytarget.business_unit_id = @v0 and fytarget.fy = @v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					}));
				yyy += Toolbox.doSQL_double("select ifnull((Select ifnull(fytarget.m" + y
					+ ",0) from fytarget where fytarget.target_name = 'Revenue' and fytarget.business_unit_id = @v0 and fytarget.fy = @v1),0)", new object[] {
				hid_business_unit_id.Value,spn.Value
				});
				}

			lb2.Text = Convert.ToString(Math.Round(target - dolled, 1));
			if ((target - dolled > 0))
				{
				lb2.ForeColor = Color.Red;
				lb2.Font.Bold = true;
				}
			else
				{
				lb2.ForeColor = Color.White;
				lb2.Font.Bold = false;
				}

			y++;
			if (cb.Text == "Revenue")
				{
				ttt = ttt + (target - dolled);
				}
			else if (cb.Text == "Margin")
				{
				ttt = Math.Round(((_total_target_margin - uuu) / yyy) * 100, 1);
				}
			else if (cb.Text == "Days to Invoice")
				{
				lb2.ClientVisible = false;
				}
			}
		var lb5 = (ASPxLabel)gv_targets.FindTitleTemplateControl("t0total");
		lb5.Text = ttt.ToString();
		if (ttt > 0)
			{
			lb5.ForeColor = Color.Red;
			lb5.Font.Bold = true;
			}
		else
			{
			lb5.ForeColor = Color.White;
			lb5.Font.Bold = true;
			}
		}
	protected void load_totals0()
		{
		var cb = (ASPxComboBox)gv_targets0.FindTitleTemplateControl("ddltarget0");
		var spn = (ASPxSpinEdit)gv_targets0.FindTitleTemplateControl("spnfy0");
		var y = 1;
		if (spn.Text == null)
			{
			spn.Text = spnfy1.Text;
			}
		double ttt = 0;
		double uuu = 0;
		double yyy = 0;
		double _total_target_margin = 0;
		while (y <= 12)
			{
			var lb2 = (ASPxLabel)gv_targets0.FindTitleTemplateControl("t0" + y);
			var lb3 = (ASPxLabel)gv_targets0.FindTitleTemplateControl("tbr" + y);

			var target = Toolbox.doSQL_double(@"select ifnull((Select ifnull(fytarget.m" + y + ",0) " +
											  "from fytarget where fytarget.target_name =@v0 and fytarget.business_unit_id =@v1  and fytarget.fy =@v2 ),0) ", new object[] { cb.Text, hid_business_unit_id.Value, spn.Value });
			lb3.Text = target.ToString();
			_total_target_margin += Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y
				+ " from fytarget where business_unit_id = "
				+ hid_business_unit_id.Value + " and target_name = 'Revenue' and fy = a.fy)) " +
													  "from fytarget a where a.business_unit_id = @v0 and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
			hid_business_unit_id.Value,spn.Value
			});
			double dolled = 0;
			if (cb.Text == "Revenue")
				{
				dolled = Toolbox.doSQL_double("Select ifnull((select sum(ifnull(fytarget_employees.m" + y
					+ ",0)) from fytarget_employees where fytarget_employees.target_name =@v0 and fytarget_employees.business_unit_id = @v1 and fytarget_employees.fy = @v2),0)", new object[] {
					cb.Text,hid_business_unit_id.Value,spn.Value
					});
				}
			else if (cb.Text == "Margin")
				{
				dolled = Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y
					+ " from fytarget_employees where business_unit_id = @v0 and target_name = 'Revenue' and member_id = a.member_id and fy = a.fy)) " +
											  "from fytarget_employees a where a.business_unit_id = @v0 and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					});
				uuu += Toolbox.doSQL_double("select ifnull((select sum(m" + y + "/100*(Select m" + y + " from fytarget_employees where business_unit_id = @v0 and target_name = 'Revenue' and member_id = a.member_id and fy = a.fy)) " +
											"from fytarget_employees a where a.business_unit_id = @v0 and a.target_name = 'Margin' and a.fy=@v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					});
				dolled = 100 * (dolled / Toolbox.doSQL_double("select ifnull((Select ifnull(fytarget.m" + y + ",0) " +
															  "from fytarget where fytarget.target_name = 'Revenue' and fytarget.business_unit_id = @v0 and fytarget.fy =@v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					}));
				yyy += Toolbox.doSQL_double("select ifnull((Select ifnull(fytarget.m" + y + ",0) from fytarget where fytarget.target_name = 'Revenue' and fytarget.business_unit_id =@v0 and fytarget.fy = @v1),0)", new object[] {
					hid_business_unit_id.Value,spn.Value
					});
				}

			lb2.Text = Convert.ToString(Math.Round(target - dolled, 1));
			if ((target - dolled > 0))
				{
				lb2.ForeColor = Color.Red;
				lb2.Font.Bold = true;
				}
			else
				{
				lb2.ForeColor = Color.White;
				lb2.Font.Bold = false;
				}
			y++;
			if (cb.Text == "Revenue")
				{
				ttt = ttt + (target - dolled);
				}
			else if (cb.Text == "Margin")
				{
				ttt = Math.Round(((_total_target_margin - uuu) / yyy) * 100, 1);
				}
			else if (cb.Text == "Days to Invoice")
				{
				lb2.ClientVisible = false;
				}
			}
		var lb5 = (ASPxLabel)gv_targets0.FindTitleTemplateControl("t0total");
		lb5.Text = ttt.ToString();
		if (ttt > 0)
			{
			lb5.ForeColor = Color.Red;
			lb5.Font.Bold = true;
			}
		else
			{
			lb5.ForeColor = Color.White;
			lb5.Font.Bold = true;
			}

		}
	protected void ASPxTextBox3_Init(object sender, EventArgs e)
		{
		var txtq = sender as ASPxTextBox;
		var container = txtq.NamingContainer as GridViewDataItemTemplateContainer;
		txtq.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_targets.PerformCallback('s|' + s.GetText()+ '|{0}|{1}'); }}", container.KeyValue, container.ID);
		}
	protected void ASPxTextBox03_Init(object sender, EventArgs e)
		{
		var txtq = sender as ASPxTextBox;
		var container = txtq.NamingContainer as GridViewDataItemTemplateContainer;
		txtq.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_targets0.PerformCallback('s|' + s.GetText()+ '|{0}|{1}'); }}", container.KeyValue, container.ID);
		}
	protected void gv_targets_ClientLayout(object sender, ASPxClientLayoutArgs e)
		{
		var comp = new NeBusinessUnit(Session["bu_id"]);


		int x;
		var y = 1;

		if (comp.yearend_month == 12)
			{
			x = 1;
			}
		else
			{
			x = comp.yearend_month + 1;
			}


		while (y <= 12)
			{
			var lb1 = (ASPxLabel)gv_targets.FindTitleTemplateControl("lb" + y);
			lb1.Text = GetMonthName(x);
			x = x + 1;
			if (x > 12)
				{
				x = 1;
				}
			y++;
			}

		}
	public static string GetMonthName(int month)
		{
		return new DateTime(2010, month, 1).ToString("MMMM");
		}
	protected void gv_targets_PreRender(object sender, EventArgs e)
		{
		var comp = new NeBusinessUnit(Session["bu_id"]);
		var se = (ASPxSpinEdit)gv_targets.FindTitleTemplateControl("spnfy");
		var cb = (ASPxComboBox)gv_targets.FindTitleTemplateControl("ddltarget");
		//		se.Value = Convert.ToInt32(System.DateTime.Today.Year);
		int x;
		var y = 1;
		if (comp.yearend_month == 12)
			{
			x = 1;
			}
		else
			{
			x = comp.yearend_month + 1;
			}
		while (y <= 12)
			{

			var lb1 = (ASPxLabel)gv_targets.FindTitleTemplateControl("lb" + y);
			lb1.Text = GetMonthName(x);

			gv_targets.Columns[y + 3].Caption = GetMonthName(x);
			x = x + 1;
			if (x > 12)
				{
				x = 1;
				}
			y++;
			}
		//	if ((!IsPostBack))
		//	{
		load_totals();
		//	}
		}

	protected void gv_targets_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
		{
		Toolbox.doSQL_void(@"Delete from fytarget  where id =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		gv_targets.CancelEdit();
		gv_targets.DataBind();

		}
	protected void gv_targets0_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var tb1 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb01");
		var tb2 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb02");
		var tb3 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb03");
		var tb4 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb04");
		var tb5 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb05");
		var tb6 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb06");
		var tb7 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb07");
		var tb8 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb08");
		var tb9 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb09");
		var tb10 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb010");
		var tb11 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb011");
		var tb12 = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb012");
		var cb = (ASPxComboBox)gv_targets0.FindTitleTemplateControl("ddltarget0");
		var spn = (ASPxSpinEdit)gv_targets0.FindTitleTemplateControl("spnfy0");
		if (e.Parameters[0].ToString() == "a")
			{
			var x = 1;
			var insert = "";
			while (x <= 12)
				{
				var tb = (ASPxTextBox)gv_targets0.FindTitleTemplateControl("tb0" + x);
				if ((tb.Text == null) || (tb.Text == ""))
					{
					throw new Exception("You must enter a valid target for Month " + x);
					}
				insert += tb.Text + ",";
				x++;
				}
			insert = insert.TrimEnd(',');

			var cb_member = (ASPxComboBox)gv_targets0.FindTitleTemplateControl("ddl_member");
			if (cb_member.Text == "")
				{
				throw new Exception("You must select a valid member");
				}

			if (cb.Text != "")
				{
				if (Toolbox.doSQL_int(@"Select count(id) from fytarget_employees  where fytarget_employees.member_id =@v0 and fytarget_employees.business_unit_id =@v1  and fytarget_employees.target_name =@v2  and fytarget_employees.fy =@v3 ", new object[] { cb_member.Value, hid_business_unit_id.Value, cb.Text, spn.Value }) == 0)
					{
					Toolbox.doSQL_void(string.Format(@"Insert into fytarget_employees (member_id,target_name,business_unit_id,fy,m1,m2,m3,m4,m5,m6,m7,m8,m9,m10,m11,m12)  values(@v0,@v1,@v2,@v3,{0})", insert), new object[] { cb_member.Value, cb.Text, hid_business_unit_id.Value, spn.Value });
					gv_targets0.DataBind();
					}
				else
					{
					Toolbox.doSQL_void(@"update fytarget_employees  set m1=@v0,m2=@v1 ,m3=@v2 ,m4=@v3 ,m5=@v4 ,m6=@v5 ,m7=@v6 ,m8=@v7 ,m9=@v8 ,m10=@v9 ,m11=@v10 ,m12=@v11   where fy =@v12 and business_unit_id=@v13  and member_id =@v14  and target_name =@v15 ", new object[] { tb1.Text, tb2.Text, tb3.Text, tb4.Text, tb5.Text, tb6.Text, tb7.Text, tb8.Text, tb9.Text, tb10.Text, tb11.Text, tb12.Text, spn.Value, hid_business_unit_id.Value, cb_member.Value, cb.Text });
					gv_targets0.DataBind();
					}
				}
			else
				{
				throw new Exception("You must select a valid target name");
				}
			load_totals0();

			}
		else if (e.Parameters[0].ToString() == "s")
			{
			var ee = e.Parameters.Split('|');
			var _value = ee.GetValue(3).ToString().Split('_');
			Toolbox.doSQL_void(string.Format(@"update fytarget_employees  set fytarget_employees.m{0} =@v0 where id =@v1", (Convert.ToInt32(_value.GetValue(1)) - 3)), new object[] { ee.GetValue(1), ee.GetValue(2) });
			load_totals0();
			}
		else if (e.Parameters[0].ToString() == "z")
			{
			if (cb.Text == "Margin")
				{
				tb1.Text = "50";
				tb2.Text = "50";
				tb3.Text = "50";
				tb4.Text = "50";
				tb5.Text = "50";
				tb6.Text = "50";
				tb7.Text = "50";
				tb8.Text = "50";
				tb9.Text = "50";
				tb10.Text = "50";
				tb11.Text = "50";
				tb12.Text = "50";
				}
			else if (cb.Text == "Revenue")
				{
				tb1.Text = "";
				tb2.Text = "";
				tb3.Text = "";
				tb4.Text = "";
				tb5.Text = "";
				tb6.Text = "";
				tb7.Text = "";
				tb8.Text = "";
				tb9.Text = "";
				tb10.Text = "";
				tb11.Text = "";
				tb12.Text = "";
				}
			else if (cb.Text == "Days to Invoice")
				{
				tb1.Text = "3";
				tb2.Text = "3";
				tb3.Text = "3";
				tb4.Text = "3";
				tb5.Text = "3";
				tb6.Text = "3";
				tb7.Text = "3";
				tb8.Text = "3";
				tb9.Text = "3";
				tb10.Text = "3";
				tb11.Text = "3";
				tb12.Text = "3";
				}
			load_totals0();

			}
		//gv_targets0.DataBind();

		}
	protected void gv_targets0_PreRender(object sender, EventArgs e)
		{
		var comp = new NeBusinessUnit(Session["bu_id"]);
		var se = (ASPxSpinEdit)gv_targets0.FindTitleTemplateControl("spnfy0");
		//	se.Value = Convert.ToInt32(System.DateTime.Today.Year);
		int x;
		var y = 1;
		if (comp.yearend_month == 12)
			{
			x = 1;
			}
		else
			{
			x = comp.yearend_month + 1;
			}
		while (y <= 12)
			{
			var lb1 = (ASPxLabel)gv_targets0.FindTitleTemplateControl("lb0" + y);
			lb1.Text = GetMonthName(x);
			gv_targets0.Columns[y + 3].Caption = GetMonthName(x);
			x = x + 1;
			if (x > 12)
				{
				x = 1;
				}
			y++;
			}
		//	if ((!IsPostBack))
		//	{
		load_totals0();
		//	}
		}
	protected void gv_targets0_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
		{
		Toolbox.doSQL_void(@"Delete from fytarget_employees  where id =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		gv_targets0.CancelEdit();
		gv_targets0.DataBind();
		}

	protected void gv_targets_PreRender0(object sender, EventArgs e)
		{

		}
	protected void ASPxGridView2_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var reg_rate = Convert.ToDouble(gv.GetRowValues(e.VisibleIndex, "Reg"));
		if (reg_rate == 0)
			{
			e.Row.Style["background-color"] = "#f00";
			e.Row.Style["color"] = "#fff";
			}

		}
	protected void gv_targets_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			if (e.DataColumn.FieldName == "_total")
				{
				if (gv_targets.GetRowValues(e.VisibleIndex, "target_name").ToString() == "Margin")
					{
					var fy = Convert.ToInt32(gv_targets.GetRowValues(e.VisibleIndex, "fy"));

					var revt = Toolbox.doSQL_double("Select ifnull((Select ifnull((m1+m2+m3+m4+m5+m6+m7+m8+m9+m10+m11+m12),0) " +
													"from fytarget where business_unit_id = @v0 and target_name = 'Revenue' and fy = @v1),0)", new object[] {
						hid_business_unit_id.Value,fy
						});
					var gmt = Toolbox.doSQL_double(@"
select ifnull((
Select m1/100*(Select m1 from fytarget where business_unit_id =@v0 and target_name = 'Revenue' and fy = a.fy) +
m2/100*(Select m2 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m3/100*(Select m3 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m4/100*(Select m4 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m5/100*(Select m5 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m6/100*(Select m6 from fytarget where business_unit_id=@v0 and target_name = 'Revenue' and fy = a.fy)  +
m7/100*(Select m7 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) + 
m8/100*(Select m8 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m9/100*(Select m9 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m10/100*(Select m10 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) + 
m11/100*(Select m11 from fytarget where business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m12/100*(Select m12 from fytarget where business_unit_id=@v0  and target_name = 'Revenue' and fy = a.fy)),0) gm 
from fytarget a where business_unit_id =@v0  and target_name = 'Margin' and fy =@v1 ", new object[] { hid_business_unit_id.Value, fy });
					e.Cell.Text = Convert.ToString(Math.Round((gmt / revt) * 100, 1));
					}
				else if (gv_targets.GetRowValues(e.VisibleIndex, "target_name").ToString() == "Days to Invoice")
					{
					e.Cell.Text = "";
					}
				}
			}
		}
	protected void spnfy1_ValueChanged(object sender, EventArgs e)
		{
		var spn0 = (ASPxSpinEdit)gv_targets0.FindTitleTemplateControl("spnfy0");
		var spn = (ASPxSpinEdit)gv_targets.FindTitleTemplateControl("spnfy");
		spn.Value = spnfy1.Value;
		spn0.Value = spnfy1.Value;
		load_totals();
		load_totals0();
		}
	protected void spnfy_Init(object sender, EventArgs e)
		{
		var spn0 = (ASPxSpinEdit)sender;
		spn0.Value = spnfy1.Value;
		}
	protected void spnfy0_Init(object sender, EventArgs e)
		{
		var spn0 = (ASPxSpinEdit)sender;
		spn0.Value = spnfy1.Value;
		}
	protected void gv_targets0_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			if (e.DataColumn.FieldName == "_total")
				{
				if (gv_targets0.GetRowValues(e.VisibleIndex, "target_name").ToString() == "Margin")
					{
					var fy = Convert.ToInt32(gv_targets0.GetRowValues(e.VisibleIndex, "fy"));
					var mid = Convert.ToInt32(gv_targets0.GetRowValues(e.VisibleIndex, "member_id"));
					var revt = Toolbox.doSQL_double("select ifnull((Select ifnull((m1+m2+m3+m4+m5+m6+m7+m8+m9+m10+m11+m12),0) " +
													"from fytarget_employees where member_id =@v0 and business_unit_id = @v1 and target_name = 'Revenue' and fy = @v2),0)", new object[] {
						mid,hid_business_unit_id.Value,fy
						});
					var gmt = Toolbox.doSQL_double(@"
select ifnull((
Select m1/100*(Select m1 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0 and target_name = 'Revenue' and fy = a.fy) +
m2/100*(Select m2 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m3/100*(Select m3 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m4/100*(Select m4 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m5/100*(Select m5 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) +
m6/100*(Select m6 from fytarget_employees  where member_id = a.member_id and business_unit_id=@v0 and target_name = 'Revenue' and fy = a.fy)  +
m7/100*(Select m7 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) + 
m8/100*(Select m8 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m9/100*(Select m9 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m10/100*(Select m10 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy) + 
m11/100*(Select m11 from fytarget_employees  where member_id = a.member_id and business_unit_id =@v0  and target_name = 'Revenue' and fy = a.fy)  +
m12/100*(Select m12 from fytarget_employees  where member_id = a.member_id and business_unit_id=@v0  and target_name = 'Revenue' and fy = a.fy)),0) gm 
from fytarget_employees a where business_unit_id =@v0  and target_name = 'Margin' and fy =@v1  and a.member_id =@v2", new object[] { hid_business_unit_id.Value, fy, mid });


					e.Cell.Text = Convert.ToString(Math.Round((gmt / revt) * 100, 1));
					}
				else if (gv_targets0.GetRowValues(e.VisibleIndex, "target_name").ToString() == "Days to Invoice")
					{
					e.Cell.Text = "";
					}
				}
			}
		}
	protected void gv_targets_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			if (e.GetValue("target_name").ToString() == "Margin")
				{
				e.Row.BackColor = ColorTranslator.FromHtml("#FFFFB7");
				}
			else if (e.GetValue("target_name").ToString() == "Revenue")
				{
				e.Row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
				}
			}
		}
	protected void gv_targets0_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{

		if (e.VisibleIndex >= 0)
			{
			if (e.GetValue("target_name").ToString() == "Margin")
				{
				e.Row.BackColor = ColorTranslator.FromHtml("#FFFFB7");
				}
			else if (e.GetValue("target_name").ToString() == "Revenue")
				{
				e.Row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
				}
			}

		}
	protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
		{
		if (ASPxPageControl1.ActiveTabIndex == 6)
			{
			load_org();
			}
		}


	}




