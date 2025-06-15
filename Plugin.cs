using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterConsole
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        internal static ConfigEntry<bool> VerboseLoggingConfig;
        internal static bool VerboseLogging => VerboseLoggingConfig.Value;

        public void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PluginInfo.GUID} is loaded!");

            BindConfigs();

            Harmony harmony = new(PluginInfo.GUID);

            harmony.PatchAll();
        }

        public void BindConfigs()
        {
            VerboseLoggingConfig = Config.Bind(
                "General",
                "Verbose Logging",
                false,
                "makes logging verbose"
            );
        }

        public void Start()
        {
            CommandHandler.Initialize();
        }
    }
}
