using System;
using System.IO;
using System.Reflection;

namespace NESI.BLL.EmbeddedFiles
{
	public static class AssemblyFileLoader
	{
		/// <summary>
		/// Loads resource file name from executing assembly
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string LoadFileFromExecutingAssembly(string fileName) =>
			LoadEmbeddedFile(fileName, Assembly.GetExecutingAssembly());

		/// <summary>
		/// Loads resource file from assembly containing the provided type
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string LoadFile<TAssemblyMarkerType>(string fileName) =>
			LoadEmbeddedFile(fileName, typeof(TAssemblyMarkerType).Assembly);
		/// <summary>
		/// Loads resource file from assembly containing the provided type
		/// Use the marker type for generating namepace for the file so that you can just provide
		/// file name
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string LoadFile<TAssemblyMarkerType, TNamespaceMarkerType>(string fileName)
			=> LoadEmbeddedFile($"{typeof(TNamespaceMarkerType).Namespace}.{fileName}", typeof(TAssemblyMarkerType).Assembly);

		/// <summary>
		/// Do the actual load, propagate exceptions
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		private static string LoadEmbeddedFile(string fileName, Assembly assembly)
		{
			using (var stream = assembly.GetManifestResourceStream(fileName))
			using (var reader = new StreamReader(stream ?? throw new InvalidOperationException("Unable to load file")))
			{
				var result = reader.ReadToEnd();
				return result;
			}
		}

	}
}