using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using NESI.Common.Serialization;

namespace NESI.Common
{
    public static class ObjectHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Cast the given item to a value of the given type using reflection property
        /// mapping. Only public properties will be mapped
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T Cast<T>(object item)
            where T : class, new()
        {

            if (item == null) return default(T);
            var targetInstance = new T();

            /*
             * We could cache these type maps, but the runtime is overshadowed by the Get/Set property value use
            */

            var sourcePropertyMap = item
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(pi => pi.Name, pi => pi);

            var targetPropertyMap = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi=>pi.GetCustomAttribute<ReflectionMapIgnoreAttribute>() == null)
                .ToDictionary(pi => pi.Name, pi => pi);

            /*
             * Here we could traverse both and output unmapped properties
             * For now warn only if target props have no source equivalent
             */

            foreach (var targetPropertyName in targetPropertyMap.Keys)
            {
                if (sourcePropertyMap.TryGetValue(targetPropertyName, out PropertyInfo sourcePropertyInfo))
                {
                    MapProperty(targetInstance, targetPropertyMap[targetPropertyName], item, sourcePropertyInfo);
                }
                else
                {
                    Logger.Warn($"Property {targetPropertyName} is unmapped in source type {item.GetType().Name}");
                }
            }

            return targetInstance;
        }

        /// <summary>
        /// Map the given key value pair to an object of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourcePropertyValueMap"></param>
        /// <returns></returns>
        public static T Cast<T>(IDictionary<string, object> sourcePropertyValueMap)
            where T : class, new()
        {

            if (sourcePropertyValueMap == null) return default(T);
            var targetInstance = new T();

            var targetPropertyMap = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.GetCustomAttribute<ReflectionMapIgnoreAttribute>() == null)
                .ToDictionary(pi => pi.Name, pi => pi);

            foreach (var targetPropertyName in targetPropertyMap.Keys)
            {
                if (sourcePropertyValueMap.TryGetValue(targetPropertyName, out var sourcePropertyValue) && sourcePropertyValue != null)
                {
                    MapPropertyToValue(targetInstance, targetPropertyMap[targetPropertyName], sourcePropertyValue,
                        sourcePropertyValue.GetType());
                }
                else
                {
                    Logger.Warn($"Property {targetPropertyName} is unmapped in source.");
                }
            }

            return targetInstance;
        }

        /// <summary>
        /// Copy source property info into target property info
        /// </summary>
        /// <param name="targetInstance"></param>
        /// <param name="targetProperty"></param>
        /// <param name="sourceInstance"></param>
        /// <param name="sourcePropertyInfo"></param>
        private static void MapProperty(object targetInstance, PropertyInfo targetProperty, object sourceInstance, PropertyInfo sourcePropertyInfo)
        {
            if (targetInstance == null) throw new ArgumentNullException(nameof(targetInstance));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
            if (sourcePropertyInfo == null) throw new ArgumentNullException(nameof(sourcePropertyInfo));
            var sourceValue = sourcePropertyInfo.GetValue(sourceInstance);
            MapPropertyToValue(targetInstance, targetProperty, sourceValue, sourcePropertyInfo.PropertyType);
        }

        /// <summary>
        /// Map the source value into the provided target property on the target instance object
        /// </summary>
        /// <param name="targetInstance"></param>
        /// <param name="targetProperty"></param>
        /// <param name="sourceValue"></param>
        /// <param name="sourceType"></param>
        private static void MapPropertyToValue(object targetInstance, PropertyInfo targetProperty, object sourceValue, Type sourceType)
        {
            /*
           * We need to deal with type mismatches. First, look for custom defined type converters,
            * then try to use the built-in type conversion system safely (supress errors)
           */
            if (sourceType != targetProperty.PropertyType)
            {
                var converter = TypeDescriptor.GetConverter(targetProperty.PropertyType);
                if (converter.CanConvertFrom(sourceType))
                {
                    sourceValue = converter.ConvertFrom(sourceValue);
                }
                else
                {
                    /*
                     * Try the builtin system and if it doesn't work abandon this property
                     */
                    if (!sourceValue.SafeConvert(targetProperty.PropertyType, out sourceValue)) return;
                }
            }
            targetProperty.SetValue(targetInstance, sourceValue);
        }

        /// <summary>
        /// Attempt to convert the provided item to the specified target type
        /// If the types implement <see cref="IConvertible"/> then this conversion will likely succeed
        /// </summary>
        /// <param name="item"></param>
        /// <param name="targetType"></param>
        /// <param name="converted"></param>
        /// <returns></returns>
        private static bool SafeConvert(this object item, Type targetType, out object converted)
        {
            try
            {
                converted = Convert.ChangeType(item, targetType);
                return true;
            }
            catch (InvalidCastException)
            {
                Logger.Warn($"Conversion from {item.GetType().Name} to {targetType.Name} is not currently supported");
                converted = null;
                return false;
            }
        }
    }
}
