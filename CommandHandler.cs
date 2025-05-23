using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    internal static class CommandHandler
    {
        public delegate void CommandDelegate(object[] args);

        internal class CommandEntry(string name, MethodInfo method, object target, string help = "")
        {
            internal string name = name;
            internal MethodInfo method = method;
            internal ParameterInfo[] parameters = method.GetParameters();
            internal object target = target;
            internal string help = help;

            internal void Invoke(string[] args)
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
                        convertedArgs[i] =
                            paramType == typeof(bool) ? bool.Parse(args[i]) :
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

        internal static bool TryGetCommand(string command_name, out CommandEntry output_command)
        {
            output_command = CommandList.FirstOrDefault(c => c.name == command_name);
            return output_command != null;
        }

        internal static void InitializeCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
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
                            string name = $"{method_.Name.ToLower()}";
                            if (class_ != typeof(GameCommands))
                            {
                                name = $"{class_.Name.ToLower()}.{name}";
                            }

                            string help = "";
                            if (Attribute.IsDefined(method_, typeof(CommandHelp)))
                            {
                                CommandHelp commandHelp = method_.GetCustomAttribute<CommandHelp>();
                                help = commandHelp.commandHelp;
                            }

                            CommandEntry commandEntry = new(name, method_, null, help);
                            CommandList.Add(commandEntry);
                            Console.WriteLine($"Created command: '{commandEntry.name}'");
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
            if (!TryGetCommand(commandName, out CommandEntry command))
            {
                Console.WriteLine($"Command '{commandName}' not found.");
                return;
            }

            command.Invoke(args);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Command : Attribute
    {
        public Command() { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandHelp(string CommandHelp) : Attribute
    {
        public string commandHelp = CommandHelp;
    }
}
