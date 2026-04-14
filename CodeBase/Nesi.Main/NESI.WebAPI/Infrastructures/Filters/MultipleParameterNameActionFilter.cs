using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace NESI.WebAPI.Infrastructures.Filters
{
	[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class MultipleNameBindAttribute : Attribute
	{
		public MultipleNameBindAttribute(string parameters)
		{
			if (!String.IsNullOrEmpty(parameters))
			{
				AvailableName = parameters.Split(',');
			}
		}

		public string[] AvailableName { get; private set; }
	}

	public class MultipleParameterNameActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var defaults = GetDefaults(filterContext);
			var actionParameters = filterContext.ActionParameters;
			foreach (var value in defaults)
				if (actionParameters[value.Key] == null)
					actionParameters[value.Key] = value.Value;
		}

		internal static IDictionary<string, object> GetDefaults(ActionExecutingContext filterContext)
		{
			IDictionary<string, object> defaults = null;
			defaults = new Dictionary<string, object>(filterContext.ActionParameters.Count);
			foreach (var parameter in filterContext.ActionDescriptor.GetParameters())
			{
				if (parameter.IsDefined(typeof(MultipleNameBindAttribute), false))
				{
					MultipleNameBindAttribute attr =
						parameter.GetCustomAttributes(typeof(MultipleNameBindAttribute), false)[0] as MultipleNameBindAttribute;
					string parameterName = parameter.ParameterName;
					object parameterValue = filterContext.ActionParameters[parameterName];
					foreach (var name in attr.AvailableName)
					{
						var value = filterContext.HttpContext.Request[name];
						if (string.IsNullOrEmpty(value)) continue;
						parameterValue = value;
					}

					try
					{
						defaults.Add(parameterName, Convert.ChangeType(parameterValue, parameter.ParameterType));
					}
					catch (Exception exc)
					{
						throw new InvalidOperationException(String.Format(
							CultureInfo.CurrentUICulture,
							"The value of the DefaultAttribute could not be converted to the parameter '{0}'",
							parameterName), exc);
					}
				}
			}

			return defaults;
		}
	}
}