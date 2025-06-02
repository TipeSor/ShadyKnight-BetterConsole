using System;

namespace BetterConsole
{
    public interface ITypeParser
    {
        bool TryParse(string input, out object value);
        Type TargetType { get; }
    }

    public abstract class TypeParser<T> : ITypeParser
    {
        public abstract bool TryParse(string input, out T value);
        public bool TryParse(string input, out object value)
        {
            bool success = TryParse(input, out T result);
            value = result;
            return success;
        }

        public Type TargetType => typeof(T);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeParserAttribute(Type parseType) : Attribute
    {
        public Type ParseType = parseType;
    }
}
