using System.Reflection;

namespace BetterConsole
{
    public class CommandEntry
    {
        public string Command { get; }
        public string DomainName { get; }
        public string HelpMessage { get; }
        public MethodInfo MethodInfo { get; }
        public ParameterInfo[] ParameterInfo { get; }

        public string FullName => string.IsNullOrEmpty(DomainName) ? Command : DomainName + "." + Command;

        public CommandEntry(MethodInfo methodInfo)
        {
            CommandClassCustomizerAttribute customAttribute = methodInfo.DeclaringType.GetCustomAttribute<CommandClassCustomizerAttribute>();
            Command = methodInfo.Name;
            DomainName = customAttribute?.NewDomainName ?? methodInfo.DeclaringType.Name;

            CommandHelpAttribute helpAttribute = methodInfo.GetCustomAttribute<CommandHelpAttribute>();
            HelpMessage = helpAttribute.HelpMessage ?? string.Empty;

            ParameterInfo = methodInfo.GetParameters();
            MethodInfo = methodInfo;
        }
    }
}
