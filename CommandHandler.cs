using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace BetterConsole
{
    internal static class CommandHandler
    {
        public delegate void CommandDelegate(object[] args);

        internal class CommandEntry(string name, MethodInfo method, string help = "")
        {
            internal string name = name;
            internal MethodInfo method = method;
            internal ParameterInfo[] parameters = method.GetParameters();
            internal string help = help;
            private readonly Delegate compiledDelegate = CompileDelegate(method);

            private static Delegate CompileDelegate(MethodInfo method)
            {
                Type[] paramTypes = [.. method.GetParameters().Select(static p => p.ParameterType)];
                Type delegateType = Expression.GetActionType(paramTypes);
                return method.CreateDelegate(delegateType);
            }

            internal void Invoke(string[] args)
            {
                try
                {
                    object[] convertedArgs = ConvertArgs(args, parameters);
                    _ = compiledDelegate.DynamicInvoke(convertedArgs);
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

        private static readonly Dictionary<string, CommandEntry> CommandList = [];
        public static string[] GetCommandList()
        {
            return [.. CommandList.Keys];
        }

        internal static bool TryGetCommand(string name, out CommandEntry command)
        {
            return CommandList.TryGetValue(name, out command);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void RegisterCommands()
        {
            BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.Static;

            foreach (MethodInfo method in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetMethods(MethodFlags))
                .Where(m => Attribute.IsDefined(m, typeof(Command))))
            {
                Type type = method.DeclaringType;
                string name = method.Name.ToLowerInvariant();
                if (type != typeof(GameCommands))
                {
                    name = $"{type.Name.ToLowerInvariant()}.{name}";
                }

                string help = method.GetCustomAttribute<CommandHelp>()?.commandHelp ?? string.Empty;
                CommandList[name] = new(name, method, help);
                Console.WriteLine($"Created command: `{name}`");
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
