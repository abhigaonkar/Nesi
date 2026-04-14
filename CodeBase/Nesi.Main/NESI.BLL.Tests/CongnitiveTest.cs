using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static NESI.BLL.Core.OCR.CognitiveOCR;

namespace NESI.BLL.Tests
{
	[TestClass]
	public class CongnitiveTest
	{
		[TestMethod]
		public void OCRTest()
		{
			var  o =  MakeRequest(@"d:\1.jpg");

		}
	}
}