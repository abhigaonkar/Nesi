using System;
using System.Data;
using DevExpress.Web;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Xpo;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using nesi.core;
using NESI.Common.Models;

public partial class mobile_shopping_cart : System.Web.UI.Page
	{
	public NeMember current_user;
	private const int page_id = 206; // from Page table in DB
	NeBusinessUnit _this_company;
	DataTable _shopping_cart_dt;
	NameValueCollection _q;
	bool is_mobile;
	int woprog_id;
	int location_id;
    int is_annual;
	string search_string = "";
    public branch_options bo_obj;
	MySqlConnection conn	= new MySqlConnection();
	private NeBusinessUnit WorkingBusinessUnit {get; set;}
	private NeBusinessUnit WarehouseBusinessUnit {get; set;}
	NeWOProg wo { get; set;}
	protected void Page_PreInit(object _sender, EventArgs _e)
		{
		conn				= Toolbox.connect();
		}
	protected void Page_Unload(object _sender, EventArgs _e)
		{
		if (conn.State != ConnectionState.Open) return;
		conn.Close();
		conn.Dispose();
		}
	protected void Page_Init(object _sender, EventArgs _e)
		{
			current_user = Toolbox.do_handle_authentication(1);
			WorkingBusinessUnit = new NeBusinessUnit(current_user.business_unit_id);
			WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
			_q = Request.QueryString;
			if (!string.IsNullOrEmpty(_q["woprog_id"]))
				{
				int.TryParse(_q["woprog_id"], out woprog_id);
				}
			if (woprog_id != 0)
				{
				wo = new NeWOProg(woprog_id);
				WorkingBusinessUnit = new NeBusinessUnit(wo.business_unit_id);
				WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
				}

			bo_obj = new branch_options(WarehouseBusinessUnit.id);
			hdn_woprog_id.Value = woprog_id.ToString();
			if (!string.IsNullOrEmpty(_q["location_id"]))
				{
				int.TryParse(_q["location_id"], out location_id);
				}
			hdn_location_id.Value = location_id.ToString();
			if (!string.IsNullOrEmpty(_q["is_annual"]))
				{
				int.TryParse(_q["is_annual"], out is_annual);
				}
			hdn_is_annual.Value = is_annual.ToString();

			if (!string.IsNullOrEmpty(_q["search_string"]))
				{
				search_string = Server.UrlDecode(_q["search_string"]);
				}

			Session["member_id"] = current_user.id;


			ddl_cart.DataBind();
			if (!IsPostBack)
				{
				Session["shopping_cart_master_dt"] = null;
				Session["shopping_cart_selected_values"] = null;
				Session["shopping_cart_gv"] = null;
				Session["shopping_cart_builder"] = null;
				Session["shopping_cart_full_gv"] = null;
				Session["shopping_cart_gv_distinct"] = null;
				Session["shopping_cart_other_full_gv"] = null;
				Session["shopping_cart_focused_row"] = null;
				Session["shopping_cart_active_cart"] = null;
				Session["mobile_cart_woprog_id"] = null;
				Session["mobile_cart_customer"] = null;
				gv_cart.JSProperties["cp_refresh_other_grid"] = 0;
				gv_other_parts.JSProperties["cp_refresh_other_grid"] = 0;

				gv_cart.FocusedRowIndex = -1;
				}
			update_other_parts_gv();
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{

		ddl_cart.DataBind();
		if (!IsPostBack)
			{
			ddl_cart.SelectedIndex = -1;
			mv.ActiveViewIndex = 0;
			update_view();
			}
		else
			{
			update_cart_gv();
			}
			
		if (Session["shopping_cart_gv_distinct"] != null)
			{
			//gv_sc.DataSource = (DataTable)Session["shopping_cart_gv_distinct"];
			//	_shopping_cart_dt = (DataTable)gv_sc.DataSource;
			//gv_sc.DataBind();
			}
		if (Session["shopping_cart_full_gv"] != null)
			{
			_shopping_cart_dt = new DataView((DataTable)Session["shopping_cart_full_gv"]).ToTable(true, "master_id");
			}


		if (!IsPostBack)
			{
			update_cart_gv();
			if (search_string != "")
				{
				tb_search.Value = search_string;
				update_navbar(true);
				}
			}

		set_columns();

		tr_qty_in_stock.Visible = is_annual != 1;
		tr_qty_on_order.Visible = is_annual != 1;
		tr_qty_on_truck.Visible = is_annual != 1;
		tr_qty_on_wo.Visible = is_annual != 1;


		}
	protected void update_other_parts_gv()
		{
		//		if (ddl_cart.Value == null)
		//		{
		//			ddl_cart.DataBind();
		//			if (ddl_cart.Items.Count > 0)
		//			{
		//				ddl_cart.Value = Session["shopping_cart_active_cart"];
		//			}
		//		}

		//	** change description to use canada or USA stuff

		if (Session["shopping_cart_other_full_gv"] == null)
			{
			if (Session["shopping_cart_focused_row"] != null && !Session["shopping_cart_focused_row"].Equals(-1) && gv_cart.VisibleRowCount > 0)
				{
				var master_id			= gv_cart.GetRowValues(Convert.ToInt32(Session["shopping_cart_focused_row"]), "master_id");
				var header_id			= ddl_cart.Value;
				if(header_id != null && master_id != null)
					{
				Session["shopping_cart_other_full_gv"] = Toolbox.doSQL_dt(conn, @"
SELECT 
	a.wo_detail_history_master_id master_id,
	t.tag,
	SUBSTRING(id.description FROM LOCATE(' - ', id.description) + 3)  description,
	ROUND(SUM(a.wo_detail_history_qty_committed),0) qty_on 
from 
	wo_detail_history a
LEFT JOIN 
	inventory_description id on id.master_id = a.wo_detail_history_master_id 
LEFT JOIN 
	inventory_item_master i on i.master_id = a.wo_detail_history_master_id
LEFT JOIN 
	inventory_tag t on i.tag_id = t.tag_id

WHERE 
	a.wo_detail_history_master_id < 990000 AND 
t.is_exclude=0 and t.allowed_to_stock = 1 and t.active = 1 and
	a.wo_detail_history_master_id NOT IN (2139,9595) AND
	a.wo_detail_history_woprog_id IN 
		(
		SELECT
			b.wo_detail_history_woprog_id
		FROM
			wo_detail_history AS b
		WHERE
			b.wo_detail_history_master_id = " + master_id + @" AND
			b.wo_detail_history_date_added > DATE_SUB(CURDATE(), INTERVAL 2 MONTH) AND
			b.business_unit_id NOT IN (8, 11, 47, 48)
		) AND 
	a.wo_detail_history_master_id NOT IN 
		(
		SELECT
			master_id
		FROM
			shopping_cart
		WHERE 
			shopping_cart_header_id = " + header_id + @"
		)
GROUP BY 
	a.wo_detail_history_master_id
HAVING 
	SUM(a.wo_detail_history_qty_committed) > 5 AND 
	COUNT(a.wo_detail_history_woprog_id) > 10
ORDER BY 
	i.tag_id, SUM(a.wo_detail_history_qty_committed) desc",null);
					}
				}


			}
		gv_other_parts.DataSource = Session["shopping_cart_other_full_gv"] ?? new DataTable();
		gv_other_parts.DataBind();
		}
	protected void update_cart_gv()
		{
/*		if (Session["shopping_cart_full_gv"] == null)
			{
			Session["shopping_cart_full_gv"] = Toolbox.doSQL_dt(conn,@" SELECT b.desc_full_usa `desc`, a.id, a.member_id, a.dt, a.master_id, a.qty FROM shopping_cart a INNER JOIN inventory_description b ON a.master_id = b.master_id  WHERE a.shopping_cart_header_id =@v0 ORDER BY id", new object[] { (ddl_cart.Value ?? 0) });

			}
		gv_cart.DataSource = Session["shopping_cart_full_gv"];
		gv_cart.DataBind();
*/  // commented out by andy as shopping cart is bypassed at this point.
		
		}
	protected string get_master_ids_in_gv()
		{
		if (Session["shopping_cart_gv"] != null)
			{
			var dt = (DataTable)Session["shopping_cart_gv"];
			var string_arr = new List<string>();

			// Classic version :-)
			for (var a = 0; a < dt.Rows.Count; a++)
				{
				string_arr.Add(dt.Rows[a]["master_id"].ToString());
				}
			var master_ids = string.Join(",", string_arr);

			if (master_ids.Length > 1)
				{
				master_ids = master_ids.TrimEnd(',');
				}
			return master_ids;
			}
		else
			{
			return "999999";
			}
		}
	protected string get_values_in_tokenbox()
		{
		var value_ids = "";
		var dt = new DataTable();
		if (Session["shopping_cart_selected_values"] != null)
			{
			dt = (DataTable)Session["shopping_cart_selected_values"];
			for (var x = 0; x < dt.Rows.Count; x++)
				{
				value_ids += dt.Rows[x][0].ToString().Split('|').GetValue(0) + ",";
				}
			if (value_ids.Length > 1)
				{
				value_ids = value_ids.TrimEnd(',');
				}
			}
		return value_ids;
		}
	protected void fill_grid()
		{
		#region if there are any results at all so far
		if (Session["shopping_cart_master_dt"] != null)
			{
			Session["shopping_cart_builder"] = Session["shopping_cart_master_dt"];
			var view_master = new DataView((DataTable)Session["shopping_cart_master_dt"]);

			view_master.Sort = "onhand DESC,description";
			var values = get_values_in_tokenbox();
			var master_ids_in_gv = get_master_ids_in_gv();

			#region if there are some filter tokens
			if (values.Length > 1)
				{
				Session["shopping_cart_builder"] = Session["shopping_cart_gv"];
				//	if (master_ids_in_gv != "")
				//	{
				//		view_master.RowFilter = "master_id in (" + master_ids_in_gv + ")";
				//	}
				//	Session["shopping_cart_builder"] = view_master.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag", "attribute_id", "attribute", "attribute_value_id", "value");
				view_master = new DataView((DataTable)Session["shopping_cart_builder"]);
				var filter = "";
				if ((values.Split(',').Length == 1) && (master_ids_in_gv.Length > 0))
					{
					filter = " (attribute_value_id = " + values + " and master_id in (" + master_ids_in_gv + "))";
					}
				else
					{
					foreach (var masterid in master_ids_in_gv.Split(','))
						{
						var temp_filter_by_masterid = new DataView((DataTable)Session["shopping_cart_master_dt"]);
						temp_filter_by_masterid.RowFilter = "master_id=" + masterid;
						var dt_temp_filter_by_masterid = temp_filter_by_masterid.ToTable(true, "attribute_value_id");
						foreach (DataRow dr_temp_filter_by_masterid in dt_temp_filter_by_masterid.Rows)
							{
							foreach (var value in values.Split(','))
								{
								var temp_filter_by_masterid_and_value = new DataView(dt_temp_filter_by_masterid);
								temp_filter_by_masterid_and_value.RowFilter = "attribute_value_id=" + value;
								var xxx = temp_filter_by_masterid_and_value.ToTable(true, "attribute_value_id");
								if (xxx.Rows.Count == 0)
									{
									filter += masterid + ",";
									}

								}

							}
						}
					if (filter.Length > 1)
						{
						filter = "master_id not in(" + filter.TrimEnd(',') + ")";
						}
					}
				if (filter.Length > 1)
					{
					view_master.RowFilter = filter;
					Session["shopping_cart_builder"] = view_master.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag", "attribute_id", "attribute", "attribute_value_id", "value");
					Session["shopping_cart_gv"] = Session["shopping_cart_builder"];
					}
				}
			#endregion
			//		DataView view_master_gv = new DataView((DataTable)Session["shopping_cart_gv"]);
			//		view_master_gv.RowFilter="Select distinct master_id,description,onhand,last_used,tag_id,tag";
			//		gv_sc.DataSource = view_master_gv.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag");
			var ddd = (DataTable)Session["shopping_cart_gv"];
			Session["shopping_cart_gv_distinct"] = ddd.DefaultView.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag");
			//gv_sc.DataSource = Session["shopping_cart_gv_distinct"];
			//gv_sc.DataBind();
			//gv_sc.JSProperties["cp_count"] = gv_sc.VisibleRowCount.ToString();
			if (view_master.Count > 0)
				{
				//var btn = (ASPxButton)gv_sc.FindFooterCellTemplateControl(gv_sc.Columns["qty"], "btn_add_to_cart");
				//btn.ClientVisible = true;
				}
			}
		#endregion

		}
	protected void update_navbar(bool _new_search)
		{
		if (_new_search)
			{
			Session["shopping_cart_gv_distinct"] = null;
			Session["shopping_cart_master_dt"] = null;
			Session["shopping_cart_selected_values"] = null;
			Session["shopping_cart_gv"] = null;
			Session["shopping_cart_builder"] = null;
			}
		var dt	= new DataTable();
		DataView view;
		var dt_navbar = new DataTable();
		dt_navbar.Columns.Add("_group_name");
		dt_navbar.Columns.Add("_group_text");
		dt_navbar.Columns.Add("_item_name");
		dt_navbar.Columns.Add("_item_text");

		var type_id			= (int) ddl_type.Value;
		if (tb_search.Value != "" || Toolbox.Contains(type_id, new []{3,4,6}) && ddl_type_id.SelectedIndex >= 0)
			{
			#region set new search datatable
	var desc_country = current_user.Country == "USA" ? "usa" : "cdn";
			if (_new_search)
				{
				
				var select		= @"
SELECT 
    f.is_rental,
	f.tag_id,
	f.tag,
	a.master_id,
	d.desc_full_" + desc_country + @" description,
	c.attribute_id,
	c.attribute,
	b.value,
	b.attribute_value_id,
	IFNULL(g.min_qty, 0.0) min,
	IFNULL(g.int_onhand_qty, 0.0) onhand,
	g.ts last_used,
" + (location_id !=0? @"(ifnull((Select il.qty from inventory_location il where il.location_master_id = " + location_id + @"  and il.master_id=a.master_id),0.0))" : "9999999999") + @" qty_truck,
" + (woprog_id != 0 ? @"(select ifnull((Select aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @" qty_cmt_on_wo,
" + (woprog_id != 0 ? @"(select ifnull((Select aa.wo_detail_current_qty_ordered-aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @"  qty_req_on_wo

FROM
	inventory_item_detail a
LEFT JOIN 
	inventory_attribute_value b ON a.attribute_value_id = b.attribute_value_id
LEFT JOIN 
	inventory_attribute c ON b.attribute_id = c.attribute_id
LEFT JOIN 
	inventory_description d ON d.master_id = a.master_id
LEFT JOIN 
	inventory_item_master e ON e.master_id = a.master_id
LEFT JOIN 
	inventory_tag f ON e.tag_id = f.tag_id
LEFT JOIN 
	inventory_branch g ON e.master_id = g.master_id";

				var where_clause	= @"
WHERE 
f.is_exclude=0 and f.allowed_to_stock = 1 and f.active = 1 and 
	e.active = true AND 
	e.master_id = g.master_id AND 
	g.business_unit_id = '" + WarehouseBusinessUnit.id + @"' 
ORDER BY 
	g.wo_usage desc, min DESC";
				if (type_id == 30)  //group
					{
					select += @"
LEFT JOIN 
	inventory_group_dtl h ON 
		e.master_id = h.master_id AND 
		h.group_id = " + ddl_type_id.Value;
					}
				else if (type_id == 40) //kit
					{
					select += @"
LEFT JOIN 
	inventory_kit_dtl h ON 
		e.master_id = h.inventory_kit_dtl_master_id AND 
		h.inventory_kit_dtl_hdr_id = " + ddl_type_id.Value;
					}
				else if (type_id == 3) //wo
					{
					select += @"
LEFT JOIN 
	vw_wo_parts h ON 
		e.master_id = h.master_id AND 
		h.woprog_id = '" + ddl_type_id.Value +"'" ;
					}
				else if (type_id == 4) //quote
					{
					select += @"
LEFT JOIN 
	quote_worksheet h ON 
		e.master_id = h.part_no AND 
		h.quote_id = " + ddl_type_id.Value;
					}
				else if (type_id==5) // vendor no
				{
					select += @"
LEFT JOIN 
	inventory_price ip ON 
		e.master_id = ip.master_id AND 
		ip.vendor_code = '" + tb_search.Value + "'";
				}
				

				if(Toolbox.Contains(type_id, new []{3,4,6}))
					{
					dt			= Toolbox.doSQL_dt(conn, select+where_clause,null);
					}
				else // All=0, Commonly Used=1, Stocked = 2 
					{
					int search_type;
					int.TryParse(tb_search.Value, out search_type);
					var show_common			= type_id == 1;
					var show_stocked		= type_id == 1 || type_id == 2;
					if (type_id == 5)
					{
						dt = inventory.search_results_sc(tb_search.Value, current_user.business_unit, "vendor_code", false, false, false, false, woprog_id.ToString(), location_id.ToString());
					}
					else
					{
						dt = inventory.search_results_sc(tb_search.Value, current_user.business_unit, search_type > 0 ? "master_id" : "description", false, false, show_stocked, show_common, woprog_id.ToString(), location_id.ToString());
					}
					}

				    if (dt.Rows.Count > 0)
				    {
				        dt.DefaultView.RowFilter = "is_rental = 0";
				        dt = dt.DefaultView.ToTable();
                    }

				    if(dt.Rows.Count > 0)
					{


					view = new DataView(dt);
					view.Sort = "description";
					var Json		= new JavaScriptSerializer();
					Json.MaxJsonLength				= 50000000;
					var main_inv_obj				= dt.AsEnumerable()
														.GroupBy(g => new 
															{ 
															m = g.Field<int>("master_id"),
															t = g.Field<int>("tag_id"),
                                                            d = g.Field<string>("description") == null ? " " : g.Field<string>("description").Replace(g.Field<string>("tag") + " - ", ""),                                                         
															q = g.Field<double>("onhand"),
															l = Toolbox.MySQL_shortdt(g.Field<DateTime>("last_used")),
															tr =g.Field<object>("qty_truck"),
															w =g.Field<decimal>("qty_cmt_on_wo"),
															r = g.Field<decimal>("qty_req_on_wo")
															})
														.Select(g => new
															{
															g.Key.m,
															g.Key.t,
															v = g.Select(_z => _z.Field<int>("attribute_value_id")),
															a = g.Select(_z => _z.Field<int>("attribute_id")),
															g.Key.d,
															g.Key.q,
															g.Key.l,
															g.Key.tr,
															g.Key.w,
															g.Key.r
															})
														.OrderByDescending(o => o.q)
														.ToList();
					var tags_obj					= (from p in dt.AsEnumerable()
														select new 
															{
															id = p.Field<int>("tag_id"),
															t = p.Field<string>("tag")
															}).Distinct().ToList();
					var atts_obj					= (from p in dt.AsEnumerable()
														select new 
															{
															id = p.Field<int>("attribute_id"),
															a = p.Field<string>("attribute")
															}).Distinct().OrderBy( _a => _a.a).ToList();
					var vals_obj					= (from p in dt.AsEnumerable()
														select new 
															{
															id = p.Field<int>("attribute_value_id"),
															aid = p.Field<int>("attribute_id"),
															v = p.Field<string>("value")
															}).Distinct().OrderBy( _v => _v.v).ToList();

					var js						= string.Format(@"
	cart.inv_base = {0};
	cart.tags = {1};
	cart.atts = {2};
	cart.vals = {3};",
					Json.Serialize(main_inv_obj),
					Json.Serialize(tags_obj),
					Json.Serialize(atts_obj),
					Json.Serialize(vals_obj)
					);
					System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),"asdf", js,true);
					}
				else
					{
					var js						= @"
	cart.inv_base = [];
	cart.tags = [];
	cart.atts = [];
	cart.vals = [];";
					System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),"asdf", js,true);
					}

				/*
				Session["shopping_cart_gv"] = dt;
				Session["shopping_cart_master_dt"] = dt;
				if (dt.Rows.Count > 0)
					{
					fill_grid();
					//		DataTable dt_temp = view.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag", "attribute_id", "attribute", "attribute_value_id", "value");
					//		DataView view2 = new DataView(dt_temp);
					var view2 = new DataView(dt);
					view.Sort = "attribute";
					var tags = view2.ToTable(true, "tag_id", "tag");
					var atts = view2.ToTable(true, "attribute_id", "attribute");
					var vals = view2.ToTable(true, "attribute_value_id", "value");
					var gv_dt = (DataTable)gv_sc.DataSource;

					var gv_master_dt = (DataTable)Session["shopping_cart_master_dt"];
					navbar.Groups.Clear();
					foreach (DataRow dr_attr in atts.Rows)
						{
						var nbg = new NavBarGroup(dr_attr["attribute"].ToString(), dr_attr["attribute_id"].ToString());
						var view_values = new DataView(dt.Select("attribute_id=" + dr_attr["attribute_id"]).CopyToDataTable());
						var vals2 = view_values.ToTable(true, "attribute_value_id", "value");
						foreach (DataRow dr_vals in vals2.Rows)
							{
							var x = string.Format(dr_vals["value"] + " (" + gv_master_dt.Select("attribute_value_id=" + dr_vals["attribute_value_id"]).CopyToDataTable().Rows.Count + ")");
							nbg.Items.Add(new NavBarItem(x, dr_vals["attribute_value_id"] + "|" + dr_vals["value"]));
							dt_navbar.Rows.Add(dr_attr["attribute_id"].ToString(), dr_attr["attribute"].ToString(), dr_vals["attribute_value_id"].ToString(), x);
							}

						navbar.Groups.Add(nbg);

						}
					navbar.DataBind();
					}
				 */

				}
			#endregion
			#region work with drill down
			else
				{
				dt = (DataTable)Session["shopping_cart_master_dt"];
				var dt_filter = (DataTable)Session["shopping_cart_selected_values"];

				view = new DataView(dt);
				view.RowFilter = "master_id in(" + get_master_ids_in_gv() + ")";

				if (dt.Rows.Count > 0)
					{
					view.Sort = "last_used desc";
					var dt_temp = view.ToTable(true, "master_id", "description", "onhand", "last_used", "tag_id", "tag", "attribute_id", "attribute", "attribute_value_id", "value");
					var view2 = new DataView(dt_temp);
					view.Sort = "attribute";
					var tags = view2.ToTable(true, "tag_id", "tag");
					var atts = view2.ToTable(true, "attribute_id", "attribute");
					var vals = view2.ToTable(true, "attribute_value_id", "value");
					var gv_dt = dt_temp;
					//		DataTable gv_master_dt = (DataTable)Session["shopping_cart_master_dt"];

				//	navbar.Groups.Clear();
					foreach (DataRow dr_attr in atts.Rows)
						{
						var nbg = new NavBarGroup(dr_attr["attribute"].ToString(), dr_attr["attribute_id"].ToString());
						var view_values = new DataView(dt.Select("attribute_id=" + dr_attr["attribute_id"]).CopyToDataTable());
						var vals2 = view_values.ToTable(true, "attribute_value_id", "value");
						foreach (DataRow dr_vals in vals2.Rows)
							{
							if (gv_dt.Select("attribute_value_id=" + dr_vals["attribute_value_id"]).Length > 0)
								{
								var x = string.Format(dr_vals["value"] + " (" + gv_dt.Select("attribute_value_id=" + dr_vals["attribute_value_id"]).CopyToDataTable().Rows.Count + ")");
								nbg.Items.Add(new NavBarItem(x, dr_vals["attribute_value_id"] + "|" + dr_vals["value"]));
								dt_navbar.Rows.Add(dr_attr["attribute_id"].ToString(), dr_attr["attribute"].ToString(), dr_vals["attribute_value_id"].ToString(), x);
								}
							}
						if (dt_filter.Select("group_id = " + dr_attr["attribute_id"]).Length == 0)
							{
							//navbar.Groups.Add(nbg);
							}
						}
					//	navbar.DataSource = _dt_navbar;
				//	navbar.DataBind();
					}
				}
			#endregion
			}


		#region final clean up on groups
		//if (navbar.Groups.GetVisibleItemCount() > 6)
		//	{
		//	navbar.Groups.CollapseAll();
		//	}
		//else
		//	{
		//	navbar.Groups.ExpandAll();
		//	}
		#endregion

		}
	protected void tb_search_TextChanged(object _sender, EventArgs _e)
		{
		//	update_navbar(true);
		}
	
	protected void cb_search_Callback(object _sender, CallbackEventArgsBase _e)
		{
		cb_search.JSProperties["cp_alert"] = "";
		if (_e.Parameter == "tb_changed")
			{
			/*	DataTable dt = (DataTable)Session["shopping_cart_selected_values"];
				DataTable dt_temp = dt.Copy();
				dt_temp.Rows.Clear();
				update_navbar(true);
				if (dt!=null)
				{
					foreach(DataRow dr in dt.Rows)
					{
						if (!tb.Tokens.Contains(dr[2].ToString() + " : " + dr[0].ToString().Split('|').GetValue(1)))
						{
						}
						else
						{
							dt_temp.ImportRow(dr);
							Session["shopping_cart_selected_values"] = dt_temp;
							fill_grid();
							update_navbar(false);
						}
					}
				}
			 */
			}
		else if (_e.Parameter == "add_to_cart")
			{
			var x = "tb_qty";
			//var max_rows = gv_sc.VisibleRowCount;
			var items = 0;
			//if (gv_sc.PageCount > 1)
			//	{
			//	max_rows = gv_sc.SettingsPager.PageSize;
			//	}
				/*
			for (var i = 0; i < max_rows; i++)
				{
				var tb = (ASPxTextBox)gv_sc.FindRowCellTemplateControl(i, (GridViewDataColumn)gv_sc.Columns["qty"], x);
				if (tb.Text != "")
					{
					cb_search.JSProperties["cp_last_modified"] = Toolbox.MySQLNow_int();
					if (ddl_cart.Value == null)
						{
						var name = my_member.Nickname + "-" + Toolbox.MySQLNow_int();
						var sc		= new shopping_cart { name = name, member_id = my_member.id };
						sc.save();
						ddl_cart.DataBind();
						if (ddl_cart.Items.FindByText(name) != null)
							{
							ddl_cart.Text = name;
							Session["shopping_cart_active_cart"] = ddl_cart.Items.FindByText(name).Value;
							}

						}
					items++;
					try
						{
						var master_id = 0;
						var header_id = (int) ddl_cart.Value;
						var qty			= Convert.ToDouble(tb.Text);
						if(gv_sc.GetRowValues(i, "master_id") != null)
							{
							int.TryParse(gv_sc.GetRowValues(i, "master_id").ToString(), out master_id);
							}
						if(master_id > 0)
							{
							if (!shopping_cart.line_exists(header_id, master_id))
								{
								var si		= new shopping_cart.item { master_id = master_id, member_id = my_member.id, qty = qty, shopping_cart_header_id = header_id };
								si.save();
								}
							else
								{
								var si		= new shopping_cart.item(header_id, master_id);
								si.qty		= si.qty + qty;
								si.save();
								}
							}

						Session["shopping_cart_full_gv"] = null;
						Session["shopping_cart_other_full_gv"] = null;
						}
					catch
						{
						tb.Text = "";

						}
					}

				//		cb_search.JSProperties["cp_alert"] = items + " added to cart";
				}
				*/
			update_cart_gv();
			update_other_parts_gv();

			fill_grid();
			update_navbar(false);
			cb_search.JSProperties["cp_no_of_items"] = gv_cart.VisibleRowCount;


			}
		else
			{
			//tb.Tokens.Clear();
			//if (Session["shopping_cart_selected_values"] == null)
			//	{
			//	var _vals = new DataTable();
			//	_vals.Columns.Add("id");
			//	_vals.Columns.Add("value");
			//	_vals.Columns.Add("group_name");
			//	_vals.Columns.Add("group_id");
			//	Session["shopping_cart_selected_values"] = _vals;
			//	}
			//var dt = (DataTable)Session["shopping_cart_selected_values"];
			//var vals = _e.Parameter.Split('|');
			//dt.Rows.Add(vals.GetValue(0) + "|" + vals.GetValue(1), vals.GetValue(2).ToString(), vals.GetValue(3).ToString(), vals.GetValue(4).ToString());
			//foreach (DataRow s_val in (dt.Rows))
			//	{
			//	tb.Tokens.Add(s_val[2] + " : " + s_val[0].ToString().Split('|').GetValue(1));
			//	}
			//fill_grid();
			//update_navbar(false);
			}

		}
	/*protected void btn_search_Click(object sender, EventArgs e)
	{
		tb.Tokens.Clear();
		update_navbar(true);
		if (ddl_type.Value.Equals("3") || ddl_type.Value.Equals("4") || ddl_type.Value.Equals("5") || ddl_type.Value.Equals("6"))
		{
			ddl_type_id.ClientVisible = true;
			tb_search.ClientVisible = false;
			
		}
		else
		{
			ddl_type_id.ClientVisible = false;
			tb_search.ClientVisible = true;
		}
	}*/
	protected void chk_Init(object _sender, EventArgs _e)
		{
		var c = _sender as ASPxCheckBox;
		var container = c.NamingContainer as NavBarItemTemplateContainer;
		c.ClientSideEvents.CheckedChanged = string.Format(@"function(s, e) {{cb_search.PerformCallback('{0}|{1}|{2}|{3}|'+ s.GetValue());}}", container.Item.Name.Replace("'", "ft").Replace("\"", "in"), container.Item.Text.Replace("'", "ft").Replace("\"", "in"), container.Item.Group.Text.Replace("'", "ft").Replace("\"", "in"), container.Item.Group.Name.Replace("'", "ft").Replace("\"", "in"));

		}
	protected void navbar_DataBound(object _sender, EventArgs _e)
		{

		}
	protected void ASPxButton1_Click(object _sender, EventArgs _e)
		{

		}
	protected void gv_cart_RowDeleting(object _sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs _e)
		{
		var gv			= (ASPxGridView) _sender;
		if ((int) _e.Keys[0] != 0)
			{
			shopping_cart.item.delete((int) _e.Keys[0]);
			}
		gv.CancelEdit();
		gv.FocusedRowIndex = -1;
		Session["shopping_cart_full_gv"] = null;
		update_cart_gv();
		gv.JSProperties["cp_refresh_other_grid"] = 1;
		_e.Cancel = true;
		}

	
	protected void ddl_cart_SelectedIndexChanged(object _sender, EventArgs _e)
		{
		if (!IsCallback)
			{
			if (ddl_cart.Value != null)
				{
				Session["shopping_cart_active_cart"] = ddl_cart.Value;
				}
			Session["shopping_cart_master_dt"] = null;
			Session["shopping_cart_selected_values"] = null;
			Session["shopping_cart_gv"] = null;
			Session["shopping_cart_builder"] = null;
			Session["shopping_cart_full_gv"] = null;
			Session["shopping_cart_gv_distinct"] = null;
			Session["shopping_cart_other_full_gv"] = null;
			Session["shopping_cart_focused_row"] = null;
			//		fill_grid();
			update_cart_gv();
			update_other_parts_gv();

			}

		}
	protected void ddl_cart_Callback(object _sender, CallbackEventArgsBase _e)
		{
		//		if ((ddl_cart.SelectedIndex<0)&&(ddl_cart.Items.Count>0))
		//		{
		//			ddl_cart.SelectedIndex = 0;
		//		}
		Session["shopping_cart_other_full_gv"] = null;
		update_other_parts_gv();
		Session["shopping_cart_full_gv"] = null;
		update_cart_gv();
		}
	protected void gv_cart_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		var p = _e.Parameters;
		var value = "";
		var key = "";
		if (_e.Parameters.Split('|').Length > 1)
			{
			p = _e.Parameters.Split('|').GetValue(0).ToString();
			value = _e.Parameters.Split('|').GetValue(2).ToString();
			key = _e.Parameters.Split('|').GetValue(1).ToString();
			}

		if (p != "")
			{
			shopping_cart.item si;
			switch (p)
				{
				case "refresh":
					Session["shopping_cart_full_gv"] = null;
					update_cart_gv();
				break;
				case "qty":
					si	= new shopping_cart.item(Convert.ToInt32(key));
					si.qty		= Convert.ToDouble(value);
					si.save();
					Session["shopping_cart_full_gv"] = null;
					update_cart_gv();
					break;

				case "clear":
					if (ddl_cart.Value != null)
						{
						var header_id		= Convert.ToInt32(ddl_cart.Value);
						shopping_cart.delete_children(header_id);
						Session["shopping_cart_full_gv"] = null;
						update_cart_gv();
						gv_cart.JSProperties["cp_refresh_other_grid"] = 1;
						}
					break;
				case "consolidate":
					var did_consolidate = false;
					for (var x = 0; x < gv_cart.VisibleRowCount; x++)
						{
						var master_id_top = Convert.ToInt32(gv_cart.GetRowValues(x, "master_id"));
						var id_ = Convert.ToInt32(gv_cart.GetRowValues(x, "id"));
						for (var y = x; y < gv_cart.VisibleRowCount; y++)
							{
							var master_id_child = Convert.ToInt32(gv_cart.GetRowValues(y, "master_id"));
							var id_child = Convert.ToInt32(gv_cart.GetRowValues(y, "id"));
							if (master_id_child == master_id_top && id_ != id_child)
								{
								si			= new shopping_cart.item(id_);
								var qty		= Convert.ToDouble(gv_cart.GetRowValues(y, "qty"));
								si.qty		= si.qty + qty;
								si.save();
								shopping_cart.item.delete(id_child);
								did_consolidate = true;
								}

							}

						}
					if (did_consolidate)
						{

						Session["shopping_cart_full_gv"] = null;
						update_cart_gv();
					
						}
				break;



				}
			}
		else
			{
			Session["shopping_cart_full_gv"] = null;
			update_cart_gv();
			}
		}
	protected void btn_group0_Click(object _sender, EventArgs _e)
		{
		var header_id		= Convert.ToInt32(ddl_cart.Value);
		shopping_cart.delete_children(header_id);
		shopping_cart.delete(header_id);
		Session["shopping_cart_full_gv"] = null;
		update_cart_gv();
		ddl_cart.DataBind();

		}
	protected void btn_group1_Click(object _sender, EventArgs _e)
		{
		var name = current_user.Nickname + "-" + Toolbox.MySQLNow_long();
		var sc		= new shopping_cart { member_id = current_user.id, name = name };
		sc.save();
		ddl_cart.DataBind();
		if (ddl_cart.Items.FindByText(name) != null)
			{
			Session["shopping_cart_active_cart"] = ddl_cart.Items.FindByText(name).Value;
			ddl_cart.Text = name;
			}


		Session["shopping_cart_full_gv"] = null;
		update_cart_gv();


		}
	protected void gv_destination_HtmlRowPrepared(object _sender, ASPxGridViewTableRowEventArgs _e)
		{
		if (_e.VisibleIndex >= 0)
			{
			var x = _e.GetValue("master_id").ToString();
			if (_shopping_cart_dt.Select("master_id=" + x).Length > 0)
				{
				_e.Row.Font.Bold = true;

				}

			}
		}
	protected void cb_cart1_Callback(object _sender, CallbackEventArgsBase _e)
		{

		}
	protected void gv_other_parts_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		switch (_e.Parameters)
			{


			case "refresh":
				Session["shopping_cart_focused_row"] = gv_cart.FocusedRowIndex;
				Session["shopping_cart_other_full_gv"] = null;
				update_other_parts_gv();

				break;


			case "add_other":
				var max_rows = gv_other_parts.VisibleRowCount;
				var items = 0;
				if (gv_other_parts.PageCount > 1)
					{
					max_rows = gv_other_parts.SettingsPager.PageSize;
					}
				for (var i = 0; i < max_rows; i++)
					{
					var tb1 = (ASPxTextBox)gv_other_parts.FindRowCellTemplateControl(i, (GridViewDataColumn)gv_other_parts.Columns["Qty"], "tb_other_qty");
					if (tb1.Text != "")
						{
						if (ddl_cart.Value == null)
							{
							var name = current_user.Nickname + "-" + Toolbox.MySQLNow_long();
							var sc = new shopping_cart { name = name, member_id = current_user.id };
							sc.save();
							ddl_cart.DataBind();
							if (ddl_cart.Items.FindByText(name) != null)
								{
								Session["shopping_cart_active_cart"] = ddl_cart.Items.FindByText(name).Value;
								ddl_cart.Text = name;
								}

							}
						items++;
						try
							{
							var sci = new shopping_cart.item 
										{ 
										shopping_cart_header_id = (int) ddl_cart.Value, 
										master_id = Convert.ToInt32(gv_other_parts.GetRowValues(i, "master_id")), 
										qty = Convert.ToDouble(tb1.Text), 
										member_id = current_user.id 
										};
							sci.save();
							Session["shopping_cart_full_gv"] = null;
							}
						catch
							{
							tb1.Text = "";

							}
						}

					cb_search.JSProperties["cp_alert"] = items + " added to work order";
					}
				fill_grid();

				Session["shopping_cart_other_full_gv"] = null;
				update_other_parts_gv();
				Session["shopping_cart_full_gv"] = null;
				update_cart_gv();
				gv_other_parts.JSProperties["cp_refresh_other_grid"] = 1;
				break;
			}
		}
	protected void gv_cart_FocusedRowChanged(object _sender, EventArgs _e)
		{

		}
	protected void ddl_type_id_Callback(object _sender, CallbackEventArgsBase _e)
		{
//		if (_e.Parameter != null && _e.Parameter.Equals("3")) // group
//			{
//			ddl_type_id.DataSource = Toolbox.doSQL_dt(conn,@"Select distinct igh.id,igh.name from inventory_group_hdr igh inner join inventory_group_dtl on inventory_group_dtl.group_id = igh.id order by igh.name "  , null);
//			}
//		else if (_e.Parameter != null && _e.Parameter.Equals("4")) // kit
//			{
//			ddl_type_id.DataSource = Toolbox.doSQL_dt(conn,@"Select distinct igh.inventory_kit_hdr_id id,igh.inventory_kit_hdr_name name from inventory_kit_hdr igh inner join inventory_kit_dtl on inventory_kit_dtl.inventory_kit_dtl_hdr_id = igh.inventory_kit_hdr_id order by igh.inventory_kit_hdr_name "  , null);
//			}
		 if (_e.Parameter != null && _e.Parameter.Equals("3")) // wo
			{
                ddl_type_id.DataSource = Toolbox.doSQL_dt(conn,@"Select woprog_id id,concat('0',woprog_bvwo/1,' - ',woprog_customername,' - ',woprog_description) name from woprog  where business_unit_id =@v0 order by woprog_id desc limit 100", new object[] { WorkingBusinessUnit.id });
			}
		else if (_e.Parameter != null && _e.Parameter.Equals("4")) // quote
			{
			ddl_type_id.DataSource = Toolbox.doSQL_dt(conn,@"SELECT DISTINCT quote_master.quote_id id, concat(quote_master.quote_id, ' - ' ,customer.customer_name,' - ', quote_master.job_description) name FROM quote_worksheet INNER JOIN quote_master ON quote_worksheet.quote_id = quote_master.quote_id INNER JOIN customer ON quote_master.customer_id = customer.customer_id  WHERE quote_master.business_unit_id =@v0 ORDER BY quote_master.quote_id DESC limit 400", new object[] { WorkingBusinessUnit.id });
			}
		ddl_type_id.DataBind();
		}
	protected void tb_car_qty_Init(object _sender, EventArgs _e)
		{
		var qty = _sender as ASPxTextBox;
		var container = qty.NamingContainer as GridViewDataItemTemplateContainer;
		qty.ClientSideEvents.TextChanged = "function (s,e) {{gv_cart.PerformCallback('qty|" + container.KeyValue + "|' + s.GetText());}}";
		}
	protected void btn_add_to_cart_Init(object _sender, EventArgs _e)
		{

		}
	
	protected void ddl_destination_type_SelectedIndexChanged(object _sender, EventArgs _e)
		{


		}
	protected void btn_search_Click(object _sender, System.Web.UI.ImageClickEventArgs _e)
		{
		//tb.Tokens.Clear();
		update_navbar(true);
		var type_id = (int) ddl_type.Value;
		if (Toolbox.Contains(type_id, new []{3,4,6}))
			{
			ddl_type_id.ClientVisible = true;
			tb_search.Style["display"] = "none";
			}
		else
			{
			ddl_type_id.ClientVisible = false;
			tb_search.Style["display"] = "block";
			}
		}
	protected void tb_commit_Init(object _sender, EventArgs _e)
		{
		var tb = _sender as ASPxTextBox;
		var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		//tb.JSProperties["cp_max"] = gv_destination.GetRowValues(container.VisibleIndex, "qty_stock");   // String.Format("function (s, e) {{ cb_milestones.PerformCallback('m|{0}|' + s.GetText()); }}", container.KeyValue);

		}
	
	protected void btn_fill_requested_Init(object _sender, EventArgs _e)
		{
		var b = (ASPxButton)_sender;
		//b.JSProperties["cp_rowcount"] = gv_destination.VisibleRowCount;

		}
	protected void tb_require_Load(object _sender, EventArgs _e)
		{
		//var tb = (ASPxTextBox)_sender;
		//var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		//tb.ClientInstanceName = "cs_qty_cart_" + container.VisibleIndex;
		//tb.JSProperties["cp_cartqty"] = gv_destination.GetRowValues(container.VisibleIndex, "qty_req").ToString();
		//
		}
	protected void tb_stock_to_wo_Load(object _sender, EventArgs _e)
		{
		//var tb = (ASPxTextBox)_sender;
		//var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		//tb.ClientInstanceName = "cs_qty_cart_stock_to_wo_" + container.VisibleIndex;
		//var qty_req = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_req"));
		//var qty_stock = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_stock"));
		//var qty_min_temp = (Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_on_wo")) * -1);
		//tb.JSProperties["cp_max"] = qty_stock.ToString();
		//tb.JSProperties["cp_cartqty"] = (qty_req <= qty_stock) ? qty_req.ToString() : qty_stock.ToString();
		//tb.JSProperties["cp_min"] = (qty_req < qty_min_temp) ? qty_min_temp.ToString() : qty_req.ToString();

		}
	protected void tb_stock_to_truck_Load(object _sender, EventArgs _e)
		{
		//var tb = (ASPxTextBox)_sender;
		//var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		//tb.ClientInstanceName = "cs_qty_cart_stock_to_truck_" + container.VisibleIndex;
		//var qty_req = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_req"));
		//var qty_stock = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_stock"));
		//var qty_min_temp = (Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_truck")) * -1);
		//tb.JSProperties["cp_cartqty"] = (qty_req <= qty_stock) ? qty_req.ToString() : qty_stock.ToString();
		//tb.JSProperties["cp_min"] = (qty_req < qty_min_temp) ? qty_min_temp.ToString() : qty_req.ToString();
		//tb.JSProperties["cp_max"] = gv_destination.GetRowValues(container.VisibleIndex, "qty_stock").ToString();

		}
	protected void tb_truck_to_wo_Load(object _sender, EventArgs _e)
		{
		//var tb = (ASPxTextBox)_sender;
		//var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		//tb.ClientInstanceName = "cs_qty_cart_truck_to_wo_" + container.VisibleIndex;
		//var qty_req = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_req"));
		//var qty_truck = Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_truck"));
		//var qty_min_temp = (Convert.ToDouble(gv_destination.GetRowValues(container.VisibleIndex, "qty_on_wo")) * -1);
		//tb.JSProperties["cp_cartqty"] = (qty_req <= qty_truck) ? qty_req.ToString() : qty_truck.ToString();
		//tb.JSProperties["cp_min"] = (qty_req < qty_min_temp) ? qty_min_temp.ToString() : qty_req.ToString();
		//tb.JSProperties["cp_max"] = gv_destination.GetRowValues(container.VisibleIndex, "qty_truck").ToString();

		}
	protected void gv_destination_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		//if (_e.Parameters == "go")
		//	{
		//	var i = 0;
		//	while (i < gv_destination.VisibleRowCount)
		//		{
		//		var tb_req = (ASPxTextBox)gv_destination.FindRowCellTemplateControl(i, (GridViewDataColumn)gv_destination.Columns["tb_require"], "tb_require");
		//		if (tb_req.Text != "")
		//			{
		//
		//			}
		//		i++;
		//		}
		//	}
		}
	protected void btn_clear_Load(object _sender, EventArgs _e)
		{

		}
	protected void dt_require_Load(object _sender, EventArgs _e)
		{
		var tb = (ASPxDateEdit)_sender;
		var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		tb.ClientInstanceName = "cs_date_" + container.VisibleIndex;

		tb.JSProperties["cp_date"] = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
		}
	protected void set_columns()
		{
		//var destination		= (int) rdo_list.Value;
		//var gv				= gv_destination;
		//switch(destination)
		//	{
		//	case 0:// request on wo
		//		gv.Columns["qty_truck"].Visible                    = false;
		//		gv.Columns["qty_stock"].Visible                    = false;
		//		gv.Columns["Commit on WO from Truck"].Visible      = false;
		//		gv.Columns["Transfer from stock to Truck"].Visible = false;
		//		gv.Columns["Commit on WO from Stock"].Visible      = false;
		//		gv.Columns["Add to Required Qty"].Visible          = true;
		//		gv.Columns["still_needed"].Visible                 = true;
		//		gv.Columns["qty_on_wo"].Visible                    = true;
		//	break;
		//	case 1:// commit from stcok
		//		gv.Columns["qty_truck"].Visible                    = true;
		//		gv.Columns["qty_stock"].Visible                    = true;
		//		gv.Columns["Commit on WO from Truck"].Visible      = true;
		//		gv.Columns["Transfer from stock to Truck"].Visible = false;
		//		gv.Columns["Commit on WO from Stock"].Visible      = true;
		//		gv.Columns["Add to Required Qty"].Visible          = false;
		//		gv.Columns["still_needed"].Visible                 = true;
		//		gv.Columns["qty_on_wo"].Visible                    = true;
		//	break;
		//	case 2:// transfer to truck
		//		gv.Columns["qty_truck"].Visible                    = true;
		//		gv.Columns["qty_stock"].Visible                    = true;
		//		gv.Columns["Commit on WO from Truck"].Visible      = false;
		//		gv.Columns["Transfer from stock to Truck"].Visible = true;
		//		gv.Columns["Commit on WO from Stock"].Visible      = false;
		//		gv.Columns["Add to Required Qty"].Visible          = false;
		//		gv.Columns["still_needed"].Visible                 = false;
		//		gv.Columns["qty_on_wo"].Visible                    = false;
		//	break;
		//	}
		}

    protected void cb_addpart_Callback(object source, CallbackEventArgs e)
	{
		var json_data = e.Parameter;
		var json = new JavaScriptSerializer();
		var parsed = json.Deserialize<json_cart_object>(json_data);

      
            if (woprog_id != 0)
            {
                var wo = new NeWOProg(woprog_id);
                Session["mobile_cart_woprog_id"] = wo;

                var i = 0;
                foreach (var ol in parsed.data)
                {
                    i++;
                    var master_id = ol.m;
                    var qty_req = ol.q;
                    var qty_from_stock = ol.s;
                    var qty_from_truck = ol.t;
                if (master_id != 0)
                {
                    var inv = new inventory();
                    var part_exists = inv.part_exists(master_id);
                    if (part_exists)
                    {
                        inv.Load(master_id, WarehouseBusinessUnit.id);
                    }
               

                        if (qty_from_stock != 0)  // pull from stock
                        {

                            var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, master_id));
                            add_part_on_wo(wo.woprog_id, master_id, qty_from_stock, "com", current_user.id, il.location_master_id);
                            // This will add the req quantity along with the committed quantity... you will want to bypass the second req block
                        }
                        if (location_id != 0 && qty_from_truck != 0)  // pull from truck
                        {
                            var il = new location(location_id);
                            add_part_on_wo(wo.woprog_id, master_id, qty_from_truck, "com", current_user.id, location_id);

                        }
                        if (qty_req != 0 && qty_from_stock == 0)  // req
                        {

                            add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id), qty_req, "req", current_user.id, 0);
                        }

                        cb_addpart.JSProperties["cp_alert"] = " Parts have been added to WO " + woprog_id;
                        e.Result += master_id + "|" + ol.s + ",";

                  
                 }
                }

            }
            else
            {
               
                e.Result = "";
                var i = 0;
                if (bo_obj.annual_count_on)  // if its an annual count addition
                {
                   
                    foreach (var ol in parsed.data)
                    {
                        i++;
                        var master_id = ol.m;
                        var qty_from_stock = ol.s;
                        if (qty_from_stock != 0 && (master_id != 0) && (location_id != 0))
                        {
                            // if the part does not exist, add the part to the location for that branch
                            var location_to = new location(Convert.ToInt32(location_id), WarehouseBusinessUnit.id, Convert.ToInt32(master_id));  // this sets up the part at that location for that branch, and then loads the location


                            cb_addpart.JSProperties["cp_alert"] = " Parts have been added to " + location_to.this_master.name;

						var sql = @"call get_inventory_qtys_at_location(" + WarehouseBusinessUnit.id + ",'" + bo_obj.annual_count_start_date.ToString("yyyy-MM-dd") + "','" + bo_obj.annual_count_end_date.ToString("yyyy-MM-dd") + "'," + Convert.ToInt32(location_id) + ")";
                            var part_info = Toolbox.doSQL_dt(conn, sql,null).Select("master_id=" + master_id);
							if(part_info.Length == 0)
								{
								var inv		= new inventory();
								inv.Load(master_id, WarehouseBusinessUnit.id);
								if(inv.cost_price_branch == 0)
									{
									throw new Exception(string.Format("Part {0} does not have a cost currently, and cannot be used. This can be fixed by adding vendor information to this part.", master_id));
									}
								else
									{
									throw new Exception(string.Format("Part {0} cannot be used.", master_id));
									}
								}
							var dt = part_info.CopyToDataTable();

                            var is_replace = dt.Rows[0]["last_inv_count"] == DBNull.Value;
                        
                            var old_qty = Convert.ToDouble(dt.Rows[0]["qty"]);
                            var dollar_balance = Convert.ToDouble(dt.Rows[0]["dollar_balance"]);



                            if (location_to.id > 0 && location_id > 0 && Convert.ToInt32(master_id) > 0 )
                            {
                                double qty = 0;
                                var can_convert = double.TryParse(qty_from_stock.ToString(), out qty);

                                if (!can_convert)
                                {
             //                       xfer_lb_warning.Text = "Invalid Quantity Supplied.";
                                    return;
                                }
                                else if (qty < 0)
                                {
                //                    xfer_lb_warning.Text = "Negative Quantity Supplied.";
                                    return;
                                }

                                var newqty = Convert.ToDouble(qty);

                                var ib = new branch(master_id, WarehouseBusinessUnit.id);
                                var prev_qty = location_to.qty;
                                var this_cost = Toolbox.doSQL_double(conn,@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  master_id, WarehouseBusinessUnit.id } );
                                location_to.log_is_manual = true;
                                location_to.member_id = current_user.id;
                                location_to.alert_worthy = false;
                                var diff = is_replace ? location_to.qty - qty: qty;
                                location_to.qty = !is_replace ? location_to.qty + qty : qty;

                                location_to.section_id = 10;
                                var this_type = prev_qty > qty ? 3 : 2;
                                location_to.save();
                                location_to.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
                                var post_ib = new branch(location_to.master_id, location_to.business_unit_id);


                                var list_hist = new List<history_vars>();
                                var h = new history_vars();
                                h.business_unit_id = location_to.business_unit_id;
                                h.cost = Convert.ToDouble(this_cost);

                                h.location_master_id = Convert.ToInt32(location_id);
                                h.master_id = Convert.ToInt32(master_id);
                                h.member_id = current_user.id;
                                h.origin = "mobile";
                                h.post_dollar_balance = is_replace ? (qty * this_cost) : (qty + old_qty) * this_cost;
                                h.post_onhandqty = is_replace ? (qty) : (qty + old_qty);
                                h.pre_dollar_balance = dollar_balance;
                                h.pre_onhandqty = Convert.ToDouble(old_qty);
                                h.qty = qty;
                                h.when_ts = System.DateTime.Now;
                                list_hist.Add(h);
                                add_history_line(list_hist);
                                
                                

                            }

                        }
                    }

                    

                }
                else  // if its a truck stock move
                {
                    foreach (var ol in parsed.data)
                    {
                        i++;
                        var master_id = ol.m;
                        var qty_from_stock = ol.s;
                        if (qty_from_stock != 0 && (master_id != 0) && (location_id != 0))
                        {
                            var _default_internal_location_id = branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id));
                            var il = new location();
                            var location_from = new location(_default_internal_location_id, WarehouseBusinessUnit.id, Convert.ToInt32(master_id));
                            var location_to = new location(Convert.ToInt32(location_id), WarehouseBusinessUnit.id, Convert.ToInt32(master_id));
                            il.section_id = 12;
                            il.xfer((int)location_from.id, (int)location_to.id, Convert.ToDouble(qty_from_stock), current_user);
                            cb_addpart.JSProperties["cp_alert"] = " Parts have been moved";
                            e.Result += master_id + "|" + ol.s + ",";

                        }
                    }
                }
                e.Result = e.Result.TrimEnd(',');
            }
        
		//update_navbar(true);
	}

    private struct history_vars
    {
        public int business_unit_id { get; set; }
        public int master_id { get; set; }
        public double qty { get; set; }
        public int location_master_id { get; set; }
        public int member_id { get; set; }
        public string origin { get; set; }
        public double cost { get; set; }
        public double pre_dollar_balance { get; set; }
        public double post_dollar_balance { get; set; }
        public double pre_onhandqty { get; set; }
        public double post_onhandqty { get; set; }
        public DateTime when_ts { get; set; }
    }
    private void add_history_line(List<history_vars> list_vars)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
	qty,
	locationid,
	memberid,
	date_transferred,
	origin,

	cost,
	prev_dollar_balance,
	post_dollar_balance,
	pre_onhand_qty,
	post_onhand_qty
	)
