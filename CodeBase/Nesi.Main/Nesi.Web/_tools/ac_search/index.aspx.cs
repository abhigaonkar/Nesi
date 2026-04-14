using System;
using System.Data;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
using nesi.core;

public partial class this_customer_search : System.Web.UI.Page
{
    NeMember _member;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        var func = new this_functions();
        if (Session["working_business_unit_id"] != null)
        {
            func.working_business_unit_id = Convert.ToInt32(Session["working_business_unit_id"]);
        }

        func._member = _member;
        func.handle_query();
    }


    public class this_functions
    {
        #region variable declaration

        Toolbox _tools = new Toolbox();
        private HttpRequest _req = HttpContext.Current.Request;
        private HttpResponse _resp = HttpContext.Current.Response;
        public NeMember _member = null;
        public int working_business_unit_id = 0;
        DataTable _dt = new DataTable();
        string q, _type = "";

        #endregion variable declaration

        public void handle_query()
        {
            _resp.Clear();
            _resp.ClearContent();
            _resp.ClearHeaders();
            _resp.ContentType = "text/plain";
            _tools.dont_cache_page();
            var _q = _req.QueryString;
            var cansee_homephone = _member.AuthenticatedForPrivilege(33) || _member.is_backoffice;
            try
            {
                if (_q["q"] != null && _q["type"] != null)
                {
                    q = _q["q"];
                    _type = _q["type"];
                    switch (_type)
                    {
                        #region company

                        case "company":
                            _dt = _tools.getSQL_datatable(
                                @" SELECT id, name, '' number, '' matches FROM business_unit WHERE name LIKE CONCAT('%',@v0,'%')",
                                new object[] { q });
                            break;

                        #endregion company

                        #region competitor

                        case "competitor":
                            _dt = _tools.getSQL_datatable(
                                @" SELECT id, name, '' number, '' matches FROM quote_competitor WHERE name LIKE CONCAT('%',@v0,'%')",
                                new object[] { q });
                            break;

                        #endregion competitor

                        #region customer

                        case "customer":
                            _dt = _tools.getSQL_datatable(
                                @" SELECT a.customer_id id, UPPER(a.customer_name) name, customer_hold, customer_number, '' number, '' matches FROM customer a LEFT JOIN address b ON b.address_table = 'Customer' AND a.customer_id = b.address_table_id LEFT JOIN customer_sales_properties c ON c.address_id = b.address_id WHERE (a.customer_id LIKE CONCAT('%',@v0,'%') OR a.customer_name LIKE CONCAT('%',@v0,'%')) AND c.status_id != 4 ORDER BY name",
                                new object[] { q });
                            break;

                        #endregion customer

                        #region customer_bv

                        case "customer_bv":
                            _dt = _tools.getSQL_datatable(
                                @" SELECT a.customer_number id, upper(a.customer_name) name, '' number, '' matches FROM customer a WHERE a.customer_number LIKE CONCAT('%',@v0,'%') OR a.customer_name like CONCAT('%',@v0,'%') ORDER BY name",
                                new object[] { q });
                            break;

                        #endregion customer_bv

                        #region inventory_tag

                        case "inventory_tag":
                            _dt = _tools.getSQL_datatable(
                                @" SELECT a.tag_id id, a.tag name, '' number, '' matches FROM inventory_tag a LEFT JOIN inventory_tag_tradename b ON a.tag_id = b.tag_id WHERE a.approved = true AND a.include_search = 1 AND ( a.tag LIKE CONCAT('%',@v0,'%') OR a.tag LIKE CONCAT(@v0,'%') OR b.tradename LIKE CONCAT('%',@v0,'%') OR b.tradename LIKE CONCAT(@v0,'%')) GROUP BY name ORDER BY name",
                                new object[] { q });
                            break;

                        #endregion inventory_tag

                        #region member

                        case "member":
                            var where_clause = new StringBuilder();
                            var strip_chars = new Regex("[-(,)\"']");
                            q = strip_chars.Replace(q, " ").Trim();
                            if (q.Contains(" "))
                            {
                                var qs = q.Split(' ');
                                foreach (var q_i in qs)
                                {
                                    where_clause.AppendFormat(@"
	(
	a.member_firstname LIKE ""%{0}%"" OR 
	a.member_lastname LIKE ""%{0}%"" OR
	a.member_nickname LIKE ""%{0}%"" OR 
	a.member_id LIKE ""%{0}%"" OR 
	a.member_phoneextension LIKE ""%{0}%"" OR
	a.member_necellareacode LIKE ""%{0}%"" OR 
	a.member_necellphonefirst LIKE ""%{0}%"" OR 
	a.member_necellphonelast LIKE ""%{0}%"" OR
	b.name LIKE ""%{0}%""
	) AND", Toolbox.AddSlashes(q_i));
                                }
                            }
                            else
                            {
                                where_clause.AppendFormat(@"
	(
	a.member_firstname LIKE ""%{0}%"" OR 
	a.member_lastname LIKE ""%{0}%"" OR
	a.member_nickname LIKE ""%{0}%"" OR 
	a.member_id LIKE ""%{0}%"" OR
	a.member_phoneextension LIKE ""%{0}%"" OR 
	a.member_necellareacode LIKE ""%{0}%"" OR 
	a.member_necellphonefirst LIKE ""%{0}%"" OR 
	a.member_necellphonelast LIKE ""%{0}%"" OR
	b.name LIKE ""%{0}%""
	) AND ", Toolbox.AddSlashes(q));
                            }

                            var active_search = cansee_homephone ? "true = true" : "a.member_status = 'Active'";
                            _dt = _tools.getSQL_datatable(string.Format(@"
SELECT 
	a.member_id id,
	a.member_fullname name, 
	IF(	c.number IS NOT NULL, 
		CONCAT(	c.number, 
				IF(member_phoneextension != '', 
					CONCAT(' x', member_phoneextension), 
					'')
				), 
		IF(member_phoneextension != '', CONCAT(' x', member_phoneextension), ' No Phone')
		) number, 
	b.ddl_name matches,
	a.member_status status
FROM 
	member a
LEFT JOIN
	business_unit b ON
		a.business_unit_id = b.id
LEFT JOIN
	cellphone_number c ON 
		a.cellphone_number_id = c.id 
WHERE 
	a.business_unit_id != 8 AND
	{0} 
	{1} 
ORDER BY
	a.member_status, b.name,a.member_nickname ,a.member_lastname
", where_clause, active_search), null);
                            break;

                        #endregion member

                        #region vendor

                        case "vendor":
                            //_dt	= _tools.getSQL_datatable(@"SELECT vendor_number id, vendor_name name FROM vendor WHERE vendor_number LIKE ""%@v0 %"" OR vendor_name LIKE ""%@v0 %"" AND business_unit_id = @v1  ORDER BY CAST(id AS DEC(10))", new object[] {  q, _member.business_unit_id } );
                            string business_unit_id;
                            if (_q["business_unit_id"] != null && _q["business_unit_id"] != "")
                            {
                                business_unit_id = _q["business_unit_id"];
                            }
                            else
                            {
                                business_unit_id = HttpContext.Current.Session["working_business_unit_id"] != null
                                    ? HttpContext.Current.Session["working_business_unit_id"].ToString()
                                    : _member.business_unit_id.ToString();
                            }

                            string master_id;
                            if (_q["master_id"] != null)
                            {
                                master_id = _q["master_id"];
                            }
                            else
                            {
                                master_id = "";
                            }

                            string tag_id;
                            if (master_id != "" && (_q["tag_id"] == null || _q["tag_id"] == ""))
                            {
                                tag_id = _tools.getSQL_string(
                                    @"SELECT tag_id FROM inventory_item_master  WHERE master_id =@v0",
                                    new object[] { master_id });
                            }
                            else if (master_id == "" && _q["tag_id"] != null && _q["tag_id"] != "")
                            {
                                tag_id = _q["tag_id"];
                            }
                            else
                            {
                                tag_id = "";
                            }

                            if (tag_id == "673" || tag_id == "843")
                            {
                                //_resp.Write("1");
                                if (Regex.Match(q, "/^[0-9]+$/").Success)
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE vendor_number LIKE CONCAT(@v0,'%') AND Vendor_Active = 1 ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q });
                                }
                                else
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE (vendor_number LIKE  CONCAT('%',@v0,'%')  OR vendor_name LIKE  CONCAT('%',@v0,'%')  OR vendor_number = @v0 ) AND Vendor_Active = 1 ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q });
                                }
                            }
                            else if (business_unit_id != "" && tag_id != "")
                            {
                                //_resp.Write("2");	
                                if (Regex.Match(q, "/^[0-9]+$/").Success)
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE vendor_number LIKE CONCAT(@v0,'%')  AND Vendor_Active = 1  ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q, _member.business_unit_id });
                                }
                                else
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE (vendor_number LIKE  CONCAT('%',@v0,'%')  OR vendor_name LIKE  CONCAT('%',@v0,'%')  OR vendor_number = @v0 ) AND Vendor_Active = 1 ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q });
                                }
                            }
                            else
                            {
                                //_resp.Write("3");
                                if (Regex.Match(q, "/^[0-9]+$/").Success)
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE vendor_number LIKE CONCAT(@v0,'%')  AND Vendor_Active = 1 ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q });
                                }
                                else
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT vendor_id id, vendor_name name, vendor_number_int number, '' matches FROM vendor WHERE (vendor_number LIKE  CONCAT('%',@v0,'%')  OR vendor_name LIKE  CONCAT('%',@v0,'%')  OR vendor_number = @v0 ) AND Vendor_Active = 1 ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q });
                                }
                            }

                            break;

                        #endregion vendor

                        #region value

                        case "value":
                            if (_q["attribute_id"] != null)
                            {
                                if (_q["attribute_id"] == "17" && _q["current_manufacturer"] != null)
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @" SELECT a.attribute_value_id id, value name, (SELECT MAX(master_id) FROM inventory_item_detail WHERE attribute_value_id = a.attribute_value_id AND master_id IN (SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = @v1 )) master_id, '' number, '' matches FROM inventory_attribute_value a LEFT JOIN inventory_item_detail b ON a.attribute_value_id = b.attribute_value_id WHERE a.active= true AND attribute_id = 17 AND a.value like  CONCAT('%',@v0,'%')  ORDER BY CAST(id AS DEC(10))",
                                        new object[] { _tools.value_to(q), _q["current_manufacturer"] });
                                }
                                else if (_q["attribute_id"] == "17")
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT a.attribute_value_id id, value name, b.master_id, '' number, '' matches FROM inventory_attribute_value a LEFT JOIN inventory_item_detail b ON a.attribute_value_id = b.attribute_value_id WHERE a.active= true AND attribute_id = 17 AND a.value like  CONCAT('%',@v0,'%')  ORDER BY CAST(id AS DEC(10))",
                                        new object[] { _tools.value_to(q) });
                                }
                                else
                                {
                                    _dt = _tools.getSQL_datatable(
                                        @"SELECT attribute_value_id id, value name, '' number, '' matches FROM inventory_attribute_value WHERE active = true and value LIKE  CONCAT('%',@v0,'%')  AND attribute_id = @v1  ORDER BY CAST(id AS DEC(10))",
                                        new object[] { q, _q["attribute_id"] });
                                }
                            }

                            break;

                        #endregion value

                        #region manufacturer

                        case "manufacturer":
                            _dt = _tools.getSQL_datatable(
                                @"SELECT attribute_value_id id, value name, '' number, '' matches, '' matches FROM inventory_attribute_value WHERE active = true and value LIKE  CONCAT('%',@v0,'%')  AND attribute_id = 16 ORDER BY CAST(id AS DEC(10))",
                                new object[] { _tools.value_to(q) });
                            break;

                        #endregion manufacturer

                        case "master_id":
                            _dt = _tools.getSQL_datatable(
                                @"SELECT master_id id, master_id name, '' number, '' matches, '' matches FROM inventory_item_master WHERE active = true AND master_id LIKE  CONCAT('%',@v0,'%')  ORDER BY master_id",
                                new object[] { q });
                            break;
                        case "master_location":
                            /**
                             * Converted BU to warehouse_BU
                             */
                            var tmp_business_unit = new NeBusinessUnit(working_business_unit_id);
                            var warehose_business_unit_id = new NeBusinessUnit(tmp_business_unit.warehouse_bu_id);
                            _dt = _tools.getSQL_datatable(
                                @"SELECT id, name, '' number, '' matches, '' matches FROM inventory_location_master WHERE business_unit_id = @v1  AND name LIKE  CONCAT('%',@v0,'%')  ORDER BY name",
                                new object[] { q, warehose_business_unit_id.id });
                            break;
                    }

                    if (_dt.Rows.Count > 0)
                    {
                        var sb = new StringBuilder();
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            var _id = _dr["id"].ToString();
                            var _number = _dr["number"].ToString();
                            var _name = _tools.value_from(_dr["name"]);
                            var _customer_number = "";
                            var _customer_hold = "";
                            var _matches = _dr["matches"].ToString();
                            if (_type == "customer")
                            {
                                _customer_number = _dr["customer_number"].ToString();
                                _customer_hold = _dr["customer_hold"].ToString() == "T" ? "On Hold" : "Active";
                                _name = _name.Replace("&amp;", "&").Replace("&quot;", "\"");
                                sb.AppendFormat("({0}) {1} - {2}|{3}|{1}\n", _customer_number, _name, _customer_hold,
                                    _id);
                            }
                            else if (_type == "member")
                            {
                                _name = _name.Replace("&amp;", "&").Replace("&quot;", "\"");
                                var status = _dr["status"].ToString() == "Active" ? 1 : 0;
                                sb.AppendFormat("{0}|{1}|{2}|{3}|{4}\n", _name, _id, status, _number, _matches);
                            }
                            else
                            {
                                var _master_id = _dt.Columns["master_id"] != null ? _dr["master_id"].ToString() : "";
                                _name = _name.Replace("&amp;", "&").Replace("&quot;", "\"");
                                sb.AppendFormat("{0}|{1}|{2}|{3}|{4}\n", _name, _id, _master_id, _number, _matches);
                            }
                        }

                        _resp.Write(sb);
                    }
                    else
                    {
                        _resp.Write("");
                    }
                }
            }
            catch (Exception ee)
            {
                _resp.Write(ee.ToString());
            }

            _resp.End();
        }
    }
}
