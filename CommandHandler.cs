using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    internal static class CommandHandler
    {
        public delegate void CommandDelegate(object[] args);

        public struct CommandEntry(string name, MethodInfo method, object target)
        {
            public string name = name;
            public MethodInfo method = method;
            public ParameterInfo[] parameters = method.GetParameters();
            public object target = target;

            public readonly void Invoke(string[] args)
            {
                try
                {
                    object[] convertedArgs = ConvertArgs(args, parameters);
                    _ = method.Invoke(target, convertedArgs);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command '{name}': {ex.Message}");
                }
            }

            private static object[] ConvertArgs(string[] args, ParameterInfo[] parameters)
            {
                if (args.Length != parameters.Length)
                {
                    throw new ArgumentException($"Expected {parameters.Length} arguments, but got {args.Length}.");
                }

                object[] convertedArgs = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    Type paramType = parameters[i].ParameterType;
                    try
                    {
                        convertedArgs[i] = paramType ==
                            typeof(bool) ? bool.Parse(args[i]) :
                            paramType.IsEnum ? Enum.Parse(paramType, args[i], true) :
                            Convert.ChangeType(args[i], paramType);
                    }
                    catch
                    (Exception)
                    {
                        throw new ArgumentException($"Invalid argument '{args[i]}' for parameter {parameters[i].Name} (expected type: {paramType.Name}).");
                    }
                }

                return convertedArgs;
            }
        }

        private static readonly List<CommandEntry> CommandList = [];

        private static CommandEntry GetCommandByName(string command_name)
        {
            return CommandList.First(c => c.name == command_name);
        }

        private static bool HasCommand(string command_name)
        {
            return CommandList.Any(c => c.name == command_name);
        }

        internal static void InitializeCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<CommandEntry> command_list = [];

            foreach (Assembly assembly in assemblies)
            {
                Type[] classes = assembly.GetTypes();
                foreach (Type class_ in classes)
                {
                    MethodInfo[] methods = class_.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    foreach (MethodInfo method_ in methods)
                    {
                        if (!Attribute.IsDefined(method_, typeof(Command)))
                        {
                            continue;
                        }

                        try
                        {
                            string name = $"{class_.Name.ToLower()}.{method_.Name.ToLower()}";
                            if (class_.GetType() == typeof(GameCommands))
                            {
                                name = $"{method_.Name.ToLower()}";
                            }

                            CommandEntry commandEntry = new(name, method_, null);
                            command_list.Add(commandEntry);
                            Console.WriteLine($"Created command: {commandEntry.name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Cannot create command: {method_.Name} - {ex.Message}");
                        }
                    }
                }
            }
        }

        internal static void ExecuteCommand(string commandName, string[] args)
        {
            if (!HasCommand(commandName))
            {
                Console.WriteLine($"Command '{commandName}' not found.");
                return;
            }

            CommandEntry command = GetCommandByName(commandName);
            command.Invoke(args);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Command : Attribute
    {
        public Command() { }
    }
}
