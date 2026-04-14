using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_member_inventory_user_controls_vendor_data_import : UserControl
	{
	private int business_unit_id = 0;
	NeBusinessUnit company;
	public NeMember current_user { get; set; }
	public NeBusinessUnit WorkingBusinessUnit  { get; set; }
	public NeBusinessUnit WarehouseBusinessUnit  { get; set; }

	protected void Page_Init(object sender, EventArgs e)
		{
		if(Session["working_business_unit_id"] != null)
			{
			int.TryParse(Session["working_business_unit_id"].ToString(), out business_unit_id);
			company = new NeBusinessUnit(business_unit_id);
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(Visible)
			{
			Toolbox.inject_css("/_tools/handlers/css.ashx?p=1VDI", Parent.Page);
			}
		if(!IsPostBack)
			{
			Session["dt_vendor_import_results"]		= null;
			}
		else
			{
			bind_results();
			}
		if(business_unit_id == 0 && Session["working_business_unit_id"] != null)
			{
			int.TryParse(Session["working_business_unit_id"].ToString(), out business_unit_id);
			company = new NeBusinessUnit(business_unit_id);
			}
		}

	protected void up_file_OnFileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{
		using (var r = new StreamReader(e.UploadedFile.FileNameInStorage))
			{
			string line;
			var line_id	= 1;
			var errors				= new List<string>();
			var vendors				= Toolbox.doSQL_dt(@"SELECT vendor_id, vendor_number, vendor_name FROM vendor"  , null);
			var country				= company.country == "USA" ? "usa" : "cdn";
			var inventory			= Toolbox.doSQL_dt(string.Format(@"SELECT a.master_id, active, b.desc_full_{0} 'description' FROM inventory_item_master a LEFT JOIN inventory_description b ON a.master_id = b.master_id", country),null);
			var existing_pricing	= Toolbox.doSQL_dt(@"SELECT master_id, vendor_id, vendor_code, cost FROM inventory_price WHERE business_unit_id = @v0 ", new object[] {  business_unit_id } );
			var results			= new DataTable("results");
			results.Columns.Add("id", typeof(int));
			results.Columns.Add("master_id", typeof(int));
			results.Columns.Add("vendor_id", typeof(int));
			results.Columns.Add("exists", typeof(bool));
			results.Columns.Add("description", typeof(string));
			results.Columns.Add("vendor_name", typeof(string));
			results.Columns.Add("vendor_code", typeof(string));
			results.Columns.Add("total_cost", typeof(double));
			results.Columns.Add("qty_per", typeof(double));
			results.Columns.Add("per_unit_cost", typeof(double));

			while((line = r.ReadLine()) != null)
				{
				var line_error		= false;
				if(line.Contains(","))
					{
					var values		= line.Split(',');
					if(values.Length == 5)
						{
						int master_id, vendor_id;
						double total_cost, qty_per;
						var code					= values[2].Trim();
						int.TryParse(values[0], out master_id);
						int.TryParse(values[1], out vendor_id);
						double.TryParse(values[3], out total_cost);
						double.TryParse(values[4], out qty_per);
						var master_id_exists		= inventory.Select("master_id = "+master_id).Length == 1;
						var vendor_id_exists		= vendors.Select("vendor_id = "+vendor_id).Length == 1;
						var description				= master_id_exists ? inventory.Select("master_id = "+master_id)[0]["description"].ToString() : "";
						var vendor_number			= vendor_id_exists ? vendors.Select("vendor_id = "+vendor_id)[0]["vendor_number"].ToString() : "";
						var vendor_name				= vendor_id_exists ? vendors.Select("vendor_id = "+vendor_id)[0]["vendor_name"].ToString() : "";
						var code_exists_elsewhere	= vendor_id_exists && Toolbox.doSQL_int(@"SELECT COUNT(a.id)
FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_code = @v0 
AND a.vendor_id = @v1 AND a.master_id != @v2 AND b.active = true", new object[] { code, vendor_id, master_id}) > 0;
						var vendor_line_exists		= existing_pricing.Select(string.Format("vendor_code = '{0}' AND master_id = '{1}' AND vendor_id = '{2}'", code, master_id, vendor_id)).Length == 1;
						if(vendor_name != "")
							{
							vendor_name				= string.Format("({0}) {1}", vendor_number, vendor_name);
							}
						var active_part				= master_id_exists && inventory.Select("master_id = "+master_id)[0]["active"].ToString() == "1";
						if(master_id == 0)
							{
							add_error(ref errors, "Master ID is invalid", line_id);
							line_error				= true;
							}
						if(!master_id_exists)
							{
							add_error(ref errors, "Master ID doesn't exist", line_id);
							line_error				= true;
							}
						if(master_id_exists && !active_part)
							{
							add_error(ref errors, "Master ID isn't active (possibly merged?)", line_id);
							line_error				= true;
							}
						if(vendor_id == 0)
							{
							add_error(ref errors, "Vendor ID is invalid", line_id);
							line_error				= true;
							}
						if(!vendor_id_exists)
							{
							add_error(ref errors, "Vendor ID doesn't exist", line_id);
							line_error				= true;
							}
						if(code_exists_elsewhere)
							{
							add_error(ref errors, "Vendor code exists elsewhere on different part", line_id);
							line_error				= true;
							}
						if(total_cost <= 0)
							{
							add_error(ref errors, "Total is invalid", line_id);
							line_error				= true;
							}
						if(qty_per <= 0)
							{
							add_error(ref errors, "Qty Per is invalid", line_id);
							line_error				= true;
							}
						if(total_cost != 0 && qty_per != 0)
							{
							var cost_per = total_cost / qty_per;
							if(cost_per < 0.001)
								{
								add_error(ref errors, "The cost per is less than $0.001", line_id);
								line_error				= true;
								}
							}
						if(!line_error)
							{
							var dr				= results.NewRow();
							dr["id"]			= line_id;
							dr["exists"]		= vendor_line_exists;
							dr["master_id"]		= master_id;
							dr["vendor_id"]		= vendor_id;
							dr["vendor_name"]	= vendor_name;
							dr["vendor_code"]	= code;
							dr["description"]	= description;
							dr["total_cost"]	= total_cost;
							dr["qty_per"]		= qty_per;
							dr["per_unit_cost"]	= total_cost / qty_per;
							results.Rows.Add(dr);
							}
						}
					else
						{
						add_error(ref errors, "The file is not set up correctly.", 0);
						break;
						}
					}
				else
					{
					add_error(ref errors, "Not a valid CSV file.", 0);
					break;
					}
				line_id++;
				}
			if(errors.Any())
				{
				e.CallbackData			= "FAILED|"+string.Join(",", errors);
				}
			else
				{
				e.CallbackData							= "SUCCESS|"+e.UploadedFile.FileNameInStorage;
				Session["dt_vendor_import_results"]		= results;
				}
			}
		}
	private void add_error(ref List<string> list, string error_string, int line_id)
		{
		list.Add(line_id > 0
			? string.Format("Line {1}: {0}", error_string, line_id)
			: string.Format("Critical error: {0}", error_string));
		}
	private void bind_results()
		{
		var dt						= (DataTable) Session["dt_vendor_import_results"];
		if(dt != null && dt.Rows.Count > 0)
			{
			gv_results.ClientVisible	= true;
			gv_results.DataSource		= dt;
			gv_results.DataBind();
			}
		}
	protected void up_cbp_OnCallback(object sender, CallbackEventArgsBase e)
		{
		var vals			= e.Parameter.Split('|');
		var result			= vals[0];
		switch(result)
			{
			case "FAILED":
				lb_error.Text				= @"<div style='font-size:20px;color:#f00;font-decoration:underline;'>There were errors, please review and try again:</div><ul><li>"+vals[1].Replace(",", "<li>")+"</ul>";
				lb_error.ClientVisible		= true;
				gv_results.ClientVisible	= false;
				bt_import.ClientVisible		= false;
			break;
			case "SUCCESS":
				lb_error.Text				= "";
				lb_error.ClientVisible		= false;
				bt_import.ClientVisible		= true;
				bind_results();
			break;
			case "PROCESS":
				var dt						= (DataTable) Session["dt_vendor_import_results"];
				if(dt != null && dt.Rows.Count > 0)
					{
					foreach(DataRow dr in dt.Rows)
						{
						var master_id			= Convert.ToInt32(dr["master_id"]);
						var vendor_id			= Convert.ToInt32(dr["vendor_id"]);
						var cost				= Convert.ToDouble(dr["per_unit_cost"]);
						var total_cost			= Convert.ToDouble(dr["total_cost"]);
						var qty_per				= Convert.ToDouble(dr["qty_per"]);
						var vendor_code			= dr["vendor_code"].ToString();
						var vpr					= new vendor_price_row(master_id, business_unit_id, vendor_id);
						vpr.master_id			= master_id;
						vpr.vendor_id			= vendor_id;
						vpr.business_unit_id			= business_unit_id;
						vpr.vendor_code			= vendor_code;
						vpr.cost				= cost;
						vpr.total				= total_cost;
						vpr.qty					= qty_per;
						vpr.origin				= "Mass vendor import";
						vpr.member_id			= current_user.id;
						vpr.save(true);
						}
					lb_error.Text						= string.Format(@"<div style='font-size:20px;color:#090;'>Successfully imported {0} row(s)</div>", dt.Rows.Count);
					lb_error.ClientVisible				= true;
					gv_results.ClientVisible			= false;
					bt_import.ClientVisible				= false;
					Session["dt_vendor_import_results"]	= null;
					}
			break;
			}
		}

	protected void gv_results_OnDataBound(object sender, EventArgs e)
		{
		((ASPxGridView)sender).DetailRows.ExpandAllRows();
		((ASPxGridView)sender).SettingsDetail.ShowDetailButtons = false;
		}
	}