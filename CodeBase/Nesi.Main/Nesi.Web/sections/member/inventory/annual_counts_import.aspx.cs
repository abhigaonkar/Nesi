using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Linq;
using nesi.core;

public partial class sections_member_inventory_annual_counts_import : System.Web.UI.Page
	{
	private int business_unit_id				= 0;
	NeMember current_user;
    Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools					= new Toolbox();
		current_user					= Toolbox.do_handle_authentication(119);
		var _q			= Request.QueryString;
		if(string.IsNullOrEmpty(_q["business_unit_id"]))
			{
			Toolbox.FriendlyException(Response, "Branch not set", "window.close()");
			}
		else
			{
			business_unit_id					= Convert.ToInt32(_q["business_unit_id"]);
			}
		}
	private void run_check(bool do_process)
		{
		var merged_parts	= new List<merged_lineitem>();
		var dt_merged					= Toolbox.doSQL_dt(@"SELECT master_id, new_id FROM inventory_item_master  where new_id is not null" , null);
		foreach(DataRow dr in dt_merged.Rows)
			{
			var from_id		= (int) dr["master_id"];
			var new_id		= (int) dr["new_id"];
			merged_parts.Add(new merged_lineitem(){ master_id = from_id, new_id = new_id });
			}
		var master_locations			= build_locations();
		var master_costs				= build_costs();
		if(uploader.HasFile)
			{
			var fi			= new FileInfo(uploader.FileName);
			if(fi.Extension != ".csv")
				{
				lb_message.Text		= "Not a CSV file - "+uploader.FileName;
				}
			else
				{
				lb_message.Text		= "";
				uploader.SaveAs(@"C:\Windows\Temp\"+uploader.FileName);
				fi					= new FileInfo(@"C:\Windows\Temp\"+uploader.FileName);
				var reader = fi.OpenText();
				string[] lineIn;
				var all_lineitems	= new List<lineitem>();
				var good_lineitems	= new List<good_lineitem>();
				var bad_lineitems	= new List<bad_lineitem>();
				var line_n							= 0;
				while (!reader.EndOfStream)
					{
					line_n++;
					lineIn				= reader.ReadLine().Split(',');
					if(lineIn.Count() > 3)
						{
						throw new Exception("Bad format, needs to be master_id, location_id, quantity");
						}
					var bi			= new bad_lineitem();
					try
						{
						bi.line_n			= line_n;
						bi.master_id		= lineIn[0];
						bi.location_id		= lineIn[1];
						bi.qty				= lineIn[2];
						}
					catch (Exception ee)
						{
                        _tools.catch_error(ee);
						throw new Exception("Can't understand - "+reader.ReadLine());
						}
					try
						{
						// No errors... will be handled outside of this
						var item		= new lineitem();
						var try_master_id	= 0;
						var try_location_id	= 0;
						double try_qty		= 0;
						int.TryParse(lineIn[0].Trim(), out try_master_id);
						int.TryParse(lineIn[1].Trim(), out try_location_id);
						double.TryParse(lineIn[2].Trim(), out try_qty);
						item.master_id		= try_master_id;
						item.location_id	= try_location_id;
						item.qty			= try_qty;
						all_lineitems.Add(item);
						}
					catch {}
					var	master_id		= 0;
					int.TryParse(lineIn[0].Trim(), out master_id);
					if(master_id > 0)
						{
						var location_id	= 0;
						int.TryParse(lineIn[1], out location_id);
						double qty		= 0;
						double.TryParse(lineIn[2], out qty);
						if(location_id != 0)
							{
							if(master_locations.Contains(location_id))
								{
								var li	= new good_lineitem();
								li.master_id		= master_id;
								li.location_id		= location_id;
								li.qty				= qty;
								if(qty == 0 && lineIn[2].Trim() != "0")
									{
									if(bi.qty.ToString().Trim() == "")
										{
										bi.qty			 = 0;
										bi.reason		= "Quantity  is blank";
										}
									else
										{
										bi.reason				= "Quantity is invalid";
										}
									bad_lineitems.Add(bi);
									}
								else if(qty < 0)
									{
									bi.reason		= "Quantity is less than zero";
									bad_lineitems.Add(bi);
									}
								else
									{
									if(!master_costs.Contains(master_id) && qty != 0)
										{
										if(Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  master_id, business_unit_id } ) == 0)
											{
											bi.reason		= "Cost is zero";
											bad_lineitems.Add(bi);
											}
										else
											{
											var me			= (from m in merged_parts where m.master_id == master_id select new { master_id = m.master_id, new_id = m.new_id }).FirstOrDefault();
											if(me != null)
												{
												bi.reason		= "Merged to - "+me.new_id;
												bad_lineitems.Add(bi);
												}
											else
												{
												good_lineitems.Add(li);
												}
											}
										}
									else
										{
										good_lineitems.Add(li);
										}
									}
								}
							else
								{
								bi.reason		= "Location does not belong to this branch";
								bad_lineitems.Add(bi);
								}
							}
						else
							{
							if(bi.location_id.ToString().Trim() == "")
								{
								bi.location_id = 0;
								bi.reason		= "Location ID is blank";
								}
							else
								{
								bi.reason				= "Invalid Location ID";
								}
							bad_lineitems.Add(bi);
							}
						}
					else
						{
						if(bi.master_id.ToString().Trim() == "")
							{
							bi.master_id = 0;
							bi.reason		= "Master ID is blank";
							}
						else
							{
							bi.reason				= "Invalid Master ID";
							}
						bad_lineitems.Add(bi);
						}
					}
				reader.Close();

				if(bad_lineitems.Count == 0)
					{
					// Duplicate check

					foreach(var li in all_lineitems)
						{
						var bi	= new bad_lineitem() { location_id = li.location_id, master_id = li.master_id, qty = li.qty};

						var c = (from y in all_lineitems 
									where y.master_id == li.master_id &&
											y.location_id == li.location_id
									select y).Count();
						if(c > 1)
							{
							bi.reason		= "Duplicate master_id/location_id combo in file";
							bad_lineitems.Add(bi);
							}
						}
					}
				if(bad_lineitems.Count > 0)
					{
					bad_lineitems.Sort((i1, i2) => int.Parse(i1.master_id.ToString()).CompareTo(int.Parse(i2.master_id.ToString())));
					lb_message.Text		= @"
	<div style='color:#f00;font-weight:bold;'>YOU WILL NEED TO REATTACH THE UPLOAD FILE TO PROCESS AGAIN</div>
					<div style='font-size:13px;font-weight:bold;font-family:arial;'>These rows could not be fully processed and need to be addressed before this can fully process.</div>";
					var sb	= new StringBuilder();
					sb.Append(@"
	<table cellspacing='0' cellpadding='2' style='width:100%;font-size:11px;'>
		<thead>
			<tr>
				<th>Line #</th>
				<th>Master ID</th>
				<th>Location ID</th>
				<th>QTY</th>
				<th>Reason for Failure</th>
			</tr>
		</thead>
		<tbody>
");
					for(var i = 0; i < bad_lineitems.Count; i++)
						{
						var bi		= bad_lineitems[i];
						sb.AppendFormat(@"
		<tr>
			<td align='center'>{0}</td>
			<td align='center'>{1}</td>
			<td align='center'>{2}</td>
			<td align='center'>{3}</td>
			<td align='center' style='color:#f00;'>{4}</td>
		</tr>
", bi.line_n, bi.master_id, bi.location_id, bi.qty, bi.reason);
							}
					sb.Append(@"
		</tbody>
	</table>");
					lb_message.Text		+= sb.ToString();
					}
				else if(do_process)
					{
					for(var i = 0; i < good_lineitems.Count; i++)
						{
						var li		= good_lineitems[i];
						Toolbox.doSQL_void(@"CALL inv_adjust_location(@v0,@v1,@v2,@v3,@v4)", new object[] { li.master_id, li.location_id,li.qty, business_unit_id, current_user.id});
						add_history_line(business_unit_id, li.master_id, li.qty, li.location_id, current_user.id32, "CSV");
						Response.Write(string.Format(@"(Master ID:{0}, Location:{1},Qty:{2},CompanyID:{3},User:{4}) - Finished<br/>", li.master_id, li.location_id,li.qty, business_unit_id, current_user.id));
						Response.Flush();
						}
					lb_message.Text		= "<div style='font-size:13px;font-weight:bold;font-family:arial;color:#090;'>All lines processed.</div>";
					}
				}
			}
		else
			{
			lb_message.Text		= "No file supplied";
			}
		}
	protected void bt_check_Click(object sender, EventArgs e)
		{
		run_check(false);
		}
	protected void bt_upload_Click(object sender, EventArgs e)
		{
		run_check(true);
		}
	private void add_history_line(int business_unit_id, int master_id, double qty, int location_master_id, int member_id, string origin)
		{
		Toolbox.doSQL_void(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
	qty,
	locationid,
	memberid,
	date_transferred,
	origin
	)
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	NOW(),
	@v5
	)
", new object[] {
	business_unit_id,			  // {0}
	master_id,			  // {1}
	qty,					  // {2}
	location_master_id,   // {3}
	member_id,			  // {4}
	origin				  // {5}
 });
		}

	private List<int> build_locations()
		{
		if(business_unit_id != 0)
			{
			var _dt			= Toolbox.doSQL_dt(@"SELECT id FROM inventory_location_master WHERE business_unit_id = @v0 ", new object[] {  business_unit_id } );
			var temp_list		= new List<int>();
			foreach(DataRow dr in _dt.Rows)
				{
				temp_list.Add((int)dr["id"]);
				}
			return temp_list;
			}
		else
			{
			throw new Exception("Branch not set");
			}
		}
	private List<int> build_costs()
		{
		if(business_unit_id != 0)
			{
			var _dt			= Toolbox.doSQL_dt(@"SELECT master_id id FROM inventory_cost WHERE business_unit_id = @v0  AND cost > 0", new object[] {  business_unit_id } );
			var temp_list		= new List<int>();
			foreach(DataRow dr in _dt.Rows)
				{
				temp_list.Add((int)dr["id"]);
				}
			return temp_list;
			}
		else
			{
			throw new Exception("Branch not set");
			}
		}
	private class merged_lineitem
		{
		public int master_id  { get; set; }
		public int new_id  { get; set; }
		}
	private class good_lineitem
		{
		public int master_id  { get; set; }
		public int location_id  { get; set; }
		public double qty  { get; set; }
		}
	private class bad_lineitem
		{
		public int line_n {get;set;}
		public object master_id  { get; set; }
		public object location_id  { get; set; }
		public object qty  { get; set; }
		public string reason { get; set; }
		}
	private class lineitem
		{
		public int master_id  { get; set; }
		public int location_id  { get; set; }
		public double qty  { get; set; }
		}
	}