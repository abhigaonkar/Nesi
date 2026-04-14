using System.Web;
using System.Web.Util;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class CustomRequestValidator : RequestValidator
	{
		protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
		{
			if (context.Items["AllowFormHtml"] as bool? == true && requestValidationSource == RequestValidationSource.Form)
			{
				validationFailureIndex = 0;
				return true;
			}

			return base.IsValidRequestString(
				context, value, requestValidationSource, collectionKey, out validationFailureIndex);
		}
	}
}