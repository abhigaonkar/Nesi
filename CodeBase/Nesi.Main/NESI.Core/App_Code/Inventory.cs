using System;
using System.Data;
using System.Linq;
using System.Drawing.Printing;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using NESI.Common.Models;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace nesi.core
{
	/// <summary>
	/// Accessor for Intranet based inventory
	/// </summary>
	public class inventory
	{
		#region Variable Declaration
		public NeMember current_user = null;
		private System.Web.SessionState.HttpSessionState _session;
		private string _description = "PARTNOTFOUND";
		private string _description_short = "PARTNOTFOUND";
		private string _description_int = "PARTNOTFOUND";
		private string _description_full = "PARTNOTFOUND";
		private string _location_name;
		private string _location_id;
		private string _location_qty;
		private string _bv_part_number = "";
		private string _sold_as_canadian = "";
		private string _sold_as_usa = "";
		private string _sold_as_canadian_name = "";
		private string _sold_as_usa_name = "";
		private string _vendor_price_last_dt = "";
		private double _cost_price_weighted = 0;
		private double _onhand_qty = 0;
		private double _dollar_balance = 0;
		private global_price _global_lowest = null;
		private double _country_average_cost = 0;
		private double _company_average_cost = 0;

		#endregion

		public double wo_usage { get; private set; }
		public double po_usage { get; private set; }

		public double int_onhand_qty { get; private set; }
		public double ext_onhand_qty { get; private set; }

		public double total_int_onhand_qty { get; private set; }
		public double total_ext_onhand_qty { get; private set; }
		public double n_minmax { get; set; }
		/// <summary>
		/// Will return the active state of the part.
		/// </summary>
		public bool active { get; set; }

		public bool part_is_loaded { get; set; }

		/// <summary>
		/// Will return if the part was merged.
		/// </summary>
		public bool merged { get; set; }

		/// <summary>
		/// <para>Currently this will only allow for setting an existing master ID to work with</para>
		/// <para>This class will probably be built out further in the future to create parts.</para>
		/// </summary>
		public string master_id { get; set; }
		public int id { get; set; }
		/// <summary>
		/// Will provide the supplied company id
		/// </summary>
		public int business_unit_id { get; set; }

		/// <summary>
		/// Provides the description of the part. Including all non-prioritized attributes
		/// </summary>
		public string description_full { get { return _description_full; } set { _description_full = value; } }
		public string old_id { get; set; }

		public string new_id { get; set; }

		public tag Tag { get; set; }
		public branch branch_obj { get; set; }

		public double ttl_ms { get; set; }
		public bool allowed_to_stock { get; set; }
		public bool allowed_to_edit_after_issue { get; set; }

		public int order_n { get; set; }
		/// <summary>
		/// Provides the description of the part. (deprecated)
		/// </summary>
		public string description { get { return _description; } set { _description = value; } }
		/// <summary>
		/// <para>Provides the (short) description of the part.</para>
		/// <para>Description length is 77 characters with a 3 character ellipsis (...), equaling a total of 80 characters</para>
		/// </summary>
		public string description_short { get { return _description_short; } set { _description_short = value; } }
		/// <summary>
		/// Provides the (int/full) description of the part.
		/// </summary>
		public string description_int { get { return _description_int; } set { _description_int = value; } }
		/// <summary>
		/// Provides the current sell price.
		/// </summary>
		public double sell_price { get; set; }

		public int cost_price_branch_level { get; set; }
		public global_price global_highest { get; set; }

		/// <summary>
		/// Provides the highest current branch cost price.
		/// </summary>
		public double cost_price_branch_highest { get; set; }

		public double cost_price_branch_lowest { get; set; }

		public double cost_price_branch { get; set; }

		/// <summary>
		/// Provides the equivalent BV part number.
		/// </summary>
		public string bv_part_number { get { return _bv_part_number; } set { _bv_part_number = value; } }
		/// <summary>
		/// Provides the unit this part is sold at in Canada
		/// </summary>
		public string sold_as_canadian { get { return _sold_as_canadian; } set { _sold_as_canadian = value; } }
		/// <summary>
		/// Provides the unit this part is sold at in the USA
		/// </summary>
		public string sold_as_usa { get { return _sold_as_usa; } set { _sold_as_usa = value; } }
		public double minqty { get; set; }

		public double maxqty { get; set; }

		/// <summary>
		/// Provides the unit's name that this part is sold at in the USA
		/// </summary>
		public string sold_as_usa_name { get { return _sold_as_usa_name; } set { _sold_as_usa_name = value; } }
		/// <summary>
		/// Provides the unit's name that this part is sold at in Canada
		/// </summary>
		public string sold_as_canadian_name { get { return _sold_as_canadian_name; } set { _sold_as_canadian_name = value; } }
		/// <summary>
		/// 
		/// </summary>
		public double qty { get; set; }

		public bool is_stocked { get; set; }

		public string location_name
		{
			get
			{
				return _location_name;
			}
			set
			{ _location_name = value; }
		}

		/// <summary>
		/// Provides the Number of units listed as being in inventory
		/// </summary>
		public double onhand_qty
		{
			get { return _onhand_qty; }
			set
			{
				if (_onhand_qty == value)
				{
					return;
				}
				_onhand_qty = value;
			}
		}
		public bool has_pic { get; set; }

		public double dollar_balance
		{
			get { return _dollar_balance; }
			set
			{
				if (_dollar_balance == value)
				{
					return;
				}
				_dollar_balance = value;
			}
		}
		/// Provides the part's tag name.
		/// </summary>
		public string tag_name { get; set; }

		/// <summary>
		/// Provides the part's tag id.
		/// </summary>
		public string tag_id { get; set; }

		/// <summary>
		/// This will tell you if this part is a QTY part or not
		/// </summary>
		public bool is_qty { get; set; }

		/// <summary>
		/// Only one price line can be a benchmark price 
		/// </summary>
		public bool is_benchmark { get; set; }

		/// <para>Provides the associated vendor ID the current cost price is set at.</para>
		/// <para>This will be built out to provide the vendor object in the future</para>
		/// </summary>
		public string vendor_id { get; set; }

		public string vendor_price_last_dt { get { return _vendor_price_last_dt; } set { _vendor_price_last_dt = value; } }
		/// <summary>
		/// Returns true if this part's tag is flagged as an excluded tag
		/// </summary>
		public bool is_exclude { get; set; }

		/// <summary>
		/// Returns true if this part's tag is flagged as to have a static sell price
		/// </summary>
		public bool is_static_sellprice { get; set; }

		public bool subcontract { get; set; }
		public bool shipping { get; set; }
		public bool others { get; set; }

		public inventory()
		{
			is_static_sellprice = false;
			has_pic = false;
			Tag = new tag();
			maxqty = 0;
			minqty = 0;
			cost_price_branch = 0;
			cost_price_branch_lowest = 0;
			cost_price_branch_highest = 0;
			global_highest = null;
			merged = false;
			part_is_loaded = false;
			active = false;
			ext_onhand_qty = 0;
			int_onhand_qty = 0;
			total_ext_onhand_qty = 0;
			total_int_onhand_qty = 0;
			po_usage = 0;
			wo_usage = 0;
			if (HttpContext.Current != null)
			{
				_session = HttpContext.Current.Session;
			}
		}

		/// <summary>
		/// Enables a part.
		/// </summary>
		/// <param name="_id"></param>
		public void SetPartActive(object _id)
		{
			Toolbox.doSQL_void("UPDATE inventory_item_master SET active = true WHERE master_id = @v0 LIMIT 1", new object[] { _id });
			Toolbox.doSQL_void(@"
		INSERT INTO inventory_description 
			(
			master_id, 
			description, 
			desc_short_cdn,
			desc_short_usa,
			desc_full_cdn, 
			desc_full_usa
			) 
		VALUES 
			(
			@v0, 
			full_part_description(@v0, true, ''),
			part_description(@v0, true, 'CDN'),
			part_description(@v0, true, 'USA'),
			full_part_description(@v0, true, 'CDN'),
			full_part_description(@v0, true, 'USA')			
			)", new object[] { _id });
			//DataTable _bvs		= NeShared.GetBV7DSNs();
			//foreach(DataRow _bvdr in _bvs.Rows)
			//	{
			//	string dsnbv7	= _bvdr["company_dsnbv7"].ToString();
			//	if(dsnbv7 != "")
			//		{
			//		Toolbox.doSQL_void(@"UPDATE inventory SET HOLD = 0 WHERE code = @v0 ", dsnbv7 , new object[] {  _id } );
			//		}
			//	}
		}
		/// <summary>
		/// Disables a part.
		/// </summary>
		/// <param name="_id"></param>
		public void SetPartInactive(object _id)
		{
			Toolbox.doSQL_void("UPDATE inventory_item_master SET active = false WHERE master_id = @v0 LIMIT 1", new object[] { _id });
			Toolbox.doSQL_void("DELETE FROM inventory_description WHERE master_id = @v0", new object[] { _id });
			//DataTable _bvs		= NeShared.GetBV7DSNs();
			//foreach(DataRow _bvdr in _bvs.Rows)
			//	{
			//	string dsnbv7	= _bvdr["company_dsnbv7"].ToString();
			//	if(dsnbv7 != "")
			//		{
			//		Toolbox.doSQL_void(@"UPDATE inventory SET HOLD = 1 WHERE code = @v0 ", dsnbv7 , new object[] {  _id } );
			//		}
			//	}
		}
		/// <summary>
		/// prints bar code label
		/// </summary>
		/// <param name="business_unit_id", master_id></param>
		public void print_barcode_label(int copies)
		{
			var current_ip = HttpContext.Current.Request.UserHostAddress;
			if (current_ip != "192.168.0.117")
			{
				var to_print = $"{master_id}|{business_unit_id}|{copies}";
				WebResponse resp;
				var req = HttpWebRequest.Create("http://print.nesi.ca/_tools/handlers/remote_barcode_print.ashx?r=" + to_print);
				resp = req.GetResponse();
				var rr = "";
				using (var sr = new System.IO.StreamReader(resp.GetResponseStream()))
				{
					rr = sr.ReadToEnd();
					sr.Close();
				}
				if (rr != "SUCCESS")
				{
					Toolbox.do_catch_error(new Exception(
						$"Tried to print {master_id}, for company id {business_unit_id}\nGot this reply \n-----------\n {rr}"), 1295);
				}
			}
			else
			{
				try
				{
					var printDoc = new PrintDocument();
					var WOPrinter = new NeBusinessUnit(business_unit_id);
					var yy = new PaperSize("Custom Paper Size", 220, 99);
					printDoc.DefaultPageSettings.PaperSize = yy;
					printDoc.DefaultPageSettings.Margins.Left = 1;
					printDoc.DefaultPageSettings.PrinterSettings.Copies = (short)copies;
					printDoc.PrinterSettings.PrinterName = WOPrinter.BarCodePrinter;
					printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
					printDoc.Print();
				}
				catch (Exception ee)
				{
					throw new Exception("Printing Barcode Failed - " + ee);
				}
			}
		}
		private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			var rightAlign = new StringFormat();
			rightAlign.Alignment = StringAlignment.Far;
			rightAlign.LineAlignment = StringAlignment.Far;
			var leftAlign = new StringFormat();
			leftAlign.Alignment = StringAlignment.Near;
			leftAlign.LineAlignment = StringAlignment.Near;
			var printFont = new Font("Arial", 9);
			var printFont1 = new Font("Arial", 8);
			var printFontdesc = new Font("Arial", 5);
			var barcodefont = new Font("Free 3 of 9 Extended", 24);
			var rect = new Rectangle(10, 30, 195, 50);
			var br = new SolidBrush(Color.Black);
			e.Graphics.DrawRectangle(Pens.Transparent, rect);
			e.Graphics.DrawString(master_id, printFont, br, 199, 30, rightAlign);
			e.Graphics.DrawString(_description_full, printFontdesc, br, rect);
			// TODO: Min/Max Are not based on the part #, we will need the location supplied in order to add these.
			// Commenting out for now.
			//e.Graphics.DrawString(minqty.ToString(), printFont1, br, 10, 71, leftAlign);
			//e.Graphics.DrawString(maxqty.ToString(), printFont1, br, 10, 85, leftAlign);
			e.Graphics.DrawString("*001-" + master_id + "*", barcodefont, br, 15, 5);
			get_location();
		}
		static public DataTable get_ext_locations(int _business_unit_id)
		{
			DataTable _dt;
			try
			{
				_dt = Toolbox.doSQL_dt(@"SELECT id,urldecode(name) as name FROM inventory_location_master WHERE type_id = 2 and business_unit_id = @v0  ORDER BY name", new object[] { _business_unit_id });
			}
			catch
			{
				_dt = new DataTable();
			}
			return _dt;
		}
		/// <summary>
		/// Toggles a part.
		/// </summary>
		/// <param name="_id"></param>
		public void TogglePart(object _id)
		{
			Toolbox.doSQL_void("UPDATE inventory_item_master SET active = 1 - active WHERE master_id = @v0 LIMIT 1", new object[] { _id });
			var _current = Toolbox.doSQL_int(@"SELECT active FROM inventory_item_master WHERE master_id = @v0", new object[] { _id });
			if (_current == 0)
			{
				Toolbox.doSQL_void("DELETE FROM inventory_description WHERE master_id = @v0", new object[] { _id });
			}
			else
			{
				Toolbox.doSQL_void("INSERT INTO inventory_description (master_id, description) VALUES (@v0, full_part_description(@v0, false, ''))", new object[] { _id });
			}
			//DataTable _bvs		= NeShared.GetBV7DSNs();
			//foreach(DataRow _bvdr in _bvs.Rows)
			//	{
			//	string dsnbv7	= _bvdr["company_dsnbv7"].ToString();
			//	if(dsnbv7 != "")
			//		{
			//		Toolbox.doSQL_void(@"UPDATE inventory SET HOLD = 1 - HOLD WHERE code = @v0 ", dsnbv7 , new object[] {  _id } );
			//		}
			//	}
		}
		/// <summary>
		/// <para>Starts a new inventory item</para>
		/// <para>When using this please be mindful of the attval organizing.</para>
		/// </summary>
		/// <param name="this_tag_id">The ID of the originating tag</param>
		/// <param name="this_attvals">Organized from least to greatest, commas in between each pair, no leading or trailing commas. <example>###,###,##,###</example></param>
		/// <param name="this_member_id"></param>
		/// <param name="this_active"></param>
		/// <returns><para>Returns Master ID or the reason why it failed.</para></returns>
		public string NewPart(string this_tag_id, string this_attvals, string this_member_id, bool this_active)
		{
			return NewPart(Convert.ToInt32(this_tag_id), this_attvals, Convert.ToInt32(this_member_id), this_active).ToString();
		}
		public string NewPart(int this_tag_id, string this_attvals, int this_member_id, bool this_active)
		{
			return Toolbox.doSQL_string(@"CALL inventory_newitem(@v0,@v1,@v2,@v3)",
				new object[] {
						this_tag_id, this_attvals, this_member_id, this_active});

		}

		/// <summary>
		/// Centralized Part Search DT
		/// </summary>
		/// <param name="query">The string that is being looked for</param>
		/// <param name="company">The company this is searching against</param>
		/// <param name="search_type">master_id, reference, description, vendor_code</param>
		/// <param name="allow_gl">Allow GL parts to be included</param>
		/// <param name="allow_nonstock">Allow Non-Stocks to be included</param>
		/// <returns></returns>
		public static DataTable search_results(object query, int _business_unit_id, string search_type, bool allow_gl, bool allow_nonstock, bool show_stocked)
		{
			var q = query.ToString().ToUpper().Trim();
			var exact_q = q;
			var appendature_my = "";
			var where_my = "AND ";
			var sql = "";
			var _dt = new DataTable();
			q = q.Replace(" ", ",");
			var or_matches = new Dictionary<int, string>();
			var q_items = new List<string>();
			if (q != "")
			{
				q = q.Replace(",", "|");
				#region appendature creation
				if (q.Contains("|") || q.Contains(","))
				{
					var qs = q.Contains(",") ? q.Split(',') : q.Split('|');
					q_items = new List<string>(qs);
					if (q_items.Contains("OR") && search_type == "description")
					{
						for (var o = 0; o < q_items.Count; o++)
						{
							// If they are trying to screw up the program by prefacing the search string with an "OR", remove it
							if (q_items[o] == "OR" && o == 0 || o + 1 > q_items.Count)
							{
								q_items[o] = "";
							}
							else if (q_items[o] == "OR" && o > 0)
							{
								var o_first = o - 1;
								var o_last = o + 1;
								var o_target = or_matches.Count == 0 ? 0 : or_matches.Count + 1;
								or_matches.Add(o_target, q_items[o_first] + "," + q_items[o_last]);
								q_items.RemoveAt(o_last);
								q_items.RemoveAt(o);
								q_items.RemoveAt(o_first);
							}
						}
					}
					q_items.RemoveAll(item => item == "" || item == null);
					for (var i = 0; i < q_items.Count; i++)
					{
						var this_val = q_items[i].Trim();
						if (this_val != "")
						{
							if (search_type == "description")
							{
								// Last one
								if (i == q_items.Count - 1)
								{
									where_my += $@"
												(
												b.description LIKE ""%{Toolbox.AddSlashes(this_val)}%""
												) ";
								}
								// normal one
								else
								{
									where_my += $@"
												(
												b.description LIKE ""%{Toolbox.AddSlashes(this_val)}%""
												) AND";
								}
							}
							else
							{
								// Last one
								if (i == q_items.Count - 1)
								{
									appendature_my += $@"
									_searchable LIKE ""%{this_val}%""";
								}
								// normal one
								else
								{
									appendature_my += $@"
									_searchable LIKE ""%{this_val}%"" AND";
								}
							}
						}
					}
					if (or_matches.Count > 1)
					{
						for (var om = 0; om < or_matches.Count; om++)
						{
							var and_preface = where_my == "" ? "" : "AND";
							var or_match = or_matches[om].Split(',');
							where_my += $@"
													{and_preface} (
														b.description LIKE ""% {Toolbox.do_value_to(or_match[0])}%,""%"" OR
														b.description LIKE ""% {Toolbox.do_value_to(or_match[1])}%,""%""
														)";
						}
					}
					else if (or_matches.Count == 1)
					{
						var and_preface = where_my == "" ? "" : "AND";
						var or_match = or_matches[0].Split(',');
						where_my += $@"
												{and_preface} (
													b.description LIKE ""% {Toolbox.do_value_to(or_match[0])}%,%"" OR
													b.description LIKE ""% {Toolbox.do_value_to(or_match[1])}%,%""
													)";
					}
				}
				else
				{
					// straight query
					where_my += $@"
								(
								b.description LIKE '%{Toolbox.AddSlashes(q)}%'
								) ";
				}
				#endregion
				#region MySQL DataTable
				var extra_where = "";
				switch (search_type)
				{
					case "master_id":
						var id_test = 0;
						int.TryParse(q, out id_test);
						if (id_test > 0)
						{
							extra_where += !allow_gl ? " AND a.tag_id != " + OpsSpecialTag.GL : "";
							extra_where += !allow_nonstock ? " AND a.tag_id != " + OpsSpecialTag.NonStockItem : "";
							extra_where += show_stocked ? " AND (ib.int_min > 0 OR ib.ext_min > 0)" : "";
							sql = string.Format(@"
							SELECT 
								a.master_id,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min_qty,
								IFNULL(ib.int_onhand_qty, 0)+IFNULL(ib.ext_onhand_qty, 0) onhand,
								IFNULL(ib.wo_usage, 0) wo_usage,
								IFNULL(ib.po_usage, 0) po_usage,
								d.description descript
							FROM
								inventory_item_master a, inventory_branch ib, inventory_description d, inventory_tag e
							WHERE 
								a.active = true AND 
								a.tag_id = e.tag_id AND
								e.include_search = 1 AND 
								a.master_id = ib.master_id AND 
								ib.business_unit_id = {2} AND
								a.master_id = d.master_id AND
								(a.master_id = '{0}' OR a.old_id = '{0}')
								{1}
							ORDER BY min DESC, po_usage desc 
							LIMIT 150
								",
								Toolbox.AddSlashes(q),
								extra_where,
								_business_unit_id
							);
						}
						else
						{
							return _dt;
						}
						break;

					case "reference":
						extra_where += !allow_gl ? " AND b.tag != " + OpsSpecialTag.GL : "";
						extra_where += !allow_nonstock ? " AND b.tag_id != " + OpsSpecialTag.NonStockItem : "";
						extra_where += show_stocked ? " AND (ib.int_min > 0 OR ib.ext_min > 0)" : "";
						sql = string.Format(@"
						SELECT 
							a.master_id,
							IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min,
							IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min_qty,
							IFNULL(ib.int_onhand_qty, 0)+IFNULL(ib.ext_onhand_qty, 0) onhand,
							IFNULL(ib.wo_usage, 0) wo_usage,
							IFNULL(ib.po_usage, 0) po_usage,
							d.description descript
						FROM
							inventory_reference a
						LEFT JOIN inventory_item_master b ON a.master_id = b.master_id
						INNER JOIN inventory_tag c ON b.tag_id = c.tag_id AND c.include_search = 1
						LEFT JOIN inventory_branch ib ON a.master_id = ib.master_id AND ib.business_unit_id = {2}
						LEFT JOIN inventory_description d ON a.master_id = d.master_id 
						WHERE 
							a.reference_part_number LIKE ""%{0}%"" AND 
							b.active = true
							{1}
						GROUP BY a.master_id
						ORDER BY min DESC, po_usage desc 
						LIMIT 150
							",
							Toolbox.AddSlashes(q),
							extra_where,
							_business_unit_id
						);
						break;

					case "description":
						extra_where += !allow_gl ? " AND a.tag_id != " + OpsSpecialTag.GL : "";
						extra_where += !allow_nonstock ? " AND a.tag_id != " + OpsSpecialTag.NonStockItem : "";
						extra_where += show_stocked ? " AND (ib.int_min > 0 OR ib.ext_min > 0)" : "";
						sql = string.Format(@"
						SELECT 
							a.master_id,
							IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min,
							IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min_qty,
							IFNULL(ib.int_onhand_qty, 0)+IFNULL(ib.ext_onhand_qty, 0) onhand,
							IFNULL(ib.wo_usage, 0) wo_usage,
							IFNULL(ib.po_usage, 0) po_usage,
							d.description descript
						FROM
							inventory_item_master a, inventory_description b, inventory_branch ib, inventory_description d, inventory_tag e 
						WHERE 
							a.master_id = b.master_id AND 
							a.master_id = ib.master_id AND 
							a.tag_id = e.tag_id AND
							e.include_search = 1 AND 
							ib.business_unit_id = {2} AND 
							a.master_id = d.master_id AND
							a.active = true
							{0}
							{1}
						ORDER BY min DESC, po_usage desc 
						LIMIT 100",
							where_my,
							extra_where,
							_business_unit_id
						);
						//_tools.debug_note(sql);
						break;
					case "vendor_code":
						extra_where += !allow_gl ? " AND b.tag_id != " + OpsSpecialTag.GL : "";
						extra_where += !allow_nonstock ? " AND b.tag_id != " + OpsSpecialTag.NonStockItem : "";
						extra_where += show_stocked ? " AND (ib.int_min > 0 OR ib.ext_min > 0)" : "";
						var quoted = new Regex("^[\'|\"].+[\'|\"]$");
						if (quoted.Match(exact_q).Success)
						{
							char[] quotes = { '\'', '"' };
							exact_q = HttpUtility.UrlDecode(exact_q).TrimStart(quotes);
							exact_q = exact_q.TrimEnd(quotes);
							sql = string.Format(@"
							SELECT 
								a.master_id,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min_qty,
								IFNULL(ib.int_onhand_qty, 0)+IFNULL(ib.ext_onhand_qty, 0) onhand,
								IFNULL(ib.wo_usage, 0) wo_usage,
								IFNULL(ib.po_usage, 0) po_usage,
								d.description descript
							FROM
								inventory_price a, inventory_item_master b, inventory_branch ib, inventory_description d , inventory_tag e
							WHERE 
								a.master_id = b.master_id AND
								b.active = true AND 
								b.tag_id = e.tag_id AND
								e.include_search = 1 AND 
								a.vendor_code = '{0}' AND
								a.master_id = ib.master_id AND 
								a.master_id = d.master_id AND
								ib.business_unit_id = {2}
								{1}

							GROUP BY master_id
							ORDER BY min DESC
							LIMIT 50

								",
								Toolbox.AddSlashes(exact_q),
								extra_where,
								_business_unit_id
							);
						}
						else
						{
							sql = string.Format(@"
							SELECT 
								a.master_id,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min,
								IFNULL(ib.int_min,0)+IFNULL(ib.ext_min, 0) min_qty,
								IFNULL(ib.int_onhand_qty, 0)+IFNULL(ib.ext_onhand_qty, 0) onhand,
								IFNULL(ib.wo_usage, 0) wo_usage,
								IFNULL(ib.po_usage, 0) po_usage,
								d.description descript
							FROM
								inventory_price a, inventory_item_master b, inventory_branch ib, inventory_description d , inventory_tag e
							WHERE 
								b.master_id = a.master_id AND 
								a.master_id = ib.master_id AND 
								b.tag_id = e.tag_id AND
								e.include_search = 1 AND 
								ib.business_unit_id = {2} AND
								b.active = true AND 
								a.vendor_code LIKE '%{0}%' AND
								a.master_id = d.master_id 
								{1}
							GROUP BY a.master_id

							ORDER BY ib.min_qty DESC
							LIMIT 50

								",
								Toolbox.AddSlashes(exact_q),
								extra_where,
								_business_unit_id
							);
						}
						break;
					default:
						throw new Exception("search type not declared");
				}
				_dt = Toolbox.doSQL_dt(sql, null);
				#endregion
			}
			return _dt;
		}

		/// <summary>
		/// search through parts database for shopping cart purposes
		/// </summary>
		public static DataTable search_results_sc(object query, NeBusinessUnit company, string search_type, bool allow_gl, bool allow_nonstock, bool show_stocked, bool show_common, string woprog_id, string location_master_id, string other_condition = "", bool matchwhole = false)
		{
			var q = query.ToString().ToUpper().Trim();
			var exact_q = q;
			var appendature_my = "";
			var where_my = "AND ";
			var sql = "";
			var businss_unit_id = company.warehouse_bu_id;
			var show_common_join = string.Format(@" INNER JOIN (SELECT DISTINCT master_id FROM common_used_master_id WHERE business_unit_id=" + businss_unit_id + @" ) AS h	ON h.master_id = e.master_id");

			var desc_country = new NeBusinessUnit(company.warehouse_bu_id).country == "USA" ? "usa" : "cdn";
			var _dt = new DataTable();
			q = q.Replace(" ", ",");
			var or_matches = new Dictionary<int, string>();
			var q_items = new List<string>();
			if (q != "")
			{
				q = q.Replace(",", "|");
				#region appendature creation
				if (q.Contains("|") || q.Contains(","))
				{
					var qs = q.Contains(",") ? q.Split(',') : q.Split('|');
					q_items = new List<string>(qs);
					if (q_items.Contains("OR") && search_type == "description")
					{
						for (var o = 0; o < q_items.Count; o++)
						{
							// If they are trying to screw up the program by prefacing the search string with an "OR", remove it
							if (q_items[o] == "OR" && o == 0 || o + 1 > q_items.Count)
							{
								q_items[o] = "";
							}
							else if (q_items[o] == "OR" && o > 0)
							{
								var o_first = o - 1;
								var o_last = o + 1;
								var o_target = or_matches.Count == 0 ? 0 : or_matches.Count + 1;
								or_matches.Add(o_target, q_items[o_first] + "," + q_items[o_last]);
								q_items.RemoveAt(o_last);
								q_items.RemoveAt(o);
								q_items.RemoveAt(o_first);
							}
						}
					}
					q_items.RemoveAll(item => string.IsNullOrEmpty(item));
					for (var i = 0; i < q_items.Count; i++)
					{
						var this_val = q_items[i].Trim();
						if (this_val != "")
						{
							if (search_type == "description")
							{

								if (i == q_items.Count - 1)// Last one
								{
									if (matchwhole)
									{
										where_my += string.Format(@"((REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""% {0} %"")
						or (REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""{0} %""))  ",
											Toolbox.AddSlashes(this_val));

										//where_my += string.Format(@"((d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{0}[[:>:]]"")or(d.desc_full_" + desc_country + @" REGEXP ""{0}[[:>:]]""))  ",
										//	Toolbox.AddSlashes(this_val));
									}
									else
									{

										where_my += string.Format(@"((d.desc_full_" + desc_country + @" LIKE ""%{0}%"")or(d.desc_full_" + desc_country + @" LIKE ""{0}%""))  ",
											Toolbox.AddSlashes(this_val));
									}
								}
								// normal one
								else
								{
									if (matchwhole)
									{
										where_my += string.Format(
											@"((REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""% {0} %"") 
											or (REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""{0} %"")) AND ",
											Toolbox.AddSlashes(this_val));

										//where_my += string.Format(
										//	@"((d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{0}[[:>:]]"")or(d.desc_full_" + desc_country +
										//	@" REGEXP ""{0}[[:>:]]""))  AND",
										//	Toolbox.AddSlashes(this_val));
									}
									else
									{
										where_my += string.Format(
											@"((d.desc_full_" + desc_country + @" LIKE ""%{0}%"")or(d.desc_full_" + desc_country +
											@" LIKE ""{0}%"")) AND", Toolbox.AddSlashes(this_val));
									}
								}
							}
							else
							{
								// Last one

								if (i == q_items.Count - 1)
								{
									if (matchwhole)
									{
										appendature_my += $@" REPLACE(_searchable,',',' ') LIKE ""% {this_val} %""";
										//appendature_my += $@" _searchable REGEXP ""[[:<:]]{this_val}[[:>:]]""";
									}
									else
									{
										appendature_my += $@" _searchable LIKE ""%{this_val}%""";
									}
								}
								// normal one
								else
								{
									if (matchwhole)
									{
										appendature_my += $@" REPLACE(_searchable,',',' ') LIKE ""% {this_val} %"" AND ";
										//appendature_my += $@" _searchable REGEXP ""[[:<:]]{this_val}[[:>:]]"" AND ";
									}
									else
									{
										appendature_my += $@" _searchable LIKE ""%{this_val}%"" AND ";
									}
								}
							}
						}
					}
					if (or_matches.Count > 1)
					{
						for (var om = 0; om < or_matches.Count; om++)
						{
							var and_preface = where_my == "" ? "" : "AND";
							var or_match = or_matches[om].Split(',');
							if (matchwhole)
							{
								where_my += string.Format(@"
													{0} (REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""% {1} %,""%"" or REPLACE(d.desc_full_"
														  + desc_country + @",',',' ') LIKE ""{1} %,""%"" OR REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""% {2} %,""%"" OR
														  REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""{2} %,""%"" )",
									and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));

								//where_my += string.Format(@"
								//					{0} (d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{1}[[:>:]],""%"" or d.desc_full_"
								//						  + desc_country + @" REGEXP ""{1}[[:>:]],""%"" OR d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{2}[[:>:]],""%"" OR d.desc_full_"
								//						  + desc_country + @" REGEXP ""{2}[[:>:]],""%"" )",
								//	and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));
							}
							else
							{
								where_my += string.Format(@"
													{0} (d.desc_full_" + desc_country + @" LIKE ""%{1}%,""%"" or d.desc_full_"
														  + desc_country + @" LIKE ""{1}%,""%"" OR d.desc_full_" + desc_country + @" LIKE ""%{2}%,""%"" OR d.desc_full_"
														  + desc_country + @" LIKE ""{2}%,""%"" )",
									and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));
							}



						}
					}
					else if (or_matches.Count == 1)
					{
						var and_preface = where_my == "" ? "" : "AND";
						var or_match = or_matches[0].Split(',');
						if (matchwhole)
						{
							where_my += string.Format(@"
												{0} (
													REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE ""% {1} %"" OR
													REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE "" {1} %"" OR
													REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE "" % {2} %"" OR
													REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE "" {2} %""
													)", and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));

							//where_my += string.Format(@"
							//					{0} (
							//						d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{1}[[:>:]],%"" OR
							//						d.desc_full_" + desc_country + @" REGEXP ""{1}[[:>:]],%"" OR
							//						d.desc_full_" + desc_country + @" REGEXP ""[[:<:]]{2}[[:>:]],%"" OR
							//						d.desc_full_" + desc_country + @" REGEXP ""{2}[[:>:]],%""
							//						)", and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));
						}
						else
						{
							where_my += string.Format(@"
												{0} (
													d.desc_full_" + desc_country + @" LIKE ""%{1}%,%"" OR
													d.desc_full_" + desc_country + @" LIKE ""{1}%,%"" OR
													d.desc_full_" + desc_country + @" LIKE ""%{2}%,%"" OR
													d.desc_full_" + desc_country + @" LIKE ""{2}%,%""
													)", and_preface, Toolbox.do_value_to(or_match[0]), Toolbox.do_value_to(or_match[1]));
						}

					}
				}
				else
				{
					if (matchwhole)
					{

						where_my += string.Format(@"
								(
								REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE '% {0} %' or REPLACE(d.desc_full_" + desc_country + @",',',' ') LIKE '{0} %'
								) ", Toolbox.AddSlashes(q));
						//// straight query
						//where_my += string.Format(@"
						//		(
						//		d.desc_full_" + desc_country + @" REGEXP '[[:<:]]{0}[[:>:]]' or d.desc_full_" + desc_country + @" REGEXP '{0}[[:>:]]'
						//		) ", Toolbox.AddSlashes(q));
					}
					else
					{
						// straight query
						where_my += string.Format(@"
								(
								d.desc_full_" + desc_country + @" LIKE '%{0}%' or d.desc_full_" + desc_country + @" LIKE '{0}%'
								) ", Toolbox.AddSlashes(q));
					}

				}
				#endregion
				#region MySQL DataTable
				var extra_where = "";
				switch (search_type)
				{
					case "master_id":
						var id_test = 0;
						int.TryParse(q, out id_test);
						if (id_test > 0)
						{
							extra_where += !allow_gl ? " AND e.tag_id != " + OpsSpecialTag.GL : "";
							extra_where += !allow_nonstock ? " AND e.tag_id != " + OpsSpecialTag.NonStockItem : "";
							extra_where += show_stocked ? " AND (g.int_min > 0 OR g.ext_min > 0)" : "";
							//extra_where += show_common ? "and h.wo_detail_history_date_added > (CURDATE()-interval 1 year)" : "";
							sql = string.Format(@"Select 
							f.tag_id,
f.tag,
a.master_id,
d.desc_full_" + desc_country + @" description,
c.attribute_id,
c.attribute,
b.`value`,
b.attribute_value_id,
IFNULL(g.min_qty, 0) min,
IFNULL(g.onhand_qty, 0) onhand,
IFNULL(g.int_onhand_qty, 0) int_onhand_qty,
IFNULL(g.ext_onhand_qty, 0) ext_onhand_qty,
" + other_condition + @"
g.ts last_used,
" + (location_master_id != "" ? @"(ifnull((Select il.qty from inventory_location il where il.location_master_id = " + location_master_id + @"  and il.master_id=a.master_id),0))" : "0") + @" qty_truck,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0))" : "0") + @" qty_cmt_on_wo,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_ordered-aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0))" : "0") + @"  qty_req_on_wo,
f.is_rental

FROM
inventory_item_detail AS a
INNER JOIN inventory_attribute_value AS b ON a.attribute_value_id = b.attribute_value_id
INNER JOIN inventory_attribute AS c ON b.attribute_id = c.attribute_id
INNER JOIN inventory_description AS d ON d.master_id = a.master_id
INNER JOIN inventory_item_master AS e ON e.master_id = a.master_id
INNER JOIN inventory_tag AS f ON e.tag_id = f.tag_id
{3}
INNER JOIN inventory_branch AS g ON e.master_id = g.master_id AND g.business_unit_id = " + company.warehouse_bu_id +
(other_condition != "" ? @" 
LEFT JOIN 
										inventory_cost p 
											ON a.master_id = p.master_id AND
											p.business_unit_id =g.business_unit_id" : "") +
												@"
							WHERE 
 f.active = 1 and 
								e.active = true AND 
								e.master_id = g.master_id AND 
								(e.master_id = '{0}' OR e.old_id = '{0}')
								{1}
							ORDER BY 
g.wo_usage 
desc, `min` DESC
								",
								Toolbox.AddSlashes(q),
								extra_where,
								company.id,
								"" // show_common?show_common_join:""
							);
						}
						else
						{
							return _dt;
						}
						break;

					case "description":
						extra_where += !allow_gl ? " AND e.tag_id != " + OpsSpecialTag.GL : "";
						extra_where += !allow_nonstock ? " AND e.tag_id != " + OpsSpecialTag.NonStockItem : "";
						extra_where += show_stocked ? " AND (g.int_min > 0 OR g.ext_min > 0)" : "";
						// extra_where += show_common ? " and h.wo_detail_history_date_added > (CURDATE()-interval 1 year)" : "";
						sql = string.Format(@"
						SELECT
f.tag_id,
f.tag,
a.master_id,
d.desc_full_" + desc_country + @" description,
c.attribute_id,
c.attribute,
b.`value`,
b.attribute_value_id,
IFNULL(g.min_qty, 0.0) min,
IFNULL(g.onhand_qty, 0) onhand,
IFNULL(g.int_onhand_qty, 0) int_onhand_qty,
IFNULL(g.ext_onhand_qty, 0) ext_onhand_qty,
" + other_condition + @"
g.ts last_used,
" + (location_master_id != "" ? @"(ifnull((Select il.qty from inventory_location il where il.location_master_id = " + location_master_id + @"  and il.master_id=a.master_id limit 1),0.0))" : "0.0") + @" qty_truck,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @" qty_cmt_on_wo,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_ordered-aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @"  qty_req_on_wo,
f.is_rental

FROM
inventory_item_detail AS a
INNER JOIN inventory_attribute_value AS b ON a.attribute_value_id = b.attribute_value_id
INNER JOIN inventory_attribute AS c ON b.attribute_id = c.attribute_id
INNER JOIN inventory_description AS d ON d.master_id = a.master_id
INNER JOIN inventory_item_master AS e ON e.master_id = a.master_id
INNER JOIN inventory_tag AS f ON e.tag_id = f.tag_id
{2}
INNER JOIN inventory_branch AS g ON e.master_id = g.master_id AND g.business_unit_id = " + company.warehouse_bu_id +
											(other_condition != "" ? @" 
LEFT JOIN 	inventory_cost p 
											ON a.master_id = p.master_id AND
											p.business_unit_id =g.business_unit_id" : "") + @"
WHERE 
 f.active = 1 and 
							e.active = true
							{0}
							{1}
							ORDER BY " + (other_condition != "" ? @"
IFNULL((SELECT SUM(wo_detail_history_qty_committed) FROM wo_detail_history WHERE wo_detail_history_master_id=a.master_id),0) 
" : @"
g.wo_usage ") + @"
desc, `min` DESC
",
							where_my,
							extra_where,
							show_common == true ? show_common_join : ""
						);
						// _tools.debug_note(sql);
						break;
					case "vendor_code":
						extra_where += !allow_gl ? " AND e.tag_id != " + OpsSpecialTag.GL : "";
						extra_where += !allow_nonstock ? " AND e.tag_id != " + OpsSpecialTag.NonStockItem : "";


						//		Regex quoted = new Regex("^[\'|\"].+[\'|\"]$");
						//		if (quoted.Match(exact_q).Success)
						//		{
						char[] quotes = { '\'', '"' };
						exact_q = HttpUtility.UrlDecode(exact_q).TrimStart(quotes);
						exact_q = exact_q.TrimEnd(quotes);
						sql = string.Format(@"Select 
f.tag_id,
f.tag,
a.master_id,
d.desc_full_" + desc_country + @" description,
c.attribute_id,
c.attribute,
b.`value`,
b.attribute_value_id,
IFNULL(g.min_qty, 0) min,
IFNULL(g.onhand_qty, 0) onhand,
IFNULL(g.int_onhand_qty, 0) int_onhand_qty,
IFNULL(g.ext_onhand_qty, 0) ext_onhand_qty,
" + other_condition + @"
g.ts last_used,
" + (location_master_id != "" ? @"(ifnull((Select il.qty from inventory_location il where il.location_master_id = " + location_master_id + @"  and il.master_id=a.master_id),0))" : "0") + @" qty_truck,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0))" : "0") + @" qty_cmt_on_wo,
" + (woprog_id != "" ? "(select ifnull((Select aa.wo_detail_current_qty_ordered-aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0))" : "0") + @"  qty_req_on_wo,
f.is_rental

FROM
inventory_item_detail AS a

INNER JOIN inventory_attribute_value AS b ON a.attribute_value_id = b.attribute_value_id
INNER JOIN inventory_attribute AS c ON b.attribute_id = c.attribute_id
INNER JOIN inventory_description AS d ON d.master_id = a.master_id
INNER JOIN inventory_item_master AS e ON e.master_id = a.master_id
INNER JOIN inventory_tag AS f ON e.tag_id = f.tag_id
{3}
INNER JOIN inventory_branch AS g ON e.master_id = g.master_id AND g.business_unit_id = " + company.warehouse_bu_id +
											(other_condition != "" ? @" 
LEFT JOIN 
										inventory_cost p 
											ON a.master_id = p.master_id AND
											p.business_unit_id =g.business_unit_id" : "") + @"
INNER JOIN inventory_price ip on e.master_id = ip.master_id
							WHERE 
 f.active = 1 and 
								a.master_id = e.master_id AND
								e.active = true AND 
								ip.vendor_code like '%{0}%' 
								{1}
							group by a.master_id,a.attribute_value_id
							ORDER BY " + (other_condition != "" ? @"
IFNULL((SELECT SUM(wo_detail_history_qty_committed) FROM wo_detail_history WHERE wo_detail_history_master_id=a.master_id),0) 
" : @"
g.wo_usage ") + @"
desc, `min` DESC
",
							Toolbox.AddSlashes(exact_q),
							extra_where,
							company.id,
							""
						);

						break;
					default:
						throw new Exception("search type not declared");
				}

				_dt = Toolbox.doSQL_dt(sql, null);
				#endregion
			}
			return _dt;
		}

		/// <summary>
		/// Deletes a location from supplied branch
		/// </summary>
		public void delete_location()
		{
			if (_location_id != null)
			{
				Toolbox.doSQL_void("DELETE FROM inventory_location WHERE id = @v0 LIMIT 1", new object[] { _location_id });
			}
		}
		/// <summary>
		/// <para>Gets the currently set location.</para>
		/// <para>Returns blank string if not currently set.</para>
		/// </summary>
		private void get_location()
		{
			if (master_id != "")
			{
				_location_name = Toolbox.doSQL_string(@"select ifnull((SELECT ilm.name FROM inventory_location inner join inventory_location_master ilm 
on ilm.id=inventory_location.location_master_id 
WHERE master_id = @v0
AND ilm.business_unit_id = @v1 
and ilm.type_id=1 order by qty desc,min desc,max desc limit 1),'')",
					new object[] {
							master_id, business_unit_id});
			}
			else
			{
				throw new Exception("Master ID & Company ID not provided; Location failed");
			}
		}
		/// <summary>
		/// Checks the existance of a specified part number
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public bool part_exists(object _id)
		{
			var count = Toolbox.doSQL_int(@"SELECT IFNULL(COUNT(*), 0) C FROM inventory_item_master WHERE master_id = @v0", new object[] { _id });
			return count > 0;
		}

		public bool part_is_Rental_Item(object _id)
		{
			if (_id == null)
			{
				return false;
			}

			var rental = false;
			try
			{
				int v = Toolbox.doSQL_int(
					@"SELECT IFNULL(b.is_rental, 0) FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id = @v0",
					new object[] { _id });
				return v == 1;
			}
			catch (Exception ex)
			{
				// when there is no record, we still return false by rental variable.
			}
			finally
			{
			}

			return rental;
		}

		/// <summary>
		/// Checks the existance of a part and tag combination
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_tag"></param>
		/// <returns></returns>
		public bool part_exists(object _id, object _tag)
		{
			var count = Toolbox.doSQL_int(@"SELECT IFNULL(COUNT(*), 0) C FROM inventory_item_master WHERE master_id = @v0 AND tag_id = @v1",
				new object[] { _id, _tag });
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public void save_reference(object this_id, object _business_unit_id, object this_reference)
		{
			try
			{
				if (_session != null && _session["session"] != null)
				{
					current_user = new NeMember(_session["session"].ToString());
				}
				var ref_count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_reference WHERE referencetype_id = 1 AND typereference_id = @v0 AND master_id = @v1",
					new object[] {
							_business_unit_id, this_id});
				if (ref_count == 0)
				{
					Toolbox.doSQL_void(@"INSERT INTO inventory_reference 
			(reference_part_number, master_id, active, ref_member_id, ref_business_unit_id) 
		VALUES (@v2, @v0,  true, @v3, @v1)", new object[] {
							this_id, _business_unit_id, this_reference, current_user.id});
				}
				else if (ref_count > 0)
				{
					Toolbox.doSQL_void(
						@"UPDATE inventory_reference SET reference_part_number = @v0, ref_member_id = @v3 WHERE ref_business_unit_id = @v1 AND master_id = @v2",
						new object[]
						{
					this_reference, _business_unit_id, this_id, current_user.id
						});
				}
			}
			catch (Exception ee)
			{
				throw ee;
			}
		}
		/// <summary>
		/// <para>Merges an inventory part from one master_id to the other</para>
		/// <para>Effected modules</para>
		/// <para></para>
		/// </summary>
		/// <param name="from_master_id"></param>
		/// <param name="to_master_id"></param>
		public void merge_part(string from_master_id, string to_master_id)
		{
			return;
			if (_session != null && _session["session"] != null && current_user == null)
			{
				current_user = new NeMember(_session["session"].ToString());
			}
			if (part_exists(from_master_id) && part_exists(to_master_id)) // Exists
			{
				if (from_master_id == to_master_id)
				{
					throw new Exception($"Cannot merge {from_master_id} into {to_master_id}, they are the same part.");
				}


				#region Disable old part number
				SetPartInactive(from_master_id);
				#endregion Disable old part number
				#region Move Prices
				var from_prices = Toolbox.doSQL_dt(@"SELECT * FROM inventory_price WHERE master_id = @v0  ORDER BY business_unit_id", new object[] { from_master_id });
				Toolbox.doSQL_void("DELETE FROM inventory_price WHERE master_id = @v0", new object[] { from_master_id });
				double used_cost = 0;
				double used_total = 0;
				double used_qty = 0;
				var last_business_unit_id = 0;
				if (from_prices.Rows.Count > 0)
				{
					#region current_pricing
					foreach (DataRow from_row in from_prices.Rows)
					{
						var from_price_id = from_row["id"].ToString();
						var from_vendor_id = from_row["vendor_id"].ToString();
						var from_edited_dt = DateTime.Parse(from_row["edited_dt"].ToString());
						var from_business_unit_id = (int)from_row["business_unit_id"];
						var from_company = new NeBusinessUnit(from_business_unit_id);
						var from_member_id = Convert.ToInt32(from_row["member_id"]);
						var from_vendor_code = from_row["vendor_code"].ToString();
						var from_cost = Convert.ToDouble(from_row["cost"]);
						var from_total = Convert.ToDouble(from_row["total"]);
						var from_qty = Convert.ToDouble(from_row["qty"]);
						var update_row = false;
						var update_price_id = "";
						// Grab rows that might match this combination of vendor & master id
						var to_prices = Toolbox.doSQL_dt(@"SELECT * FROM inventory_price WHERE master_id = @v0  AND vendor_id = @v1  AND vendor_code = @v2  AND business_unit_id = @v3  LIMIT 1", new object[] { to_master_id, from_vendor_id, from_vendor_code, from_business_unit_id });
						if (to_prices.Rows.Count > 0)
						{
							// If there are matches, figure out which is more recent and use it.
							foreach (DataRow to_row in to_prices.Rows)
							{
								var to_cost = Convert.ToDouble(to_row["cost"]);
								var to_total = Convert.ToDouble(to_row["total"]);
								var to_qty = Convert.ToDouble(to_row["qty"]);
								var to_edited_dt = DateTime.Parse(to_row["edited_dt"].ToString());
								var compared_dt = to_edited_dt.CompareTo(from_edited_dt);
								if (compared_dt > 0)
								{
									update_price_id = to_row["id"].ToString();
									used_cost = to_cost;
									used_total = to_total;
									used_qty = to_qty;
									update_row = true;
								}
								else if (compared_dt < 0)
								{
									update_price_id = to_row["id"].ToString();
									used_cost = from_cost;
									used_total = from_total;
									used_qty = from_qty;
									update_row = true;
								}
							}
						} // End new row check
						if (used_cost == 0)
						{
							used_cost = from_cost;
						}
						if (used_qty == 0)
						{
							used_qty = from_qty;
						}
						if (used_total == 0)
						{
							used_total = from_total;
						}
						var pr = new vendor_price_row();
						if (update_price_id != "" && update_row)
						{
							pr.price_id = update_price_id;
						}
						pr.master_id = to_master_id;
						pr.vendor_code = from_vendor_code.Trim();
						pr.vendor_id = from_vendor_id;
						pr.cost = used_cost;
						pr.member_id = from_member_id;
						pr.qty = used_qty;
						pr.total = used_total;
						pr.business_unit_id = from_business_unit_id;
						pr.origin = $"Merged from Part #{from_master_id}";
						pr.save();
						used_cost = 0;
						used_qty = 0;
						used_total = 0;
					}
					#endregion current_pricing
				}
				var from_price_history = Toolbox.doSQL_dt(@"SELECT * FROM inventory_price_history WHERE master_id = @v0  ORDER BY business_unit_id", new object[] { from_master_id });
				if (from_price_history.Rows.Count > 0)
				{
					#region historic pricing
					foreach (DataRow _history in from_price_history.Rows)
					{
						var prh = new vendor_price_row();
						prh.master_id = to_master_id;
						prh.vendor_code = _history["vendor_code"];
						prh.member_id = Convert.ToInt32(_history["member_id"]);
						prh.business_unit_id = Convert.ToInt32(_history["business_unit_id"]);
						prh.cost = _history["cost"];
						prh.total = _history["total"];
						prh.qty = _history["qty"];
						prh.vendor_id = _history["vendor_id"];
						prh.origin = $"({_history["origin"]}) - Merged From Part #{from_master_id}";
						prh.save_history();
					}
					#endregion historic pricing
				}
				#endregion Move Prices
				#region non-invoiced work orders
				var _dt = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current WHERE wo_detail_current_code = @v0 ", new object[] { from_master_id });
				if (_dt.Rows.Count > 0)
				{
					foreach (DataRow _dr in _dt.Rows)
					{
						var row_id = _dr["wo_detail_current_id"].ToString();
						var row_business_unit_id = _dr["business_unit_id"].ToString();
						var row_country = Toolbox.doSQL_string(@"SELECT country FROM business_unit WHERE id = @v0 ", new object[] { row_business_unit_id });
						Toolbox.doSQL_void(@" UPDATE wo_detail_current SET wo_detail_current_code = @v1 , wo_detail_current_master_id = @v1 , wo_detail_current_description = part_description(@v1 , false, @v2 ) WHERE wo_detail_current_id = @v0  LIMIT 1", new object[] { row_id, to_master_id, row_country });
					}
				}
				Toolbox.doSQL_void(@"UPDATE po_details_current SET po_details_part_no = @v0  WHERE po_details_part_no = @v1 ", new object[] { to_master_id, from_master_id });
				#endregion
				#region update old_id
				Toolbox.doSQL_void(@"UPDATE inventory_item_master SET old_id = @v0  WHERE master_id = @v1  LIMIT 1", new object[] { from_master_id, to_master_id });
				#endregion
				#region update new_id's
				Toolbox.doSQL_void(@"UPDATE inventory_item_master SET new_id = @v0  WHERE master_id = @v1  LIMIT 1", new object[] { to_master_id, from_master_id });
				Toolbox.doSQL_void(@"UPDATE inventory_item_master SET new_id = NULL WHERE master_id = @v0  LIMIT 1", new object[] { to_master_id });
				#endregion
				#region barcodes
				var bcs = Toolbox.doSQL_dt(@"SELECT * FROM inventory_barcode  WHERE master_id =@v0", new object[] { from_master_id });
				if (bcs.Rows.Count > 0)
				{
					foreach (DataRow bc in bcs.Rows)
					{
						var bc_id = Convert.ToInt32(bc["id"]);
						var new_bc = new barcode();
						var old_bc = new barcode(bc_id);
						Toolbox.doSQL_void(@"DELETE FROM inventory_barcode  WHERE ID =@v0 limit 1 ", new object[] { bc_id });
						new_bc.table_id = old_bc.table_id;
						new_bc.table_type = old_bc.table_type;
						new_bc.ref_name = old_bc.ref_name;
						new_bc.member_id_added = old_bc.member_id_added;
						new_bc.dt_created = Toolbox.MySQLNow_long();
						new_bc.barcode_no = old_bc.barcode_no;
						new_bc.is_active = old_bc.is_active;
						new_bc.master_id = Convert.ToInt32(from_master_id);
						new_bc.Save();
					}
				}
				#endregion barcodes
				#region inventory_branch
				var i_branches = Toolbox.doSQL_dt(@"SELECT * FROM inventory_branch  WHERE master_id =@v0", new object[] { from_master_id });
				if (i_branches.Rows.Count > 0)
				{
					foreach (DataRow i_branch in i_branches.Rows)
					{
						var i_business_unit_id = Convert.ToInt32(i_branch["business_unit_id"]);
						var old_i_b = new branch(from_master_id, i_business_unit_id);
						var new_i_b = new branch();
						try
						{
							new_i_b.load(Convert.ToInt32(to_master_id), i_business_unit_id);
							new_i_b.onhand_qty += old_i_b.onhand_qty;
							new_i_b.dollar_balance += old_i_b.dollar_balance;
							new_i_b.save();
						}
						catch
						{
							new_i_b.business_unit_id = old_i_b.business_unit_id;
							new_i_b.master_id = Convert.ToInt32(to_master_id);
							new_i_b.max_qty = old_i_b.max_qty;
							new_i_b.dollar_balance = old_i_b.dollar_balance;
							new_i_b.member_id = old_i_b.member_id;
							new_i_b.min_qty = old_i_b.min_qty;
							new_i_b.onhand_qty = old_i_b.onhand_qty;
							new_i_b.save();
						}
						old_i_b.dollar_balance = 0;
						old_i_b.onhand_qty = 0;
						old_i_b.save();
					}
				}
				#endregion inventory_branch
				#region inventory_location
				var i_locations = Toolbox.doSQL_dt(@"SELECT * FROM inventory_location  WHERE master_id =@v0", new object[] { from_master_id });
				if (i_locations.Rows.Count > 0)
				{
					foreach (DataRow i_location in i_locations.Rows)
					{
						try
						{
							var l_id = Convert.ToInt32(i_location["id"]);
							var location_master_id = Convert.ToInt32(i_location["location_master_id"]);
							var business_unit_id = (int)i_location["business_unit_id"];
							var qty = Convert.ToDouble(i_location["qty"]);
							var min = Convert.ToDouble(i_location["min"]);
							var max = Convert.ToDouble(i_location["max"]);
							var il_old = new location(l_id);
							var il_new = new location(location_master_id, business_unit_id, Convert.ToInt32(to_master_id));
							il_new.qty += qty;
							il_new.member_id = 14;
							il_new.section_id = 14;
							il_new.min = il_new.min == 0 && il_old.min != 0 ? il_old.min : il_new.min;
							il_new.max = il_new.max == 0 && il_old.max != 0 ? il_old.max : il_new.max;
							il_new.save();
							if (il_old.qty != 0)
							{
								il_old.qty = 0;
								il_old.section_id = 14;
								il_old.member_id = 14;
								il_old.save();
							}
							il_old.delete(true);
						}
						catch (Exception ee)
						{
						}
					}
				}
				#endregion inventory_location
				#region groups
				Toolbox.doSQL_void(@"UPDATE inventory_group_dtl SET master_id = @v0  WHERE master_id = @v1 ", new object[] { to_master_id, from_master_id });
				#endregion groups
				#region kits
				Toolbox.doSQL_void(@"UPDATE inventory_kit_dtl SET inventory_kit_dtl_master_id = @v0  WHERE inventory_kit_dtl_master_id = @v1 ", new object[] { to_master_id, from_master_id });
				#endregion kits
				#region inventory_consignment
				_dt = Toolbox.doSQL_dt(@"SELECT * FROM inventory_consignment  WHERE master_id =@v0", new object[] { from_master_id });
				foreach (DataRow _dr in _dt.Rows)
				{
					var i = new consignment(Convert.ToInt32(_dr["id"]));
					i.master_id = Convert.ToInt32(to_master_id);
					i.save();
				}
				#endregion inventory_consignment
				#region Create barcode reference for old part number
				var bco = new barcode();
				bco.master_id = Convert.ToInt32(to_master_id);
				bco.barcode_no = from_master_id;
				bco.table_type = "O";
				bco.member_id_added = current_user.id;
				bco.table_id = Convert.ToInt32(to_master_id);
				bco.is_active = true;
				bco.Save();
				#endregion Create barcode reference for old part number
				#region Delete old description
				Toolbox.doSQL_void(@"DELETE FROM inventory_description WHERE master_id = @v0 ", new object[] { from_master_id });
				#endregion Delete old description
				#region Update Branch locations
				foreach (DataRow dr in i_branches.Rows)
				{
					var i_business_unit_id = dr["business_unit_id"];
					var loc = new location();
					loc.update_branch_locations(from_master_id, i_business_unit_id);
					loc.update_branch_locations(to_master_id, i_business_unit_id);
				}
				#endregion Update Branch locations
			}
			else
			{
				// Doesn't Exist
				throw new Exception($"{from_master_id} or {to_master_id}, does not exist. Merge Failed.");
			}
		}
		/// <summary>
		/// Used for getting a tag's name based on the supplied tag_id
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>	
		public string TagName(object _id)
		{
			return Toolbox.doSQL_string(@"SELECT tag FROM inventory_tag WHERE tag_id = @v0 ", new object[] { _id });
		}

		public ArrayList parts { get; set; }
		public void Load(object _id, string _business_unit_id)
		{
			Load(_id, Convert.ToInt32(_business_unit_id), false);
		}
		public void Load(object _id, int _business_unit_id)
		{
			Load(_id, _business_unit_id, false);
		}


		public string GetBarCodeHTML(string master_id)
		{
			string barcode = "*001-" + master_id + "*";
			string Render_Html = @"<html><head><link href ='https://fonts.googleapis.com/css?family=Libre+Barcode+39+Text&display=swap' rel ='stylesheet'>
                             <style>.barcode {font-family: 'Libre Barcode 39 Text', cursive; font-size:100px; }</style></head><body><div class='barcode'>" + barcode + "</div></body></html>";
			return Render_Html;

		}

		/// <summary>
		/// This one is called when calling from po/wo to add a new line item.
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_business_unit_id"></param>
		/// <param name="origin"></param>
		public void Load(object _id, int _business_unit_id, string origin)
		{
			var try_part_number = 0;
			if (!int.TryParse(_id.ToString(), out try_part_number))
			{
				// If can't be parsed, call previous load version to hanlde it.
				this.Load(_id, _business_unit_id);
				return;
			}

			if (string.IsNullOrWhiteSpace(origin) || origin.ToLower() != "workorder")
			{
				if (!string.IsNullOrWhiteSpace(origin) && origin.ToLower() == "purchaseorder")
				{
					if (try_part_number == OpsSpecialPart.CompanyCreditCardExpense ||
						try_part_number == OpsSpecialPart.ExpenseReimbursement ||
						try_part_number == OpsSpecialPart.PerDiem ||
						try_part_number == OpsSpecialPart.MiscMaterial ||
						try_part_number == OpsSpecialPart.NewChildWO)
					{
						// from PO, we can't allow to add above three things. by the way they also can't be added from wo due to excluded tag property.
						throw new Exception("Part Cannot Be Added To Purchase Order - " + try_part_number.ToString());
					}
				}

				// The origin is empty or not from work order page, so we call the old function.
				this.Load(_id, _business_unit_id);
				return;
			}

			/*
             •	For all Ops users, lines cannot be added to a work order that have a cost but do not have an origin
                o	Any parts that are under a tag* denoted as is_exclude cannot be added directly to a work order, and must derive from a PO.
                o	*Tags: SELECT * FROM inventory_tag WHERE is_exclude = 1 AND active = 1 AND approved = 1;(new constrains)
             */

			// Now we have a part ID, and also calling origin from work order.
			if (!this.IsPartFromAnExcludedTag(try_part_number) ||
				try_part_number == OpsSpecialPart.MiscMaterial ||
				try_part_number == OpsSpecialPart.NewChildWO)
			{
				throw new Exception("Part Cannot Be Added To Workorder - " + try_part_number.ToString());
			}

			//
			// Call to load data if pass the checks from here.
			//
			this.Load(_id, _business_unit_id);
		}
		/// <summary>
		/// Feed this the master ID of the inventory part and it will auto-populate the necessary fields.
		/// </summary>
		/// <param name="_id">The Master ID of the inventory part</param>
		/// <param name="_business_unit_id">The company id that is accessing this inventory.</param>
		public void Load(object _id, int _business_unit_id, bool _load_list)
		{
			using (var conn = Toolbox.connect())
			{
				Toolbox.do_debug("start - inventory Load (" + _id + "," + _business_unit_id + ")");
				var try_part_number = 0;
				if (!_load_list)
				{
					int.TryParse(_id.ToString(), out try_part_number);
					if (try_part_number > 900000)
					{
						return;
					}
				}
				if (_session != null && _session["session"] != null && current_user == null && !_load_list)
				{
					current_user = new NeMember(_session["session"].ToString());
				}
				if (!_load_list && !part_exists(_id))
				{
					throw new Exception("Part Does Not Exist - " + _id);
				}
				else if (!_load_list)
				{
					#region Is not a list
					var _merged_part = check_merged(Convert.ToInt32(_id));
					if (_merged_part[0] > 0 && _merged_part[1] > 0)
					{
						_id = _merged_part[1];
						merged = true;
					}
					master_id = Convert.ToString(_id);
					id = Convert.ToInt32(_id);
					this.business_unit_id = _business_unit_id;
					var _country = Toolbox.doSQL_string(@"Select ifnull((SELECT country from business_unit WHERE id = @v0  LIMIT 1),'CDN')", new object[] { this.business_unit_id });
					DataTable _dt;
					try
					{
						#region DataTable retrieval

						_dt = Toolbox.doSQL_dt($@"
	SELECT 
		a.master_id,
		IFNULL(a.active, 0) active,
		CAST(IFNULL((Select p.po_details_date_added from po_details_current p INNER join poprog_header po on p.po_details_poprog_id = po.poprog_id and p.business_unit_id = @v1 where po_details_part_no = @v0 order by p.po_details_date_added desc limit 1)  , '--') AS CHAR(20)) vendor_price_last_dt,
		a.tag_id,
		e.tag,
		e.canadian_sold_as,
		a.old_id,
		a.new_id,
		e.usa_sold_as,
		f.unit canadian_sold_as_name,
		g.unit usa_sold_as_name,
		e.is_qty,
		e.is_exclude,
		e.is_static_sellprice,
		IFNULL(b.benchmark, 0) benchmark,
		IFNULL(b.vendor_id, '0') vendor_id,
		FORMAT(IFNULL(b.cost, 0), 5) cost,
		IFNULL(c.sellprice, 0) sellprice,
		IFNULL(b.qty, 1) qty,
		k.description description,
		k.desc_full_{_country} description_full,
		IFNULL(h.onhand_qty, 0) onhand_qty,
		IFNULL(h.dollar_balance, 0) dollar_balance,
		COUNT(i.id) n_pics,
		(SELECT COUNT(id) FROM inventory_location WHERE master_id = a.master_id AND business_unit_id = @v1 AND min > 0 AND max > 0) n_minmax,
		(SELECT count(*) FROM inventory_location WHERE master_id = @v0 AND business_unit_id = @v1 AND (int_min > 0 OR ext_min > 0)) > 0 is_stocked,
e.allowed_to_stock,
h.ext_onhand_qty,
h.int_onhand_qty,
(SELECT SUM(inb.ext_onhand_qty)  FROM inventory_branch inb INNER JOIN business_unit bu ON bu.ID = inb.business_unit_id AND bu.has_inventory = 1 WHERE inb.master_id = a.master_id AND bu.netsuite_bu_internal_id > 0 AND bu.active = 'T' AND bu.id NOT IN (8, 11,@v1)) AS total_ext_onhand_qty,
(SELECT SUM(inb.int_onhand_qty)FROM inventory_branch inb INNER JOIN business_unit bu ON bu.ID = inb.business_unit_id AND bu.has_inventory = 1 WHERE inb.master_id = a.master_id AND bu.netsuite_bu_internal_id > 0 AND bu.active = 'T' AND bu.id NOT IN (8, 11, @v1)) AS total_int_onhand_qty,
h.wo_usage wo_usage,
h.po_usage po_usage,
e.allowed_to_edit_after_issue,
e.is_subcontractor,
e.is_shipping,
e.is_other
	FROM 
		inventory_item_master a 
	LEFT JOIN 
		inventory_price b 
			ON a.master_id = b.master_id AND
			b.business_unit_id = @v1
	LEFT JOIN 
		inventory_sellprice c 
			ON a.master_id = c.master_id AND
			c.business_unit_id = @v1
	LEFT JOIN
		inventory_tag e
			ON e.tag_id = a.tag_id
	LEFT JOIN
		inventory_sold_as f
			ON e.canadian_sold_as = f.id
	LEFT JOIN
		inventory_sold_as g
			ON e.usa_sold_as = g.id
	LEFT JOIN
		inventory_branch as h
			ON a.master_id = h.master_id AND
			h.business_unit_id = @v1
	LEFT JOIN
		inventory_picture i
			ON a.master_id = i.master_id
	LEFT JOIN
		inventory_price_history j ON
			j.master_id = a.master_id AND
			j.business_unit_id = @v1
	LEFT JOIN
		inventory_description k ON
			k.master_id = a.master_id

	WHERE 
		a.master_id = @v0 
	ORDER BY b.cost limit 1", new object[] { master_id, this.business_unit_id });
						#endregion
					}
					catch (Exception ee)
					{
						throw new Exception("Could not load supplied part: " + _id + " " + ee);
					}
					foreach (DataRow _dr in _dt.Rows)
					{
						#region Active State Assignment
						try
						{
							active = Convert.ToBoolean(_dr["active"]);
						}
						catch
						{
							active = false;
						}
						#endregion
						#region Benchmark Assignment
						try
						{
							is_benchmark = Convert.ToBoolean(_dr["benchmark"]);
						}
						catch
						{
							is_benchmark = false;
						}
						#endregion
						old_id = _dr["old_id"].ToString();
						new_id = _dr["new_id"].ToString();
						has_pic = Convert.ToInt32(_dr["n_pics"]) > 0;
						allowed_to_stock = Convert.ToBoolean(_dr["allowed_to_stock"]);
						allowed_to_edit_after_issue = Convert.ToBoolean(_dr["allowed_to_edit_after_issue"]);
						n_minmax = Convert.ToDouble(_dr["n_minmax"]);
						int_onhand_qty = _dr["int_onhand_qty"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["int_onhand_qty"]);
						ext_onhand_qty = _dr["ext_onhand_qty"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["ext_onhand_qty"]);
						total_int_onhand_qty = _dr["total_int_onhand_qty"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["total_int_onhand_qty"]);
						total_ext_onhand_qty = _dr["total_ext_onhand_qty"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["total_ext_onhand_qty"]);
						wo_usage = _dr["wo_usage"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["wo_usage"]);
						po_usage = _dr["po_usage"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["po_usage"]);
						#region Part Description Assignment
						_description = _dr["description"].ToString();
						_description_full = _dr["description_full"].ToString();
						if (_description.Length > 80)
						{
							_description_int = _description_full;
							_description_short = _description.Substring(0, 77) + "...";
						}
						else
						{
							_description_int = _description_full;
							_description_short = _description;
						}
						#endregion
						#region Sold As - Canada
						_sold_as_canadian = _dr["canadian_sold_as"].ToString();
						_sold_as_canadian_name = _dr["canadian_sold_as_name"].ToString();
						#endregion
						#region Sold As - U.S.A
						_sold_as_usa = _dr["usa_sold_as"].ToString();
						_sold_as_usa_name = _dr["usa_sold_as_name"].ToString();
						#endregion
						#region Tag Name Assignment
						try
						{
							tag_name = _dr["tag"].ToString();
						}
						catch
						{
							tag_name = "ERROR";
						}
						#endregion
						#region Force Cost to 0.01 Assignment
						try
						{
							is_exclude = Convert.ToBoolean(_dr["is_exclude"]);
						}
						catch
						{
							is_exclude = false;
						}
						#endregion
						#region Is Static Sell Price Assignment
						try
						{
							is_static_sellprice = false;
						}
						catch
						{
							is_static_sellprice = false;
						}
						#endregion
						#region Cost Price Assignment
						get_costs(master_id, this.business_unit_id);
						#endregion

						subcontract = false;
						shipping = false;
						others = false;

						if (!(_dr["is_subcontractor"] == DBNull.Value))
						{
							subcontract = Convert.ToBoolean(_dr["is_subcontractor"]);
						}

						if (!(_dr["is_shipping"] == DBNull.Value))
						{
							shipping = Convert.ToBoolean(_dr["is_shipping"]);
						}

						if (!(_dr["is_other"] == DBNull.Value))
						{
							others = Convert.ToBoolean(_dr["is_other"]);
						}

						#region Tag ID Assignment
						try
						{
							tag_id = _dr["tag_id"].ToString();
						}
						catch
						{
							tag_id = "ERROR";
						}
						Tag = new tag(tag_id, conn);
						branch_obj = new branch(master_id, this.business_unit_id, conn);
						#endregion
						#region Vendor ID Assignment
						try
						{
							vendor_id = _dr["vendor_id"].ToString();
						}
						catch
						{
							vendor_id = "0";
						}
						#endregion
						#region Quantity Assignment
						try
						{
							qty = Convert.ToDouble(_dr["qty"].ToString());
						}
						catch
						{
							qty = 1;
						}
						#endregion
						#region Is QTY part variable Assignment
						try
						{
							is_qty = Convert.ToBoolean(_dr["is_qty"]);
						}
						catch
						{
							is_qty = false;
						}
						#endregion
						#region Sellprice Assignment
						try
						{
							sell_price = shared.GetSellPrice(cost_price_branch, 0, is_qty, 1, this.business_unit_id);
						}
						catch
						{
							sell_price = 0;
						}
						#endregion
						#region Quantity on Hand
						_onhand_qty = Convert.ToDouble(_dr["onhand_qty"]);
						#endregion
						#region dollar balance
						_dollar_balance = Convert.ToDouble(_dr["dollar_balance"]);
						#endregion
						#region Vendor Price Last DT
						var _tempdt = _dr["vendor_price_last_dt"].ToString();
						var _dts = new DateTime();
						try
						{
							if (_tempdt != "--" && _tempdt != "")
							{
								_dts = DateTime.Parse(_tempdt);
								_vendor_price_last_dt = _dts.ToString("MM/dd/yyyy");
							}
							else if (_tempdt == "--")
							{
								_vendor_price_last_dt = "--";
							}
							else
							{
								_vendor_price_last_dt = _tempdt;
							}
						}
						catch
						{
							_vendor_price_last_dt = "--";
						}
						#endregion Vendor Price Last DT
						get_location();
					}
					#region BV Part Number
					try
					{
						_bv_part_number = "N/A";
					}
					catch
					{
						_bv_part_number = "N/A";
					}
					#endregion
					#region get min max qtys
					try
					{
						minqty = Toolbox.doSQL_double(conn, @"Select ifnull(MAX(min_qty),0) from inventory_branch  where business_unit_id =@v0 and master_id =@v1 ", new object[] { this.business_unit_id, master_id });
					}
					catch
					{
					}
					try
					{
						maxqty = Toolbox.doSQL_double(conn, @"Select ifnull(MAX(max_qty),0) from inventory_branch  where business_unit_id =@v0 and master_id =@v1 ", new object[] { this.business_unit_id, master_id });
					}
					catch
					{
					}
					#endregion

					#region usage


					#endregion

					part_is_loaded = true;
					#endregion Is not a list
				}
				else if (_load_list)
				{
					#region Is a list
					parts = new ArrayList();
					var _ids = _id.ToString().Split(',');
					var ids = _ids[0] + ",";
					Debug.WriteLine("ST-LIST:" + DateTime.Now.ToString("mm:ss:fff"));
					//StringBuilder _idlist		= new StringBuilder();
					//_idlist.AppendFormat(" INNER JOIN (SELECT {0} master_id\n", _ids[0]);
					for (var i = 1; i < _ids.Length; i++)
					{
						ids += _ids[i];
						ids += i == _ids.Length - 1 ? "" : ",";
						//	_idlist.AppendFormat("UNION ALL SELECT {0} ", _ids[i]);
					}
					var proc_param_ids = " (" + ids.TrimEnd(',').Replace(",", "), (") + ") ";
					//_idlist.Append(") aa ON a.master_id = aa.master_id ");
					Debug.WriteLine("EN-LIST:" + DateTime.Now.ToString("mm:ss:fff"));
					Debug.WriteLine("ST COSTSELLS:" + DateTime.Now.ToString("mm:ss:fff") + " -- ");
					var cost_sells = get_costs_and_sells(proc_param_ids, _business_unit_id);
					Debug.WriteLine("EN COSTSELLS:" + DateTime.Now.ToString("mm:ss:fff") + " -- ");
					this.business_unit_id = _business_unit_id;
					var _country = Toolbox.doSQL_string(conn, @"SELECT IFNULL(country, 'CDN') FROM business_unit WHERE id = @v0  LIMIT 1", new object[] { this.business_unit_id });

					DataTable _dt;
					try
					{
						#region old DataTable retrieval
						/*
				string sql = string.Format(@"
	SELECT 
		a.master_id,
		IFNULL(a.active, 0) active,
		CAST(IFNULL(DATE_FORMAT(MAX(j.insert_dt), '%Y-%m-%d'), '--') AS CHAR(20)) vendor_price_last_dt,
		a.tag_id,
		e.tag,
		e.canadian_sold_as,
		IFNULL(a.old_id, 0) old_id,
		IFNULL(a.new_id, 0) new_id,
		e.usa_sold_as,
		f.unit canadian_sold_as_name,
		g.unit usa_sold_as_name,
		e.is_qty,
		e.is_exclude,
		e.is_static_sellprice,
		IFNULL(b.benchmark, 0) benchmark,
		IFNULL(b.vendor_id, '0') vendor_id,
		FORMAT(IFNULL(MIN(b.cost), 0), 5) cost,
		IFNULL(MIN(c.sellprice), 0) sellprice,
		IFNULL(MIN(b.qty), 1) qty,
		part_description(a.master_id, true, '{2}') description,
		full_part_description(a.master_id, true, '{2}') description_full,
		IFNULL(h.onhand_qty, 0) onhand_qty,
		IFNULL(h.dollar_balance, 0) dollar_balance,
		COUNT(i.id) n_pics,
		(SELECT COUNT(*) FROM inventory_location WHERE master_id = a.master_id AND business_unit_id = {1} AND min > 0 AND max > 0) n_minmax,
		(SELECT IFNULL(MAX(min_qty),0.00) FROM inventory_branch WHERE business_unit_id = {1} and master_id = a.master_id) min_qty,
		(SELECT IFNULL(MAX(max_qty),0.00) FROM inventory_branch WHERE business_unit_id = {1} and master_id = a.master_id) max_qty
	FROM 
		inventory_item_master a 
	{0}
	LEFT JOIN 
		inventory_price b 
			ON a.master_id = b.master_id AND
			b.business_unit_id = {1}
	LEFT JOIN 
		inventory_sellprice c 
			ON a.master_id = c.master_id AND
			c.business_unit_id = {1}
	LEFT JOIN
		inventory_tag e
			ON e.tag_id = a.tag_id
	LEFT JOIN
		inventory_sold_as f
			ON e.canadian_sold_as = f.id
	LEFT JOIN
		inventory_sold_as g
			ON e.usa_sold_as = g.id
	LEFT JOIN
		inventory_branch as h
			ON a.master_id = h.master_id AND
			h.business_unit_id = {1}
	LEFT JOIN
		inventory_picture i
			ON a.master_id = i.master_id
	LEFT JOIN
		inventory_price_history j ON
			j.master_id = a.master_id AND
			j.business_unit_id = {1}
	GROUP BY master_id
	ORDER BY master_id", _idlist, _business_unit_id, _country);
				 */
						#endregion
						Debug.WriteLine("ST-QUERY:" + DateTime.Now.ToString("mm:ss:fff"));
						_dt = Toolbox.doSQL_dt(@"CALL inventory_mass_load(@v0 , @v1 , @v2 )", new object[] { proc_param_ids, this.business_unit_id, _country });
						Debug.WriteLine("EN-QUERY:" + DateTime.Now.ToString("mm:ss:fff"));
					}
					catch (Exception ee)
					{
						throw new Exception("Could not load supplied part: " + _id + " " + ee);
					}
					inventory inve;
					foreach (DataRow _dr in _dt.Rows)
					{
						Debug.WriteLine("ST:" + DateTime.Now.ToString("mm:ss:fff") + " -- " + _dr["master_id"]);
						inve = new inventory();
						var _start = DateTime.Now;
						inve.master_id = _dr["master_id"].ToString();
						inve.id = Convert.ToInt32(inve.master_id);
						inve.old_id = _dr["old_id"].ToString();
						inve.new_id = _dr["new_id"].ToString();
						inve.int_onhand_qty = Convert.ToDouble(_dr["int_onhand_qty"]);
						inve.ext_onhand_qty = Convert.ToDouble(_dr["ext_onhand_qty"]);
						inve.total_int_onhand_qty = Convert.ToDouble(_dr["total_int_onhand_qty"]);
						inve.total_ext_onhand_qty = Convert.ToDouble(_dr["total_ext_onhand_qty"]);
						inve.wo_usage = _dr["wo_usage"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["wo_usage"]);
						inve.po_usage = _dr["po_usage"] == DBNull.Value ? 0 : Convert.ToDouble(_dr["po_usage"]);
						inve.allowed_to_edit_after_issue = Convert.ToBoolean(_dr["allowed_to_edit_after_issue"]);
						var _merged_part = inve.new_id != "0" ? check_merged(Convert.ToInt32(inve.master_id)) : new int[] { 0, 0 };
						if (_merged_part[0] > 0 && _merged_part[1] > 0)
						{
							inve.master_id = _merged_part[1].ToString();
							inve.id = _merged_part[1];
							inve.merged = true;
							inve.Load(inve.master_id, _business_unit_id, false);
						}
						else
						{
							#region Active State Assignment
							inve.active = Convert.ToBoolean(_dr["active"]);
							#endregion
							#region Benchmark Assignment
							inve.is_benchmark = Convert.ToBoolean(_dr["benchmark"]);
							#endregion
							inve.has_pic = Convert.ToInt32(_dr["n_pics"]) > 0;
							n_minmax = Convert.ToDouble(_dr["n_minmax"]);
							#region Part Description Assignment
							inve.description = _dr["description"].ToString();
							inve.description_full = _dr["description_full"].ToString();
							inve.description_int = _description.Length > 80 ? inve.description_full : inve.description_full;
							inve.description_short = _description.Length > 80 ? inve.description.Substring(0, 77) + "..." : inve.description;
							#endregion
							#region Sold As - Canada
							inve.sold_as_canadian = _dr["canadian_sold_as"].ToString();
							inve.sold_as_canadian_name = _dr["canadian_sold_as_name"].ToString();
							#endregion
							#region Sold As - U.S.A
							inve.sold_as_usa = _dr["usa_sold_as"].ToString();
							inve.sold_as_usa_name = _dr["usa_sold_as_name"].ToString();
							#endregion
							#region Tag Name Assignment
							inve.tag_name = _dr["tag"].ToString();
							#endregion
							#region Force Cost to 0.01 Assignment
							inve.is_exclude = Convert.ToBoolean(_dr["is_exclude"]);
							#endregion
							inve.is_stocked = Convert.ToBoolean(_dr["is_stocked"]);
							#region Is Static Sell Price Assignment
							inve.is_static_sellprice = Convert.ToBoolean(_dr["is_static_sellprice"]);
							#endregion
							#region Cost Price Assignment
							//Debug.WriteLine("ST-COSTS:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_dr["master_id"]);
							inve.cost_price_branch = cost_sells.Select("master_id = " + inve.master_id).Length == 1 ? (double)cost_sells.Select("master_id = " + inve.master_id)[0]["weighted"] : 0;
							inve.cost_price_branch_lowest = cost_sells.Select("master_id = " + inve.master_id).Length == 1 ? (double)cost_sells.Select("master_id = " + inve.master_id)[0]["lowest"] : 0;
							inve.cost_price_branch_highest = cost_sells.Select("master_id = " + inve.master_id).Length == 1 ? (double)cost_sells.Select("master_id = " + inve.master_id)[0]["highest"] : 0;
							inve.cost_price_branch_level = cost_sells.Select("master_id = " + inve.master_id).Length == 1 ? Convert.ToInt32(cost_sells.Select("master_id = " + inve.master_id)[0]["level"]) : 0;
							//Debug.WriteLine("EN-COSTS:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_dr["master_id"]);
							#endregion
							#region Tag ID Assignment
							inve.tag_id = _dr["tag_id"].ToString();
							#endregion
							#region Vendor ID Assignment
							inve.vendor_id = _dr["vendor_id"].ToString();
							#endregion
							#region Quantity Assignment
							inve.qty = Convert.ToDouble(_dr["qty"].ToString());
							#endregion
							#region Is QTY part variable Assignment
							inve.is_qty = Convert.ToBoolean(_dr["is_qty"]);
							#endregion
							#region Sellprice Assignment
							//Debug.WriteLine("ST-SELL:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_dr["master_id"]);
							inve.sell_price = cost_sells.Select("master_id = " + inve.master_id).Length == 1 ? (double)cost_sells.Select("master_id = " + inve.master_id)[0]["sell"] : 0;
							//Debug.WriteLine("EN-SELL:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_dr["master_id"]);
							#endregion
							#region Quantity on Hand
							inve.onhand_qty = Convert.ToDouble(_dr["onhand_qty"]);
							#endregion
							#region dollar balance
							inve.dollar_balance = Convert.ToDouble(_dr["dollar_balance"]);
							#endregion
							#region Vendor Price Last DT
							inve.vendor_price_last_dt = _dr["vendor_price_last_dt"].ToString();
							#endregion Vendor Price Last DT
							//_location_name		= _dr["location_name"].ToString();// 	i.get_location();
							#region BV Part Number
							inve.bv_part_number = "N/A";
							#endregion
							#region get min max qtys
							#endregion
							#region get company average cost

							#endregion
							#region get min max qtys
							inve.minqty = Convert.ToDouble(_dr["min_qty"]);
							inve.maxqty = Convert.ToDouble(_dr["max_qty"]);
							#endregion
							inve.part_is_loaded = true;
							Debug.WriteLine("EN:" + DateTime.Now.ToString("mm:ss:fff") + " -- " + _dr["master_id"]);
						}
						var _end = DateTime.Now;
						var ttl = _end.Subtract(_start).TotalMilliseconds;
						inve.ttl_ms = ttl;
						parts.Add(inve);
					}
					#endregion Is a list
				}
				Toolbox.do_debug("end - inventory Load (" + _id + "," + _business_unit_id + ")");
			}
		}

		public int[] check_merged(int _id)
		{
			//Debug.WriteLine("ST - merged:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_id);
			int[] arr = { 0, 0 };
			var count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_item_master  WHERE old_id =@v0", new object[] { _id });
			arr[0] = count > 0 ? 1 : 0;
			var real_id = 0;
			if (count == 1)
			{
				var merged_to = Toolbox.doSQL_dt(@"SELECT master_id, active, CAST(IFNULL(new_id, '0') AS UNSIGNED) new_id FROM inventory_item_master  WHERE old_id =@v0", new object[] { _id }).Rows[0];
				var is_active = Convert.ToBoolean(merged_to["active"]);
				var merged_id = Convert.ToInt32(merged_to["new_id"]);
				var _master_id = Convert.ToInt32(merged_to["master_id"]);
				if (merged_id == _id)
				{
					Toolbox.doSQL_void(@"UPDATE inventory_item_master SET old_id = 0 WHERE master_id = @v0  LIMIT 1", new object[] { _master_id });
					real_id = 0;
				}
				else if (is_active && merged_id != 0)
				{
					real_id = merged_id;
				}
				else if (is_active && merged_id == 0)
				{
					var merge = check_merged(_master_id);
					real_id = merge[1];
				}
				else if (!is_active && merged_id != 0)
				{
					var merge = check_merged(merged_id);
					real_id = merge[1];
				}
				else
				{
					throw new Exception("The part " + _id + " has been merged but the merged to part cannot be used");
				}
			}
			real_id = real_id == 0 ? _id : real_id;
			arr[1] = real_id;
			//Debug.WriteLine("EN - merged:"+DateTime.Now.ToString("mm:ss:fff")+" -- "+_id);
			return arr;
		}
		public DataTable get_costs_and_sells(string part_list, object c_id)
		{
			return Toolbox.doSQL_dt(@"CALL inventory_get_costs_and_sells(@v0 , @v1 )", new object[] { part_list, c_id });
		}
		public void get_costs(object m_id, object c_id)
		{
			try
			{
				var branch_array = Toolbox.doSQL_dt(@" SELECT IFNULL((SELECT cost FROM inventory_price a WHERE a.master_id = @v0  AND a.business_unit_id = @v1  ORDER BY a.cost ASC limit 1), 0) lowest, IFNULL((SELECT cost FROM inventory_price WHERE master_id = @v0  AND business_unit_id = @v1  ORDER BY cost DESC limit 1), 0) highest, (SELECT GET_CURRENT_COST(@v0 ,@v1 )) weighted, GET_COST(@v0 , @v1 , 1) level ", new object[] { m_id, c_id }).Rows[0];
				cost_price_branch_lowest = is_exclude ? 0.01 : Convert.ToDouble(branch_array["lowest"]);
				cost_price_branch_highest = is_exclude ? 0.01 : Convert.ToDouble(branch_array["highest"]);
				_cost_price_weighted = is_exclude ? 0.01 : Convert.ToDouble(branch_array["weighted"]);
				cost_price_branch = is_exclude ? 0.01 : _cost_price_weighted;
				cost_price_branch_level = is_exclude ? 0 : Convert.ToInt32(branch_array["level"]);
			}
			catch
			{
				cost_price_branch_lowest = is_exclude ? 0.01 : 0;
				_cost_price_weighted = is_exclude ? 0.01 : 0;
				cost_price_branch_highest = is_exclude ? 0.01 : 0;
				cost_price_branch = is_exclude ? 0.01 : 0;
				cost_price_branch_level = is_exclude ? 0 : 0;
			}
		}
		public class tag
		{
			public int id { get; set; }

			public string name { get; set; }

			public bool is_qty { get; set; }

			public bool approved { get; set; }

			public bool is_exclude { get; set; }

			public bool tax_1 { get; set; }

			public bool tax_2 { get; set; }

			public bool tax_3 { get; set; }

			public bool active { get; set; }
			public bool allowed_to_edit_after_issue { get; set; }

			public bool is_static_sellprice { get; set; }
			public bool is_consumable { get; set; }
			public bool quick_add { get; set; }

			public bool allow_branch_as_vendor { get; set; }

			public NeMember approved_by { get; set; }

			public DateTime approved_date { get; set; }
			public DateTime ts { get; set; }

			public int gl_id { get; set; }

			public int expiry_period { get; set; }

			public sold_as sold_as { get; set; }

			public sold_as usa_sold_as { get; set; }

			public sold_as canadian_sold_as { get; set; }

			public DateTime insert_dt { get; set; }

			public bool is_fixedmarkup { get; set; }
			public double markup_amount { get; set; }
			public bool IncludeSearch { get; set; }
			public tag()
			{
			}
			public tag(object _tag_id)
			{
				using (var conn = Toolbox.connect())
				{
					id = Convert.ToInt32(_tag_id);
					load(conn);
				}
			}
			public tag(object _tag_id, MySqlConnection _conn)
			{
				id = Convert.ToInt32(_tag_id);
				load(_conn);
			}
			private void load(MySqlConnection _conn)
			{
				var _tag = Toolbox.doSQL_dt(_conn, @" SELECT tag, IFNULL(is_qty, 0) is_qty, IFNULL(is_exclude, 0) is_exclude, IFNULL(allow_branch_as_vendor, 0) allow_branch_as_vendor, IFNULL(gl_id,0) gl_id, IFNULL(expiry_period, 0) expiry_period, IFNULL(sold_as, 3) sold_as, IFNULL(usa_sold_as, 3) usa_sold_as, IFNULL(canadian_sold_as,3) canadian_sold_as, IFNULL(tax_1, 0) tax_1, IFNULL(tax_2, 0) tax_2, IFNULL(tax_3, 0) tax_3, IFNULL(active, 0) active, IFNULL(is_static_sellprice, 0) is_static_sellprice, IFNULL(is_fixedmarkup, 0) is_fixedmarkup, IFNULL(markup_amount, 0) markup_amount, allowed_to_edit_after_issue, ts, IFNULL(is_consumable, 0) is_consumable, IFNULL(include_search, 1) include_search FROM inventory_tag WHERE tag_id = @v0  LIMIT 1", new object[] { id }).Rows[0];
				name = _tag["tag"].ToString();
				sold_as = new sold_as(_tag["sold_as"]);
				usa_sold_as = new sold_as(_tag["usa_sold_as"]);
				canadian_sold_as = new sold_as(_tag["canadian_sold_as"]);
				is_qty = Convert.ToBoolean(_tag["is_qty"]);
				is_exclude = Convert.ToBoolean(_tag["is_exclude"]);
				allow_branch_as_vendor = Convert.ToBoolean(_tag["allow_branch_as_vendor"]);
				gl_id = Convert.ToInt32(_tag["gl_id"]);
				expiry_period = Convert.ToInt32(_tag["expiry_period"]);
				tax_1 = Convert.ToBoolean(_tag["tax_1"]);
				tax_2 = Convert.ToBoolean(_tag["tax_2"]);
				tax_3 = Convert.ToBoolean(_tag["tax_3"]);
				//_quick_add				= Convert.ToBoolean(_tag["quick_add"]); 
				active = Convert.ToBoolean(_tag["active"]);
				is_static_sellprice = Convert.ToBoolean(_tag["is_static_sellprice"]);
				//_approved_date			= Convert.ToDateTime(_tag["approved_date"]);
				//_insert_dt				= Convert.ToDateTime(_tag["insert_dt"]);
				is_fixedmarkup = Convert.ToBoolean(_tag["is_fixedmarkup"]);
				is_consumable = Convert.ToBoolean(_tag["is_consumable"]);
				markup_amount = Convert.ToDouble(_tag["markup_amount"]);
				allowed_to_edit_after_issue = Convert.ToBoolean(_tag["allowed_to_edit_after_issue"]);
				IncludeSearch = Convert.ToBoolean(_tag["include_search"]);
				ts = Toolbox.ReturnBlankDateTimeIfNull(_tag["ts"]);
			}
		}

		private bool IsPartFromAnExcludedTag(int id)
		{
			var query = @"
            SELECT *
	        FROM
	            inventory_item_master iim
	            INNER JOIN inventory_tag it
	            ON it.tag_id = iim.tag_id
	        WHERE master_id = @v0
	        AND it.is_exclude = 1
	        AND it.active = 1
	        AND it.approved = 1;
	        ";

			DataTable dt = Toolbox.doSQL_dt(query, new object[] { id });
			if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
			{
				//
				// No data: we return false, not excluded.
				// We may check the "it.is_exclude = 0" condition. But if there is no this materId, the load function will handle it - and give you warning 'Part Does Not Exist - xxxx'.
				//
				return false;
			}

			// master id is the key, so there is one.
			var row = dt.Rows[0];
			if (row["is_exclude"] == null || (row["is_exclude"] is DBNull))
			{
				// Treat it as NOT Excluded.
				return false;
			}

			var isExclude = Convert.ToBoolean(row["is_exclude"]);
			return isExclude;
		}
	}
	public class sold_as
	{
		public int id { get; set; }

		public string unit { get; set; }

		public string abbr { get; set; }

		public sold_as(object _row_id)
		{
			id = Convert.ToInt32(_row_id);
			load();
		}
		private void load()
		{
			var sold_as_row = Toolbox.doSQL_dt(@"SELECT unit,abbr FROM inventory_sold_as WHERE id = @v0  LIMIT 1", new object[] { id }).Rows[0];
			unit = sold_as_row["unit"].ToString();
			abbr = sold_as_row["abbr"].ToString();
		}
	}
	public class vendor_price_row
	{
		#region private
		private object _price_id = 0;
		private object _master_id = 0;
		private object _vendor_code = "";
		private object _cost = 0;
		private object _total = 0;
		private object _qty = 0;
		private int _lead_time = 7;
		private object _vendor_id = 0;
		private object _origin = "";
		private string _note = "";

		#endregion
		#region public
		public object price_id { get { return _price_id; } set { _price_id = value; } }
		public object master_id { get { return _master_id; } set { _master_id = value; } }
		public object vendor_code { get { return _vendor_code; } set { _vendor_code = value; } }
		public int member_id { get; set; }
		public int business_unit_id { get; set; }
		public object cost { get { return _cost; } set { _cost = value; } }
		public object total { get { return _total; } set { _total = value; } }
		public int lead_time { get { return _lead_time; } set { _lead_time = value; } }
		public bool exists { get; set; }

		public bool is_benchmark { get; set; }

		public bool is_preferred { get; set; }

		public string note { get { return _note; } set { _note = value; } }
		public DateTime last_edited { get; set; }

		public DateTime insert_dt { get; set; }

		public object qty { get { return _qty; } set { _qty = value; } }
		/// <summary>
		/// This currently is the vendor number from BV, not the MySQL vendor id, use it as such until changed.
		/// </summary>
		public object vendor_id { get { return _vendor_id; } set { _vendor_id = value; } }
		public object origin { get { return _origin; } set { _origin = value; } }
		#endregion
		public vendor_price_row()
		{
			exists = false;
		}

		public vendor_price_row(object mid, object cid, object vid)
		{
			var row_id = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(id), 0) FROM inventory_price WHERE master_id = @v0  AND business_unit_id = @v1  AND vendor_id = @v2 ", new object[] { mid, cid, vid });
			if (row_id != "0")
			{
				exists = true;
				getrow(row_id);
			}
			else
			{
				exists = false;
			}
		}
		public vendor_price_row(object mid, object cid, object vid, object vcode)
		{
			var row_id = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(id), 0) FROM inventory_price WHERE master_id = @v0  AND business_unit_id = @v1  AND vendor_id = @v2  AND (vendor_code = @v3  OR vendor_code = @v4 )", new object[] { mid, cid, vid, vcode, Toolbox.MySQL_safe(vcode.ToString()) });
			if (row_id != "0")
			{
				exists = true;
				getrow(row_id);
			}
			else
			{
				exists = false;
			}
		}
		public vendor_price_row(object row_id)
		{
			exists = true;
			getrow(row_id);
		}
		private void getrow(object id)
		{
			var vpr = Toolbox.doSQL_dt(@"SELECT * FROM inventory_price WHERE id = @v0 ", new object[] { id }).Rows[0];
			price_id = Convert.ToInt32(vpr["id"]);
			cost = Convert.ToDouble(vpr["cost"]);
			master_id = vpr["master_id"].ToString();
			total = Convert.ToDouble(vpr["total"]);
			business_unit_id = (int)vpr["business_unit_id"];
			is_benchmark = Convert.ToBoolean(vpr["benchmark"]);
			is_preferred = Convert.ToBoolean(vpr["is_preferred"]);
			qty = Convert.ToDouble(vpr["qty"]);
			note = vpr["note"].ToString();
			vendor_code = vpr["vendor_code"].ToString();
			last_edited = vpr["edited_dt"] != DBNull.Value ? Convert.ToDateTime(vpr["edited_dt"]) : new DateTime();
			insert_dt = vpr["insert_dt"] != DBNull.Value ? Convert.ToDateTime(vpr["insert_dt"]) : new DateTime();
			vendor_id = vpr["vendor_id"].ToString();
			member_id = Toolbox.ReturnZeroIfNull_int(vpr["member_id"]);
			lead_time = Convert.ToInt32(vpr["lead_time"]);
		}
		public bool save()
		{
			return save(true);
		}
		public void default_benchmarks()
		{
			Toolbox.doSQL_void(@"UPDATE inventory_price SET benchmark = false, edited_dt = edited_dt  WHERE master_id =@v0 AND business_unit_id =@v1 ", new object[] { master_id, business_unit_id });
		}

		public void default_preferred()
		{
			Toolbox.doSQL_void(@"UPDATE inventory_price SET is_preferred = false, edited_dt = edited_dt  WHERE master_id =@v0 AND business_unit_id =@v1 ", new object[] { master_id, business_unit_id });
		}
		public bool save(bool do_save_history)
		{
			var is_edit = false;
			var _errors = "";
			if (_vendor_id == null || _vendor_id.ToString() == "" || _vendor_id.ToString() == "0")
			{
				_errors += "- Vendor not set\n";
			}
			if (business_unit_id == null || business_unit_id.ToString() == "")
			{
				_errors += "- Business Unit ID not set\n";
			}
			if (_vendor_code == null || _vendor_code.ToString().Trim() == "")
			{
				_errors += "- Vendor Code not set\n";
			}

			if (_price_id != null && _price_id.ToString() != "" && Convert.ToInt32(_price_id) != 0)
			{
				is_edit = true;
			}
			else
			{
				// price id might not have been supplied, look it up based on the vendor_code/business_unit_id/vendor_id/master_id
				var c = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(id), 0) FROM inventory_price WHERE master_id = @v0  AND vendor_id = @v1  AND business_unit_id = @v2  AND vendor_code = @v3 ", new object[] { master_id, vendor_id, business_unit_id, vendor_code });
				if (c > 0)
				{
					_price_id = c;
					is_edit = true;
				}
			}
			var vendor_code_occurrances = Toolbox.doSQL_int(@"SELECT COUNT(a.id) FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_code = @v0  AND a.vendor_id = @v1  AND a.master_id != @v2  AND b.active = true", new object[] { _vendor_code, _vendor_id, _master_id });
			var _parts = "";
			try
			{
				_parts = vendor_code_occurrances > 0 ? Toolbox.doSQL_string(@"SELECT CAST(GROUP_CONCAT(DISTINCT(a.master_id) ORDER BY a.master_id) AS CHAR) _parts FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_code = @v0  AND a.vendor_id = @v1  AND a.id != @v2 ", new object[] { _vendor_code, _vendor_id, _price_id, business_unit_id }) : "";
			}
			catch (Exception ex)
			{
				throw new Exception(ex + " Duplicate Vendor Part Number");
			}
			#region check validity
			if (string.IsNullOrEmpty(_master_id.ToString()) || _master_id.ToString() == "0")
			{
				_errors += @"
- Master ID not set
";
			}
			if (vendor_code_occurrances > 0)
			{
				var plural = vendor_code_occurrances == 1 ? "this part number" : "these part numbers";
				_errors += string.Format(@"
- The supplied vendor part number ({2}) currently exists in {1}: ({0}).
{3} is in the process of making it so one vendor's part number equals one {3} part number.
So as a result, the supplied vendor price could not be saved.
Martin has been sent an email with information pertaining to this error.
If this is URGENT, please call Martin at (800) 204 4153.
", _parts, plural, _vendor_code, new NeBusinessUnit(business_unit_id).name);
				var temp_member = new NeMember(member_id);
				var _message = new NeEMail();
				_message.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				_message.To = "mkettenbach@" + Toolbox.app_setting("DomainForEmail");
				_message.CC = temp_member.NEEmail;
				_message.Body = $@"
Master ID: {_master_id} <br/>
Vendor ID: {_vendor_id} <br/>
Vendor Part #: {_vendor_code} <br/>
List of other parts that have this vendor part # associated with them: {_parts}<br/>
Employee Trying to perform duplication: {temp_member.FullName}<br/>
";
				_message.Subject = "Vendor code duplication";
				_message.isHTML = true;
				if (temp_member.id != 711)
				{
					_message.Send();
				}
			}
			if (member_id.ToString() == "")
			{
				_errors += "- Member ID not set\n";
			}
			if (_cost == null || _cost.ToString() == "" || Convert.ToDouble(_cost) <= 0)
			{
				_errors += "- Cost not set\n";
			}
			if (_total == null || _total.ToString() == "" || Convert.ToDouble(_total) <= 0)
			{
				_errors += "- Total (Vendor Sell) not set\n";
			}
			if (_qty == null || _qty.ToString() == "")
			{
				_errors += "- Quantity not set\n";
			}
			if (_errors != "")
			{
				throw new Exception(_errors);
			}
			var _inv = new inventory();
			_inv.Load(_master_id, business_unit_id);
			//if (_inv.is_exclude)
			//	{
			//	_cost = 0.01;
			//	_total = 0.01;
			//	}
			#endregion
			var is_saved = false;
			var sql = "";
			object[] objectParams;
			//	_is_benchmark = _is_benchmark == null ? false : _is_benchmark;
			is_benchmark = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_price WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] { master_id, business_unit_id }) == 0 ? true : is_benchmark;
			//	_is_preferred = _is_preferred == null ? false : _is_preferred;
			if (is_edit) // Validity has been checked, otherwise an exception would be thrown.
			{
				#region edit

				sql = @"
UPDATE 
	inventory_price 
SET 
	master_id = @v0, 
	vendor_code = @v1,
	member_id = @v2,
	business_unit_id = @v3,
	cost = @v4,
	total = @v5,
	qty = @v6,
	vendor_id = @v7,
	origin = @v8,
	lead_time = @v10,
	benchmark = @v11,
	is_preferred = @v12,
	note = @v13
WHERE
	id = @v9
	";
				objectParams = new object[]
				{
						_master_id, // {0}
						_vendor_code, // {1}
						member_id, // {2}
						business_unit_id, // {3}
						_cost, // {4}
						_total, // {5}
						_qty, // {6}
						_vendor_id, // {7}
						_origin, // {8}
						_price_id, // {9}
						_lead_time, // {10}
						is_benchmark, // {11}
						is_preferred, // {12}
						_note // {13}
				};

				#endregion
			}
			else
			{
				#region insert

				sql = @"
INSERT INTO inventory_price
	(
	master_id,
	vendor_code,
	member_id,
	insert_dt,
	business_unit_id,
	cost,
	total,
	qty,
	vendor_id,
	origin,
	lead_time,
	benchmark,
	is_preferred,
	note
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	now(),
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10,
	@v11,
	@v12
	)";
				objectParams = new object[]
				{
						_master_id, // {0}
						_vendor_code, // {1}
						member_id, // {2}
						business_unit_id, // {3}
						_cost, // {4}
						_total, // {5}
						_qty, // {6}
						_vendor_id, // {7}
						_origin, // {8}
						_lead_time, // {9}
						is_benchmark, // {10}
						is_preferred, // {11}
						_note // {12}
				};

				#endregion
			}
			try
			{
				if (is_preferred)
				{
					default_preferred();
				}
				if (is_benchmark)
				{
					default_benchmarks();
				}
				Toolbox.doSQL_void(sql, objectParams);
				#region insert history
				if (do_save_history)
				{
					save_history();
				}
				is_saved = true;
				#endregion
			}
			catch (Exception ee)
			{
				throw ee;
				//is_saved = false; c
			}
			return is_saved;
		}
		public void save_history()
		{
			_origin = (string)_origin == string.Empty ? "Manually Updated" : _origin;
			Toolbox.doSQL_void(@" INSERT INTO inventory_price_history ( master_id, vendor_code, member_id, insert_dt, business_unit_id, cost, total, qty, vendor_id, origin, lead_time, benchmark, is_preferred, note ) VALUES ( @v0 , @v1 , @v2 , now(), @v3 , @v4 , @v5 , @v6 , @v7 , @v8 , 7, @v9 , @v10 , @v11  )", new object[] { _master_id, _vendor_code, member_id, business_unit_id, _cost, _total, _qty, _vendor_id, _origin, is_benchmark, is_preferred, _note });
		}
	}
	public class global_price
	{
		private string _price_id = "0";
		private string _business_unit_id = "";
		private string _country = "";

		public global_price()
		{
			sell = 0;
			cost = 0;
		}

		public double cost { get; set; }

		public double sell { get; set; }

	}
	public class barcode
	{
		public int id { get; set; }

		public int master_id { get; set; }

		public string barcode_no { get; set; }

		public string table_type { get; set; }

		public int table_id { get; set; }

		public string dt_created { get; set; }

		public int member_id_added { get; set; }

		public string member_name { get; set; }

		public string ref_name { get; set; }

		public bool is_active { get; set; }

		public barcode()
		{
			id = 0;
		}

		public barcode(int row_id)
		{
			id = row_id;
			Load();
		}
		public void Load()
		{
			if (Exists())
			{
				var _dr = Toolbox.doSQL_dt(@"SELECT * FROM inventory_barcode WHERE id = @v0 ", new object[] { id }).Rows[0];
				master_id = Convert.ToInt32(_dr["master_id"]);
				barcode_no = (string)_dr["barcode_no"];
				table_type = (string)_dr["table_type"];
				table_id = Convert.ToInt32(_dr["table_id"]);
				dt_created = _dr["dt_created"].ToString();
				member_id_added = Convert.ToInt32(_dr["member_id_added"]);
				is_active = Convert.ToBoolean(_dr["is_active"]);
			}
			else
			{
				throw new Exception("Inventory barcode row doesn't exist, or is not set.");
			}
		}
		public bool Exists()
		{
			var does_exist = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_barcode WHERE id = @v0 ", new object[] { id }) == 1;
			return does_exist;
		}
		public void Save()
		{
			var is_new = id == 0;
			if (is_new)
			{
				Toolbox.doSQL_void(@" INSERT INTO inventory_barcode ( master_id, barcode_no, table_type, table_id, dt_created, member_id_added, is_active ) VALUES ( @v0 , @v1 , @v2 , @v3 , NOW(), @v4 , @v5  )", new object[] { master_id, barcode_no, table_type, table_id, member_id_added, is_active });
			}
			else
			{
				Toolbox.doSQL_void(@" UPDATE inventory_barcode SET master_id = @v0 , barcode_no = @v1 , table_type = @v2 , table_id = @v3 , is_active = @v4  WHERE id = @v5  LIMIT 1", new object[] { master_id, barcode_no, table_type, table_id, is_active, id });
			}
		}
	}
	public class incorrect_levels
	{
		public int id { get; set; }
		public int location_id { get; set; }
		public int member_id { get; set; }
		public int cleared_by { get; set; }
		public int master_id { get; set; }
		public DateTime? date { get; set; }
		public DateTime? cleared_date { get; set; }
		public double reported_qty { get; set; }
		public double incorrect_qty { get; set; }
		public double changed_to_qty { get; set; }
		public string notes { get; set; }

		public incorrect_levels()
		{
			location_id = 0;
			member_id = 0;
			cleared_by = 0;
			master_id = 0;
			reported_qty = 0;
			incorrect_qty = 0;
			changed_to_qty = 0;
			notes = "";
			id = 0;
		}

		public incorrect_levels(int _id)
		{
			var dt = Toolbox.doSQL_dt(@"Select * from inventory_incorrect_levels  where id =@v0", new object[] { _id });
			if (dt.Rows.Count > 0)
			{
				id = _id;
				location_id = Convert.ToInt32(dt.Rows[0]["location_id"]);
				member_id = Convert.ToInt32(dt.Rows[0]["member_id"]);
				cleared_by = dt.Rows[0]["cleared_by"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["cleared_by"]);
				master_id = Convert.ToInt32(dt.Rows[0]["master_id"]);
				reported_qty = Convert.ToDouble(dt.Rows[0]["reported_qty"]);
				incorrect_qty = Convert.ToDouble(dt.Rows[0]["incorrect_qty"]);
				changed_to_qty = Convert.ToDouble(dt.Rows[0]["changed_to_qty"]);
				notes = dt.Rows[0]["notes"] == DBNull.Value ? "" : dt.Rows[0]["notes"].ToString();
				if (dt.Rows[0]["date"] != DBNull.Value)
				{
					date = Convert.ToDateTime(dt.Rows[0]["date"]);
				}
				if (dt.Rows[0]["cleared_date"] != DBNull.Value)
				{
					cleared_date = Convert.ToDateTime(dt.Rows[0]["cleared_date"]);
				}
			}
		}

		public void approve(int id, int _cleared_by_member_id, double _changed_to_qty)
		{
			Toolbox.doSQL_void(@"update inventory_incorrect_levels  set cleared_by=@v0, cleared_date = now(),changed_to_qty=@v1   where id =@v2", new object[] { _cleared_by_member_id, _changed_to_qty, id });
		}

		public void save()
		{
			if (id == 0)
			{
				if (Toolbox.doSQL_int(@"Select count(id) from inventory_incorrect_levels  where master_id =@v0 and location_id =@v1  and (cleared_date is null or cleared_by=0)", new object[] { master_id, location_id }) > 0)
				{
					Toolbox.doSQL_void(@"update inventory_incorrect_levels  set date=now(),member_id=@v0,reported_qty=@v1 ,incorrect_qty=@v2 ,notes=@v3   where master_id =@v4 and location_id =@v5  and (cleared_date is null or cleared_by=0) limit 1", new object[] { member_id, reported_qty, incorrect_qty, notes, master_id, location_id });
				}
				else
				{
					Toolbox.doSQL_void(@"Insert into inventory_incorrect_levels( location_id, member_id, cleared_by, master_id, reported_qty, incorrect_qty, changed_to_qty, notes,date)  values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,now())", new object[] { location_id, member_id, cleared_by, master_id, reported_qty, incorrect_qty, changed_to_qty, notes });
					id = Toolbox.doSQL_int(@"Select id from inventory_incorrect_levels order by id desc limit 1");
				}
			}
			else
			{
				Toolbox.doSQL_void(@"update inventory_incorrect_levels  set location_id=@v0,member_id=@v1 ,cleared_by=@v2 ,master_id=@v3 ,reported_qty=@v4 ,incorrect_qty=@v5 ,notes=@v6 ,changed_to_qty=@v7   where id =@v8", new object[] { location_id, member_id, cleared_by, master_id, reported_qty, incorrect_qty, notes, changed_to_qty, id });
			}
		}

		public static DataTable get_open_incorrect_locations_qtys(int business_unit_id)
		{

			var dt = Toolbox.doSQL_dt(@"SELECT inventory_incorrect_levels.id, inventory_incorrect_levels.location_id, inventory_incorrect_levels.member_id AS EnteredBy_id, EnteredBy.member_fullname AS EnteredBy_name, inventory_incorrect_levels.date AS date_entered, inventory_incorrect_levels.cleared_date, inventory_incorrect_levels.cleared_by AS ClearedBy_id, ClearedBy.member_fullname AS ClearedBy_name, inventory_incorrect_levels.reported_qty, inventory_incorrect_levels.incorrect_qty, inventory_incorrect_levels.notes, inventory_incorrect_levels.master_id, Location.`name` AS location_name FROM inventory_incorrect_levels INNER JOIN member AS EnteredBy ON EnteredBy.Member_ID = inventory_incorrect_levels.member_id INNER JOIN member AS ClearedBy ON ClearedBy.Member_ID = inventory_incorrect_levels.cleared_by INNER JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id  where inventory_incorrect_levels.cleared_by = 0 and inventory_location_master.business_unit_id =@v0", new object[] { business_unit_id });
			return dt;
		}


	}
	public class branch
	{
		private int _section_id = 1;
		private bool preferred = false;
		public int master_id { get; set; }

		public int section_id { get { return _section_id; } set { _section_id = value; } }
		public int business_unit_id { get; set; }

		public int member_id { get; set; }

		public double min_qty { get; set; }

		public double max_qty { get; set; }

		public double onhand_qty { get; set; }

		public double dollar_balance { get; set; }

		public double int_onhand_qty { get; set; }
		public double ext_onhand_qty { get; set; }
		public double total_int_onhand_qty { get; set; }
		public double total_ext_onhand_qty { get; set; }
		public double int_min { get; set; }
		public double ext_min { get; set; }
		public double int_max { get; set; }
		public double ext_max { get; set; }
		public double po_usage { get; set; }
		public double wo_usage { get; set; }
		public bool is_stocked { get; set; }
		public bool is_consumable { get; set; }

		public branch()
		{
			dollar_balance = 0;
			onhand_qty = 0;
			max_qty = 0;
			min_qty = 0;
			member_id = 0;
			business_unit_id = 0;
			master_id = 0;
			is_consumable = false;
		}

		public branch(object master_id, int business_unit_id, MySqlConnection _conn)
		{
			dollar_balance = 0;
			onhand_qty = 0;
			max_qty = 0;
			min_qty = 0;
			member_id = 0;
			this.business_unit_id = 0;
			this.master_id = 0;
			is_consumable = false;
			load(Convert.ToInt32(master_id), business_unit_id, _conn);
		}
		public branch(object master_id, int business_unit_id)
		{
			using (var conn = Toolbox.connect())
			{
				dollar_balance = 0;
				onhand_qty = 0;
				max_qty = 0;
				min_qty = 0;
				member_id = 0;
				this.business_unit_id = 0;
				this.master_id = 0;
				is_consumable = false;
				load(Convert.ToInt32(master_id), business_unit_id, conn);
			}
		}

		public static int get_default_internal_location(int business_unit_id, int master_id)
		{
			return Toolbox.doSQL_int(@"select ifnull((Select inventory_location.id from inventory_location inner join inventory_location_master ilm on ilm.id = inventory_location.location_master_id  where master_id =@v0 and inventory_location.business_unit_id =@v1  and ilm.type_id = 1 order by qty desc, min desc, max desc limit 1),0)", new object[] { master_id, business_unit_id });

		}

		public void load(int master, int company, MySqlConnection _conn)
		{
			master_id = master;
			business_unit_id = company;
			if (exists(_conn))
			{
				var _dr = Toolbox.doSQL_dt(_conn, @"SELECT * FROM inventory_branch WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] { master_id, business_unit_id }).Rows[0];
				onhand_qty = Toolbox.ReturnZeroIfNull_double(_dr["onhand_qty"]);
				dollar_balance = Toolbox.ReturnZeroIfNull_double(_dr["dollar_balance"]);
				preferred = Toolbox.ReturnZeroIfNull_int(_dr["preferred"]) == 1;
				int_onhand_qty = Toolbox.ReturnZeroIfNull_double(_dr["int_onhand_qty"]);
				ext_onhand_qty = Toolbox.ReturnZeroIfNull_double(_dr["ext_onhand_qty"]);
				int_min = Toolbox.ReturnZeroIfNull_double(_dr["int_min"]);
				ext_min = Toolbox.ReturnZeroIfNull_double(_dr["ext_min"]);
				int_max = Toolbox.ReturnZeroIfNull_double(_dr["int_max"]);
				ext_max = Toolbox.ReturnZeroIfNull_double(_dr["ext_max"]);
				po_usage = Toolbox.ReturnZeroIfNull_double(_dr["po_usage"]);
				wo_usage = Toolbox.ReturnZeroIfNull_double(_dr["wo_usage"]);
				is_stocked = int_min > 0 || ext_min > 0;
				is_consumable = Toolbox.ReturnZeroIfNull_int(_dr["is_consumable"]) == 1;
			}
			else
			{
				min_qty = 0;
				max_qty = 0;
				onhand_qty = 0;
				dollar_balance = 0;
				preferred = false;
				is_stocked = false;
				is_consumable = false;
				save(_conn);
			}

		}
		public void load(int master, int company)
		{
			using (var conn = Toolbox.connect())
			{
				load(master, company, conn);
			}
		}
		public bool exists(MySqlConnection _conn)
		{
			return exists(master_id, business_unit_id, _conn);
		}
		public bool exists()
		{
			using (var conn = Toolbox.connect())
			{
				return exists(master_id, business_unit_id, conn);
			}
		}
		public bool exists(int master, int company)
		{
			using (var conn = Toolbox.connect())
			{
				return exists(master, company, conn);
			}
		}
		public bool exists(int master, int company, MySqlConnection _conn)
		{
			return Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM inventory_branch WHERE business_unit_id = @v0  AND master_id = @v1 ", new object[] { company, master }) == 1;
		}
		public void save()
		{
			using (var conn = Toolbox.connect())
			{
				save(conn);
			}
		}
		public void save(MySqlConnection _conn)
		{
			/*
		if(dollar_balance > 0 && onhand_qty < 0)
			{
			_tools.catch_error(new Exception("Dollar balance can't be positive when the on-hand quantity is negative business_unit_id:"+business_unit_id));
			throw new Exception("Dollar balance can't be positive when the on-hand quantity is negative");
			}
		if(onhand_qty > 0 && dollar_balance < 0)
			{
			_tools.catch_error(new Exception("Dollar balance can't be negative when the on-hand quantity is positive business_unit_id:"+business_unit_id));
			throw new Exception("Dollar balance can't be negative when the on-hand quantity is positive");
			}
		 */
			//NELog l = new NELog();
			//double temp_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  master_id, business_unit_id } );
			if (exists(_conn))
			{
				#region Exists
				/*
			inventory_branch temp = new inventory_branch(master_id, business_unit_id);
			if (onhand_qty < 0)
				{
				onhand_qty = 0;
				dollar_balance = 0;
				}
			l.section_id = section_id;
			l.business_unit_id = business_unit_id;
			l.member_id = member_id;
			l.table = "inventory_branch";
			l.table_id = master_id;
			if (dollar_balance == 0 && (temp.onhand_qty == 0 || onhand_qty == 0))
				{
				dollar_balance = Convert.ToDouble(onhand_qty*temp_cost);
				}A
			if(dollar_balance == 0)
				{
				onhand_qty	= 0;
				}
			if(onhand_qty == 0)
				{
				dollar_balance = 0;
				}
			if (temp.min_qty != min_qty)
				{
				l.action_id = 1;
				l.value_old = temp.min_qty;
				l.value_new = min_qty;
				l.save();
				}
			if (temp.max_qty != max_qty)
				{
				l.action_id = 2;
				l.value_old = temp.max_qty;
				l.value_new = max_qty;
				l.save();
				}
			if (temp.onhand_qty != onhand_qty)
				{
				l.action_id = 3;
				l.value_old = temp.onhand_qty;
				l.value_new = onhand_qty;
				l.save();
				}
			if (temp.dollar_balance != dollar_balance)
				{
				dollar_balance = dollar_balance == 0 ? Convert.ToDouble(onhand_qty*temp_cost) : dollar_balance;
				l.action_id = 6;
				l.value_old = temp.dollar_balance;
				l.value_new = dollar_balance;
				l.save();
				}
			 */
				var temp_onhand_qty = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(qty), 0) FROM inventory_location WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] { master_id, business_unit_id });
				if (dollar_balance == 0)
				{
					var this_cost = Toolbox.doSQL_double(_conn, @"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] { master_id, business_unit_id });
					onhand_qty = temp_onhand_qty;
					dollar_balance = this_cost * temp_onhand_qty;
				}
				else if (onhand_qty == 0 && temp_onhand_qty > 0)
				{
					var this_cost = Toolbox.doSQL_double(_conn, @"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] { master_id, business_unit_id });
					onhand_qty = temp_onhand_qty;
					dollar_balance = this_cost * temp_onhand_qty;
				}
				else if (onhand_qty == 0)
				{
					dollar_balance = 0;
				}
				if (onhand_qty < 0 && dollar_balance > 0)
				{
					dollar_balance = 0 - dollar_balance;
				}

				if (onhand_qty > 0 && dollar_balance < 0)
				{
					dollar_balance = 0 + dollar_balance;
				}
				if (dollar_balance > 0 && onhand_qty <= 0 || onhand_qty < 0 && dollar_balance >= 0)
				{
					Toolbox.do_catch_error(new Exception("Dollar balance can't be positive when the on-hand quantity is less than zero - business_unit_id:" + business_unit_id), 711);
					throw new Exception("Dollar balance can't be positive when the on-hand quantity is less than zero");
				}
				if (dollar_balance < 0 && onhand_qty >= 0 || onhand_qty > 0 && dollar_balance <= 0)
				{
					Toolbox.do_catch_error(new Exception("Dollar balance can't be negative when the on-hand quantity is greater than zero - business_unit_id:" + business_unit_id), 711);
					throw new Exception("Dollar balance can't be negative when the on-hand quantity is greater than zero");
				}
				Toolbox.doSQL_void(_conn, @" UPDATE inventory_branch SET min_qty = @v0 , max_qty = @v1 , onhand_qty = @v2 , dollar_balance = @v6 , preferred = @v3 , is_consumable = @v7  WHERE master_id = @v4  AND business_unit_id = @v5  LIMIT 1", new object[] { min_qty, max_qty, onhand_qty, Convert.ToInt32(preferred), master_id, business_unit_id, dollar_balance, is_consumable ? 1 : 0 });
				#endregion Exists
			}
			else
			{
				#region Doesn't Exist
				/*
			dollar_balance = dollar_balance == 0 ? Convert.ToDouble(onhand_qty*temp_cost) : dollar_balance;
			if (onhand_qty < 0)
				{
				onhand_qty = 0;
				dollar_balance = 0;
				}
			l.table = "inventory_branch";
			l.table_id = master_id;
			l.section_id = section_id;
			l.business_unit_id = business_unit_id;
			l.member_id = member_id;
			l.action_id = 1;
			l.value_old = "NULL";
			l.value_new = min_qty;
			l.save();
			
			if(dollar_balance == 0)
				{
				onhand_qty	= 0;
				}
			if(onhand_qty == 0)
				{
				dollar_balance = 0;
				}

			l.action_id = 2;
			l.value_old = "NULL";
			l.value_new = max_qty;
			l.save();

			l.action_id = 3;
			l.value_old = "NULL";
			l.value_new = onhand_qty;
			l.save();

			l.action_id = 6;
			l.value_old = "NULL";
			l.value_new = dollar_balance;
			l.save();

			 */
				if (dollar_balance == 0 || onhand_qty == 0)
				{
					dollar_balance = 0;
					onhand_qty = 0;
				}
				Toolbox.doSQL_void(_conn, @" INSERT INTO inventory_branch ( master_id, business_unit_id, min_qty, max_qty, onhand_qty, preferred, dollar_balance ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6  )", new object[] { master_id, business_unit_id, 0, 0, onhand_qty, Convert.ToInt32(preferred), dollar_balance });
				#endregion Doesn't Exist
			}
		}
	}
	public class consignment
	{
		public int id { get; set; }

		public int business_unit_id { get; set; }

		public int master_id { get; set; }

		public int customer_id { get; set; }

		//'Waiting to be Quoted', 'Quoted', 'In Process', 'Testing', 'Returned - Repaired', 'Returned - Not Repaired', 'Mothballed'

		public enum StatusType
		{
			WaitingToBeQuoted,
			Quoted,
			InProcess,
			Testing,
			ReturnedRepaired,
			ReturnedNotRepaired,
			MothBalled
		};

		public StatusType status { get; set; }

		public DateTime date_entered { get; set; }

		public DateTime? date_returned { get; set; }

		public string serial { get; set; }

		public double repair_price { get; set; }

		public string note { get; set; }


		public consignment()
		{
			// Constructor
		}
		/// <summary>
		/// Default way of loading, provide the line id
		/// </summary>
		/// <param name="r_id"></param>
		public consignment(int r_id)
		{
			id = r_id;
			load();
		}
		public consignment(int m_id, int cu_id, int co_id)
		{
			master_id = m_id;
			customer_id = cu_id;
			business_unit_id = co_id;
			load();
		}
		public string Convert_Status(StatusType s)
		{
			switch (s)
			{
				case StatusType.WaitingToBeQuoted: return "Waiting to be Quoted";
				case StatusType.Quoted: return "Quoted";
				case StatusType.InProcess: return "In Process";
				case StatusType.Testing: return "Testing";
				case StatusType.ReturnedRepaired: return "Returned - Repaired";
				case StatusType.ReturnedNotRepaired: return "Returned - Not Repaired";
				case StatusType.MothBalled: return "Mothballed";
				default: return "";
			}
		}
		public StatusType Convert_Status(string s)
		{
			var __status = StatusType.WaitingToBeQuoted;
			switch (s)
			{
				case "Waiting to be Quoted":
					__status = StatusType.WaitingToBeQuoted;
					break;
				case "Quoted":
					__status = StatusType.Quoted;
					break;
				case "In Process":
					__status = StatusType.InProcess;
					break;
				case "Testing":
					__status = StatusType.Testing;
					break;
				case "Returned - Repaired":
					__status = StatusType.ReturnedRepaired;
					break;
				case "Returned - Not Repaired":
					__status = StatusType.ReturnedNotRepaired;
					break;
				case "Mothballed":
					__status = StatusType.MothBalled;
					break;
			}
			return __status;
		}
		public void save()
		{
			if (id == 0)
			{
				//insert
				Toolbox.doSQL_void(@" INSERT INTO inventory_consignment ( business_unit_id, master_id, customer_id, status, date_entered, serial, repair_price ) VALUES ( @v0 , @v1 , @v2 , @v3 , NOW(), @v4 , @v5  )", new object[] { business_unit_id, master_id, customer_id, Convert_Status(status), serial, repair_price });
			}
			else
			{
				//edit
				Toolbox.doSQL_void(@" UPDATE inventory_consignment SET customer_id = @v0 , status = @v1 , serial = @v2 , date_returned = @v6,  repair_price = @v3 , note = @v4 , master_id = @v7  WHERE id = @v5  LIMIT 1", new object[] { customer_id, Convert_Status(status), serial, repair_price, note, id, date_returned, master_id });
			}
		}
		public void delete()
		{
			Toolbox.doSQL_void(@"DELETE FROM inventory_consignment WHERE id = @v0  LIMIT 1", new object[] { id });
		}
		public bool exists()
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(id) FROM inventory_consignment WHERE master_id = @v0  AND business_unit_id = @v1  AND customer_id = @v2  AND serial = @v3 ", new object[] { master_id, business_unit_id, customer_id, serial });
			return c > 0;
		}
		public void load()
		{
			DataRow dr;
			if (id > 0)
			{
				dr = Toolbox.doSQL_dt(@"SELECT * FROM inventory_consignment  WHERE id =@v0", new object[] { id }).Rows[0];
			}
			else if (master_id > 0 && customer_id > 0 && business_unit_id > 0)
			{
				dr = Toolbox.doSQL_dt(@"SELECT * FROM inventory_consignment WHERE master_id = @v0  AND business_unit_id = @v1  AND customer_id = @v2  LIMIT 1", new object[] { master_id, business_unit_id, customer_id }).Rows[0];
			}
			else
			{
				throw new Exception("Nothing supplied to load from");
			}
			id = Convert.ToInt32(dr["id"]);
			business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
			date_entered = Convert.ToDateTime(dr["date_entered"]);
			customer_id = Convert.ToInt32(dr["customer_id"]);
			repair_price = Convert.ToDouble(dr["repair_price"]);
			status = Convert_Status(dr["status"].ToString());
			note = dr["note"].ToString();
			serial = dr["serial"].ToString();
			master_id = Convert.ToInt32(dr["master_id"]);
			if (dr["date_returned"] != DBNull.Value)
			{
				date_returned = Convert.ToDateTime(dr["date_returned"]);
			}
			else
			{
				date_returned = null;
			}
		}
	}
	public class kit_item
	{
		private int _row_id = 0;

		public kit_item()
		{
			hdr_id = 0;
		}

		public int hdr_id { get; set; }

	}
	public class location
	{
		public bool loaded { get; set; }
		public bool log_is_manual { get; set; }
		public bool alert_worthy { get; set; }

		public int? id { get; set; }
		public int master_id { get; set; }
		public int business_unit_id { get; set; }
		public int location_master_id { get; set; }

		/// <summary>
		/// Used for logging saved changes, defines the page that saved it.
		/// </summary>
		public int section_id { get; set; }
		public int member_id { get; set; }
		public double qty { get; set; }
		public double min { get; set; }
		public double max { get; set; }
		public DateTime? timestamp { get; set; }
		public ArrayList items { get; set; }
		public location_master this_master { get; set; }
		public location()
		{
			alert_worthy = false;
		}

		/// <summary>
		/// Loading from an already known row ID
		/// </summary>
		/// <param name="l_id"></param>
		public location(int l_id)
		{
			using (var conn = Toolbox.connect())
			{
				alert_worthy = false;
				load(l_id, conn);
			}
		}
		public location(int masterId, location_master locationMasterObj)
		{
			var l_id = Toolbox.doSQL_int($@"SELECT IFNULL(MAX(id), 0) FROM inventory_location WHERE master_id = {masterId} AND business_unit_id = {locationMasterObj.business_unit_id} AND location_master_id = {locationMasterObj.id}", new object[] { });
			if (l_id > 0)
			{
				load(l_id);
			}
		}
		public location(int l_id, MySqlConnection _conn)
		{
			alert_worthy = false;
			load(l_id, _conn);
		}
		public location(int l_id, int _business_unit_id, int m_id)
		{
			using (var conn = Toolbox.connect())
			{
				load(l_id, _business_unit_id, m_id, conn);
			}
		}
		/// <summary>
		/// Built in safety, it will check if the id is owned by the supplied company id, if not, it will make it.
		/// </summary>
		/// <param name="l_id"></param>
		/// <param name="c_id"></param>
		public location(int l_id, int _business_unit_id, int m_id, MySqlConnection _conn)
		{
			alert_worthy = false;
			load(l_id, _business_unit_id, m_id, _conn);
		}

		public location(IEnumerable l_ids)
		{
			using (var conn = Toolbox.connect())
			{
				alert_worthy = false;
				items = new ArrayList();
				mass_load(l_ids, conn);
			}
		}
		public location(IEnumerable l_ids, MySqlConnection _conn)
		{
			alert_worthy = false;
			items = new ArrayList();
			mass_load(l_ids, _conn);
		}
		public static void zero_out(int _section_id, int _location_master_id, int _business_unit_id, int _master_id, double _cost, int _user_id)
		{
			using (var conn = Toolbox.connect())
			{
				zero_out(_section_id, _location_master_id, _business_unit_id, _master_id, _cost, _user_id, conn);
			}
		}
		public static void zero_out(int _section_id, int _location_master_id, int _business_unit_id, int _master_id, double _cost, int _user_id, MySqlConnection _conn)
		{
			var ib = new branch(_master_id, _business_unit_id, _conn);
			var il = new location(_location_master_id, _business_unit_id, _master_id, _conn);
			var prev_qty = il.qty;
			il.log_is_manual = true;
			il.member_id = _user_id;
			ib.member_id = _user_id;
			var pre_qty = il.qty;
			il.qty = 0;
			il.section_id = _section_id;
			il.save(_conn);
			il.update_branch(3, ib.dollar_balance, pre_qty, _cost, ib, _conn);
		}
		public void save()
		{
			using (var conn = Toolbox.connect())
			{
				save(conn);
			}
		}
		public void save(MySqlConnection _conn)
		{
			var i_m = new location_master(location_master_id, _conn);
			var inv = new inventory();
			if (master_id != 0)
			{
				// Items needed here from inventory
				// - cost price (from get_current_cost) -- Needs current
				// - Force Cost to 0.01 (is_exclude) (from inventory_tag)
				// - description_full (from inventory_description)
				var dt = Toolbox.doSQL_dt(_conn, @" SELECT b.is_exclude, c.description, GET_CURRENT_COST(@v0 , @v1 ) cost FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id LEFT JOIN inventory_description c ON a.master_id = c.master_id WHERE a.master_id = @v0  LIMIT 1 ", new object[] { master_id, business_unit_id });
				var dr = dt.Rows[0];
				inv.is_exclude = Convert.ToBoolean(dr["is_exclude"]);
				inv.description_full = dr["description"].ToString();
				inv.cost_price_branch = Convert.ToDouble(dr["cost"]);
				if (inv.is_exclude)
				{
					qty = 0;
				}

				if (inv.cost_price_branch < 0)
				{
					throw new Exception("A location with a qty for an item must have a cost greater than $0.00 saved for that item");
				}

				if (inv.cost_price_branch == 0 && qty != 0)
				{
					throw new Exception("A location with a qty for an item must have a cost greater than $0.00 saved for that item");
				}

				#region insert 
				if (id == null)
				{
					id = Toolbox.doSQL_return_id(_conn, @" INSERT INTO inventory_location ( master_id, business_unit_id, location_master_id, qty, min, max ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5  )", new object[] { master_id, business_unit_id, location_master_id, qty, min, max });
					update_branch_locations(master_id, business_unit_id, _conn);
				}
				#endregion insert 
				#region update 
				else
				{
					if (!loaded)
					{
						throw new Exception("Nothing is loaded to save");
					}
					#region logging
					var l = new NELog();
					var temp = new location((int)id, _conn);
					l.section_id = section_id;
					l.business_unit_id = business_unit_id;
					l.member_id = member_id;
					l.is_manual = log_is_manual;
					l.table = OpsLog.Table.InventoryLocation;
					l.table_id = id;
					l.alt_table_id = master_id;
					if (temp.min != min)
					{
						l.action_id = OpsLog.Action.AdjustedMinimumQuantity;
						l.value_old = temp.min;
						l.value_new = min;
						l.save(_conn);
					}
					if (temp.max != max)
					{
						l.action_id = OpsLog.Action.AdjustedMaximumQuantity;
						l.value_old = temp.max;
						l.value_new = max;
						l.save(_conn);
					}
					if (temp.qty != qty)
					{
						if (alert_worthy)
						{
							var bma = new bm_alert();
							bma.business_unit_id = business_unit_id;
							bma.master_id = master_id;
							bma.member_id = member_id;
							bma.location_id = id;
							bma.location_name = i_m.type_id == 1 ? "Internal - " + i_m.name : "External - " + i_m.name;
							bma.count_old = temp.qty;
							bma.count_new = qty;
							bma.description = inv.description_full;
							bma.send_alert();
						}
						l.action_id = OpsLog.Action.AdjustedLocationQuantity;
						l.value_old = temp.qty;
						l.value_new = qty;
						l.save(_conn);
					}
					#endregion logging
					if (qty < 0)
					{
						// add item to correction table

						try
						{
							var iil = new incorrect_levels();
							iil.master_id = master_id;
							iil.location_id = Convert.ToInt32(temp.id);
							iil.incorrect_qty = qty;
							iil.reported_qty = Convert.ToDouble(temp.qty - qty);
							iil.member_id = member_id;
							iil.date = System.DateTime.Today;
							iil.id = 0;
							iil.save();
						}
						catch { }

						// Send email to branch purchaser / branch manager / controller
						var em = new NeEMail();
						var businessUnit = new NeBusinessUnit(business_unit_id);
						// Get Purchaser / BM / inventory admin
						var shipper_em = Toolbox.doSQL_string(_conn, @"SELECT GROUP_CONCAT(IFNULL(member_neemail, '') SEPARATOR ';') FROM member WHERE member_membertype_id = 28 AND business_unit_id = @v0 AND membeR_neemail NOT LIKE '%nomail%' AND member_status = 'Active'", new object[] { business_unit_id });
						//var purchaser_em = Toolbox.doSQL_string(_conn, @"SELECT IFNULL(MIN(member_neemail),'') FROM member WHERE member_membertype_id = 9 AND business_unit_id = @v0  AND member_status = 'Active'", new object[] { business_unit_id });
						var bm_em = Toolbox.doSQL_string(_conn, @"SELECT IFNULL(MIN(member_neemail),'') FROM member WHERE member_membertype_id = 5 AND business_unit_id = @v0  AND member_status = 'Active'", new object[] { business_unit_id });
						var controller_em = Toolbox.doSQL_string(_conn, @"SELECT IFNULL(MIN(member_neemail),'') FROM member  WHERE member_membertype_id = 67 AND member_status = 'Active'", null);
						var user = new NeMember(member_id);
						em.To = businessUnit.purchaser.NEEmail != "" ? businessUnit.purchaser.NEEmail : "devnotification@" + Toolbox.app_setting("DomainForEmail");
						em.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
						em.CC = shipper_em == "" || shipper_em.Contains("nomail") ? "" : shipper_em;
						em.Bcc = businessUnit.purchaser.NEEmail == bm_em ? controller_em : bm_em + ";" + controller_em;
						//em.Bcc					= "mhyde@newelectric.com";
						em.Subject = "Inventory Location Quantity Below Zero";
						em.Body = $@"
This is a notification to let you know that <b>{
								user.FullName
							}</b> just made the quantity in location <b>{i_m.name}</b>, for part <b>{master_id}</b>, go from <b>{
								temp.qty
							}</b> to <b>{qty}</b> in their last transaction.
";
						em.isHTML = true;
						em.Send();
					}
					Toolbox.doSQL_void(_conn, @" UPDATE inventory_location SET location_master_id = @v0 , qty = @v1 , min = @v2 , max = @v3  WHERE id = @v4  LIMIT 1", new object[] { location_master_id, qty, min, max, id });
					update_branch_locations(master_id, business_unit_id, _conn);
				}
				#endregion update 
			}
		}
		public void update_branch_locations(object _master_id, object _business_unit_id)
		{
			using (var conn = Toolbox.connect())
			{
				update_branch_locations(_master_id, _business_unit_id, conn);
			}
		}
		public void update_branch_locations(object _master_id, object _business_unit_id, MySqlConnection _conn)
		{
			Toolbox.doSQL_void(_conn, @" UPDATE inventory_branch a SET a.int_locations = (SELECT GROUP_CONCAT(c.name SEPARATOR '|') FROM inventory_location b LEFT JOIN inventory_location_master c ON b.location_master_id = c.id WHERE b.master_id = a.master_id AND b.business_unit_id = a.business_unit_id and c.type_id = 1), a.ext_locations = (SELECT GROUP_CONCAT(c.name SEPARATOR '|') FROM inventory_location b LEFT JOIN inventory_location_master c ON b.location_master_id = c.id WHERE b.master_id = a.master_id AND b.business_unit_id = a.business_unit_id and c.type_id = 2) WHERE a.master_id = @v0  AND a.business_unit_id = @v1 ", new object[] { _master_id, _business_unit_id });
		}
		public void update_branch(int type, double dbal_before, double qty_incoming, double cost_incoming, branch _preBranch)
		{
			using (var conn = Toolbox.connect())
			{
				update_branch(type, dbal_before, qty_incoming, cost_incoming, _preBranch, conn);
			}
		}
		public void update_branch(int type, double dbal_before, double qty_incoming, double cost_incoming, branch _preBranch, MySqlConnection _conn)
		{
			if (qty_incoming == 0 || cost_incoming == 0 || master_id == 0)
			{
				return;
			}

			var onhandQtyBefore = _preBranch.onhand_qty;
			var dbBefore = _preBranch.dollar_balance;
			var brObject = new branch(master_id, business_unit_id); // MH:The reason I am instantiating a new BR object is this is after the mysql trigger updates the onhand qty.
			var absQtyIncoming = Math.Abs(qty_incoming);
			var thisCost = cost_incoming != 0
												? cost_incoming
												: Toolbox.doSQL_double(_conn, @"SELECT GET_CURRENT_COST(@v0 ,@v1 )", new object[] { master_id, business_unit_id });
			var inventoryObj = new inventory();
			inventoryObj.Load(master_id, business_unit_id);
			if (inventoryObj.is_exclude) return;

			var newDollarBalanceBase = qty_incoming * cost_incoming;
			var oldPlusNewDollarBalance = _preBranch.dollar_balance + newDollarBalanceBase;
			var isAdding = type == 2;
			var isRemoving = type == 3;
			if (isAdding || isRemoving)
			{
				var qtyNegativePassing = onhandQtyBefore > 0 && onhandQtyBefore + qty_incoming < 0;
				var qtyPositivePassing = onhandQtyBefore < 0 && onhandQtyBefore + qty_incoming > 0;
				var dbNegativePassing = _preBranch.dollar_balance > 0 && oldPlusNewDollarBalance < 0;
				var dbPositivePassing = _preBranch.dollar_balance < 0 && oldPlusNewDollarBalance > 0;
				double remainderOnhandQty = 0;
				if (qtyNegativePassing && isRemoving || qtyPositivePassing && isAdding || dbNegativePassing && isRemoving || dbPositivePassing && isAdding)
				{
					remainderOnhandQty = onhandQtyBefore + qty_incoming; // This is if it's passing through 0, you want only the final amount... the remainder
				}

				if (isRemoving && (_preBranch.onhand_qty - qty_incoming == 0 || dbal_before - qty_incoming * qty_incoming == 0))
				{
					// qty_incoming = 0;
					brObject.onhand_qty = 0;
				}
				else
				{
					if (isAdding && qty_incoming != 0) // adding
					{
						brObject.dollar_balance = remainderOnhandQty != 0
													? remainderOnhandQty * cost_incoming
													: _preBranch.dollar_balance + cost_incoming * absQtyIncoming;
					}
					else if (isRemoving && qty_incoming != 0) // removing
					{
						brObject.dollar_balance = remainderOnhandQty != 0
													? remainderOnhandQty * cost_incoming
													: _preBranch.dollar_balance - cost_incoming * absQtyIncoming;
					}
					if (_preBranch.onhand_qty == 0 && qty_incoming == 0)
					{
						brObject.dollar_balance = 0;
					}
				}
				brObject.save(_conn);
			}
			else
			{
				brObject.dollar_balance = _preBranch.onhand_qty == 0
												? 0
												: dbal_before;
				brObject.save(_conn);
			}
			var dbAfter = brObject.dollar_balance;
			var dbDiff = dbAfter - dbBefore;
			var locationObj = id == null ? new location() : new location((int)id, _conn);
			this_master = this_master == null ? new location_master(locationObj.location_master_id) : this_master;
			var qtyBefore = id == null ? 0 : locationObj.qty - (isRemoving ? qty_incoming * -1 : Math.Abs(qty_incoming));
			var qtyAfter = id == null ? 0 : locationObj.qty;
			var qtyDiff = id == null ? 0 : qtyAfter - qtyBefore;
			var m = new NeMember(member_id);
			var branch = new NeBusinessUnit(business_unit_id);
			var sectionName = Toolbox.doSQL_string(_conn, @"SELECT name FROM log_section WHERE id = @v0 ", new object[] { section_id });
			var l = new NELog
			{
				section_id = section_id,
				business_unit_id = business_unit_id,
				member_id = member_id,
				is_manual = log_is_manual,
				table = "inventory_location",
				table_id = id,
				alt_table_id = master_id
			};

			if (dbal_before != brObject.dollar_balance)
			{
				var is_manual_string = log_is_manual == true ? "True" : "False";
				l.action_id = OpsLog.Action.AdjustedDollarBalance;
				l.value_old = dbal_before;
				l.value_new = brObject.dollar_balance;
				l.save();
				Toolbox.doSQL_void(_conn, @"
INSERT INTO inventory_usage 
	( 
	dt, 
	member_name, 
	business_unit_id, 
	branch, 
	master_id, 
	description, 
	location, 
	section, 
	qty_before, 
	qty_after, 
	qty_diff, 
	db_before, 
	db_after, 
	db_diff, 
	is_manual, 
	cost_per 
	) 
VALUES 
	( 
	NOW(), 
	@v0, 
	@v1, 
	@v2, 
	@v3, 
	@v4, 
	@v5, 
	@v6, 
	@v7, 
	@v8, 
	@v9, 
	@v10, 
	@v11, 
	@v12, 
	@v13, 
	@v14  
	)", new object[]
					{
					m.FullName,						 // @v0
					business_unit_id,				 // @v1
					branch.name,					 // @v2
					inventoryObj.master_id,			 // @v3
					inventoryObj.description_full,	 // @v4
					this_master.name,				 // @v5
					sectionName,					 // @v6
					Math.Round(qtyBefore, 5),		 // @v7
					Math.Round(qtyAfter, 5),		 // @v8
					Math.Round(qtyDiff, 5),			 // @v9
					Math.Round(dbBefore, 5),		 // @v10
					Math.Round(dbAfter, 5),			 // @v11
					Math.Round(dbDiff, 5),			 // @v12
					is_manual_string,					 // @v13 String
					thisCost						 // @v14
					});
			}
			if (onhandQtyBefore == _preBranch.onhand_qty) return;
			l.table = OpsLog.Table.InventoryBranch;
			l.table_id = master_id;
			l.alt_table_id = business_unit_id;
			l.action_id = OpsLog.Action.AdjustedBranchOnHandQuantity;
			l.value_old = onhandQtyBefore;
			l.value_new = _preBranch.onhand_qty;
			l.save();
		}
		public void mass_save()
		{
			if (items.Count > 1)
			{
				foreach (location inv_l in items)
				{
					inv_l.save();
				}
			}
		}
		private void load(object l_id)
		{
			using (var conn = Toolbox.connect())
			{
				load(l_id, conn);
			}
		}
		private void load(object l_id, MySqlConnection _conn)
		{
			if (l_id != null)
			{
				var dt = Toolbox.doSQL_dt(_conn, @"SELECT * FROM inventory_location  WHERE id =@v0 limit 1 ", new object[] { l_id });
				if (dt.Rows.Count <= 0) return;
				var r = dt.Rows[0];
				id = Convert.ToInt32(r["id"]);
				master_id = Convert.ToInt32(r["master_id"]);
				business_unit_id = (int)r["business_unit_id"];
				location_master_id = Convert.ToInt32(r["location_master_id"]);
				qty = Convert.ToDouble(r["qty"]);
				min = r["min"] == DBNull.Value ? 0 : Convert.ToDouble(r["min"]);
				max = r["max"] == DBNull.Value ? 0 : Convert.ToDouble(r["max"]);
				timestamp = r["timestamp"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["timestamp"]);
				this_master = new location_master(location_master_id, _conn);
				loaded = true;
			}
			else
			{
				loaded = false;
			}
		}
		private void load(object l_id, int _business_unit_id, object m_id)
		{
			using (var conn = Toolbox.connect())
			{
				load(l_id, _business_unit_id, m_id, conn);
			}
		}
		private void load(object l_id, int _business_unit_id, object m_id, MySqlConnection _conn)
		{
			if (l_id != null && _business_unit_id != null)
			{
				var dt = Toolbox.doSQL_dt(_conn, @"SELECT * FROM inventory_location WHERE id = @v0  AND business_unit_id = @v1  AND master_id = @v2  ", new object[] { l_id, _business_unit_id, m_id });
				if (dt.Rows.Count == 0)
				{
					// Make one... but first check if the location master id is being passed
					if (Toolbox.doSQL_int(_conn, @"SELECT IFNULL(MIN(id),0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2  ", new object[] { l_id, _business_unit_id, m_id }) != 0)
					{
						// You want to check if what you have is in fact a location master id, if not it's still a location id (think loops), and the location master still needs to be fetched
						l_id = Toolbox.doSQL_int(_conn, @"SELECT id FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2  ", new object[] { l_id, _business_unit_id, m_id });
					}
					else
					{
						var im = new location_master(Convert.ToInt32(l_id), _conn);
						var il = new location();
						if (im.id == null) return;
						il.master_id = Convert.ToInt32(m_id);
						il.business_unit_id = _business_unit_id;
						il.location_master_id = (int)im.id;
						il.min = 0;
						il.max = 0;
						il.save(_conn);
						l_id = il.id;
					}
					load(l_id, _conn);
				}
				else
				{
					// Load existing
					load(l_id, _conn);
				}
			}
			else
			{
				loaded = false;
			}
		}
		public void xfer_using_master_id(int master_id, int business_unit_id, int from_location_master_id, int to_location_master_id, double quantity, NeMember m)
		{
			using (var conn = Toolbox.connect())
			{
				xfer_using_master_id(master_id, business_unit_id, from_location_master_id, to_location_master_id, quantity, m, conn);
			}
		}
		public void xfer_using_master_id(int master_id, int _business_unit_id, int from_location_master_id, int to_location_master_id, double quantity, NeMember m, MySqlConnection _conn)
		{
			if (master_id != 0 && _business_unit_id != 0 && from_location_master_id != 0 && to_location_master_id != 0 && quantity != 0 && m != null)
			{
				var from_id = Toolbox.doSQL_int(_conn, @"Select ifnull((Select id from inventory_location where master_id = @v0  and business_unit_id = @v1  and location_master_id = @v2  limit 1),0)", new object[] { master_id, _business_unit_id, from_location_master_id });
				var to_id = Toolbox.doSQL_int(_conn, @"Select ifnull((Select id from inventory_location where master_id = @v0  and business_unit_id = @v1  and location_master_id = @v2  limit 1),0)", new object[] { master_id, _business_unit_id, to_location_master_id });

				if (to_id == 0)
				{
					var il = new location
					{
						business_unit_id = _business_unit_id,
						location_master_id = to_location_master_id,
						master_id = master_id,
						member_id = m.id
					};
					il.save(_conn);
					to_id = Convert.ToInt32(il.id);
				}
				xfer(from_id, to_id, quantity, m, _conn);

			}
		}


		public void xfer(int from_id, int to_id, double quantity, NeMember m)
		{
			using (var conn = Toolbox.connect())
			{
				xfer(from_id, to_id, quantity, m, conn);
			}
		}
		public void xfer(int from_id, int to_id, double quantity, NeMember m, MySqlConnection _conn)
		{
			var from = new location(from_id, _conn);
			var to = new location(to_id, _conn);

			var i = new inventory();
			i.Load(to.master_id, to.business_unit_id);

			from.qty = from.qty - quantity;
			from.log_is_manual = true;
			from.member_id = m.id;
			from.section_id = section_id;
			from.save();

			to.qty = to.qty + quantity;
			to.log_is_manual = true;
			to.member_id = m.id;
			to.section_id = section_id;
			to.save(_conn);

			var st = new Nestock_transfer
			{
				business_unit_id = to.business_unit_id,
				cost = i.cost_price_branch,
				description = i.description_full,
				from_id = @from.location_master_id,
				from_location_id = @from.location_master_id,
				to_id = to.location_master_id,
				to_location_id = to.location_master_id,
				master_id = Convert.ToInt32(i.master_id),
				member_id = m.id,
				note = "Manual Transfer",
				quantity = quantity,
				type_id = 3
			};
			st.save();
		}
		public void delete()
		{
			using (var conn = Toolbox.connect())
			{
				delete(false, conn);
			}
		}
		public void delete(bool from_merge)
		{
			using (var conn = Toolbox.connect())
			{
				delete(from_merge, conn);
			}
		}
		public void delete(bool from_merge, MySqlConnection _conn)
		{
			var inv_m = new location_master(location_master_id, _conn);
			if (inv_m.type_id == 1)
			{
				var c = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM inventory_location  WHERE master_id =@v0 AND location_master_id !=@v1  AND business_unit_id =@v2 ", new object[] { master_id, location_master_id, business_unit_id });
				if (c == 0 && !from_merge)
				{
					throw new Exception("Cannot delete the last location mapping for a part.");
				}
			}
			if (qty > 0)
			{
				throw new Exception("Cannot delete a location with a quantity other than zero.");
			}
			if (id != 0 && id != null)
			{
				Toolbox.doSQL_void(_conn, @"DELETE FROM inventory_location WHERE id = '" + id + "' LIMIT 1", null);
			}
			update_branch_locations(master_id, business_unit_id, _conn);
		}
		private void mass_load(IEnumerable l_ids)
		{
			using (var conn = Toolbox.connect())
			{
				mass_load(l_ids, conn);
			}
		}
		private void mass_load(IEnumerable l_ids, MySqlConnection _conn)
		{
			foreach (var l in l_ids)
			{
				var i_lo = new location(Convert.ToInt32(l), _conn);
				items.Add(i_lo);
			}
		}
		public void print_location_barcode_label(int copies)
		{
			try
			{
				var printDoc = new PrintDocument();
				var WOPrinter = new NeBusinessUnit(business_unit_id);
				var yy = new PaperSize("Custom Paper Size", 220, 99);
				printDoc.DefaultPageSettings.PaperSize = yy;
				printDoc.DefaultPageSettings.Margins.Left = 1;
				printDoc.DefaultPageSettings.PrinterSettings.Copies = (short)copies;
				printDoc.PrinterSettings.PrinterName = WOPrinter.BarCodePrinter;
				printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
				printDoc.Print();
			}
			catch (Exception ee)
			{
				Toolbox.do_catch_error(ee, 711);
				throw new Exception("Printing Location Barcode Failed");
			}
		}
		private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			var rightAlign = new StringFormat();
			rightAlign.Alignment = StringAlignment.Far;
			rightAlign.LineAlignment = StringAlignment.Far;
			var leftAlign = new StringFormat();
			leftAlign.Alignment = StringAlignment.Near;
			leftAlign.LineAlignment = StringAlignment.Near;
			var printFont = new Font("Arial", 9);
			var printFont1 = new Font("Arial", 8);
			var printFontdesc = new Font("Arial", 9);
			var barcodefont = new Font("Free 3 of 9 Extended", 24);
			var rect = new Rectangle(10, 30, 195, 50);
			var br = new SolidBrush(Color.Black);
			e.Graphics.DrawRectangle(Pens.Transparent, rect);
			e.Graphics.DrawString(this_master.name, printFontdesc, br, rect);
			e.Graphics.DrawString("*005-" + id + "*", barcodefont, br, 15, 5);
		}
		public class bm_alert
		{
			private int? _location_id = 0;
			public int? location_id { get { return _location_id; } set { _location_id = value; } }
			public int business_unit_id { get; set; }

			public int master_id { get; set; }

			public int member_id { get; set; }

			public double count_old { get; set; }

			public double count_new { get; set; }

			private string _description = "";
			public string description { get { return _description; } set { _description = value; } }
			private string _location_name = "";
			public string location_name { get { return _location_name; } set { _location_name = value; } }

			private double dollar_balance_part_old = 0;
			private double dollar_balance_part_new = 0;
			private double dollar_balance_location_new = 0;
			private double dollar_balance_total = 0;
			private int n_manual_adjustments = 0;

			public bm_alert()
			{
				count_new = 0;
				count_old = 0;
				member_id = 0;
				master_id = 0;
				business_unit_id = 0;
			}

			public void send_alert()
			{
				var employee = new NeMember(member_id);
				var branch = new NeBusinessUnit(business_unit_id);
				var bm = employee.id != 711 ? branch.branch_manager : employee;
				build_alert();
				var e = new NeEMail();
				e.From = employee.NEEmail == "" ? "administrator@" + Toolbox.app_setting("DomainForEmail") : employee.NEEmail;
				e.To = bm.NEEmail == "" ? "mhyde@" + Toolbox.app_setting("DomainForEmail") : bm.NEEmail;
				e.Subject = $"Manual Inventory Qty Adjustment - Part #{master_id} - By: {employee.FullName}";
				e.isHTML = true;
				//e.Bcc				= "mhyde@newelectric.com";
				e.Body = string.Format(@"
<div style='font-family: arial;'>This is an automatically generated alert to let you know {0} just manually adjusted the onhand quantity for part #: {1}.</div>

<table style='font-family: arial;' cellpadding='3' cellspacing='0'>
	<tr>
		<td><b>Date:</b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Part #:</b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Description:</b></td>
		<td>{3}</td>
	</tr>
	<tr>
		<td><b>Person Executing the Change:</b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Location:</b></td>
		<td>{10}</td>
	</tr>
	<tr>
		<td><b>Old Count:</b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>New Count:</b></td>
		<td>{5}</td>
	</tr>
	<tr>
		<td><b>Old Dollar Balance for this part:</b></td>
		<td>{11:C2}</td>
	</tr>
	<tr>
		<td><b>New Dollar Balance for this part:</b></td>
		<td>{6:C2}</td>
	</tr>
	<tr>
		<td><b>New Dollar Balance for this location:</b></td>
		<td>{7:C2}</td>
	</tr>
	<tr>
		<td><b>Number of manual onhand adjustments since last annual count:</b></td>
		<td>{8}</td>
	</tr>
	<tr>
		<td><b>Total Branch Inventory Dollar Balance:</b></td>
		<td>{9:C2}</td>
	</tr>
</table>
",
					employee.FullName,
					master_id,
					Toolbox.MySQLNow_long(),
					description,
					count_old,
					count_new,
					dollar_balance_part_new,
					dollar_balance_location_new,
					n_manual_adjustments,
					dollar_balance_total,
					location_name,
					dollar_balance_part_old
				);
				e.Send();
			}
			public void build_alert()
			{
				// cost * qty in location
				var current_cost = Toolbox.doSQL_double(@"SELECT GET_current_COST(@v0 , @v1 )", new object[] { master_id, business_unit_id });
				dollar_balance_location_new = current_cost * count_new;
				var dr = Toolbox.doSQL_dt(@"SELECT IFNULL(MAX(value_old),0) old, IFNULL(MAX(value_new),0) new FROM (SELECT value_old, value_new FROM log where section_id = 2 and action_id = 6 AND business_unit_id = 2 AND associated_alt_table_id = @v1  AND associated_table_id = @v2  ORDER BY id DESC LIMIT 1) asdf", new object[] { business_unit_id, master_id, location_id }).Rows[0];
				// From inventory_branch
				dollar_balance_part_old = Convert.ToDouble(dr["old"]);
				// From inventory_branch
				dollar_balance_part_new = Convert.ToDouble(dr["new"]);
				// SUM from inventory_branch
				dollar_balance_total = Toolbox.doSQL_double(@"SELECT SUM(dollar_balance) FROM inventory_branch WHERE business_unit_id = @v0 ", new object[] { business_unit_id });
				var max_date = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(date_transferred), '') FROM inventory_annual_counts WHERE masterid = @v0  and business_unit_id = @v1 ", new object[] { master_id, business_unit_id });
				n_manual_adjustments = max_date == ""
					? Toolbox.doSQL_int(@"SELECT COUNT(*) FROM log WHERE action_id = 3 AND business_unit_id = 2 AND associated_table = 'inventory_location' AND is_manual = 1")
					: Toolbox.doSQL_int(@"SELECT COUNT(*) FROM log WHERE action_id = 3 AND business_unit_id = 2 AND associated_table = 'inventory_location' AND dt > '2013-01-03' AND is_manual = 1");
			}
		}
	}
	public class location_master
	{
		public bool loaded { get; set; }
		public int? id { get; set; }
		public int business_unit_id { get; set; }
		public int type_id { get; set; }
		public int children { get; set; }
		public string name { get; set; }
		public location_master() { }
		/// <summary>
		/// Loading from a already know row ID
		/// </summary>
		/// <param name="l_id"></param>
		public location_master(int l_id)
		{
			using (var conn = Toolbox.connect())
			{
				load(l_id, conn);
			}
		}
		public location_master(int l_id, MySqlConnection _conn)
		{
			load(l_id, _conn);
		}
		public void save()
		{
			using (var conn = Toolbox.connect())
			{
				save(conn);
			}
		}
		public void save(MySqlConnection _conn)
		{
			#region insert 
			if (id == null)
			{
				Toolbox.doSQL_void(_conn, @" INSERT INTO inventory_location_master ( business_unit_id, name, type_id ) VALUES ( @v0 , @v1 , @v2  )", new object[] { business_unit_id, name, type_id });
			}
			#endregion insert 
			#region update 
			else if (id != 0)
			{
				if (!loaded)
				{
					throw new Exception("Nothing is loaded to save");
				}
				var this_im = new location_master(Convert.ToInt32(id), _conn);
				if (this_im.name == "0.0.0" && name != "0.0.0")
				{
					throw new Exception("Cannot rename 0.0.0's name");
				}
				if (type_id == 2 && name == "0.0.0")
				{
					throw new Exception("Location 0.0.0 cannot be anything but an internal location");
				}

				Toolbox.doSQL_void(_conn, @" UPDATE inventory_location_master SET name = @v0 , type_id = @v1  WHERE id = @v2  LIMIT 1", new object[] { name, type_id, id });
			}
			#endregion update 
		}
		private void load(object l_id)
		{
			using (var conn = Toolbox.connect())
			{
				load(l_id, conn);
			}
		}
		private void load(object l_id, MySqlConnection _conn)
		{
			if (l_id != null && l_id.ToString() != "0")
			{
				var r = Toolbox.doSQL_dt(_conn, @"SELECT * FROM inventory_location_master  WHERE id =@v0 limit 1 ", new object[] { l_id }).Rows[0];
				id = Convert.ToInt32(r["id"]);
				business_unit_id = Convert.ToInt32(r["business_unit_id"]);
				type_id = Convert.ToInt32(r["type_id"]);
				name = r["name"].ToString();
				children = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM inventory_location  WHERE location_master_id =@v0", new object[] { l_id });
				loaded = true;
			}
			else
			{
				loaded = false;
			}
		}
		public void delete()
		{
			using (var conn = Toolbox.connect())
			{
				delete(conn);
			}
		}
		public void delete(MySqlConnection _conn)
		{
			if (children > 0)
			{
				throw new Exception("Cannot delete a master location with existing location(s) mapped to it. This one has (" + children + ") location(s) mapped to it");
			}
			if (id != 0 && id != null)
			{
				Toolbox.doSQL_void(_conn, @"DELETE FROM inventory_location_master WHERE id = '" + id + "' LIMIT 1", null);
			}
		}
	}
	public class branch_options
	{
		public int id { get; set; }
		public int business_unit_id { get; set; }
		public bool stk_adj { get; set; }
		public bool annual_count_on { get; set; }
		public DateTime stk_adj_enddt { get; set; }
		public DateTime annual_count_start_date { get; set; }
		public DateTime annual_count_end_date { get; set; }
		private bool is_loaded = false;
		public branch_options() { }
		public branch_options(int c_id)
		{
			load(c_id);
		}
		private void load(int _business_unit_id)
		{
			business_unit_id = _business_unit_id;
			if (exists(_business_unit_id))
			{
				var opt = Toolbox.doSQL_dt(@"SELECT * FROM inventory_branch_options WHERE business_unit_id = @v0 ", new object[] { _business_unit_id }).Rows[0];
				id = (int)opt["id"];
				stk_adj = (bool)opt["stk_adj"];
				stk_adj_enddt = opt["stk_adj_enddt"] == DBNull.Value ? new DateTime() : (DateTime)opt["stk_adj_enddt"];
				switch (opt["annual_count_start_date"] == DBNull.Value || opt["annual_count_end_date"] == DBNull.Value)
				{
					case true:
						annual_count_on = false;
						break;
					case false:
						if (Convert.ToDateTime(opt["annual_count_start_date"]).Date <= System.DateTime.Today && Convert.ToDateTime(opt["annual_count_end_date"]).Date >= System.DateTime.Today)
						{
							annual_count_on = true;
						}
						else
						{
							annual_count_on = false;
						}
						annual_count_start_date = Convert.ToDateTime(opt["annual_count_start_date"]);
						annual_count_end_date = Convert.ToDateTime(opt["annual_count_end_date"]);
						break;
				}

				//      annual_count_on = (Toolbox.doSQL_int(@"select ifnull((Select id from inventory_annual_count_header  where start_date<=now() and end_date >=now() and business_unit_id =@v0" 0)")>0, new object[] { _business_unit_id,) });
				is_loaded = true;
			}
		}
		public void start_adjustment_period(int _member_id)
		{
			if (is_loaded)
			{
				// log
				var l = new NELog
				{
					section_id = OpsLog.Section.BranchInventory,
					action_id = OpsLog.Action.ActivatedManualAdjustmentPeriod,
					business_unit_id = business_unit_id,
					is_manual = true,
					member_id = _member_id,
					table = OpsLog.Table.InventoryBranchOptions,
					table_id = id,
					value_old = 0,
					value_new = 1,
					alt_table_id = 0
				};
				l.save();

				stk_adj = true;
				stk_adj_enddt = DateTime.Now.AddHours(24);
				save();
				send_notice();
			}
		}
		public void stop_adjustment_period(int _member_id)
		{
			if (is_loaded)
			{
				// log
				var l = new NELog
				{
					section_id = OpsLog.Section.BranchInventory,
					action_id = OpsLog.Action.DeactivatedManualAdjustmentPeriod,
					business_unit_id = business_unit_id,
					is_manual = true,
					member_id = _member_id,
					table = OpsLog.Table.InventoryBranchOptions,
					table_id = id,
					value_old = 1,
					value_new = 0,
					alt_table_id = 0
				};
				l.save();

				stk_adj = false;
				stk_adj_enddt = DateTime.Now.AddHours(-24);
				save();
				send_notice();
			}
		}
		private void send_notice()
		{
			var co = new NeBusinessUnit(business_unit_id);
			var em = new NeEMail();
			var controller_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(member_id),0) FROM member  WHERE member_membertype_id = 67 AND member_status = 'Active'");
			if (controller_id > 0)
			{
				var controller = new NeMember(controller_id);
				em.To = controller.NEEmail;
				em.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
				em.CC = co.branch_manager != null ? co.branch_manager.NEEmail : "";
				//em.Bcc				= "mhyde@newelectric.com";
				em.isHTML = false;
				em.Subject = stk_adj
					? "Notice: Inventory adjustments have been Enabled for " + co.name
					: "Notice: Inventory adjustments have been Disabled for " + co.name;
				em.Body = stk_adj
					? "This manual adjustment period will automatically end: " + stk_adj_enddt
					: "";
				em.Send();
			}
		}
		private bool exists(int _business_unit_id)
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_branch_options WHERE business_unit_id = @v0 ", new object[] { _business_unit_id });
			if (c == 0)
			{
				// Make it.
				business_unit_id = _business_unit_id;
				stk_adj = false;
				try
				{
					save();
					return true;
				}
				catch
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}
		public void save()
		{
			var enddt_used = stk_adj_enddt < DateTime.Now ? null : (DateTime?)stk_adj_enddt;
			if (business_unit_id != 0)
			{
				if (id == 0)
				{

					// new 
					Toolbox.doSQL_void(@" INSERT INTO inventory_branch_options ( business_unit_id, stk_adj, stk_adj_enddt ) VALUES ( @v0 , @v1 , @v2  ) ", new object[] { business_unit_id, stk_adj, enddt_used });
				}
				else
				{
					// update
					Toolbox.doSQL_void(@" UPDATE inventory_branch_options SET stk_adj = @v1 , stk_adj_enddt = @v2  WHERE id = @v0  LIMIT 1 ", new object[] { id, stk_adj, enddt_used });
				}
			}
			else
			{
				// Put this here just to have a place to catch when the company id is set to 0.
			}
		}
	}
	public class shopping_cart
	{
		public int id { get; set; }
		public string name { get; set; }
		public int member_id { get; set; }
		public DateTime date_created { get; set; }
		public List<item> items { get; set; }
		public shopping_cart() { } // constructor
		public shopping_cart(int _id)
		{
			load(_id);
		}
		private void load(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var sh = uow.GetObjectByKey<ne_xpo.cs.shopping_cart_header>(_id);
				id = _id;
				name = sh.name;
				member_id = sh.member_id.member_id;
				date_created = sh.date_created;
				items = item.get_items(_id);
			}
		}
		public void save()
		{
			using (var uow = new UnitOfWork())
			{
				var sc = id > 0
					? uow.GetObjectByKey<ne_xpo.cs.shopping_cart_header>(id)
					: new ne_xpo.cs.shopping_cart_header(uow);
				sc.member_id = uow.GetObjectByKey<ne_xpo.cs.member>(member_id);
				sc.name = name;
				sc.date_created = id == 0 ? DateTime.Now : date_created;
				sc.Save();
				uow.CommitChanges();
				id = sc.id;
			}
		}
		public void delete()
		{
			delete(id);
		}
		public static void delete(int _id)
		{
			if (_id <= 0) return;
			if (has_children(_id)) throw new Exception("Can't delete a shopping cart that has items linked to it.");
			using (var uow = new UnitOfWork())
			{
				var sc = uow.GetObjectByKey<ne_xpo.cs.shopping_cart_header>(_id);
				sc.Delete();
				uow.CommitChanges();
			}
		}
		public static bool has_children(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				return (from i in new XPQuery<ne_xpo.cs.shopping_cart>(uow)
						where
						i.shopping_cart_header_id.id == _id
						select i).Any();
			}
		}
		public static void delete_children(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var links = (from i in new XPQuery<ne_xpo.cs.shopping_cart>(uow)
							 where
							 i.shopping_cart_header_id.id == _id
							 select i).ToList();
				foreach (var l in links)
				{
					l.Delete();
				}
				uow.CommitChanges();
			}
		}
		public static bool line_exists(int _header_id, int _master_id)
		{
			using (var uow = new UnitOfWork())
			{
				return (from i in new XPQuery<ne_xpo.cs.shopping_cart>(uow)
						where
						i.shopping_cart_header_id.id == _header_id &&
						i.master_id == _master_id
						select i).Any();
			}
		}
		public class item
		{
			public int id { get; set; }
			public int member_id { get; set; }
			public int master_id { get; set; }
			public double qty { get; set; }
			public int shopping_cart_header_id { get; set; }
			public item() { }
			public item(int _id)
			{
				load(_id);
			}
			public item(int _header_id, int _master_id)
			{
				using (var uow = new UnitOfWork())
				{
					var _id = (from i in new XPQuery<ne_xpo.cs.shopping_cart>(uow)
							   where
							   i.shopping_cart_header_id.id == _header_id &&
							   i.master_id == _master_id
							   select i.id).SingleOrDefault();
					load(_id);
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var si = uow.GetObjectByKey<ne_xpo.cs.shopping_cart>(_id);
					id = _id;
					if (si != null)
					{
						master_id = si.master_id;
						member_id = si.member_id.member_id;
						qty = si.qty;
						shopping_cart_header_id = si.shopping_cart_header_id.id;
					}
				}
			}
			public static List<item> get_items(int _header_id)
			{
				using (var uow = new UnitOfWork())
				{
					return (from i in new XPQuery<ne_xpo.cs.shopping_cart>(uow)
							where
							i.shopping_cart_header_id.id == _header_id
							select new item
							{
								id = i.id,
								member_id = i.member_id.member_id,
								master_id = i.master_id,
								qty = i.qty
							}).ToList();

				}
			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var si = id > 0
						? uow.GetObjectByKey<ne_xpo.cs.shopping_cart>(id)
						: new ne_xpo.cs.shopping_cart(uow);
					si.member_id = uow.GetObjectByKey<ne_xpo.cs.member>(member_id);
					si.master_id = master_id;
					si.shopping_cart_header_id = uow.GetObjectByKey<ne_xpo.cs.shopping_cart_header>(shopping_cart_header_id);
					si.dt = DateTime.Now;
					si.qty = qty;
					si.Save();
					uow.CommitChanges();
				}
			}
			public void delete()
			{
				delete(id);
			}
			public static void delete(int _id)
			{
				if (_id <= 0) return;
				using (var uow = new UnitOfWork())
				{
					var si = uow.GetObjectByKey<ne_xpo.cs.shopping_cart>(_id);
					si.Delete();
					uow.CommitChanges();
				}
			}
		}
	}
}