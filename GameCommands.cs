using System.Reflection;
using Steamworks;
using UnityEngine;

namespace BetterConsole
{
    public static class GameCommands
    {
#pragma warning disable IDE1006
        [Command]
        [CommandHelp("Plays a skull sequence by index")]
        public static void skull(int value)
        {
            if (value >= 0 && value < Game.instance.sequences.Length)
            {
                Game.instance.LoadLevel("Skull");
                TheSkull.overrideSequence = Game.instance.sequences[value];
                TheSkull.overrideSceneToLoad = Game.fallbackScene;
            }
        }

        [Command]
        [CommandHelp("Loads a level by name")]
        public static void load(string name)
        {
            Game.instance.LoadLevel(name);
        }

        [Command]
        [CommandHelp("Sets mouse sensitivity")]
        public static void mousesens(float sens)
        {
            int num = Mathf.RoundToInt(sens * 100f);
            if (sens > 0f)
            {
                if (!GamePrefs.cachedPrefs.ContainsKey("MouseSensitivityOverride"))
                {
                    GamePrefs.cachedPrefs.Add("MouseSensitivityOverride", num);
                }
                else
                {
                    Game.gamePrefs.UpdateValue("MouseSensitivityOverride", num);
                }
                Game.message.Show($"Mouse Sens. Overridden to {num / 100f}");
            }
        }

        [Command]
        [CommandHelp("Sets resolution")]
        public static void setres(int width, int height)
        {
            if (width > 460 && height > 320)
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
                MenuItem_Resolution.OnResolutionChange?.Invoke();
            }
        }

        [Command]
        [CommandHelp("Unlocks levels")]
        public static void unlock(string value)
        {
            Object.FindObjectOfType<QuickConsole>().
            Unlock(value, full: false);
        }

        [Command]
        [CommandHelp("Toggles debug mode")]
        public static void debug()
        {
            Game.debug = !Game.debug;
            Game.message.Show($"debug {Game.debug}");
        }

        [Command]
        [CommandHelp("Never used lmao")]
        public static void mod()
        {
            Game.mod = !Game.mod;
            Game.message.Show($"mod {Game.mod}");
        }

        [Command]
        [CommandHelp("Toggles no slowmo")]
        public static void noslowmo()
        {
            Game.noslowmo = !Game.noslowmo;
            Game.message.Show($"no slow-mo {Game.noslowmo}");
        }

        [Command]
        [CommandHelp("Backup current save data")]
        public static void backup()
        {
            FBPP.DeleteAll();
            Game.levels.BackupTest();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
        [CommandHelp("Clears this text field")]
        public static void clear()
        {
            Game.message.Hide();
        }

        [Command]
        [CommandHelp("Resets current slot data")]
        public static void reset()
        {
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
        [CommandHelp("Resets all game data")]
        public static void resetall()
        {
            PlayerPrefs.DeleteAll();
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
        [CommandHelp("Resets steam data")]
        public static void resetsteamstats()
        {
            if (SteamManager.Initialized)
            {
                _ = SteamUserStats.ResetAllStats(bAchievementsToo: true);
                _ = SteamUserStats.StoreStats();
                Game.message.Show("Steam Stats Cleared");
            }
        }

        [Command]
        [CommandHelp("Starts Level Rush")]
        public static void levelrush()
        {
            LevelRush.Start();
        }

        [Command]
        [CommandHelp("Toggles MixedInputs")]
        public static void mixedinputs()
        {
            Game.inputs.ToggleMixedInputs();
        }

        [Command]
        [CommandHelp("Shows command usage")]
        public static void help(string name)
        {
            if (CommandHandler.TryGetCommand(name, out CommandHandler.CommandEntry command))
            {
                string commandHelp = $"usage `{command?.name}";
                foreach (ParameterInfo param in command?.parameters)
                {
                    commandHelp += $" [{Utils.GetCleanTypeName(param.ParameterType)} {param.Name}]";
                }
                commandHelp += "`\n";

                if (command?.help != "")
                {
                    commandHelp += $"help: {command?.help}";
                }

                Game.message.Show(commandHelp);
                return;
            }

            Game.message.Show("Command not found");
        }
    }
}
