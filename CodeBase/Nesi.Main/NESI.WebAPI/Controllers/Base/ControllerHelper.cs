using System;

namespace NESI.WebAPI.Controllers.Base
{
	public static class ControllerHelper
	{
		public static object ConvertoJsonData(object value)
		{
			return new { Data = value };
		}
	}
}