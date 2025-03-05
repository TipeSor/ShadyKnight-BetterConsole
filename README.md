# Shady Knight - Better Console
## What is this? 
This is a Shady Knight mod that allows custom commands to be added with an attribute to run within the game console.
## How to download?  
The [latest release of the mod](https://github.com/TipeSor/ShadyKnight-BetterConsole/releases/latest) can be downloaded and placed in the `GameFolder/BepInEx/plugins` directory. 
## How to use custom commands? 
Custom commands given to the game console follow the format: `class.method arg1 arg2 ...`
## How to create custom commands? 
To create custom commands: 
```cs
class ExampleClass
{
    [BetterConsole.CustomCommand]
    static void ExampleCommand1(string[] args)
    {
        // code that runs
    }

	[BetterConsole.CustomCommand]
	static void ExampleCommand2(string[] args) 
	{
		// other code
	}

	[BetterConsole.CustomCommad]
	static void ExampleCommand3(string[] args)
	{
		// more code
	}
}

