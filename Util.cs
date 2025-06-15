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

            bool escape = false;
            bool inQuotes = false;

            Dictionary<char, char> pairs = new()
            {
                ['('] = ')',
                ['['] = ']',
                ['{'] = '}',
            };

            foreach (char c in input)
            {
                if (escape)
                {
                    current.Append(c);
                    escape = false;
                    continue;
                }

                switch (c)
                {
                    case '\\':
                        escape = true;
                        continue;
                    case '"':
                        inQuotes = !inQuotes;
                        continue;
                    default:
                        break;
                }

                if (stack.Count == 0 && !inQuotes && c == ' ')
                {
                    string temp = current.ToString();
                    if (!string.IsNullOrWhiteSpace(temp)) tokens.Add(temp);
                    current.Clear();
                    continue;
                }

                if (!inQuotes)
                {
                    if (pairs.ContainsKey(c))
                        stack.Push(c);

                    if (pairs.ContainsValue(c))
                        if (stack.Count == 0 || c != pairs[stack.Pop()])
                            throw new InvalidOperationException($"Mismatched delimiter: '{c}'.");
                }

                current.Append(c);
            }

            if (escape)
                throw new InvalidOperationException("Unterminated escape sequence.");
            if (inQuotes)
                throw new InvalidOperationException("Unclosed quotes.");
            if (stack.Count > 0)
                throw new InvalidOperationException($"Unclosed delimiter: '{stack.Peek()}'.");


            if (current.Length > 0)
                tokens.Add(current.ToString());

            return [.. tokens];
        }
    }
}
