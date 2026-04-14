using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NESI.BLL.Common.Shared;
using WebGrease.Css.Extensions;

namespace NESI.BLL.Core.OCR
{
	public class CognitiveOCR
	{
		public static string MakeRequest(string filename)
		{

			// Source: https://www.abbyy.com/en-us/receipt-capture-ocr/

			var client = new HttpClient();
			var queryString = HttpUtility.ParseQueryString(string.Empty);

			// Request headers
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "8172f7f3c43f4fe6b60c69affb10381a");

			// Request parameters
			queryString["language"] = "unk";
			queryString["detectOrientation"] = "true";
			var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr?" + queryString;

			// Request body
			byte[] byteData = GetImageAsByteArray(filename);
			string contentString;
			using (var content = new ByteArrayContent(byteData))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				var response = client.PostAsync(uri, content);
				contentString = response.Result.Content.ToString();
			}
			return contentString;
		}

		/// <summary>
		/// Returns the contents of the specified file as a byte array.
		/// </summary>
		/// <param name="imageFilePath">The image file to read.</param>
		/// <returns>The byte array of the image data.</returns>
		static byte[] GetImageAsByteArray(string imageFilePath)
		{
			FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			return binaryReader.ReadBytes((int)fileStream.Length);
		}

		/// <summary>
		/// Formats the given JSON string by adding line breaks and indents.
		/// </summary>
		/// <param name="json">The raw JSON string to format.</param>
		/// <returns>The formatted JSON string.</returns>
		static string JsonPrettyPrint(string json)
		{
			if (string.IsNullOrEmpty(json))
				return string.Empty;

			json = json.Replace(Environment.NewLine, "").Replace("\t", "");

			string INDENT_STRING = "    ";
			var indent = 0;
			var quoted = false;
			var sb = new StringBuilder();
			for (var i = 0; i < json.Length; i++)
			{
				var ch = json[i];
				switch (ch)
				{
					case '{':
					case '[':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case '}':
					case ']':
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						sb.Append(ch);
						break;
					case '"':
						sb.Append(ch);
						bool escaped = false;
						var index = i;
						while (index > 0 && json[--index] == '\\')
							escaped = !escaped;
						if (!escaped)
							quoted = !quoted;
						break;
					case ',':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case ':':
						sb.Append(ch);
						if (!quoted)
							sb.Append(" ");
						break;
					default:
						sb.Append(ch);
						break;
				}
			}
			return sb.ToString();
		}
	}
	static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
		{
			foreach (var i in ie)
			{
				action(i);
			}
		}
	}

}
