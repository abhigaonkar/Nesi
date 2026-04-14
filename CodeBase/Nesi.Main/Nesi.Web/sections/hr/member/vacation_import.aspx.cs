using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_hr_member_vacation_import : System.Web.UI.Page
	{
	NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(216);

		}
	protected void Page_Load(object sender, EventArgs e)
		{

		}
	protected void bt_upload_Click(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			if (uc.HasFile)
				{
				var file = uc.PostedFile;
				var line_i = 0;
				var sb_results = new StringBuilder();
				var sb_errors = new StringBuilder();
				var good_lines = new List<Tuple<int, double>>();
				var processedIds = new List<int>();

				using (var sr = new StreamReader(file.InputStream))
					{
					while (!sr.EndOfStream)
						{
						var read_line = sr.ReadLine();
						line_i++;
						if(read_line == null) continue;
						if(read_line == ",") continue;
						var is_comma_delimited = read_line.Contains(",");
						if (is_comma_delimited)
							{
							var vals = read_line.Split(',');
							if (vals.Length != 2)
								{
								sb_errors.AppendFormat("Line {0} isn't formatted correctly... too many values.<br />", line_i);
								}
							else
								{
								var payrollId		= vals[0].Trim();
								var memberCount = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM member WHERE member_payroll_id = @v0", new object[] { payrollId });
								if (memberCount > 1)
									{
									sb_errors.AppendFormat("Line {0} has an invalid payroll id <b>{1}</b>, multiple detected - line text:{2}<br />", line_i, payrollId, read_line);
									}
								else
									{
									var memberId = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(member_id), 0) FROM member WHERE member_payroll_id = @v0", new object[] { payrollId });
									if(processedIds.Contains(memberId))
										{
										sb_errors.AppendFormat(@"Line {0} has a payroll id {1} that is already processed on a previous line.<br />", line_i, payrollId);
										}
									else
										{
										processedIds.Add(memberId);
										var amount = vals[1];
										if(amount.Contains("("))
											{
											amount = "-"+amount.Replace("(", "").Replace(")", "").Trim();
											}
										double qty;
										double.TryParse(amount, out qty);
										if (memberId == 0)
											{
											sb_errors.AppendFormat("Line {0} has a payroll id ({1}) that isn't assigned to an employee - line text: {2}<br />", line_i, payrollId, read_line);
											}
										else if (qty == 0 && !Toolbox.Contains(amount, new[] { "0", "0.0", "0.00" }))
											{
											sb_errors.AppendFormat("Line {0} ({1}) has an invalid quantity/dollar value, if it's supposed to be 0, please either format it as 0, 0.0 or 0.00<br />", line_i, read_line);
											}
										else
											{
											good_lines.Add(new Tuple<int, double>(memberId, qty));
											}
										}
									}
								}
							}
						else
							{
							sb_errors.AppendFormat("Line {0} isn't formatted correctly... Not comma delimited.<br />", line_i);
							}
						}
					}
				if (sb_errors.Length > 0)
					{
					results.InnerHtml = sb_errors.ToString();
					}
				else
					{
					var ordered_lines = good_lines.OrderBy(_x => _x.Item1);
					foreach (var line in ordered_lines)
						{
						var user = new NeMember(line.Item1);
						var prev_vac = user.Holiday;
						payroll.vacation.add_adjustment(conn, ref user, current_user, "Mass update", line.Item2, true);
						sb_results.AppendFormat("Updated: <b>{0}</b> ({1}) from <b>{2}</b> to <b>{3}</b><br />", user.FullName, line.Item1, prev_vac, user.Holiday);
						}
					results.InnerHtml = sb_results.ToString();
					}
				}
			}
		}
	protected void bt_template_Click(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			if (ddl_branches.SelectedValue == "") return;
			var branches = string.Join(",", (from ListItem i in ddl_branches.Items where i.Selected select i.Value).ToList());
			var users = Toolbox.doSQL_dt(conn, string.Format(@"SELECT member_payroll_id, member_holiday FROM member WHERE business_unit_id in ({0}) and member_status = 'Active' ORDER BY member_id", branches), null);
			var sb = new StringBuilder();
			foreach (DataRow user in users.Rows)
				{
				sb.AppendFormat("{0},{1}\r\n", user["member_payroll_id"], user["member_holiday"]);
				}

			Response.AddHeader("Content-Disposition", "attachment; filename=holiday_template.csv");
			Response.ContentType = NeFiles.GetMimeType(".csv");
			Response.Write(sb.ToString());
			Response.End();
			}
		}
	}