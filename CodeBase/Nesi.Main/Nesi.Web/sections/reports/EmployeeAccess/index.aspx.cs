using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;
using System.Linq;

public partial class sections_reports_employee_access_index : Page
	{
    int _page_id = 107;
        NeMember myMember;
        Toolbox _tools;
        //private const string _hr_id = "3"; //From Page Table in DB  Employees Page
	private int id;
 
        protected void Page_Load(object sender, EventArgs e)
        {
            _tools = new Toolbox();
            	myMember = Toolbox.do_handle_authentication(_page_id);
		    _tools.dont_cache_page();
            if (!IsPostBack)
            {
                var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
                divMenu.InnerHtml = menu.MenuHTML;
               
                //pnlView.Visible = false;
                hidMemberID.Value = myMember.id.ToString();
                ViewState["memberid"] = hidMemberID.Value;
	            id = Convert.ToInt32(ViewState["memberid"]);
                var editing_member = new NeMember(id);



                ViewState["CompanyID"] = editing_member.business_unit_id;
	            ddlcompany.DataSource = NeBusinessUnit.units_active();
                ddlcompany.DataTextField = "name";
                ddlcompany.DataValueField = "id";
                ddlcompany.DataBind();
                var li = new ListItem("All Companies", "99");
                ddlcompany.Items.Add(new ListItem(li.Text, li.Value));
                ddlcompany.SelectedValue = editing_member.business_unit_id.ToString();
	            lblPrivileges.Text = editing_member.FullName + " is a membertype: " +
	                                 _tools.getSQL_string(@"Select membertype_name from membertype  where membertype_id =@v0", new object[] { editing_member.MemberTypeID }) + ". The highlighted rows indicate where " + editing_member.FirstName + " deviates from their membertype default settings.";

                ddlview.Items.Clear();
                ddlview.ClearSelection();
                li.Text = "View membertype templates";
                li.Value = "0";
                ddlview.Items.Add(new ListItem(li.Text, li.Value));
                li.Text = "View " + editing_member.FullName;
                li.Value = "1";
                ddlview.Items.Add(new ListItem(li.Text, li.Value));
                ddlview.SelectedIndex = 1;
                //          li.Text = "view membertype templates";
                //          li.Value = "1";
                GeneratePrivileges();


            }

            /*        if (ddlview.SelectedValue == "1")
                    {
                        GeneratePriveleges();
                    }
                    else
                    {
                        GenerateTypePriveleges();
                    }
             */
        }


        protected void load_membertype_ddl()
        {


            DataTable dt2;
            if (ddlcompany.SelectedValue != "99")
            {
                dt2 = _tools.getSQL_datatable(@"Select member_fullname, membertype_name, business_unit.name from member,business_unit,membertype  where member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and member_membertype_id =@v1  order by business_unit.name, member_nickname", new object[] { ddlcompany.SelectedValue,ddlMemberType.SelectedValue });
            }
            else
            {
                dt2 = _tools.getSQL_datatable(@"Select member_fullname, membertype_name, business_unit.name from member,business_unit,membertype  where member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and member_membertype_id =@v0 order by business_unit.name, member_nickname", new object[] { ddlMemberType.SelectedValue });
            }
            var _list_of_members2 = "<table>";
            var company2 = "";
            foreach (DataRow dr2 in dt2.Rows)
            {
                if (dr2[2].ToString() != company2)
                {
                    company2 = dr2[2].ToString();
                    _list_of_members2 += "<tr><td>" + dr2[2] + "</td>";
                }
                else
                {
                    _list_of_members2 += "<tr><td></td>";
                }
                _list_of_members2 += "<td>" + dr2[0] + "</td><td>" + dr2[1] + "</td></tr>";
            }
            _list_of_members2 += "</table>";



            divmembertype.InnerHtml = "<span width: '200px'; class='opt1' data-tooltip='" + _list_of_members2 + "'> Members of this Member Type (put mouse here) </span>";


        }
	
	protected void GeneratePrivileges()
		{
		var sb		= new StringBuilder();
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Member Page Privileges</div>");
		var memEdit = myMember;
		var TypePriv = new NEMemberTypePrivilege();
		var strPageSQL = "";
		var strPrivSQL = "";


		TypePriv.TypePages(Convert.ToInt32(memEdit.MemberTypeID.ToString()));
		TypePriv.TypePrivileges(Convert.ToInt32(memEdit.MemberTypeID.ToString()));

		var dsPriv = new DataSet();
		var dtPage = Toolbox.doSQL_dt(@"SELECT page_id,page_parent_id,page_name,page_desc,page_scriptpath,page_order FROM page"  , null);
		dtPage.TableName = "Page";
		var dtPriv = Toolbox.doSQL_dt(@"SELECT privilege_id,privilege_page_id,privilege_name,privilege_desc FROM privilege"  , null);
		dtPriv.TableName = "Privilege";
		dsPriv.Tables.Add(dtPage);
		dsPriv.Tables.Add(dtPriv);
		//string strMap = "\r<ul>";
		GetChildren(0, ref sb, ref dsPriv, ref memEdit, ref TypePriv);
		divSiteMap.InnerHtml = sb.ToString();

		/*string strMemberSQL = "SELECT Member_FirstName,Member_LastName FROM Member"
							+ " WHERE  Member_ID =" + hidMemberID.Value;
		OdbcCommand comMember = new OdbcCommand(strMemberSQL, conn);
		OdbcDataReader drMember = comMember.ExecuteReader();
		drMember.Read();
		spanMemberName.InnerHtml = drMember["Member_FirstName"].ToString() + " " + drMember["Member_LastName"].ToString();*/
		

		}
	private void GetChildren(int parent_id, ref StringBuilder sb, ref DataSet ds, ref NeMember memEdit, ref NEMemberTypePrivilege TypePriv)
		{
		sb.Append("<ul>");

		foreach (DataRow row in ds.Tables[0].Rows)
			{
			var page_id			= Convert.ToInt32(row["page_id"]);
			var page_name	= row["page_name"].ToString();
			var page_desc	= row["page_desc"].ToString();
			var page_parent_id	= Convert.ToInt32(row["page_parent_id"]);

			if (parent_id == page_parent_id)
				{
				var strChecked = "";

				//Nic: sep 1st, 2011
				var strStyle = "";

				// check to see if page is in mem object
				if (memEdit.AuthenticatedForPage(page_id))
					{
					strChecked = "CHECKED";
					}

				//Nic: sep 1st, 2011
				if (TypePriv.AuthenticatedForTypePage(page_id))
					{
					strStyle = strChecked != "CHECKED" ? "style='background-color:#F4FA58;'" : "";
					}
				else
					{
					strStyle = strChecked == "CHECKED" ? "style='background-color:#F4FA58;'" : "";
					}

				DataTable dt;

				if (ddlcompany.SelectedValue != "99")
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpage_page_id =@v1  order by business_unit.name, member_firstname", new object[] { ddlcompany.SelectedValue,page_id });
					}
				else
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpage_page_id =@v0 order by business_unit.name, member_firstname", new object[] { page_id });
					}

				var _list_of_members = new StringBuilder();
				_list_of_members.Append(@"<table width='100%' style='align:left;'>");
				var name = "";
				foreach (DataRow dr in dt.Rows)
					{
					var member_name			= (string) dr["name"];
					var membertype_name		= (string) dr["membertype_name"];
					var this_name	= (string) dr["name"];
					if (this_name != name)
						{
						name = this_name;
						_list_of_members.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", name);
						}
					_list_of_members.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
					}
				_list_of_members.Append(@"</table>");

				var chkname		= "!page_" + page_id + "_" + parent_id;
				var onclick		= ds.Tables["Privilege"].Select("privilege_page_id = "+page_id).Any() || ds.Tables["Page"].Select("page_parent_id = "+page_id).Any()
										? @"data-expanded='false' style=""list-style-image:url('/images/icon/icon[minus].gif');"" onclick='toggle_children(this, event)'"
										: @" style=""list-style-image:url('/images/pixel.gif');{0}"" ";
				sb.AppendFormat(@"
<li class='page' style=""list-style-image:url('/images/pixel.gif');"">
		<span class='opt1' data-title='FALSE' {7} data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);' disabled='disabled'><label for='{1}'><b> ({3}) {4}:</b> <i style='font-size:11px;color:#777;'>{5}</i></label></span>", 
				_list_of_members, 
				chkname, 
				strChecked, 
				page_id, 
				page_name, 
				page_desc,
				onclick,
				strStyle
				);
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("<ul>");
					}

				foreach (DataRow prow in ds.Tables["Privilege"].Rows)
					{
					#region privileges
					var privilege_id				= Convert.ToInt32(prow["privilege_id"]);
					var privilege_name			= prow["privilege_name"].ToString();
					var privilege_description	= prow["privilege_desc"].ToString();
					var privilege_page_id			= Convert.ToInt32(prow["privilege_page_id"]);
					if (privilege_page_id == page_id)
						{
						var strCheckedp = "";

						var strStylep = "";

						if (memEdit.AuthenticatedForPrivilege(privilege_id))
							{
							strCheckedp = "CHECKED";
							}

						if (TypePriv.AuthenticatedForTypePrivilege(privilege_id))
							{
							strStylep			= strCheckedp != "CHECKED" ? "background-color:#F4FA58;" : "";
							}
						else
							{
							strStylep			= strCheckedp == "CHECKED" ? "background-color:#F4FA58;" : "";
							}

						DataTable dt2;
						if (ddlcompany.SelectedValue != "99")
							{
							dt2 = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpageprivilege_privilege_id =@v1  order by business_unit.name, member_firstname", new object[] { ddlcompany.SelectedValue,privilege_id });
							}
						else
							{
							dt2 = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpageprivilege_privilege_id =@v0 order by business_unit.name, member_firstname", new object[] { privilege_id });
							}	
						var _list_of_members2 = new StringBuilder();
						_list_of_members2.Append(@"<table width='100%' style='align:left;'>");
						var sub_name = "";
						foreach (DataRow dr2 in dt2.Rows)
							{
							var member_name			= (string) dr2["name"];
							var membertype_name		= (string) dr2["membertype_name"];
							var this_name	= (string) dr2["name"];
							if (this_name != sub_name)
								{
								sub_name = this_name;
								_list_of_members2.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", sub_name);
								}
							_list_of_members2.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
							}
						_list_of_members2.Append(@"</table>");
						var chknamep = string.Format(@"!priv_{0}_{1}", privilege_id, page_id);
						
						sb.AppendFormat(@"
<li class='priv opt1' style=""list-style-image:url('/images/pixel.gif');{6}""  data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);' disabled='disabled'><label for='{1}'><b>({5}) {3}</b> <i style='font-size:11px;color:#777;'>{4}</i></label>", 
						_list_of_members2, 
						chknamep, 
						strCheckedp, 
						privilege_name, 
						privilege_description,
						privilege_id,
						strStylep
						);
						}
					#endregion privileges
					}
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("</ul>");
					}
				GetChildren(page_id, ref sb, ref ds, ref memEdit, ref TypePriv);
				sb.Append("</li>");
				}
			//  strConcat += "</ul>";
			}
		sb.Append("</ul>");
		}

	


        protected void ddlMemberType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPrivileges.Visible = false;

            load_membertype_ddl();
            GenerateTypePrivileges();
        }
	
	protected void GenerateTypePrivileges()
		{
		var TypePriv = new NEMemberTypePrivilege();
		TypePriv.TypePages(Convert.ToInt32(ddlMemberType.SelectedValue));
		TypePriv.TypePrivileges(Convert.ToInt32(ddlMemberType.SelectedValue));

		var dsTypePriv = new DataSet();
		var dtPage = Toolbox.doSQL_dt(@"SELECT page_id,page_parent_id,page_name,page_desc,page_scriptpath,page_order FROM page"  , null);
		dtPage.TableName = "Page";
		var dtPriv = Toolbox.doSQL_dt(@"SELECT privilege_id,privilege_page_id,privilege_name,privilege_desc FROM privilege"  , null);
		dtPriv.TableName = "Privilege";
		dsTypePriv.Tables.Add(dtPage);
		dsTypePriv.Tables.Add(dtPriv);
		var sb		= new StringBuilder();
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Membertype Page Privileges</div>");
		GetTypeChildren(0, ref sb, ref dsTypePriv, ref TypePriv);

		divSiteMap.InnerHtml = sb.ToString();

		}
	
	private void GetTypeChildren(int parent_id, ref StringBuilder sb, ref DataSet ds, ref NEMemberTypePrivilege TypePriv)
		{
		sb.Append("\n<ul>");
		foreach (DataRow row in ds.Tables[0].Rows)
			{
			var page_id			= Convert.ToInt32(row["page_id"]);
			var page_name	= row["page_name"].ToString();
			var page_desc	= row["page_desc"].ToString();
			var page_parent_id	= Convert.ToInt32(row["page_parent_id"]);
			if (parent_id == page_parent_id)
				{
				var strChecked = "";
				// check to see if page is in mem object
				if (TypePriv.AuthenticatedForTypePage(page_id))
					{
					strChecked = "CHECKED";
					}

				DataTable dt;
				if (ddlcompany.SelectedValue != "99")
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpage_page_id =@v1  order by business_unit.name, member_firstname", new object[] { ddlcompany.SelectedValue,page_id });
					}
				else
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, business_unit.name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpage_page_id =@v0 order by business_unit.name, member_firstname", new object[] { page_id });
					} 
				var _list_of_members = new StringBuilder();
				_list_of_members.Append(@"<table width='100%'>");
				var name = "";
				foreach (DataRow dr in dt.Rows)
					{
					var member_name			= (string) dr["name"];
					var membertype_name		= (string) dr["membertype_name"];
					var this_name	= (string) dr["name"];
					if (this_name != name)
						{
						name = this_name;
						_list_of_members.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", name);
						}
					_list_of_members.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
					}
				_list_of_members.Append(@"</table>");

				var chkname		= "!page_" + page_id + "_" + parent_id;
				var onclick		= ds.Tables["Privilege"].Select("privilege_page_id = "+page_id).Any() || ds.Tables["Page"].Select("page_parent_id = "+page_id).Any()
										? @"data-expanded='false' style=""list-style-image:url('/images/icon/icon[minus].gif')"" onclick='toggle_children(this, event)'" 
										: @" style=""list-style-image:url('/images/pixel.gif')"" ";
				sb.AppendFormat(@"
<li class='page' {6}>
		<span class='opt1' data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);' disabled='disabled'><label for='{1}'><b> ({3}) {4}:</b> <i style='font-size:11px;color:#777;'>{5}</i></label></span>", 
				_list_of_members, 
				chkname, 
				strChecked, 
				page_id, 
				page_name, 
				page_desc,
				onclick);
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("<ul>");
					}
				foreach (DataRow prow in ds.Tables["Privilege"].Rows)
					{
					#region privileges
					var privilege_id				= Convert.ToInt32(prow["privilege_id"]);
					var privilege_name			= prow["privilege_name"].ToString();
					var privilege_description	= prow["privilege_desc"].ToString();
					var privilege_page_id			= Convert.ToInt32(prow["privilege_page_id"]);
					if (privilege_page_id == page_id)
						{

						DataTable dt2;
						if (ddlcompany.SelectedValue != "99")
							{
							dt2 = _tools.getSQL_datatable(@"SELECT member_fullname name, membertype_name, business_unit.name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpageprivilege_privilege_id =@v1  order by business_unit.name, member_firstname", new object[] { ddlcompany.SelectedValue,prow["Privilege_ID"] });
							}
						else
							{
							dt2 = _tools.getSQL_datatable(@"SELECT member_fullname name, membertype_name, business_unit.name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpageprivilege_privilege_id =@v0 order by business_unit.name, member_firstname", new object[] { privilege_id });
							}
						var _list_of_members2 = new StringBuilder();
						_list_of_members2.Append(@"<table width='100%'>");
						var sub_name = "";
						foreach (DataRow dr2 in dt2.Rows)
							{
							var member_name			= (string) dr2["name"];
							var membertype_name		= (string) dr2["membertype_name"];
							var this_name	= (string) dr2["name"];
							if (this_name != sub_name)
								{
								sub_name = this_name;
								_list_of_members2.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", sub_name);
								}
							_list_of_members2.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
							}
						_list_of_members2.Append(@"</table>");

						var strCheckedp = "";
						// check to see if page is in mem object
						if (TypePriv.AuthenticatedForTypePrivilege(privilege_id))
							{
							strCheckedp = "CHECKED";
							}
						var chknamep = string.Format(@"!priv_{0}_{1}", privilege_id, page_id);
						sb.AppendFormat(@"
<li class='priv opt1' style=""list-style-image:url('/images/pixel.gif')""  data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);' disabled='disabled'><label for='{1}'><b>({5}) {3}</b> <i style='font-size:11px;color:#777;'>{4}</i></label>", 
						_list_of_members2, 
						chknamep, 
						strCheckedp, 
						privilege_name, 
						privilege_description,
						privilege_id
						);
						}
					#endregion privileges
					}
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("</ul>");
					}
				GetTypeChildren(page_id, ref sb, ref ds, ref TypePriv);
				sb.Append("</li>");
				}
			}
		sb.Append("</ul>");
		}


        protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlview.SelectedValue == "1")
            {
                GeneratePrivileges();
            }
            else
            {
                GenerateTypePrivileges();
                load_membertype_ddl();
            }

        }
        protected void ddlview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlview.SelectedValue == "0")
            {
                ddlMemberType.Visible = true;
                var type = new NeMemberType();
                var typelist = new ArrayList();

                typelist = type.LoadMemberList();
                ddlMemberType.DataSource = typelist;
                ddlMemberType.DataBind();

                load_membertype_ddl();

                lblPrivileges.Visible = false;
                GenerateTypePrivileges();
                divmembertype.Visible = true;

            }
            else
            {
                //pnlView.Visible = false;
                var editting_member = new NeMember(Convert.ToInt32(hidMemberID.Value));
	            lblPrivileges.Text = editting_member.FullName + " is a membertype: " +
	                                 _tools.getSQL_string(@"Select membertype_name from membertype  where membertype_id =@v0", new object[] { editting_member.MemberTypeID }) + ". The highlighted rows indicate where " + editting_member.FirstName + " deviates from their membertype default settings.";
                ddlMemberType.Visible = false;
                divmembertype.Visible = false;
                GeneratePrivileges();
            }
        }
}
