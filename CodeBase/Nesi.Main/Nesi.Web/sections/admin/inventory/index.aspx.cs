using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using NVC = System.Collections.Specialized.NameValueCollection;
using System.Text;
using DevExpress.Web;
using nesi.core;
using NESI.Common.Models;

public partial class inventory_start : System.Web.UI.Page
{
	public NeMember my_member;
	public NeBusinessUnit this_branch;

	private const int page_id = 49; // from Page table in DB
	private const string page_description = "Inventory / Admin Access";
	public const string _page_name = "AdminInventory";
	public string page_name = "";
	Toolbox _tools;
	ASPxHiddenField _h;
	SqlDataSource _ds_templates;
	ASPxDropDownEdit _dde_filter;
	NVC _q;
	Panel _panel_export;
	bool hasProtectedTagPropertyAccess;
	protected void Page_Init(object _sender, EventArgs _e)
	{
		_tools = new Toolbox();
		my_member = Toolbox.do_handle_authentication(page_id);
		_q = Request.QueryString;
		_h = (ASPxHiddenField)layout.FindControl("h");
		_ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		_dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		_panel_export = (Panel)layout.FindControl("panel_export");
		layout.__page_name = !string.IsNullOrEmpty(_q["a"]) ? _q["a"] == "splitparts" ? "AdminInventory_gv_splitparts" : _page_name : _page_name;
		layout.used_gv = gv_splitparts;
		_tools.dont_cache_page();
		_tools.page_author = new NeMember(711);
		_tools.current_user = my_member;
	}
	#region Page_Load
	protected void Page_Load(object _sender, EventArgs _e)
	{
		#region Variable Declaration
		var menu = new NeMenu(my_member, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		var f = Request.Form;
		divSide.InnerHtml = shared.PrintSidePanelHTML(my_member);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = page_description;
		string output = null;
		var business_unit_id = 0;
		string aid = "", value_id = "", value_name = "", this_id = "", name = "", vendor_number = "", vendor_id = "", master_id = "", attribute_id = "", order_id = "", attribute_name = "", type_of = "";
		object total_cost = null, unit_cost = null, part_number = null, qty = null, price_id = null, benchmark = null;
		var pages = "";
		var per_page = 20;
		double curr_page = 1;
		var seed = "0";
		var attribute_rows = 0;
		double n_pages = 0;
		string TagID = null;
		string tag_name = null;
		var inv_class = new this_inventory(my_member);
		var inventory = new inventory();
		inv_class.this_member = my_member;

		inv_class.this_business_unit = new NeBusinessUnit(my_member.business_unit_id);
		hasProtectedTagPropertyAccess = my_member.AuthenticatedForPrivilege(OpsPrivilege.ProtectedTagPropertyAccess) ; 
		var branches = inv_class.available_ds_ns();

		var _dt = new DataTable();
		var tradename_id = "";
		var subdt = new DataTable();
		for (var i = 0; i < branches.Length; i++)
		{
			if (i + 1 == branches.Length)
			{
				BRANCH_DSNS.Text += "'" + branches[i] + "'";
			}
			else
			{
				BRANCH_DSNS.Text += "'" + branches[i] + "',";
			}
		}
		#endregion
		switch (Request.HttpMethod)
		{
			#region POST
			case "POST":
				if (!IsCallback)
				{
					switch (Request["action"])
					{
						#region new attribute
						case "new attribute":
							var new_attribute = Request["attribute"];
							try
							{
								inv_class.NewAttribute(new_attribute);
								Response.Write("<script>location.href='./index.aspx?a=att';</script>");
							}
							catch
							{
								Response.Write("<script>alert('You have submitted an invalid attribute name');location.href='./index.aspx?a=att';</script>");
							}
							break;
						#endregion
						#region create linkage
						case "create linkage":
							try
							{
								attribute_rows = Convert.ToInt32(Request.Form["n_AttributeRows"]);
								TagID = Request.Form["tag_id"];
							}
							catch
							{
								Response.Write("<script>alert('An error occured with your submission.');location.href='./index.aspx?a=tag';</script>");
								Response.End();
							}
							try
							{
								if (attribute_rows > 0)
								{
									for (var i = 0; i < attribute_rows; i++)
									{
										//Insert Tag Linkage
										var att_target = "attribute_" + i;
										var selected_target = "selected_" + i;
										var order_target = "order_" + i;
										var attribute = Request[att_target];
										var order = Request[order_target];
										if (order.Trim() == "" || order.Trim() == "0")
										{
											order = "99";
										}
										inv_class.insert_tag_link(TagID, attribute, order);

										// Insert Tag Linkage Presets
										var selected = Request.Form[selected_target].Split(new char[] { ',' });
										var selected_value = "";
										for (var p = 0; p < selected.Length; p++)
										{
											selected_value = selected[p];
											inv_class.insert_tag_preset(TagID, attribute, selected_value);
										}
									}

									Response.Write("<script>alert('You have successfully created a linkage for this tag.');location.href='./index.aspx?a=tag';</script>");
									Response.End();
								}
								else
								{
									Response.Write("<script>alert('No attributes were selected.');location.href='./index.aspx?a=link_attributes&tid=" + TagID + "';</script>");
									Response.End();
								}
							}
							catch (Exception ee)
							{
								Response.Write("<script>alert('An error occured with your submission.');location.href='./index.aspx?a=tag';</script>");
								Response.End();
								_tools.catch_error(ee);
							}
							break;
						#endregion
						#region edit linkage
						case "edit linkage":
							try
							{
								attribute_rows = Convert.ToInt32(Request.Form["n_AttributeRows"]);
								TagID = Request.Form["tag_id"];
								var tag = new inventory.tag(TagID);
								var ts = Convert.ToInt64(Request.Form["ts"]);
								if (tag.ts.Ticks > ts)
								{
									Response.Write("<script>alert('This tag has been edited since you last loaded it.. refreshing the page.');location.href=location.href;</script>");
									Response.End();
								}
								inv_class.delete_previous_linkage(TagID);
								inv_class.delete_previous_predefined(TagID);
							}
							catch
							{
								Response.Write("<script>alert('An error occured with your submission.');location.href='./index.aspx?a=tag';</script>");
								Response.End();
							}
							if (attribute_rows > 0)
							{
								var attribute = "";
								var order = "";
								for (var i = 0; i < attribute_rows; i++)
								{
									var att_target = "attribute_" + i;
									var selected_target = "selected_" + i;
									var order_target = "order_" + i;
									var predefined_selected = "predefined_selected_" + i;
									attribute = Request[att_target];
									order = Request[order_target];

									if (string.IsNullOrEmpty(order) || order == "0")
									{
										order = "99";
									}
									inv_class.insert_tag_link(TagID, attribute, order);

									// Insert Tag Linkage Presets
									var selected = Request.Form[selected_target].Split(new char[] { ',' });
									var selected_value = "";
									var preset_insert = new StringBuilder();
									preset_insert.Append("INSERT INTO inventory_tag_preset (tag_id, attribute_id, value_id) VALUES ");
									var preset_c = 0;
									for (var p = 0; p < selected.Length; p++)
									{
										preset_c++;
										selected_value = selected[p];
										preset_insert.AppendFormat("('{0}', '{1}', '{2}')", TagID, attribute, selected_value);
										if (p < selected.Length - 1 && selected.Length > 1)
										{
											preset_insert.Append(",");
										}
									}
									if (preset_c > 0)
									{
										Toolbox.doSQL_void(preset_insert.ToString());
									}
									var predefined_value = "";
									if (f[predefined_selected] != null)
									{
										predefined_value = f[predefined_selected];
									}
									if (predefined_value != "")
									{
										var current_parts = Toolbox.doSQL_dt(@"SELECT master_id FROM inventory_item_master WHERE tag_id = @v0 ", new object[] { TagID });
										foreach (DataRow part in current_parts.Rows)
										{
											master_id = part["master_id"].ToString();
											Toolbox.doSQL_void(@"INSERT INTO inventory_item_detail 
(master_id, attribute_value_id) VALUES (@v0, @v1)", new object[] { master_id, predefined_value });
										}
									}
								}

								Toolbox.doSQL_void(@"DELETE FROM inventory_item_detail 
where associated_tag(master_id) = @v0 AND associated_attribute(attribute_value_id) not in (SELECT attribute_id FROM inventory_tag_link where tag_id = @v0)", TagID);

								//inv_class.UnapproveTag(TagID);
								Response.Write("<script>alert('You have successfully edited the linkage for this tag.');location.href='./index.aspx?a=tag';</script>");
							}
							else
							{
								Response.Write(string.Format("<script>alert('No attributes were selected.');location.href='./index.aspx?a=edit_linkage&tid={0}';</script>", TagID));
							}
							Response.End();
							break;
						#endregion
						#region editvalue
						case "editvalue":
							if (Request.Form["attribute_value_id"] != null && Request.Form["new_value"] != null && Request.Form["aid"] != null)
							{
								var attribute_value_id = Request.Form["attribute_value_id"];
								var new_value = Request.Form["new_value"];
								aid = Request.Form["aid"];
								if (inv_class.edit_value(attribute_value_id, new_value))
								{
									Response.Write("SUCCESS");
									Response.End();
								}
								else
								{
									Response.Write("FAILED");
									Response.End();
								}
							}
							else
							{
								Response.Write("FAILED");
								Response.End();
							}
							break;
						#endregion
						#region picture
						case "picture":
							break;
						#endregion
						#region default - new part
						default:
							Response.Clear();
							_tools.dont_cache_page();
							if (f["tag"] != null && f["attributes_n"] != null)
							{
								var tag_id = f["tag"];
								var n_attributes = Convert.ToInt32(f["attributes_n"]);
								master_id = inventory.NewPart(tag_id, f["att_vals[]"], my_member.id.ToString(), true);
								var is_number = new Regex(@"^\d+$");
								var success = is_number.Match(master_id).Success;
								var old_data_n = Convert.ToInt32(f["old_data_n"]);
								if (success)
								{
									if (old_data_n > 0)
									{
									}
									Response.Write("SUCCESS-" + master_id);
									Response.End();
								}
								else
								{
									Response.Write("ERROR:" + master_id);
									Response.End();
								}
							}
							break;
							#endregion
					}
				}
				break;
			#endregion
			#region GET
			case "GET":
				#region action assignment
				var action = "";
				if (_q["a"] != null)
				{
					action = _q["a"];
				}
				else
				{
					action = "new";
				}
				ACTION_VAR.Text = action;
				#endregion action assignment
				#region XML HEADER INFO
				if (action.Contains("xml"))
				{
					_tools.set_XML_header();
					output = null;
				}
				#endregion
				#region QC action header change
				if (action.Contains("qc") && !action.Contains("xml"))
				{
					var actions = new NVC();
					actions["parts"] = "<a href='./index.aspx?a=qc-parts'>PARTS</a>";
					actions["attributes"] = "<a href='./index.aspx?a=qc-attributes'>ATTRIBUTES</a>";
					actions["values"] = "<a href='./index.aspx?a=qc-values'>VALUES</a>";
					actions["pics"] = "<a href='./index.aspx?a=qc-pics'>PICS</a>";
					switch (action)
					{
						case "qc-parts":
							actions["parts"] = "<div>PARTS</div>";
							break;
						case "qc-attributes":
							actions["attributes"] = "<div>ATTRIBUTES</div>";
							break;
						case "qc-values":
							actions["values"] = "<div>VALUES</div>";
							break;
						case "qc-pics":
							actions["pics"] = "<div>PICS</div>";
							break;
						default:
							lbltemp.Text = "Inventory / Admin Access / Quality Check Home";
							break;
					}
					output += string.Format(@"
					<table class='qc-menu' cellpadding='0' cellspacing='5'>
						<tr>
							<td>{0}</td>
							<td>{1}</td>
							<td>{2}</td>
							<td>{3}</td>
						</tr>
					</table>",
					actions["parts"],
					actions["attributes"],
					actions["values"],
					actions["pics"]);
				}
				#endregion  QC action header change
				switch (action)
				{
					#region PAGES
					#region PAGE - att
					case "att":
						button_attributes.Disabled = true;
						page_title.InnerText = "Attributes & Values";
						lbltemp.Text = "Inventory / Admin Access / Attributes & Values";
						if (_q["aid"] != null)
						{
							attribute_id = _q["aid"];
						}
						output += @"
					<table class='detail' cellpadding='0' cellspacing='0'>
						<tr>
							<td class='attributes' valign='top'>
									<b>New Attribute</b><br />
									<input type='text' name='attribute'><br />
									<button type='button' onclick='add_attribute(this);'>Submit Attribute</button>
									<div class='list'>";

						_dt = inv_class.attributes();
						var att_sb = new StringBuilder();
						foreach (DataRow dr in _dt.Rows)
						{
							var value = dr["attribute_id"].ToString();
							var disabled = value == attribute_id ? "disabled" : "";
							var arrow = value == attribute_id ? ">" : "";
							var href = value == attribute_id ? "" : string.Format(" href='./index.aspx?a=att&aid={0}#att_{0}'", value);
							var option = dr["attribute"].ToString();
							att_sb.Append(string.Format(@"
									<a{4} name='att_{0}' {2}>{3}{1}</a>", value, option, disabled, arrow, href));
						}

						output += att_sb + @"
									</div>
							</td>";
						output += @"
							<td class='valuesbox' valign='top'>&nbsp;";
						if (attribute_id != "" && attribute_id != null)
						{
							try
							{
								_dt = inv_class.values(attribute_id);
								var del_att_disabled = _dt.Rows.Count > 0 ? "disabled" : "";
								var Attribute_Name = inv_class.attribute_name(attribute_id);

								output += string.Format(@"
								<input type='hidden' value='yes' id='attribute_is_edittable'>
								<div id='attribute_name' style='min-height:32px;' onclick='edit_attribute({0}, this);'>
									{1}
								</div>
								<div id='attribute_delete' style='padding-bottom:25px;'>
									<button type='button' onclick='delete_attribute({0});' style='font-size:11px;font-family:arial;float:left;' {2}>delete attribute</button>
									<select style='font-size:11px;font-family:arial;float:right;width:250px;' onchange=""if(this.value != '0'){{location.href='./index.aspx?a=edit_linkage&tid='+this.value;}}"">
										<option value='0'>Preset in these tags</option>", attribute_id, Attribute_Name, del_att_disabled);
								var preset_tags = Toolbox.doSQL_dt(@"SELECT DISTINCT(b.tag_id) tag_id, b.tag tag_name FROM inventory_tag_preset a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id  WHERE a.attribute_id =@v0", new object[] { attribute_id });
								var pre_sb = new StringBuilder();
								foreach (DataRow dr in preset_tags.Rows)
								{
									TagID = dr["tag_id"].ToString();
									tag_name = dr["tag_name"].ToString();
									pre_sb.Append(string.Format("<option title=\"{1}\" value='{0}'>{1}</option>", TagID, tag_name));
								}
								output += pre_sb + string.Format(@"
									</select>
								</div>
								<table cellpadding='0' cellspacing='0' class='values'>
									<tr>
										<td class='add' align='left'><input type='text' id='new_value' onmouseover='this.focus();' onkeyup=""var code = (event.keyCode ? event.keyCode : event.which);if(code==13){{send_value({0});}}""></td>
									</tr>
								</table>", attribute_id, Attribute_Name, del_att_disabled);
								/*
								 * <button class='button' type='button' onclick='new_value("+attribute_id+@");'><img src='/images/icon/icon[add].gif'></button>
								 */
								if (_dt.Rows.Count > 0)
								{
									output += string.Format(@"
								<table cellpadding='0' cellspacing='0' class='values' id='value_list' style='display:none'>
									<thead>
									<tr>	
										<th class='header' style='cursor:pointer'>Value</th>
										<th class='header' style='cursor:pointer'># of items</th>
										<th class='header' style='cursor:pointer'>Used in Tag?</th>
										<th class='header' style='cursor:pointer'>&nbsp;</th>
										<th class='header' style='cursor:pointer'>&nbsp;</th>
									</tr></thead><tbody>", del_att_disabled, attribute_id);
									var sb_vals = new StringBuilder();
									foreach (DataRow dr in _dt.Rows)
									{
										var id = dr["attribute_value_id"].ToString();
										var value = dr["value"].ToString();
										var is_approved = Convert.ToBoolean(dr["approved"]);
										var howmany = Convert.ToInt32(dr["howmany"]);
										var used_in_tag_b = Convert.ToInt32(dr["used_in_tag"]) > 0;
										var used_in_tag_s = used_in_tag_b ? "Y" : "N";
										var disabled = howmany > 0 || used_in_tag_b ? "disabled" : "";
										var unapprove_button = is_approved ? string.Format(@"<button title='Unapprove Value, Sends value back to QC' class='button' onclick=""unapprove_value({0}, {1});"" type='button'><img src='/images/icon/icon[back].gif'></button>", id, attribute_id) : "&nbsp;";
										sb_vals.Append(string.Format(@"
									<tr>
										<td class='value'>
											<span id=""value_{0}"" data-row_id='{0}' data-aid='{3}'><div onclick='edit_value(this)' width='100%' style='cursor:pointer;'>{1}</div></span>
										</td>
										<td class='howmany'>{2}</td>
										<td class='howmany'>{6}</td>
										<td class='action'><button class='button{4}' onclick=""delete_value({0}, {3});"" type='button' {4}><img src='/images/icon/icon[delete].gif'></button></td>
										<td class='action'>{7}</td>
									</tr>",
						id,                                 // {0}
						value,                              // {1}
						inv_class.pseudo_part_select(id),       // {2}
						attribute_id,                       // {3}
						disabled,                           // {4}
						value.Replace("&", "\\&"),          // {5}
						used_in_tag_s,                      // {6}
						unapprove_button                    // {7}
						));
									}
									output += sb_vals + @"
									</tbody>
								</table>
				<div id='pages_' align='center' style='display:none;'>
					<form>
						<button type='button' class='first'>&lt;&lt;</button>
						<button type='button' class='prev'>&lt;</button>
						<input type='text' size='5' class='pagedisplay'/>
						<button type='button' class='next'>&gt;</button>
						<button type='button' class='last'>&gt;&gt;</button>
						<input type='hidden' class='pagesize' value='50'/>
						</select>
					</form>
				</div>
<script>
	$('document').ready(function()
		{
		$('#value_list').tablesorter().tablesorterPager(	{
											positionFixed:false, 
											size:50,
											container: $('#pages_')
											}).show();
		$('#pages_').slideDown();
		});
</script>
";
								}
								else
								{
									output += @"
								<div class='no_value'>There aren't values set for this attribute</div>";
								}
							}
							catch (Exception ee)
							{
								output += @"
								Could not retrieve values for this attribute" + ee;
							}
						}
						output += @"
							</td>
						</tr>
					</table>";
						break;
					#endregion
					#region PAGE - browse
					case "browse":
						button_browse.Disabled = true;
						page_title.InnerText = "Part Management";
						lbltemp.Text = "Inventory / Admin Access / Part Management";

						output += @"
									<div id='part_detail' onclick='close_part();'></div>
									<div id='part_info'>
										<div class='part_number'></div>
										<div id='part_attvals'></div>
										<button type='button' onclick='close_part();'>close part</button>
									</div>
									<table class='browse_part'>
										<tr>
											<td class='tag_space' colspan='2'>
												TAG<br/>
												<select id='TAG_ID' name='tag' class='tag' onchange='if(this.value != 0){populate_browser(this.value);}'>
													<option value='0'>PLEASE SELECT A TAG FROM AN EXISTING PART</option>";
						_dt = inv_class.Browse_Tags();
						foreach (DataRow dr in _dt.Rows)
						{
							TagID = dr["tag_id"].ToString();
							tag_name = dr["tag"].ToString();

							output += string.Format(@"
													<option value='{0}'>{1}</option>", TagID, tag_name);
						}
						output += @"
												</select>
											<input type='hidden' id='n_attributes' value='0'>
											</td>
										</tr>
										<tr>
											<td id='results' valign='top' align='center' width='25%'>asdf</td>
											<td id='parts' valign='top' width='75%'>asdf</td>
										</tr>
									</table>";
						break;
					#endregion browse
					#region PAGE - create_linkage
					case "create_linkage":
						if (inv_class.check_tag_id(_q["tid"]))
						{
							_tools.dont_cache_page();

							TagID = _q["tid"];
							tag_name = inv_class.tag_name(TagID);

							page_title.InnerText = "Create Tag's Attribute Linkage";
							lbltemp.Text = "Inventory / Admin Access / Create Tag's Attribute Linkage";

							output += string.Format(@"
	<form method='post' action='/sections/admin/inventory/index.aspx' onsubmit=""document.getElementById('save_tag').disabled=true;"">

		<input type='hidden' name='action' value='create linkage'>
		<table cellpadding='0' cellspacing='0' class='newitem'>
			<tr>
				<td colspan='2' class='head'>{1}</td>
			</tr>
			<tr>
				<td class='cat' valign='top'>
					<button type='button' id='add_row' onclick='add_attribute_row();' title='Add Attribute Row'>ADD <img src='/images/icon/icon[add].gif' align='absmiddle' /></button>
					<input type='hidden' name='n_AttributeRows' value='0' id='n_AttributeRows'/>
					<input type='hidden' name='tag_id' value='{0}' id='tag_id'/>
				</td>
				<td class='val' valign='top'>
					<table cellpadding='5' cellspacing='0' class='header'>
						<tr>
							<td width='265' align='center'><b>attribute</b></td>
							<td width='265' align='center'><b>default value</b></td>
						</tr>
					</table>
					<div id='attributes'></div>
				</td>
			</tr>
			<tr>
				<td class='submit' colspan='2'>
					<button type='submit' id='save_tag' disabled>Save</button>
					<button type='button' onclick='location.href=""./index.aspx?a=tag"";'>Cancel</button>
				</td>
			</tr>
		</table>
	</form>", TagID, tag_name);
						}
						else
						{
							Response.Write("<script>alert('Invalid Tag ID');location.href('./index.aspx?a=tag');</script>");
							Response.End();
						}
						break;
					#endregion
					#region PAGE - default
					default:
						page_title.InnerText = "Home";
						lbltemp.Text = "Inventory / Admin Access / Home";
						button_home.Disabled = true;
						output += @"&nbsp;";
						break;
					#endregion 


					#region PAGE - edit_linkage (tag)
					case "edit_linkage":
						page_title.InnerText = "Edit Tag's Attribute Linkage";
						lbltemp.Text = "Inventory / Admin Access / Edit Tag's Attribute Linkage";
						output += page_edit_linkage(Convert.ToInt32(_q["tid"]));
						break;
					#endregion
					#region PAGE - edit_part
					case "edit_part":
						page_title.InnerText = "Edit Part";
						lbltemp.Text = "Inventory / Admin Access / Edit Part";
						if (_q["master_id"] != null)
						{
							master_id = (string)_q["master_id"];
							TagID = inv_class.get_TagIDfromMaster(master_id);
							var properties = Toolbox.doSQL_dt(@" SELECT a.is_qty, a.is_exclude, b.unit usa_sold_as, c.unit canadian_sold_as, a.quick_add, a.allowed_to_stock, a.allowed_to_edit_after_issue FROM inventory_tag a LEFT JOIN inventory_sold_as b ON a.usa_sold_as = b.id LEFT JOIN inventory_sold_as c ON a.canadian_sold_as = c.id WHERE a.tag_id = @v0  LIMIT 1", new object[] { TagID });
							var prop_is_qty = "";
							var prop_is_exclude = "";
							var prop_usa_sold_as = "";
							var prop_canadian_sold_as = "";
							var prop_is_allowed_to_stock = "";
							var prop_quick_add = "";
							var prop_allowed_to_edit_after_issue = "";
							foreach (DataRow property in properties.Rows)
							{
								prop_is_qty = Convert.ToBoolean(property["is_qty"]).ToString();
								prop_is_exclude = Convert.ToBoolean(property["is_exclude"]).ToString();
								prop_usa_sold_as = property["usa_sold_as"].ToString();
								prop_canadian_sold_as = property["canadian_sold_as"].ToString();
								prop_quick_add = Convert.ToBoolean(property["quick_add"]).ToString();
								prop_is_allowed_to_stock = Convert.ToBoolean(property["allowed_to_stock"]).ToString();
								prop_allowed_to_edit_after_issue = Convert.ToBoolean(property["allowed_to_edit_after_issue"]).ToString();
							}
							if (!inventory.part_exists(master_id, TagID))
							{
								Response.Clear();
								_tools.set_plain_header();
								_tools.dont_cache_page();
								Response.Write("Invalid Part #");
								Response.End();
							}
							inventory.Load(master_id, my_member.business_unit_id);
							var bind_to_member = "";
							if (TagID == "704" || TagID == "571")
							{
								bind_to_member = string.Format(@"<div align='left'><b>Bound to:</b><br/><input type='text' value='{0}' style='font-weight:bold;border:solid 1px #999;width:95%;padding:5px;margin:5px;' data-isac='false' onfocus=""attach_ac(this, 'member')"" data-id='{1}' /></div>",
								inv_class.Part_Member(master_id, true),         // ITEM 0 - Member Name
								inv_class.Part_Member(master_id, false)         // ITEM 1 - Member ID
								);
							}
							tag_name = inv_class.tag_name(TagID);
							_dt = inv_class.tag_att_vals(master_id, TagID);
							var i = 0;
							output += @"
					<script>
						function this_tab(enabled_button)
							{
							var enabled_button_id		= $(enabled_button).attr('id');
							var enabled_button_src		= $(enabled_button).attr('src');
							if(!enabled_button_src.match(/disabled/g))
								{
								var disabled_button, disabled_button_id, disabled_button_src;
								var enabled_tab_id, disabled_tab_id;
								
								switch(enabled_button_id)
									{
									case 'button_details':
										disabled_button		= '#button_pricing';
										$('#SAVE_BUTTON').show();
									break;
									case 'button_pricing':
										disabled_button		= '#button_details';
										$('#SAVE_BUTTON').hide();
									break;
									}
									
								disabled_button_id	= $(disabled_button).attr('id');
								disabled_button_src	= $(disabled_button).attr('src');
								
								enabled_tab_id		= enabled_button_id.replace(/button_/, '.edit_');
								disabled_tab_id		= disabled_button_id.replace(/button_/, '.edit_');
								$(enabled_tab_id).css('display', 'block');
								$(enabled_button).css('cursor', '');
								$(disabled_tab_id).css('display', 'none');
								$(disabled_button).css('cursor', 'pointer');
								
								$(enabled_button).attr('src', enabled_button_src.replace(/\.png/, '[disabled].png'));				
								$(disabled_button).attr('src', disabled_button_src.replace(/\[disabled\]/, ''));		
								
								$(enabled_button).attr('disabled', true);
								$(disabled_button).attr('disabled', false);
								if(enabled_button_id == 'button_pricing')
									{
									get_pricing();
									}
								}
							}
					</script>";

							var sell_price_element = "";
							/*
							 IF the current user has the correct permission, the sell price is a button versus just a text element.
							*/
							sell_price_element = _tools.Monetize(inventory.sell_price);
							#region current_wos
							var current_wos = Toolbox.doSQL_dt(@" SELECT b.woprog_id id, CAST(CONCAT(c.name,' - ',b.woprog_bvwo) AS CHAR) name FROM wo_detail_current a LEFT JOIN woprog b ON a.wo_detail_current_woprog_id = b.woprog_id LEFT JOIN business_unit c ON b.business_unit_id = c.id WHERE a.wo_detail_current_code = @v0  AND b.business_unit_id != 8 ORDER BY c.name,b.woprog_bvwo", new object[] { master_id });
							var current_wos_options = "";
							if (current_wos.Rows.Count > 0)
							{
								foreach (DataRow wo in current_wos.Rows)
								{
									var id = wo["id"];
									var _name = wo["name"];
									current_wos_options += string.Format("<option value='{0}'>{1}</option>", id, _name);
								}
							}
							else
							{
								current_wos_options = "<option disabled>None</option>";
							}
							#endregion current_wos
							#region current_quotes
							var current_quotes = Toolbox.doSQL_dt(@"SELECT DISTINCT(a.quote_id) id, CAST(CONCAT(a.quote_id,' v', a.revision) as CHAR) name FROM quote_worksheet a LEFT JOIN quote_master b ON a.quote_id = b.quote_id AND a.revision = b.revision WHERE part_no = @v0  AND b.status_id IN (1,2,3,4,8) ORDER BY id", new object[] { master_id });
							var current_quotes_options = "";
							if (current_quotes.Rows.Count > 0)
							{
								foreach (DataRow quote in current_quotes.Rows)
								{
									var id = quote["id"];
									var _name = quote["name"];
									current_quotes_options += string.Format("<option value='{0}'>{1}</option>", id, _name);
								}
							}
							else
							{
								current_quotes_options = "<option disabled>None</option>";
							}
							#endregion current_quotes
							#region current_pos
							var current_pos = Toolbox.doSQL_dt(@"SELECT b.poprog_id id,CAST(CONCAT(c.name,' - ',b.poprog_bvpo) AS CHAR) name FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id LEFT JOIN business_unit c ON b.business_unit_id = c.id WHERE a.po_details_part_no = @v0  AND b.poprog_status NOT IN (7,8) AND b.business_unit_id != 8 ORDER BY c.name, b.poprog_bvpo", new object[] { master_id });
							var current_pos_options = "";
							if (current_pos.Rows.Count > 0)
							{
								foreach (DataRow po in current_pos.Rows)
								{
									var id = po["id"];
									var _name = po["name"];
									current_pos_options += string.Format("<option value='{0}'>{1}</option>", id, name);
								}
							}
							else
							{
								current_pos_options = "<option disabled>None</option>";
							}
							#endregion current_pos
							var button_toggle_part_text = "";
							if (inventory.active)
							{
								button_toggle_part_text = "<b>Disable Part</b><div style='font-size:10px;'>(currently enabled)</div>";
							}
							else
							{
								button_toggle_part_text = "<b>Enable Part</b><div style='font-size:10px;'>(currently disabled)</div>";
							}
							var toggle_button = "";
							var merge_button = "";

							if (inventory.old_id.Length > 0)
							{
								merge_button = string.Format(@"<button onclick=""location.href='./index.aspx?a=edit_part&master_id={0}';"" type='button'>&lt;&lt;<div style='font-size:10px;'>go</div></button><button type='button' class='merge_part' onclick=""$('#merge_pane').dialog('open');""><b>Was {0}</b><div style='font-size:10px;'> Merge Again?</div></button>", inventory.old_id);
								toggle_button = string.Format(@"<button type='button' class='toggle_part' value='{1}'>{0}</button>", button_toggle_part_text, inventory.active);
							}
							else if (inventory.new_id.Length > 0)
							{
								merge_button = string.Format(@"<button type='button' class='merge_part' disabled><b>Now {0}</b><div style='font-size:10px;'> Merging Disabled</div></button><button onclick=""location.href='./index.aspx?a=edit_part&master_id={0}';"" type='button'>&gt;&gt;<div style='font-size:10px;'>go</div></button>", inventory.new_id);
								toggle_button = string.Format(@"<button type='button' class='toggle_part' value='{1}' disabled>{0}</button>", button_toggle_part_text, inventory.active);
							}
							else
							{
								merge_button = string.Format(@"<button type='button' class='merge_part' onclick=""$('#merge_pane').dialog('open');""><b>Merge</b><div style='font-size:10px;'>with existing part</div></button>", "");
								toggle_button = string.Format(@"<button type='button' class='toggle_part' value='{1}'>{0}</button>", button_toggle_part_text, inventory.active);
							}

							output += string.Format(@"
					<input type='hidden' id='type_of' value='EDIT' />
					<input type='hidden' id='master_id' value='{0}' />
					<input type='hidden' id='TAG_ID' value='{1}' />
					<div style='float:left;width:450px;padding:5px;border:0;margin:5px;text-align:right;'>
						<button type='button' onclick='PushEditPart()' id='SAVE_BUTTON' disabled><b>Save Part</b><div style='font-size:10px;'>&nbsp;</div></button>
						{9}
						{8}
					</div>
					<div style='float:right;width:200px;padding:5px;border:0;margin:2px;'>
						<b>Used in these WOs</b><br/>
						<select style='width:200px;font-size:11px;font-weight:bold;' onchange=""boing('/sections/workorder/index.aspx?woprog_id='+this.value,'WO',1280,500)"" multiple>{4}</select>
					</div>
					<div style='float:right;width:200px;padding:5px;border:0;margin:2px;'>
						<b>Used in these POs</b><br/>
						<select style='width:200px;font-size:11px;font-weight:bold;' onchange=""boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid='+this.value,'PO',1280,500)"" multiple>{10}</select>
					</div>
					<div style='float:right;width:150px;padding:5px;border:0;margin:2px;'>
						<b>Used in these Quotes</b><br/>
						<select style='width:125px;font-size:11px;font-weight:bold;' multiple>{5}</select>
					</div>
					<table cellpadding='0' cellspacing='0' class='tag_space'>
						<tr>
							<td>
								Part #: {1}-{0} 
								<div class='subtext'>TAG: {2}</div>
								<div class='sub_menu'>
								<span style='float:right;'><img onclick='this_tab(this);' id='button_details' src='/images/inventory/button/button[details][disabled].png' disabled/><img onclick='this_tab(this);' id='button_pricing' src='/images/inventory/button/button[pricing].png' style='cursor:pointer;'/></span></div>
							</td>
						</tr>
					</table>
					<table cellspacing='0' cellpadding='5' id='sell_price_table'>
						<thead>
							<tr style='background-color:#000;color:#fff;'>
								<th width='215' align='left'>Branch</th>
								<th width='55' align='center'><i style='font-size:9px;'>Current.</i><br/>NESI SELL</th>
								<th width='65' align='center'><i style='font-size:9px;'>Current</i><br/>BV SELL</th>
								<th width='100' align='center'><i style='font-size:9px;'>Orig.</i><br/>BV Part</th>
								<th width='100' align='center'><i style='font-size:9px;'>Orig.</i><br/>BV Sell</th>
								<th width='100' align='center'><i style='font-size:9px;'>Orig.</i><br/>BV COST</th>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
					<table cellpadding='0' cellspacing='0' class='edit_details'>
						<tr>
							<td valign='top' width='160'>
								<div id='att_vals'>
								{3}",
									master_id,                                              // {0}
									TagID,                                                  // {1}
									tag_name,                                               // {2}
									bind_to_member,                                         // {3}
									current_wos_options,                                    // {4}
									current_quotes_options,                                 // {5}
									button_toggle_part_text,                                // {6}
									inventory.active,                                       // {7}
									"",                                           // {8} Removed Merge Button
									"",                                          // {9} Removed Toggle Button
									current_pos_options                                     // {10}
									);

							foreach (DataRow dr in _dt.Rows)
							{
								attribute_name = dr["attribute"].ToString();
								attribute_id = dr["attribute_id"].ToString();
								value_id = dr["value_id"].ToString();

								subdt = inv_class.tag_attribute_selected_values(TagID, attribute_id);
								output += string.Format(@"
								<div class='att'>{0}</div>
								<input type='hidden' id='attributebox_{2}' value='{1}'>
								<div class='val'>
									<select onchange='chk_part();' name='' id='valuebox_{2}'>
										{3}
	", attribute_name, attribute_id, i, value_id == "0" ? "<option value='0' selected></option>" : "");
								foreach (DataRow subdr in subdt.Rows)
								{
									var this_value_name = subdr["value"].ToString();
									var this_value_id = subdr["attribute_value_id"].ToString();
									var this_selected = "";
									if (this_value_id == value_id)
									{
										this_selected = " selected";
									}
									else
									{
										this_selected = "";
									}
									output += string.Format(@"
										<option value='{0}'{2}>{1}</option>", this_value_id, this_value_name, this_selected);
								}
								output += @"
									</select>
								</div>";
								i++;
							}
								output += string.Format(@"
									<div id='stuff'></div>
								<input type='hidden' id='n_attributes' value='" + i + @"'>	
								</div>		
								</td>
							
							<td id='_properties' valign='top' align='left' style='padding:10px;'>
									<div><b style='display:table-cell;width:150px;'>Canadian Sold As:</b> <span style='display:table-cell;'>{2}</span></div>
									<div><b style='display:table-cell;width:150px;'>USA Sold As:</b> <span style='display:table-cell;'>{6}</span></div>
									<div><b style='display:table-cell;width:150px;'>Is Exclude?</b> <span style='display:table-cell;'>{3}</span></div>
									<div><b style='display:table-cell;width:150px;'>Allowed to Stock?</b> <span style='display:table-cell;'>{7}</span></div>
									<div><b style='display:table-cell;width:150px;'>Is QTY:</b> <span style='display:table-cell;'>{4}</span></div>
									<div><b style='display:table-cell;width:150px;'>Is Quick Add:</b> <span style='display:table-cell;'>{5}</span></div>
							</td>
						</tr>
					</table>
					<div align='center' width='100%' class='edit_pricing'>
					<table cellspacing='0' cellpadding='0' width='100%'>
						<tr>
							<td id='new_pricing'>
								<div id='price_boxes'>
									<div class='price_box' align='center'>
										<table cellspacing='0' cellpadding='2' width='100%'>
											<tr class='add_price'>
												<td width='125' class='company'>{0}</td>
												<td class='vendor'><input type='text' onfocus=""attach_ac(this, 'vendor')"" data-isac='false' value='' data-id='' data-business_unit_id='' data-master_id='" + master_id + @"' /></td>
												<td width='150' class='part_number'><input type='text' /></td>
												<td width='75' class='total_cost'><input type='text' onkeyup='refactor_pricing(this)' onkeydown='refactor_pricing(this)' /></td>
												<td width='65' class='qty'><input type='text' value='1' onkeyup='refactor_pricing(this)' onkeydown='refactor_pricing(this)' /></td>
												<td width='100' class='unit_cost' align='center'>0.00000</td>
												<td width='50'><button type='button' onclick='price_save(this, 1);' style='background-color:#aaa'><img src='/images/icon/icon[save].gif' align='absmiddle' /></button><button type='button' style='background-color:#999' disabled><img src='/images/icon/icon[delete].gif' align='absmiddle'/></button></td>
											</tr>
										</table>
									</div>
								</div>
							</td>
						</tr>
						<tr>
							<td id='existing_pricing'>
							
							</td>
						</tr>
					</table>
					</div>
					<div id='search_shade' onclick='search_inventory();'></div>
					<div id='search_popup'>
						<table cellspacing='0' cellpadding='0'>
							<tr>
								<td align='left' id='BRANCH_BV'></td>
							</tr>
							<tr>
								<td align='center' style='padding:10px;' id='SEARCH_BOX'>
									<input type='text' onkeyup='if(event.keyCode == 13){{search_bv();}}' id='KEYWORD'><button class='button' type='button' id='search_button' onclick='search_bv();'>search</button>
								</td>
							</tr>
							<tr>
								<td>
									<div id='BV_RESULTS'>&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td align='center' style='padding:5px;'>
									<button class='button' type='button' onclick='search_inventory();'>close</button>
								</td>
							</tr>
						</table>
					</div>
					<div id='merge_pane' align='center' style='display:none'>
						<input type='text' data-type='merge' onfocus='$(this).inventory()' style='width:100%;'/><br/><button style='width:100%;' data-old_master_id='{1}' disabled>merge</button>
					</div>
					<script>
						$(document).ready(function()
											{{
											$('#sell_price_table').dialog(
												{{
												autoOpen:	false,
												title:		'Sell Prices',
												resizable:	true,
												draggable:	true,
												modal:		true,
												width:		1024
												}});
											//$('#merge_pane').dialog(
											//	{{
											//	autoOpen:	false,
											//	title:		'Merged to Part',
											//	resizable:	false,
											//	draggable:	false,
											//	modal:		true,
											//	width:		150
											//	}});
											$('.sellprice').click(function()
																			{{
																			$('#sell_price_table').dialog('open');
																			}});
											$('.toggle_part').click(function()
															{{
															toggle_part({1}, this);
															}});
											refresh_sellprices();
											}});
					</script>
					",
	inv_class.business_unit_selector(), // {0}
	inventory.master_id,            // {1}
	prop_canadian_sold_as,          // {2}
	prop_is_exclude,                // {3}
	prop_is_qty,                    // {4}
	prop_quick_add,                 // {5}
	prop_usa_sold_as,               // {6}
	prop_is_allowed_to_stock        // {7}
	);
						} //if this is a genuine call
						else
						{
							Response.Redirect("./index.aspx");
						}
						break;
					#endregion
					#region PAGE - tag 
					case "tag":
						button_tags.Disabled = true;
						_tools.dont_cache_page();
						page_title.InnerText = "Tags";
						lbltemp.Text = "Inventory / Admin Access / Tags";
						string query = null;
						if (_q["q"] != null && _q["q"] != "")
						{
							query = _q["q"];
						}
						else if (_q["q"] == "")
						{
							Response.Redirect("./index.aspx?a=tag");
						}
						else
						{
							query = "";
						}
						var n_tags = inv_class.tag_count(query);
						n_pages = Math.Ceiling((double)n_tags / per_page);


						if (_q["seed"] != null)
						{
							seed = (string)_q["seed"];
							var i_seed = Convert.ToInt32(seed);
							if (i_seed % per_page != 0)
							{
								Response.Write("<script>alert('Invalid increment.');location.href='./index.aspx?a=tag';</script>");
								Response.End();
							}
							curr_page = (i_seed / per_page) + 1;
						}

						if (n_pages > 1)
						{
							for (var i = 0; i < n_pages; i++)
							{
								var i_page = i + 1;
								if (i_page == curr_page)
								{
									pages += "<i>" + i_page + "</i>";
								}
								else
								{
									if (query == "")
									{
										pages += string.Format(" <a href='./index.aspx?a=tag&seed={0}'>{1}</a>", (i * per_page), i_page);
									}
									else
									{
										pages += string.Format(" <a href='./index.aspx?a=tag&seed={0}&q={1}'>{2}</a>", (i * per_page), query, i_page);
									}
								}
							}
						}
						else if (n_tags == 0)
						{
							pages = "<a href='./index.aspx?a=tag'>1</a>";
						}

						else
						{
							pages = "<i>1</i>";
						}

						output += string.Format(@"
													<script>
													$(document).ready(function(){{
													$('#get_part').keydown(function(e)
														{{
														if(e.which == 13)
															{{
															location.href	= 'index.aspx?a=tag&q='+$(this).val();
															}}
														}});}});
													</script>
											<table cellpadding='10' cellspacing='0' class='nav_pane'>
												<tr>
													<td align='left' class='page_title'>Pages:</td>	
													<td align='left' class='page_numbers'>{0}</td>
													<td align='right' class='search_title'>SEARCH:</td>
													<td align='left' class='search_box'><input type='hidden' value='tag' name='a'><input type='text' id='get_part' onmouseover='this.focus();this.select();' value='{1}' name='q'></td>
												</tr>
											</table>", pages, query);


						if (query != "")
						{
							output += string.Format(@"
											<div class='query'>SEARCHING FOR: ""<i>{0}</i>""", query);
						}
						else
						{
							output += "<div class='query'>No query provided";
						}
						if (n_tags > 1)
						{
							output += " - " + n_tags + " Tags returned</div>";
						}
						else if (n_tags == 1)
						{
							output += " - " + n_tags + " Tag returned</div>";
						}
						else
						{
							output += "</div>";
						}

						var n_unapproved_tags = inv_class.n_Unapproved("inventory_tag");
						var n_approved_tags = inv_class.n_Approved("inventory_tag");
						output += string.Format(@"
		<div style='padding:10px;font-weight:bold;font-size:11px;' align='right'>
			{0} Unapproved Tags, {1} Approved Tags
		</div>", n_unapproved_tags, n_approved_tags);

						try
						{
							_dt = inv_class.tags(seed, query, per_page.ToString());
							output += @"
											<table class='tags' cellpadding='3' cellspacing='0'>
												<tr>
													<td class='add' colspan='6'>New Tag<br><input id='new_tag' type='text'  onmouseover='this.focus();' onkeydown=""var e = window.event;var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode; if(key == 13 && this.value != ''){create_tag(this);}else if(key == 13) {return false}"" /></td>
												</tr>";
							var counter = 0;
							var edit_button = "";
							var new_button = "";
							var delete_button = "";
							if (_dt.Rows.Count > 0)
							{
								foreach (DataRow dr in _dt.Rows)
								{
									var tag_id = dr["tag_id"].ToString();
									var tag = dr["tag"].ToString();
									var approved = Convert.ToInt32(dr["approved"].ToString());
									var approved_by = Convert.ToInt32(dr["approved_by"].ToString());
									var descriptor_scheme = inv_class.tag_descriptors(tag_id);
									var approved_by_name = "";
									var qc = "";
									var qc_class = "";
									var properties = Toolbox.doSQL_dt(@" SELECT a.is_qty, a.is_exclude, b.unit usa_sold_as, c.unit canadian_sold_as, a.quick_add, a.allowed_to_stock, a.allowed_to_edit_after_issue, a.is_consumable FROM inventory_tag a LEFT JOIN inventory_sold_as b ON a.usa_sold_as = b.id LEFT JOIN inventory_sold_as c ON a.canadian_sold_as = c.id WHERE a.tag_id = @v0  LIMIT 1", new object[] { tag_id });
									var prop_is_qty = "";
									var prop_is_consumable = "";
									var prop_is_exclude = "";
									var prop_usa_sold_as = "";
									var prop_canadian_sold_as = "";
									var prop_quick_add = "";
									var prop_allowed_to_stock = "";
									var prop_allowed_to_edit_after_issue = "";
									foreach (DataRow property in properties.Rows)
									{
										prop_is_qty = Convert.ToBoolean(property["is_qty"]).ToString();
										prop_is_consumable = Convert.ToBoolean(property["is_consumable"]).ToString();
										prop_is_exclude = Convert.ToBoolean(property["is_exclude"]).ToString();
										prop_usa_sold_as = property["usa_sold_as"].ToString();
										prop_canadian_sold_as = property["canadian_sold_as"].ToString();
										prop_quick_add = Convert.ToBoolean(property["quick_add"]).ToString();
										prop_allowed_to_stock = Convert.ToBoolean(property["allowed_to_stock"]).ToString();
										prop_allowed_to_edit_after_issue = Convert.ToBoolean(property["allowed_to_edit_after_issue"]).ToString();
									}

									#region new & edit button
									if (inv_class.been_linked(tag_id))
									{
										edit_button = string.Format(@"<button type='button' onclick='location.href=""./index.aspx?a=edit_linkage&tid={0}"";'><img src='/images/icon/icon[edit_mask].gif'/></button>", tag_id);
										new_button = "<img src='/images/icon/icon[edit_mask].gif' disabled/>";
									}
									else
									{
										edit_button = "<img src='/images/icon/icon[new_mask].gif' disabled/>";
										new_button = string.Format(@"<button type='button' onclick='location.href=""./index.aspx?a=create_linkage&tid={0}"";'><img src='/images/icon/icon[new_mask].gif'/></button>", tag_id);
									}
									#endregion
									#region delete button
									if (inv_class.tag_has_dependants(tag_id))
									{
										delete_button = string.Format(@"<button title='This tag has dependants and cannot be deleted.' type='button' disabled><img src='/images/icon/icon[delete].gif'/></button>", tag_id); ;
									}
									else
									{
										delete_button = string.Format(@"<button title='delete this tag' type='button' onclick='delete_tag({0});'><img src='/images/icon/icon[delete].gif'/></button>", tag_id);
									}
									#endregion delete button
									#region approval checkbox
									if (approved == 1)
									{
										qc = " CHECKED";
										qc_class = " style='background-color: #cfc;'";
									}
									#endregion approval checkbox
									counter++;
									output += string.Format(@"
												<tr class='att_row'>
													<td class='tag' data-editable='true' onclick='edit_tag({0},this);'><div style='cursor:pointer;width:100%;'>{1}</div></td>
													<td class='part_scheme'>{6}
														<div align='left'>
														<ul style='font-weight:normal'>
															<li>Canadian Sold As: <b>{7}</b></li>
															<li>USA Sold As: <b>{8}</b></li>
															<li>Allowed to Stock: <b>{13}</b></li>
															<li>Force Cost to 0.01? <b>{11}</b></li>
															<li>Is QTY: <b>{10}</b></li>
															<li>Is Consumable: <b>{15}</b></li>
															<li>Is Quick Add: <b>{9}</b></li>
															<li>Allowed to Edit after PO Issued: <b>{14}</b></li>
															<li># of parts created under this tag: {12}</li>
														</ul>
														</div></td>
													<td class='action' title='Create Linkage'>{3}</td>
													<td class='action' title='Edit Linkage'>{4}</td>
													<td class='action'>{5}</td>",
										tag_id,                         // {0}
										tag,
										counter,
										new_button,                     // {3}
										edit_button,
										delete_button,                  // {5}
										descriptor_scheme,
										prop_canadian_sold_as,          // {7}
										prop_usa_sold_as,
										prop_quick_add,                 // {9}
										prop_is_qty,
										prop_is_exclude,                // {11}
										Toolbox.doSQL_int(@"SELECT count(master_id) FROM inventory_item_master WHERE tag_id = @v0", tag_id),//12
										prop_allowed_to_stock, //13
										prop_allowed_to_edit_after_issue,  //14
										prop_is_consumable                  // 15
										);

									if (my_member.AuthenticatedForPrivilege(43) && inv_class.been_linked(tag_id))
									{
										if (Convert.ToBoolean(approved))
										{
											var app_by_member = new NeMember((int)approved_by);
											approved_by_name = "Approved By: " + app_by_member.FirstName + " " + app_by_member.LastName;
										}
										else
										{
											approved_by_name = "Has not been approved";
										}
										output += string.Format(@"
													<td class='approved' title='{3}' {2}><input onclick='approve({1}, ""tag"", ""tag"");' id='qc{1}' type='checkbox' {0}></td>", qc, tag_id, qc_class, approved_by_name);
									}
									else
									{
										if (approved == 1)
										{
											var app_by_member = new NeMember((int)approved_by);
											approved_by_name = "Approved By: " + app_by_member.FullName;
										}
										else
										{
											approved_by_name = "Has not been approved";
										}
										output += string.Format(@"
													<td class='approved' title='{3}' {2}><input onclick='approve({1}, ""tag"", ""tag"");' id='qc{1}' type='checkbox'></td>", qc, tag_id, qc_class, approved_by_name);
									}

									output += @"
												</tr>";
								}
							}
							else if (query != "")
							{
								output += @"
												<tr>
													<td class='error'>No tags match found using that query.</td>
												</tr>";
							}
							else
							{
								output += @"
												<tr>
													<td class='error'>No tags have been defined, please define the first one by inputing it into the field above, then press enter.</td>
												</tr>";
							}
							output += @"
											</table>";

						}
						catch (Exception ee)
						{
							output += @"
						<div class='error'>Couldn't pull the tags... An Error Was returned<br/>" + ee + "</div>";
						}
						break;
					#endregion tags
					#region PAGE - wo_parts
					case "wo_parts":
						page_title.InnerText = "Parts on Work Orders, Not QC'ed";
						lbltemp.Text = "Inventory / Admin Access / Parts on Work Orders, Not QC'ed";
						button_wo_parts.Disabled = true;
						gv_wo_unqced_parts.Visible = true;
						output += "<b></b>";
						break;
					#endregion
					#region PAGE - qc-attributes
					case "qc-attributes":
						page_title.InnerText = "Quality Check Attributes";
						lbltemp.Text = "Inventory / Admin Access / Quality Check Attributes";
						var n_Attributes = inv_class.n_Unapproved("inventory_attribute");
						n_pages = Math.Ceiling((double)n_Attributes / per_page);
						if (_q["seed"] != null)
						{
							seed = _q["seed"];
							var i_seed = Convert.ToInt32(seed);
							if (i_seed % per_page != 0)
							{
								Response.Write("<script>alert('Invalid increment.');location.href='./index.aspx?a=qc-attributes';</script>");
								Response.End();
							}
							curr_page = (i_seed / per_page) + 1;
						}

						if (n_pages > 1)
						{
							for (var i = 0; i < n_pages; i++)
							{
								var i_page = i + 1;
								if (i_page == curr_page)
								{
									pages += "<i>" + i_page + "</i>";
								}
								else
								{
									pages += string.Format(" <a href='./index.aspx?a=qc-attributes&seed={0}'>{1}</a>", (i * per_page), i_page);
								}
							}
						}
						else
						{
							pages = "<i>1</i>";
						}


						_dt = inv_class.Unapproved_Attributes(seed, per_page);
						output += string.Format(@"
						<div class='total'>{0} Total Unapproved Attributes, {1} Page(s)</div>
						<div class='page_numbers' style='padding:10px;background-color:#263C69;width:1030px;'><b style='margin-right:25px;color:#fff;'>Pages:</b> {2}</div>
						<table class='qc_attributes' cellpadding='5' cellspacing='0'>", n_Attributes, n_pages, pages);
						foreach (DataRow dr in _dt.Rows)
						{
							attribute_id = dr["attribute_id"].ToString();
							var counter = inv_class.values_for_attribute(attribute_id);
							attribute_name = dr["attribute"].ToString();
							output += string.Format(@"
							<tr>
								<td class='attribute_name'>{1} ({2} Values)</td>
								<td class='checkbox'><input id='qc{0}' type='checkbox' onclick='approve({0}, ""attribute"");' title='approve this attribute'></td>
							</tr>", attribute_id, attribute_name, counter);
						}
						output += @"
						</table>";
						break;
					#endregion qc-attributes
					#region PAGE - qc-values
					case "qc-values":
						page_title.InnerText = "Quality Check Values";
						lbltemp.Text = "Inventory / Admin Access / Quality Check Values";
						var nvalues = 0;
						if (_q["attribute_id"] != null)
						{
							nvalues = inv_class.n_Unapproved("inventory_attribute_value", _q["attribute_id"]);
						}
						else
						{
							nvalues = inv_class.n_Unapproved("inventory_attribute_value");
						}
						n_pages = Math.Ceiling((double)nvalues / per_page);
						if (_q["seed"] != null)
						{
							seed = _q["seed"];
							var i_seed = Convert.ToInt32(seed);
							if (i_seed % per_page != 0)
							{
								Response.Write("<script>alert('Invalid increment.');location.href='./index.aspx?a=qc-values';</script>");
								Response.End();
							}
							curr_page = (i_seed / per_page) + 1;
						}

						if (n_pages > 1)
						{
							for (var i = 0; i < n_pages; i++)
							{
								var i_page = i + 1;
								if (i_page == curr_page)
								{
									pages += "<i>" + i_page + "</i>";
								}
								else
								{
									if (_q["attribute_id"] != null)
									{
										pages += string.Format(" <a href='./index.aspx?a=qc-values&seed={0}&attribute_id={2}'>{1}</a>", (i * per_page), i_page, _q["attribute_id"]);
									}
									else
									{
										pages += string.Format(" <a href='./index.aspx?a=qc-values&seed={0}'>{1}</a>", (i * per_page), i_page);
									}
								}
							}
						}
						else
						{
							pages = "<i>1</i>";
						}
						if (_q["attribute_id"] != null)
						{
							_dt = inv_class.Unapproved_Values(seed, per_page, _q["attribute_id"]);
						}
						else
						{
							_dt = inv_class.Unapproved_Values(seed, per_page);
						}
						var attributes = inv_class.attributes_with_unapproved_values();
						var attributes_options = "";
						foreach (DataRow attribute in attributes.Rows)
						{
							var selected = "";
							if (_q["attribute_id"] != null && attribute["id"].ToString() == _q["attribute_id"])
							{
								selected = "SELECTED";
							}
							attributes_options += string.Format("<option value='{0}' {2}>{1} -- {3} VALUES</option>", attribute["id"], attribute["name"], selected, attribute["count"]);
						}
						output += string.Format(@"
		<div class='total'>{0} Total Unapproved Values, {1} Page(s)</div>
		<div style='background-color:#cfc;padding:10px;'><b>Attribute:</b> <select style='font-size:12px;' onchange=""location.href='./index.aspx?a=qc-values&attribute_id='+this.value;"">{3}</select></div>
		<div class='page_numbers' style='padding:10px;background-color:#263C69;width:1030px;'><b style='margin-right:25px;color:#fff;'>Pages:</b> {2}</div>
		<table class='qc_values' cellpadding='5' cellspacing='0'>
			<thead>
				<tr>
					<th>Value Info</th>
					<th width='150'>Used in Tag</th>
					<th width='100'>Used in Part</th>
					<th width='100'>Alternate Values</th>
					<th width='32'>Approve</th>
				</tr>
			</thead>", nvalues, n_pages, pages, attributes_options);
						foreach (DataRow dr in _dt.Rows)
						{
							attribute_id = dr["attribute_id"].ToString();
							attribute_name = dr["attribute_name"].ToString();
							value_id = dr["value_id"].ToString();
							value_name = dr["value_name"].ToString();
							var member_name = dr["member_name"].ToString();
							var tags_using_value = "";
							var parts_using_value = "";
							var alternate_values_list = "";
							var tags_using = Toolbox.doSQL_dt(@" SELECT a.tag_id, a.tag FROM inventory_tag a LEFT JOIN inventory_tag_preset b ON a.tag_id = b.tag_id LEFT JOIN inventory_attribute_value c on b.value_id = c.attribute_value_id WHERE c.attribute_value_id = @v0  AND c.active = true", new object[] { value_id });
							if (tags_using.Rows.Count > 0)
							{
								foreach (DataRow tag in tags_using.Rows)
								{
									tags_using_value += string.Format(@"<option value='{0}'>{1}</option>", tag["tag_id"], tag["tag"]);
								}
							}
							else
							{
								tags_using_value = "<option disabled>None</option>";
							}
							var parts_using = Toolbox.doSQL_dt(@" SELECT a.master_id FROM inventory_item_master a LEFT JOIN inventory_item_detail b on b.master_id = a.master_id LEFT JOIN inventory_attribute_value c ON b.attribute_value_id = c.attribute_value_id WHERE c.attribute_value_id = @v0  AND c.active = true", new object[] { value_id });
							if (parts_using.Rows.Count > 0)
							{
								foreach (DataRow part in parts_using.Rows)
								{
									parts_using_value += string.Format("<option value='{0}'>{0}</option>", part["master_id"]);
								}
							}
							else
							{
								parts_using_value = "<option disabled>None</option>";
							}
							if (attribute_id != "17")
							{
								var alternate_values = Toolbox.doSQL_dt(@"SELECT attribute_value_id, value value FROM inventory_attribute_value WHERE attribute_id = @v0  AND active = true AND attribute_value_id != @v1  ORDER BY value", new object[] { attribute_id, value_id });
								if (alternate_values.Rows.Count > 0)
								{
									foreach (DataRow val in alternate_values.Rows)
									{
										var attribute_value_id = val["attribute_value_id"].ToString();
										var attribute_value = val["value"].ToString();
										alternate_values_list += string.Format("<option value='{0}'>{1}</option>", attribute_value_id, attribute_value);
									}
								}
							}
							output += string.Format(@"
			<tr>
				<td class='attval'>
					<div class='value'>Value: {1}</div>
					<div class='attribute'><a style='color:#000;' href='./index.aspx?a=att&aid={8}#att_{8}'>Attribute: {2}</a></div>
					<div>Created By: {3}</div></td>
				<td style='border-bottom:solid 1px #ccc;'><select onchange=""location.href='./index.aspx?a=edit_linkage&tid='+this.value"" style='width:150px;font-size:11px;'><option value=''>Tags ({6})</option>{4}</select></td>
				<td style='border-bottom:solid 1px #ccc;'><select onchange=""location.href='./index.aspx?a=edit_part&master_id='+this.value"" style='width:100px;font-size:11px;'><option value=''>Parts ({7})</option>{5}</select></td>
				<td style='border-bottom:solid 1px #ccc;'><select class='alternates' style='width:150px;font-size:11px;' onchange=""if(this.value != 0){{if(confirm('Are you sure you wish to switch this value?')){{change_value({0}, this.value, this);}}}}""><option value='0'>Switch To:</option>{9}</select></td>
				<td class='checkbox'><input id='qc{0}' type='checkbox' onclick='approve({0}, ""value"");' title='approve this value'></td>
			</tr>",
		value_id,                   // {0}
		value_name,                 // {1}
		attribute_name,             // {2}
		member_name,                // {3}
		tags_using_value,           // {4}
		parts_using_value,          // {5}
		tags_using.Rows.Count,      // {6}
		parts_using.Rows.Count,     // {7}
		attribute_id,               // {8}
		alternate_values_list       // {9}
		);
						}
						output += @"
		</table>
		<script>
			$('document').ready(function()
									{
									$('body').find('select > option').each(function()
																				{
																				$(this).attr('title', $(this).text());
																				});
									});
		</script>
		";
						break;
					#endregion qc-values
					#region PAGE - qc-pics
					case "qc-pics":
						page_title.InnerText = "Quality Check Pictures";
						lbltemp.Text = "Inventory / Admin Access / Quality Check Pictures";
						output += @"<div id='unqcedpics'></div>
							<script>
							$(document).ready(function()
												{
												load_unqcedpics();
												});
							</script>";
						break;
					#endregion qc-pics
					#region PAGE - qc-parts
					case "qc-parts":
						page_title.InnerText = "Quality Check Parts";
						lbltemp.Text = "Inventory / Admin Access / Quality Check Parts";
						var n_parts = 0;
						if (_q["tag_id"] != null)
						{
							TagID = _q["tag_id"];
							n_parts = inv_class.n_Unapproved("inventory_item_master", TagID);
						}
						else
						{
							TagID = "";
							n_parts = inv_class.n_Unapproved("inventory_item_master");
						}
						n_pages = Math.Ceiling((double)n_parts / per_page);
						if (_q["seed"] != null)
						{
							seed = _q["seed"];
							var i_seed = Convert.ToInt32(seed);
							if (i_seed % per_page != 0)
							{
								Response.Write("<script>alert('Invalid increment.');location.href='./index.aspx?a=qc-parts';</script>");
								Response.End();
							}
							curr_page = (i_seed / per_page) + 1;
						}


						if (n_pages > 1)
						{
							for (var i = 0; i < n_pages; i++)
							{
								var i_page = i + 1;
								if (i_page == curr_page)
								{
									pages += "<i>" + i_page + "</i>";
								}
								else
								{
									var url = TagID == "" ? string.Format(" <a href='./index.aspx?a=qc-parts&seed={0}'>{1}</a>", (i * per_page), i_page) : string.Format(" <a href='./index.aspx?a=qc-parts&seed={0}&tag_id={2}'>{1}</a>", (i * per_page), i_page, TagID);
									pages += url;
								}
							}
						}
						else
						{
							pages = "<i>1</i>";
						}

						_dt = inv_class.Unapproved_Parts(seed, per_page, TagID);
						output += string.Format(@"
						<div id='part_detail' onclick='close_part();'></div>
						<div id='part_info'>
							<div class='part_number'></div>
							<div id='part_attvals'></div>
							<button type='button' onclick='close_part();'>close part</button>
						</div>
						<div class='total'>{0} Total Unapproved Parts, {1} Page(s)</div>
						<div class='page_numbers' style='padding:10px;background-color:#263C69;width:1030px;'><b style='margin-right:25px;color:#fff;'>Pages:</b> {2}</div>", n_parts, n_pages, pages);
						output += @"
						<select style='width:1030px;' onchange=""if(this.value != 0){location.href='/sections/admin/inventory/index.aspx?a=qc-parts&tag_id='+this.value}else{location.href='/sections/admin/inventory/index.aspx?a=qc-parts';}this.blur();"">
							<option value='0'>All Tags</option>";
						foreach (DataRow tag in inv_class.tags().Rows)
						{
							var tag_id = tag["tag_id"].ToString();
							tag_name = tag["tag"].ToString();
							var this_selected = "";
							if (TagID == tag_id)
							{
								this_selected = " selected";
							}
							else
							{
								this_selected = "";
							}
							output += string.Format(@"
							<option value='{0}' {2}>{1}</option>", tag_id, tag_name, this_selected); ;
						}
						output += @"
						</select>
						<table class='qc_parts' cellpadding='5' cellspacing='0'>";
						if (_dt.Rows.Count > 0)
						{
							foreach (DataRow dr in _dt.Rows)
							{
								var obj = new NVC();
								obj["tag_id"] = dr["tag_id"].ToString();
								obj["master_id"] = dr["master_id"].ToString();
								obj["created_by"] = dr["created_by"].ToString();
								obj["create_date"] = dr["create_date"].ToString();
								var this_member = new NeMember(Convert.ToInt32(obj["created_by"]));
								obj["created_by_name"] = string.Format("{0} {1}", this_member.FirstName, this_member.LastName);
								obj["created_by_company"] = this_member.business_unit_name;
								obj["description"] = inv_class.Part_Description(obj["master_id"]);
								obj["tag_name"] = inv_class.tag_name(obj["tag_id"]);
								obj["unapproved_values"] = Toolbox.doSQL_string(@"SELECT count(*) FROM inventory_attribute_value a
WHERE a.attribute_value_id IN (SELECT b.attribute_value_id FROM inventory_item_detail b WHERE b.master_id = @v0) AND a.approved = false", obj["master_id"]);

								output += string.Format(@"
		<tr>
			<td class='partspace' title='click to edit/view part' onclick=""location.href='./index.aspx?a=edit_part&master_id={1}&tag_id={0}';""><div class='partnumber'>{1} - {3}</div><div style='color:red;font-size:11px;'>{6}</div><div style='font-size:9px;'>CREATED BY: {4} FROM {5}</div><div>({7}) Unapproved Values</div></td>
			<td class='checkbox'><input type='checkbox' id='qc{1}' onclick='approve({1}, ""part"");' title='approve this part'></td>
		</tr>
		",
			obj["tag_id"],
			obj["master_id"],
			obj["tag_name"],
			obj["description"],
			obj["created_by_name"],
			obj["created_by_company"],
			obj["create_date"],
			obj["unapproved_values"]
			);
							}
						}
						else
						{
							output += @"
		<tr>
			<td colspan='2'>No current unapproved parts</td>
		</tr>";
						}
						output += @"
						</table>";
						break;
					#endregion qc-parts
					#region PAGE - splitparts
					case "splitparts":
						page_title.InnerText = "Split Parts";
						lbltemp.Text = "Inventory / Admin Access / Split Parts";
						button_split_parts.Disabled = true;
						pane_splitparts.Visible = true;
						_h.Set("gridview_id", "gv_splitparts");
						_panel_export.Visible = true;
						page_name = "AdminInventory_gv_splitparts";
						layout.__page_name = page_name;
						var gl = new NeGridLayouts(Convert.ToInt32(my_member.id), page_name);

						if (gl.GridLayout_Layout != "")
						{
							gv_splitparts.LoadClientLayout(gl.GridLayout_Layout);
						}
						else
						{
							gl = new NeGridLayouts();
							gl.GridLayout_Layout = gv_splitparts.SaveClientLayout();
							gl.member_id = my_member.id;
							gl.GridLayout_Name = "Default";
							gl.GridLayout_Gridid = page_name;
							gl.SaveGridLayout();
							Toolbox.doSQL_void(@"UPDATE gridviewlayouts SET is_default = true WHERE gridviewlayouts_id=@v0 LIMIT 1", gl.GridLayoutID);
						}
						hidexp.Value = gv_splitparts.SaveClientLayout();
						_h.Set("ID", gl.GridLayoutID);
						_h.Set("NAME", gl.GridLayout_Name);
						_ds_templates.SelectParameters["@page_name"].DefaultValue = page_name;
						_ds_templates.SelectParameters["@member_id"].DefaultValue = my_member.id.ToString();
						output += "<b></b>";
						break;
					#endregion PAGE - splitparts
					#endregion PAGES
					#region XML Handlers
					#region available_xml_values
					case "available_xml_values":
						aid = _q["aid"];
						TagID = _q["tag_id"];
						_dt = inv_class.available_values(TagID, aid);
						var sb_available_xml_values = new StringBuilder();
						sb_available_xml_values.Append(@"
<values>");
						foreach (DataRow dr in _dt.Rows)
						{
							var xml_value_id = dr["attribute_value_id"].ToString();
							var xml_value = dr["value"].ToString();
							sb_available_xml_values.AppendFormat(@"
	<value id='{0}' name=""{1}"" />", xml_value_id, HttpUtility.HtmlEncode(xml_value));
						}
						sb_available_xml_values.Append(@"
</values>");
						Response.Write(sb_available_xml_values.ToString());
						break;
					#endregion available_xml_values
					#region prepop_xml_tag_att_val
					case "prepop_xml_tag_att_val":
						TagID = _q["tag_id"];
						_dt = inv_class.Browse_Attributes(TagID);
						if (_dt.Rows.Count > 0)
						{
							Response.Write("<attvals>");

							foreach (DataRow dr in _dt.Rows)
							{
								attribute_id = dr["attribute_id"].ToString();
								attribute_name = HttpUtility.HtmlEncode(dr["attribute"]);
								Response.Write(string.Format(@"
			<attribute id=""{0}"" name=""{1}"">", attribute_id, attribute_name));

								subdt = inv_class.Browse_Values(TagID, attribute_id);
								foreach (DataRow subdr in subdt.Rows)
								{
									value_id = subdr["value_id"].ToString();
									value_name = HttpUtility.HtmlEncode(subdr["value"]);
									Response.Write(string.Format(@"
					<value id='{0}' name=""{1}"" />", value_id, value_name));
								}
								Response.Write(@"
			</attribute>");
							}
							Response.Write("\n</attvals>");
						}
						else
						{
							Response.Write(@"
		<attvals>
			<attribute id='0' name='Invalid Tag ID'>
				<value id='0' name='No values available'/>
			</attribute>
		</attvals>");
						}
						break;
					#endregion prepop_xml_tag_att_val
					#region selected_xml_values
					case "selected_xml_values":
						aid = _q["aid"];
						TagID = _q["tag_id"];
						_dt = inv_class.selected_values(TagID, aid);
						if (_dt.Rows.Count > 0)
						{
							Response.Write(@"
					<values>");
							foreach (DataRow dr in _dt.Rows)
							{
								var xml_value_id = dr["attribute_value_id"].ToString();
								var xml_value = HttpUtility.HtmlEncode(dr["value"]);
								Response.Write(string.Format(@"
						<value id='{0}' name=""{1}"" />", xml_value_id, xml_value));
							}
							Response.Write(@"
					</values>");
						}
						else
						{
							Response.Write(@"
					<values>
					
					</values>");
						}
						break;
					#endregion selected_xml_values
					#region xml_attributes
					case "xml_attributes":
						_dt = inv_class.attributes();
						if (_dt.Rows.Count > 0)
						{
							Response.Write(@"
<attributes>");
							foreach (DataRow dr in _dt.Rows)
							{
								var xml_attribute_id = dr["attribute_id"].ToString();
								var xml_attribute = dr["attribute"].ToString();
								Response.Write(string.Format(@"
	<attribute id='{0}' name=""{1}"" />
", xml_attribute_id, xml_attribute));
							}
							Response.Write(@"
</attributes>");
						}
						Response.End();
						break;
					#endregion xml_attributes
					#region xml_company
					case "xml_company":
						_dt = Toolbox.doSQL_dt(@"SELECT * from business_unit  WHERE has_inventory = true", null);
						//_dt							= NeShared.GetBV7DSNs();
						Response.Write(@"
<companies>");
						foreach (DataRow dr in _dt.Rows)
						{
							business_unit_id = (int)dr["business_unit_id"];
							name = dr["name"].ToString();
							var this_is_canadian = false;
							if (dr["country"].ToString() == "CDN")
							{
								this_is_canadian = true;
							}
							Response.Write(string.Format(@"
	<company id='{0}' name=""{1}"" is_canadian=""{2}"" />", business_unit_id, name, this_is_canadian));
						}
						Response.Write(@"
</companies>");
						break;
					#endregion
					#region xml_existant_woparts
					//case "xml_existant_woparts":
					//	if (_q["DSN"] != null)
					//	{
					//		var dsn = _q["DSN"];
					//		var this_output = inv_class.linked_parts(dsn);
					//		Response.Write(this_output);
					//	}
						break;
					#endregion xml_existant_woparts
					#region xml_lookup_part
//					case "xml_lookup_part":
//						if (_q["Q"] != "" && _q["DSN"] != "")
//						{
//							var dsn = (string)_q["DSN"];
//							var q = (string)_q["Q"];
//							_dt = inv_class.part_lookup(q, dsn);
//							var code = "";
//							var description = "";
//							Response.Write(@"
//<results>");
//							foreach (DataRow dr in _dt.Rows)
//							{
//								code = dr["CODE"].ToString();
//								code = code.Replace("\"", "");
//								description = dr["DESCRIPTION"].ToString();
//								description = description.Replace("\"", " inch");
//								Response.Write(string.Format(@"
//	<match code=""{0}"" description=""{1}"" />", code, description));
//							}
//							Response.Write(@"
//</results>");
//						}
//						break;
					#endregion xml_lookup_part
					#region xml_match_parts
					case "xml_match_parts":
						try
						{
							var tag_id = _q["tag_id"];
							var attval_pattern = _q["attvals"];
							var description = "";
							_dt = inv_class.Match_Parts(tag_id, attval_pattern);
							Response.Write(@"
<matches>");
							foreach (DataRow dr in _dt.Rows)
							{
								master_id = dr["item"].ToString();
								description = HttpUtility.HtmlEncode(inv_class.Part_Description(master_id));
								var this_active = dr["active"].ToString();
								Response.Write(string.Format(@"
	<part tag=""{0}"" masterid=""{1}"" description=""{2}"" number=""{0}-{1}"" active=""{3}""/>", tag_id, master_id, description, this_active));
							}
							Response.Write(@"
</matches>");
						}
						catch (Exception ee)
						{
							_tools.catch_error(ee);
							Response.Write(@"
<matches>
<part number='error' />
</matches>");
						}
						break;
					#endregion xml_match_parts
					#region xml_nonexistant_woparts
					//case "xml_nonexistant_woparts":
					//	if (_q["DSN"] != null)
					//	{
					//		var dsn = _q["DSN"];
					//		var this_output = inv_class.not_linked_parts(dsn);
					//		Response.Write(this_output);
					//	}
					//	break;
					#endregion xml_non_existant_woparts
					#region xml_part_att_val
					case "xml_part_att_val":
						master_id = _q["master_id"];
						_dt = inv_class.Part_Att_Vals(master_id);
						_dt = _tools.HTMLize_datatable(_dt);
						_dt.TableName = "info";
						_dt.WriteXml(Response.OutputStream);
						break;
					#endregion
//					#region xml_part_history
//					case "xml_bv_special_pricing":
//						if (_q["CODE"] != null && _q["DSN"] != null)
//						{
//							var code = _q["CODE"];
//							var dsn = _q["DSN"];
//							var vendor_pricing = Toolbox.doSQL_dt(@" SELECT b.VEN_NO vendor_code, b.NAME vendor_name, a.selling_price cost_price,
//a.vendor_code vendor_part_no FROM special_pricing a LEFT JOIN vendor b ON a.BVSPECPRICECODE = b.VEN_NO WHERE a.bvspecpricepartno = ? ", dsn, new object[] { code });
//							Response.Clear();
//							vendor_pricing.TableName = "vendor_pricing";
//							vendor_pricing.WriteXml(Response.OutputStream);
//							Response.End();
//						}
//						break;
//					#endregion xml_part_history
					#region xml_pics_not_qced
					case "xml_pics_not_qced":
						_dt = Toolbox.doSQL_dt(@"
							SELECT 
								a.id,
								d.tag_id,
								e.tag tag,
								a.master_id,
								inventory_description.description AS description,
								DATE_FORMAT(a.insert_dt, '%m/%d/%Y') insert_dt, 
								b.member_id,
								CONCAT(b.member_firstname,' ', b.member_lastname) by_who, 
								c.name,
								ext, 
								x, 
								y 
							FROM 
								inventory_picture a 
							LEFT JOIN 
								member b ON a.added_by = b.member_id 
							LEFT JOIN 
								business_unit c ON b.business_unit_id = c.id 
							LEFT JOIN
								inventory_item_master d ON a.master_id = d.master_id
							LEFT JOIN
								inventory_tag e ON d.tag_id = e.tag_id
INNER JOIN inventory_description ON a.master_id = inventory_description.master_id
							WHERE 
								a.approved is null
							ORDER BY insert_dt desc limit 25;", null);
						_dt.TableName = "pic";
						_dt.WriteXml(Response.OutputStream);
						break;
					#endregion xml_pics_not_qced
					#region xml_sellprices
					case "xml_sellprices":
						var this_inventory = new inventory();
						if (_q["master_id"] != null && this_inventory.part_exists(_q["master_id"]))
						{
							Response.Write(@"
<prices>");
							Response.Write(@"
</prices>");
							Response.End();
						}
						break;
					#endregion
					#region xml_prices
					case "xml_prices":
						master_id = _q["master_id"];
						_dt = inv_class.Get_Pricing(master_id);
						_dt.TableName = "price";
						_dt = _tools.HTMLize_datatable(_dt);
						_dt.WriteXml(Response.OutputStream);
						break;
					#endregion xml_prices
					#region xml_tag_att_val
					case "xml_tag_att_val":
						TagID = _q["tag_id"];
						_dt = inv_class.attributes(TagID);
						if (_dt.Rows.Count > 0)
						{
							Response.Write("<attvals>");

							foreach (DataRow dr in _dt.Rows)
							{
								attribute_id = dr["attribute_id"].ToString();
								attribute_name = dr["attribute"].ToString();
								Response.Write(string.Format(@"
			<attribute id=""{0}"" name=""{1}"">", attribute_id, HttpUtility.HtmlEncode(attribute_name)));

								subdt = inv_class.tag_attribute_selected_values(TagID, attribute_id);
								foreach (DataRow subdr in subdt.Rows)
								{
									value_id = subdr["attribute_value_id"].ToString();
									value_name = subdr["value"].ToString();
									Response.Write(string.Format(@"
					<value id='{0}' name=""{1}"" />", value_id, HttpUtility.HtmlEncode(value_name)));
								}
								Response.Write(@"
			</attribute>");
							}

							Response.Write("\n</attvals>");
						}
						else
						{
							Response.Write(@"
		<attvals>
			<attribute id='0' name='Invalid Tag ID'>
				<value id='0' name='No values available'/>
			</attribute>
		</attvals>");
						}
						Response.End();
						break;
					#endregion xml_tag_att_val
					#region xml_tax_defaults
					case "xml_tax_defaults":
						if (_q["tag_id"] != null)
						{
							_dt = Toolbox.doSQL_dt(@"SELECT * FROM inventory_tax  WHERE tag_id =@v0", new object[] { _q["tag_id"] });
							if (_dt.Rows.Count > 0)
							{
								_dt.TableName = "tax";
								_dt.WriteXml(Response.OutputStream);
							}
							else
							{
								Response.Write(@"<error>No Rows Returned</error>");
							}
						}
						else if (_q["master_id"] != null)
						{
							_dt = Toolbox.doSQL_dt(@"SELECT * FROM inventory_tax  WHERE master_id =@v0", new object[] { _q["master_id"] });
							if (_dt.Rows.Count > 0)
							{
								_dt.TableName = "tax";
								_dt.WriteXml(Response.OutputStream);
							}
							else
							{
								Response.Write(@"<error>No Rows Returned</error>");
							}
						}
						else
						{
							Response.Write(@"<error>Master ID not supplied</error>");
						}
						Response.End();
						break;
					#endregion xml_tax_defaults
					#region xml_tag_tradename
					case "xml_tag_tradename":
						_dt = Toolbox.doSQL_dt(@"SELECT id,tradename tradename from inventory_tag_tradename WHERE tag_id = @v0 ", new object[] { _q["tag_id"] });
						if (_dt.Rows.Count > 0)
						{
							Response.Write(@"
		<tradenames>");
							foreach (DataRow dr in _dt.Rows)
							{
								var id = dr["id"].ToString();
								var tradename = dr["tradename"].ToString();
								Response.Write(string.Format(@"
			<tradename>
				<id>{0}</id>
				<name>{1}</name>
			</tradename>", id, tradename));
							}
							Response.Write(@"
		</tradenames>");
						}
						else
						{
							Response.Write("<error>No tradenames exist</error>");
						}
						Response.End();
						break;
					#endregion xml_tag_tradename
					#region xml_values
					case "xml_values":
						aid = _q["aid"];
						_dt = inv_class.values(aid);
						if (_dt.Rows.Count > 0)
						{
							Response.Write(@"
		<values>");
							foreach (DataRow dr in _dt.Rows)
							{
								var xml_value_id = dr["attribute_value_id"].ToString();
								var xml_value = dr["value"].ToString();
								Response.Write(string.Format(@"
			<value id='{0}' name=""{1}"" />
		", xml_value_id, xml_value));
							}
							Response.Write(@"
		</values>");
						}
						Response.End();
						break;
					#endregion xml_values
					#endregion XML Handlers
					#region GET/SET Handlers
					#region add_attribute
					case "add_attribute":
						if (_q["attribute"] != null)
						{
							attribute_name = Toolbox.do_value_from(_q["attribute"], false);
							aid = inv_class.add_attribute(attribute_name);
							try
							{
								Convert.ToInt32(aid);
								Response.Redirect("./index.aspx?a=att&aid=" + aid);
							}
							catch
							{
								Response.Write("<script>alert('There was an error adding the supplied attribute.\\n\\nPossible causes:\\n - blank value\\n - duplicate value\\n - invalid characters');location.href('./index.aspx?a=att&aid');</script>");
								Response.End();
							}
						}
						else
						{
							Response.Write("<script>alert('There was an error adding the supplied attribute.\\n\\nPossible causes:\\n - blank value\\n - duplicate value\\n - invalid characters');location.href('./index.aspx?a=att&aid');</script>");
							Response.End();
						}
						break;
					#endregion add_attribute
					#region add_value
					case "add_value":
						if (_q["value"] != null && Regex.IsMatch(_q["value"], @"(\w+)") && _q["aid"] != null)
						{
							var value = _q["value"].Trim();
							aid = _q["aid"];
							try
							{
								inv_class.add_value(aid, value);
								Response.Redirect("./index.aspx?a=att&aid=" + aid);
							}
							catch
							{
								Response.Write("<script>alert('There was an error adding the supplied value.\\n\\nPossible causes:\\n - blank value\\n - duplicate value\\n - invalid characters');location.href('./index.aspx?a=att&aid=" + aid + @"');</script>");
								Response.End();
							}
						}
						else
						{
							aid = _q["aid"];
							Response.Write("<script>alert('There was an error adding the supplied value.\\n\\nPossible causes:\\n - blank value\\n - duplicate value\\n - invalid characters');location.href('./index.aspx?a=att&aid=" + aid + "');</script>");
							Response.End();
						}
						break;
					#endregion add_value
					#region tag_value
					case "tag_value":
						inv_class.tag_value();
						break;
					#endregion tag_value
					#region change_value
					case "change_value":
						// Goal: Change all links from the old value to the new value.
						var proper_value_id = "";
						var improper_value_id = "";
						Response.Clear();
						var val_sb = new StringBuilder();
						if (_q["improper_value_id"] != null && _q["proper_value_id"] != null)   // Good
						{
							improper_value_id = _q["improper_value_id"];    // i.e 2351
							proper_value_id = _q["proper_value_id"];        // i.e 242
							val_sb.AppendFormat("Improper Value ID: {0}<br/>", improper_value_id);
							val_sb.AppendFormat("Proper Value ID: {0}<br/>", proper_value_id);
							var preset_tags = Toolbox.doSQL_dt(@"SELECT tag_id FROM inventory_tag_preset WHERE value_id = @v0 ", new object[] { improper_value_id });
							if (preset_tags.Rows.Count > 0)
							{
								var c = 1;
								foreach (DataRow preset_tag in preset_tags.Rows)
								{
									val_sb.AppendFormat("---Result {0}<br/>", c);
									var preset_tag_id = preset_tag["tag_id"].ToString();
									val_sb.AppendFormat("Preset Tag ID: {0}<br/>", preset_tag_id);
									var proper_preset_exists = Toolbox.doSQL_int(@"SELECT COUNT(*) c FROM inventory_tag_preset
WHERE tag_id = @v0 AND value_id = @v1", new object[] { preset_tag_id, proper_value_id });
									val_sb.AppendFormat("Proper Preset Exists?: {0}<br/>", proper_preset_exists);
									var preset_parts = Toolbox.doSQL_dt(@"SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = @v1  AND associated_tag(master_id) = @v0 ", new object[] { preset_tag_id, improper_value_id });
									if (preset_parts.Rows.Count > 0)
									{
										val_sb.Append("Has Subs\n");
										var sub_c = 1;
										foreach (DataRow preset_part in preset_parts.Rows)
										{
											val_sb.AppendFormat("---Sub Result {0}<br/>", sub_c);
											var preset_part_id = preset_part["master_id"].ToString();
											val_sb.AppendFormat("Preset Part ID: {0}<br/>", preset_part_id);
											var preset_part_csv = Toolbox.doSQL_string(@"SELECT CAST(CONCAT(',',SUBSTRING(attval, 2, length(attval) - 2)) AS CHAR) attval
FROM inventory_lookup WHERE item = @v0", preset_part_id);
											val_sb.AppendFormat("Preset Part CSV: {0}<br/>", preset_part_csv);
											// This had the possibility/probability of being wrong, an attval preset csv is ordered by the attribute value id ascending, if you were to
											// change a higher value, to a lower number, it would not match any possible dupes.
											// i.e 1,2,3,4,5
											// now replace 3 with 6
											// 1,2,6,4,5
											// If one existed with 1,2,4,5,6... this wouldn't be matched as a duplicate... and parts wouldn't be merged.
											var corrected_preset_part_csv = preset_part_csv.Replace(string.Format(",{0},", improper_value_id), string.Format(",{0},", proper_value_id));
											// So we must prepare this object...
											corrected_preset_part_csv = corrected_preset_part_csv.TrimStart(',').TrimEnd(',');
											// Make it into a simple array
											var exploded_csv = corrected_preset_part_csv.Split(',');
											var attval_ids = new List<int>();
											int parsed_id;
											foreach (var n in exploded_csv)
											{
												if (int.TryParse(n, out parsed_id))
												{
													attval_ids.Add(parsed_id);
												}
											}
											attval_ids.Sort();
											// Now remake the csv
											corrected_preset_part_csv = ",";
											foreach (var n in attval_ids)
											{
												corrected_preset_part_csv += n + ",";
											}

											val_sb.AppendFormat("Corrected Preset Part CSV: {0}<br/>", corrected_preset_part_csv);
											var sub_part_csv = corrected_preset_part_csv.Remove(0, 1);
											val_sb.AppendFormat("Sub Part CSV: {0}<br/>", sub_part_csv);
											corrected_preset_part_csv = string.Format("|{0}|", sub_part_csv);
											val_sb.AppendFormat("Corrected Preset Part CSV: {0}<br/>", corrected_preset_part_csv);
											var proper_master_id = "";
											var proper_master_check = Toolbox.doSQL_int(@"SELECT COUNT(item) FROM inventory_lookup
WHERE attval = @v0", corrected_preset_part_csv);
											val_sb.AppendFormat("Proper Master Check: {0}<br/>", proper_master_check);

											if (proper_master_check > 0)
											{
												val_sb.Append("Has Proper Master ID<br/>");
												proper_master_id = Toolbox.doSQL_string(@"SELECT item master_id FROM inventory_lookup 
WHERE attval = @v0", corrected_preset_part_csv);
												val_sb.AppendFormat("Proper Master ID: {0}<br/>", proper_master_id);
												inventory.merge_part(preset_part_id, proper_master_id);
												val_sb.AppendFormat("Merged Master ID: {0} to Master ID: {1}<br/>", preset_part_id, proper_master_id);
											}
											else
											{
												val_sb.Append("Doesn't have a Proper Master ID<br/>");
												Toolbox.doSQL_void(@"UPDATE inventory_item_detail
SET attribute_value_id = @v0 WHERE master_id = @v1 AND attribute_value_id = @v2", new object[] { proper_value_id, preset_part_id, improper_value_id });
												val_sb.AppendFormat(@"UPDATE inventory_item_detail SET attribute_value_id = {0} WHERE master_id = {1} AND attribute_value_id = {2}<br/>", proper_value_id, preset_part_id, improper_value_id);
												Toolbox.doSQL_void(@"UPDATE inventory_description SET 
	description = FULL_PART_DESCRIPTION(master_id, false, ''), 
	desc_short_cdn = PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_short_usa = PART_DESCRIPTION(master_id, false, 'USA'), 
	desc_full_cdn = FULL_PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_full_usa = FULL_PART_DESCRIPTION(master_id, false, 'USA') WHERE master_id in (SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = @v0)", proper_value_id);
												val_sb.AppendFormat(@"UPDATE inventory_description SET 
	description = FULL_PART_DESCRIPTION(master_id, false, ''), 
	desc_short_cdn = PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_short_usa = PART_DESCRIPTION(master_id, false, 'USA'), 
	desc_full_cdn = FULL_PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_full_usa = FULL_PART_DESCRIPTION(master_id, false, 'USA') WHERE master_id in (SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = {0})<br/>", proper_value_id);

											}
										}
									}
									if (proper_preset_exists == 1)
									{
										Toolbox.doSQL_void(@"DELETE FROM inventory_tag_preset WHERE tag_id = @v0 AND value_id = @v1 LIMIT 1", new object[] {
											preset_tag_id, improper_value_id});
										val_sb.AppendFormat("DELETE FROM inventory_tag_preset WHERE tag_id = {0} AND value_id = {1} LIMIT 1<br/>", preset_tag_id, improper_value_id);
									}
									else
									{
										Toolbox.doSQL_void(string.Format("UPDATE inventory_tag_preset SET value_id = {2} WHERE value_id = {1} AND tag_id = {0} LIMIT 1", preset_tag_id, improper_value_id, proper_value_id));
										val_sb.AppendFormat("UPDATE inventory_tag_preset SET value_id = {2} WHERE value_id = {1} AND tag_id = {0} LIMIT 1<br/>", preset_tag_id, improper_value_id, proper_value_id);
									}
								}
							}
							inv_class.delete_value(improper_value_id);
							Toolbox.doSQL_void(string.Format("UPDATE inventory_item_master SET approved = true, approved_by = {0}, approved_date = now() WHERE unapproved_values(master_id) = 0 and (approved = false or approved = 0 or approved is null);", my_member.id));
							val_sb.AppendFormat("UPDATE inventory_item_master SET approved = true, approved_by = {0}, approved_date = now() WHERE unapproved_values(master_id) = 0 and (approved = false or approved = 0 or approved is null);<br/>", my_member.id);
							try
							{
								_tools.page_author = new NeMember(711);
								var message = new NeEMail();
								message.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
								message.isHTML = true;
								message.To = _tools.page_author.NEEmail;
								message.Body = val_sb.ToString();
								message.Body = message.Body.Replace("\n", "<br/>\n");
								message.Subject = string.Format("Value Merge Debug Trace");
								message.Send();
							}
							catch (Exception ee)
							{
								_tools.catch_error(ee);
							}
							Response.Write("SUCCESS");
						}
						else
						{
							Response.Write("FAILED");
						}
						Response.End();
						break;
					#endregion
					#region chk_uniqueness
					case "chk_uniqueness":
						_tools.dont_cache_page();
						if (_q["tag"] != null && _q["tag_pattern"] != null)
						{
							TagID = (string)_q["tag"];
							var tag_pattern = (string)_q["tag_pattern"];

							if (_q["master_id"] != null)
							{
								master_id = (string)_q["master_id"];
								if (inv_class.is_item_unique(TagID, tag_pattern, master_id))
								{
									Response.Write("UNIQUE");
								}
								else
								{
									Response.Write("COMMON");
								}
							}
							else
							{
								if (inv_class.is_item_unique(TagID, tag_pattern))
								{
									Response.Write("UNIQUE");
								}
								else
								{
									Response.Write("COMMON");
								}
							}
							Response.End();
						}
						else
						{
							Response.Write("FAILED");
							Response.End();
						}
						break;
					#endregion chk_uniqueness	
					#region deletetag
					case "deletetag":
						if (_q["tag_id"] != null)
						{
							var tag_id = _q["tag_id"];
							if (inv_class.delete_tag(tag_id))
							{
								Response.Redirect("./index.aspx?a=tag");
							}
							else
							{
								Response.Write(@"
<script>
	alert('There was an error deleting the requested tag.');
	location.href('./index.aspx?a=tag');
</script>
");
								Response.End();
							}
						}
						else
						{
							Response.Redirect("./index.aspx?a=tag");
						}
						break;
					#endregion deletetag
					#region save_tradename
					case "save_tradename":
						tradename_id = _q["id"] != null ? _q["id"] : "";
						var _tag_id = _q["tag_id"] != null ? _q["tag_id"] : "";
						var tradename_name = _q["tradename"] != null ? _q["tradename"] : "";
						if (tradename_name == "")
						{
							Response.Write("Tradename not supplied");
							Response.End();
						}
						var is_new = tradename_id == "" ? true : false;
						try
						{
							if (is_new)
							{
								if (_tag_id == "")
								{
									Response.Write("Tag ID not supplied");
									Response.End();
								}
								Toolbox.doSQL_void(@"INSERT INTO inventory_tag_tradename 
	(
	tag_id, 
	tradename
	) 
VALUES 
	(
	@v0, 
	@v1
	)", new object[] {
									_tag_id,	// {0}
									tradename_name		// {1}
									});
							}
							else
							{
								Toolbox.doSQL_void(@"
UPDATE 
	inventory_tag_tradename 
SET 
	tradename = @v0
WHERE 
	id = @v1 
LIMIT 1", new object[] {
									tradename_name,		// {0}
									tradename_id			// {1}
									});
							}
							Response.Write("SUCCESS");
						}
						catch (Exception ee)
						{
							Response.Write(ee.Message);
						}
						Response.End();
						break;
					#endregion save_tradename
					#region delete_tradename
					case "delete_tradename":
						tradename_id = _q["id"] != null ? _q["id"] : "";
						if (tradename_id != "")
						{
							try
							{
								Toolbox.doSQL_void(@"DELETE FROM inventory_tag_tradename WHERE id = @v0 LIMIT 1", tradename_id);
								Response.Write("SUCCESS");
							}
							catch (Exception ee)
							{
								Response.Write(ee.Message);
							}
						}
						else
						{
							Response.Write("Blank Tradename ID");
						}
						Response.End();
						break;
					#endregion delete_tradename
					#region delete
					case "delete":
						if (_q["attribute_value_id"] != null && _q["aid"] != null)
						{
							var attribute_value_id = _q["attribute_value_id"];
							aid = _q["aid"];
							try
							{
								inv_class.delete_value(attribute_value_id);
								Response.Redirect("./index.aspx?a=att&aid=" + aid);
							}
							catch (Exception ee)
							{
								Response.Write(@"
		<script>
			alert('There was an error deleting the requested value.');
			location.href('./index.aspx?a=att&aid=" + aid + @"');
		</script>
		");
								Response.End();

							}
						}
						else
						{
							Response.Redirect("./index.aspx?a=att");
						}
						break;
					#endregion delete
					#region unapprove_value
					case "unapprove_value":
						if (_q["attribute_value_id"] != null && _q["aid"] != null)
						{
							var attribute_value_id = _q["attribute_value_id"];
							aid = _q["aid"];
							if (inv_class.unapprove_value(attribute_value_id))
							{
								Response.Redirect("./index.aspx?a=att&aid=" + aid);
							}
							else
							{
								Response.Write(@"
		<script>
			alert('There was an error deleting the requested value. \nMost likely it is used in a prepopulated tag.');
			location.href('./index.aspx?a=att&aid=" + aid + @"');
		</script>
		");
								Response.End();
							}
						}
						else
						{
							Response.Redirect("./index.aspx?a=att");
						}
						break;
					#endregion unapprove_value
					#region delete_aid
					case "delete_aid":
						if (_q["attribute_id"] != null)
						{
							aid = _q["attribute_id"];
							if (inv_class.delete_attribute(aid))
							{
								Response.Redirect("./index.aspx?a=att");
							}
							else
							{
								Response.Write(@"
			<script>
				alert('There was an error deleting the requested value.');
				location.href('./index.aspx?a=att&aid=" + aid + @"');
			</script>
			");
								Response.End();
							}
						}
						else
						{
							Response.Redirect("./index.aspx?a=att");
						}
						break;
					#endregion delete_aid
					#region toggle_part
					case "toggle_part":
						_tools.dont_cache_page();
						Response.Clear();
						if (_q["master_id"] != null)
						{
							master_id = _q["master_id"];
							var current_state = Convert.ToBoolean(Toolbox.doSQL_int(@"SELECT IFNULL(active, 0) FROM inventory_item_master WHERE master_id =@v0 ", master_id));
							try
							{
								if (current_state)
								{
									inventory.SetPartInactive(master_id);
								}
								else
								{
									inventory.SetPartActive(master_id);
								}
								Response.Write("success");
							}
							catch(Exception ee)
							{
								Response.Write("failed");
							}
						}
						else
						{
							Response.Write("failed");
						}
						Response.End();
						break;
					#endregion toggle_part
					#region merge_part
					case "merge_part":
						_tools.dont_cache_page();
						Response.End();
						if (_q["from_master_id"] != null && _q["to_master_id"] != null)
						{
							var from_master_id = _q["from_master_id"];
							var to_master_id = _q["to_master_id"];
							if (from_master_id.Length > 0 && to_master_id.Length > 0)
							{
								inventory.merge_part(from_master_id, to_master_id);
								Response.Redirect("./index.aspx?a=edit_part&master_id=" + to_master_id);
							}
							else
							{
								Response.Redirect(Request.RawUrl);
							}
						}
						else
						{
							Response.Write("failed");
						}
						Response.End();
						break;
					#endregion
					#region delete_price
					case "delete_price":
						Response.Clear();
						_tools.dont_cache_page();
						Response.ContentType = "text/plain";
						if (_q["price_id"] != null && _q["price_id"] != "")
						{
							try
							{
								Toolbox.doSQL_void(@"DELETE FROM inventory_price WHERE ID = @v0 LIMIT 1", _q["price_id"]);
								//int count					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM SPECIAL_PRICING WHERE bvspecpricepartno = @v0  and bvspecpricecode = @v1 ", DSN , new object[] {  master_id, vendor_number } );
								//if(count != 0)
								//	{
								//Toolbox.doSQL_void(@"DELETE FROM SPECIAL_PRICING WHERE bvspecpricepartno = @v0  and bvspecpricecode = @v1 ", DSN , new object[] {  master_id, vendor_number } );
								//	}
								Response.Write("Success");
							}
							catch (Exception ee)
							{
								Response.Write("Error");
								_tools.catch_error(ee);
							}
						}
						else
						{
							Response.Write("Error");
						}
						Response.End();
						break;
					#endregion delete_price
					#region new_price
					case "new_price":
						Response.Clear();
						_tools.dont_cache_page();
						_tools.set_plain_header();
						master_id = _q["master_id"];
						business_unit_id = Convert.ToInt32(_q["business_unit_id"]);
						vendor_id = _q["vendor_id"];
						part_number = _q["part_number"];
						total_cost = _q["total_cost"];
						unit_cost = _q["unit_cost"];
						qty = _q["qty"];
						inv_class.price_new(master_id, business_unit_id, vendor_id, part_number, unit_cost, total_cost, qty);
						Response.End();
						break;
					#endregion new_price
					#region edit_price
					case "edit_price":
						Response.Clear();
						_tools.set_plain_header();
						_tools.dont_cache_page();
						price_id = _q["price_id"];
						master_id = _q["master_id"];
						business_unit_id = Convert.ToInt32(_q["business_unit_id"]);
						vendor_id = _q["vendor_id"];
						part_number = _q["part_number"];
						unit_cost = _q["unit_cost"].Replace(",", "");
						total_cost = _q["total_cost"].Replace(",", "");
						qty = _q["qty"].Replace(",", "");
						benchmark = _q["benchmark"];
						inv_class.price_edit(master_id, business_unit_id, vendor_id, part_number, unit_cost, total_cost, qty, price_id, benchmark);
						Response.End();
						break;
					#endregion edit_price
					#region approve
					case "approve":
						Response.Clear();
						_tools.set_plain_header();
						_tools.dont_cache_page();
						if (_q["type_of"] != null)
						{
							type_of = (string)_q["type_of"];
							if (_q["id"] != null)
							{
								this_id = _q["id"];
								if (inv_class.approve(this_id, (int)my_member.id, type_of))
								{
									Response.Write("SUCCESS");
								}
								else
								{
									Response.Write("FAILED inserting");
								}
							}
							else
							{
								Response.Write("FAILED approval");
							}
						}
						else
						{
							Response.Write("FAILED blank");
						}
						Response.End();
						break;
					#endregion approve
					#region newtag
					case "newtag":
						if (_q["tag"] != null && Regex.IsMatch(_q["tag"], @"(\w+)"))
						{
							var newtag = _q["tag"].Trim();
							TagID = inv_class.add_tag(newtag);
							if (TagID != "0")
							{
								Response.Redirect("./index.aspx?a=create_linkage&tid=" + TagID);
							}
							else
							{
								Response.Write("<script>alert('There was an error adding the supplied tag.\\n\\nPossible causes:\\n - duplicate tag\\n - invalid characters');location.href('./index.aspx?a=tag');</script>");
								Response.End();
							}
						}
						break;
					#endregion newtag
					#region handle_pic
					case "handle_pic":
						if (_q["id"] != null && _q["which"] != null && _q["master_id"] != null)
						{
							Response.Write(inv_class.handle_pic(_q["id"], _q["which"], _q["master_id"]));
						}
						else
						{
							Response.Redirect("./index.aspx");
						}
						break;
					#endregion handle_pic
					#region editatt
					case "editatt":
						if (_q["aid"] != null && _q["new_name"] != null)
						{
							aid = _q["aid"];
							var new_name = Toolbox.do_value_from(_q["new_name"], false);
							try
							{
								inv_class.edit_attribute(aid, new_name);
								Response.Redirect("./index.aspx?a=att&aid=" + aid);
							}
							catch
							{
								Response.Write("<script>alert('You cannot have attributes with the same name');location.href='./index.aspx?a=att&aid='+" + aid + ";</script>");
							}
						}
						else
						{
							Response.Redirect("./index.aspx?a=att");
						}
						break;
					#endregion editatt
					#region edit_tag
					case "edit_tag":
						Response.Clear();
						if (_q["tag_id"] != null && _q["new_name"] != null)
						{
							TagID = _q["tag_id"];
							var new_name = _q["new_name"];
							try
							{
								inv_class.edit_tag(TagID, new_name);
								Response.Write("SUCCESS");
							}
							catch
							{
								Response.Write("Could not post your edited tag to the database.");
							}
						}
						else
						{
							Response.Write("There was a problem retrieving values from submission.");
						}
						Response.End();
						break;
					#endregion edit_tag
					#region update_property
					case "update_property":
						Response.Clear();
						if (_q["tag_id"] != null && _q["val"] != null && _q["prop"] != null)
						{
							try
							{
								Toolbox.doSQL_void(string.Format("UPDATE inventory_tag SET {2} = {0} WHERE tag_id = {1}", _q["val"], _q["tag_id"], _q["prop"]));
								Response.Write("SUCCESS");
							}
							catch
							{
								Response.Write(string.Format("Could not update the '{0}' field for this tag", _q["prop"]));
							}
						}
						else
						{
							Response.Write("There was a problem validating your submission.");
						}
						Response.End();
						break;
					#endregion update_property
					#region editpart
					case "editpart":
						Response.Clear();
						_tools.dont_cache_page();
						_tools.set_plain_header();
						if (_q["tag_id"] != null &&
								_q["n_attributes"] != null)
						{
							var n_attributes = Convert.ToInt32(_q["n_attributes"]);
							master_id = _q["master_id"];
							var av_id = new string[100];
							if (inv_class.clear_part_detail(master_id))
							{
								for (var i = 0; i < n_attributes; i++)
								{
									av_id[i] = _q["AV" + i];
									if (!inv_class.insert_part_detail(master_id, av_id[i]))
									{
										try
										{
											inv_class.roll_back_part(master_id);
											Response.Write("DETAIL FAILED");
											Response.End();
										}
										catch
										{
											Response.Write("CANT ROLLBACK");
											Response.End();
										}
									}
								}
							}
							else
							{
								Response.Write("CANT CLEAR DETAILS");
								Response.End();
							}

							
							Toolbox.doSQL_void(@"UPDATE inventory_description SET 
	description = FULL_PART_DESCRIPTION(master_id, false, ''), 
	desc_short_cdn = PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_short_usa = PART_DESCRIPTION(master_id, false, 'USA'), 
	desc_full_cdn = FULL_PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_full_usa = FULL_PART_DESCRIPTION(master_id, false, 'USA') WHERE master_id = @v0", master_id);
							Response.Write("SUCCESS");
							Response.End();
						}
						else
						{
							Response.Write("INVALID REQUEST");
							Response.End();
						}
						break;
					#endregion editpart
					#region qc
					case "qc":
						page_title.InnerText = "Quality Check Home";

						break;
					#endregion qc
					#region save_split
					case "save_split":
						try
						{
							var ss_vendor_code = _q["ss_vendor_code"];
							var ss_qty_per = Convert.ToDouble(_q["ss_qty_per"]);
							var ss_cost = Convert.ToDouble(_q["ss_cost"]);
							var ss_row_id = Convert.ToInt32(_q["ss_row_id"]);
							var ss_tag_id = Convert.ToInt32(_q["ss_tag_id"]);
							var ss_business_unit_id = Convert.ToInt32(_q["ss_business_unit_id"]);
							var ss_vendor_id = Convert.ToInt32(_q["ss_vendor_id"]);
							var ss_master_id = Convert.ToInt32(_q["ss_master_id"]);
							var ss_link = _q["ss_link"].Split(',');
							var ss_attvals = new List<int>();
							var vpr = new vendor_price_row();
							var old_vpr = new vendor_price_row(ss_row_id);
							for (var i = 0; i < ss_link.Length; i++)
							{
								ss_attvals.Add(Convert.ToInt32(ss_link[i]));
							}
							ss_attvals.Sort();
							inventory.Load(ss_master_id, ss_business_unit_id);
							var ss_company = new NeBusinessUnit(ss_business_unit_id);
							var tag_pattern = string.Join(",", ss_attvals.ToArray());
							_dt = match_parts(ss_tag_id, tag_pattern, ss_company.country, ss_company.id);
							if (_dt.Rows.Count > 0)
							{
								// Existing Item (should only be one, check it though)
								if (_dt.Rows.Count > 1)
								{
									output = "This shouldn't happen, but there are " + _dt.Rows.Count + " parts matching the supplied attributes/values";
								}
								else
								{
									// There is only one part matching.. Load it
									var dr = _dt.Rows[0];
									var ret_master_id = dr["item"].ToString();
									var ret_part = new inventory();
									ret_part.Load(ret_master_id, ss_business_unit_id);
									// Is this the same master ID that was supplied?
									if (ss_master_id.ToString() == ret_master_id) // yes, Basically just an update at this point
									{
										vpr = new vendor_price_row(_q["ss_row_id"]);
										vpr.qty = ss_qty_per;
										vpr.cost = ss_cost;
										vpr.total = ss_qty_per * ss_cost;
										vpr.vendor_code = ss_vendor_code;
										vpr.origin = "Admin Split Parts Page";
										vpr.save(true);
										output = "SUCCESS";
									}
									else // no -- use ret_master_id
									{
										// Save submitted data
										vpr = new vendor_price_row(ss_row_id);
										vpr.qty = ss_qty_per;
										vpr.vendor_code = ss_vendor_code;
										vpr.cost = ss_cost;
										vpr.save();
										output = "SUCCESS";
										// Need to delete the originating vendor row
									}

								}
							}
							else
							{
								// New Item
								var new_id = inventory.NewPart(ss_tag_id.ToString(), tag_pattern, my_member.id.ToString(), true);
								// Approve Item
								inv_class.approve(new_id, Convert.ToInt32(my_member.id), "part");
								// Add Vendor Info
								inventory.Load(new_id, ss_business_unit_id);
								Toolbox.doSQL_void(@"
UPDATE 
	inventory_price 
SET 
	master_id = @v0, 
	member_id = @v1, 
	note = CONCAT(IFNULL(note, ''), 'Moved from ',@v2)  
WHERE 
	master_id=@v2 AND 
	vendor_id = @v3 AND 
	vendor_code = @v4", new object[] {
										new_id,					// {0}
										my_member.id,			// {1}
										ss_master_id,			// {2}
										ss_vendor_id,			// {3}
										old_vpr.vendor_code		// {4}
										});

								vpr = new vendor_price_row(ss_row_id);
								vpr.qty = ss_qty_per;
								vpr.vendor_code = ss_vendor_code;
								vpr.cost = ss_cost;
								vpr.save();
								output = "SUCCESS";
							}
						}
						catch (Exception ee)
						{
							output = ee.Message;
						}
						Response.Write(output);
						Response.End();
						break;
					#endregion save_split
					#region save_move
					case "save_move":
						var f_row_id = Convert.ToInt32(_q["f_row_id"]);
						var f_master_id = Convert.ToInt32(_q["f_master_id"]);
						var f_vendor_id = Convert.ToInt32(_q["f_vendor_id"]);
						var f_business_unit_id = Convert.ToInt32(_q["f_business_unit_id"]);

						var t_row_id = Convert.ToInt32(_q["t_row_id"]);
						var t_master_id = Convert.ToInt32(_q["t_master_id"]);
						var t_vendor_id = Convert.ToInt32(_q["t_vendor_id"]);
						var t_business_unit_id = Convert.ToInt32(_q["t_business_unit_id"]);
						//poprog_status NOT IN (9,8,7,6,5,4) AND 
						var n_pos = Toolbox.doSQL_int(@"
SELECT 
	COUNT(poprog_id)
FROM 
	poprog_header 
WHERE 
	poprog_vendor_id IN (@v0, @v1) AND 
	business_unit_id IN (@v2, @v3) AND 
	poprog_id IN
		(
		SELECT 
			po_details_poprog_id 
		FROM 
			po_details_current 
		WHERE 
			po_details_part_no IN (@v4, @v5)
		)", new object[] {
								f_vendor_id,
								t_vendor_id,
								f_business_unit_id,
								t_business_unit_id,
								f_master_id,
								t_master_id
								});
						//b.status NOT IN ('Verified', 'Closed') AND
						var n_rfqs = Toolbox.doSQL_int(@"
SELECT 
	COUNT(a.id) 
FROM 
	rfq_part_list a 
LEFT JOIN 
	rfq_header b ON 
		a.rfq_header_id = b.id 
WHERE 
	a.master_id IN (@v0, @v1) AND 
	b.business_unit_id IN (@v2,@v3) AND 
	(SELECT COUNT(*) FROM rfq_vendor_list WHERE rfq_header_id = b.id AND vendor_id IN (@v4,@v5)) > 0", new object[] {
								f_master_id,
								t_master_id,
								f_business_unit_id,
								t_business_unit_id,
								f_vendor_id,
								t_vendor_id
								});

						var cant_move_reasons = new List<string>();
						if (f_master_id == t_master_id)
						{
							cant_move_reasons.Add("Master ID's are the same");
						}
						if (n_pos > 0)
						{
							cant_move_reasons.Add("Vendor row exists on open or past PO's.");
						}
						if (n_rfqs > 0)
						{
							cant_move_reasons.Add("Vendor row exists on open or past RFQ's.");
						}
						if (cant_move_reasons.Count > 0)
						{
							output += "Error, Can't Transfer:\n";
							foreach (var r in cant_move_reasons)
							{
								output += "- " + r + "\n";
							}
						}
						else
						{
							var vpr = new vendor_price_row(f_row_id);
							// Move all existing combinations over
							Toolbox.doSQL_void(@"
UPDATE 
	inventory_price 
SET 
	master_id = @v0, 
	member_id = @v1, 
	note = CONCAT(IFNULL(note, ''), 'Moved from ',@v2) 
WHERE 
	master_id = @v2 AND 
	vendor_code = @v3 AND 
	vendor_id = @v4", new object[] {
								t_master_id,		// {0}
								my_member.id,		// {1}
								f_master_id,		// {2}
								vpr.vendor_code,	// {3}
								f_vendor_id			// {4}
								});
							output = "SUCCESS";
						}
						Response.Write(output);
						Response.End();
						break;
					#endregion save_move
					#region save_markup
					case "save_markup":
						TagID = _q["tag_id"];
						double markup_amount = 0;
						var markup_isfixed = _q["is_fixed"].ToLower() == "true";
						double.TryParse(_q["markup_amount"], out markup_amount);
						Toolbox.doSQL_void(@"UPDATE inventory_tag SET is_fixedmarkup = @v1, markup_amount = @v2 WHERE tag_id = @v0", new object[] {
							TagID, markup_isfixed,  markup_amount});
						Toolbox.QuickReponse(Response, "SUCCESS");
						break;
						#endregion save_markup
						#endregion GET/SET Handlers
				}
				if (output != null)
				{
					detail.InnerHtml = output;
				}
				else
				{
					Response.End();
				}
				break;
				#endregion GET
		}
	}
	#endregion Page_Load
	private string page_edit_linkage(int _id)
	{
		var tag_name = "";
		var sb = new StringBuilder();
		var tag = new inventory.tag(_id);
		var can_view_protected_part = hasProtectedTagPropertyAccess ? "" : "Disabled";
		using (var conn = Toolbox.connect())
		{
			var counter = 0;
			var dt = new DataTable();
			long ts = 0;
			try
			{
				tag_name = tag.name;
				ts = tag.ts.Ticks;
				dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_tag_link WHERE tag_id = @v0  ORDER BY order_id", new object[] { _id });
			}
			catch
			{
				Response.Write("<script>alert('Invalid Tag ID');location.href('./index.aspx?a=tag');</script>");
				Response.End();
			}
			var disabler = "";
			var dependants = Toolbox.doSQL_int(conn, "SELECT IFNULL(COUNT(*), 0) C FROM inventory_tag_link WHERE tag_id = @v0 ", new object[] { _id });
			if (dependants > 0)
			{
				disabler = "disabled";
			}
			var total_attributes = Toolbox.doSQL_int(conn, "SELECT IFNULL(COUNT(*), 0) C FROM inventory_tag_link WHERE tag_id =@v0 ", new object[] { _id });
			var options = new NVC();


			#region _options["canadian_sold_as"]
			var canadian_sold_as_id = Toolbox.doSQL_string(conn, "SELECT canadian_sold_as FROM inventory_tag WHERE tag_id = " + _id, null);
			var canadian_sold_as_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_sold_as", null);
			options["canadian_sold_as"] = "";
			foreach (DataRow dr in canadian_sold_as_dt.Rows)
			{
				var temp_id = dr["id"].ToString();
				var temp_unit = dr["unit"].ToString();
				var temp_selected = "";
				if (temp_id == canadian_sold_as_id)
				{
					temp_selected = " selected";
				}
				else
				{
					temp_selected = "";
				}
				options["canadian_sold_as"] += "<option value='" + temp_id + "' " + temp_selected + ">[CAN] SOLD AS: per " + temp_unit + "</option>";
			}
			#endregion _options["canadian_sold_as"]	
			#region _options["exclude"]
			var is_exclude = Convert.ToBoolean(Toolbox.doSQL_int(conn,
				@"SELECT IFNULL(is_exclude, 0) exclude FROM inventory_tag WHERE tag_id =@v0 ", new object[] { _id }));
			options["is_exclude"] = "";
			if (is_exclude)
			{
				options["is_exclude"] = "<option value='0'>Exclude: NO</option><option value='1' selected>Exclude: YES</option>";
			}
			else
			{
				options["is_exclude"] = "<option value='0' selected>Exclude: NO</option><option value='1'>Exclude: YES</option>";
			}
			#endregion _options["is_exclude"]	
			#region _options["usa_sold_as"]
			var usa_sold_as_id = Toolbox.doSQL_string(conn, "SELECT usa_sold_as FROM inventory_tag WHERE tag_id = " + _id, null);
			var usa_sold_as_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_sold_as", null);
			options["usa_sold_as"] = "";
			foreach (DataRow dr in usa_sold_as_dt.Rows)
			{
				var temp_id = dr["id"].ToString();
				var temp_unit = dr["unit"].ToString();
				var temp_selected = "";
				if (temp_id == usa_sold_as_id)
				{
					temp_selected = " selected";
				}
				else
				{
					temp_selected = "";
				}
				options["usa_sold_as"] += "<option value='" + temp_id + "' " + temp_selected + ">[USA] SOLD AS: per " + temp_unit + "</option>";
			}
			#endregion _options["usa_sold_as"]	
			#region _options["allowed_to_stock"]
			var allowed_to_stock = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT IFNULL(allowed_to_stock, 0) allowed_to_stock 
FROM inventory_tag WHERE tag_id = " + _id, null));
			options["allowed_to_stock"] = allowed_to_stock
														? "<option value='0'>Allowed to Stock: NO</option><option value='1' selected>Allowed to Stock: YES</option>"
														: "<option value='0' selected>Allowed to Stock: NO</option><option value='1'>Allowed to Stock: YES</option>";
			#endregion _options["allowed_to_stock"]	
			#region _options["expiry"]
			var period_id = Toolbox.doSQL_string(conn, "SELECT expiry_period FROM inventory_tag WHERE tag_id = @v0 ", new object[] { _id });
			var period_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_expiry_period ORDER BY length", null);
			options["expiry"] = "";
			if (period_id == "0")
			{
				options["expiry"] += "<option value='0' SELECTED>N/A</option>";
			}
			foreach (DataRow dr in period_dt.Rows)
			{
				var temp_id = dr["id"].ToString();
				var temp_unit = dr["name"].ToString();
				var temp_selected = "";
				if (temp_id == period_id && period_id != "0")
				{
					temp_selected = " selected";
				}
				else
				{
					temp_selected = "";
				}
				options["expiry"] += "<option value='" + temp_id + "' " + temp_selected + ">Price Expires After: " + temp_unit + "</option>";
			}
			#endregion _options["expiry"]	
			#region _options["is_qty"]
			string[] is_qty_str = { "Parts in this tag are NORMAL parts", "Parts in this tag are QTY parts" };
			var is_qty = Convert.ToBoolean(Toolbox.doSQL_int(conn,
				@"SELECT is_qty FROM inventory_tag WHERE tag_id = @v0", new object[] { _id }));
			for (var i = 1; i <= is_qty_str.Length; i++)
			{
				var is_qty_selected = "";
				if (is_qty && i - 1 == 1)
				{
					is_qty_selected = " selected";
				}
				else
				{
					is_qty_selected = "";
				}
				options["is_qty"] += string.Format("<option value='{0}'{2}>{1}</option>", i - 1, is_qty_str[i - 1], is_qty_selected);
			}
			#endregion _options["is_qty"]
			#region _options["is_consumable"]
			var is_consumable = Toolbox.doSQL_int(conn, "SELECT is_consumable FROM inventory_tag WHERE tag_id = " + _id, null) == 1;
			options["is_consumable"] = is_consumable
													? "<option value='0'>Is Consumable: NO</option><option value='1' selected>Is Consumable: YES</option>"
													: "<option value='0' selected>Is Consumable: NO</option><option value='1'>Is Consumable: YES</option>";
			#endregion _options["is_consumable"]
			#region _options["is_commodity"]
			var is_commodity = Toolbox.doSQL_bool(conn, "SELECT is_commodity FROM inventory_tag WHERE tag_id = " + _id, null);
			options["is_commodity"] = is_commodity
													? "<option value='0'>Is Commodity: NO</option><option value='1' selected>Is Commodity: YES</option>"
													: "<option value='0' selected>Is Commodity: NO</option><option value='1'>Is Commodity: YES</option>";
			#endregion _options["is_commodity"]
			#region _options["tax[1-3]"]
			var tax_1 = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT tax_1 FROM inventory_tag WHERE tag_id =@v0", new object[] { _id }));
			var tax_2 = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT tax_2 FROM inventory_tag WHERE tag_id =@v0", new object[] { _id }));
			var tax_3 = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT tax_3 FROM inventory_tag WHERE tag_id =@v0", new object[] { _id }));
			if (tax_1)
			{
				options["tax_1"] = "<option value='0'>Tax 1 Not Applicable</option><option value='1' selected>Tax 1 Applicable</option>";
			}
			else
			{
				options["tax_1"] = "<option value='0' selected>Tax 1 Not Applicable</option><option value='1'>Tax 1 Applicable</option>";
			}
			if (tax_2)
			{
				options["tax_2"] = "<option value='0'>Tax 2 Not Applicable</option><option value='1' selected>Tax 2 Applicable</option>";
			}
			else
			{
				options["tax_2"] = "<option value='0' selected>Tax 2 Not Applicable</option><option value='1'>Tax 2 Applicable</option>";
			}
			if (tax_3)
			{
				options["tax_3"] = "<option value='0'>Tax 3 Not Applicable</option><option value='1' selected>Tax 3 Applicable</option>";
			}
			else
			{
				options["tax_3"] = "<option value='0' selected>Tax 3 Not Applicable</option><option value='1'>Tax 3 Applicable</option>";
			}
			#endregion _options["tax[1-3]"]
			#region _options["allowed_to_edit_after_issue"]
			var allowed_to_edit_after_issue = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT IFNULL(allowed_to_edit_after_issue, 0) exclude
FROM inventory_tag WHERE tag_id = @v0", new object[] { _id }));
			options["allowed_to_edit_after_issue"] = "";
			if (allowed_to_edit_after_issue)
			{
				options["allowed_to_edit_after_issue"] = "<option value='0'>Allowed to edit on PO after Issuing?: NO</option><option value='1' selected>Allowed to edit on PO after Issuing?: YES</option>";
			}
			else
			{
				options["allowed_to_edit_after_issue"] = "<option value='0' selected>Allowed to edit on PO after Issuing?: NO</option><option value='1'>Allowed to edit on PO after Issuing?: YES</option>";
			}
			#endregion _options["allowed_to_edit_after_issue"]	
			#region Output
			var markup_checked = tag.is_fixedmarkup ? " CHECKED" : "";
			var markup_disabler = tag.is_fixedmarkup ? "" : " DISABLED";
			
			var fixed_markup = my_member.id == 8 || my_member.id == 711 || my_member.id == 1005 || my_member.id == 14
										? string.Format(@"
								<tr>
									<td class='td fixed_markup'>
										<div><label for='c_fixed_markup'>Fixed Markup?</label><input type='checkbox' onclick='markup_handler(this)' id='c_fixed_markup' {0}/></div>
										<div><input type='text' id='t_fixed_markup' onkeydown='only_numeric(event)' value='{2}' class='markup' {1}/></div>
										<div><button type='button' id='b_fixed_markup' class='save' onclick='save_markup(this)' ><img src='/images/icon/icon[save].gif' align='absmiddle'/> Save</button></div>
									</td>
								</tr>",
				markup_checked,             // {0}
				markup_disabler,            // {1}
				tag.markup_amount   // {2}
				)
										: "";
			
			sb.AppendFormat(@"
		<center>
			<div id='tag_properties'>
				<table cellpadding='0' cellspacing='0' width='100%' class='table'>
					<tr>
						<td class='left'><img src='/images/inventory/filler/filler[left].png' style='display:none' height='100% '/></td>
						<td valign='top'>
							<table cellpadding='0' cellspacing='0' width='100%' class='bordered'>
								<tr>
									<td class='td'><select data-type='is_commodity' onchange='update_tag_properties(this)'>
											{13}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='is_exclude'  {19} onchange='update_tag_properties(this)'>
											{12}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='canadian_sold_as' onchange='update_tag_properties(this)'>
											{4}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='usa_sold_as' onchange='update_tag_properties(this)'>
											{11}
										</select></td>
								</tr>				
								<tr>
									<td class='td'><select data-type='expiry_period' onchange='update_tag_properties(this)'>
											{9}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='gl_id' onchange='update_tag_properties(this)'>
											{10}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='allowed_to_stock' {19} onchange='update_tag_properties(this)'>
											{14}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='allowed_to_edit_after_issue' onchange='update_tag_properties(this)'>
											{16}
										</select></td>
								</tr>
								<tr>
									<td class='td'><select data-type='is_consumable' onchange='update_tag_properties(this)'>
											{18}
										</select></td>
								</tr>
								{15}
							</table>
							<button type='button' class='push' id='tag_properties_push' onclick='handle_properties_toggle()'><b>vv</b> TAG PROPERTIES <b>vv</b></button>
						</td>
						<td class='right'><img src='/images/inventory/filler/filler[right].png' height='100%' style='display:none' /></td>
					</tr>
				</table>
			</div>
		</center>
		<input type='hidden' name='action' value='edit linkage'>
		<table cellpadding='0' cellspacing='0' class='head' width='100%'>
			<tr>
				<td width='150' style='height:25px;' valign='middle' align='center'>
					<button id='add_row' type='button' onclick='add_attribute_row();' title='Add Attribute Row'><img src='/images/icon/icon[add].gif' align='absmiddle' /> ADD ATTRIBUTE</button><br />
					<input type='hidden' name='tag_id' value='{0}' id='tag_id'/>
					<input type='hidden' name='n_descriptors' value='0' id='n_descriptors'/>
					<input type='hidden' name='n_attributes' value='{3}' id='n_attributes'/>
					<input type='hidden' name='ts' value='{17}' id='ts' />
				</td>
				<td align='left' valign='bottom'>{1}</td>
				<td width='45%' align='right' valign='bottom'>
					<button class='tab' type='button' id='tab_tag' type='button' disabled>Tag Makeup</button>
					<button class='tab' type='button' id='tab_tradenames' type='button'>Trade Names</button>
				</td>
			</tr>
		</table>
		<table cellpadding='0' cellspacing='0' class='newitem'>
			<tr>
				<td colspan='3' class='val' valign='top'>
				<div id='trade_names' align='center'>
					<table cellpadding='0' cellspacing='0' width='350'>
						<thead>
							<tr>
								<td style='width:85%;background-color:#def;'><input type='text'  onkeydown=""if(checkEnter()){{$(this).parents('tr:first').find('button:first').click();}}"" class='tradename' style='width:99%'/></td>
								<td style='width:15%;background-color:#def;' colspan='2'><button type='button' data-id='' onclick='save_tradename(this)' style='width:99%'><img src='/images/icon/icon[save].gif' /></button></td>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
				</div>
				<div id='tag_defaults'>
					<div id='attributes'>",
_id,                                        // {0}
tag_name,                                   // {1}
disabler,                                   // {2}
total_attributes,                           // {3}
options["canadian_sold_as"],                // {4}
options["is_qty"],                          // {5}
options["tax_1"],                           // {6}
options["tax_2"],                           // {7}
options["tax_3"],                           // {8}
options["expiry"],                          // {9}
options["revenue"],                     // {10}
options["usa_sold_as"],                 // {11}
options["is_exclude"],                      // {12}
options["is_commodity"],                       // {13}
options["allowed_to_stock"],                // {14}
fixed_markup,                               // {15}
options["allowed_to_edit_after_issue"], // {16}
ts,                                         // {17}
options["is_consumable"] ,               // {18}
can_view_protected_part // {19}
);
			#endregion Output
			var tag_approved = Toolbox.doSQL_int(conn, @"SELECT approved FROM inventory_tag WHERE tag_id = @v0", new object[] { _id });
			if (dt.Rows.Count > 0)
			{
				var linkage_attributes = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_attribute ORDER BY attribute", null);
				foreach (DataRow dr in dt.Rows)
				{

					var row_type = "";
					var attribute_id = dr["attribute_id"].ToString();
					var order_id = dr["order_id"].ToString();
					var tag_attribute = attribute_id;
					var linkage_values = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  AND active = true ORDER BY value", new object[] { attribute_id });
					var this_check = "";
					var this_visibile = "";
					var this_enabled = "";
					if (counter % 2 == 0)
					{
						row_type = "_even";
					}
					else
					{
						row_type = "_odd";
					}
					if (order_id != "99" && order_id != "NULL" && order_id != "")
					{
						this_check = " checked";
						this_enabled = "";
					}
					else
					{
						this_check = "";
						this_visibile = " style='display:none;'";

						if (Toolbox.doSQL_int(conn, @"SELECT count(*) c FROM inventory_tag_link WHERE order_id != 99 AND tag_id =@v0", new object[] { _id }) < total_attributes)
						{
							this_enabled = "";
						}
						else
						{
							this_enabled = " disabled";
						}
					}

					var n_descriptors = total_attributes;
					//	
					var n_occ_sql = string.Format(@"
SELECT 
	(
	SELECT 
		COUNT(DISTINCT(id.attribute_value_id)) 
	FROM 
		inventory_item_detail id 
	LEFT JOIN
		inventory_item_master im ON im.master_id = id.master_id
	LEFT JOIN
		inventory_attribute_value iav on id.attribute_value_id = iav.attribute_value_id
	WHERE 
		im.tag_id = a.tag_id AND 
		iav.attribute_id = a.attribute_id
	) 'distinct occurances' 
FROM 
	inventory_tag_link a
where 
a.tag_id ={0} and a.attribute_id = {1}
	", _id, attribute_id);
					var n_occurances = Toolbox.doSQL_int(conn, n_occ_sql, null);
					//	
					sb.AppendFormat(@"
							<div class='attribute_row{1}' id='div_{0}'>
							<span class='itemize_row'>
								<input type='checkbox' value ='{0}' class='descriptor' onclick='if(this.checked == 1){{descriptors(1, {0});}}else{{descriptors(0, {0});}}'{2}{4}>
								<select id='order_{0}'  name='order_{0}' onchange='descriptor_order_chk(this)' class='n_order'{3}>", counter, row_type, this_check, this_visibile, this_enabled);
					for (var i = 0; i <= 10; i++)
					{
						var this_selected = "";
						var this_text = "";
						if (i == Convert.ToInt32(order_id))
						{
							this_selected = " selected";
						}
						this_text = i == 0 ? "&nbsp;" : i.ToString();
						sb.AppendFormat(@"
									<option value='{0}'{1}>{2}</option>", i, this_selected, this_text);
					}

					var remove_span = "";
					if (n_occurances <= 1)
					{
						remove_span = "<span class='remove_row'><img onclick='remove_certain_attribute_row(this);' src='/images/inventory/button/button[remove].png' /></span>";
					}
					else
					{
						var master_ids = Toolbox.doSQL_dt(conn, @"SELECT master_id FROM inventory_item_master a where a.tag_id = @v0  and a.master_id in (select master_id from inventory_item_detail where associated_attribute(attribute_value_id) = @v1 )", new object[] { _id, attribute_id });
						var master_id_selector = "<select style='font-size:11px;font-family:arial;' onchange=\"if($(this).val() != 0){location.href='./index.aspx?a=edit_part&master_id='+$(this).val();}\"><option value='0'>Parts using this attribute, with different values</option>";
						foreach (DataRow occur in master_ids.Rows)
						{
							master_id_selector += "<option value='" + occur["master_id"] + "'>" + occur["master_id"];
						}
						master_id_selector += "</select>";
						remove_span = string.Format("<span class='remove_row'>{0}</span>", master_id_selector);
					}
					sb.AppendFormat(@"
								</select>
							</span>
							{3}
								<table id='table_{0}' cellpadding='0' cellspacing='0' width='100%'>
									<tr>
										<td colspan='3'>
											<select class='attribute' onchange=""if(this.value != 0){{populate_selects(this.id, this.value);}} else {{document.getElementById('value_{0}').disabled=true;document.getElementById('value_{0}').selectedIndex = 0;}}"" disabled>
												<option value='0'>SELECT ATTRIBUTE</option>", counter, row_type, n_occurances, remove_span);

					if (linkage_attributes.Rows.Count > 0)
					{
						foreach (DataRow linkage_attribute in linkage_attributes.Rows)
						{
							attribute_id = null;
							attribute_id = linkage_attribute["attribute_id"].ToString();
							var attribute_name = linkage_attribute["attribute"].ToString();
							if (tag_attribute == attribute_id)
							{
								var selected_attribute = " selected";
								sb.Append("<option value='" + attribute_id + "'" + selected_attribute + ">" + attribute_name);
							}
						}
					}
					sb.AppendFormat(@"
											</select>
											<input type='hidden' name='attribute_{0}' id='attribute_{0}' value='{1}'>
										</td>", counter, tag_attribute);

					var n_values = Toolbox.doSQL_int(conn, @"SELECT IFNULL(COUNT(attribute_value_id), 0) howmany
FROM inventory_attribute_value WHERE attribute_id = @v0 AND active = true", new object[] { tag_attribute });
					sb.AppendFormat(@"
									</tr>
									<tr class='cat_head'>
										<td>Available Values</td>
										<td width='50'>&nbsp;</td>
										<td>Selected Values</td>
									</tr>
									<tr>
										<td align='center'>
											<select name='available_{0}' id='available_{0}' class='preselected' multiple>", counter);

					var available_vals = Toolbox.doSQL_dt(conn, @"SELECT attribute_value_id, value FROM inventory_attribute_value WHERE attribute_id = @v1  AND active = true AND attribute_value_id NOT IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) ORDER BY value", new object[] { _id, tag_attribute });

					var list = new List<DataRow>(available_vals.Select());
					available_vals.Dispose();

					for (var rr = 0; rr < list.Count; rr++)
					{
						sb.Append("<option value='" + list[rr]["attribute_value_id"] + "'>" + list[rr]["value"]);
					}

					sb.AppendFormat(@"
											</select>
										</td>
										<td align='center' width='50'><button class='action' type='button' onclick='preselect_push(this);'>add &gt;</button><br/><button type='button' class='action' onclick='preselect_pull(this);'>&lt; delete</button></td>
										<td align='center'>
											<select name='selected_{0}' id='selected_{0}' class='preselected'  multiple>", counter);
					var selected_vals = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_attribute_value WHERE active = true and attribute_value_id in (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) ORDER BY value", new object[] { _id, tag_attribute });

					if (selected_vals.Rows.Count > 0)
					{
						foreach (DataRow selected_val in selected_vals.Rows)
						{
							sb.Append("<option value='" + selected_val["attribute_value_id"] + "'>" + selected_val["value"]);
						}
					}

					sb.Append(@"
											</select>
										</td>
									</tr>
								</table>
							</div>");
					counter++;
				}
			}
			var approve_button = "";
			if (tag_approved == 1)
			{
				approve_button = "<button type='button' disabled>Approve</button>";
			}
			else
			{
				approve_button = my_member.AuthenticatedForPrivilege(43)
									? string.Format(@"<button type='button' onclick='approve({0},""tag"", ""edit_linkage"");' id='approve{0}'>Approve</button>", _id)
									: "<button type='button' disabled>Approve</button>";
			}
			sb.AppendFormat(@"
					</div>
				</div>
				</td>
			</tr>
			<tr>
				<td class='submit' colspan='3'>
					<button type='button' id='save_tag' onclick='select_selected();' disabled>Save</button>
					<button type='button' onclick='location.href=""./index.aspx?a=tag"";'>Cancel</button>
					{2}
					<input type='hidden' value='{1}' id='HasDependants' />
					<input type='hidden' value='{0}' id='AttributeThreshold'/>
					<input type='hidden' name='n_AttributeRows' value='{0}' size='1' id='n_AttributeRows'/>
				</td>
			</tr>
		</table>", counter, dependants, approve_button);
		}
		return sb.ToString();
	}
	protected bool is_int(object _i)
	{
		var i_out = 0;
		return int.TryParse(_i.ToString(), out i_out);
	}
	public DataTable match_parts(object _tag_id, object _attval_pattern, string _country, int _business_unit_id)
	{
		var pattern = _attval_pattern.ToString();
		return Toolbox.doSQL_dt(@"CALL MATCH_PARTS(@v0 , @v1, @v2 , 200, 1)", new object[] { _tag_id, pattern, _business_unit_id });
	}
	protected void gv_splitparts_HtmlDataCellPrepared(object _sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		if (_e.DataColumn.Name == "open_pos")
		{
			var master_id = gv.GetDataRow(_e.VisibleIndex)["master_id"].ToString();
			var business_unit_id = gv.GetDataRow(_e.VisibleIndex)["business_unit_id"].ToString();
			var vendor_id = gv.GetDataRow(_e.VisibleIndex)["vendor_id"].ToString();
			var dt = Toolbox.doSQL_dt(@"SELECT poprog_id, poprog_bvpo FROM poprog_header  WHERE poprog_status NOT IN (9,8,7,6,5,4) AND poprog_vendor_id =@v0 AND business_unit_id =@v1  AND poprog_id IN (SELECT po_details_poprog_id FROM po_details_current WHERE po_details_part_no =@v2 ) ", new object[] { vendor_id, business_unit_id, master_id });
			var sb = new StringBuilder();
			foreach (DataRow dr in dt.Rows)
			{
				sb.Append("<a href='javascript:alert(" + dr["poprog_id"] + ");'>" + dr["poprog_bvpo"] + "</a>, ");
			}
			_e.Cell.Text = sb.ToString().TrimEnd(' ').TrimEnd(',');
		}
		if (_e.DataColumn.Name.Contains("att_"))
		{
			var master_id = gv.GetDataRow(_e.VisibleIndex)["master_id"].ToString();
			var tag_id = combo_selecttag.Value != null ? combo_selecttag.Value.ToString() : "";
			var attribute_id = _e.DataColumn.Name.Substring(4, _e.DataColumn.Name.Length - 4);
			if (!string.IsNullOrEmpty(master_id) && !string.IsNullOrEmpty(tag_id))
			{
				var value_id = "";
				try
				{
					value_id = Toolbox.doSQL_string(@"SELECT a.attribute_value_id
FROM inventory_item_detail a WHERE a.master_id =@v0  AND ASSOCIATED_ATTRIBUTE(attribute_value_id) = @v1 LIMIT 1", new object[] { master_id, attribute_id });
					var gvc = (GridViewDataColumn)gv.Columns[_e.DataColumn.Index];
					var cb = (DropDownList)gv.FindRowCellTemplateControl(_e.VisibleIndex, gvc, "attribute");
					//ASPxComboBox cb			= (ASPxComboBox) gv.FindRowCellTemplateControl(e.VisibleIndex, gvc, "attribute");
					cb.Items.FindByValue(value_id).Selected = true;
					//foreach(ListEditItem li in cb.Items)
					//	{
					//	li.Value			= li.Value;
					//	}
				}
				catch
				{
				}
			}
		}
	}
	protected void gv_splitparts_DataBinding(object _sender, EventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		var tag_id = combo_selecttag.Value != null ? Convert.ToInt32(combo_selecttag.Value) : 0;
		if (tag_id > 0)
		{
			// Get the attributes
			var atts = Toolbox.doSQL_dt(@"SELECT a.attribute_id id, b.attribute name FROM inventory_tag_link a LEFT JOIN inventory_attribute b ON a.attribute_id = b.attribute_id  WHERE a.tag_id =@v0", new object[] { tag_id });
			foreach (DataRow att in atts.Rows)
			{
				var c = new GridViewDataTextColumn();
				c.Caption = Toolbox.do_value_from(att["name"]);
				c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
				c.Name = "att_" + att["id"];
				c.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
				c.DataItemTemplate = new attribute_template(att["id"], tag_id);
				c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
				c.Width = Unit.Pixel(100);
				gv.Columns.Add(c);
			}
			var ca = new GridViewDataTextColumn();
			ca.Caption = "Actions";
			ca.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			ca.Name = "Actions";
			ca.CellStyle.CssClass = "actions";
			ca.DataItemTemplate = new action_template();
			ca.CellStyle.HorizontalAlign = HorizontalAlign.Center;
			ca.CellStyle.BackColor = System.Drawing.Color.White;
			ca.Width = Unit.Pixel(100);
			gv.Columns.Add(ca);
		}
	}
	protected void gv_splitparts_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		if (_e.Parameters != "" && _e.Parameters != "reload" && gv.ID == "gv_splitparts")
		{
			gv.LoadClientLayout(_e.Parameters);
		}
		else if (_e.Parameters != "" && _e.Parameters == "reload" && gv.ID == "gv_splitparts")
		{
			page_name = string.Format("{0}_gv_splitparts", _page_name);
			var gl = new NeGridLayouts(Convert.ToInt32(my_member.id), page_name);
			gv.LoadClientLayout(gl.GridLayout_Layout);
		}
		else if (_e.Parameters != "")
		{
			gv.LoadClientLayout(_e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
	protected void gv_splitparts_CustomJSProperties(object _sender, ASPxGridViewClientJSPropertiesEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		_e.Properties["cpExp"] = gv.SaveClientLayout();
		_e.Properties["cpFil"] = gv.FilterExpression;
	}
}
public class action_template : ITemplate
{
	public void InstantiateIn(Control _container)
	{
		//HtmlGenericControl im_drag			= new HtmlGenericControl();
		//im_drag.InnerHtml					= "<div style=\"background-color:#fff;background-image:url('/images/icon/icon[arrows].png');background-repeat:no-repeat;background-position:top center;display:inline-block;width:35px;cursor:move;border:solid 1px #ccc;margin:2px;padding:2px;padding-top:17px;\" class='move'> Move</div>";
		//container.Controls.Add(im_drag);

		var im_save = new HtmlGenericControl();
		im_save.InnerHtml = "<button type='button' onclick='split_save(this)'><img src='/images/icon/icon[save].gif' align='absmiddle' title='save' /> Save</button>";
		_container.Controls.Add(im_save);

		//HtmlGenericControl im_cancel		= new HtmlGenericControl();
		//im_cancel.InnerHtml					= "<div style='display:inline-block;width:25px;'><img src='/images/icon/icon[cancel].gif' style='cursor:pointer' title='drag me' /></div>";
		//container.Controls.Add(im_cancel);
	}
}
public class attribute_template : ITemplate
{
	private int _a_id;
	private int _t_id;
	public attribute_template(object a_id, object t_id)
	{
		_a_id = Convert.ToInt32(a_id);
		_t_id = Convert.ToInt32(t_id);
	}
	public void InstantiateIn(Control _container)
	{
		var dt = new DataTable();
		if (_a_id == 16) // To provide ALL manufacturers
		{
			dt = Toolbox.doSQL_dt(@"SELECT a.attribute_value_id id, a.value value FROM inventory_attribute_value a  WHERE a.attribute_id =@v0 AND a.active = true ORDER BY value", new object[] { _a_id });
		}
		else
		{
			dt = Toolbox.doSQL_dt(@"SELECT a.value_id id, b.value value FROM inventory_tag_preset a LEFT JOIN inventory_attribute_value b ON a.value_id = b.attribute_value_id  WHERE a.attribute_id =@v0 AND a.tag_id =@v1  AND b.active = true ORDER BY b.value", new object[] { _a_id, _t_id });
		}
		var cb = new DropDownList();
		//ASPxComboBox cb			= new ASPxComboBox();
		cb.ID = "attribute";
		cb.Width = Unit.Pixel(100);
		//cb.Native				= true;
		cb.Style.Add("font-family", "Tahoma");
		cb.Style.Add("font-size", "12px");
		//cb.ReadOnly				= false;
		cb.Enabled = true;
		//cb.EnableViewState		= true;
		cb.CssClass = "attribute";
		//cb.ClientInstanceName	= "cb_"+_a_id;
		//cb.ClientEnabled		= true;
		//cb.EnableCallbackMode	= true;
		/*
		foreach(DataRow _dr in _dt.Rows)
			{
			ListEditItem lei	= new ListEditItem();
			lei.Text			= _dr["value"].ToString();
			lei.Value			= _dr["id"].ToString();
			cb.Items.Add(lei);
			}
		 */
		_container.Controls.Add(cb);
		cb.DataSource = dt;
		//cb.TextField			= "value";
		//cb.ValueField			= "id";
		cb.DataTextField = "value";
		cb.DataValueField = "id";
		cb.DataBind();
	}
}
public class this_inventory
{
	#region Variable Declarations
	public NeMember this_member;
	public NeBusinessUnit this_business_unit;
	private Toolbox _tools;
	private inventory _inventory;
	public this_inventory(NeMember _member)
	{
		_tools = new Toolbox();
		this_member = _member;
		_inventory = new inventory();
		_tools.current_user = _member;
		_tools.page_author = new NeMember(711);
	}

	string _sql = "";
	int _this_int = 0;
	string _this_string = "";
	bool _this_bool = false;
	List<string> _this_list = new List<string>();
	private DataTable _dt = new DataTable();
	HttpRequest _req = HttpContext.Current.Request;
	NameValueCollection _form = HttpContext.Current.Request.Form;
	HttpResponse _resp = HttpContext.Current.Response;
	Hashtable _this_hash;
	#endregion
	#region Boolean Methods
	#region Private
	private bool delete_detail(string _item_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_detail WHERE master_id = @v0;", _item_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	private bool delete_master(string _item_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_master WHERE master_id = @v0", _item_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	#endregion
	#region Public
	public bool add_value(string _attribute_id, string _new_value)
	{
		try
		{
			Toolbox.doSQL_void(@"INSERT INTO inventory_attribute_value (attribute_id, value, created_by, approved, approved_by, qced) VALUES (@v0, @v1, @v2, 1, @v2, 1)", new object[] {
				_attribute_id, _new_value, this_member.id});
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool approve(string _id, int _approvedby, string _type_of)
	{
		var table = "";
		var column = "";
		switch (_type_of)
		{
			case "part":
				table = "inventory_item_master";
				column = "master_id";
				break;
			case "value":
				table = "inventory_attribute_value";
				column = "attribute_value_id";
				break;
			case "attribute":
				table = "inventory_attribute";
				column = "attribute_id";
				break;
			case "tag":
				table = "inventory_tag";
				column = "tag_id";
				break;
			default:
				throw new Exception("The type of approval is not properly defined.");
		}
		try
		{
			if (_type_of == "value")
			{
				// Get a list of current parts that use this value
				var part_list = Toolbox.doSQL_dt(@"SELECT DISTINCT(a.master_id) FROM inventory_item_detail a WHERE a.attribute_value_id = @v0  AND (SELECT IFNULL(approved, false) approved FROM inventory_item_master b WHERE b.master_id = a.master_id) = false", new object[] { _id });
				if (part_list.Rows.Count > 0)
				{
					// There are parts that use this value
					foreach (DataRow part in part_list.Rows)
					{
						// Are there other values in this part that need to be qc'ed ?
						var master_id = part["master_id"].ToString();
						var c = Toolbox.doSQL_int(@"SELECT COUNT(*) c FROM inventory_attribute_value 
WHERE attribute_value_id IN (SELECT attribute_value_id FROM inventory_item_detail WHERE master_id = @v0) AND (approved = false OR approved is null) ", master_id);
						if (c == 1)
						{
							// This is the only value that is unqc'ed, approved the part
							approve(master_id, _approvedby, "part");
						}
					}
				}
				Toolbox.doSQL_void(@"UPDATE inventory_attribute_value 
SET approved = IF(approved = 1, 1, (1 - IFNULL(approved,0))), approved_by = @v0, approved_date = now() WHERE attribute_value_id = @v1 LIMIT 1", new object[] { _approvedby, _id });
			}
			else if (_type_of == "part")
			{
				Toolbox.doSQL_void(@"UPDATE inventory_item_master SET approved = 1 - IFNULL(approved,0),
approved_by = @v0, approved_date = now() WHERE master_id = @v1 LIMIT 1", new object[] { _approvedby, _id });
				var value_list = Toolbox.doSQL_dt(@"SELECT DISTINCT(attribute_value_id) FROM inventory_item_detail a WHERE master_id = @v0  AND (SELECT IFNULL(approved, false) approved FROM inventory_attribute_value b WHERE b.attribute_value_id = a.attribute_value_id) = false", new object[] { _id });
				if (value_list.Rows.Count > 0)
				{
					foreach (DataRow value in value_list.Rows)
					{
						var attribute_value_id = value["attribute_value_id"].ToString();
						approve(attribute_value_id, _approvedby, "value");
					}
				}
			}
			else if (_type_of == "tag")
			{
				Toolbox.doSQL_void(string.Format("UPDATE {0} SET approved = 1 - IFNULL(approved,0),active = 1- IFNULL(active,0), approved_by = {1}, approved_date = now() WHERE {2} = {3} LIMIT 1", table, _approvedby, column, _id));
			}
			else
			{
				Toolbox.doSQL_void(string.Format("UPDATE {0} SET approved = 1 - IFNULL(approved,0), approved_by = {1}, approved_date = now() WHERE {2} = {3} LIMIT 1", table, _approvedby, column, _id));
			}
			_this_bool = true;
		}
		catch (Exception ee)
		{
			throw ee;
		}
		return _this_bool;
	}
	public bool been_linked(string _tag_id)
	{
		try
		{
			_this_int = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_tag_link WHERE tag_id = @v0", new object[] { _tag_id });
		}
		catch
		{
			_this_int = 0;
		}
		if (_this_int > 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool check_tag_id(string _tag)
	{
		try
		{
			_this_int = Toolbox.doSQL_int(@"SELECT COUNT(tag_id) FROM inventory_tag WHERE tag_id = @v0", _tag);
		}
		catch
		{
			_this_int = 0;
		}

		if (_this_int > 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool clear_part_detail(string _master_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_detail WHERE master_id = @v0", _master_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool delete_attribute(string _attribute_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_attribute WHERE attribute_id = @v0", _attribute_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return true;
	}
	public bool Delete_Part(string _master_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_price WHERE master_id = @v0", _master_id);               // DELETE PRICE
			Toolbox.doSQL_void(@"DELETE FROM inventory_sellprice WHERE master_id = @v0", _master_id);           // DELETE SELLPRICE
			Toolbox.doSQL_void(@"DELETE FROM inventory_sellprice_history WHERE master_id =  @v0", _master_id);  // DELETE SELLPRICE HISTORY
			Toolbox.doSQL_void(@"DELETE FROM inventory_reference WHERE master_id =  @v0", _master_id);          // DELETE REFERENCES
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_detail WHERE master_id =  @v0", _master_id);            // DELETE ITEM DETAIL
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_master WHERE master_id =  @v0", _master_id);            // DELETE ITEM MASTER
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool delete_previous_linkage(object _tag_id)
	{
		try
		{
			Toolbox.doSQL_void(string.Format(@"DELETE FROM inventory_tag_link WHERE tag_id in ({0})", _tag_id));
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool delete_previous_predefined(object _tag_id)
	{
		try
		{
			Toolbox.doSQL_void(string.Format(@"DELETE FROM inventory_tag_preset WHERE tag_id in ({0})", _tag_id));
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool delete_tag(string _tag_id)
	{
		var table = "";
		var effected_tables = new string[3] { "inventory_tag_preset", "inventory_tag_link", "inventory_tag" };
		for (var i = 0; i < effected_tables.Length; i++)
		{
			table = effected_tables[i];
			try
			{
				Toolbox.doSQL_void(string.Format(@"DELETE FROM {0} WHERE tag_id = @v1", table), new object[] { _tag_id });
				_this_bool = true;
			}
			catch
			{
				_this_bool = false;
			}
		}
		return true;
	}
	public void delete_value(string _attribute_value_id)
	{
		Toolbox.doSQL_void(@"DELETE FROM inventory_attribute_value WHERE attribute_value_id = @v0", _attribute_value_id);
	}
	public bool unapprove_value(string _attribute_value_id)
	{
		try
		{
			Toolbox.doSQL_void(@"UPDATE inventory_attribute_value 
SET approved = 0, approved_by = null WHERE attribute_value_id = @v0 LIMIT 1", _attribute_value_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool edit_value(string _attribute_value_id, string _new_value)
	{
		_sql = @"
UPDATE 
	inventory_attribute_value 
SET 
	value = @v0
WHERE 
	attribute_value_id = @v1";
		try
		{
			Toolbox.doSQL_void(_sql, new object[] { _new_value, _attribute_value_id });
			Toolbox.doSQL_void(@"UPDATE inventory_description SET 
	description = FULL_PART_DESCRIPTION(master_id, false, ''), 
	desc_short_cdn = PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_short_usa = PART_DESCRIPTION(master_id, false, 'USA'), 
	desc_full_cdn = FULL_PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_full_usa = FULL_PART_DESCRIPTION(master_id, false, 'USA') WHERE master_id in (SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = @v0)", _attribute_value_id);
			_this_bool = true;
		}
		catch (Exception ee)
		{
			_tools.catch_error(ee);
			_this_bool = false;
		}
		return _this_bool;
	}
	//public bool exists_in_purchase_order_detail(string _part_number, string _dsn)
	//{
	//	_this_int = _tools.getSQL_int(@"SELECT COUNT(*) FROM PURCHASE_ORDER_DTAIL WHERE CODE = ? ", _dsn, new object[] { _part_number });
	//	if (_this_int > 0)
	//	{
	//		_this_bool = true;
	//	}
	//	else
	//	{
	//		_this_bool = false;
	//	}
	//	return _this_bool;
	//}
	//public bool exists_in_sales_order_detail(string _part_number, string _dsn)
	//{
	//	_this_int = _tools.getSQL_int(@"SELECT COUNT(*) FROM SALES_ORDER_DETAIL WHERE CODE = ? ", _dsn, new object[] { _part_number });
	//	if (_this_int > 0)
	//	{
	//		_this_bool = true;
	//	}
	//	else
	//	{
	//		_this_bool = false;
	//	}
	//	return _this_bool;
	//}
	public bool insert_part_detail(string _master_id, string _attribute_value_id)
	{
		try
		{
			Toolbox.doSQL_void(@"INSERT INTO inventory_item_detail (master_id, attribute_value_id) VALUES (@v0, @v1)", new object[] { _master_id, _attribute_value_id });
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool is_item_unique(string _tag, string _tagpattern)
	{
		_this_int = Toolbox.doSQL_int(string.Format("SELECT COUNT(*) count FROM inventory_lookup WHERE TAG = {0} AND ATTVAL = \"|{1},|\"", _tag, _tagpattern));
		if (_this_int == 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool is_item_unique(string _tag, string _tagpattern, string _master_id)
	{
		_this_int = Toolbox.doSQL_int(string.Format("SELECT COUNT(*) count FROM inventory_lookup WHERE TAG = {0} AND ATTVAL = \"|{1},|\" AND item != {2} ", _tag, _tagpattern, _master_id));
		if (_this_int == 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool is_qty(object _tag_id)
	{
		return Convert.ToBoolean(Toolbox.doSQL_int("SELECT is_qty FROM inventory_tag WHERE tag_id =@v0 ", new object[] { _tag_id }));
	}
	public bool NewAttribute(string _new_attribute)
	{
		if (_new_attribute != "")
		{
			try
			{
				Toolbox.doSQL_void(@"INSERT INTO inventory_attribute (attribute) VALUES (@v0)", _new_attribute);
				_this_bool = true;
			}
			catch
			{
				_this_bool = false;
			}
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	//public bool part_exists_in_bv(string _part_number, string _dsn)
	//{
	//	_this_int = _tools.getSQL_int(@"SELECT COUNT(*) FROM INVENTORY WHERE CODE = ? ", _dsn, new object[] { _part_number });
	//	if (_this_int > 0)
	//	{
	//		_this_bool = true;
	//	}
	//	else
	//	{
	//		_this_bool = false;
	//	}
	//	return _this_bool;
	//}
	public void price_edit(object _master_id, int _business_unit_id, object _vendor_id, object _part_number, object _unit_cost, object _total_cost, object _qty, object _price_id, object _benchmark)
	{
		try
		{
			var vpr = new vendor_price_row();
			vpr.cost = _unit_cost;
			vpr.total = _total_cost;
			vpr.master_id = _master_id;
			vpr.vendor_id = _vendor_id;
			vpr.price_id = _price_id;
			vpr.vendor_code = _part_number;
			vpr.qty = _qty;
			vpr.member_id = this_member.id;
			vpr.is_benchmark = Convert.ToBoolean(_benchmark);
			vpr.business_unit_id = _business_unit_id;
			vpr.save();
			_resp.Write("Success");
		}
		catch (Exception ee)
		{
			_resp.Write(ee.ToString());
		}
	}
	#region PriceNew
	/// <summary>
	/// 
	/// </summary>
	/// <param name="_master_id"></param>
	/// <param name="_business_unit_id"></param>
	/// <param name="_vendor_id"></param>
	/// <param name="_part_number"></param>
	/// <param name="_unit_cost"></param>
	/// <param name="_total_cost"></param>
	/// <param name="_qty"></param>
	/// <returns></returns>
	public void price_new(object _master_id, int _business_unit_id, object _vendor_id, object _part_number, object _unit_cost, object _total_cost, object _qty)
	{
		try
		{
			var vpr = new vendor_price_row();
			vpr.cost = _unit_cost;
			vpr.total = _total_cost;
			vpr.master_id = _master_id;
			vpr.vendor_id = _vendor_id;
			vpr.vendor_code =_part_number;
			vpr.qty = _qty;
			vpr.is_benchmark = false;
			vpr.member_id = this_member.id;
			vpr.business_unit_id = _business_unit_id;
			vpr.save();
			_resp.Write("Success");
		}
		catch (Exception ee)
		{
			_resp.Write(ee.ToString());
		}
	}
	#endregion PriceNew
	public bool roll_back_part(string _master_id)
	{
		try
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_master WHERE master_id =@v0", _master_id);
			Toolbox.doSQL_void(@"DELETE FROM inventory_item_detail WHERE master_id =@v0", _master_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool tag_approved(string _tag)
	{
		try
		{
			_this_int = Toolbox.doSQL_int(@"SELECT approved FROM inventory_tag WHERE tag_id = @v0", _tag);
		}
		catch
		{
			_this_int = 0;
		}
		if (_this_int == 1)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool tag_has_dependants(string _tag_id)
	{
		_this_int = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_item_master WHERE tag_id =@v0 ", _tag_id);
		if (_this_int > 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool unapprove_tag(string _tag_id)
	{
		try
		{
			Toolbox.doSQL_void(@"UPDATE inventory_tag SET approved = 0, approved_by = 0, active = 0 WHERE tag_id =@v0 ", _tag_id);
			_this_bool = true;
		}
		catch
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	public bool used_in_tag(string _value_id)
	{
		try
		{
			_this_int = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_tag_preset WHERE value_id = @v0", _value_id);
		}
		catch
		{
			_this_int = 0;
		}
		if (_this_int > 0)
		{
			_this_bool = true;
		}
		else
		{
			_this_bool = false;
		}
		return _this_bool;
	}
	#endregion
	#endregion
	#region HashTable Methods
	//public Hashtable header_info(string _part_number, string _dsn)
	//{
	//	_dt = Toolbox.doSQL_dt(@"SELECT TOP 1 MAX(SELLING_PRICE) PRICE, VENDOR_CODE FROM SPECIAL_PRICING WHERE BVSPECPRICEPARTNO = ?  GROUP BY VENDOR_CODE ORDER BY PRICE DESC", _dsn, new object[] { _part_number });
	//	_this_hash = new Hashtable();
	//	foreach (DataRow dr in _dt.Rows)
	//	{
	//		var cost_price = dr["PRICE"].ToString();
	//		var vendor_code = dr["VENDOR_CODE"].ToString();
	//		_this_hash["COSTPRICE"] = cost_price;
	//		_this_hash["VENDORCODE"] = vendor_code.TrimEnd();
	//	}
	//	return _this_hash;
	//}
	#endregion
	#region Integer Methods
	public int[] business_unit_id_from_dsn(object _dsn)
		{
		//return Convert.ToInt32(_dsn);
		var dt= Toolbox.doSQL_dt(@"SELECT b.id id FROM business_unit b inner join tax_entity t on t.id = b.tax_entity_id WHERE t.dsn = @v0", new object[] { _dsn });
		List<int> buss_Ids=new List<int>();
		foreach (DataRow dr in dt.Rows)
			{
				buss_Ids.Add(Convert.ToInt32(dr["id"]));
			}
		return buss_Ids.ToArray();
		}
	public int Attribute_n_Descriptors(string _tag_id)
	{
		if (_tag_id.Length > 0)
		{
			if (_tag_id != "")
			{
				try
				{
					_this_int = Toolbox.doSQL_int(@"SELECT count(*) c FROM inventory_tag_link WHERE order_id != 99 AND tag_id = @v0", _tag_id);
				}
				catch
				{
					_this_int = 0;
				}
			}
			else
			{
				_this_int = 0;
			}
		}
		return _this_int;
	}
	public int tag_count()
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(tag_id) FROM inventory_tag");
	}
	public int tag_count(string _searchterm)
	{
		try
		{
			_this_int = Toolbox.doSQL_int(@"SELECT COUNT(tag_id) FROM inventory_tag WHERE tag LIKE CONCAT('%',@v0,'%') OR tag_id LIKE CONCAT('%',@v0,'%')", _searchterm);
		}
		catch
		{
			_this_int = 0;
		}
		return _this_int;
	}
	public int values_for_attribute(string _attribute_id)
	{
		return Toolbox.doSQL_int(@"
SELECT
IFNULL(COUNT(attribute_value_id), 0) howmany
FROM 
inventory_attribute_value
WHERE 
attribute_id = @v0 AND active = true", _attribute_id);
	}
	public int n_Tag_attributes(string _tag_id)
	{
		return Toolbox.doSQL_int(@"SELECT IFNULL(COUNT(*), 0) C FROM inventory_tag_link WHERE tag_id = @v0", _tag_id);
	}
	public int n_Unapproved(string _table)
	{
		return Toolbox.doSQL_int(string.Format("SELECT IFNULL(COUNT(*), 0) C FROM {0} WHERE (approved = 0  OR approved is NULL)  AND active = true", _table));
	}
	public int n_Unapproved(string _table, object _id)
	{
		var field_name = "";
		switch (_table)
		{
			case "inventory_attribute_value":
				field_name = "active = true AND attribute_id";
				break;
			case "inventory_attribute":
				field_name = "";
				break;
			case "inventory_picture":
				field_name = "";
				break;
			case "inventory_item_master":
				field_name = "tag_id";
				break;
		}
		return Toolbox.doSQL_int(string.Format(@"SELECT IFNULL(COUNT(*), 0) C FROM {0} WHERE (approved = 0 OR approved is NULL) AND active = 1 AND {1} = @v0", _table, field_name),new object[] {
		_id});
	}
	public int n_Approved(string _table)
	{
		return Toolbox.doSQL_int(string.Format(@"SELECT IFNULL(COUNT(*),0) C FROM {0} WHERE approved = 1",  _table));
	}
	public int how_many_values(string _attribute_value_id)
	{
		return Toolbox.doSQL_int(@"
SELECT
IFNULL(COUNT(b.attribute_value_id), 0) howmany
FROM 
inventory_attribute_value a
LEFT JOIN
inventory_item_detail b ON a.attribute_value_id = b.attribute_value_id
WHERE 
a.attribute_value_id = @v0 AND 
a.active = true
GROUP BY 
value", new object[] { _attribute_value_id });
	}
	public int insert_master_part(string _tag_id)
	{
		return Convert.ToInt32(_tools.returnSQL_id(@"INSERT INTO inventory_item_master 
(tag_id, create_date, created_by, approved) VALUES (@v0, now(), @v1 ,1)", new object[] { _tag_id, this_member.id }));
	}
	#endregion
	#region DataTable Methods
	public DataTable approved_tags()
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_tag  WHERE approved = 1 ORDER BY tag", null);
	}
	public DataTable attributes()
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute ORDER BY attribute", null);
	}
	public DataTable attributes_with_unapproved_values()
	{
		return Toolbox.doSQL_dt(@"
SELECT 
	a.attribute_id id, 
	a.attribute name, 
	count(b.value) count 
FROM 
	inventory_attribute a 
LEFT JOIN inventory_attribute_value b ON a.attribute_id = b.attribute_id
WHERE b.approved = 0 AND b.active = true
GROUP BY id, name  
ORDER BY a.attribute;", null);
	}
	public DataTable attributes(string _tag_id)
	{
		_sql = @"
SELECT 
	a.attribute_id, 
	b.attribute 
FROM 
	inventory_tag_link a 
LEFT JOIN 
	inventory_attribute b 
	ON 
	a.attribute_id = b.attribute_id
WHERE 
	tag_id = " + _tag_id + " ORDER BY a.attribute_id";
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable Browse_Attributes(string _tag_id)
	{
		_sql = string.Format(@"
SELECT 
	d.attribute,
	d.attribute_id
FROM 
	inventory_item_detail a 
LEFT JOIN 
	inventory_attribute_value b
		ON a.attribute_value_id = b.attribute_value_id 
LEFT JOIN
	inventory_item_master c
		ON a.master_id = c.master_id
LEFT JOIN
	inventory_attribute d
		on b.attribute_id = d.attribute_id
WHERE c.tag_id = {0} AND b.active = true
GROUP BY d.attribute_id, d.attribute
ORDER BY d.attribute_id", _tag_id);
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable Browse_Basic()
	{
		return Toolbox.doSQL_dt(@"SELECT id,name FROM inventory group BY id", null);
	}
	public DataTable Browse_Tags()
	{
		return Toolbox.doSQL_dt(@"SELECT DISTINCT(tag) TAG, TAG_ID FROM inventory ORDER BY TAG", null);
	}
	public DataTable Browse_Values(string _tag_id, string _att_id)
	{
		_sql = string.Format(@"
SELECT 
	b.value,
	b.attribute_value_id value_id
FROM 
	inventory_item_detail a 
LEFT JOIN 
	inventory_attribute_value b
		ON a.attribute_value_id = b.attribute_value_id 
LEFT JOIN
	inventory_item_master c
		ON a.master_id = c.master_id
LEFT JOIN
	inventory_attribute d
		ON b.attribute_id = d.attribute_id
WHERE c.tag_id = {0} and d.attribute_id = {1} AND b.active = true
GROUP BY value_id", _tag_id, _att_id);
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable company()
	{
		return Toolbox.doSQL_dt(@"SELECT * from business_unit ", null);
	}
	public DataTable tag_attribute_available_values(string _tag_id, string _attribute_id)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v1  AND active = true AND attribute_value_id NOT IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 )", new object[] { _tag_id, _attribute_id });
	}
	public DataTable tag_attribute_selected_values(string _tag_id, string _attribute_id)
	{
		if (_attribute_id == "16")// || attribute_id == "17")
		{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v1  AND active = true ORDER BY value", new object[] { _tag_id, _attribute_id });
		}
		else
		{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_value_id in (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) and active = true ORDER BY value", new object[] { _tag_id, _attribute_id });
		}
	}
	public DataTable tag_att_vals(object _master_id, object _tag_id)
	{
		return Toolbox.doSQL_dt(@"SELECT a.tag_id, a.attribute_id, (SELECT IFNULL(MAX(b.attribute_value_id), 0) FROM inventory_item_detail b LEFT JOIN inventory_attribute_value c ON b.attribute_value_id = c.attribute_value_id WHERE b.master_id = @v0  AND c.attribute_id = a.attribute_id) value_id,d.attribute, a.order_id FROM inventory_tag_link a LEFT JOIN inventory_attribute d ON a.attribute_id = d.attribute_id where a.tag_id = @v1 ", new object[] { _master_id, _tag_id });
	}
	public DataTable tag_linkage(string _tagid)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_tag_link WHERE tag_id = @v0  ORDER BY attribute_id", new object[] { _tagid });
	}
	public DataTable tags(string _seed, string _searchterm, string _per_page)
	{
		switch (_searchterm)
		{
			case "APPROVED":
			case "approved":
				_sql = string.Format("SELECT tag_id,tag,ifnull(approved,0) approved,ifnull(approved_by,0) approved_by  FROM inventory_tag WHERE approved = 1 ORDER BY tag LIMIT {0},{1}", _seed, _per_page);
				break;
			case "UNAPPROVED":
			case "unapproved":
				_sql = string.Format("SELECT tag_id,tag,ifnull(approved,0) approved,ifnull(approved_by,0) approved_by  FROM inventory_tag WHERE approved = 0 ORDER BY tag LIMIT {0},{1}", _seed, _per_page);
				break;
			default:
				_sql = string.Format("SELECT tag_id,tag,ifnull(approved,0) approved,ifnull(approved_by,0) approved_by  FROM inventory_tag WHERE tag LIKE '%{0}%' OR tag_id LIKE \"%{0}%\" ORDER BY tag LIMIT {1},{2}", _searchterm, _seed, _per_page);
				break;
		}
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable tags()
	{
		return Toolbox.doSQL_dt(@"SELECT DISTINCT(b.tag_id) tag_id, b.tag FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id  WHERE a.active = 1 AND b.active = 1 AND (a.approved = 0 OR a.approved is null) ORDER BY b.tag", null);
	}
	public DataTable tags(int _exclude)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  and active = true ORDER BY value", new object[] { _exclude });
	}
	public DataTable values(string _attribute_id)
	{
		return Toolbox.doSQL_dt(@" SELECT *, ( SELECT IFNULL(COUNT(b.attribute_value_id), 0) FROM inventory_attribute_value a LEFT JOIN inventory_item_detail b ON a.attribute_value_id = b.attribute_value_id WHERE a.attribute_value_id = c.attribute_value_id AND a.active = true GROUP BY value ) howmany, ( SELECT COUNT(*) FROM inventory_tag_preset WHERE value_id = c.attribute_value_id ) used_in_tag FROM inventory_attribute_value c WHERE c.attribute_id = @v0  and c.active = true ORDER BY c.value", new object[] { _attribute_id });
	}
	public DataTable available_values(string _tag_id, string _attribute_id)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v1  AND active = true AND attribute_value_id NOT IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) ORDER BY value", new object[] { _tag_id, _attribute_id });
	}
	public DataTable selected_values(string _tag_id, string _attribute_id)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_value_id IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) AND active = true ORDER BY value", new object[] { _tag_id, _attribute_id });
	}
	public DataTable view_item(string _item_id)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  AND active = true ORDER BY value", new object[] { _item_id });
	}
	//public DataTable part_lookup(string _q, string _dsn)
	//{
	//	return Toolbox.doSQL_dt(@" SELECT RTRIM(CODE) CODE, RTRIM(INV_DESCRIPTION) DESCRIPTION FROM INVENTORY WHERE CODE LIKE ? OR INV_DESCRIPTION LIKE ?", _dsn, new object[] { "%"+ _q + "%", "%" + _q + "%" });
	//}
	public DataTable Get_Pricing(string _master_id)
	{
		_sql = @"
SELECT 
	a.id,
	CAST(IFNULL(a.edited_dt, 'N/A') AS CHAR) edited_dt,
	b.name, 
	b.id, 
	c.vendor_number, 
	c.vendor_id, 
	c.vendor_name, 
	IFNULL(a.vendor_code, '') reference, 
	IF(b.country = 'CDN', true, false) is_canadian,
	a.total, 
	a.cost, 
	a.qty,
	a.benchmark
FROM 
	inventory_price a 
LEFT JOIN 
	business_unit b 
		ON a.business_unit_id = b.id 
LEFT JOIN 
	vendor c 
		ON a.vendor_id = c.vendor_id 
WHERE 
	a.master_id = " + _master_id + " ORDER BY a.cost";
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable Match_Parts(string _tag_id, string _attval_pattern)
	{
		_sql = string.Format("SELECT active, tag, item FROM inventory_lookup WHERE tag = {0} ", _tag_id);
		var s = _attval_pattern.Split(new string[] { "," }, StringSplitOptions.None);
		if (_attval_pattern.Trim().Length > 0)
		{
			for (var i = 0; i < s.Length; i++)
			{
				if (s[i] != "0" && s[i] != "")
				{
					if (i == (s.Length - 1))
					{
						_sql += " AND attval REGEXP '[|,]" + s[i] + ",'";
					}
					else
					{
						_sql += " AND attval REGEXP '[|,]" + s[i] + ",'";
					}
				}
				else
				{
					_sql += " AND 1 = 1";
				}
			}
		}
		else
		{
			_sql += " AND 1 = 1";
		}
		return Toolbox.doSQL_dt(_sql, null);
	}
	public DataTable Part_Att_Vals(string _master_id)
	{
		return Toolbox.doSQL_dt(@"SELECT * FROM inventory WHERE master_id = @v0  ORDER BY order_id ASC", new object[] { _master_id });
	}
	public DataTable Unapproved_Attributes(string _from, int _to)
	{
		return Toolbox.doSQL_dt(string.Format("SELECT * FROM inventory_attribute WHERE approved = 0 ORDER BY attribute LIMIT {0}, {1}", _from, _to), null);
	}
	public DataTable Unapproved_Values(string _from, int _to)
	{
		return Toolbox.doSQL_dt(string.Format(@"
		SELECT 
			attribute_id, 
			get_attribute(attribute_id) attribute_name, 
			attribute_value_id value_id, 
			get_value(attribute_value_id) value_name, 
			get_name(created_by) member_name 
		FROM 
			inventory_attribute_value 
		WHERE 
			approved = 0 AND
			active = true
		ORDER BY 
			attribute_name, value 
		LIMIT {0}, {1}", _from, _to), null);
	}
	public DataTable Unapproved_Values(string _from, int _to, object _attribute_id)
	{
		return Toolbox.doSQL_dt(string.Format(@"
		SELECT 
			attribute_id, 
			get_attribute(attribute_id) attribute_name, 
			attribute_value_id value_id, 
			get_value(attribute_value_id) value_name, 
			get_name(created_by) member_name 
		FROM 
			inventory_attribute_value 
		WHERE 
			approved = 0 AND
			attribute_id = {2} AND
			active = true
		ORDER BY 
			attribute_name, value 
		LIMIT {0}, {1}", _from, _to, _attribute_id), null);
	}
	public DataTable Unapproved_Parts(string _from, int _to)
	{
		return Unapproved_Parts(_from, _to, "");
	}
	public DataTable Unapproved_Parts(string _from, int _to, string _tag_id)
	{
		if (_tag_id != "")
		{
			_sql = string.Format("SELECT * FROM inventory_item_master WHERE (approved = 0 OR approved is null) AND tag_id = {2} AND active = 1 ORDER BY create_date LIMIT {0}, {1}", _from, _to, _tag_id);
		}
		else
		{
			_sql = string.Format("SELECT * FROM inventory_item_master WHERE (approved = 0 OR approved is null) AND active = 1  ORDER BY create_date LIMIT {0}, {1}", _from, _to);
		}
		return Toolbox.doSQL_dt(_sql, null);
	}
	#endregion
	#region String Methods
	#region Private
	private string part_list(string _dsn)
	{
		try
		{
			_this_string = Toolbox.doSQL_string(@"
SELECT 
	GROUP_CONCAT('\'',a.vendor_code,'\'') parts 
FROM 
	inventory_price a 
LEFT JOIN 
	business_unit b 
	ON a.business_unit_id = b.id 
LEFT JOIN
	tax_entity c ON b.tax_entity_id = c.id
WHERE 
	c.DSN = '" + _dsn + @"' AND 
	a.vendor_code != ''");
		}
		catch
		{
			_this_string = "";
		}
		return _this_string;
	}
	#endregion Private	
	#region Public
	public string get_vendor_id(string _v_num)
	{
		return Toolbox.doSQL_string(@"SELECT vendor_id FROM vendor WHERE vendor_number_int = @v0 LIMIT 1", _v_num);
	}
	public string business_unit_selector()
	{
		var selector = "<select onchange='refactor_pricing(this);'>";
		_dt = Toolbox.doSQL_dt("SELECT id, name FROM business_unit WHERE id IN (SELECT DISTINCT warehouse_bu_id FROM business_unit)", null);
		foreach (DataRow dr in _dt.Rows)
		{
			var temp_name = dr["name"].ToString().Replace("NE ", "");
			var temp_business_unit_id = dr["id"].ToString();
			var temp_selected = "";
			if (temp_business_unit_id == this_business_unit.id.ToString())
			{
				temp_selected = " selected";
			}
			else
			{
				temp_selected = "";
			}
			selector += string.Format("<option value='{0}' {1}>{2}</option>", temp_business_unit_id, temp_selected, temp_name);
		}
		selector += "</select>";
		return selector;
	}
	public string add_attribute(string _attribute)
	{
		return _tools.returnSQL_id(@"INSERT INTO inventory_attribute (attribute) VALUES (@v0)", new object[] { _attribute });
	}
	public string add_tag(object _new_tag)
	{
		try
		{
			_this_string = _tools.returnSQL_id(@"INSERT INTO inventory_tag (tag, approved, sold_as, insert_dt) VALUES (@v0, 0, 1, now())", new object[] { _new_tag });
		}
		catch (Exception ee)
		{
			_tools.catch_error(ee);
			_this_string = "0";
		}
		return _this_string;
	}
	
	public string[] available_ds_ns()
		{
		
		_dt = Toolbox.doSQL_dt(@"SELECT DISTINCT t.dsn dsn from business_unit b inner join tax_entity t on t.id = b.tax_entity_id  WHERE has_inventory = 1 ORDER BY dsn", null);
		foreach (DataRow dr in _dt.Rows)
			{
			_this_string = dr["dsn"].ToString();
			_this_list.Add(_this_string);
			}
		return _this_list.ToArray();
		}

	public string attribute_name(string _attribute_id)
	{
		if (_attribute_id.Length > 0)
		{
			try
			{
				_this_string = Toolbox.doSQL_string(@"SELECT attribute FROM inventory_attribute WHERE attribute_id = @v0", _attribute_id);
			}
			catch
			{
				_this_string = "Failed";
			}
		}
		return _this_string;
	}
	//public string business_unit_name(string _dsn)
	//{
	//	try
	//	{
	//		_this_string = Toolbox.doSQL_string(@"SELECT name from business_unit  WHERE company_dsnbv7 = @v0", _dsn);
	//	}
	//	catch
	//	{
	//		_this_string = "Failed";
	//	}
	//	return _this_string;
	//}
	public string get_TagIDfromMaster(object _master_id)
	{
		return Toolbox.doSQL_string(@"SELECT tag_id FROM inventory_item_master WHERE master_id = @v0", new object[] { _master_id });
	}
	public string handle_pic(object _picture_id, object _which, object _master_id)
	{
		try
		{
			if (_which.ToString() == "1")
			{
				Toolbox.doSQL_void(@"UPDATE inventory_picture SET approved = true, active = true WHERE id =@v0", new object[] { _picture_id });
				Toolbox.doSQL_void(@"UPDATE inventory_picture SET approved = false, active = false WHERE master_id =@v0 AND id !=@v1 ", new object[] { _master_id, _picture_id });
			}
			else if (_which.ToString() == "0")
			{
				Toolbox.doSQL_void(@"UPDATE inventory_picture SET approved = false, active = false WHERE id =@v0", new object[] { _picture_id });
			}
			else
			{
				Toolbox.doSQL_void(@"DELETE FROM inventory_picture WHERE id =@v0", new object[] { _picture_id });
			}
			return "SUCCESS";
		}
		catch
		{
			return "FAILED";
		}
	}
	//public string last_bought_date(string _part_number, string _dsn, string _table)
	//{
	//	return _tools.getSQL_string(string.Format(@"SELECT MAX(LAST_RCVE_DATE) FROM {0}  WHERE CODE = ? ", _table), _dsn, new object[] { _part_number  });
	//}
	//public string last_sold_date(string _part_number, string _dsn, string _table)
	//{
	//	var column = "";
	//	switch (_table)
	//	{
	//		case "SALES_ORDER_DETAIL":
	//			column = "BVRVADDDATE";
	//			break;
	//		case "SALES_HISTORY_DETAIL":
	//			column = "INVOICE_DATE";
	//			break;
	//	}
	//	return _tools.getSQL_string(string.Format(@"SELECT MAX({1}) FROM {0}  WHERE CODE = ? ",_table, column), _dsn, new object[] { _part_number });
	//}
//	public string linked_parts(string _dsn)
//	{
//		var current_parts = part_list(_dsn);
//		_dt = Toolbox.doSQL_dt(string.Format(@"SELECT CODE, COUNT(CODE) C 
//FROM SALES_ORDER_DETAIL WHERE CODE != '' AND CODE IN ({0}) GROUP BY CODE ORDER BY C DESC", current_parts), _dsn, null);
//		if (_dt.Rows.Count > 0)
//		{
//			_this_string = @"
//<parts>";
//			foreach (DataRow dr in _dt.Rows)
//			{
//				var count = dr["C"].ToString();
//				var code = dr["CODE"].ToString();
//				code = code.Trim();
//				_this_string += string.Format(@"
//	<part code=""{0}"" count='{1}' />", code, count);
//			}
//			_this_string += @"
//</parts>";
//		}
//		else
//		{
//			_this_string = @"
//<parts>
//	<part code=""NOTHING EXISTS"" count='0'/>
//</parts>";
//		}
//		return _this_string;
//	}
//	public string not_linked_parts(string _dsn)
//	{
//		var current_parts = part_list(_dsn);
//		_dt = Toolbox.doSQL_dt(string.Format(@"SELECT CODE, COUNT(CODE) C FROM SALES_ORDER_DETAIL WHERE CODE != '' 
//AND CODE NOT IN ({0}) GROUP BY CODE ORDER BY C DESC", current_parts), _dsn, null);
//		if (_dt.Rows.Count > 0)
//		{
//			_this_string = @"
//<parts>";
//			foreach (DataRow dr in _dt.Rows)
//			{
//				var count = dr["C"].ToString();
//				var code = dr["CODE"].ToString();
//				code = code.Trim();
//				_this_string += string.Format(@"
//	<part code=""{0}"" count='{1}' />", code, count);
//			}
//			_this_string += @"
//</parts>";
//		}
//		else
//		{
//			_this_string = @"
//<parts>
//	<part code=""NOTHING EXISTS"" count='0' />
//</parts>";
//		}
//		return _this_string;
//	}
	public string Part_Reference(string _dsn, string _master_id)
	{
		var business_unit_id = business_unit_id_from_dsn(_dsn);
		try
		{
			_this_string = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(reference_part_number), '--') part 
FROM inventory_reference WHERE ref_business_unit_id FIND_IN_SET(@v0) AND master_id = @v1", new object[] { business_unit_id, _master_id });
		}
		catch
		{
			_this_string = "";
		}
		return _this_string.Trim();
	}
	public string Part_Member(object _master_id, bool _get_name)
	{
		var id = Toolbox.doSQL_string(@"SELECT member_id FROM inventory_item_master WHERE master_id = @v0", new object[] { _master_id });
		if (_get_name)
		{
			if (id != "")
			{
				return Toolbox.doSQL_string(@"SELECT get_name(@v0)", id);
			}
			else
			{
				return "";
			}
		}
		else
		{
			if (id != "")
			{
				return id;
			}
			else
			{
				return "";
			}
		}
	}
	public string Part_Description(object _master_id)
	{
		return Toolbox.doSQL_string(@"SELECT part_description(@v0, true, 'CDN')", new object[] { _master_id });
	}
	public string tag_descriptors(string _tag_id)
	{
		_this_string = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(b.attribute ORDER BY a.order_id SEPARATOR ', ') attributes 
FROM inventory_tag_link a LEFT JOIN inventory_attribute b ON a.attribute_id = b.attribute_id WHERE a.tag_id = @v0", new object[] { _tag_id });
		if (_this_string != "")
		{
			return _this_string;
		}
		else
		{
			return "Not Mapped";
		}
	}
	public string tag_name(string _tag_id)
	{
		try
		{
			_this_string = Toolbox.doSQL_string("SELECT tag FROM inventory_tag WHERE tag_id = @v0", new object[] { _tag_id });
		}
		catch
		{
			_this_string = "Failed";
		}
		return _this_string;
	}
	public string pseudo_part_select(object _value_id)
	{
		_dt = Toolbox.doSQL_dt(@"SELECT master_id FROM inventory_item_detail WHERE attribute_value_id = @v0 ", new object[] { _value_id });
		if (_dt.Rows.Count > 0)
		{
			var @select = string.Format(@"<select style='width:100%;font-size:11px;' onchange=""if(this.value != 0){{location.href='./index.aspx?a=edit_part&master_id='+this.value}}""><option value='0'>{0} Part(s)</option>", _dt.Rows.Count);
			var select_sb = new StringBuilder();
			foreach (DataRow dr in _dt.Rows)
			{
				var master_id = dr["master_id"];
				select_sb.Append(string.Format("<option value='{0}'>{0}</option>", master_id));
			}
			@select += select_sb + "</select>";
			return @select;
		}
		else
		{
			return "0";
		}
	}
	public string value_name(string _value_id)
	{
		try
		{
			_this_string = Toolbox.doSQL_string(@"SELECT value FROM inventory_attribute_value WHERE active = true AND attribute_value_id = @v0", _value_id);
		}
		catch
		{
			_this_string = "Failed";
		}
		return _this_string;
	}
	#endregion Public
	#endregion
	#region Void Methods
	#region EditTag
	/// <summary>
	/// <para>Updates the Tag name</para>
	/// <para>Supply a non-escaped name, this function takes cares of the escaping.</para>
	/// </summary>
	/// <param name="_tag_id"></param>
	/// <param name="_new_name"></param>
	public void edit_tag(string _tag_id, string _new_name)
	{
		Toolbox.doSQL_void(@"
UPDATE 
	inventory_tag 
SET 
	tag = @v0
WHERE 
	tag_id = @v1", new object[] { _new_name, _tag_id });
		Toolbox.doSQL_void(@"
UPDATE 
	inventory_description 
SET 
	description = FULL_PART_DESCRIPTION(master_id, false, ''), 
	desc_short_cdn = PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_short_usa = PART_DESCRIPTION(master_id, false, 'USA'), 
	desc_full_cdn = FULL_PART_DESCRIPTION(master_id, false, 'CDN'), 
	desc_full_usa = FULL_PART_DESCRIPTION(master_id, false, 'USA')
WHERE 
	master_id IN (SELECT master_id FROM inventory_item_master WHERE tag_id = @v0)", _tag_id);
	}
	#endregion
	#region EditAttribute
	/// <summary>
	/// <para>Updates the attribute name</para>
	/// <para>Supply a non-escaped name, this function takes cares of the escaping.</para>
	/// </summary>
	/// <param name="_attribute_id"></param>
	/// <param name="_new_name"></param>
	public void edit_attribute(string _attribute_id, string _new_name)
	{
		Toolbox.doSQL_void(@"
UPDATE 
	inventory_attribute 
SET 
	attribute = @v0
WHERE 
	attribute_id = @v1", new object[] { _new_name, _attribute_id });
	}
	#endregion
	#region InsertTagLink
	/// <summary>
	/// <para>Creates a link from the attribute to a tag</para>
	/// <para>Needed for tag creation</para>
	/// </summary>
	/// <param name="_masterid"></param>
	/// <param name="_aid"></param>
	/// <param name="_oid"></param>
	public void insert_tag_link(string _masterid, string _aid, string _oid)
	{
		Toolbox.doSQL_void(@"INSERT INTO inventory_tag_link (tag_id, attribute_id, order_id) VALUES (@v0, @v1, @v2);", new object[] { _masterid, _aid, _oid });
	}
	#endregion
	#region tag_value
	public void tag_value()
	{
		_resp.Clear();
		_tools.dont_cache_page();
		var q = _req.QueryString;
		if (q["att"] != null && q["val"] != null && q["tag_id"] != null)
		{
			var att = q["att"];
			var val = q["val"];
			var tag_id = q["tag_id"];

			// Run check on value first
			var check = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_attribute_value
WHERE attribute_id = @v0 AND active = true AND value = @v1", new object[] { att, val });
			if (check == 0)
			{
				var new_val_id = _tools.returnSQL_id(@"INSERT INTO inventory_attribute_value
(attribute_id, value, approved, approved_by, create_date, created_by, qced)
VALUES (@v0, @v1, true, @v2, now(), @v2, null)", new object[] { att, val, this_member.id, this_member.business_unit_id });
				try
				{
					Toolbox.doSQL_void(@"INSERT INTO inventory_tag_preset (tag_id, attribute_id, value_id, qced) 
VALUES (@v0, @v1, @v2, @v3);", new object[] { tag_id, att, new_val_id, this_member.business_unit_id });
					_resp.Write(new_val_id);
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
					Toolbox.doSQL_void(@"DELETE FROM inventory_attribute_value WHERE attribute_value_id = @v0", new_val_id);
					_resp.Write("Rollback");
				}
			}
			else
			{
				_resp.Write("Exists");
			}
		}
		else
		{
			_resp.Write("Failed");
		}
		_resp.End();
	}
	#endregion
	#region InsetTagPreset
	/// <summary>
	/// Insert a preset value for a supplied tag
	/// </summary>
	/// <param name="_masterid"></param>
	/// <param name="_aid"></param>
	/// <param name="_vid"></param>
	public void insert_tag_preset(string _masterid, string _aid, string _vid)
	{
		if (_vid == "0" || _vid == "")
		{
			_vid = "NULL";
		}
		Toolbox.doSQL_void(@"INSERT INTO inventory_tag_preset (tag_id, attribute_id, value_id) VALUES (@v0, @v1, @v2);", new object[] { _masterid, _aid, _vid });
	}
	#endregion
	#region InsertAttValCombo
	public void insert_att_val_combo(string _masterid, string _aid, string _vid)
	{
		Toolbox.doSQL_void(@"INSERT INTO inventory_item_detail (master_id, attribute_id, attribute_value_id) VALUES (@v0, @v1, @v2)", new object[] { _masterid, _aid, _vid });
	}
	#endregion
	#endregion Void Methods
}

