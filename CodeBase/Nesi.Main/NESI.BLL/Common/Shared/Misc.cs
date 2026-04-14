using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NESI.BLL.Common.Shared
{
	public struct Boolstr
	{
		public bool Success;
		public string Message;
	}

	public static class Misc
	{
		public static Dictionary<string, string> dict_create(object obj)
		{
			var result = new Dictionary<string, string>();
			var t = obj.GetType();
			var properties = t.GetProperties();

			foreach (var property in properties)
			{
				try
				{
					var value = property.GetValue(obj, null);
					if (value != null)
					{
						DateTime parsedt;
						if (property.PropertyType == Type.GetType("System.String") && DateTime.TryParse(value.ToString(), out parsedt))
						{
							value = DataTypeConvert.MySQL_shortdt(parsedt);
						}
						var displayValue = value.ToString();
						if (value is string) displayValue = string.Concat('"', displayValue, '"');
						if (value is ArrayList
							// || value is NeBusinessUnit
							//TODO: after we finished NeBusinessUnit class
							)
						{

						}
						else
						{
							result.Add(property.Name, displayValue);
						}
					}
					else
					{
						result.Add(property.Name, "null");
					}
				}
				catch
				{
					// ignored
				}
			}

			return result;
		}
	
	

		public static string dict_dump(Dictionary<string, string> obj)
		{
			var result = new StringBuilder();
			foreach (var e in obj)
			{
				result.AppendFormat("{0} - {1}<br/>", e.Key, e.Value);
			}
			return result.ToString();
		}
	}
}