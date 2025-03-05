using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    internal static class CommandHandler
    {
        public delegate void Command(params string[] args);

        public struct commandEntry
        {
            public string name;

            public Command function;

            public commandEntry(string name, Command function)
            {
                this.name = name;
                this.function = function;
            }
        }

        private static List<commandEntry> commandList = new List<commandEntry>();

        public static commandEntry GetCommandByName(string command_name) => commandList.First(c => c.name == command_name);
        public static bool HasCommand(string command_name) => commandList.Any(c => c.name == command_name);

        public static void InitializeCommands()
        {
            Assembly current_assembly = Assembly.GetExecutingAssembly();
            Type[] classes = current_assembly.GetTypes();

            List<commandEntry> command_list = new List<commandEntry>();

            foreach (Type class_ in classes)
            {
                MethodInfo[] methods = class_.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                foreach (MethodInfo method_ in methods)
                {
                    if (!Attribute.IsDefined(method_, typeof(CustomCommand))) continue;
                    try
                    {
                        string name = "";
                        if (class_ == typeof(CommandList)) name = $"{method_.Name.ToLower()}";
                        else name = $"{class_.Name.ToLower()}.{method_.Name.ToLower()}";

                        Command function = (Command)method_.CreateDelegate(typeof(Command));
                        commandEntry command = new commandEntry
                        {
                            name = name,
                            function = function,
                        };

                        command_list.Add(command);

                        Console.WriteLine($"Created command: {command.name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot create command: '{method_.Name}'");
                        Console.WriteLine($"{ex}");
                    }

                }
            }

            commandList.AddRange(command_list);
        }

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomCommand : Attribute
    {
        public CustomCommand() { }
    }
}
