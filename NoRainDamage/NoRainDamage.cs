using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace NoRainDamage
{
    public class PluginInfo
    {
        public const string Name = "NoRainDamage";
        public const string Guid = "beardedkwan" + Name;
        public const string Version = "1.0.0";
    }

    public class NoRainDamageConfig
    {
        public static ConfigEntry<bool> NoRainDamage { get; set; }
        public static ConfigEntry<bool> NoUnderwaterDamage { get; set; }
    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class NoRainDamage : BaseUnityPlugin
    {
        void Awake()
        {
            // Initialize config
            NoRainDamageConfig.NoRainDamage = Config.Bind("General", "NoRainDamage", true, "Prevent rain damage on buildings.");
            NoRainDamageConfig.NoUnderwaterDamage = Config.Bind("General", "NoUnderwaterDamage", true, "Prevent underwater damage on buildings.");

            Harmony harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }

        // rain damage
        [HarmonyPatch(typeof(WearNTear), "HaveRoof")]
        public static class RainDamage_Patch
        {
            private static void Postfix(ref bool __result)
            {
                __result = NoRainDamageConfig.NoRainDamage.Value;
            }
        }

        // underwater damage
        [HarmonyPatch(typeof(WearNTear), "IsUnderWater")]
        public static class UnderwaterDamage_Patch
        {
            private static void Postfix(ref bool __result)
            {
                __result = !NoRainDamageConfig.NoUnderwaterDamage.Value;
            }
        }
    }
}
