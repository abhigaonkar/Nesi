using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class TestFileManager: TestBase
	{
		[TestMethod]
		public void TestGetDirectory()
		{
			var dir = new BLL.Core.FileManager.WorkOrderFile(1024451).GetDirectory();
			Assert.AreEqual(dir != null, true);
		}
	}
}
