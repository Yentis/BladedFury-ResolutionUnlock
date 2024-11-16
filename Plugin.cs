using BepInEx;
using BepInEx.Logging;
using ChopGhost;
using HarmonyLib;
using System.Reflection;

namespace ResolutionUnlock
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.Patch(
                original: typeof(OptionUI).GetMethod("SetupResolutionUI", BindingFlags.NonPublic | BindingFlags.Instance),
                prefix: new HarmonyMethod(typeof(Patch).GetMethod(nameof(Patch.Prefix)))
            );

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
        }
    }

    class Patch
    {
        private static readonly FieldInfo MaxWidthField = typeof(OptionUI).GetField("MAX_WIDTH", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly FieldInfo MaxHeightField = typeof(OptionUI).GetField("MAX_HEIGHT", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Prefix()
        {
            MaxWidthField.SetValue(null, int.MaxValue);
            MaxHeightField.SetValue(null, int.MaxValue);
        }
    }
}
