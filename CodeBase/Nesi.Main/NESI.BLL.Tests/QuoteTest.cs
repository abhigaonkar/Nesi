using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class QuoteTest : TestBase
	{
		[TestMethod]
		public void TestToDoNowlist()
		{
			var q = new BLL.Pages.Quotes.QuoteProfile(new Employee(2024));
			Assert.AreEqual(q.Summary != null, true);
		}
	}
}
