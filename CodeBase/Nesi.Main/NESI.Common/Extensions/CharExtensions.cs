namespace NESI.Common.Extensions
{
	public static class CharExtensions
	{
		/// <summary>
		/// Returns true if the character is between 0 and 9
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsDigit(this char c) => c >= '0' && c <= '9';

		/// <summary>
		///     Returns true if the character is between 'A' and 'Z'
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsUpper(this char c) => c >= 'A' && c <= 'Z';

		/// <summary>
		///     Returns true if the character is between 'a' and 'z'
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsLower(this char c) => c >= 'a' && c <= 'z';

		/// <summary>
		///     Returns true if the character is upper, lower, or a digit
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsLetterOrDigit(this char c) => c.IsUpper() || c.IsLower() || c.IsDigit();
	}
}