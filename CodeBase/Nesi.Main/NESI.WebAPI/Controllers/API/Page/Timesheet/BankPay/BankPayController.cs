using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Pages.Timesheet.Bank;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.BankPay
{
	[RoutePrefix("api/Page/Timesheet/Bankpay")]
	public class PageBankPayController : TimeSheetControllerBase
	{
		[Route("Summary")]
		public IHttpActionResult GetSummary()
		{
			var bank = new Banked_pay(CurrentUser);
			return Ok(new
			{
				balance_money = bank.balance().ToString("N2"),
				balance_hours = bank.withdrawable_hours().ToString("N2"),
				ledger = bank.GetLedger(),
				history = bank.GetHistory(),
				availableHours = bank.available_hours(),
				withdrawableHours = bank.withdrawable_hours(),
				payrate = bank.user_pay_rate(),
				currentPayPeriod = bank.date_start+" - "+bank.date_end
			});
		}

		[Route("ledger")]
		public IHttpActionResult Getledger()
		{
			return Getledger(0);
		}

		[Route("ledger/{payperiodId}")]
		public IHttpActionResult Getledger(int payperiodId)
		{
			var bank = new Banked_pay(CurrentUser);
			return Ok(new
			{
				ledger = bank.GetLedger(payperiodId),
				histroy = bank.GetHistory(payperiodId)
			});
		}

		[Route("AvailableHours")]
		public IHttpActionResult GetAvailableHours()
		{
			var bank = new Banked_pay(CurrentUser);
			return OkD(bank.available_hours());
		}

		[HttpDelete]
		[Route("Retract/{id}")]
		public IHttpActionResult Retract(int id)
		{
			//TODO: check does user can retract this id
			var bank = new Banked_pay(CurrentUser);
			return OkD(bank.retract_hours(id));
		}
		[Route("WithdrawableHours")]
		public IHttpActionResult GetWithdrawableHours()
		{
			var bank = new Banked_pay(CurrentUser);
			return OkD(bank.withdrawable_hours());
		}

		[HttpPost]
		[Route("BankHours")]
		public IHttpActionResult PostBankHours([FromBody] DTO.ViewModels.Core.DataDouble hour)
		{
			var bank = new Banked_pay(CurrentUser);
			return OkD(bank.bank_hours(hour.Data));
		}
		[HttpPost]
		[Route("WithdrawHours")]
		public IHttpActionResult PostWithdrawHours([FromBody] DTO.ViewModels.Core.DataDouble hour)
		{
			var bank = new Banked_pay(CurrentUser);
			return OkD(bank.withdraw_hours(hour.Data));
		}
	}
}
