using UnityEngine;
using Steamworks;

namespace BetterConsole
{
    class CommandList
    {
        [CommandHandler.CommandMethod()]
        static void skull(string[] args)
        {
            TheSkullSequence[] sequences = Game.instance.sequences;
            foreach (TheSkullSequence theSkullSequence in sequences)
            {
                if (int.TryParse(args[0], out int value))
                {
                    if (value < Game.instance.sequences.Length)
                    {
                        Game.instance.LoadLevel("Skull");
                        TheSkull.overrideSequence = Game.instance.sequences[value];
                        TheSkull.overrideSceneToLoad = Game.fallbackScene;
                    }
                }
                else if (theSkullSequence.name == args[0])
                {
                    Game.instance.LoadLevel("Skull");
                    TheSkull.overrideSequence = theSkullSequence;
                    TheSkull.overrideSceneToLoad = Game.fallbackScene;
                }
            }
        }

        [CommandHandler.CommandMethod()]
        static void load(string[] args)
        {
            Game.instance.LoadLevel(args[0]);
        }

        [CommandHandler.CommandMethod()]
        static void mousesens(string[] args)
        {
            float.TryParse(args[0], out var result3);
            int num = Mathf.RoundToInt(result3 * 100f);
            if (result3 > 0f)
            {
                if (!GamePrefs.cachedPrefs.ContainsKey("MouseSensitivityOverride"))
                {
                    GamePrefs.cachedPrefs.Add("MouseSensitivityOverride", num);
                }
                else
                {
                    Game.gamePrefs.UpdateValue("MouseSensitivityOverride", num);
                }
                Game.message.Show($"Mouse Sens. Overridden to {(float)num / 100f}");
            }
        }

        [CommandHandler.CommandMethod()]
        static void setres(string[] args)
        {
            string[] array2 = args[0].Split('x');
            int result = 0;
            int result2 = 0;
            int.TryParse(array2[0], out result);
            int.TryParse(array2[1], out result2);
            if (result > 460 && result2 > 320)
            {
                Screen.SetResolution(result, result2, Screen.fullScreen);
                if (MenuItem_Resolution.OnResolutionChange != null)
                {
                    MenuItem_Resolution.OnResolutionChange();
                }
            }
        }

        [CommandHandler.CommandMethod()]
        static void unlock(string[] args)
        {
            Object.FindObjectOfType<QuickConsole>().
            Unlock(args[0], full: false);
        }

        [CommandHandler.CommandMethod()]
        static void debug(string[] args)
        {
            Game.debug = !Game.debug;
            Game.message.Show($"debug {Game.debug}");
        }

        [CommandHandler.CommandMethod()]
        static void mod(string[] args)
        {
            Game.mod = !Game.mod;
            Game.message.Show($"mod {Game.mod}");
        }

        [CommandHandler.CommandMethod()]
        static void noslowmo(string[] args)
        {
            Game.noslowmo = !Game.noslowmo;
            Game.message.Show($"no slow-mo {Game.noslowmo}");
        }

        [CommandHandler.CommandMethod()]
        static void backup(string[] args)
        {
            FBPP.DeleteAll(); 
            Game.levels.BackupTest();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CommandHandler.CommandMethod()]
        static void clear(string[] args)
        {

        }

        [CommandHandler.CommandMethod()]
        static void reset(string[] args)
        {
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CommandHandler.CommandMethod()]
        static void resetall(string[] args)
        {
            PlayerPrefs.DeleteAll();
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CommandHandler.CommandMethod()]
        static void resetsteamstats(string[] args)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.ResetAllStats(bAchievementsToo: true);
                SteamUserStats.StoreStats();
                Game.message.Show("Steam Stats Cleared");
            }
        }

        [CommandHandler.CommandMethod()]
        static void levelrush(string[] args)
        {
            LevelRush.Start();
        }

        [CommandHandler.CommandMethod()]
        static void mixedinputs(string[] args)
        {
            Game.inputs.ToggleMixedInputs();
        }
    }
}
