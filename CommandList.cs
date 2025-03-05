using UnityEngine;
using Steamworks;

namespace BetterConsole
{
    class CommandList
    {
        [BetterConsole.CustomCommand]
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

        [CustomCommand]
        static void load(string[] args)
        {
            Game.instance.LoadLevel(args[0]);
        }

        [CustomCommand]
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

        [CustomCommand]
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

        [CustomCommand]
        static void unlock(string[] args)
        {
            Object.FindObjectOfType<QuickConsole>().
            Unlock(args[0], full: false);
        }

        [CustomCommand]
        static void debug(string[] args)
        {
            Game.debug = !Game.debug;
            Game.message.Show($"debug {Game.debug}");
        }

        [CustomCommand]
        static void mod(string[] args)
        {
            Game.mod = !Game.mod;
            Game.message.Show($"mod {Game.mod}");
        }

        [CustomCommand]
        static void noslowmo(string[] args)
        {
            Game.noslowmo = !Game.noslowmo;
            Game.message.Show($"no slow-mo {Game.noslowmo}");
        }

        [CustomCommand]
        static void backup(string[] args)
        {
            FBPP.DeleteAll(); 
            Game.levels.BackupTest();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CustomCommand]
        static void clear(string[] args)
        {

        }

        [CustomCommand]
        static void reset(string[] args)
        {
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CustomCommand]
        static void resetall(string[] args)
        {
            PlayerPrefs.DeleteAll();
            FBPP.DeleteAll();
            GameLevels.instance.ClearAll();
            Game.instance.Restart();
            Game.fading.Set(1f);
        }

        [CustomCommand]
        static void resetsteamstats(string[] args)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.ResetAllStats(bAchievementsToo: true);
                SteamUserStats.StoreStats();
                Game.message.Show("Steam Stats Cleared");
            }
        }

        [CustomCommand]
        static void levelrush(string[] args)
        {
            LevelRush.Start();
        }

        [CustomCommand]
        static void mixedinputs(string[] args)
        {
            Game.inputs.ToggleMixedInputs();
        }
    }
}
