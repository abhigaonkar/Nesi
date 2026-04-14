using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet.PEL;

namespace NESI.BLL.Pages.Timesheet.PEL
{
	public class PELSchedule : Vacation.VacationBLL
	{
		public PELSchedule(Employee currentuser) : base(currentuser)
		{

		}



		public PELSummary GetSummary(out List<Data.Entities.vacation> historyList)
		{
			var p = new ProfileBase(CurrentUser);
			var total_days = Convert.ToDouble(p.GetValueByPropertyName("total_pel_days"));
			var total_hours = total_days * 8;
			var total_paid_days = Convert.ToDouble(p.GetValueByPropertyName("total_pel_paid_days"));
			var total_paid_hours = total_paid_days * 8;
			var total_unpaid_days = total_days - total_paid_days;
			var total_unpaid_hours = total_unpaid_days * 8;

			historyList = _db.vacation.Where(x => x.member_id == UserId && x.type_id == 9 && x.status_id != 4).ToList();
			var list = historyList.Where(x => !x.date_start.StartsWith("0001-01-01") && Convert.ToDateTime(x.date_start) <= new DateTime(DateTime.Now.Year, 12, 31));
			var used_hours = Convert.ToDouble(list.Sum(x => x.hours_requested));
			var used_days = used_hours / 8;

			var used_paid_hours = used_hours > total_paid_hours ? 16 : used_hours;
			var used_paid_days = used_paid_hours / 8;

			var used_unpaid_hours = used_hours - used_paid_hours;
			var used_unpaid_days = used_unpaid_hours / 8;

			return new PELSummary()
			{
				total_days = total_days,
				total_hours = total_hours,
				total_paid_days = total_paid_days,
				total_paid_hours = total_paid_hours,
				total_unpaid_days = total_unpaid_days,
				total_unpaid_hours = total_unpaid_hours,
				used_hours = used_hours,
				used_days = used_days,
				used_paid_days = used_paid_days,
				used_paid_hours = used_paid_hours,
				used_unpaid_days = used_unpaid_days,
				used_unpaid_hours = used_unpaid_days
			};
		}

		public PELProfile Profile()
		{
			var summary = GetSummary(out var historyList);
			return new PELProfile()
			{
				historyList = historyList,
				summary = summary,
			};
		}

		public DataExtra Review(AddPEL model)
		{
			var summary = GetSummary(out var historyList);
			var type_of_payment = "";

			if (summary.used_hours + model.Hours > summary.total_hours)
			{
				return new DataExtra($@"You only have {summary.total_hours - summary.used_hours} hours PEL days avaliable.");
			}

			if (summary.used_hours > summary.total_hours)
			{
				type_of_payment = $@"{model.Hours} hours UNPAID";
			}
			else
			{
				if (summary.used_paid_hours + model.Hours <= summary.total_paid_hours)
				{
					type_of_payment = $@"{model.Hours} hours PAID";
				}
				else
				{
					var paid = summary.total_paid_hours - summary.used_paid_hours;
					var unpaid = model.Hours - paid;
					type_of_payment = $@"{paid} hours PAID, {unpaid} hours UNPAID";

				}
			}


			return new DataExtra("success", new
			{
				date_start = model.Date_start,
				date_end = model.Date_end,
				hours = model.Hours,
				note = model.Note,
				type_of_payment
			});
		}

		public object Save(AddPEL model)
		{

			var summary = GetSummary(out var historyList);
			var paid = 0d;
			var unpaid = 0d;
			model.Date_start = new DateTime(model.Date_start.Year, model.Date_start.Month, model.Date_start.Day, 8, 0, 0);
			if (summary.used_hours + model.Hours > summary.total_hours)
			{
				return new DataExtra($@"You only have {summary.total_hours - summary.used_hours} hours PEL days avaliable.");
			}

