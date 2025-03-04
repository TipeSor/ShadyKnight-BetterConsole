using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace BetterConsole
{
    [HarmonyPatch(typeof(QuickConsole))]
    [HarmonyPatch("Update")]
    class ConsolePatch
    {
        static bool Prefix(QuickConsole __instance)
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
                    float time = (float)__instance.GetType().GetField("time").GetValue(__instance);
                    time = Mathf.MoveTowards(time, 1f, TimeManager.unscaledDelta);
                    if (time == 1f)
                    {
                        QuickUI.lastActive.Deactivate();
                        __instance.menu.Activate();
                    }
                    __instance.GetType().GetField("time").SetValue(0f, __instance);
                }
                else
                {
                    __instance.GetType().GetField("time").SetValue(0f, __instance);
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

            InputField inputField = (InputField)__instance.GetType().GetField("inputField").GetValue(__instance);

            string[] array = inputField.text.Split(' ');
            inputField.text = "";
            inputField.ActivateInputField();
            inputField.Select();

            __instance.GetType().GetField("inputField").SetValue(inputField, __instance);

            return true;
        }
    }
}
