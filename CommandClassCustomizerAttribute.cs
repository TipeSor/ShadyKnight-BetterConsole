using System;

namespace BetterConsole
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandClassCustomizerAttribute(string newDomainName) : Attribute
    {
        public string NewDomainName = newDomainName;
    }
}
