using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable IDE0011, IDE0058
namespace BetterConsole
{
    public static class Utils
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

        public static string[] SplitInput(string input)
        {
            List<string> tokens = [];
            StringBuilder current = new();
            Stack<char> stack = new();

            bool inQuotes = false;
            char lastChar = '\0';

            Dictionary<char, char> pairs = new()
            {
                ['('] = ')',
                ['['] = ']',
                ['{'] = '}'
            };

            foreach (char c in input)
            {
                if (c == '"' && lastChar != '\\')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                lastChar = c;

                if (stack.Count == 0 && !inQuotes && c == ' ')
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                if (pairs.ContainsKey(c))
                {
                    stack.Push(c);
                    current.Append(c);
                    continue;
                }

                if (pairs.ContainsValue(c))
                {
                    if (stack.Count == 0 || c != pairs[stack.Pop()])
                        throw new InvalidOperationException("Mismatched delimiter.");
                }

                current.Append(c);
            }

            if (current.Length > 0)
                tokens.Add(current.ToString());

            return [.. tokens];
        }
    }
}
