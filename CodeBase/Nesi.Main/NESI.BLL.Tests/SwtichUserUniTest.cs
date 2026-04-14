using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Tests
{

	[TestClass]
	public class SwtichUserUniTest
	{
		public SwtichUserUniTest()
		{
			NESI.DTO.Mapper.MapperConfig.Initialize();
		}

		[TestMethod]
		public void TestSwitchUser()
		{
			var obj = new Employee(2024);
			Assert.AreEqual(obj.Password,"1234@nesi");
		}
		[TestMethod]
		public void TestRequestPassword()
		{
			
		}
	}
}