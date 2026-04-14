namespace NESI.BLL.Tests
{
	public class TestBase
	{
		public TestBase()
		{
			NESI.DTO.Mapper.MapperConfig.Initialize();
		}
	}
}