VALUES 
");
        for (var i = 0; i < list_vars.Count(); i++)
        {
            var hv = (history_vars)list_vars[i];
            sb.AppendFormat(@"	
	(
	'{0}',
	'{1}',
	'{2}',
	'{3}',
	'{4}',
	'{11}',
	'{5}',
	'{6}',
	'{7}',
	'{8}',
	'{9}',
	'{10}'
	)",
    hv.business_unit_id,						// {0}
    hv.master_id,						// {1}
    hv.qty,								// {2}
    hv.location_master_id,				// {3}
    hv.member_id,						// {4}
    hv.origin,							// {5}
    hv.cost,							// {6}
    hv.pre_dollar_balance,				// {7}
    hv.post_dollar_balance,				// {8}
    hv.pre_onhandqty,					// {9}
    hv.post_onhandqty,					// {10}
    Toolbox.MySQL_longdt(hv.when_ts)	// {11}
            );
            if (i < (list_vars.Count() - 1))
            {
                sb.Append(",");
            }
        }
        Toolbox.doSQL_void(conn, sb.ToString(),null);
    }


	protected void add_part_on_wo(int woprog_id, int master_id, double add_qty, string req_or_committed, int member_id, int master_location_id)
	{
		double dbl_required_on_wo = 0;
	
			if (Session["mobile_cart_woprog_id"] == null)
			{
				Session["mobile_cart_woprog_id"] = new NeWOProg(woprog_id);
			}
			var wo = (NeWOProg)Session["mobile_cart_woprog_id"];
			if (Session["mobile_cart_customer"] == null)
			{
				Session["mobile_cart_customer"] = new NECustomer(Convert.ToInt32(wo.WOProg_Customer_ID));
			}
			var cust = (NECustomer)Session["mobile_cart_customer"];
		

		#region validation

		if (wo.Status == "Invoiced" || wo.Status == OpsWOStatus.WaitingToBeInvoiced || wo.Status == "Waiting For PO")
		{
	//		xfer_lb_warning.Text = "You can not add or alter an item on this work order because it has been invoiced/is waiting to be invoiced.";
			return;
		}
		if (master_id == 777 && add_qty == 0)
		{
	//		xfer_lb_warning.Text = "Error";
	//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('777 can only be added to the required amounts on a WO')", "Server"), true);
			return;
		}

		#endregion

		#region prep_variables

		var inv = new inventory();
		double dbl_com_qty_on_wo = 0;
		if (master_id != 777)
		{
			inv.Load(master_id, WarehouseBusinessUnit.id);
			 dbl_com_qty_on_wo = Toolbox.doSQL_double(conn,@"select ifnull((SELECT wo_detail_current_qty_committed FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1  limit 1),0) ", new object[] { wo.woprog_id,master_id });
			 dbl_required_on_wo = Toolbox.doSQL_double(conn,@"select ifnull((SELECT wo_detail_current_qty_ordered FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1  limit 1),0) ", new object[] { wo.woprog_id,master_id });

			 if (req_or_committed == "req")
			 {
				 if ((dbl_required_on_wo - dbl_com_qty_on_wo) == add_qty)
				 {
					 inv = null;
					
					 return;
				 }
			 }
		
		}
		var detail = new NeWODetailCurrent();
		var so = new NeSalesOrder();
		
		var line_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(wo_detail_current_id), 0)
