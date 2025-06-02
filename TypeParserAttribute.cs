using System;

namespace BetterConsole
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeParserAttribute(Type parseType) : Attribute
    {
        public Type ParseType = parseType;
    }
}
