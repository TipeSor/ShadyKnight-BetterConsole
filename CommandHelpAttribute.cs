using System;

namespace BetterConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandHelpAttribute(string helpMessage) : Attribute
    {
        public readonly string HelpMessage = helpMessage;
    }
}
