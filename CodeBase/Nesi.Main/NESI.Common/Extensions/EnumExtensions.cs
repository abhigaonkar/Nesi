using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NESI.Common.Extensions
{
	/// <summary>
	/// General enumeration extensions
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Get the <see cref="DescriptionAttribute"/> value associated with enum member
		/// or the <see cref="Enum.ToString()"/> if none provided
		/// </summary>
		/// <param name="value"></param>
		public static string GetEnumDescription(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
            if(fi == null) throw new InvalidOperationException($"Unable to retrieve enum field for {value}. Ensure not multiple flags set.");

			var descriptionAttribute = fi.GetCustomAttributes(
				typeof(DescriptionAttribute),
				false).OfType<DescriptionAttribute>().FirstOrDefault();
			return descriptionAttribute?.Description ?? value.ToString();
		}

	    /// <summary>
	    /// Get all the set enums for the given flags enum
	    /// </summary>
	    /// <param name="input"></param>
	    /// <param name="exclusions">Optional list of flags to ignore from result</param>
	    /// <remarks>If the passed in enum type is not a <see cref="FlagsAttribute"/> enum </remarks>
	    /// <returns></returns>
	    public static IEnumerable<Enum> GetSetFlags(this Enum input, params Enum[] exclusions)
		{
            if(input.GetType().GetCustomAttribute<FlagsAttribute>() == null) throw new ArgumentException("Method not valid for non-flags enum", nameof(input));
            if(exclusions.Any(excl=>excl.GetType()!= input.GetType())) throw new ArgumentException("Exclusion types do not match input type", nameof(exclusions));
			foreach (Enum value in Enum.GetValues(input.GetType()))
				if (input.HasFlag(value) && !exclusions.Contains(value))
					yield return value;
		}

	    /// <summary>
	    /// Extract all descriptions for all set flags in an enum
	    /// </summary>
	    /// <param name="input"></param>
	    /// <param name="exclusions"></param>
	    /// <returns></returns>
	    public static IEnumerable<string> GetSetFlagsDescriptions(this Enum input, params Enum[] exclusions)
	    {
            if (input.GetType().GetCustomAttribute<FlagsAttribute>() == null) throw new ArgumentException("Method not valid for non-flags enum", nameof(input));
            return input.GetSetFlags(exclusions).Select(setFlag => setFlag.GetEnumDescription());
	    }
	}
}