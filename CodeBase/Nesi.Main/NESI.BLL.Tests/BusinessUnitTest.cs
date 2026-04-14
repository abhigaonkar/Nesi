using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NESI.BLL.Core;
using NESI.BLL.Layout.Menu;
using core;
using nesi.core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class BusinessUnitTest
	{

		public BusinessUnitTest()
		{
			NESI.DTO.Mapper.MapperConfig.Initialize();
		}
		[TestMethod]
		public void TestVisibleBusinessUnitList()
		{
			var emp = new Employee(2024);
			var bu = new BusinessUnit(emp);
			var result = bu.GetVisibileBusinessUnitDropDownLists();
		    var temp = new NeMember(1359);
			Assert.AreEqual(result.Count > 0, true);
		}

		[TestMethod]
		public void TestDashMessage()
		{
			var emp = new Employee(2024);
			var msg = new DashMessage(emp);
			var result = msg.GetDashMessageListByBusinessUnitId(11);
			Assert.AreEqual(result.Length > 0, true);
		}

		[TestMethod]
		public void TestPostDashMessage()
		{
			var emp = new Employee(2024);
			var msg = new DashMessage(emp);
			var result = msg.InsertDashMessage(new DTO.ViewModels.CurrentUser.Layout.DashMessage()
			{
				BusinessUnitId = 11,
				Text = "test"
			});
			Assert.AreEqual(result, true);
		}
	}
}
