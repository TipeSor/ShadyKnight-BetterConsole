using System;
using System.Reflection;
using Steamworks;
using UnityEngine;

namespace BetterConsole
{
    public static class GameCommands
    {
#pragma warning disable IDE1006
        [Command]
        public static void skull(int value)
        {
            if (value < Game.instance.sequences.Length)
            {
                Game.instance.LoadLevel("Skull");
                TheSkull.overrideSequence = Game.instance.sequences[value];
                TheSkull.overrideSceneToLoad = Game.fallbackScene;
            }
        }

        [Command]
        public static void load(string name)
        {
            Game.instance.LoadLevel(name);
        }

        [Command]
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
        public static void setres(int width, int height)
        {
            if (width > 460 && height > 320)
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
                MenuItem_Resolution.OnResolutionChange?.Invoke();
            }
        }

        [Command]
        public static void unlock(string value)
        {
            UnityEngine.Object.FindObjectOfType<QuickConsole>().
            Unlock(value, full: false);
        }

        [Command]
        public static void debug()
        {
            Game.debug = !Game.debug;
            Game.message.Show($"debug {Game.debug}");
        }

        [Command]
        public static void mod()
        {
            Game.mod = !Game.mod;
            Game.message.Show($"mod {Game.mod}");
        }

        [Command]
        public static void noslowmo()
        {
            Game.noslowmo = !Game.noslowmo;
            Game.message.Show($"no slow-mo {Game.noslowmo}");
        }

        [Command]
        public static void backup()
        {
            FBPP.DeleteAll();
            Game.levels.BackupTest();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
        public static void clear()
        {
            Game.message.Hide();
        }

        [Command]
        public static void reset()
        {
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
        public static void resetall()
        {
            PlayerPrefs.DeleteAll();
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [Command]
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
        public static void levelrush()
        {
            LevelRush.Start();
        }

        [Command]
        public static void mixedinputs()
        {
            Game.inputs.ToggleMixedInputs();
        }

        [Command]
        public static void help(string name)
        {
            if (CommandHandler.HasCommand(name))
            {
                CommandHandler.CommandEntry command = CommandHandler.GetCommandByName(name);
                string commandHelp = $"usage `{command.name}";
                foreach (ParameterInfo param in command.parameters)
                {
                    commandHelp += $" {Utils.GetCleanTypeName(param.ParameterType)}";
                }
                commandHelp += "`";
                Game.message.Show(commandHelp);
                return;
            }

            Game.message.Show("Command not found");
        }
    }
}
