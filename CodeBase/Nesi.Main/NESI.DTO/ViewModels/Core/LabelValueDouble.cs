namespace NESI.DTO.ViewModels.Core
{
	public class LabelValueDouble
	{
		public string Label { get; set; }
		public double Value { get; set; }

		public LabelValueDouble()
		{
			
		}

		public LabelValueDouble(string l, double v)
		{
			Label = l;
			Value = v;
		}
	}
}