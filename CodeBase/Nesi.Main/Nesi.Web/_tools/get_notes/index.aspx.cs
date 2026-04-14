using System;
using System.Data;
using nesi.core;

public partial class this_get_notes : System.Web.UI.Page
	{
	NeMember _member;
	NeBusinessUnit _company;
	private const int _page_id = 1; // from Page table in DB

	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		_member = Toolbox.do_handle_authentication(_page_id);
		_company = new NeBusinessUnit(_member.business_unit_id);
		var _q = Request.QueryString;  //System.Collections.Specialized.NameValueCollection
		//string _a = Request.QueryString[1];
		var OUTPUT = "";
		// DataTable _dt;
		_tools.set_XML_header();
		var sql = "";
		// string sqlissues = "";
		if (_q["id"] != null)
			{
			var notes	= "";
			var part = "";
			var rec = "";
			if (_q["source"] == "purchaseorder")
				{
				sql = "SELECT URLDECODE(po_details_notes) notes, po_details_part_no part, po_details_rec_no rec from po_details_current WHERE po_details_id=" + _q["id"];
				}
			else if (_q["source"] == "groupings")
				{
				sql = "SELECT URLDECODE(inventory_group_dtl_notes) notes, master_id part, '' rec from inventory_group_dtl WHERE id=" + _q["id"];
				}
			else if (_q["source"] == "alternates")
				{
				sql = "SELECT URLDECODE(Inventory_Alternate_Notes) notes, Inventory_Alternate_Alternate_Master_ID part, '' rec from Inventory_Alternate WHERE Inventory_Alternate_id=" + _q["id"];
				}
			else if (_q["source"] == "workorder")
				{
				sql = "SELECT IF(IFNULL(wo_detail_current_notes,'') LIKE 'A_%|%', SUBSTRING(wo_detail_current_notes FROM LOCATE('|', wo_detail_current_notes)+1) ,TRIM(wo_detail_current_notes)) notes, wo_detail_current_master_id part, wo_detail_current_rec_no rec from wo_detail_current WHERE wo_detail_current_ID=" + _q["id"];
				}
			else if (_q["source"] == "quote")
				{
				sql = "SELECT URLDECODE(notes) notes, part_no part, '' rec from quote_worksheet WHERE id=" + _q["id"];
				}



			try
				{
				var dt = Toolbox.doSQL_dt(sql,null);
				if (dt.Rows.Count > 0)
					{
					var dr = dt.Rows[0];
					notes = dr["notes"].ToString();
					part = dr["part"].ToString();
					rec = dr["rec"].ToString();
					}
				}
			catch { }

			OUTPUT += string.Format(@"
<notes>
	<part>{1}</part>
	<body>{0}</body>
	<rec>{2}</rec>
</notes>
		",
			   notes,part, rec);			// {0}
			Response.Write(OUTPUT);
			Response.End();

			}

		}
	}
