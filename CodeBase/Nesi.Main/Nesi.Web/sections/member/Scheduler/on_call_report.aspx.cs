using System;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Drawing;
using nesi.core;
using nesi.core.print;
using NESI.Common.Models;

public partial class sections_member_scheduler_on_call_report : System.Web.UI.Page
{
	private int lastInsertedAppointmentId;

	public NeMember mymember;
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	private bool can_edit = false;
	public DataTable dt = new DataTable();
	protected void Page_Init()
	{
		mymember = Toolbox.do_handle_authentication(OpsPage.OnCallSchedule);  // allowed to see on call schedule
		can_edit = mymember.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments);
		if (!IsPostBack)
		{

		}
	}
	protected void Page_Load()
	{

		if (!IsPostBack)
		{
			ASPxDateEdit1.Date = DateTime.Today;
			ASPxDateEdit2.Date = DateTime.Today.AddDays(14);
			cl_companies.DataBind();
			if (cl_companies.Items.FindByValue(mymember.business_unit_id.ToString()) != null)
			{
				cl_companies.Items.FindByValue(mymember.business_unit_id.ToString()).Selected = true;
			}
		}

		fill_dt();

		var oc = new oncall();
		oc.dt = dt;
		oc.CreateXRTable_header();
		oc.CreateXRTable();
		InvoicePreviewer.Report = oc;
		InvoicePreviewer.DataBind();

		txtSubject.Text = Toolbox.app_setting("Domain") + " On Call Schedule";

	}

	protected void fill_dt()
	{
		if (cl_companies.SelectedValues.Count > 0)
		{
			dt.Columns.Clear();
			dt.Columns.Add("Date", typeof(string));
			dt.Columns.Add("Role", typeof(string));


			var xx = "";
			foreach (string c_selected in cl_companies.SelectedValues)
			{
				dt.Columns.Add(cl_companies.Items.FindByValue(c_selected).Text);
				xx += c_selected + ",";
			}
			xx = xx.Remove(xx.Length - 1, 1);

			// Here we add five DataRows.

			//	table.Rows.Add(25, "Indocin", "David", DateTime.Now);
			var t = ASPxDateEdit1.Date;
			var branches = Toolbox.doSQL_dt(string.Format(@"Select x.id, x.name Branch, member.member_fullname bm_name, 
cellphone_number.number bm_number from (SELECT id, name, get_bm(id) bm_id FROM business_unit  WHERE istest = 'F' 
AND id IN ({0}) ORDER BY country, id) x INNER JOIN member on x.bm_id = member_id left JOIN cellphone_number 
ON member.cellphone_number_id = cellphone_number.id AND cellphone_number.`status` = 'Active' ", xx), null);
			while (t < ASPxDateEdit2.Date)
			{
				var new_row = dt.NewRow();
				new_row.BeginEdit();
				new_row["Date"] = t.ToString("yyyy-MM-dd");
				new_row["Role"] = "OnCall";
				foreach (DataRow dr_branch in branches.Rows)
				{

					var oncall_member = Toolbox.doSQL_string(@"select ifnull((SELECT member.member_fullname FROM oncall_schedule INNER JOIN member ON oncall_schedule.member_id = member.Member_ID  WHERE oncall_schedule.date =@v0 and oncall_schedule.business_unit_id=@v1  and is_backup=0 limit 1), '@v2') ", new object[] { t.ToString("yyyy-MM-dd"), dr_branch["id"], dr_branch["bm_name"] });

					var oncall_number = Toolbox.doSQL_string(@"select ifnull((SELECT cellphone_number.number FROM oncall_schedule INNER JOIN member ON oncall_schedule.member_id = member.Member_ID left JOIN cellphone_number ON member.cellphone_number_id = cellphone_number.id AND cellphone_number.`status` = 'Active'  WHERE oncall_schedule.date =@v0 and oncall_schedule.business_unit_id=@v1  and is_backup=0 limit 1), '') ", new object[] { t.ToString("yyyy-MM-dd"), dr_branch["id"], dr_branch["bm_number"] });

					oncall_member = oncall_member + " " + oncall_number;
					new_row[dr_branch["Branch"].ToString()] = oncall_member;

				}
				dt.Rows.Add(new_row);
				new_row = dt.NewRow();
				new_row.BeginEdit();
				new_row["Date"] = t.ToString("yyyy-MM-dd");
				new_row["Role"] = "Backup";
				foreach (DataRow dr_branch in branches.Rows)
				{
					var backup_member = Toolbox.doSQL_string(@"select ifnull((SELECT member.member_fullname FROM oncall_schedule INNER JOIN member ON oncall_schedule.member_id = member.Member_ID  WHERE oncall_schedule.date =@v0 and oncall_schedule.business_unit_id=@v1  and is_backup=1 limit 1),'Not Set')", new object[] { t.ToString("yyyy-MM-dd"), dr_branch["id"] });

					var backup_number = Toolbox.doSQL_string(@"select ifnull((SELECT cellphone_number.number FROM oncall_schedule INNER JOIN member ON oncall_schedule.member_id = member.Member_ID left JOIN cellphone_number ON member.cellphone_number_id = cellphone_number.id AND cellphone_number.`status` = 'Active'  WHERE oncall_schedule.date =@v0 and oncall_schedule.business_unit_id=@v1  and is_backup=1 limit 1),'')", new object[] { t.ToString("yyyy-MM-dd"), dr_branch["id"] });

					backup_member = backup_member + " " + backup_number;
					new_row[dr_branch["branch"].ToString()] = backup_member;

				}
				dt.Rows.Add(new_row);

				t = t.AddDays(1);
			}
		}



	}


	protected void btnSendEmail_Click(object sender, EventArgs e)
	{
		if (txtEmailAddress.Text == "")
		{
			return;
		}
		var file = new MemoryStream();
		InvoicePreviewer.Report.CreateDocument(false);
		InvoicePreviewer.Report.ExportToPdf(file);
		file.Seek(0, SeekOrigin.Begin);

		var mail = new NeEMail();
		mail.To = txtEmailAddress.Text;
		//		mail.To = "aketelaars@newelectric.com";
		mail.From = "admin@" + Toolbox.app_setting("DomainForEmail");
		mail.Subject = txtSubject.Text;
		mail.Body = memoBody.Text;



		mail.Attachment = new Attachment(file, "OnCallSchedule.pdf");


		mail.Send();

		// end void

	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{

	}
}




