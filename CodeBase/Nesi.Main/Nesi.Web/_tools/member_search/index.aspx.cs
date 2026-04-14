using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class member_search : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 1; // from Page table in DB
    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        var func = new this_functions();
        DataTable _dt;
        var _q = Request.QueryString;
        _tools.set_XML_header();
        if (_q["q"] != null)
        {
            _dt = func.search_results(_q["q"]);
            _dt.TableName = "MEMBER";
            _dt.WriteXml(Response.OutputStream);
        }
        else
        {
            Response.Write("<error>Query not provided</error>");
        }
        Response.End();
    }


    public class this_functions
    {
        string sql = "";
        Toolbox _tools = new Toolbox();

        public DataTable search_results(string q)
        {
            var appendature = "";
            if (q.Contains(" "))
            {
                var qs = q.Split(' ');
                for (var i = 0; i < qs.Length; i++)
                {
                    var this_val = qs[i].Trim();

                    if (i == qs.Length - 1)
                    {
                        appendature += "_searchable LIKE '%" + this_val + "%'";
                    }
                    else
                    {
                        appendature += "_searchable LIKE '%" + this_val + "%' AND ";
                    }
                }
            }
            else
            {
                appendature = " _searchable LIKE '%" + q + "%'";
            }
            sql = @"
SELECT 
	id,
	name_first,
	name_last,
	email,
	username,
	password,
	name_company,
	member_type,
	status 
FROM 
	(
	SELECT
		a.member_id id,
		a.member_nickname name_first,
		a.member_lastname name_last,
		a.member_user username,
		if(a.member_neemail = 'nomail@newelectric.ca', '', a.member_neemail) email,
		a.member_pass password,
		b.name name_company,
		c.membertype_name member_type,
		if(a.member_status = 'Not Active', 'N/A' ,a.member_status) status,
		cast(group_concat(a.member_id,' ', a.member_nickname,' ', a.member_lastname,' ', a.member_user,' ', if(a.member_status = 'Not Active', 'N/A' ,a.member_status),' ', b.name,' ', c.membertype_name) as char) _searchable 
	FROM 
		member a
	LEFT JOIN
		business_unit b ON a.business_unit_id = b.id
	LEFT JOIN
		membertype c ON a.member_membertype_id = c.membertype_id
	GROUP BY a.member_id
	) sub 
WHERE 
	" + appendature + @"
ORDER BY name_last, name_first
";
            return _tools.getSQL_datatable(sql, null);
        }
    }
}