FROM wo_detail_current 
WHERE wo_detail_current_woprog_id = @v0 AND wo_detail_current_master_id =@v1  LIMIT 1", new object[] { wo.woprog_id,master_id});
		if (line_id != 0 && master_id != 777)
		{
			detail = new NeWODetailCurrent(line_id);
		}

		#endregion

		#region write data to wo

		so.working_line_id = master_id == 777 ? 0 : line_id;
		so.forcenewpartline = false;
		so.PartNo = master_id.ToString();

		var bd = new bingo_data();
		bd.before_cost = detail.cost;
		bd.before_qty = detail.qty_committed;
		bd.before_sell = detail.sell;
		bd.detail_id = detail.id;
		bd.master_id = detail.master_id;
		bd.woprog_id = detail.woprog_id;
		bd.ca_qty = add_qty;
		var do_bd_save = true;
		so.Desc = string.IsNullOrEmpty(detail.description) ? inv.description_int : detail.description;
		if (req_or_committed == "req") // Cost or sell should not be changed 
		{
			so.OrderedQuantity = add_qty;
			so.ManualChange = true;
			//	so.DateRequired = d.ToString("yyyy-MM-dd");   ** matt to fill in 
			so.SellOverride = master_id == 777 ? 0 : detail.sell;
			so.CostOverRide = master_id == 777 ? 0 : detail.cost;
			so.forcenewpartline = master_id == 777;
			do_bd_save = false;
		}
		else if (req_or_committed == "com")
		{
			so.Quantity = add_qty;
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
			{
				temp_qty = (dbl_com_qty_on_wo + so.Quantity) <= 0 ? 1 : (dbl_com_qty_on_wo + so.Quantity);
				temp_cost = Math.Round(Toolbox.doSQL_double(conn,@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )",  new object[] { detail.master_id, WarehouseBusinessUnit.id, temp_qty }),3 );
				if (detail.qty_committed > 0)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(conn,@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )",  new object[] {  detail.woprog_id, detail.master_id, so.Quantity, temp_cost }),3 );
				}
				bd.ca_cost = temp_cost;
			}
			else
			{
				temp_qty = dbl_com_qty_on_wo + so.Quantity;
				if (so.Quantity > 0 && detail.qty_committed > 0 && detail.cost != inv.cost_price_branch)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(conn,@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { detail.woprog_id, detail.master_id, so.Quantity, inv.cost_price_branch }),3 );
					bd.ca_cost = temp_cost;
				}
				else
				{
					temp_cost = Math.Round(inv.cost_price_branch, 2, MidpointRounding.AwayFromZero);
					bd.ca_cost = temp_cost;
				}
			}
			if ((dbl_required_on_wo < temp_qty) && (so.Quantity > 0))
			{
				so.OrderedQuantity = (temp_qty - dbl_required_on_wo);
			}
			so.CostOverRide = temp_cost;
			so.SellOverride = wo.use_fixed_material_markup?wo.fixed_material_markup* so.CostOverRide: shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
		}
		else if (req_or_committed == "ret")
		{
			so.OrderedQuantity = Math.Abs(add_qty) * -1;
			so.Quantity = Math.Abs(add_qty) * -1;
			so.ActualQuantity = Math.Abs(add_qty);
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
			{
				temp_qty = (dbl_com_qty_on_wo + so.Quantity) <= 0 ? 1 : (dbl_com_qty_on_wo + so.Quantity);
				temp_cost = Math.Round(Toolbox.doSQL_double(conn,@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )",  new object[] { detail.master_id, WarehouseBusinessUnit.id, temp_qty }),3 );
				if (detail.qty_committed > 0)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(conn,@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )",  new object[] { detail.woprog_id, detail.master_id, so.Quantity, temp_cost }),3);
				}
			}
			else
			{
				temp_qty = dbl_com_qty_on_wo + so.Quantity;
				temp_cost = detail.cost;
			}
			so.CostOverRide = temp_cost;
			bd.ca_cost = temp_cost;
			bd.ca_qty = temp_qty;
			so.SellOverride = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
		}
		if (do_bd_save)
		{
			bd.ca_sell = so.SellOverride;
			bd.after_cost = so.CostOverRide;
			bd.after_sell = so.SellOverride;
			bd.after_qty = dbl_com_qty_on_wo + so.Quantity;
			bd.save();
		}
		so.BillingTypeID = wo.QuoteID != "0" ? 1 : 0;
		so.CustomerDiscount = wo.woprog_apply_discount == 0 ? 0 : Convert.ToDouble(cust.Discount);
		var ilm = new location_master();
		if (master_id != 777)
		{
			so.Desc = detail.description == null ? inv.description : detail.description;
			switch (req_or_committed)
			{
				case "com":
					ilm = new location_master(master_location_id);
					so.committing_from_location = ilm.name;
					break;
				case "ret":
					ilm = new location_master(master_location_id);
					so.uncommitting_to_location = ilm.name;
					break;
			}
		}
		else
		{
				so.Desc = tb_777_desc.Text; 
		}
		var recnumber = 0;

		try
		{
			recnumber = so.SavePart(wo.woprog_id, "FALSE", WorkingBusinessUnit.DSN, current_user.FullName, current_user.id, (line_id == 0 || master_id == 777), "");
	//		if (master_id != 777)
	//		{
				var woc = new NeWOProgChanges();
				woc.WoProgChanges_WOProg_ID = (int)wo.woprog_id;
				woc.WoProgChanges_BVWO = wo.OrderNumber;
				woc.WoProgChanges_BVWORec = recnumber.ToString();
				woc.WoProgChanges_DateTime = Toolbox.MySQLNow_long();
				woc.WoProgChanges_Modified_Member_ID = current_user.id;
				woc.WoProgChanges_WOProgComment_ID = 0;
				woc.WoProgChanges_WasPartNo = detail.master_id.ToString();
				woc.WoProgChanges_WasPrice = detail.sell.ToString();
				woc.WoProgChanges_WasQty = detail.qty_committed.ToString();
				woc.WoProgChanges_IsPartNo = so.PartNo;
				woc.WoProgChanges_IsPrice = so.SellOverride.ToString();
				woc.WoProgChanges_IsQty = (detail.qty_committed + so.Quantity).ToString();
				woc.WoProgChanges_DeleteFlag = "false";
				woc.WoProgChanges_WasDesc = master_id==777?"": detail.description;
				woc.WoProgChanges_IsDesc = master_id == 777 ? tb_777_desc.Text : inv.description_int;
				woc.business_unit_id = WorkingBusinessUnit.id;
				woc.WOProgChanges_WasBillingType = detail.billtypeid;
				woc.WoProgChanges_IsBillingType = so.BillingTypeID;
				woc.WOProgChanges_OrderedQty = (detail.qty_ordered + so.OrderedQuantity).ToString();
				woc.WOProgChanges_ManualPriceChange = 0;
				woc.was_req_qty = detail.qty_ordered;
				if (req_or_committed == "com" || req_or_committed == "ret")
				{
					woc.FullWOComment = req_or_committed == "com" ? "Committed from Location: " + ilm.name : "Returned to Location: " + ilm.name;
				}
				else if (req_or_committed == "req")
				{
					woc.FullWOComment = "Requested " + (detail.qty_ordered != 0 ? " another " : "") + so.OrderedQuantity + " of " + so.PartNo;
				}
				woc.AddtoWOProgChanges();
				NeWOProg.update_header_totals(wo.woprog_id.ToString(), WorkingBusinessUnit.id, wo.OrderNumber);
	//		}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		if (Convert.ToInt32(master_id) < 990000 && !inv.is_exclude && inv.allowed_to_stock)
		{
			//	inv.Load(global_pv.master_id, current_user.business_unit_id);

			var fromstock = new Nestock_transfer();
			fromstock.type_id = 1;
			fromstock.master_id = master_id;
			fromstock.description = inv.description;
			fromstock.business_unit_id = WarehouseBusinessUnit.id;
			fromstock.member_id = current_user.id;
			fromstock.stock_transfer_log_section_id = 19;

			if (req_or_committed == "ret")
			{
				fromstock.to_id = 0;
				fromstock.from_id = wo.woprog_id;
				fromstock.to_id = master_location_id; // int_ is the FROM ddl
				fromstock.to_location_id = master_location_id;
				
				fromstock.quantity = add_qty;
				fromstock.note = "Returned to stock via mobile";
				fromstock.cost = detail.cost > 0 ? detail.cost : inv.cost_price_branch;
			}
			else if (req_or_committed == "com")
			{
				fromstock.to_id = wo.woprog_id;
				fromstock.from_id = master_location_id; // int_ is the FROM ddl
				fromstock.from_location_id = master_location_id;
				fromstock.quantity = add_qty * -1;
				fromstock.note = "committed via mobile";
				fromstock.cost = inv.cost_price_branch;
			}
			if (req_or_committed != "req")
			{
				fromstock.Nestock_transfer_save();
			}
		}





		#endregion

	}



	public struct json_cart_object
		{
		public List<json_cart_object_line> data {get;set;}
		}
	public struct json_cart_object_line
		{
		public int m {get;set;}
		public double q {get;set;}
		public double s { get; set; }
		public double t { get; set; }
		}
	protected void cbp_header_Callback(object sender, CallbackEventArgsBase e)
		{
		Session["shopping_cart_full_gv"] = null;
		update_cart_gv();
		}
	protected void btn_show_cart_Click(object sender, EventArgs e)
	{
		if (btn_show_cart.Text == "View Cart")
		{
			mv.ActiveViewIndex = 1;
			
		}
		else
		{
			mv.ActiveViewIndex = 0;
			
		}
		update_view();
	}
	protected void update_view()
	{
		if (mv.ActiveViewIndex == 1)
		{
			btn_show_cart.Text = "Add Parts";
		}
		else
		{
			
			btn_show_cart.Text = "View Cart";
		}
	}

	protected void cb_777_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter != "")
		{
			if (tb_777_qty.Text != "" && tb_777_desc.Text != "")
			{
				add_part_on_wo(woprog_id, 777, Convert.ToDouble(tb_777_qty.Text), "req", current_user.id, 0);
				cb_777.JSProperties["cp_close"] = "close";
				
			}
			else
			{
				if (tb_777_desc.Text=="")
				{
					cb_777.JSProperties["cp_alert"] = "You must enter a valid description when requesting a 777 part number";
				}
				else if(tb_777_qty.Text=="")
				{
					cb_777.JSProperties["cp_alert"] = "You must enter a valid qty";
				}
			}
		}
	}

	protected void cb_part_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter != "")
		{
			if (!e.Parameter.Contains("|"))
			{
				var i = new inventory();
				i.Load(e.Parameter, WarehouseBusinessUnit.id);
				var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));
				lbl_pop_part.Text = i.master_id;
				lbl_pop_description.Text = i.description_full;
				if (i.master_id == "777")
				{
					lbl_pop_qty_onhand.Text = "0";
					lbl_pop_qty_external.Text = "0";
					lbl_pop_qty_onorder.Text = "0";
					img_pop_part.ImageUrl = "";
					img_pop_part.DataBind();
				}
				else
				{
					lbl_pop_qty_onhand.Text = il.qty + " (*)";
					lbl_pop_qty_external.Text = Toolbox.doSQL_string(conn, @"select ifnull((Select il.qty from inventory_location il
where il.location_master_id =@v0 and il.master_id=@v1 and il.business_unit_id=@v2),0)", new object[] { location_id,i.master_id,WarehouseBusinessUnit.id});
					lbl_pop_qty_onorder.Text = i.po_usage.ToString();
					img_pop_part.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + e.Parameter;
					img_pop_part.DataBind();
				}
				if (woprog_id != 0)
				{
					lbl_pop_qty_onwos.Text = Toolbox.doSQL_string(conn, @"select ifnull((Select a.wo_detail_current_qty_committed from wo_detail_current a 
where a.wo_detail_current_woprog_id = @v0 and wo_detail_current_master_id=@v1 limit 1),0)", new object[] {woprog_id, i.master_id});
				}
				else
				{
					lbl_pop_qty_onwos.Text = Toolbox.doSQL_string(conn, @"select ifnull((Select a.wo_detail_current_qty_committed 
from wo_detail_current a where a.business_unit_id = @v0 and wo_detail_current_master_id=@v1 limit 1),0)",
						new object[] {
WorkingBusinessUnit.id,i.master_id
							}
);
				}
				lbl_pop_location.Text = i.location_name.Replace("'", "");
			}
			else
			{
				var i = new inventory();
				i.Load(e.Parameter.Split('|').GetValue(1), WarehouseBusinessUnit.id);
				var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));

				i.print_barcode_label(1);
				lbl_pop_part.Text = i.master_id;
				lbl_pop_description.Text = i.description_full;
				lbl_pop_qty_onhand.Text = il.qty + " (*)";
				lbl_pop_qty_external.Text = i.ext_onhand_qty.ToString();
				lbl_pop_qty_onwos.Text = i.wo_usage.ToString();
				lbl_pop_qty_onorder.Text = i.po_usage.ToString();
				lbl_pop_location.Text = i.location_name.Replace("'", "");
				img_pop_part.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + e.Parameter;
				img_pop_part.DataBind();
			}

			
		}
	}

	protected void cb_fix_qty_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter != "save")
		{
			var i = new inventory();
            i.Load(e.Parameter, WarehouseBusinessUnit.id);
			var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));

			//hdn_fix_qty_internal_location_id.Value
			lbl_fix_part.Text = i.master_id;
			lbl_fix_description.Text = i.description_full;
			lbl_fix_qty_insystem.Text = il.qty.ToString();
			hdn_fix_qty_internal_location_id.Value = il.id.ToString();

			lbl_fix_location.Text = i.location_name.Replace("'", "");
			Session["fix_master_id"] = i.master_id;
			Session["fix_location_id"] = hdn_fix_qty_internal_location_id.Value;

		}
		else
		{
			double new_qty = 0;
			var master_id = Convert.ToInt32(Session["fix_master_id"]);
			double.TryParse(txt_fix_actual_qty.Text, out new_qty);
			var location_id = Convert.ToInt32(Session["fix_location_id"]);
			if (master_id != 0 && location_id != 0 && txt_fix_actual_qty.Text != "")
			{
				var il = new location(location_id, WarehouseBusinessUnit.id, master_id);
				var ilm = new location_master(il.location_master_id);
				var em = new NeEMail();
				em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				em.To = WarehouseBusinessUnit.purchaser.NEEmail == "" ? WorkingBusinessUnit.branch_manager.NEEmail : WarehouseBusinessUnit.purchaser.NEEmail;
				if (WarehouseBusinessUnit.id == 1)
				{
					em.CC = "oakshipper@" + Toolbox.app_setting("DomainForEmail");
				}
				em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
				em.isHTML = true;
				var wo_n = woprog_id == null ? "N/A" : woprog_id.ToString();
				em.Body = string.Format(@"
<table cellpadding='3' cellspacing='0' style='font-family:arial;font-size:12px;'>
	<tr>
		<td width='150'><b>Reportee: </b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Master ID: </b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Location: </b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Work Order (if applicable): </b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Current System Qty: </b></td>
		<td>{3}</td>
	</tr>
<tr>
		<td><b>Reported Qty: </b></td>
		<td>{5}</td>
	</tr>
</table>
", current_user.FullName, master_id, ilm.name, il.qty, wo_n, txt_fix_actual_qty.Text);
				//		em.Bcc = "aketelaars@newelectric.com";
				em.Send();

				var iil = new incorrect_levels();
				iil.master_id = master_id;
				iil.location_id = location_id;
				iil.incorrect_qty = il.qty;
				iil.reported_qty = Convert.ToDouble(txt_fix_actual_qty.Text);
				iil.member_id = current_user.id;
				iil.date = System.DateTime.Today;
				iil.id = 0;
				iil.save();
				cb_fix_qty.JSProperties["cp_close"] = "1";


			}
			else
			{



			}



		}
	}

}
