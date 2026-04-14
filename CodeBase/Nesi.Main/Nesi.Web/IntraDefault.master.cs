using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using nesi.core;
// ReSharper disable InconsistentNaming

public partial class IntraDefault : MasterPage
{
    NeMember myMember;

    public bool is_mobile { get; set; }
    ne_page_log pl = new ne_page_log();
    public string page_name { get; set; }
    public bool show_branch_selector { get; set; }
    public DataTable ddl_branch_selector_ds { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        var _q = Page.Request.QueryString;
        if (Session["session"] != null)
        {
            myMember = new NeMember(Session["session"].ToString());

            
               
            

            pl.ip_address = Request.UserHostAddress;
            pl.member_id = myMember.id;
            pl.url = Request.Url.AbsolutePath;
            pl.host = Toolbox.GetSubDomain(Request.Url);
            pl.query_string = Request.Url.Query;
            pl.request_start = DateTime.Now;

            pl.save();


        }
            js_functions_block.Controls.Add(new Literal
            {
                Text = "<script type ='text/javascript' src = '/js/functions.js?id=" + Toolbox.do_RandomString(5) + "'></script>"
            });

        is_mobile = !string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1";

        if (is_mobile)
        {
            page_heading.Visible = false;
            mobile_header_tags.Visible = true;
        }
        ico.Href = "/images/Logos/" + new Current_User().logo_icon;

       
            page_heading.Visible = false;
            mobile_header_tags.Visible = false;
            page_heading.Visible = true;
        
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        pl.request_end = DateTime.Now;

        pl.save();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        using (var conn = Toolbox.connect())
        {
            var _q = Page.Request.QueryString;
            if (!string.IsNullOrEmpty(page_name))
            {
                lblHeading.Text = page_name;
            }
            if (!string.IsNullOrEmpty(_q["page_id"]))
            {
                int page_id;
                int.TryParse(_q["page_id"], out page_id);
                if (page_id > 0)
                {
                    lblHeading.Text = NePage.get_page_name(page_id);
                }
                var invalidDefaultPages = Toolbox.doSQL_int(@"SELECT IFNULL((SELECT page_id FROM page WHERE menu_type IN(4, 5) AND page_id = @v0), 0)", new object[] { page_id });
                if (invalidDefaultPages != 0)
                {
                    hl_default.Visible = false;
                }


            }


            cb_defaultpage.JSProperties["cp_page"] = "1";


            if (Session["session"] != null)
            {
                welcomebox(conn);
            }
            if (myMember != null && myMember.isContact)
            {
                cb_defaultpage.Visible = false;
            }
            if (!show_branch_selector || Session["session"] == null || myMember == null) return;

        }
    }
    public void welcomebox(MySqlConnection _conn)
    {
        myMember = new NeMember(Session["session"].ToString());
        Toolbox.do_add_css(Page, "/css/jquery.tip.css");
        Toolbox.do_add_css(Page, "/css/jquery_custom_mods.css");




        if (myMember.isContact)
        {

        }
        else
        {
            //var name_select = new StringBuilder();
            var authenticated_for_masq = (bool?)Session["masq"] ?? myMember.AuthenticatedForPrivilege(168);
            var n_mapped_users = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM user_switch WHERE member_id = @v0",
                new object[] {
                    Session["orig_masq_user"] == null ? myMember.id : Session["orig_masq_user"]});
            if (authenticated_for_masq || n_mapped_users > 0)
            {
                // Set masq variable
                if (authenticated_for_masq)
                {
                    if (Session["masq"] == null)
                    {
                        Session["masq"] = authenticated_for_masq;
                        Session["orig_masq_user"] = myMember.id;
                    }
                }
                else if (n_mapped_users > 0)
                {
                    if (Session["orig_masq_user"] == null)
                    {
                        Session["orig_masq_user"] = myMember.id;
                    }
                }
                /*
                    //var _users = authenticated_for_masq
                    //							? Toolbox.doSQL_dt(_conn,@"SELECT b.name bu_name, a.member_id id, a.member_fullname name FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id  WHERE a.member_status = 'Active' ORDER BY a.business_unit_id, a.member_fullname" , null)
                    //							: Toolbox.doSQL_dt(_conn,@" SELECT c.id, c.name bu_name, a.mapped_member_id id, b.member_fullname name FROM user_switch a LEFT JOIN member b ON a.mapped_member_id = b.member_id LEFT JOIN business_unit c ON c.business_unit_id = cid WHERE a.member_id = @v0  UNION SELECT b.id, b.name bu_name, a.member_id id, a.member_fullname name FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_id = @v0  ORDER BY business_unit_id, name", new object[] {  Session["orig_masq_user"] } );
                    var prev_company = "";
                    foreach (DataRow dr in _users.Rows)
                    {
                        var _this_name = dr["bu_name"].ToString();
                        var _this_member_id = Convert.ToInt32(dr["id"]);
                        var _this_member_name = dr["name"].ToString();
                        if (prev_company != _this_name)
                        {
                            if (prev_company != "")
                            {
                            //	name_select.Append("</optgroup>");
                            }
                        //	name_select.AppendFormat("<optgroup label='{0}'>", _this_name);
                        //	prev_company = _this_name;
                        }
                        var selected = myMember.id == _this_member_id ? " selected" : "";
                        name_select.AppendFormat("<option value='{0}' {1}>{2}</option>", _this_member_id, selected, _this_member_name);
                    }
            if ((int)Session["orig_masq_user"] != myMember.id)
                {
                //	name_select.AppendFormat("</optgroup></select><img onclick='switch_active_user(this, true)' data-main_id='{0}' src='/images/icon/icon[cancel].png' width='16' height='16' align='absmiddle' style='cursor:pointer;margin:2px' title='Cancel masquerade' />", Session["orig_masq_user"]);
                }
                else
                {
                    //name_select.Append("</optgroup></select>");
                }
            }
            else
            {
            //	name_select.AppendFormat(@"<a title='Update / View your employee information' onclick=""boing('/sections/hr/member/membercontactdisplay.aspx?MemID={0}', 'memberinfo', 850,650);"" href=""javascript:void(0);"">{1}</a>", myMember.id, memname);
            }

            
            */
            }
        }
    }

    protected void cb_defaultpage_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        var page_id = Page.Request.QueryString["page_id"] == null ? "" : Page.Request.QueryString["page_id"];
        if (page_id != "")
        {
            Toolbox.doSQL_void(@"update member set member_default_page = @v0  where member_id =@v1 ", new object[] {
                    page_id, myMember.id});
        }
    }

}

