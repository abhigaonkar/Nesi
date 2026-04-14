using Newtonsoft.Json.Converters;

namespace NESI.DTO.Validation
{
	public class DateFormatConverter : IsoDateTimeConverter
	{
		public DateFormatConverter(string format)
		{
			DateTimeFormat = format;
		}
	}

	public class DateOnlyFormatConverter : DateFormatConverter
	{
		public DateOnlyFormatConverter() : base("yyyy-MM-dd")
		{
		}
	}

	public class DateTimeFormatConverter : DateFormatConverter
	{
		public DateTimeFormatConverter() : base("yyyy-MM-dd HH:mm:ss")
		{
		}
	}
}