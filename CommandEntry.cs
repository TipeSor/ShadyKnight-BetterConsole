using System.Reflection;

namespace BetterConsole
{
    public struct CommandEntry
    {
        public string Command;
        public string DomainName;
        public readonly string FullName;
        public readonly string HelpMessage;
        public MethodInfo MethodInfo;
        public ParameterInfo[] ParameterInfo;

        public CommandEntry(MethodInfo methodInfo)
        {
            CommandClassCustomizerAttribute customAttribute = methodInfo.DeclaringType.GetCustomAttribute<CommandClassCustomizerAttribute>();
            Command = methodInfo.Name;
            DomainName = customAttribute?.NewDomainName ?? methodInfo.DeclaringType.Name;
            FullName = string.IsNullOrEmpty(DomainName) ? Command : DomainName + "." + Command;

            CommandHelpAttribute helpAttribute = methodInfo.GetCustomAttribute<CommandHelpAttribute>();
            HelpMessage = helpAttribute.HelpMessage ?? string.Empty;

            ParameterInfo = methodInfo.GetParameters();
            MethodInfo = methodInfo;
        }
    }
}
