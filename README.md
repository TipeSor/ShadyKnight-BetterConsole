# Shady Knight - Better Console
## What is this? 
This is a Shady Knight mod that allows custom commands to be added with an attribute to run within the game console.
## How to download?  
The [latest release of the mod](https://github.com/TipeSor/ShadyKnight-BetterConsole/releases/latest) can be downloaded and placed in the `GameFolder/BepInEx/plugins` directory. 
## How to use custom commands? 
Custom commands given to the game console follow the format: `class.method value_1 value_2 ...`
You can check what the command requires with the `help command_name` command.

## How to create custom commands? 
To create custom commands: 
```cs
public class ExampleClass
{
    [BetterConsole.Command]
    public static void ExampleCommand1(int int_arg, float float_var, bool bool_var)
    {
        // code that runs
    }
}
```

