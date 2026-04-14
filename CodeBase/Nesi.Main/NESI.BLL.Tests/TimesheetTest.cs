using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages;
using NESI.BLL.Pages.Timesheet;
using NESI.BLL.Pages.Timesheet.Expense;
using NESI.BLL.Pages.Timesheet.Vacation;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class TimesheetTest
	{
		public TimesheetTest()
		{
			NESI.DTO.Mapper.MapperConfig.Initialize();
		}
		[TestMethod]
		public void TestTimeSheetList()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
			var list = t.GetList(2024, DateTime.Parse("2017-08-25"));
			Assert.AreEqual(list.Count > 0, true);
		}
		[TestMethod]
		public void TestTimeVacationHistory()
		{
			var emp = new Employee(2024);
			var t = new Vacation(emp);
			var list = t.View_Past();
			Assert.AreEqual(list.Rows.Count > 0, true);
		}
		[TestMethod]
		public void TestTimeSheetPerDiemGetForEmployees()
		{
			var emp = new Employee(2024);
			var t = new ExpensePerDiem(emp);
			var list = t.GetForEmployees(11, 2024);
			Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestTimeSheetGetQuoteQuote()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
			var list = t.GetQuoteQuoteById(1434351);
			Assert.AreEqual(list != null, true);
		}
		[TestMethod]
		public void TestPhoneLog()
		{

			var emp = new Employee(2024);
			var t = new Ne2PhoneLog();
			var list = t.GetPhoneLogByExt("571", new DateTime(2017, 8, 28));
			Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestPassDay()
		{
			var emp = new Employee(2093);
			var t = new Timesheet(emp);
			var list = t.GetPastDays();
			Assert.AreEqual(list != null, true);
		}

		[TestMethod]
		public void TestLabbor()
		{
			var emp = new Employee(2093);
			var t = new Timesheet();
			var list = t.GetlabourMasterId(109, emp, 1);
			Assert.AreEqual(list > 0, true);
		}
		[TestMethod]
		public void TestShopType()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
            var list = t.GetShopTypeList(emp);
            Assert.AreEqual(list.Length > 0, true);
		}

		[TestMethod]
		public void TestQuoteCustomer()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
			var list = t.GetQuoteCustomerList(11, emp);
			Assert.AreEqual(list.Length > 0, true);
		}

		[TestMethod]
		public void TestQuoteQuote()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
			var list = t.GetQuoteQuoteList(11, 103173);
			Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestFillBusinessUnits()
		{
			var emp = new Employee(1359);
			var t = new Timesheet(emp);
			//	var list = t.GetCustomerWorkOrderList(11, emp);
			//	Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestFillCustomer()
		{
			var emp = new Employee(1359);
			var t = new Timesheet(emp);
			var list = t.GetCustomerWorkOrderList(11, emp);
			Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestLoadWorkorder()
		{
			var emp = new Employee(1359);
			var t = new Timesheet(emp);
			//var list = t.LoadCustWOPROGWIPLIST(11, emp, 340);
			//Assert.AreEqual(list.Length > 0, true);
		}
		[TestMethod]
		public void TestLoadWorkorderById()
		{
			var emp = new Employee(1359);
			var t = new Timesheet(emp);
			//	var list = t.GetWOPROGById(1024451);
			//	Assert.AreEqual(list != null, true);
		}
		[TestMethod]
		public void TestLoadWorkorderComment()
		{
			var emp = new Employee(2024);
			var t = new Timesheet(emp);
			var list = t.LoadWOCommentsList(11, 2024, 1020043);
			Assert.AreEqual(list.Length > 0, true);
		}
	}
}