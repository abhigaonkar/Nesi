namespace NESI.DTO.ViewModels.Core
{
	public class LabelValueInt
	{
		public string Label { get; set; }
		public int Value { get; set; }

		public LabelValueInt()
		{
			
		}

		public LabelValueInt(string l, int v)
		{
			Label = l;
			Value = v;
		}
	}
}