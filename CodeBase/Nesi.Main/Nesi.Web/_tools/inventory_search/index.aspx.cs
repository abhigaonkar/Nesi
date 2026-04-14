using System;
using System.Data;
using System.Web;
using System.Text;
using nesi.core;
using NESI.Common.Models;

public partial class this_inv_search : System.Web.UI.Page
	{
	NeMember _member;
	public NeBusinessUnit BusinessUnit  { get; set; }
	bool is_stocked;

    protected void Page_Load(object sender, EventArgs e)
		{
		if(Session["profile"] == null)
			{
			Toolbox.do_set_XML_header(Response);
			Toolbox.QuickReponse(Response, "<critical>You're not logged in.</critical>");
			return;
			}
		_member					= Session["profile"] == null 
									? Toolbox.do_handle_authentication(OpsPage.Home) 
									: (NeMember) Session["profile"];
		var _inventory			= new inventory();
		var _q					= Request.QueryString;
		int.TryParse(_q["q"], out var partNo);
		if(Toolbox.Contains(partNo, new []{	OpsSpecialPart.AssetLine, 
											OpsSpecialPart.CompanyCreditCardExpense, 
											OpsSpecialPart.ExpenseReimbursement, 
											OpsSpecialPart.PerDiem,
											OpsSpecialPart.NewChildWO,
											OpsSpecialPart.QuoteLine,
											OpsSpecialPart.OldSubContractor
											}))
			{
			Toolbox.do_set_XML_header(Response);
			Toolbox.QuickReponse(Response, "<critical>Invalid search - Part not allowed for direct addition</critical>");
			return;
			}
		var output				= "";
		var show_cost			= _member.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders);
		var show_sell			= _member.AuthenticatedForPrivilege(OpsPrivilege.ViewGrossMarginInformation);
		var _dt					= new DataTable();
		is_stocked	 			= _q["is_stocked"] == "true";
		var wo_usage			= _q["wo_usage"] == "true";
		var po_usage			= _q["po_usage"] == "true";
		var total				= 0;
		var origin				= _q["origin"] ?? "";
		var search_type			= _q["search_type"] ?? "";
		var allow_gl			= _q["allow_gl"] == null || Convert.ToBoolean(_q["allow_gl"]);
		var allow_nonstock		= _q["allow_nonstock"] == null || Convert.ToBoolean(_q["allow_nonstock"]);
		var show_nosell			= _q["show_nosell"] == null || Convert.ToBoolean(_q["show_nosell"]);
		var start				= DateTime.Now;
		var length				= _q["length_of"] != null ? Convert.ToInt32(_q["length_of"]) : 100;
		var business_unit_id	= 0;

		DataTable _subdt;
		var _dt_results = new DataTable();
		object tag_id;
		Toolbox.do_set_XML_header(Response);
		if(!string.IsNullOrEmpty(_q["business_unit_id"]) && _q["business_unit_id"] != "null")
			{
			business_unit_id			= Convert.ToInt32(_q["business_unit_id"]);
			}
		else if(Session["working_business_unit_id"] != null)
			{
			business_unit_id			= Convert.ToInt32(Session["working_business_unit_id"]);
			}
		else
			{
			business_unit_id			= _member.business_unit.id;
			}
		BusinessUnit		= new NeBusinessUnit(business_unit_id);
		var sb		= new StringBuilder();
		Toolbox.do_set_XML_header(Response);
		Response.Clear();
		if(_q["a"] == null)
			{
			if(_q["q"] != null)
				{
				#region SEARCH MySQL
					var keywords						= Toolbox.do_value_from(_q["q"], false);
					_dt									= inventory.search_results(keywords, BusinessUnit.warehouse_bu_id, search_type, allow_gl, allow_nonstock, is_stocked);
					var vendor_id						= !string.IsNullOrEmpty(_q["vendor_id"]) && _q["vendor_id"] != "null" ? Convert.ToInt32(_q["vendor_id"]) : 0;
					total								= _dt.Rows.Count;
					if(_dt.Rows.Count > 0)
						{
						var id_list			= "";
						var _dv			= _dt.DefaultView;
						_dv.Sort				= wo_usage ? "wo_usage DESC, min_qty DESC, onhand DESC" : po_usage ? "po_usage DESC, min_qty DESC, onhand DESC" : "wo_usage DESC, min_qty DESC, onhand DESC";
						_dt						= _dv.ToTable();
							
						foreach(DataRow _dr in _dt.Rows)
							{
							var woUsage				= Convert.ToDouble(_dr["wo_usage"]);
							var poUsage				= Convert.ToDouble(_dr["po_usage"]);
							if(po_usage && poUsage == 0)
								{
								total--;
								continue;
								}
							else if(wo_usage && woUsage == 0)
								{
								total--;
								continue;
								}
							id_list				+= _dr["master_id"].ToString();
							id_list				+= _dr != _dt.Rows[_dt.Rows.Count-1] ? "," : "";
							}
						var _s				= DateTime.Now;
						if(id_list != "")
							{
							_inventory.Load(id_list, business_unit_id, true);
							}
						else
							{
							_inventory.parts		= new System.Collections.ArrayList();
							}
						var _e				= DateTime.Now;
						var _t				= _e.Subtract(_s);
						
						_dt_results.Columns.Add("master_id");
						_dt_results.Columns.Add("description");
						_dt_results.Columns.Add("sell_price");
						_dt_results.Columns.Add("cost_price_branch");
						_dt_results.Columns.Add("tag_id");
						_dt_results.Columns.Add("tag_name");
						_dt_results.Columns.Add("sold_as");
						_dt_results.Columns.Add("ttl_ms");
						_dt_results.Columns.Add("bv_part_number");
						_dt_results.Columns.Add("business_unit_id");
						_dt_results.Columns.Add("is_qty");
						_dt_results.Columns.Add("vendor_code");
						_dt_results.Columns.Add("vendor_qty");
						_dt_results.Columns.Add("vendor_sell");
						_dt_results.Columns.Add("shown_vendor_date");
						_dt_results.Columns.Add("onhand_qty");
						_dt_results.Columns.Add("is_exclude");
						_dt_results.Columns.Add("show_sell");
						_dt_results.Columns.Add("show_cost");
						_dt_results.Columns.Add("has_pic");
						_dt_results.Columns.Add("has_minmax");
						_dt_results.Columns.Add("is_stocked");
						_dt_results.Columns.Add("int_onhand_qty");
						_dt_results.Columns.Add("ext_onhand_qty");
						_dt_results.Columns.Add("total_int_onhand_qty");
						_dt_results.Columns.Add("total_ext_onhand_qty");
						_dt_results.Columns.Add("wo_usage");
						_dt_results.Columns.Add("po_usage");
						_dt_results.Columns.Add("min_qty");
				
		
						foreach(inventory i in _inventory.parts)
							{
							var sub_start		= DateTime.Now;
							object master_id		= i.master_id;
							tag_id					= i.tag_id;
							    if (!string.IsNullOrEmpty(origin) && origin == "workorder" && i.is_exclude)
							    {
                                    // Not show excluded items from search result.
                                    continue;
							    }
						if (!string.IsNullOrEmpty(origin) && origin == "purchaseorder" && i.master_id == OpsSpecialPart.MiscMaterial.ToString())
						{
							// Not show excluded items from search result.
							continue;
						}
						if (tag_id != null)
								{
								object tag_name		= HttpUtility.HtmlEncode(_inventory.tag_name);
								object description	= HttpUtility.HtmlEncode(i.description_full);
								object sold_as		= BusinessUnit.country == "CDN" ? i.sold_as_canadian_name : i.sold_as_usa_name;
								if(description.ToString() == "")
									{
									description		= "No description available for this part";
									sold_as			= null;
									}
								double vendor_sell	= 0;
								var vendor_code	= "";
								double vendor_qty	= 0;
								if(vendor_id != 0)
									{
									var _vendor_info	= Toolbox.doSQL_dt(@" SELECT a.total, a.vendor_code, a.qty FROM inventory_price a LEFT JOIN vendor b ON a.vendor_id = b.vendor_id WHERE a.master_id = @v0  AND a.business_unit_id = @v2  AND b.vendor_id = @v1  LIMIT 1", new [] {  master_id, vendor_id, BusinessUnit.warehouse_bu_id } );
									foreach(DataRow _v in _vendor_info.Rows)
										{
										vendor_sell			= Convert.ToDouble(_v["total"]);
										vendor_code			= _v["vendor_code"].ToString();
										vendor_qty			= Convert.ToDouble(_v["qty"]);
										} 
									}
								var sub_finish	= DateTime.Now;
								var _sub_diff	= sub_finish.Subtract(sub_start);
								i.cost_price_branch		= show_cost && origin == "workorder" ? Math.Round(i.cost_price_branch, 3) : show_cost ? i.cost_price_branch : 0;	
								i.sell_price				= origin == "workorder" && i.cost_price_branch > 0 ? shared.GetSellPrice(i.cost_price_branch, 0, i.is_qty, 1, BusinessUnit.id32) : i.sell_price;
								var shown_vendor_date	= "";
								try
									{
									if(i.vendor_price_last_dt != "--" && i.vendor_price_last_dt != "")
										{
										shown_vendor_date		= Convert.ToDateTime(i.vendor_price_last_dt).ToString("yyyy-MM-dd");
										}
									}
								catch
									{
									shown_vendor_date		= "--";
									}

						var newrow						= _dt_results.NewRow();
						newrow["master_id"]				= master_id;
						newrow["description"]			= description;
						newrow["sell_price"]			= i.sell_price;
						newrow["cost_price_branch"]		= i.cost_price_branch;
						newrow["tag_id"]				= i.tag_id;
						newrow["tag_name"]				= tag_name;
						newrow["sold_as"]				= sold_as;
						newrow["ttl_ms"]				= i.ttl_ms;
						newrow["bv_part_number"]		= i.bv_part_number;
						newrow["business_unit_id"]		= BusinessUnit.warehouse_bu_id;
						newrow["is_qty"]				= i.is_qty;
						newrow["vendor_code"]			= vendor_code;
						newrow["vendor_qty"]			= vendor_qty;
						newrow["vendor_sell"]			= vendor_sell;
						newrow["shown_vendor_date"]		= shown_vendor_date;
						newrow["onhand_qty"]			= i.onhand_qty;
						newrow["is_exclude"]			= i.is_exclude;
						newrow["show_sell"]				= show_sell;
						newrow["show_cost"]				= show_cost;
						newrow["has_pic"]				= i.has_pic;
						newrow["has_minmax"]			= (_inventory.n_minmax > 0);
						newrow["is_stocked"]			= i.is_stocked;
						newrow["int_onhand_qty"]		= i.int_onhand_qty;
						newrow["ext_onhand_qty"]		= i.ext_onhand_qty;
						newrow["total_int_onhand_qty"]  = i.total_int_onhand_qty;
						newrow["total_ext_onhand_qty"] = i.total_ext_onhand_qty;
						newrow["wo_usage"]				= i.wo_usage;
						newrow["po_usage"]				= i.po_usage;
						newrow["min_qty"]				= i.minqty;

						_dt_results.Rows.Add(newrow);

									}
							else
								{
								output			= "<error>Query produced (0) results</error>";
								}
							}

						var _dv1 = _dt_results.DefaultView;
						_dv1.Sort = wo_usage ? "wo_usage DESC, min_qty DESC, int_onhand_qty DESC" : po_usage ? "po_usage DESC, min_qty DESC, int_onhand_qty DESC" : "wo_usage DESC, min_qty DESC, int_onhand_qty DESC";
						_dt_results = _dv1.ToTable();

						foreach (DataRow dr in _dt_results.Rows)
						{
							sb.AppendFormat(@"
<part>
	<master_id>{0}</master_id>
    <description>{1}</description>
    <sellprice>{2}</sellprice>
    <costprice>{3}</costprice>
    <tag_id>{4}</tag_id>
    <tag_name>{5}</tag_name>
    <sold_as>{6}</sold_as> 
    <row_seconds>{7}</row_seconds>
    <bv_code>{8}</bv_code>
    <to_business_unit_id>{9}</to_business_unit_id>
    <is_QTY>{10}</is_QTY>
    <vendor_code>{11}</vendor_code>
    <vendor_qty>{12}</vendor_qty>
    <vendor_sell>{13}</vendor_sell>
    <last_vendor_dt>{14}</last_vendor_dt>
    <onhand_qty>{15}</onhand_qty>
    <is_exclude>{16}</is_exclude>
	<show_sell>{17}</show_sell>
	<show_cost>{18}</show_cost>
	<has_pic>{19}</has_pic>
	<has_minmax>{20}</has_minmax>
	<is_stocked>{21}</is_stocked>
	<int_onhand_qty>{22}</int_onhand_qty>
	<ext_onhand_qty>{23}</ext_onhand_qty>
	<total_int_onhand_qty>{24}</total_int_onhand_qty>
	<total_ext_onhand_qty>{25}</total_ext_onhand_qty>
</part>",
				dr["master_id"],							 // {0}
				dr["description"],						 // {1}
				dr["sell_price"],						 // {2}
				dr["cost_price_branch"],				 // {3}
				dr["tag_id"],							 // {4}
				dr["tag_name"],							 // {5}
				dr["sold_as"],							 // {6}
				dr["ttl_ms"],							 // {7}
				dr["bv_part_number"],					 // {8}
				dr["business_unit_id"],					 // {9}
				dr["is_qty"],							 // {10}
				dr["vendor_code"],						 // {11}
				dr["vendor_qty"],							 // {12}
				dr["vendor_sell"],						 // {13}
				dr["shown_vendor_date"],			    	 // {14}
				dr["onhand_qty"],				    	 // {15}
				dr["is_exclude"],			        	 // {16}
				dr["show_sell"],							 // {17}
				dr["show_cost"],							 // {18}
				dr["has_pic"],							 // {19}
				dr["has_minmax"],			 // {20}
				dr["is_stocked"],						// {21}
				dr["int_onhand_qty"], // {22}
				dr["ext_onhand_qty"], //23
				dr["total_int_onhand_qty"], // {24}
				dr["total_ext_onhand_qty"] //25
				);


						}

						var finish			= DateTime.Now;
						var _difference	= finish.Subtract(start);
						output					= $"\n<parts start='{start}' length='{length}' total='{total}' querytime='{_difference.TotalMilliseconds}' objectload='{_t.TotalMilliseconds}'>{sb}\n</parts>";
						}
					else
						{
						output			= "<error>Query produced (0) results</error>";
						}
					#endregion
				}
			else
				{
				sb.Append("<critical>Bad Request</critical>");
				}
			}
		else
			{
			switch(_q["a"])
				{
				#region xml_tag_att_val
				case "xml_tag_att_val":
					Toolbox.do_dont_cache_page(Response);
					_dt							= attributes(_q["tag_id"]);
					if(_dt.Rows.Count > 0)
						{
						output					+= "<attvals>";
						foreach(DataRow _dr in _dt.Rows)
							{
							output				+= $@"<attribute id=""{_dr["attribute_id"]}"" name=""{Server.HtmlEncode(_dr["attribute"].ToString())}"">";
							_subdt				= tag_attribute_selected_values(_q["tag_id"], _dr["attribute_id"]);
							foreach(DataRow _subdr in _subdt.Rows)
								{
								output			+= $@"<value id=""{_subdr["attribute_value_id"]}"" name=""{Server.HtmlEncode(_subdr["value"].ToString())}"" />";
								}
							output			+= "</attribute>";
							}
						output			+= "</attvals>";
						}
					else
						{
						output			+= @"<attvals><attribute id='0' name='Invalid Tag ID'><value id='0' name='No values available'/></attribute></attvals>";
						}
				break;
				#endregion xml_tag_att_val				
				#region xml_match_parts
				case "xml_match_parts":
					tag_id						= _q["tag_id"];
					if(tag_id == null)
						{
						Response.Write("<critical>Tag not specified</critical>");
						return;
						}
					var attval_pattern		= _q["attvals"];
					if(!string.IsNullOrEmpty(tag_id.ToString()))
						{
						_dt						= match_parts(tag_id, attval_pattern != null ? attval_pattern.TrimEnd(',') : "");
						}
						
					if(_dt.Rows.Count > 0)
						{
						var _dv			= _dt.DefaultView;
						_dv.Sort				= wo_usage ? "wo_usage DESC, min_qty DESC, onhand DESC" : po_usage ? "po_usage DESC, min_qty DESC, onhand DESC" : "wo_usage DESC, min_qty DESC, onhand DESC";
						_dt						= _dv.ToTable();
						total			= _dt.Rows.Count;
						foreach(DataRow _dr in _dt.Rows)
							{
							var sub_start			= DateTime.Now;
							var master_id			= _dr["item"];
							var woUsage				= Convert.ToDouble(_dr["wo_usage"]);
							var poUsage				= Convert.ToDouble(_dr["po_usage"]);
							if(po_usage && poUsage == 0)
								{
								total--;
								continue;
								}
							else if(wo_usage && woUsage == 0)
								{
								total--;
								continue;
								}
							try
								{
								_inventory.Load(master_id, BusinessUnit.warehouse_bu_id);
							if(_inventory.active)
								{
							var vendor_id				= 0;
							if(!string.IsNullOrEmpty(_q["vendor_id"]) && _q["vendor_id"] != "null")
								{
								vendor_id				= Convert.ToInt32(_q["vendor_id"]);
								}
							tag_id						= _inventory.tag_id;
							if(tag_id != null)
								{
								if(	(!show_nosell && _inventory.sell_price == 0 && _inventory.global_highest.sell == 0) || 
									(!allow_gl && _inventory.tag_id == "843") || 
									(!allow_nonstock && _inventory.tag_id == "673")
									)
									{
									total--;
									continue;
									}
								object tag_name			= HttpUtility.HtmlEncode(_inventory.tag_name);

								object description		= HttpUtility.HtmlEncode(_inventory.description_full);
								object sold_as			= BusinessUnit.country == "CDN" ? _inventory.sold_as_canadian_name : _inventory.sold_as_usa_name;
								if(description.ToString() == "")
									{
									description			= "No description available for this part";
									sold_as				= null;
									}
									
								var sub_finish		= DateTime.Now;
								var _sub_diff		= sub_finish.Subtract(sub_start);
								double vendor_sell		= 0;
								var vendor_code		= "";
								double vendor_qty		= 0;
								if(vendor_id != 0)
									{
									var v_i			= Toolbox.doSQL_dt(@" SELECT a.total, a.vendor_code, a.qty FROM inventory_price a LEFT JOIN vendor b ON a.vendor_id = b.vendor_id WHERE a.master_id = @v0  AND a.business_unit_id = @v2  AND b.vendor_id = @v1  LIMIT 1", new [] {  master_id, vendor_id, BusinessUnit.warehouse_bu_id } );
									if(v_i.Rows.Count > 0)
										{
										var _vendor_info	= v_i.Rows[0];
										vendor_sell				= Convert.ToDouble(_vendor_info["total"]);
										vendor_code				= _vendor_info["vendor_code"].ToString();
										vendor_qty				= Convert.ToDouble(_vendor_info["qty"]);
										}
									}
								_inventory.cost_price_branch		= show_cost && origin == "workorder" ? Math.Round(_inventory.cost_price_branch, 3) : show_cost ? _inventory.cost_price_branch : 0;	
								_inventory.sell_price				= origin == "workorder" ? shared.GetSellPrice(_inventory.cost_price_branch, 0, _inventory.is_qty, 1, BusinessUnit.id32) : _inventory.sell_price;	
								sb.AppendFormat(@"
<part>
	<master_id>{0}</master_id>
    <description>{1}</description>
    <sellprice>{2}</sellprice>
    <costprice>{3}</costprice>
    <tag_id>{4}</tag_id>
    <tag_name>{5}</tag_name>
    <sold_as>{6}</sold_as> 
    <is_QTY>{14}</is_QTY>
    <row_seconds>{7}</row_seconds>
    <bv_code>{8}</bv_code>
    <last_vendor_dt>{18}</last_vendor_dt>
    <vendor_code>{15}</vendor_code>
    <vendor_qty>{16}</vendor_qty>
    <vendor_sell>{17}</vendor_sell>
    <to_business_unit_id>{13}</to_business_unit_id>
    <onhand_qty>{19}</onhand_qty>
    <is_exclude>{20}</is_exclude>
	<show_sell>{21}</show_sell>
	<show_cost>{22}</show_cost>
	<has_pic>{23}</has_pic>
	<has_minmax>{24}</has_minmax>
	<is_stocked>{25}</is_stocked>
<int_onhand_qty>{26}</int_onhand_qty>
<ext_onhand_qty>{27}</ext_onhand_qty>
<total_int_onhand_qty>{28}</total_int_onhand_qty>
<total_ext_onhand_qty>{29}</total_ext_onhand_qty>
</part>", 
				master_id,										// {0}
				description,									// {1}
				_inventory.sell_price,							// {2}
				_inventory.cost_price_branch,					// {3}
				_inventory.tag_id,								// {4}
				tag_name,										// {5}
				sold_as,										// {6}
				_sub_diff.TotalSeconds,							// {7}
				_inventory.bv_part_number,						// {8}
				0,					// {9}
				0,			// {10}
				0,					// {11}
				0,				// {12}
				BusinessUnit.warehouse_bu_id,								// {13}
				_inventory.is_qty,								// {14}
				vendor_code,									// {15}
				vendor_qty,										// {16}
				vendor_sell,									// {17}
				_inventory.vendor_price_last_dt,				// {18}
                _inventory.onhand_qty,				            // {19}
                _inventory.is_exclude,			                // {20}
				show_sell,										// {21}
				show_cost,										// {22}
				_inventory.has_pic,								// {23}
				(_inventory.n_minmax > 0),						// {24}
				_inventory.is_stocked,							// {25}
				_inventory.int_onhand_qty,  // {26}
				_inventory.ext_onhand_qty, // {27}
				_inventory.total_int_onhand_qty,  // {28}
				_inventory.total_ext_onhand_qty  // {29}
				);
								}
							else
								{
								Response.Clear();
								Response.Write("<error>Query produced (0) results</error>");
								Response.End();
								}
							}

							}
						catch
							{
							}}
						var finish			= DateTime.Now;
						var _difference	= finish.Subtract(start);
						output					= $"\n<parts start='{start}' length='{length}' total='{total}' querytime='{_difference.TotalSeconds}'>{sb}\n</parts>";
							
						}
					else
						{
						Response.Write("<error>Query produced (0) results</error>");
						}
				break;
				#endregion xml_match_parts
				}
			}
		Response.Write(output);
		}

	private DataTable match_parts(object tag_id, object attval_pattern)
			{
			var _pattern		= attval_pattern.ToString();
			var _dt				= Toolbox.doSQL_dt("CALL MATCH_PARTS(@v0 , @v1 , @v2 , 100, @v3 )", new [] {  tag_id, _pattern, BusinessUnit.warehouse_bu_id , is_stocked } );
			return _dt;
			}
	private DataTable tag_attribute_selected_values(object tag_id, object attribute_id)
			{
			return Toolbox.doSQL_dt(@" SELECT distinct inventory_item_detail.attribute_value_id, inventory_attribute_value.`value` FROM inventory_item_detail INNER JOIN inventory_attribute_value ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id AND inventory_attribute_value.attribute_id = @v1  INNER JOIN inventory_tag_link ON inventory_attribute_value.attribute_id = inventory_tag_link.attribute_id AND inventory_tag_link.tag_id = @v0  INNER JOIN inventory_item_master ON inventory_item_detail.master_id = inventory_item_master.master_id AND inventory_item_master.tag_id = inventory_tag_link.tag_id ORDER BY `value`", new [] {  tag_id, attribute_id } );
			}
	private DataTable attributes(object tag_id)
			{
			return Toolbox.doSQL_dt(@" SELECT a.attribute_id, b.attribute FROM inventory_tag_link a LEFT JOIN inventory_attribute b ON a.attribute_id = b.attribute_id  WHERE a.tag_id =@v0 ORDER BY a.order_id", new object[] { tag_id });
			}
	}
