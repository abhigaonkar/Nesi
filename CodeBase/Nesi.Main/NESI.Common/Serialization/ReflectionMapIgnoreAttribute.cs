using System;

namespace NESI.Common.Serialization
{
    /// <summary>
    /// Class we we will use to mark properties as ignored for purposes of reflection
    /// based mapping. This is for cases where it's not convenient to introduce type converters, etc
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReflectionMapIgnoreAttribute : Attribute
    {

    }
}