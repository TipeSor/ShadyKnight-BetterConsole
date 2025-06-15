using System;

namespace BetterConsole
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandDomainAttribute(string newDomainName) : Attribute
    {
        public string NewDomainName { get; } = newDomainName;
    }
}
