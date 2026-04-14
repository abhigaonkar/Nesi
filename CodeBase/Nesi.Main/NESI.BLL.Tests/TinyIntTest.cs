using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class TinyIntTest
	{
		[TestMethod]
		public void PassportTableTest()
		{
			var db = new NESI.Data.Entities.NESIMySQL();
			var item1 = db.passport.FirstOrDefault(x => x.id == 1);
			var item2 = db.passport.FirstOrDefault(x => x.id == 2);
			Assert.AreEqual(item1?.valid, true);
			Assert.AreEqual(item2?.valid, false);
		}

	}
}