			if (summary.used_hours > summary.total_hours)
			{
				paid = model.Hours;
				unpaid = 0;
			}
			else
			{
				if (summary.used_paid_hours + model.Hours <= summary.total_paid_hours)
				{
					paid = model.Hours;
				}
				else
				{
					paid = summary.total_paid_hours - summary.used_paid_hours;
					unpaid = model.Hours - paid;
				}
			}
			var o = new Data.Entities.vacation_master
			{
				date_start = model.Date_start,
				date_end = model.Date_start.AddHours(paid),
				hours_requested = (int)paid,
				type_id = 9,
				status = 3,
				payment_amount = paid,
				business_unit_id = CurrentUser.BusinessUnitId,
				comments = model.Note,
				payment_method = 4,
				member_id = UserId,
				date_return = new DateTime(0001, 01, 01),
				date_insert = DateTime.Now,
				create_member_id = UserId,
				payperiod_id = NePayPeriod.get_payperiod_id(model.Date_start)
			};

			var o2 = new Data.Entities.vacation_master();
			o2 = (vacation_master)MapperFrom(o2, o);

			if (paid > 0)
			{
				o.payment_method = 4;
				_db.vacation_master.Add(o);
			}



			o2.payment_method = 1;
			o2.hours_requested = (int)unpaid;
			o2.payment_amount = unpaid;
			o2.date_start = o.date_end;
			o2.date_end = model.Date_start.AddHours(paid + unpaid);
			if (unpaid > 0)
			{
				_db.vacation_master.Add(o2);
			}
			_db.SaveChanges();
			var body = new StringBuilder();
			body.AppendFormat(
				$@"<div style='font-family:arial; font-size:12px;'>{CurrentUser.FullName} has requested Personal Emergency Leave (PEL) from {model.Date_start} to {
						o2.date_end
					}.<br/><br/>Below are a list of hours requested of the PEL off.<br/>");
			body.AppendFormat($"<div><b>PAID hours:</b> {paid}</div>");
			body.AppendFormat($"<div><b>UNPAID hours:</b> {unpaid}</div>");
			body.AppendFormat($"<br/><div><b>Below are the PEL histories of {CurrentUser.FullName}</b>");
			var p = Profile();
			body.AppendFormat(GeneratePELHTMLTable(p.historyList));
			SendMail(body.ToString());
			return new DataExtra("PEL request has been submitted and approved successfully!", p);
		}


		public string GeneratePELHTMLTable(List<vacation> list)
		{

			var html = "<table border='1'>";
			//add header row
			html += "<tr>";
			html += "<td>Status</td>";
			html += "<td>Request Date</td>";
			html += "<td>Start Date</td>";
			html += "<td>End Date</td>";
			html += "<td>Pay Type</td>";
			html += "<td>Hours Request</td>";
			html += "</tr>";
			//add rows
			foreach (var row in list)
			{
				html += "<tr>";
				html += $"<td>{row.status}</td>";
				html += $"<td>{row.date_insert}</td>";
				html += $"<td>{row.date_start}</td>";
				html += $"<td>{row.date_end}</td>";
				html += $"<td>{row.payment_method}</td>";
				html += $"<td>{row.hours_requested}</td>";
				html += "</tr>";
			}
			html += "</table>";
			return html;
		}


		public DataExtra Cancel(int vacationid)
		{
			var v = _db.vacation.FirstOrDefault(x => x.vacation_id == vacationid && x.member_id == UserId && x.type_id == 9);
			if (v != null)
			{
				bllToolbox.doSQL_void(@"UPDATE vacation_master SET status = 4 WHERE vacation_id = @v0", vacationid);
				var body = new StringBuilder();
				body.AppendFormat(
					$@"<div style='font-family:arial; font-size:12px;'>{CurrentUser.FullName} has CANCELED Personal Emergency Leave (PEL) from {v.date_start} to {v.date_end}.
				<br/>");
				body.AppendFormat($"<br/><div><b>Below are the PEL histories of {CurrentUser.FullName}</b>");
				var p = Profile();
				body.AppendFormat(GeneratePELHTMLTable(p.historyList));
				SendMail(body.ToString());
				return new DataExtra("PEL request has been canceled successfully.", p);
			}
			else
			{
				return new DataExtra("Cancel PEL request failed.", Profile());
			}

		}

		public void SendMail(string body)
		{
			var m = new nesi.core.NeEMail
			{
				From = "payroll@" + Toolbox.app_setting("DomainForEmail"),
				To = CurrentUser.ReportToManager.member_neemail,
				Subject = "PEL Request",
				isHTML = true,
				to_member_id = CurrentUser.ReportToManager.Member_ID,
				Body = body
			};
			m.Send();
		}
	}



}
