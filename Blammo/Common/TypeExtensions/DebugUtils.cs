using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Agent.Common.TypeExtensions
{
    public static class DebugUtils
    {
        public static string GetPropertiesNameValueString(this object objectToDebug)
        {
            var builder = new StringBuilder();

            Type objectType = objectToDebug.GetType();

            builder.AppendLine(objectType.Name);

            PropertyInfo[] properties = objectType.GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    object propertyValue = propertyInfo.GetValue(objectToDebug, null);

                    string propertyValueString = "null";

                    if (propertyValue != null)
                    {
                        if (propertyValue is IEnumerable)
                        {
                            propertyValueString = GetEnumerableValuesString((IEnumerable) propertyValue);
                        }
                        else
                        {
                            propertyValueString = propertyValue.ToString();
                        }
                    }

                    builder.AppendLine(String.Format("{0} = {1}",
                        propertyInfo.Name,
                        propertyValueString));
                }
            }

            return builder.ToString();
        }

        public static string GetEnumerableValuesString(this IEnumerable enumerableObject)
        {
            string debugString;

            if (HasCustomToStringImplementation(enumerableObject))
            {
                debugString = enumerableObject.ToString();
            }
            else
            {
                var valueBuilder = new StringBuilder();

                valueBuilder.Append('[');

                string itemSeparator = ", ";

                foreach (object item in enumerableObject)
                {
                    valueBuilder.AppendFormat("{0}{1}", item.ToString(), itemSeparator);
                }

                debugString = valueBuilder.ToString();

                int endCharacterIndex = debugString.Length - itemSeparator.Length;

                if (endCharacterIndex > 0)
                {
                    //                    debugString = debugString.Remove(endCharacterIndex);
                }

                debugString += ']';
            }

            return debugString;
        }

        private static bool HasCustomToStringImplementation(object propertyValue)
        {
            bool customToStringExists;

            try
            {
                MethodInfo toStringMethod = propertyValue.GetType().GetMethod("ToString",
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.DeclaredOnly);

                customToStringExists = toStringMethod != null;
            }
            catch (AmbiguousMatchException)
            {
                customToStringExists = true;
            }

            return customToStringExists;
        }
    }
}