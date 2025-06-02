using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterConsole
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        public void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PluginInfo.GUID} is loaded!");

            Harmony harmony = new(PluginInfo.GUID);

            harmony.PatchAll();
        }

        public void Start()
        {
            CommandHandler.Initialize();
        }
    }
}
