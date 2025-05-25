using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BetterConsole
{
    [HarmonyPatch(typeof(QuickConsole))]
    [HarmonyPatch("Update")]
    public class ConsolePatch
    {
        public static bool Prefix(QuickConsole __instance, ref float ___time, ref InputField ___inputField)
        {
            if ((Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.BackQuote)) && QuickUI.lastActive != __instance && !Game.paused)
            {
                QuickUI.lastActive.Deactivate();
                __instance.Activate();
            }
            if (QuickUI.lastActive != __instance.menu && !Game.paused)
            {
                if (InputsManager.lTriggerHolded && InputsManager.rTriggerHolded)
                {
                    ___time = Mathf.MoveTowards(___time, 1f, TimeManager.unscaledDelta);
                    if (___time == 1f)
                    {
                        QuickUI.lastActive.Deactivate();
                        __instance.menu.Activate();
                    }
                }
                else
                {
                    ___time = 0f;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                __instance.Back();
            }

            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return false;
            }

            string[] args = ___inputField.text.ToLower().Split(' ');
            string commandName = args[0];
            string[] CommandArgs = [.. args.Skip(1)];

            CommandHandler.ExecuteCommand(commandName, CommandArgs);

            ___inputField.text = "";
            ___inputField.ActivateInputField();
            ___inputField.Select();

            return false;
        }
    }
}
