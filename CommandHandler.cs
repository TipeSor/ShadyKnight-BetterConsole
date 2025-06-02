using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    public static class CommandHandler
    {
        private static readonly Dictionary<string, CommandEntry> commandEntries = [];
        private static readonly Dictionary<Type, TypeParser<object>> typeParsers = [];
        private static bool loaded = false;

        public static void Initialize()
        {
            if (!loaded)
            {
                loaded = true;

                BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
                (MethodInfo, CommandAttribute)[] commands = ReflectionUtility.GetMethodsWithAttribute<CommandAttribute>(flags: flags);
                (Type, TypeParserAttribute)[] parsers = ReflectionUtility.GetClassesWithAttribute<TypeParserAttribute>();

                foreach ((MethodInfo method, CommandAttribute _) in commands)
                {
                    CommandEntry commandEntry = new(method);
                    commandEntries[commandEntry.FullName] = commandEntry;
                    Plugin.Logger.LogInfo($"Initilized command: {commandEntry.FullName}");
                }

                foreach ((Type type, TypeParserAttribute attribute) in parsers)
                {
                    typeParsers.Add(attribute.ParseType, (TypeParser<object>)Activator.CreateInstance(type));
                    Plugin.Logger.LogInfo($"Initialized Type Parser for {attribute.ParseType}");
                }
            }
        }

        public static bool TryGetCommand(string commandName, out CommandEntry commandEntry)
        {
            return commandEntries.TryGetValue(commandName, out commandEntry);
        }

        public static void ProcessInput(string input)
        {
            string[] str = Utils.SplitInput(input);
            ProcessCommand(str[0], [.. str.Skip(1)]);
        }

        public static void ProcessCommand(string command, string[] args)
        {
            if (!commandEntries.TryGetValue(command, out CommandEntry commandEntry))
            {
                Plugin.Logger.LogError($"command `{command}` not found");
                return;
            }

            ParameterInfo[] parameters = commandEntry.ParameterInfo;

            if (args.Length != parameters.Length)
            {
                Plugin.Logger.LogError("argument length doesnt match parameter length");
                return;
            }

            object[] parsedParameters = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;
                if (!typeParsers.TryGetValue(parameterType, out TypeParser<object> parser))
                {
                    Plugin.Logger.LogError($"No parser for type `{parameterType.Name}`");
                    return;
                }

                if (!parser.TryParse(args[i], out object value))
                {
                    Plugin.Logger.LogError($"Invalid argument '{args[i]}' for parameter {parameters[i].Name} (expected type: {parameters[i].ParameterType.Name}).");
                    return;
                }

                parsedParameters[i] = value;
            }

            InvokeCommand(commandEntry, parsedParameters);
        }

        public static void InvokeCommand(CommandEntry command, object[] args)
        {
            try
            {
                _ = command.MethodInfo.Invoke(null, args);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
            }
        }
    }
}
