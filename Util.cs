using System;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static string[] ParseArgs(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+");
            return [.. matches.Cast<Match>().Select(static m =>
            {
                string s = m.Value;
                return s.StartsWith("\"") && s.EndsWith("\"")
                    ? s.Substring(1, s.Length - 2)
                    : s;
            })];
        }
    }
}
