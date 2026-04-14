using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NESI.BLL.Common.Shared;
using NESI.BLL.Repository;
using NESI.Data.Entities;
using Debug = System.Diagnostics.Debug;
using System.Globalization;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.BusinessUnit;
using NESI.BLL.Pages.Timesheet;
using NESI.DTO.ViewModels.CurrentUser.Layout;
using AverageDaysToInvoice = NESI.BLL.Layout.Banner.AverageDaysToInvoice;
using Fvr = NESI.BLL.Layout.Banner.Fvr;
using Message = NESI.BLL.Layout.Banner.Message;
using QuickExtension = NESI.BLL.Layout.Banner.QuickExtension;
using Tickets = NESI.BLL.Layout.Tickets.Tickets;
using ToDo = NESI.BLL.Layout.Banner.ToDo;
using UpcomingVacations = NESI.BLL.Layout.Banner.UpcomingVacations;
using WhoDoIAsk = NESI.BLL.Layout.Banner.WhoDoIAsk;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class UserUnitTest
	{
		internal class TestModel
		{
			public int Id { get; set; }
			// ReSharper disable once InconsistentNaming
			public int User_Id { get; set; }
		}


		public UserUnitTest()
		{
			NESI.DTO.Mapper.MapperConfig.Initialize();
		}


		[TestMethod]
		public void TestDatatable()
		{
			//var user= new NESI.BLL.Member.Employee("lukelu");
			//var success=user.Authenticate("1234");
			var db = new NESIMySQL();
			var res = db.Database.SqlQuery<string>(@"SELECT fun_time(@p0)", "2017-08-02 08:41:07").FirstOrDefault();
			//var m = DTO.Mapper.DataTableMapper.Map<TestModel>(res)[0];
			Assert.AreEqual(res != "", true);


		}

		[TestMethod]
		public void TestEncryptPassword()
		{
			var a = Encryption.EncryptPassword("Waxbaaa.00");
			Assert.AreEqual(a.Length > 0, true);
		}
		[TestMethod]
		public void TestTodo()
		{
			var emp = new Employee(1359);
			var todo = new ToDo(emp);

			Assert.AreEqual(todo.BadageMenus().Count > 0, true);
		}
		[TestMethod]
		public void TestPageUrl()
		{
			var emp = new Employee(1359);

			Assert.AreEqual(emp.GetMenuRouter(0, true).Url.Length > 0, true);
		}
		[TestMethod]
		public void TestMessages()
		{

			var emp = new Employee(8);
			var messages = new Message(emp);
			Assert.AreEqual(messages.GetMessages().Length > 0, true);


		}
		[TestMethod]
		public void TestTickets()
		{
			var emp = new Employee(8);
			var tickets = new Tickets(emp);

			Assert.AreEqual(tickets.GetTickets().Length > 1, true);


		}
		[TestMethod]
		public void TestWhoDoIAsk()
		{
			var emp = new Employee(8);
			var whoDoI = new WhoDoIAsk(emp);

			Assert.AreEqual(whoDoI.GetWhoDoIAsks().Length > 1, true);


		}
		[TestMethod]
		public void TestUpcomingVacations()
		{
			var emp = new Employee(8);
			var vacations = new UpcomingVacations(emp);

			Assert.AreEqual(vacations.GetUpcomingVacations().Length > 1, true);


		}
		[TestMethod]
		public void TestAverageDaysToInvoice()
		{
			var emp = new Employee(8);
			var tickets = new AverageDaysToInvoice(emp);

			Assert.AreEqual(tickets.GetAverageDaysToInvoice() != null, true);


        }
        [TestMethod]
        public void TestQuickExtensions()
        {
            var emp = new Employee(8);
            var tickets = new QuickExtension(emp);

            Assert.AreEqual(tickets.GetQuickExtensions().Length > 1, true);
        }
        [TestMethod]
        public void TestTimesheetList()
        {
            var obj = new Timesheet();
            var date = DateTime.ParseExact("170829", "yyMMdd", CultureInfo.InvariantCulture);
            var list = obj.GetList(1359, date);
            Assert.IsNotNull(list);
        }
        
		[TestMethod]
		public void TestUser()
		{
			var obj = new Employee(2024);
			var bs = obj.VisibleBusinessUnits;
			Debug.WriteLine(bs);
			Assert.IsNotNull(bs);
		}

		[TestMethod]
		public void TestGetBusinessUnitViewModelList()
		{
			var obj = new BusinessUnit();
			var list = obj.GetList(new Employee(2024));
			Assert.IsNotNull(list);
		}

		[TestMethod]
		public void TestGetLayoutProfiles()
		{
			var obj = new Employee(2024);
			var list = obj.GetLayoutProfiles();
			Assert.IsNotNull(list);
		}
		[TestMethod]
		public void TestSaveLayoutProfiles()
		{
			var obj = new Employee(2024);
			var profiles = obj.GetLayoutProfiles();

			var result = obj.SaveProfiles(profiles.ToArray());
			Assert.AreEqual(result, true);
		}

		[TestMethod]
		public void TestGetFvrs()
		{
			var obj = new Employee(2024);
			var list = new Fvr(obj);

			Assert.AreEqual(list.GetFvrs().Length, 1);
		}

	    [TestMethod]
	    public void GetNesi1PageUrl()
	    {
	        var obj = new Employee(2024);
	        var res = obj.GetMenuRouter(170, true);

	        Assert.AreEqual(res.Url.Length > 0, true);
	    }
	    [TestMethod]
	    public void TestIdSearch()
	    {
	        var emp = new Employee(1359);
	        var search = new NESI.BLL.Layout.Banner.IdSearch(emp);

	        Assert.AreEqual(search.GetSearch("123").Length > 0, true);
	    }
		[TestMethod]
		public void TesttimesheetAutoFill()
		{
			var emp = new Employee(1359);
			var ts = new NESI.BLL.Pages.Timesheet.Timesheet(emp);

			Assert.AreEqual(ts.TimesheetAutoComments(1359, DateTime.Now.Date).Length > 0, true);
		}
	}
}
