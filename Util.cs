using System;
using System.Linq;

namespace BetterConsole
{
    public class Utils
    {
        public static string GetCleanTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                string genericBase = type.Name.Substring(0, type.Name.IndexOf('`'));
                string genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetCleanTypeName));
                return $"{genericBase}<{genericArgs}>";
            }

            return type.Name;
        }
    }
}
