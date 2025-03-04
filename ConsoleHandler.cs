using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    public static class Commands
    {
        public delegate void Command(params object[] args);

        public struct command
        {
            public string name;

            public Command function;

            public command(string name, Command function)
            {
                this.name = name;
                this.function = function;
            }
        }

        public static List<command> commandList = new List<command>();

        public static command GetCommandByName(string command_name) => commandList.First(c => c.name == command_name);
        public static bool CommandExists(string command_name) => commandList.Any(c => c.name == command_name);

        public static void InitializeCommands()
        {
            Assembly current_assembly = Assembly.GetExecutingAssembly();
            MemberInfo[] classes = current_assembly.GetTypes();

            List<command> command_list = new List<command>();

            foreach (MemberInfo class_ in classes)
            {
                MethodInfo[] methods = typeof(Commands).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                foreach (MethodInfo method_ in methods)
                {
                    if (!Attribute.IsDefined(method_, typeof(CommandMethod))) continue;
                    CommandMethod attribute = (CommandMethod)method_.GetCustomAttribute(typeof(CommandMethod));
                    try
                    {
                        Command function = (Command)method_.CreateDelegate(typeof(Command));
                        command c = attribute.command;
                        c.function = function;

                        command_list.Add(c);
                    }
                    catch
                    {
                        Console.WriteLine($"Cannot create command: '{method_.Name}'");
                    }

                }
            }

            commandList.AddRange(command_list);
        }


        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class CommandMethod : Attribute
        {
            public Commands.command command;

            public CommandMethod(string name)
            {
                command = new Commands.command(name, null);
            }
        }
    }

}
