namespace NESI.DTO.ViewModels.Core
{
	public class LabelValueString
	{
		public string Label { get; set; }
		public string Value { get; set; }

		public LabelValueString()
		{
			
		}

		public LabelValueString(string str)
		{
			Label = str;
			Value = str;
		}

		public LabelValueString(string l,string v)
		{
			Label = l;
			Value = v;
		}
	}
}