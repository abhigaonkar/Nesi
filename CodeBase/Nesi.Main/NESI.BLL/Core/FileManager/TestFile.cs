namespace NESI.BLL.Core.FileManager
{
	public class TestFile : NeFileBase
	{
		public override string BaseFolder => @"E:\Test";
		public override string BasePath => "";
		public override void Validate_folder_contents()
		{
			throw new System.NotImplementedException();
		}

		public override void CreateFolder()
		{
			throw new System.NotImplementedException();
		}
	}
}