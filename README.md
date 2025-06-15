# Shady Knight - Better Console
## User Manual
### What is this? 
This is a Shady Knight mod that allows custom commands to be added with an attribute to run within the game console.
### How to download?  
The [latest release of the mod](https://github.com/TipeSor/ShadyKnight-BetterConsole/releases/latest) can be downloaded and placed in the `GameFolder/BepInEx/plugins` directory. 
### How to use custom commands? 
Custom commands given to the game console follow the format: `Domain.Command value_1 value_2 ...` (case-sensitive) 
Note: commands with an empty domain are just `Command`  

You can check what the command requires with the `help Domain.Command` command.  
it outputs the usage in a format `Domain.Command [type name] [type name]`

## Developer Manual
### How to create custom commands?
To create custom commands: 
```cs
public class ExampleClass
{
    [BetterConsole.Command]
    [BetterConsole.CommandHelp("short help message")] // optional attribute
    public static void ExampleCommand1(int int_arg, float float_var, bool bool_var)
    {
        // code that runs
    }
}
```
Note: All custom commands must be **public static** methods.

### What types are supported?
Supported argument types: `short`, `int`, `long`, `float`, `double`, `byte`, `bool`, `string`

### How to create custom parsers?
To create custom parsers:
```cs
[TypeParser(typeof(ExampleType))]
public class ExampleTypeParser : TypeParser<ExampleType>
{
    public override bool TryParse(string input, out ExampleType value)
    {
        // parsing logic
    }
}
```
