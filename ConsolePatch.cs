using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace BetterConsole
{
    [HarmonyPatch(typeof(QuickConsole))]
    [HarmonyPatch("Update")]
    public class ConsolePatch
    {
        public static bool Prefix(QuickConsole __instance)
        {
            if ((Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.BackQuote)) && QuickUI.lastActive != __instance && !Game.paused)
            {
                QuickUI.lastActive.Deactivate();
                __instance.Activate();
            }
            if (QuickUI.lastActive != __instance.menu && !Game.paused)
            {
                FieldInfo timeField = __instance.GetType().GetField("time", BindingFlags.Instance | BindingFlags.NonPublic);
                if (InputsManager.lTriggerHolded && InputsManager.rTriggerHolded)
                {
                    float time = (float)timeField.GetValue(__instance);
                    time = Mathf.MoveTowards(time, 1f, TimeManager.unscaledDelta);
                    if (time == 1f)
                    {
                        QuickUI.lastActive.Deactivate();
                        __instance.menu.Activate();
                    }
                    timeField.SetValue(__instance, time);
                }
                else
                {
                    timeField.SetValue(__instance, 0f);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                __instance.Back();
            }

            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return true;
            }

            FieldInfo inputFieldInfo = __instance.GetType().GetField("inputField", BindingFlags.Instance | BindingFlags.NonPublic);
            InputField inputField = (InputField)inputFieldInfo.GetValue(__instance);

            string[] args = inputField.text.ToLower().Split(' ');
            string commandName = args[0];
            string[] CommandArgs = [.. args.Skip(1)];

            CommandHandler.ExecuteCommand(commandName, CommandArgs);

            inputField.text = "";
            inputField.ActivateInputField();
            inputField.Select();

            __instance.GetType().GetField("inputField").SetValue(inputField, __instance);
            return true;
        }
    }
}
