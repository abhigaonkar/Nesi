using System;
using System.ComponentModel;
using System.Globalization;

namespace NESI.Common.Serialization
{
    /// <summary>
    /// Handle conversion from string to lookup, while making sure that the reverse
    /// conversion cannot take place (affects serialization)
    /// </summary>
    public class LookupConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType) => sourceType == typeof(string);
        
        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            return value is string s && !string.IsNullOrEmpty(s) ? 
                new Lookup(s) : 
                null;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, 
            Type destinationType) => false;

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType) => null;
    }
}